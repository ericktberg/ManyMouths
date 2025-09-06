using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Persistence.Configurations
{
    public class RecipeQuantConfiguration : IEntityTypeConfiguration<RecipeQuant>
    {
        public void Configure(EntityTypeBuilder<RecipeQuant> builder)
        {
            builder.HasOne(rq => rq.Recipe)
                .WithMany(r => r.IngredientQuantities)
                .HasForeignKey(rq => rq.RecipeId)
                .IsRequired();

            builder.HasOne(rq => rq.Ingredient)
                .WithMany()
                .HasForeignKey(rq => rq.IngredientId)
                .IsRequired();
        }
    }
}