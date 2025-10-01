using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.DTOs
{
    /// <summary>
    /// Lightweight DTO representing a mapping between an ingredient and a good, including mapping and good details.
    /// </summary>
    public record IngredientMappingDTOLight
    {
        public int MappingId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int GoodId { get; set; }
        public string GoodFriendlyName { get; set; }

        public IngredientMappingDTOLight() { }

        public IngredientMappingDTOLight(IngredientMapping mapping)
        {
            MappingId = mapping.MappingId;
            IngredientId = mapping.IngredientId;
            IngredientName = mapping.Ingredient?.Name ?? string.Empty;
            GoodId = mapping.GoodId;
            GoodFriendlyName = mapping.Good?.FriendlyName ?? string.Empty;
        }
    }
}
