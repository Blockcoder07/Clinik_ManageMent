using Clini_Management_System.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clini_Management_System.Server.Data.Configurations;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.Property(x => x.Amount).HasPrecision(18, 2);

        builder.HasIndex(x => x.AppointmentId).IsUnique();
        builder.HasIndex(x => new { x.ClinicId, x.CreatedAt });

        builder
            .HasOne(x => x.Appointment)
            .WithOne(x => x.Invoice)
            .HasForeignKey<Invoice>(x => x.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
