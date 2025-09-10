using Application.Interfaces;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Application.Services;
public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public NotificationService(IUnitOfWork unitOfWork, IConfiguration configuration, HttpClient httpClient)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _httpClient = httpClient;

        // Configurar HttpClient para Expo
        _httpClient.BaseAddress = new Uri("https://exp.host/--/api/v2/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<bool> SendNotificationToTokenAsync(string token, string title, string message)
    {
        try
        {
            // Crear el mensaje push
            var pushMessage = new
            {
                to = token,
                title = title,
                body = message,
                sound = "default"
            };

            var messages = new[] { pushMessage };
            return await SendPushNotifications(messages);
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error sending notification: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendNotificationToAllAsync(string title, string message)
    {
        try
        {
            // Obtener todos los residentes activos
            var residents = await _unitOfWork.ResidentRepository.GetAllAsync();

            // Filtrar tokens válidos
            var tokens = residents.Where(r => r.Active && r.User.Active)
                                 .Select(r => r.User.NotificationToken)
                                 .Where(t => !string.IsNullOrEmpty(t))
                                 .ToList();

            if (!tokens.Any())
            {
                Console.WriteLine("No valid tokens found for residents");
                return false;
            }

            // Crear mensajes para todos los tokens
            var messages = tokens.Select(token => new
            {
                to = token,
                title = title,
                body = message,
                sound = "default"
            }).ToArray();

            return await SendPushNotifications(messages);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notifications to all: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> SendPushNotifications(object[] messages)
    {
        try
        {
            var jsonContent = JsonSerializer.Serialize(messages);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("push/send", content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Expo API error: {response.StatusCode}");
                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ExpoResponse>(responseContent);

            // Verificar si hubo errores
            if (responseData?.Data != null)
            {
                var errors = responseData.Data
                    .Where(item => item.Status == "error")
                    .ToList();

                if (errors.Any())
                {
                    Console.WriteLine($"Expo push errors: {JsonSerializer.Serialize(errors)}");
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendPushNotifications: {ex.Message}");
            return false;
        }
    }
}

// Clases para deserializar la respuesta de Expo
public class ExpoResponse
{
    public List<ExpoResponseData> Data { get; set; } = new List<ExpoResponseData>();
}

public class ExpoResponseData
{
    public string Status { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public object Details { get; set; } = new object();
}