using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("to-one")]
    public async Task<ActionResult> NotificationToOne(NotificationDto notificationDto)
    {
        var result = await _notificationService.SendNotificationToTokenAsync(
            notificationDto.Token,
            notificationDto.Title,
            notificationDto.Message);

        if (!result) return BadRequest("Error al enviar la notificación");
        return Ok();
    }

    [HttpPost("to-all")]
    [Authorize(Roles = "Preceptor")]
    public async Task<ActionResult> NotificationToAll(NotificationDto notificationDto)
    {
        var result = await _notificationService.SendNotificationToAllAsync(
            notificationDto.Title,
            notificationDto.Message);

        if (!result) return BadRequest("Error al enviar las notificaciones");
        return Ok();
    }
}