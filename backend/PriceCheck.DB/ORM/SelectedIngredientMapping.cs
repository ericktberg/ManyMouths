using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCheck.DB.ORM
{
    /// <summary>
    /// Each user has a unique selection of ingredient mapped to a good.
    /// The mappings can be shared outside of a user's profile, but the user's selection is unique.
    /// </summary>
    [Table("selected_ingredient_mapping")]
    public class SelectedIngredientMapping
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("ingredient_id")]
        public int IngredientId { get; set; }

        [Column("mapping_id")]
        public int MappingId { get; set; }

        #region Navigation

        public Ingredient Ingredient { get; set; }

        public User User { get; set; }

        public IngredientMapping Mapping { get; set; }

        #endregion Navigation
    }
}