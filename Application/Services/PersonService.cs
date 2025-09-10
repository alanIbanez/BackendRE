using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;
public class PersonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public PersonService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<IEnumerable<PersonDto>> GetAllPersonsAsync()
    {
        var persons = await _unitOfWork.PersonRepository.GetAllAsync();
        return persons.Select(p => new PersonDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DNI = p.DNI,
            Email = p.Email,
            Phone = p.Phone,
            Address = p.Address,
            BirthDate = p.BirthDate,
            Active = p.Active
        });
    }

    public async Task<PersonDto> GetPersonByIdAsync(int id)
    {
        var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
        if (person == null) return null;

        return new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DNI = person.DNI,
            Email = person.Email,
            Phone = person.Phone,
            Address = person.Address,
            BirthDate = person.BirthDate,
            Active = person.Active
        };
    }

    public async Task<PersonDto> CreatePersonAsync(CreatePersonDto createPersonDto)
    {
        var person = new Person
        {
            FirstName = createPersonDto.FirstName,
            LastName = createPersonDto.LastName,
            DNI = createPersonDto.DNI,
            Email = createPersonDto.Email,
            Phone = createPersonDto.Phone,
            Address = createPersonDto.Address,
            BirthDate = createPersonDto.BirthDate,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.PersonRepository.AddAsync(person);
        await _unitOfWork.Complete();

        return new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DNI = person.DNI,
            Email = person.Email,
            Phone = person.Phone,
            Address = person.Address,
            BirthDate = person.BirthDate,
            Active = person.Active
        };
    }

    public async Task<bool> UpdatePersonAsync(int id, PersonDto personDto)
    {
        var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
        if (person == null) return false;

        person.FirstName = personDto.FirstName;
        person.LastName = personDto.LastName;
        person.DNI = personDto.DNI;
        person.Email = personDto.Email;
        person.Phone = personDto.Phone;
        person.Address = personDto.Address;
        person.BirthDate = personDto.BirthDate;
        person.Active = personDto.Active;
        person.ModifiedAt = DateTime.UtcNow;

        _unitOfWork.PersonRepository.Update(person);
        return await _unitOfWork.Complete() > 0;
    }

    public async Task<bool> DeletePersonAsync(int id)
    {
        var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
        if (person == null) return false;

        person.Active = false;
        person.ModifiedAt = DateTime.UtcNow;

        _unitOfWork.PersonRepository.Update(person);
        return await _unitOfWork.Complete() > 0;
    }

    //public async Task<bool> RegisterUserAsync(RegisterUserDto registerDto)
    //{//   ESTE METODO NO DEBERIA ESTAR AQUI, YA ESTA EN USERSERVICE
    //    // 1. Crear la persona
    //    var person = new Person
    //    {
    //        FirstName = registerDto.FirstName,
    //        LastName = registerDto.LastName,
    //        DNI = registerDto.DNI,
    //        Email = registerDto.Email,
    //        Phone = registerDto.Phone,
    //        Address = registerDto.Address,
    //        BirthDate = registerDto.BirthDate,
    //        Active = true,
    //        CreatedAt = DateTime.UtcNow
    //    };

    //    await _unitOfWork.PersonRepository.AddAsync(person);
    //    await _unitOfWork.Complete();

    //    // 2. Hashear la contraseña
    //    using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_configuration["PasswordSalt"]));
    //    var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)));

    //    // 3. Crear el usuario
    //    var user = new User
    //    {
    //        PersonId = person.Id,
    //        RoleId = registerDto.RoleId,
    //        Username = registerDto.Username,
    //        PasswordHash = passwordHash,
    //        Active = true,
    //        CreatedAt = DateTime.UtcNow
    //    };

    //    await _unitOfWork.UserRepository.AddAsync(user);
    //    await _unitOfWork.Complete();

    //    // 4. Crear entidad específica según el rol
    //    var role = await _unitOfWork.RoleRepository.GetByIdAsync(registerDto.RoleId);

    //    switch (role?.Name.ToLower())
    //    {
    //        case "preceptor":
    //            if (registerDto.PreceptorTypeId.HasValue && registerDto.ShiftId.HasValue)
    //            {
    //                var preceptor = new Preceptor
    //                {
    //                    UserId = user.Id,
    //                    PreceptorTypeId = registerDto.PreceptorTypeId.Value,
    //                    ShiftId = registerDto.ShiftId.Value,
    //                    Active = true,
    //                    CreatedAt = DateTime.UtcNow
    //                };
    //                await _unitOfWork.PreceptorRepository.AddAsync(preceptor);
    //            }
    //            break;

    //        case "tutor":
    //            var tutor = new Tutor
    //            {
    //                UserId = user.Id,
    //                Active = true,
    //                CreatedAt = DateTime.UtcNow
    //            };
    //            await _unitOfWork.TutorRepository.AddAsync(tutor);
    //            break;

    //        case "guardia":
    //            if (registerDto.ShiftId.HasValue)
    //            {
    //                var guard = new Guard
    //                {
    //                    UserId = user.Id,
    //                    ShiftId = registerDto.ShiftId.Value,
    //                    Active = true,
    //                    CreatedAt = DateTime.UtcNow
    //                };
    //                await _unitOfWork.GuardRepository.AddAsync(guard);
    //            }
    //            break;

    //        case "residente":
    //            if (registerDto.ResidentTypeId.HasValue && registerDto.TutorId.HasValue)
    //            {
    //                var resident = new Resident
    //                {
    //                    UserId = user.Id,
    //                    ResidentTypeId = registerDto.ResidentTypeId.Value,
    //                    TutorId = registerDto.TutorId.Value,
    //                    Active = true,
    //                    CreatedAt = DateTime.UtcNow
    //                };
    //                await _unitOfWork.ResidentRepository.AddAsync(resident);
    //            }
    //            break;
    //    }

    //    await _unitOfWork.Complete();
    //    return true;
    //}
}