using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PriceCheck.DB.Persistence;
using PriceCheck.DB.Persistence.Entities;
using PriceCheck.DB.Repositories;
using PriceCheck.DB.Services;

namespace Tests.ManyMouths.Unit
{
    [TestFixture]
    public class IngredientMappingRepositoryUnitTests
    {
        [Test]
        public async Task CreateOrGetMappingAsync_ReturnsNull_WhenIngredientMissing()
        {
            var options = new DbContextOptionsBuilder<ManyMouthsDbContext>()
                .UseInMemoryDatabase(databaseName: "UnitTestDb1")
                .Options;
            using var context = new ManyMouthsDbContext(options);
            var repo = new IngredientMappingRepository(context);
            var result = await repo.CreateOrGetMappingAsync(new PriceCheck.DB.DTOs.IngredientMappingCreationDTO { IngredientId = 999, GoodId = 1, UserId = 1 });
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateOrGetMappingAsync_ReturnsNull_WhenGoodMissing()
        {
            var options = new DbContextOptionsBuilder<ManyMouthsDbContext>()
                .UseInMemoryDatabase(databaseName: "UnitTestDb2")
                .Options;
            using var context = new ManyMouthsDbContext(options);
            context.Ingredients.Add(new Ingredient { Name = "Salt" });
            await context.SaveChangesAsync();
            var repo = new IngredientMappingRepository(context);
            var result = await repo.CreateOrGetMappingAsync(new PriceCheck.DB.DTOs.IngredientMappingCreationDTO { IngredientId = 1, GoodId = 999, UserId = 1 });
            Assert.IsNull(result);
        }
    }
}
