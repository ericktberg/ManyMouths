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
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult Get(int userId)
        {
            var user = Db.Users.Single(user => user.UserId == userId);
            return Ok(user);
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetList()
        {
            var users = await ReadUsers();
            return Ok(users);
        }

        private async Task<IEnumerable<User>> ReadUsers()
        {
            return Db.Users.ToList();
        }
    }
}