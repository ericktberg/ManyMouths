using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCheck.DB.ORM
{
    [Table("recipe_owner")]
    public class RecipeOwner
    {
        [Column("recipe_id")]
        public Guid RecipeId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        #region Navigation

        public Recipe Recipe { get; set; }

        public User User { get; set; }

        #endregion Navigation
    }
}