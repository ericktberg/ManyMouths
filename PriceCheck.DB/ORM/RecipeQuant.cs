using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [Resource(ClientIdGeneration = ClientIdGenerationMode.Forbidden)]
    [PrimaryKey(nameof(RecipeId), nameof(IngredientId))]
    [Table("recipe_quant")]
    public class RecipeQuant : Identifiable<int>
    {
        [Column("ingredient_id")]
        public Guid IngredientId { get; set; }

        /// <summary>
        /// In 100th counts e.g. 125 = 1.25
        /// </summary>
        [Attr]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("recipe_id")]
        public Guid RecipeId { get; set; }

        [Attr]
        [Column("unit")]
        public Units Unit { get; set; }

        #region Navigations

        [HasOne]
        public Ingredient Ingredient { get; set; }

        [HasOne]
        public Recipe Recipe { get; set; }

        #endregion Navigations
    }
}