using Clini_Management_System.Server.Data;
using Clini_Management_System.Server.Models.Entities;
using Clini_Management_System.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        _db.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Username == username, ct);

    public Task<bool> ClinicExistsAsync(string clinicName, CancellationToken ct = default) =>
        _db.Clinics.AnyAsync(x => x.Name == clinicName, ct);

    public async Task<Clinic> CreateClinicAsync(Clinic clinic, CancellationToken ct = default)
    {
        _db.Clinics.Add(clinic);
        await _db.SaveChangesAsync(ct);
        return clinic;
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return user;
    }
}
