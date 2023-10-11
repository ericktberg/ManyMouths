using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [PrimaryKey(nameof(MappingId))]
    [Table("ingredient_mapping")]
    public class IngredientMapping
    {
        [Column("good_id")]
        public Guid GoodId { get; set; }

        [Column("ingredient_id")]
        public Guid IngredientId { get; set; }

        [Column("mapping_id")]
        public int MappingId { get; set; }

        #region Navigation

        public Good Good { get; set; }

        public Ingredient Ingredient { get; set; }

        #endregion Navigation
    }
}