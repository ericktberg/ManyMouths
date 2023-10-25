using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [Resource]
    [Table("ingredient")]
    public class Ingredient : Identifiable<Guid>
    {
        [Attr]
        [Column("ingredient_name")]
        public string IngredientName { get; set; } = "";

        #region Navigation

        [HasMany]
        public ICollection<IngredientMapping> Mappings { get; } = new List<IngredientMapping>();

        #endregion Navigation
    }
}