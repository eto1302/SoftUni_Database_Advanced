using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PatientMedicamentConfig : IEntityTypeConfiguration<PatientMedicament>
    {
        public void Configure(EntityTypeBuilder<PatientMedicament> builder)
        {
            builder.HasKey(x => new { x.PatientId, x.MedicamentId });

            builder.HasOne(x => x.Patient)
              .WithMany(p => p.Prescriptions)
              .HasForeignKey(x => x.PatientId);
        }
    }
}