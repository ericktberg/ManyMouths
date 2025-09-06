using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Persistence.Configurations
{
    public class IngredientMappingConfiguration : IEntityTypeConfiguration<IngredientMapping>
    {
        public void Configure(EntityTypeBuilder<IngredientMapping> builder)
        {
            builder.HasOne(im => im.Ingredient)
                .WithMany(i => i.Mappings)
                .HasForeignKey(im => im.IngredientId)
                .IsRequired();

            builder.HasOne(im => im.Good)
                .WithMany(g => g.IngredientMappings)
                .HasForeignKey(im => im.GoodId)
                .IsRequired();
        }
    }
}