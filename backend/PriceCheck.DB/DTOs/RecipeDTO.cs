using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.DTOs
{
    public enum RecipeCostingStatus
    {
        /// <summary>
        /// There is simply not enough information to even infer a total price
        /// </summary>
        NotEnoughInformation,

        InferencesMade,
        FullyCalculated
    }

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

    public record RecipeOverviewDTO
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }

        public int PrepTimeMinutes { get; init; }

        public int CookTimeMinutes { get; init; }

        public int Servings { get; init; }
    }

    /// <summary>
    /// Include costing and ingredient mapping details with a recipe
    /// </summary>
    public record RecipeDetailDTO : RecipeOverviewDTO
    {
        public RecipeDetailDTO() 
        {
        }

        public string MarkdownInstructions { get; init; }

        public ICollection<RecipeIngredientDTO> Ingredients { get; init; }
    }
}