using Microsoft.AspNetCore.Mvc;

namespace PriceCheck.DB.Controllers
{
    public record RecipeRecord
    {
        public RecipeRecord(int recipeId, string recipeName)
        {
            RecipeId = recipeId;
            RecipeName = recipeName;
        }

        public int RecipeId { get; }

        public string RecipeName { get; }
    }

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(ManyMouthsContext db)
        {
            Db = db;
        }

        public ManyMouthsContext Db { get; }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserRecord))]
        public IActionResult Get(int id)
        {
            var user = Db.Users.Single(user => user.UserId == id);
            return Ok(user);
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserRecord>))]
        public async Task<IActionResult> GetList()
        {
            var users = await ReadUsers();
            return Ok(users);
        }

        private async Task<IEnumerable<UserRecord>> ReadUsers()
        {
            return Db.Users.ToList();
        }
    }
}