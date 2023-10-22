using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [Resource()]
    [Table("user")]
    public class User : Identifiable<Guid>
    {
        // Other user properties...
        public ICollection<RecipeOwner> OwnedRecipes { get; set; }
    }
}