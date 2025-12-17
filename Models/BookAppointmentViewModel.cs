using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentSystem.Models;

public class BookAppointmentViewModel
{
    public int Id { get; set; }
    public int DoctorId { get; set; }

    [Required] public string PatientFirstName { get; set; } = string.Empty;
    [Required] public string PatientLastName { get; set; } = string.Empty;
    [Required, EmailAddress] public string PatientEmail { get; set; } = string.Empty;
    [Required] public string PatientPhone { get; set; } = string.Empty;
    public string? PatientAddress { get; set; }
}