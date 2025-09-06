using PriceCheck.DB.ORM;

namespace PriceCheck.DB.DTOs
{

    public class RecipeCreationDto
    {
        public int UserId { get; init; }

        public string Name { get; init; } = "";

        public string Description { get; init; } = "";

        public string InstructionMarkdownText { get; init; } = "";

        public int PrepTimeMinutes { get; init; }

        public int CookTimeMinutes { get; init; }

        public int Servings { get; init; }

        public List<RecipeIngredientDTO> Ingredients { get; init; }
    }

    public enum RecipeCostingStatus
    {
        /// <summary>
        /// There is simply not enough information to even infer a total price
        /// </summary>
        NotEnoughInformation,
        InferencesMade,
        FullyCalculated
    }

    public record RecipeCostingDTO
    {
        public RecipeCostingDTO()
        {

        }

        public double SmartCostTotal { get; }

        public double GroceryCostTotal { get; }
    }


    public record RecipeOverviewDTO
    {
        public RecipeOverviewDTO(Recipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Description = recipe.Description;
            PrepTimeMinutes = recipe.PrepTimeMinutes;
            CookTimeMinutes = recipe.CookTimeMinutes;
            Servings = recipe.Servings;
        }

        public int Id { get; }

        public string Name { get; }

        public string Description { get; }

        public int PrepTimeMinutes { get; }

        public int CookTimeMinutes { get; }

        public int Servings { get; }
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