using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SalesItems");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(i => i.ProductId).IsRequired();
            builder.Property(i => i.ProductName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Quantity).IsRequired();
            builder.Property(i => i.UnitPrice).HasPrecision(10, 2).IsRequired();
            builder.Property(i => i.DiscountPercentage).HasPrecision(5, 2).IsRequired();
            builder.Ignore(i => i.Total);
        }
    }
}
