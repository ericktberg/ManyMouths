using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Persistence.Configurations
{
    public class SelectedIngredientMappingConfiguration : IEntityTypeConfiguration<SelectedIngredientMapping>
    {
        public void Configure(EntityTypeBuilder<SelectedIngredientMapping> builder)
        {
            builder.HasKey(ims => new { ims.UserId, ims.IngredientId });

            builder.HasOne(ims => ims.User)
                .WithMany(u => u.SelectedIngredientMappings)
                .HasForeignKey(ims => ims.UserId)
                .IsRequired();

            builder.HasOne(ims => ims.Ingredient)
                .WithMany()
                .HasForeignKey(ims => ims.IngredientId)
                .IsRequired();
        }
    }
}