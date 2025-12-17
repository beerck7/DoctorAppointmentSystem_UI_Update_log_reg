using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentSystem.Models;

public class AppointmentSlot
{
    public int Id { get; set; }
    [Required] public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    [Required] public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public bool IsAvailable { get; set; } = true;

    public int? PatientId { get; set; }
    public User? Patient { get; set; }

    public string? PatientFirstName { get; set; }
    public string? PatientLastName { get; set; }
    public string? PatientEmail { get; set; }
    public string? PatientPhone { get; set; }
    public string? PatientAddress { get; set; }
    public DateTime? ReservationDate { get; set; }

    public string PatientFullName => $"{PatientFirstName} {PatientLastName}";
    public string FormattedDate => StartTime.ToString("dd.MM.yyyy HH:mm");
}