using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace PriceCheck.DB.ORM
{
    [Resource]
    [Table("recipe_owner")]
    public class RecipeOwner : Identifiable<int>
    {
        [Column("recipe_id")]
        public Guid RecipeId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        #region Navigation

        [HasOne]
        public Recipe Recipe { get; set; }

        [HasOne]
        public User User { get; set; }

        #endregion Navigation
    }
}