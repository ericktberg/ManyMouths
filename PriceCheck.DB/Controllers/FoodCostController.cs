using Microsoft.AspNetCore.Mvc;

using PriceCheck.DB.FoodCenter;

namespace PriceCheck.DB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodCostController : ControllerBase
    {
        public FoodCostController(IServiceProvider serviceProvider)
        {
            FoodCenterConnection = serviceProvider.GetRequiredService<FoodCenterConnection>();
        }

        public FoodCenterConnection FoodCenterConnection { get; }

        [HttpGet(Name = "costFood/{upcCode}")]
        [ProducesResponseType(200, Type = typeof(FoodCenterFoodRecord))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string code)
        {
            var maybeResult = await FoodCenterConnection.SearchByGtin(Gtin.Parse(code));

            return maybeResult.Match<IActionResult>(
                value => Ok(value),
                error => error switch
                {
                    FoodCenterSearchErrors.NonSuccess => new StatusCodeResult(500),
                    FoodCenterSearchErrors.NoMatchingResults => NotFound(),
                }
            );
        }
    }
}