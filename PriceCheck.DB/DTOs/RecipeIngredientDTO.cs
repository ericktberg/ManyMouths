using PriceCheck.DB.ORM;

namespace PriceCheck.DB.DTOs
{
    public record RecipeIngredientDTO
    {
        public string Name { get; init; } = "";

        public double Quantity { get; init; }

        public string Unit { get; init; }

        public int Id { get; init; }

        public static RecipeIngredientDTO FromQuant(RecipeQuant quant)
        {
            return new RecipeIngredientDTO()
            {
                Name = quant.Ingredient.Name,
                Quantity = quant.Quantity * .01,
                Unit = quant.Unit,
                Id = quant.IngredientId
            };
        }
    }
}