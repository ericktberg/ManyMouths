using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.DTOs;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly ManyMouthsContext _context;

        public RecipesController(ManyMouthsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeCreationDto recipeDto)
        {
            if (recipeDto == null || string.IsNullOrEmpty(recipeDto.Name) || recipeDto.Ingredients == null || !recipeDto.Ingredients.Any())
            {
                return BadRequest("Invalid recipe data.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create the recipe
                var recipe = new Recipe
                {
                    Name = recipeDto.Name,
                };
                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                // Associate the recipe with the user
                var recipeOwner = new RecipeOwner
                {
                    RecipeId = recipe.Id,
                    UserId = recipeDto.UserId
                };
                _context.RecipeOwners.Add(recipeOwner);

                // Process each ingredient
                foreach (var ingredientDto in recipeDto.Ingredients)
                {
                    if (string.IsNullOrEmpty(ingredientDto.Name) || ingredientDto.Quantity <= 0)
                    {
                        return BadRequest("Invalid ingredient data.");
                    }

                    // Check if the ingredient already exists
                    var ingredient = await _context.Ingredients
                        .FirstOrDefaultAsync(i => i.Name == ingredientDto.Name);

                    // If the ingredient doesn't exist, create a new one
                    if (ingredient == null)
                    {
                        ingredient = new Ingredient
                        {
                            Name = ingredientDto.Name
                        };
                        _context.Ingredients.Add(ingredient);
                        await _context.SaveChangesAsync();
                    }

                    // Create the RecipeQuant entry
                    var recipeQuant = new RecipeQuant
                    {
                        RecipeId = recipe.Id,
                        IngredientId = ingredient.Id,
                        Quantity = (int)(ingredientDto.Quantity * 100), // Assuming quantity is in grams,
                        Unit = ingredientDto.Unit
                    };
                    _context.RecipeQuants.Add(recipeQuant);
                }

                // Save all changes and commit the transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return the created recipe with its ingredients
                var createdRecipe = await _context.Recipes
                    .Include(r => r.IngredientQuantities)
                    .ThenInclude(rq => rq.Ingredient)
                    .FirstOrDefaultAsync(r => r.Id == recipe.Id);

                return CreatedAtAction(nameof(GetRecipeDetails), recipe.Id, new RecipeDTO(createdRecipe));
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred while creating the recipe: {ex.Message}");
            }
        }

        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            var recipe = await _context.Recipes
                .Include(r => r.IngredientQuantities)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<RecipeOverviewDTO>), 200)]
        public async Task<IActionResult> GetAllRecipes()
        {
            List<Recipe> recipes = await _context.Recipes
                //.Include(r => r.IngredientQuantities)
                //    .ThenInclude(rq => rq.Ingredient)
                //    .ThenInclude(i => i.Mappings)
                //    .ThenInclude(map => map.Good)
                .ToListAsync();

            if (recipes == null)
            {
                return NotFound();
            }

            return Ok(recipes.Select(recipe => new RecipeOverviewDTO(recipe)));
        }

        [HttpGet("{recipeId}")]
        [ProducesResponseType(typeof(RecipeDetailDTO), 200)]
        public async Task<IActionResult> GetRecipeDetails(int recipeId)
        {
            var recipeObj = await _context.Recipes
                .Include(r => r.IngredientQuantities)
                .ThenInclude(rq => rq.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipeObj == null)
            {
                return NotFound();
            }

            /* Get ingredient mappings now */
            var ingredientMappings = _context.Users
                .Where(u => u.UserId == 1)
                .Include(u => u.SelectedIngredientMappings)
                .ThenInclude(sim => sim.Mapping)
                .ThenInclude(m => m.Good);
            Dictionary<int, GoodDTOLight?> ingredientMappingsDict = new();
            foreach (var i in recipeObj.IngredientQuantities.Select(iq => iq.IngredientId))
            {
                var ingredientMapping = await ingredientMappings
                    .SelectMany(u => u.SelectedIngredientMappings)
                    .FirstOrDefaultAsync(im => im.IngredientId == i);

                var goodDto = ingredientMapping?.Mapping == null ? null : new GoodDTOLight(ingredientMapping.Mapping.Good);
                ingredientMappingsDict.Add(i, goodDto);
            }

            if (recipeObj == null)
            {
                return NotFound();
            }

            var recipe = new RecipeDetailDTO(recipeObj, ingredientMappingsDict);

            return Ok(recipe);
        }

        [HttpPut("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, [FromBody] Recipe recipe)
        {
            if (recipe == null || recipeId != recipe.Id)
            {
                return BadRequest("Invalid recipe data.");
            }

            var existingRecipe = await _context.Recipes
                .Include(r => r.IngredientQuantities)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (existingRecipe == null)
            {
                return NotFound();
            }

            existingRecipe.Name = recipe.Name;
            // Update other properties as needed

            // Update ingredients
            _context.RecipeQuants.RemoveRange(existingRecipe.IngredientQuantities);

            foreach (var ingredientDto in recipe.IngredientQuantities)
            {
                var ingredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.Name == ingredientDto.Ingredient.Name);

                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Name = ingredientDto.Ingredient.Name
                    };
                    _context.Ingredients.Add(ingredient);
                    await _context.SaveChangesAsync();
                }

                var recipeQuant = new RecipeQuant
                {
                    RecipeId = recipe.Id,
                    IngredientId = ingredient.Id,
                    Quantity = ingredientDto.Quantity
                };
                _context.RecipeQuants.Add(recipeQuant);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}