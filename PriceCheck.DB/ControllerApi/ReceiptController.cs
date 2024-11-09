using Microsoft.AspNetCore.Mvc;

using PriceCheck.DB.DTOs;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptController : ControllerBase
    {
        private readonly ManyMouthsContext _context;

        public ReceiptController(ManyMouthsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReceipt([FromBody] ReceiptDto receiptDto)
        {
            if (receiptDto == null || !receiptDto.Goods.Any())
            {
                return BadRequest("Invalid receipt data.");
            }

            // Find or create the store chain
            var storeChain = _context.StoreChains.FirstOrDefault(sc => sc.ChainName == receiptDto.StoreChain);
            if (storeChain == null)
            {
                storeChain = new StoreChain { ChainName = receiptDto.StoreChain };
                _context.StoreChains.Add(storeChain);
                await _context.SaveChangesAsync();
            }

            // Find or create the store location
            var storeLocation = _context.StoreLocations.FirstOrDefault(sl => sl.LocationAddress == receiptDto.StoreAddress && sl.StoreChainId == storeChain.StoreChainId);
            if (storeLocation == null)
            {
                storeLocation = new StoreLocation { LocationAddress = receiptDto.StoreAddress, StoreChainId = storeChain.StoreChainId };
                _context.StoreLocations.Add(storeLocation);
                await _context.SaveChangesAsync();
            }

            foreach (var goodDto in receiptDto.Goods)
            {
                // Find or create the good
                var good = _context.Goods.FirstOrDefault(g => g.FriendlyName == goodDto.GoodName && (goodDto.StoreCode == null || g.StoreCode == goodDto.StoreCode));
                if (good == null)
                {
                    good = new Good
                    {
                        FriendlyName = goodDto.GoodName,
                        CodeType = (CodeTypes)goodDto.CodeType,
                        StoreCode = goodDto.StoreCode
                    };
                    _context.Goods.Add(good);
                    await _context.SaveChangesAsync();
                }

                // Create the good transaction
                var goodTransaction = new GoodTransaction
                {
                    GoodId = good.Id,
                    Price = goodDto.Price,
                    StoreLocationId = storeLocation.StoreLocationId,
                    Unit = goodDto.Unit
                };
                _context.GoodTransactions.Add(goodTransaction);
            }

            await _context.SaveChangesAsync();

            return Ok("Receipt submitted successfully.");
        }
    }
}