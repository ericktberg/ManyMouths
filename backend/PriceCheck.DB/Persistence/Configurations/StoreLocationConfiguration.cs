using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Persistence.Configurations
{
    public class StoreLocationConfiguration : IEntityTypeConfiguration<StoreLocation>
    {
        public void Configure(EntityTypeBuilder<StoreLocation> builder)
        {
            builder.HasKey(sl => sl.StoreLocationId);
        }
    }
}