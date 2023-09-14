namespace PriceCheck.DB.FoodCenter
{
    public record FoodCenterSearchResult
    {
        public int TotalHits { get; init; }

        public List<FoodCenterFoodRecord> Foods { get; init; } = new();
    }
}