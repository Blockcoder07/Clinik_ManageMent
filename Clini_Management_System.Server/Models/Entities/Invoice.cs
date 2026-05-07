namespace Clini_Management_System.Server.Models.Entities;

public class Invoice : TenantEntity
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Appointment? Appointment { get; set; }
}
