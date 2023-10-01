using Microsoft.EntityFrameworkCore;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB.Controllers
{
    public class ManyMouthsContext : DbContext
    {
        public ManyMouthsContext(DbContextOptions<ManyMouthsContext> context) : base(context)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeOwner>()
                .HasKey(ro => new { ro.RecipeId, ro.UserId });

            modelBuilder.Entity<RecipeOwner>()
                .HasOne(ro => ro.Recipe)
                .WithMany(r => r.RecipeOwners)
                .HasForeignKey(ro => ro.RecipeId)
                .IsRequired();

            modelBuilder.Entity<RecipeOwner>()
                .HasOne(ro => ro.User)
                .WithMany(u => u.OwnedRecipes)
                .HasForeignKey(ro => ro.UserId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<RecipeOwner> RecipeOwners { get; set; }
    }
}