namespace PriceCheck.DB.FoodCenter
{
    public record FoodCenterFoodRecord
    {
        /// <summary>
        /// The primary key for most food records is the FDC ID.
        /// </summary>
        public int FdcId { get; init; }

        public string GtinUpc { get; init; } = new Gtin().Gtin12();

        /// <summary>
        /// e.g. "Romaine"
        /// </summary>
        public string Description { get; init; } = "";

        /// <summary>
        /// e.g. "Branded"
        /// </summary>
        public string DataType { get; init; } = "";

        public string BrandOwner { get; init; } = "";

        public string PackageWeight { get; init; } = "";

        public string HouseholdServingFullText { get; init; } = "";

        public double ServingSize { get; init; } = 0;

        public string ServingSizeUnit { get; init; } = "";
    }
}