using Microsoft.AspNetCore.Mvc;

namespace PriceCheck.DB.Controllers
{

    [ApiController]
    [Route("users/{userId}/recipes")]
    public class RecipeController : ControllerBase
    {
        public RecipeController(ManyMouthsContext db)
        {
            Db = db;
        }

        public ManyMouthsContext Db { get; }

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Recipe>))]
        public async Task<IActionResult> GetList(int userId)
        {
            return Ok(Db.RecipeOwners
                .Where(ro => ro.UserId == userId)
                .Select(ro => ro.Recipe)
                .ToList());
        }
    }
}