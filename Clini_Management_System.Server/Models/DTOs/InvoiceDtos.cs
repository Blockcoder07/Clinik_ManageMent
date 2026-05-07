namespace Clini_Management_System.Server.Models.DTOs;

public class InvoiceCreateDto
{
    public int AppointmentId { get; set; }
    public decimal Amount { get; set; }
}

public class InvoiceDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
