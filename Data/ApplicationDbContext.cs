using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

using DoctorAppointmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<AppointmentSlot> AppointmentSlots => Set<AppointmentSlot>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<AppointmentSlot>()
         .HasOne(s => s.Doctor)
         .WithMany(d => d.AppointmentSlots)
         .HasForeignKey(s => s.DoctorId)
         .OnDelete(DeleteBehavior.Cascade);

        b.Entity<AppointmentSlot>()
         .HasOne(s => s.Patient)
         .WithMany(u => u.Appointments)
         .HasForeignKey(s => s.PatientId)
         .OnDelete(DeleteBehavior.SetNull);
    }
}