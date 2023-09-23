using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.Controllers
{

    [PrimaryKey(nameof(UserId))]
    [Table("user")]
    public class User
    {
        // Other user properties...
        public ICollection<RecipeOwner> RecipeOwners { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
    }
}