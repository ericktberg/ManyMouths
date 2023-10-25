using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [Resource]
    [Table("ingredient_mapping")]
    public class IngredientMapping : Identifiable<int>
    {
        [Column("good_id")]
        public Guid GoodId { get; set; }

        [Column("ingredient_id")]
        public Guid IngredientId { get; set; }

        #region Navigation

        [HasOne]
        public Good Good { get; set; }

        [HasOne]
        public Ingredient Ingredient { get; set; }

        #endregion Navigation
    }
}