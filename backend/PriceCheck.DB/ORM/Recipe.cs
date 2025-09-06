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

        [Column("recipe_markdown_instructions")]
        public string MarkdownInstructions { get; set; } = "";

        [Column("recipe_description")]
        public string Description { get; set; }

        [Column("recipe_prep_minutes")]
        public int PrepTimeMinutes { get; set; }

        [Column("recipe_cook_minutes")]
        public int CookTimeMinutes { get; set; }

        [Column("recipe_servings_count")]
        public int Servings { get; set; }

        #region Navigation

        public ICollection<RecipeQuant> IngredientQuantities { get; } = new List<RecipeQuant>();

        public ICollection<RecipeOwner> RecipeOwners { get; } = new List<RecipeOwner>();

        #endregion Navigation
    }
}