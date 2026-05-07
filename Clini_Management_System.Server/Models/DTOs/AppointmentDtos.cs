using Clini_Management_System.Server.Models.Enums;

namespace Clini_Management_System.Server.Models.DTOs;

public class AppointmentCreateDto
{
    public int PatientId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
}

public class AppointmentStatusUpdateDto
{
    public AppointmentStatus Status { get; set; }
    public string RowVersion { get; set; } = string.Empty;
}

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public string RowVersion { get; set; } = string.Empty;
}
