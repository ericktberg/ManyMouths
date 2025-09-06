using Microsoft.AspNetCore.Mvc;

using PriceCheck.DB.Persistence;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodController : ControllerBase
    {
        private readonly ManyMouthsDbContext _context;

        public GoodController(ManyMouthsDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public IActionResult SearchGoods(string query)
        {
            var goods = _context.Goods
                .Where(g => string.IsNullOrEmpty(query) || g.FriendlyName.Contains(query))
                .OrderBy(g => g.FriendlyName)
                .ToList();

            return Ok(goods);
        }
    }
}