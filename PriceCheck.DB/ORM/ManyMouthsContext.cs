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

        public void Project(Recipe recipe, RecipeDTO recipeDTO)
        {
            recipe.RecipeName = recipeDTO.Name;
            foreach (var ingredientDTO in recipeDTO.Ingredients)
            {
                var ingredient = GetOrAddIngredient(ingredientDTO.IngredientName);
                var quant = GetOrAddQuant(recipe, ingredient, ingredientDTO);

                if (recipe.IngredientQuantities.Contains(quant)) continue;
                else
                {
                    recipe.IngredientQuantities.Add(quant);
                }
            }
        }
    }

    public partial class ManyMouthsContext : DbContext
    {
        public ManyMouthsContext(DbContextOptions<ManyMouthsContext> context) : base(context)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<RecipeOwner> RecipeOwners { get; set; }

        public DbSet<RecipeQuant> RecipeQuants { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().Property(x => x.Id).HasColumnName("user_id");

            builder.Entity<RecipeOwner>()
                .HasKey(ro => new { ro.RecipeId, ro.UserId });

            builder.Entity<RecipeOwner>()
                .HasOne(ro => ro.Recipe)
                .WithMany(r => r.RecipeOwners)
                .HasForeignKey(ro => ro.RecipeId)
                .IsRequired();

            builder.Entity<RecipeOwner>()
                .HasOne(ro => ro.User)
                .WithMany(u => u.OwnedRecipes)
                .HasForeignKey(ro => ro.UserId)
                .IsRequired();

            builder.Entity<RecipeQuant>()
                .HasOne(rq => rq.Recipe)
                .WithMany(r => r.IngredientQuantities)
                .HasForeignKey(rq => rq.RecipeId)
                .IsRequired();

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