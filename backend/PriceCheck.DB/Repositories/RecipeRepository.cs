using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence;
using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.Repositories
{
    /// <summary>
    /// Options for querying recipes from the repository.
    /// </summary>
    public record RecipeQueryOptions
    {
        /// <summary>
        /// If set, filters results to a specific recipe by ID.
        /// </summary>
        public int? RecipeId { get; init; } = null;

        /// <summary>
        /// If true, includes detailed ingredient information in the query results.
        /// </summary>
        public bool IncludeIngredientDetails { get; init; } = false;
    }

    /// <summary>
    /// Defines the contract for a repository that manages recipes and their related data.
    /// </summary>
    public interface IRecipeRepository
    {
        /// <summary>
        /// Adds a collection of <see cref="RecipeQuant"/> entities to the database.
        /// </summary>
        /// <param name="recipeQuants">The recipe quantities to add.</param>
        /// <returns>The added recipe quantities.</returns>
        Task<ICollection<RecipeQuant>> AddRecipeQuantitiesAsync(ICollection<RecipeQuant> recipeQuants);

        /// <summary>
        /// Creates a new recipe in the database.
        /// </summary>
        /// <param name="recipe">The recipe to create.</param>
        /// <returns>The created recipe, including its generated ID.</returns>
        Task<Recipe> CreateRecipeAsync(Recipe recipe);

        /// <summary>
        /// Finds existing ingredients by name or adds new ones if they do not exist.
        /// </summary>
        /// <param name="ingredients">The ingredients to find or add.</param>
        /// <returns>The collection of ingredients, with IDs populated.</returns>
        /// <remarks>
        /// This method checks for existing ingredients by name and only adds those that are not present.
        /// </remarks>
        Task<ICollection<Ingredient>> FindOrAddIngredientsAsync(ICollection<Ingredient> ingredients);

        /// <summary>
        /// Fetches recipes from the database according to the specified query options.
        /// </summary>
        /// <param name="options">Query options for filtering and including related data.</param>
        /// <returns>A collection of recipes matching the query.</returns>
        /// <remarks>
        /// If <c>IncludeIngredientDetails</c> is true, ingredient quantities and their details are eagerly loaded.
        /// </remarks>
        Task<ICollection<Recipe>> FetchRecipesAsync(RecipeQueryOptions options);

        /// <summary>
        /// Updates a recipe by applying a mutator action and saves changes to the database.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe to update.</param>
        /// <param name="mutator">An action that mutates the recipe entity.</param>
        /// <returns>The updated recipe.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the recipe does not exist.</exception>
        Task<Recipe> UpdateRecipeAsync(int recipeId, Action<Recipe> mutator);

        /// <summary>
        /// Deletes all <see cref="RecipeQuant"/> entries associated with a given recipe ID.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe whose quantities should be deleted.</param>
        Task DeleteRecipeQuantitiesByRecipeIdAsync(int recipeId);

        /// <summary>
        /// Deletes a recipe by its ID.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe to delete.</param>
        Task DeleteRecipeByIdAsync(int recipeId);
    }

    /// <summary>
    /// Repository for managing recipes, their ingredients, and related data using Entity Framework Core.
    /// </summary>
    /// <remarks>
    /// This repository encapsulates all data access logic for recipes, including creation, update, deletion, and ingredient management.
    /// It uses EF Core's DbContext for persistence and supports eager loading of related entities as needed.
    /// </remarks>
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ManyMouthsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to use for data access.</param>
        public RecipeRepository(ManyMouthsDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// If the recipe does not exist, this method does nothing.
        /// </remarks>
        public async Task DeleteRecipeByIdAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Supports filtering by recipe ID and eager loading of ingredient details. Uses EF Core's <c>Include</c> and <c>ThenInclude</c> for navigation properties.
        /// </remarks>
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

        /// <inheritdoc/>
        /// <remarks>
        /// Throws if the recipe does not exist. The mutator action is applied to the tracked entity, and changes are saved.
        /// </remarks>
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

        /// <inheritdoc/>
        /// <remarks>
        /// The recipe entity is added and saved, and its generated ID is populated.
        /// </remarks>
        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;  // Will have ID added to it
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Checks for existing ingredients by name. Only new ingredients are added and saved. This avoids duplicate ingredient names in the database.
        /// </remarks>
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

        /// <inheritdoc/>
        /// <remarks>
        /// Adds all provided <see cref="RecipeQuant"/> entities and saves changes. Assumes composite primary key prevents duplicates.
        /// </remarks>
        public async Task<ICollection<RecipeQuant>> AddRecipeQuantitiesAsync(ICollection<RecipeQuant> recipeQuants)
        {
            _context.RecipeQuants.AddRange(recipeQuants);
            await _context.SaveChangesAsync();
            return recipeQuants; // These have a composite primary key, so nothing will be added.
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Removes all <see cref="RecipeQuant"/> entries for the specified recipe and saves changes.
        /// </remarks>
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