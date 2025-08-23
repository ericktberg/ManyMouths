using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [PrimaryKey(nameof(Id))]
    [Table("recipe")]
    public class Recipe
    {
        [Column("recipe_id")]
        public int Id { get; set; }

        [Column("recipe_name")]
        public string Name { get; set; } = "";

        #region Navigation

        public ICollection<RecipeQuant> IngredientQuantities { get; } = new List<RecipeQuant>();

        public ICollection<RecipeOwner> RecipeOwners { get; } = new List<RecipeOwner>();

        #endregion Navigation
    }
}