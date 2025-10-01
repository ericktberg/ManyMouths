using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence;
using PriceCheck.DB.Persistence.Entities;
using PriceCheck.DB.Repositories;
using PriceCheck.DB.Services;

namespace Tests.ManyMouths.Integration
{
    [TestFixture]
    public class IngredientMappingIntegrationTests
    {
        private ManyMouthsDbContext _context;
        private IngredientMappingRepository _repository;
        private IngredientMappingService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ManyMouthsDbContext>()
                .UseInMemoryDatabase(databaseName: "IngredientMappingTestDb")
                .Options;
            _context = new ManyMouthsDbContext(options);
            _repository = new IngredientMappingRepository(_context);
            _service = new IngredientMappingService(_repository);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddReadDelete_IngredientMapping_Works()
        {
            // Arrange: Add Ingredient, Good, User
            var ingredient = new Ingredient { Name = "Tomato" };
            var good = new Good { FriendlyName = "Fresh Tomato" };
            var user = new User();
            _context.Ingredients.Add(ingredient);
            _context.Goods.Add(good);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var mappingDto = new IngredientMappingCreationDTO
            {
                IngredientId = ingredient.Id,
                GoodId = good.Id,
                UserId = user.UserId
            };

            // Act: Add mapping
            var created = await _service.CreateMappingAsync(mappingDto);
            Assert.IsNotNull(created);
            Assert.AreEqual(ingredient.Id, created.IngredientId);
            Assert.AreEqual(good.Id, created.GoodId);

            // Act: Read mapping
            var read = await _service.GetUserMappingForIngredientAsync(user.UserId, ingredient.Id);
            Assert.IsNotNull(read);
            Assert.AreEqual(created.MappingId, read.MappingId);

            // Act: Delete mapping
            var mappingEntity = await _context.IngredientMappings.FindAsync(created.MappingId);
            _context.IngredientMappings.Remove(mappingEntity);
            await _context.SaveChangesAsync();

            var deleted = await _context.IngredientMappings.FindAsync(created.MappingId);
            Assert.IsNull(deleted);
        }
    }
}
