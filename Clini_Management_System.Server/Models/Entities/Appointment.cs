using System.ComponentModel.DataAnnotations;
using Clini_Management_System.Server.Models.Enums;

namespace Clini_Management_System.Server.Models.Entities;

public class Appointment : TenantEntity
{
    public int Id { get; set; }
    public int PatientId { get; set; }

    [Required, MaxLength(150)]
    public string DoctorName { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Patient? Patient { get; set; }
    public Invoice? Invoice { get; set; }
}
