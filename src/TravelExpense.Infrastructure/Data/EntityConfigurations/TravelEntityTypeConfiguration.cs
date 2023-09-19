using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelExpense.Domain;
using TravelExpense.Domain.Enums;

namespace TravelExpense.Infrastructure.Data.EntityConfigurations
{
    public class TravelEntityTypeConfiguration : IEntityTypeConfiguration<Travel>
    {
        public void Configure(EntityTypeBuilder<Travel> builder)
        {
            builder.ToTable($"{nameof(Travel)}s");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Description)
                .HasMaxLength(200)
                .IsRequired();

            builder.OwnsOne(t => t.Employee, employeeBuilder =>
            {
                employeeBuilder.Property(e => e.Registration)
                .HasMaxLength(50)
                .HasColumnName("EmployeeRegistration");

                employeeBuilder.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("EmployeeName");
            });
            builder.Navigation(t => t.Employee).IsRequired();

            builder.Property(t => t.Status).HasMaxLength(40).HasConversion(
                      v => v.ToString(),
                      v => (TravelStatus)Enum.Parse(typeof(TravelStatus), v));

            builder.HasMany(s => s.Expenses!)
                .WithOne()
                .HasForeignKey("TravelId")
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade)
                .Metadata
                .PrincipalToDependent.SetField("_expenses");
        }
    }
}
