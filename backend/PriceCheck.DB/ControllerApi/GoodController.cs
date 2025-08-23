using Microsoft.AspNetCore.Mvc;

using PriceCheck.DB.ORM;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodController : ControllerBase
    {
        private readonly ManyMouthsContext _context;

        public GoodController(ManyMouthsContext context)
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