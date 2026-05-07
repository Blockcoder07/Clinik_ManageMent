using System.ComponentModel.DataAnnotations;

namespace Clini_Management_System.Server.Models.Entities;

public class Clinic
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
