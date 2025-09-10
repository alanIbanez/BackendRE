using Core.Entities;

namespace Core.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Person> PersonRepository { get; }
    IGenericRepository<PreceptorType> PreceptorTypeRepository { get; }
    IGenericRepository<ResidentType> ResidentTypeRepository { get; }
    IGenericRepository<Role> RoleRepository { get; }
    IGenericRepository<Shift> ShiftRepository { get; }
    IGenericRepository<User> UserRepository { get; }
    IGenericRepository<Preceptor> PreceptorRepository { get; }
    IGenericRepository<Tutor> TutorRepository { get; }
    IGenericRepository<Guard> GuardRepository { get; }
    IGenericRepository<Resident> ResidentRepository { get; }
    IGenericRepository<Event> EventRepository { get; }
    //IGenericRepository<Device> DeviceRepositoryGeneric { get; }
    IDeviceRepository DeviceRepository { get; }
    Task<int> Complete();
}