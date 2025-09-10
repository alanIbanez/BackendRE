namespace Application.DTOs;
public class DeviceDto
{
    public int Id { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string? TokenFcm { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public class CreateDeviceDto
{
    public string DeviceId { get; set; } = string.Empty;
    public string? TokenFcm { get; set; }
}

public class UpdateDeviceDto
{
    public string? TokenFcm { get; set; }
    public bool Active { get; set; }
}