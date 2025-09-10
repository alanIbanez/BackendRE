using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Infrastructure.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<PreceptorType> PreceptorTypes { get; set; }
    public DbSet<ResidentType> ResidentTypes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Preceptor> Preceptors { get; set; }
    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Guard> Guards { get; set; }
    public DbSet<Resident> Residents { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Device>().ToTable("Device");

        // Configuraciones adicionales de relaciones si es necesario
        modelBuilder.Entity<User>()
            .HasOne(u => u.Person)
            .WithMany()
            .HasForeignKey(u => u.PersonId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Preceptor>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Preceptor>()
            .HasOne(p => p.PreceptorType)
            .WithMany()
            .HasForeignKey(p => p.PreceptorTypeId);

        modelBuilder.Entity<Preceptor>()
            .HasOne(p => p.Shift)
            .WithMany()
            .HasForeignKey(p => p.ShiftId);

        modelBuilder.Entity<Tutor>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Guard>()
            .HasOne(g => g.User)
            .WithMany()
            .HasForeignKey(g => g.UserId);

        modelBuilder.Entity<Guard>()
            .HasOne(g => g.Shift)
            .WithMany()
            .HasForeignKey(g => g.ShiftId);

        modelBuilder.Entity<Resident>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Resident>()
            .HasOne(r => r.ResidentType)
            .WithMany()
            .HasForeignKey(r => r.ResidentTypeId);

        modelBuilder.Entity<Resident>()
            .HasOne(r => r.Tutor)
            .WithMany()
            .HasForeignKey(r => r.TutorId);

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Preceptor)
            .WithMany()
            .HasForeignKey(e => e.PreceptorId);

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasIndex(d => d.DeviceId).IsUnique();
            entity.Property(d => d.DeviceId).IsRequired().HasMaxLength(255);
            entity.Property(d => d.TokenFcm).HasColumnType("text");
        });
    }
}