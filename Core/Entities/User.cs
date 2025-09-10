namespace Core.Entities;
public class User
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? NotificationToken { get; set; }
    public string NavigationToken { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
}