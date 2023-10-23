using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Controllers.Annotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [Resource]
    [Table("recipe")]
    public class Recipe : Identifiable<Guid>
    {
        [Column("recipe_name")]
        public string RecipeName { get; set; } = "";

        [HasMany]
        public ICollection<RecipeOwner> RecipeOwners { get; } = new List<RecipeOwner>();
    }
}