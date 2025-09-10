using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly PersonService _personService;

    public AuthController(AuthService authService, PersonService personService)
    {
        _authService = authService;
        _personService = personService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var result = await _authService.Login(loginDto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    //[HttpPost("register")]
    //public async Task<ActionResult> Register(RegisterUserDto registerDto)
    //{
    //    try
    //    {
    //        var result = await _personService.RegisterUserAsync(registerDto);
    //        if (!result) return BadRequest("Error al registrar usuario");
    //        return Ok(new { message = "Usuario registrado exitosamente" });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    [HttpPost("update-notification-token/{userId}")]
    public async Task<ActionResult> UpdateNotificationToken(int userId, [FromBody] string token)
    {
        var result = await _authService.UpdateNotificationToken(userId, token);
        if (!result) return BadRequest("Error al actualizar el token");
        return Ok();
    }
}