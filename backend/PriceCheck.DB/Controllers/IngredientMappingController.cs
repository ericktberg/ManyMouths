using Microsoft.AspNetCore.Mvc;
using PriceCheck.DB.DTOs;
using PriceCheck.DB.Services;

namespace PriceCheck.DB.Controllers
{
    /// <summary>
    /// API controller for managing ingredient-to-good mappings and user-specific selections.
    /// </summary>
    /// <remarks>
    /// Delegates business logic to <see cref="IIngredientMappingService"/>. All responses are returned as lightweight DTOs.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientMappingController : ControllerBase
    {
        private readonly IIngredientMappingService _ingredientMappingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IngredientMappingController"/> class.
        /// </summary>
        /// <param name="ingredientMappingService">The ingredient mapping service to use for business logic.</param>
        public IngredientMappingController(IIngredientMappingService ingredientMappingService)
        {
            _ingredientMappingService = ingredientMappingService;
        }

        /// <summary>
        /// Creates or updates a mapping between an ingredient and a good for a user.
        /// </summary>
        /// <param name="mapping">The mapping creation DTO containing ingredient, good, and user IDs.</param>
        /// <returns>A lightweight DTO for the mapped ingredient-to-good mapping, or BadRequest if any referenced entity is not found.</returns>
        /// <response code="200">Returns the mapped ingredient-to-good mapping as a DTO.</response>
        /// <response code="400">If the ingredient, good, or user is not found.</response>
        [HttpPost]
        [ProducesResponseType(typeof(IngredientMappingDTOLight), 200)]
        public async Task<IActionResult> CreateMapping(IngredientMappingCreationDTO mapping)
        {
            var result = await _ingredientMappingService.CreateMappingAsync(mapping);
            if (result == null)
                return BadRequest("Ingredient, Good, or User not found");
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the mapped good for a given user and ingredient.
        /// </summary>
        /// <param name="ingredientId">The ingredient ID to look up.</param>
        /// <returns>A lightweight DTO for the mapped ingredient-to-good mapping, or NotFound if no mapping exists.</returns>
        /// <response code="200">Returns the mapped ingredient-to-good mapping as a DTO.</response>
        /// <response code="404">If no mapping exists for the user and ingredient.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IngredientMappingDTOLight), 200)]
        public async Task<IActionResult> GetUserMappingForIngredient(int ingredientId)
        {
            int userId = 1; // TODO: Replace with actual user context
            var result = await _ingredientMappingService.GetUserMappingForIngredientAsync(userId, ingredientId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}