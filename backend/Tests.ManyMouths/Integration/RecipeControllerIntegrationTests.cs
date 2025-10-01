using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PriceCheck.DB.Controllers;
using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence;
using PriceCheck.DB.Persistence.Entities;
using PriceCheck.DB.Repositories;
using PriceCheck.DB.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.ManyMouths.Integration
{
    [TestFixture]
    public class RecipeControllerIntegrationTests
    {
        private ManyMouthsDbContext _context;
        private RecipesController _controller;
        private RecipeService _service;
        private RecipeRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ManyMouthsDbContext>()
                .UseInMemoryDatabase(databaseName: "RecipeControllerTestDb")
                .Options;
            _context = new ManyMouthsDbContext(options);
            _repo = new RecipeRepository(_context);
            _service = new RecipeService(_context, _repo);
            _controller = new RecipesController(_service);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddReadDelete_Recipe_Works()
        {
            // Arrange: Add Recipe
            var recipeDto = new RecipeCreationDto
            {
                Name = "Test Recipe",
                Description = "Test Desc",
                CookTimeMinutes = 10,
                PrepTimeMinutes = 5,
                Servings = 2,
                InstructionMarkdownText = "Step 1...",
                Ingredients = new List<RecipeIngredientDTO> {
                    new RecipeIngredientDTO { Name = "Egg", Quantity = 2, Unit = "pcs" }
                }
            };

            // Act: Add
            var addResult = await _controller.CreateRecipe(recipeDto);
            // Accept both CreatedAtActionResult and ObjectResult for flexibility
            Assert.IsTrue(addResult is CreatedAtActionResult || addResult is ObjectResult, $"Expected CreatedAtActionResult or ObjectResult but got {addResult?.GetType().Name}");
            var createdRecipe = (addResult as CreatedAtActionResult)?.Value as RecipeDetailDTO
                ?? (addResult as ObjectResult)?.Value as RecipeDetailDTO;
            Assert.IsNotNull(createdRecipe);
            Assert.AreEqual("Test Recipe", createdRecipe.Name);

            // Act: Read
            var getResult = await _controller.GetRecipeDetails(createdRecipe.Id);
            Assert.IsInstanceOf<OkObjectResult>(getResult);
            var readRecipe = (getResult as OkObjectResult)?.Value as RecipeDetailDTO;
            Assert.IsNotNull(readRecipe);
            Assert.AreEqual(createdRecipe.Id, readRecipe.Id);

            // Act: Delete
            var delResult = await _controller.DeleteRecipe(createdRecipe.Id);
            Assert.IsInstanceOf<NoContentResult>(delResult);
            var getAfterDelete = await _controller.GetRecipeDetails(createdRecipe.Id);
            Assert.IsInstanceOf<NotFoundResult>(getAfterDelete);
        }
    }
}
