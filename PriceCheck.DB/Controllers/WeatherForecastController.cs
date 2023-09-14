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
        [ProducesResponseType(200, Type=typeof(FoodCenterFoodRecord))]
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

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast

            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}