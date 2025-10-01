using Microsoft.AspNetCore.Mvc;
using PriceCheck.DB.DTOs;
using PriceCheck.DB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodController : ControllerBase
    {
        private readonly IGoodService _goodService;

        public GoodController(IGoodService goodService)
        {
            _goodService = goodService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchGoods(string query)
        {
            var goods = await _goodService.SearchGoodsAsync(query);
            return Ok(goods);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GoodDTOLight), 200)]
        public async Task<IActionResult> CreateGood(CreateGoodDTO goodDto)
        {
            var good = await _goodService.CreateGoodAsync(goodDto);
            return Ok(good);
        }

        [HttpGet("{goodId}/latest-price")]
        [ProducesResponseType(typeof(GoodTransactionDTO), 200)]
        public async Task<IActionResult> GetLatestPrice(int goodId)
        {
            var latestTransaction = await _goodService.GetLatestPriceAsync(goodId);
            if (latestTransaction == null)
                return NotFound();
            return Ok(latestTransaction);
        }

        [HttpPost("{goodId}/price")]
        [ProducesResponseType(typeof(GoodTransactionDTO), 200)]
        public async Task<IActionResult> AddPrice(int goodId, AddPriceDTO priceDto)
        {
            var transaction = await _goodService.AddPriceAsync(goodId, priceDto);
            if (transaction == null)
                return BadRequest("Good not found");
            return Ok(transaction);
        }

        [HttpGet("allowed-units")]
        public IActionResult GetAllowedUnits()
        {
            return Ok(AddPriceDTO.AllowedUnits);
        }
    }
}