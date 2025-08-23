namespace PriceCheck.DB.DTOs
{
    public record IngredientQuantCostDTO
    {
        /// <summary>
        /// Information about the good costing itself
        /// </summary>
        public GoodDTOCost Good { get; set; }

        /// <summary>
        /// The ingredient information including how much is needed for a given recipe
        /// </summary>
        public RecipeIngredientDTO Ingredient { get; set; }

        /// <summary>
        /// Calculated from the ingredient quantity and the good cost.
        /// Transformation from quantity to costing information is provided by Food Data Central
        /// supplied by the National Agricultural Library
        /// </summary>
        public double CostPerServing { get; set; }

    
    }
}