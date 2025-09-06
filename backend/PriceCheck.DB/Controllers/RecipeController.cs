using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence;
using PriceCheck.DB.Services;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RecipeDetailDTO), 200)]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeCreationDto recipeDto)
        {
            if (recipeDto == null
                || string.IsNullOrEmpty(recipeDto.Name)
                || recipeDto.Ingredients == null
                || recipeDto.Ingredients.Count == 0)
            {
                return BadRequest("Invalid recipe data.");
            }

            try
            {
                RecipeDetailDTO createdRecipe = await _recipeService.CreateNewRecipeAsync(recipeDto);

                return CreatedAtAction(
                    nameof(GetRecipeDetails),
                    new { recipeId = createdRecipe.Id }, // matches the route parameter exactly
                    createdRecipe
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the recipe: {ex.Message}");
            }
        }

        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            await _recipeService.DeleteRecipeAsync(recipeId);

            return NoContent();
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<RecipeOverviewDTO>), 200)]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeService.GetRecipeOverviewListAsync();

            if (recipes == null)
            {
                return NotFound();
            }

            return Ok(recipes);
        }

        [HttpGet("{recipeId}")]
        [ProducesResponseType(typeof(RecipeDetailDTO), 200)]
        public async Task<IActionResult> GetRecipeDetails(int recipeId)
        {
            var recipe = await _recipeService.GetRecipeDetailsAsync(recipeId);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpPut("{recipeId}")]
        [ProducesResponseType(typeof(RecipeDetailDTO), 200)]
        public async Task<IActionResult> UpdateRecipe(int recipeId, [FromBody] RecipeCreationDto recipe)
        {
            if (recipe is null)
            {
                return BadRequest("Invalid recipe data.");
            }

            try
            {
                RecipeDetailDTO modifiedRecipe = await _recipeService.UpdateRecipeAsync(recipeId, recipe);
                return Ok(modifiedRecipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the recipe: {ex.Message}");
            }
        }
    }
}