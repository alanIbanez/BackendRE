namespace Application.DTOs;
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; } = string.Empty;
}