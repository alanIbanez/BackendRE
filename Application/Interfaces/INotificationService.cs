namespace Application.Interfaces;
public interface INotificationService
{
    Task<bool> SendNotificationToTokenAsync(string token, string title, string message);
    Task<bool> SendNotificationToAllAsync(string title, string message);
}