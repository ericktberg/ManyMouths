using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Persistence.Configurations
{

    public class RecipeOwnerConfiguration : IEntityTypeConfiguration<RecipeOwner>
    {
        public void Configure(EntityTypeBuilder<RecipeOwner> builder)
        {
            builder.HasKey(ro => new { ro.RecipeId, ro.UserId });

            builder.HasOne(ro => ro.Recipe)
                .WithMany(r => r.RecipeOwners)
                .HasForeignKey(ro => ro.RecipeId)
                .IsRequired();

            builder.HasOne(ro => ro.User)
                .WithMany(u => u.OwnedRecipes)
                .HasForeignKey(ro => ro.UserId)
                .IsRequired();
        }
    }
}