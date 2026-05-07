using Clini_Management_System.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clini_Management_System.Server.Data.Configurations;

public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");
        builder.HasIndex(x => new { x.ClinicId, x.Name });
        builder.HasIndex(x => new { x.ClinicId, x.MobileNumber });
    }
}
