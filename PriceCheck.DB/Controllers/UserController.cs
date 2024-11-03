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