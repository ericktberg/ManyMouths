using PriceCheck.DB.ORM;

namespace PriceCheck.DB.DTOs
{
    public record RecipeDTO
    {
        public int Id { get; init; }

        public string Name { get; init; } = "";

        public List<RecipeIngredientDTO> Ingredients { get; init; } = new();

        public static RecipeDTO FromRecipe(Recipe recipe)
        {
            var ingredients = recipe.IngredientQuantities.Select(RecipeIngredientDTO.FromQuant).ToList();

            return new RecipeDTO()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Ingredients = ingredients
            };
        }
    }
}