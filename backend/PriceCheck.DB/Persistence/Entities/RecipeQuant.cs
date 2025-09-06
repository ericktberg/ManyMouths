using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.Persistence.Entities
{
    public enum Units
    {
        Each = 0,
        Pounds = 1,
        FluidOunces = 2,
        Slices,
        Cups,
        Tablespoons,
        Teaspoons,
        Ounces,
        Grams,
        Milliliters,
        Liters,
        Kilograms,
        Gallons,
        Quarts,
        Pints,
        Cloves,
    }

    [PrimaryKey(nameof(RecipeId), nameof(IngredientId))]
    [Table("recipe_quant")]
    public class RecipeQuant
    {
        [Column("ingredient_id")]
        public int IngredientId { get; set; }

        /// <summary>
        /// In 100th counts e.g. 125 = 1.25
        /// </summary>
        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("recipe_id")]
        public int RecipeId { get; set; }

        [Column("unit")]
        public string Unit { get; set; }

        #region Navigations

        public Ingredient Ingredient { get; set; }

        public Recipe Recipe { get; set; }

        #endregion Navigations
    }
}