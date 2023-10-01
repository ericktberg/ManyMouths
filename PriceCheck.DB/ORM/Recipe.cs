using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [PrimaryKey(nameof(RecipeId))]
    [Table("recipe")]
    public class Recipe
    {
        [Column("recipe_id")]
        public int RecipeId { get; set; }

        [Column("recipe_name")]
        public string RecipeName { get; set; }

        // Other recipe properties...
        public ICollection<RecipeOwner> RecipeOwners { get; set; }
    }
}