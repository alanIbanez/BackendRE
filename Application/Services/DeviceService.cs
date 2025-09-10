using Application.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;
public class DeviceService
{
    private readonly IUnitOfWork _unitOfWork;

    public DeviceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
    {
        var devices = await _unitOfWork.DeviceRepository.GetAllAsync();
        return devices.Select(d => new DeviceDto
        {
            Id = d.Id,
            DeviceId = d.DeviceId,
            TokenFcm = d.TokenFcm,
            Active = d.Active,
            CreatedAt = d.CreatedAt
        });
    }

    public async Task<DeviceDto> GetDeviceByIdAsync(int id)
    {
        var device = await _unitOfWork.DeviceRepository.GetByIdAsync(id);
        if (device == null) return null;

        return new DeviceDto
        {
            Id = device.Id,
            DeviceId = device.DeviceId,
            TokenFcm = device.TokenFcm,
            Active = device.Active,
            CreatedAt = device.CreatedAt
        };
    }

    public async Task<DeviceDto> GetDeviceByDeviceIdAsync(string deviceId)
    {
        try
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentException("El deviceId no puede estar vacío");
            }

            var device = await _unitOfWork.DeviceRepository.GetByDeviceIdAsync(deviceId);

            if (device == null) return null;

            return new DeviceDto
            {
                Id = device.Id,
                DeviceId = device.DeviceId,
                TokenFcm = device.TokenFcm,
                Active = device.Active,
                CreatedAt = device.CreatedAt
            };
        }
        catch (Exception ex)
        {
            // Log the error (deberías usar un logger real)
            Console.WriteLine($"Error obteniendo dispositivo por DeviceId: {ex.Message}");
            throw;
        }
    }

    public async Task<DeviceDto> CreateDeviceAsync(CreateDeviceDto createDeviceDto)
    {
        // Verificar si el deviceId ya existe
        var existingDevices = _unitOfWork.DeviceRepository.Find(d => d.DeviceId == createDeviceDto.DeviceId);
        if (existingDevices.Any())
        {
            throw new InvalidOperationException($"El DeviceId '{createDeviceDto.DeviceId}' ya existe");
        }

        var device = new Device
        {
            DeviceId = createDeviceDto.DeviceId,
            TokenFcm = createDeviceDto.TokenFcm,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.DeviceRepository.AddAsync(device);
        await _unitOfWork.Complete();

        return new DeviceDto
        {
            Id = device.Id,
            DeviceId = device.DeviceId,
            TokenFcm = device.TokenFcm,
            Active = device.Active,
            CreatedAt = device.CreatedAt
        };
    }

    public async Task<bool> DeleteDeviceAsync(int id)
    {
        var device = await _unitOfWork.DeviceRepository.GetByIdAsync(id);
        if (device == null) return false;

        device.Active = false;

        _unitOfWork.DeviceRepository.Update(device);
        return await _unitOfWork.Complete() > 0;
    }

    public async Task<bool> HardDeleteDeviceAsync(int id)
    {
        var device = await _unitOfWork.DeviceRepository.GetByIdAsync(id);
        if (device == null) return false;

        _unitOfWork.DeviceRepository.Remove(device);
        return await _unitOfWork.Complete() > 0;
    }
}