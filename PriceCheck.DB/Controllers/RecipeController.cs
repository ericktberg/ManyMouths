using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.Controllers
{
    public class ManyMouthsContext : DbContext
    {
        public ManyMouthsContext(DbContextOptions<ManyMouthsContext> context) : base(context)
        {
        }

        public DbSet<UserRecord> Users { get; set; }
    }

    [ApiController]
    [Route("users/{userId}/recipes")]
    public class RecipeController : ControllerBase
    {
        public RecipeController(ManyMouthsDb db)
        {
            Db = db;
        }

        public ManyMouthsDb Db { get; }

        //[HttpPost(Name = "users/{userId}/recipes")]
        //public async Task<IActionResult> Post(int inputUserId)
        //{
        //    var connection = Db.OpenConnection();
        //    var command = connection.CreateCommand();

        //    command.CommandText =
        //        "SELECT many_mouths.recipe.recipe_id, many_mouths.recipe.recipe_name" +
        //        "FROM many_mouths.recipe" +
        //        "INNER JOIN many_mouths.recipe_owner ON many_mouths.recipe_owner.recipe_id = many_mouths.recipe.recipe_id" +
        //        "INNER JOIN many_mouths.user ON many_mouths.recipe_owner.user_id = many_mouths.user.user_id" +
        //        "WHERE many_mouths.user.user_id=@userId";

        //}

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RecipeRecord>))]
        public async Task<IActionResult> GetList(int userId)
        {
            var connection = Db.OpenConnection();
            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT many_mouths.recipe.recipe_id, many_mouths.recipe.recipe_name " +
                "FROM many_mouths.recipe " +
                "INNER JOIN many_mouths.recipe_owner ON many_mouths.recipe_owner.recipe_id = many_mouths.recipe.recipe_id " +
                "INNER JOIN many_mouths.user ON many_mouths.recipe_owner.user_id = many_mouths.user.user_id " +
                "WHERE many_mouths.user.user_id=@userId ";

            command.Parameters.AddWithValue("@userId", userId);

            using var reader = await command.ExecuteReaderAsync();
            List<RecipeRecord> recipes = new();
            while (reader.Read())
            {
                var recipeId = reader.GetInt32(0);
                var recipeName = reader.GetString(1);
                var recipe = new RecipeRecord(recipeId, recipeName);
                recipes.Add(recipe);
            }
            return Ok(recipes);
        }
    }
}