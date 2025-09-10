namespace Core.Entities;
public class Resident
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ResidentTypeId { get; set; }
    public ResidentType ResidentType { get; set; } = null!;
    public int TutorId { get; set; }
    public Tutor Tutor { get; set; } = null!;
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}