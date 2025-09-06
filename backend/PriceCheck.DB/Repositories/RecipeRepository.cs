using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence;
using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Repositories
{
    public record RecipeQueryOptions
    {
        public int? RecipeId { get; init; } = null;

        public bool IncludeIngredientDetails { get; init; } = false;
    }

    public interface IRecipeRepository
    {
        Task<ICollection<RecipeQuant>> AddRecipeQuantitiesAsync(ICollection<RecipeQuant> recipeQuants);

        Task<Recipe> CreateRecipeAsync(Recipe recipe);

        Task<ICollection<Ingredient>> FindOrAddIngredientsAsync(ICollection<Ingredient> ingredients);

        Task<ICollection<Recipe>> FetchRecipesAsync(RecipeQueryOptions options);
        Task<Recipe> UpdateRecipeAsync(int recipeId, Action<Recipe> mutator);
        Task DeleteRecipeQuantitiesByRecipeIdAsync(int recipeId);
        Task DeleteRecipeByIdAsync(int recipeId);
    }

    public class RecipeRepository : IRecipeRepository
    {
        private readonly ManyMouthsDbContext _context;

        public RecipeRepository(ManyMouthsDbContext context)
        {
            _context = context;
        }

        public async Task DeleteRecipeByIdAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Recipe>> FetchRecipesAsync(RecipeQueryOptions options)
        {
            var query = _context.Recipes.AsQueryable();
            if (options.RecipeId.HasValue)
            {
                query = query.Where(r => r.Id == options.RecipeId.Value);
            }

            if (options.IncludeIngredientDetails)
            {
                query = query
                    .Include(r => r.IngredientQuantities)
                    .ThenInclude(rq => rq.Ingredient);
            }

            return await query.ToListAsync();

            // TODO: Do the recipe costing portion of things
            //var ingredientMappings = _context.Users
            // .Where(u => u.UserId == 1)
            // .Include(u => u.SelectedIngredientMappings)
            // .ThenInclude(sim => sim.Mapping)
            // .ThenInclude(m => m.Good);

            //Dictionary<int, GoodDTOLight?> ingredientMappingsDict = new();
            //foreach (var i in recipeObj.IngredientQuantities.Select(iq => iq.IngredientId))
            //{
            //    var ingredientMapping = await ingredientMappings
            //        .SelectMany(u => u.SelectedIngredientMappings)
            //        .FirstOrDefaultAsync(im => im.IngredientId == i);

            //    var goodDto = ingredientMapping?.Mapping == null ? null : new GoodDTOLight(ingredientMapping.Mapping.Good);
            //    ingredientMappingsDict.Add(i, goodDto);
            //}
        }

        public async Task<Recipe> UpdateRecipeAsync(int recipeId, Action<Recipe> mutator)
        {
            var recipes = await FetchRecipesAsync(new RecipeQueryOptions { RecipeId = recipeId });
            var recipe = recipes.FirstOrDefault();
            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with ID {recipeId} not found.");
            }
            mutator(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        /// <summary>
        /// Create a new recipe in the database with the given recipe object.
        /// </summary>
        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;  // Will have ID added to it
        }

        /// <summary>
        /// Try to create a new ingredient in the database. If an ingredient with the same name already exists,
        /// then return the existing ingredient instead.
        /// </summary>
        public async Task<ICollection<Ingredient>> FindOrAddIngredientsAsync(ICollection<Ingredient> ingredients)
        {
            if (ingredients.Count == 0)
            {
                return [];
            }

            var ingredientNames = ingredients.Select(i => i.Name).ToList();

            // Have to check for existing ingredients first
            // This could be done in a single SQL statement with MERGE, but EF Core doesn't support that.
            var existingIngredients = await _context.Ingredients
                .Where(i => ingredientNames.Contains(i.Name))
                .ToListAsync();

            var existingNames = existingIngredients.Select(i => i.Name).ToHashSet();
            var newIngredients = ingredients
                .Where(i => !existingNames.Contains(i.Name))
                .ToList();

            if (newIngredients.Count != 0)
            {
                _context.Ingredients.AddRange(newIngredients);
                await _context.SaveChangesAsync();
                // newIngredients now have database-generated IDs
            }

            return ingredients;
        }

        public async Task<ICollection<RecipeQuant>> AddRecipeQuantitiesAsync(ICollection<RecipeQuant> recipeQuants)
        {
            _context.RecipeQuants.AddRange(recipeQuants);
            await _context.SaveChangesAsync();
            return recipeQuants; // These have a composite primary key, so nothing will be added.
        }

        public async Task DeleteRecipeQuantitiesByRecipeIdAsync(int recipeId)
        {
            var toDelete = await _context.RecipeQuants
                .Where(rq => rq.RecipeId == recipeId)
                .ToListAsync();
            _context.RecipeQuants.RemoveRange(toDelete);
            await _context.SaveChangesAsync();
        }
    }
}