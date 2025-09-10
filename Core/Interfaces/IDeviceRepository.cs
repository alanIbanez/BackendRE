using Core.Entities;

namespace Core.Interfaces;
public interface IDeviceRepository : IGenericRepository<Device>
{
    Task<Device> GetByDeviceIdAsync(string deviceId);
    Task<Device> GetByDeviceIdWithDetailsAsync(string deviceId);
}