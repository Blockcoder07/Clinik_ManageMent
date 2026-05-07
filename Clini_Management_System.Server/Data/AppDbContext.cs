using Clini_Management_System.Server.Common;
using Clini_Management_System.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clini_Management_System.Server.Data;

public class AppDbContext : DbContext
{
    private readonly ITenantContext _tenant;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantContext tenant) : base(options)
    {
        _tenant = tenant;
    }

    public DbSet<Clinic> Clinics => Set<Clinic>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clinic>(e =>
        {
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(x => new { x.ClinicId, x.Username }).IsUnique();
            e.HasQueryFilter(x => x.ClinicId == _tenant.ClinicId);
        });

        modelBuilder.Entity<Patient>(e =>
        {
            e.HasIndex(x => new { x.ClinicId, x.Name });
            e.HasIndex(x => new { x.ClinicId, x.MobileNumber });
            e.HasQueryFilter(x => x.ClinicId == _tenant.ClinicId);
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => new { x.ClinicId, x.AppointmentDate });
            e.HasIndex(x => new { x.ClinicId, x.Status });
            e.HasOne(x => x.Patient)
             .WithMany()
             .HasForeignKey(x => x.PatientId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasQueryFilter(x => x.ClinicId == _tenant.ClinicId);
        });

        modelBuilder.Entity<Invoice>(e =>
        {
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.HasIndex(x => x.AppointmentId).IsUnique();
            e.HasIndex(x => new { x.ClinicId, x.CreatedAt });
            e.HasOne(x => x.Appointment)
             .WithOne(x => x.Invoice)
             .HasForeignKey<Invoice>(x => x.AppointmentId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasQueryFilter(x => x.ClinicId == _tenant.ClinicId);
        });

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<TenantEntity>())
        {
            if (entry.State == EntityState.Added && entry.Entity.ClinicId == 0)
                entry.Entity.ClinicId = _tenant.ClinicId;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
