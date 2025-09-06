using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Persistence.Configurations
{
    public class StoreChainConfiguration : IEntityTypeConfiguration<StoreChain>
    {
        public void Configure(EntityTypeBuilder<StoreChain> builder)
        {
            builder.HasKey(sc => sc.StoreChainId);
        }
    }
}