using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.DTOs;

namespace PriceCheck.DB.ORM
{
    public partial class ManyMouthsContext
    {
        public Ingredient GetOrAddIngredient(string ingredientName)
        {
            var ingredient = Ingredients.FirstOrDefault(i => string.Equals(i.IngredientName, ingredientName));
            if (ingredient is null)
            {
                ingredient = new Ingredient()
                {
                    IngredientId = GuidInterop.CreateGuid(),
                    IngredientName = ingredientName
                };
                Ingredients.Add(ingredient);
            }

            return ingredient;
        }

        public RecipeQuant GetOrAddQuant(Recipe recipe, Ingredient ingredient, RecipeIngredientDTO ingredientDTO)
        {
            var quant = RecipeQuants.FirstOrDefault(rq => rq.Ingredient.Equals(ingredient) && rq.Recipe.Equals(recipe));
            if (quant is null)
            {
                quant = new RecipeQuant()
                {
                    Ingredient = ingredient,
                    Recipe = recipe,
                    Quantity = (int)(ingredientDTO.Quantity * 100),
                    Unit = ingredientDTO.Unit
                };
                RecipeQuants.Add(quant);
            }

            return quant;
        }
    }

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
            builder.Entity<User>().Property(x => x.Id).HasColumnName("user_id");
            builder.Entity<Recipe>().Property(x => x.Id).HasColumnName("recipe_id");
            builder.Entity<RecipeOwner>().Property(x => x.Id).HasColumnName("recipe_owner_id");

            builder.Entity<RecipeQuant>()
                .HasOne(rq => rq.Ingredient)
                .WithMany()
                .HasForeignKey(rq => rq.IngredientId)
                .IsRequired();

            builder.Entity<IngredientMapping>()
                .HasOne(im => im.Ingredient)
                .WithMany(i => i.Mappings)
                .HasForeignKey(im => im.IngredientId)
                .IsRequired();

            builder.Entity<IngredientMapping>()
                .HasOne(im => im.Good)
                .WithMany(g => g.IngredientMappings)
                .HasForeignKey(im => im.GoodId)
                .IsRequired();

            base.OnModelCreating(builder);
        }
    }

}