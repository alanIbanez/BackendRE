using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
{
    private readonly AppDbContext _context;

    public DeviceRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Device> GetByDeviceIdAsync(string deviceId)
    {
        return await _context.Devices
            .FirstOrDefaultAsync(d => d.DeviceId == deviceId && d.Active);
    }

    public async Task<Device> GetByDeviceIdWithDetailsAsync(string deviceId)
    {
        return await _context.Devices
            .Where(d => d.DeviceId == deviceId && d.Active)
            .FirstOrDefaultAsync();
    }

    public async Task<Device> GetByDeviceIdRawSqlAsync(string deviceId)
    {
        return await _context.Devices
            .FromSqlInterpolated($@"
            SELECT * 
            FROM Devices 
            WHERE DeviceId = {deviceId} 
            AND Active = true")
            .FirstOrDefaultAsync();
    }
}