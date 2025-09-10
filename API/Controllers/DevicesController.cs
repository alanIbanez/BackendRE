using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly DeviceService _deviceService;

    public DevicesController(DeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDto>> GetDevice(int id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);
        if (device == null) return NotFound();
        return Ok(device);
    }

    [HttpGet("by-deviceid/{deviceId}")]
    public async Task<ActionResult<DeviceDto>> GetDeviceByDeviceId(string deviceId)
    {
        var device = await _deviceService.GetDeviceByDeviceIdAsync(deviceId);
        if (device == null) return NotFound();
        return Ok(device);
    }

    [HttpPost]
    public async Task<ActionResult<DeviceDto>> CreateDevice(CreateDeviceDto createDeviceDto)
    {
        try
        {
            var device = await _deviceService.CreateDeviceAsync(createDeviceDto);
            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDevice(int id)
    {
        var result = await _deviceService.DeleteDeviceAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("hard-delete/{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> HardDeleteDevice(int id)
    {
        var result = await _deviceService.HardDeleteDeviceAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}