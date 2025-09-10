namespace Core.Entities;
public class Device
{
    public int Id { get; set; }
    public string DeviceId { get; set; } = string.Empty; // Cambiado a string para mayor flexibilidad
    public string? TokenFcm { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}