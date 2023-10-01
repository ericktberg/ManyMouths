using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB.Controllers
{
    public record UserDTO
    {
        public int UserId { get; init; }

        public List<int> RecipeIds { get; init; } = new();

        public static UserDTO FromUserOrm(User user)
        {
            return new UserDTO()
            {
                UserId = user.UserId,
                RecipeIds = user.OwnedRecipes.Select(ro => ro.RecipeId).ToList()
            };
        }
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
            var user = ReadUsers()
                .Where(user => user.UserId == userId)
                .Select(UserDTO.FromUserOrm)
                .SingleOrDefault();

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetList()
        {
            var users = ReadUsers().Select(UserDTO.FromUserOrm).ToList();
            return Ok(users);
        }

        private IEnumerable<User> ReadUsers()
        {
            return Db.Users.Include(u => u.OwnedRecipes);
        }
    }
}