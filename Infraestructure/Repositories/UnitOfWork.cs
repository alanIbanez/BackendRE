using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private GenericRepository<Person> _personRepository;
    private GenericRepository<PreceptorType> _preceptorTypeRepository;
    private GenericRepository<ResidentType> _residentTypeRepository;
    private GenericRepository<Role> _roleRepository;
    private GenericRepository<Shift> _shiftRepository;
    private GenericRepository<User> _userRepository;
    private GenericRepository<Preceptor> _preceptorRepository;
    private GenericRepository<Tutor> _tutorRepository;
    private GenericRepository<Guard> _guardRepository;
    private GenericRepository<Resident> _residentRepository;
    private GenericRepository<Event> _eventRepository;
    //private GenericRepository<Device> _deviceRepositoryGeneric;
    private DeviceRepository _deviceRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Person> PersonRepository =>
        _personRepository ??= new GenericRepository<Person>(_context);

    public IGenericRepository<PreceptorType> PreceptorTypeRepository =>
        _preceptorTypeRepository ??= new GenericRepository<PreceptorType>(_context);

    public IGenericRepository<ResidentType> ResidentTypeRepository =>
        _residentTypeRepository ??= new GenericRepository<ResidentType>(_context);

    public IGenericRepository<Role> RoleRepository =>
        _roleRepository ??= new GenericRepository<Role>(_context);

    public IGenericRepository<Shift> ShiftRepository =>
        _shiftRepository ??= new GenericRepository<Shift>(_context);

    public IGenericRepository<User> UserRepository =>
        _userRepository ??= new GenericRepository<User>(_context);

    public IGenericRepository<Preceptor> PreceptorRepository =>
        _preceptorRepository ??= new GenericRepository<Preceptor>(_context);

    public IGenericRepository<Tutor> TutorRepository =>
        _tutorRepository ??= new GenericRepository<Tutor>(_context);

    public IGenericRepository<Guard> GuardRepository =>
        _guardRepository ??= new GenericRepository<Guard>(_context);

    public IGenericRepository<Resident> ResidentRepository =>
        _residentRepository ??= new GenericRepository<Resident>(_context);

    public IGenericRepository<Event> EventRepository =>
        _eventRepository ??= new GenericRepository<Event>(_context);

    //public IGenericRepository<Device> DeviceRepositoryGeneric =>
    //   _deviceRepositoryGeneric ??= new GenericRepository<Device>(_context);

    public IDeviceRepository DeviceRepository =>
        _deviceRepository ??= new DeviceRepository(_context);

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}