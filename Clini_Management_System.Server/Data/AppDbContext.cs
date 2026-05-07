using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Data;

public class AppDbContext : DbContext
{
    #region Fields

    private readonly ITenantContext _tenant;

    #endregion

    #region Constructor

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantContext tenant)
        : base(options)
    {
        _tenant = tenant;
    }

    #endregion

    #region DbSets

    public DbSet<Clinic> Clinics => Set<Clinic>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    #endregion

    #region Overrides

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<User>().HasQueryFilter(x => x.ClinicId == _tenant.ClinicId);
        modelBuilder.Entity<Patient>().HasQueryFilter(x => x.ClinicId == _tenant.ClinicId);
        modelBuilder.Entity<Appointment>().HasQueryFilter(x => x.ClinicId == _tenant.ClinicId);
        modelBuilder.Entity<Invoice>().HasQueryFilter(x => x.ClinicId == _tenant.ClinicId);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampTenant();
        return base.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Private Methods

    private void StampTenant()
    {
        foreach (var entry in ChangeTracker.Entries<TenantEntity>())
        {
            if (entry.State == EntityState.Added && entry.Entity.ClinicId == 0)
                entry.Entity.ClinicId = _tenant.ClinicId;
        }
    }

    #endregion
}
