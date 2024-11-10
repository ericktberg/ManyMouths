using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    public partial class ManyMouthsContext : DbContext
    {
        public ManyMouthsContext(DbContextOptions<ManyMouthsContext> context) : base(context)
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
            /* Recipe Ownership */
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

            /* Recipe Ingredients */
            modelBuilder.Entity<RecipeQuant>()
                .HasOne(rq => rq.Recipe)
                .WithMany(r => r.IngredientQuantities)
                .HasForeignKey(rq => rq.RecipeId)
                .IsRequired();

            modelBuilder.Entity<RecipeQuant>()
                .HasOne(rq => rq.Ingredient)
                .WithMany()
                .HasForeignKey(rq => rq.IngredientId)
                .IsRequired();

            modelBuilder.Entity<Good>()
                .HasKey(g => g.Id);

            /* Store Locations  */
            modelBuilder.Entity<StoreLocation>()
                .HasKey(sl => sl.StoreLocationId);

            modelBuilder.Entity<StoreChain>()
                .HasKey(sc => sc.StoreChainId);

            /* Good Transactions */
            modelBuilder.Entity<GoodTransaction>()
                .HasKey(gt => gt.Id);

            modelBuilder.Entity<GoodTransaction>()
                .HasOne(gt => gt.Good)
                .WithMany(g => g.GoodTransactions)
                .HasForeignKey(gt => gt.GoodId)
                .IsRequired();

            modelBuilder.Entity<GoodTransaction>()
                .HasOne(gt => gt.StoreLocation)
                .WithMany(sl => sl.GoodTransactions)
                .HasForeignKey(gt => gt.StoreLocationId)
                .IsRequired();

            /* Ingredient Mapping */
            modelBuilder.Entity<IngredientMapping>()
                .HasOne(im => im.Ingredient)
                .WithMany(i => i.Mappings)
                .HasForeignKey(im => im.IngredientId)
                .IsRequired();

            modelBuilder.Entity<IngredientMapping>()
                .HasOne(im => im.Good)
                .WithMany(g => g.IngredientMappings)
                .HasForeignKey(im => im.GoodId)
                .IsRequired();

            /* Ingredient Mapping Selection */
            modelBuilder.Entity<SelectedIngredientMapping>()
                .HasKey(ims => new { ims.UserId, ims.IngredientId });

            modelBuilder.Entity<SelectedIngredientMapping>()
                .HasOne(ims => ims.User)
                .WithMany(u => u.SelectedIngredientMappings)
                .HasForeignKey(ims => ims.UserId)
                .IsRequired();

            modelBuilder.Entity<SelectedIngredientMapping>()
                .HasOne(ims => ims.Ingredient)
                .WithMany()
                .HasForeignKey(ims => ims.IngredientId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}