using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.Persistence.Entities
{
    [PrimaryKey(nameof(UserId))]
    [Table("user")]
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }

        #region Navigation

        public ICollection<RecipeOwner> OwnedRecipes { get; set; }

        public ICollection<SelectedIngredientMapping> SelectedIngredientMappings { get; set; }

        #endregion Navigation
    }
}