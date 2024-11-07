using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.DTOs;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ManyMouthsContext _context;

        public UsersController(ManyMouthsContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}/recipes")]
        public async Task<IActionResult> GetUserRecipes(int userId)
        {
            User? user = await _context.Users
                .Where(u => u.UserId == userId)
                .Include(u => u.OwnedRecipes)
                .ThenInclude(r => r.Recipe)
                .ThenInclude(r => r.IngredientQuantities)
                .ThenInclude(rq => rq.Ingredient)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var recipes = user.OwnedRecipes.Select(r => r.Recipe)
                .Select(RecipeDTO.FromRecipe);

            return Ok(recipes);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser()
        {
            var user = new User();
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }
    }
}