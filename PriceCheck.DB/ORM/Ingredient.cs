using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [PrimaryKey(nameof(IngredientId))]
    [Table("ingredient")]
    public class Ingredient
    {
        [Column("ingredient_id")]
        public Guid IngredientId { get; set; }

        [Column("ingredient_name")]
        public string IngredientName { get; set; } = "";

        #region Navigation

        public ICollection<IngredientMapping> Mappings { get; } = new List<IngredientMapping>();

        #endregion Navigation
    }
}