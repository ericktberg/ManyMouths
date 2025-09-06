using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence;
using PriceCheck.DB.Persistence.Entities;
using PriceCheck.DB.Repositories;

namespace PriceCheck.DB.Services
{
    /// <summary>
    /// A service for accessing the
    /// </summary>
    public interface IRecipeService
    {
        Task<RecipeDetailDTO> CreateNewRecipeAsync(RecipeCreationDto recipeDto);
        Task DeleteRecipeAsync(int recipeId);
        Task<RecipeDetailDTO?> GetRecipeDetailsAsync(int recipeId);

        Task<ICollection<RecipeOverviewDTO>> GetRecipeOverviewListAsync();
        Task<RecipeDetailDTO> UpdateRecipeAsync(int recipeId, RecipeCreationDto recipeDto);
    }

    public class RecipeService : IRecipeService
    {
        private readonly ManyMouthsDbContext _context;
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(ManyMouthsDbContext context, IRecipeRepository recipeRepository)
        {
            _context = context;
            _recipeRepository = recipeRepository;
        }

        public async Task<ICollection<RecipeOverviewDTO>> GetRecipeOverviewListAsync()
        {
            var recipes = await _recipeRepository.FetchRecipesAsync(new RecipeQueryOptions());
            return recipes.Select(r => new RecipeOverviewDTO
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                PrepTimeMinutes = r.PrepTimeMinutes,
                CookTimeMinutes = r.CookTimeMinutes,
                Servings = r.Servings
            }).ToList();
        }

        public async Task<RecipeDetailDTO?> GetRecipeDetailsAsync(int recipeId)
        {
            var recipes = await _recipeRepository.FetchRecipesAsync(new RecipeQueryOptions
            {
                RecipeId = recipeId,
                IncludeIngredientDetails = true
            });
            if (recipes is null || recipes.Count != 1)
            {
                return null;
            }

            var recipe = recipes.First();
            return MapDetailDTO(recipe, recipe.IngredientQuantities);
        }

        public async Task DeleteRecipeAsync(int recipeId)
        {
            await _recipeRepository.DeleteRecipeByIdAsync(recipeId);
        }

        public async Task<RecipeDetailDTO> CreateNewRecipeAsync(RecipeCreationDto recipeDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Add and save the recipe itself.
                    // The generated id will be needed to associate other elements with it.
                    var recipe = new Recipe
                    {
                        Name = recipeDto.Name,
                        Description = recipeDto.Description,
                        CookTimeMinutes = recipeDto.CookTimeMinutes,
                        PrepTimeMinutes = recipeDto.PrepTimeMinutes,
                        Servings = recipeDto.Servings,
                        MarkdownInstructions = recipeDto.InstructionMarkdownText
                    };
                    Recipe dbRecipe = await _recipeRepository.CreateRecipeAsync(recipe);

                    // Then add all ingredients to the database so that they can be referenced.
                    var ingredients = recipeDto.Ingredients
                        .Select(i => new Ingredient { Name = i.Name })
                        .ToList();
                    ICollection<Ingredient> dbIngredients = await _recipeRepository.FindOrAddIngredientsAsync(ingredients);

                    // Then create the RecipeQuant entries to associate ingredients with the recipe.
                    var ingredientDict = dbIngredients.ToDictionary(i => i.Name, i => i);
                    var recipeQuantities = recipeDto.Ingredients
                        .Select(i => new RecipeQuant
                        {
                            RecipeId = dbRecipe.Id,
                            IngredientId = ingredientDict[i.Name].Id,
                            Quantity = (int)(i.Quantity * 100),  // Using .01 value resolution
                            Unit = i.Unit
                        })
                        .ToList();
                    var dbRecipeQuantities = await _recipeRepository.AddRecipeQuantitiesAsync(recipeQuantities);

                    // Commit the transaction now that everything has succeeded.
                    await transaction.CommitAsync();

                    // Construct the RecipeDetailDTO from our scattered data
                    // Do not use ".Include" from EF Core as that would require re-querying the database.
                    var recipeDetail = MapDetailDTO(dbRecipe, dbRecipeQuantities);

                    return recipeDetail;
                }
                catch
                {
                    // Rollback and throw so that the error can be handled upstream.
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<RecipeDetailDTO> UpdateRecipeAsync(int recipeId, RecipeCreationDto recipeDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Recipe updatedRecipe = await _recipeRepository.UpdateRecipeAsync(recipeId, existingRecipe =>
                    {
                        existingRecipe.Name = recipeDto.Name;
                        existingRecipe.Description = recipeDto.Description;
                        existingRecipe.CookTimeMinutes = recipeDto.CookTimeMinutes;
                        existingRecipe.PrepTimeMinutes = recipeDto.PrepTimeMinutes;
                        existingRecipe.Servings = recipeDto.Servings;
                        existingRecipe.MarkdownInstructions = recipeDto.InstructionMarkdownText;
                    });

                    // Handle ingredients: First ensure all ingredients exist in the database
                    var ingredients = recipeDto.Ingredients
                        .Select(i => new Ingredient { Name = i.Name })
                        .ToList();
                    ICollection<Ingredient> dbIngredients = await _recipeRepository.FindOrAddIngredientsAsync(ingredients);

                    // Remove all existing RecipeQuant entries for this recipe
                    // This is simpler than trying to diff and update individual quantities
                    await _recipeRepository.DeleteRecipeQuantitiesByRecipeIdAsync(recipeId);

                    // Create new RecipeQuant entries with the updated data
                    var ingredientDict = dbIngredients.ToDictionary(i => i.Name, i => i);
                    var recipeQuantities = recipeDto.Ingredients
                        .Select(i => new RecipeQuant
                        {
                            RecipeId = recipeId, // Use the existing recipe ID
                            IngredientId = ingredientDict[i.Name].Id,
                            Quantity = (int)(i.Quantity * 100),  // Using .01 value resolution
                            Unit = i.Unit
                        })
                        .ToList();

                    var dbRecipeQuantities = await _recipeRepository.AddRecipeQuantitiesAsync(recipeQuantities);

                    // Commit the transaction
                    await transaction.CommitAsync();

                    // Return the updated recipe detail
                    var recipeDetail = MapDetailDTO(updatedRecipe, dbRecipeQuantities);
                    return recipeDetail;
                }
                catch
                {
                    // Rollback and re-throw
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private static RecipeDetailDTO MapDetailDTO(Recipe recipe, ICollection<RecipeQuant> ingredientQuantities)
        {
            return new RecipeDetailDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                PrepTimeMinutes = recipe.PrepTimeMinutes,
                CookTimeMinutes = recipe.CookTimeMinutes,
                Servings = recipe.Servings,
                MarkdownInstructions = recipe.MarkdownInstructions,
                Ingredients = ingredientQuantities.Select(iq => new RecipeIngredientDTO
                {
                    Id = iq.Ingredient.Id,
                    Name = iq.Ingredient.Name,
                    Quantity = iq.Quantity / 100.0,  // Convert back to standard units
                    Unit = iq.Unit
                }).ToList()
            };
        }

    }
}