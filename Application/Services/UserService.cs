using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;
public class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        return users.Select(p => new UserDto
        {
            Id = p.Id,
            UserName = p.Username,
            PasswordHash = p.PasswordHash,
            Active = p.Active
        });
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            UserName = user.Username,
            PasswordHash = user.PasswordHash,
            Active = user.Active
        };
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // 1. Hashear la contraseña
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_configuration["PasswordSalt"]));
        var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(createUserDto.PasswordHash)));

        //2 Crear token
        var user = new User
        {
            Id = 0,
            Username = createUserDto.UserName,
            PasswordHash = passwordHash,
            PersonId = createUserDto.PersonId,
            RoleId = createUserDto.RoleId,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        var tokenNavigation = _tokenService.CreateToken(user);

        //3 crear usuario
        var User = new User
        {
            Username = createUserDto.UserName,
            PasswordHash = passwordHash,
            PersonId = createUserDto.PersonId,
            RoleId = createUserDto.RoleId,
            NotificationToken = createUserDto.NotificationToken,
            NavigationToken = tokenNavigation,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.UserRepository.AddAsync(User);
        await _unitOfWork.Complete();

        return new UserDto
        {
            Id = User.Id,
            UserName = User.Username,
            PasswordHash = User.PasswordHash,
            Active = User.Active
        };
    }

    public async Task<bool> UpdateUserAsync(int id, CreateUserDto userDto)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null) return false;

        user.Username = userDto.UserName;
        user.PasswordHash = userDto.PasswordHash;
        user.PersonId = userDto.PersonId;
        user.RoleId = userDto.RoleId;
        user.NotificationToken = userDto.NotificationToken;
        user.NavigationToken = userDto.NavigationToken;
        user.Active = true;

        _unitOfWork.UserRepository.Update(user);
        return await _unitOfWork.Complete() > 0;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null) return false;

        user.Active = false;

        _unitOfWork.UserRepository.Update(user);
        return await _unitOfWork.Complete() > 0;
    }
}
