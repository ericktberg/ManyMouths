using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [PrimaryKey(nameof(Id))]
    [Table("ingredient")]
    public class Ingredient
    {
        [Column("ingredient_id")]
        public int Id { get; set; }

        [Column("ingredient_name")]
        public string Name { get; set; } = "";

        #region Navigation

        public ICollection<IngredientMapping> Mappings { get; } = new List<IngredientMapping>();

        #endregion Navigation
    }
}