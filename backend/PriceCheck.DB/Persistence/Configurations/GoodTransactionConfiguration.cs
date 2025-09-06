using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Persistence.Configurations
{
    public class GoodTransactionConfiguration : IEntityTypeConfiguration<GoodTransaction>
    {
        public void Configure(EntityTypeBuilder<GoodTransaction> builder)
        {
            builder.HasKey(gt => gt.Id);

            builder.HasOne(gt => gt.Good)
                .WithMany(g => g.GoodTransactions)
                .HasForeignKey(gt => gt.GoodId)
                .IsRequired();

            builder.HasOne(gt => gt.StoreLocation)
                .WithMany(sl => sl.GoodTransactions)
                .HasForeignKey(gt => gt.StoreLocationId)
                .IsRequired();
        }
    }
}