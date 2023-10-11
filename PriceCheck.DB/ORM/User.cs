using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{

    [PrimaryKey(nameof(UserId))]
    [Table("user")]
    public class User
    {
        // Other user properties...
        public ICollection<RecipeOwner> OwnedRecipes { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
    }
}