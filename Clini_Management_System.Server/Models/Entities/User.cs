using System.ComponentModel.DataAnnotations;

namespace Clini_Management_System.Server.Models.Entities;

public class User : TenantEntity
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string Role { get; set; } = "Staff";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
