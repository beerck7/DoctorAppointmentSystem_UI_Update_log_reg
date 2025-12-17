using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentSystem.Models;

public class Doctor
{
    public int Id { get; set; }
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string Specialization { get; set; } = string.Empty;
    public string? Description { get; set; }
    public virtual ICollection<AppointmentSlot> AppointmentSlots { get; set; } = new List<AppointmentSlot>();
    public string FullName => $"{FirstName} {LastName}";
}