using Clini_Management_System.Server.Models.Entities;

namespace Clini_Management_System.Server.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<bool> ClinicExistsAsync(string clinicName, CancellationToken ct = default);
    Task<Clinic> CreateClinicAsync(Clinic clinic, CancellationToken ct = default);
    Task<User> CreateUserAsync(User user, CancellationToken ct = default);
}
