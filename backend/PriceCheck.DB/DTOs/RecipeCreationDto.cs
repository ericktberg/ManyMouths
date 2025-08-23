namespace PriceCheck.DB.DTOs
{
    public class RecipeCreationDto
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public List<RecipeIngredientDTO> Ingredients { get; set; }
    }
}