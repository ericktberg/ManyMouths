using PriceCheck.DB.Controllers;
using PriceCheck.DB.ORM;

namespace PriceCheck.DB.DTOs
{
    public record RecipeOverviewDTO
    {
        public RecipeOverviewDTO(Recipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
        }
        public int Id { get; set; }

        public string Name { get; set; }
    }

    /// <summary>
    /// Include costing and ingredient mapping details with a recipe
    /// </summary>
    public record RecipeDetailDTO : RecipeDTO
    {
        public RecipeDetailDTO(Recipe recipe, IDictionary<int, GoodDTOLight?> ingredientMappings) : base(recipe)
        {
            IngredientMappings = ingredientMappings;
        }

        public IDictionary<int, GoodDTOLight?> IngredientMappings { get; }
    }

    public record RecipeDTO : RecipeOverviewDTO
    {
        public RecipeDTO(Recipe recipe) : base(recipe)
        {
            Ingredients = recipe.IngredientQuantities.Select(RecipeIngredientDTO.FromQuant).ToList();
        }

        public List<RecipeIngredientDTO> Ingredients { get; init; } = new();
    }
}