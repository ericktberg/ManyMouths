namespace PriceCheck.DB.ExternalServices.FoodCenter
{
    public record FoodCenterSearchResult
    {
        public int TotalHits { get; init; }

        public List<FoodCenterFoodRecord> Foods { get; init; } = new();
    }
}