using System;

using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Persistence
{
    public partial class ManyMouthsDbContext : DbContext
    {
        public ManyMouthsDbContext(DbContextOptions<ManyMouthsDbContext> context) : base(context)
        {
        }

        public DbSet<Good> Goods { get; set; }
        public DbSet<GoodTransaction> GoodTransactions { get; set; }
        public DbSet<IngredientMapping> IngredientMappings { get; set; }

        public DbSet<SelectedIngredientMapping> IngredientMappingSelections { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeOwner> RecipeOwners { get; set; }
        public DbSet<RecipeQuant> RecipeQuants { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<StoreChain> StoreChains { get; set; }
        public DbSet<StoreLocation> StoreLocations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManyMouthsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}