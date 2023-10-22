using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.DTOs;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/recipes")]
    public class RecipeController : ControllerBase
    {
        public RecipeController(ManyMouthsContext db)
        {
            Db = db;
        }

        public ManyMouthsContext Db { get; }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RecipeDTO>))]
        public async Task<IActionResult> GetList(Guid userId)
        {
            var ownedRecipes = Db.Recipes
                .Include(r => r.IngredientQuantities)
                    .ThenInclude(q => q.Ingredient)
                .Include(r => r.RecipeOwners)
                    .ThenInclude(r => r.User)
                .Where(r => r.RecipeOwners.Any(o => o.UserId == userId));

            var dto = ownedRecipes
                .Select(RecipeDTO.FromRecipe)
                .ToList();

            return Ok(dto);
        }

        [HttpPut("{recipeId}")]
        public IActionResult Put(Guid userId, Guid recipeId, RecipeDTO recipeDTO)
        {
            var recipe = Db.Recipes.Where(r => r.RecipeId == recipeId).FirstOrDefault();
            recipe ??= new Recipe()
            {
                RecipeId = recipeId
            };
            Db.Project(recipe, recipeDTO);

            Db.Recipes.Add(recipe);
            var owner = new RecipeOwner()
            {
                UserId = userId,
                Recipe = recipe
            };
            Db.RecipeOwners.Add(owner);
            Db.SaveChanges();

            return Created(recipe.RecipeId.ToString(), RecipeDTO.FromRecipe(recipe));
        }

        [HttpPost("")]
        public IActionResult Post(Guid userId, RecipeDTO recipeDTO)
        {
            var recipe = new Recipe()
            {
                RecipeId = GuidInterop.CreateGuid()
            };
            Db.Project(recipe, recipeDTO);

            Db.Recipes.Add(recipe);
            var owner = new RecipeOwner()
            {
                UserId = userId,
                Recipe = recipe
            };
            Db.RecipeOwners.Add(owner);
            Db.SaveChanges();

            return Created(recipe.RecipeId.ToString(), RecipeDTO.FromRecipe(recipe));
        }
    }
}