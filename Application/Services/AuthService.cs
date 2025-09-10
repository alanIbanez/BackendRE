using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;
public class AuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        // Usando el nuevo método async
        var users = await _unitOfWork.UserRepository.FindAsync(u => u.Username == loginDto.Username && u.Active);
        var user = users.FirstOrDefault();

        if (user == null)
            throw new UnauthorizedAccessException("Usuario no existe");

        // Verificar la contraseña
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_configuration["PasswordSalt"]));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        // Comparar los hashes
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != Convert.FromBase64String(user.PasswordHash)[i])
                throw new UnauthorizedAccessException("Contraseña incorrecta");
        }

        // Actualizar último login
        user.LastLogin = DateTime.UtcNow;
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.Complete();

        return new LoginResponseDto
        {
            Token = _tokenService.CreateToken(user),
            Expiration = DateTime.Now.AddDays(7),
            UserId = user.Id,
            Role = user.Role.Name
        };
    }

    public async Task<bool> UpdateNotificationToken(int userId, string token)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null) return false;

        user.NotificationToken = token;
        _unitOfWork.UserRepository.Update(user);
        return await _unitOfWork.Complete() > 0;
    }
}