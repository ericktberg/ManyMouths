namespace PriceCheck.DB.DTOs
{
    public record IngredientMappingCreationDTO
    {
        public int IngredientId { get; set; }

        public int GoodId { get; set; }

        public int UserId { get; set; }
    }
}