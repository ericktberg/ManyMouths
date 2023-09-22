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
        public UserController(ManyMouthsDb db)
        {
            Db = db;
        }

        public ManyMouthsDb Db { get; }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UsersRecord))]
        public async Task<IActionResult> Get(int id)
        {
            var connection = Db.OpenConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT user_id FROM many_mouths.user WHERE user_id=@userId";
            command.Parameters.AddWithValue("@userId", id);

            using var reader = await command.ExecuteReaderAsync();
            List<UsersRecord> users = new();
            while (reader.Read())
            {
                var userId = reader.GetInt32(0);
                var user = new UsersRecord(userId);
                users.Add(user);
            }
            return Ok(users.FirstOrDefault());
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UsersRecord>))]
        public async Task<IActionResult> GetList()
        {
            var users = await ReadUsers();
            return Ok(users);
        }

        private async Task<IEnumerable<UsersRecord>> ReadUsers()
        {
            var connection = Db.OpenConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT user_id FROM many_mouths.user";
            using var reader = await command.ExecuteReaderAsync();
            List<UsersRecord> users = new();
            while (reader.Read())
            {
                var userId = reader.GetInt32(0);
                var user = new UsersRecord(userId);
                users.Add(user);
            }
            return users;
        }
    }
}