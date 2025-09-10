namespace Core.Entities;
public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime EventDate { get; set; }
    public int PreceptorId { get; set; }
    public Preceptor Preceptor { get; set; } = null!;
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}