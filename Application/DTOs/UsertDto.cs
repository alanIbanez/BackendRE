namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    public class CreateUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int PersonId { get; set; }
        public int RoleId { get; set; }
        public string NavigationToken { get; set; } = string.Empty;
        public string? NotificationToken { get; set; }
    }
}
