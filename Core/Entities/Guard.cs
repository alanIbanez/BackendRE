namespace Core.Entities;
public class Guard
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}