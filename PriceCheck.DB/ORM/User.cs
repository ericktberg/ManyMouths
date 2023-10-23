using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Middleware;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Repositories;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using JsonApiDotNetCore.Services;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.ORM
{
    [Resource(PublicName = "user", ClientIdGeneration = ClientIdGenerationMode.Forbidden)]
    [Table("user")]
    public class User : Identifiable<Guid>
    {
        [HasMany]
        public ICollection<RecipeOwner> OwnedRecipes { get; set; }
    }
}