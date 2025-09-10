using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var persons = await _userService.GetAllUsersAsync();
        return Ok(persons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        try
        {
            var result = await _userService.CreateUserAsync(createUserDto);
            if (result.Id == 0) return BadRequest("Error al registrar usuario");
            return Ok(new { message = "Usuario registrado exitosamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        //var user = await _userService.CreateUserAsync(createUserDto);
        //return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, CreateUserDto userDto)
    {
        if (id != userDto.Id) return BadRequest();
        var result = await _userService.UpdateUserAsync(id, userDto);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    //[HttpPost("send-notification")]
    //public async Task<IActionResult> SendNotification([FromBody] NotificationDto notificationDto)
    //{
    //    await _userService.SendNotificationAsync(notificationDto);
    //    return Ok(new { message = "Notification sent successfully" });
    //}
}
