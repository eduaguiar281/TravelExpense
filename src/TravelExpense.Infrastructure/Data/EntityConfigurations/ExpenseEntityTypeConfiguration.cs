using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelExpense.Domain;
using TravelExpense.Domain.Enums;

namespace TravelExpense.Infrastructure.Data.EntityConfigurations
{
    public class ExpenseEntityTypeConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable($"{nameof(Expense)}s");
            builder.HasKey(t => t.Id);
            builder.Property(e => e.RelatedTo).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(250).IsRequired();
            builder.Property(e => e.VoucherId).HasMaxLength(50);
            builder.Property(e => e.Status).HasMaxLength(40).HasConversion(
                      v => v.ToString(),
                      v => (ExpenseStatus)Enum.Parse(typeof(ExpenseStatus), v));
        }
    }
}
