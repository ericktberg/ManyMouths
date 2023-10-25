using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    public partial class ManyMouthsContext : DbContext
    {
        public ManyMouthsContext(DbContextOptions<ManyMouthsContext> context) : base(context)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<RecipeQuant> RecipeQuants { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Good>().Property(x => x.Id).HasColumnName("good_id");
            builder.Entity<Ingredient>().Property(x => x.Id).HasColumnName("ingredient_id");
            builder.Entity<IngredientMapping>().Property(x => x.Id).HasColumnName("mapping_id");
            builder.Entity<Recipe>().Property(x => x.Id).HasColumnName("recipe_id");
            builder.Entity<RecipeOwner>().Property(x => x.Id).HasColumnName("recipe_owner_id");
            builder.Entity<RecipeQuant>().Property(x => x.Id).HasColumnName("recipe_quant_id");
            builder.Entity<StoreChain>().Property(x => x.Id).HasColumnName("store_chain_id");
            builder.Entity<StoreLocation>().Property(x => x.Id).HasColumnName("store_location_id");
            builder.Entity<User>().Property(x => x.Id).HasColumnName("user_id");

            base.OnModelCreating(builder);
        }
    }
}