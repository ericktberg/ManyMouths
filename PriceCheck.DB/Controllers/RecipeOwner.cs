using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCheck.DB.Controllers
{
    [Table("recipe_owner")]
    public class RecipeOwner
    {
        public Recipe Recipe { get; set; }
        [Column("recipe_id")]
        public int RecipeId { get; set; }
        public User User { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
    }
}