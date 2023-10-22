using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [PrimaryKey(nameof(IngredientId))]
    [Resource]
    [Table("ingredient")]
    public class Ingredient : Identifiable<Guid>
    {
        [Column("ingredient_id")]
        public Guid IngredientId { get; set; }

        [Column("ingredient_name")]
        public string IngredientName { get; set; } = "";

        #region Navigation

        public ICollection<IngredientMapping> Mappings { get; } = new List<IngredientMapping>();

        #endregion Navigation
    }
}