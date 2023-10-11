using PriceCheck.DB.ORM;

namespace PriceCheck.DB.DTOs
{
    public record RecipeIngredientDTO
    {
        public string IngredientName { get; init; } = "";

        // public Guid IngredientId { get; init; }

        public double Quantity { get; init; }

        public Units Unit { get; init; }

        public static RecipeIngredientDTO FromQuant(RecipeQuant quant)
        {
            return new RecipeIngredientDTO()
            {
                IngredientName = quant.Ingredient.IngredientName,
                // IngredientId = quant.Ingredient.IngredientId,
                Quantity = quant.Quantity * .01,
                Unit = quant.Unit
            };
        }
    }
}