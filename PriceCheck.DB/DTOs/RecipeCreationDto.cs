namespace PriceCheck.DB.DTOs
{
    public class RecipeCreationDto
    {
        public string Name { get; set; }

        public List<RecipeIngredientDTO> Ingredients { get; set; }
    }
}