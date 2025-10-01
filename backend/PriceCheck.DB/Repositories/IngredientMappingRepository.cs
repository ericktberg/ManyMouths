using Microsoft.EntityFrameworkCore;
using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence;
using PriceCheck.DB.Persistence.Entities;
using System.Threading.Tasks;

namespace PriceCheck.DB.Repositories
{
    /// <summary>
    /// Defines the contract for a repository that manages ingredient-to-good mappings and user selections.
    /// </summary>
    public interface IIngredientMappingRepository
    {
        /// <summary>
        /// Creates a new mapping between an ingredient and a good, or retrieves the existing one. Also updates the user's selection.
        /// </summary>
        /// <param name="mappingDto">The mapping creation DTO containing ingredient, good, and user IDs.</param>
        /// <returns>The mapped <see cref="IngredientMapping"/>, or null if any referenced entity is not found.</returns>
        Task<IngredientMapping?> CreateOrGetMappingAsync(IngredientMappingCreationDTO mappingDto);

        /// <summary>
        /// Retrieves the <see cref="IngredientMapping"/> mapped to an ingredient for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="ingredientId">The ingredient ID.</param>
        /// <returns>The mapped <see cref="IngredientMapping"/>, or null if not found.</returns>
        Task<IngredientMapping?> GetUserMappingForIngredientAsync(int userId, int ingredientId);
    }

    /// <summary>
    /// Repository for managing ingredient-to-good mappings and user-specific selections using Entity Framework Core.
    /// </summary>
    /// <remarks>
    /// Handles both the creation of mappings and the association of a user's selection for a given ingredient.
    /// Ensures that mappings are not duplicated and that user selections are updated or created as needed.
    /// </remarks>
    public class IngredientMappingRepository : IIngredientMappingRepository
    {
        private readonly ManyMouthsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="IngredientMappingRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to use for data access.</param>
        public IngredientMappingRepository(ManyMouthsDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// If the mapping does not exist, it is created. The user's selection is updated to point to the mapping.
        /// If any referenced entity (ingredient, good, user) does not exist, returns null.
        /// </remarks>
        public async Task<IngredientMapping?> CreateOrGetMappingAsync(IngredientMappingCreationDTO mapping)
        {
            var ingredient = await _context.Ingredients.FindAsync(mapping.IngredientId);
            if (ingredient == null) return null;

            var good = await _context.Goods.FindAsync(mapping.GoodId);
            if (good == null) return null;

            var user = await _context.Users.FindAsync(mapping.UserId);
            if (user == null) return null;

            var existingMapping = await _context.IngredientMappings
                .Include(im => im.Good)
                .Include(im => im.Ingredient)
                .FirstOrDefaultAsync(im => im.IngredientId == mapping.IngredientId && im.GoodId == mapping.GoodId);

            IngredientMapping mappingEntity;
            if (existingMapping != null)
            {
                mappingEntity = existingMapping;
            }
            else
            {
                mappingEntity = new IngredientMapping()
                {
                    Ingredient = ingredient,
                    Good = good
                };
                _context.IngredientMappings.Add(mappingEntity);
                await _context.SaveChangesAsync();
            }

            // The same user may have selected mappings for an ingredient multiple times to different goods
            // This will update their selection to the new mapping without overriding their previous selections
            var currentSelection = await _context.IngredientMappingSelections
                .FindAsync(mapping.UserId, mapping.IngredientId);

            if (currentSelection != null)
            {
                currentSelection.Mapping = mappingEntity;
            }
            else
            {
                currentSelection = new SelectedIngredientMapping()
                {
                    Ingredient = ingredient,
                    User = user,
                    Mapping = mappingEntity
                };
                _context.IngredientMappingSelections.Add(currentSelection);
            }

            await _context.SaveChangesAsync();
            return mappingEntity;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Uses eager loading to include the mapped <see cref="Good"/> and <see cref="Ingredient"/> entities.
        /// Returns null if no mapping exists for the user and ingredient.
        /// </remarks>
        public async Task<IngredientMapping?> GetUserMappingForIngredientAsync(int userId, int ingredientId)
        {
            var mappingSelection = await _context.IngredientMappingSelections
                .Include(s => s.Mapping)
                    .ThenInclude(m => m.Good)
                .Include(s => s.Mapping)
                    .ThenInclude(m => m.Ingredient)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.IngredientId == ingredientId);

            return mappingSelection?.Mapping;
        }
    }
}
