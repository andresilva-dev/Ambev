using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(20);
            builder.Property(s => s.Branch).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Date).IsRequired();
            builder.Ignore(s => s.Total);
            builder.Property(s => s.CustomerName).IsRequired();
            builder.Property(s => s.Cancelled).IsRequired();
            builder.Ignore(s => s.ItemsNotCancelled);
            builder.Ignore(s => s.TotalWithoutDiscounts);
            builder.Ignore(s => s.TotalDiscountsPercentage);

            builder.HasMany(s => s.Items)
                   .WithOne(i => i.Sale)
                   .HasForeignKey(i => i.SaleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
