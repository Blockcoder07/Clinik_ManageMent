namespace Clini_Management_System.Server.Models.DTOs;

public class PatientCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
}

public class PatientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
}
