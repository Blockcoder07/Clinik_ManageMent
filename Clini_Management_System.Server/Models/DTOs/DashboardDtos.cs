namespace Clini_Management_System.Server.Models.DTOs;

public class RevenueSummaryDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public int CancelledAppointments { get; set; }
}
