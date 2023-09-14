using Optional;

namespace PriceCheck.DB.FoodCenter
{

    public enum FoodCenterSearchErrors
    {
        NonSuccess,
        NoMatchingResults
    }

    public class FoodCenterConnection
    {
        public FoodCenterConnection(HttpClient httpClient, SecretsFile secrets)
        {
            HttpClient = httpClient;
            Secrets = secrets;
        }

        public HttpClient HttpClient { get; }

        public SecretsFile Secrets { get; }

        /// <summary>
        /// Search the Food Center database for a food with the given GTIN / UPC code
        /// </summary>
        /// <param name="gtinValue"></param>
        public async Task<Option<FoodCenterFoodRecord, FoodCenterSearchErrors>> SearchByGtin(Gtin gtinValue)
        {
            string uri = $"https://api.nal.usda.gov/fdc/v1/foods/search?query={gtinValue.Gtin12()}";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("X-Api-Key", Secrets.DataDotGovApiKey());

            var response = HttpClient.Send(request);

            if (!response.IsSuccessStatusCode)
            {
                return Option.None<FoodCenterFoodRecord, FoodCenterSearchErrors>(FoodCenterSearchErrors.NonSuccess);
            }

            var result = await response.Content.ReadFromJsonAsync<FoodCenterSearchResult>();
            if (result is null || result.TotalHits == 0)
            {
                return Option.None<FoodCenterFoodRecord, FoodCenterSearchErrors>(FoodCenterSearchErrors.NoMatchingResults);
            }

            return result.Foods[0].Some<FoodCenterFoodRecord, FoodCenterSearchErrors>();
        }
    }
}