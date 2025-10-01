using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence.Entities;
using PriceCheck.DB.Repositories;
using System.Threading.Tasks;

namespace PriceCheck.DB.Services
{
    /// <summary>
    /// Defines the contract for a service that manages ingredient-to-good mappings and user selections.
    /// </summary>
    public interface IIngredientMappingService
    {
        /// <summary>
        /// Creates or updates a mapping between an ingredient and a good for a user, returning a lightweight DTO.
        /// </summary>
        /// <param name="mappingDto">The mapping creation DTO containing ingredient, good, and user IDs.</param>
        /// <returns>A <see cref="GoodDTOLight"/> if successful, or null if any referenced entity is not found.</returns>
        Task<GoodDTOLight?> CreateMappingAsync(IngredientMappingCreationDTO mappingDto);

        /// <summary>
        /// Retrieves the mapped good for a given user and ingredient as a lightweight DTO.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="ingredientId">The ingredient ID.</param>
        /// <returns>A <see cref="GoodDTOLight"/> if found, or null otherwise.</returns>
        Task<GoodDTOLight?> GetUserMappingForIngredientAsync(int userId, int ingredientId);
    }

    /// <summary>
    /// Service for managing ingredient-to-good mappings and user-specific selections.
    /// </summary>
    /// <remarks>
    /// Encapsulates business logic for mapping creation and retrieval, delegating data access to the repository.
    /// Converts entities to DTOs for API consumption.
    /// </remarks>
    public class IngredientMappingService : IIngredientMappingService
    {
        private readonly IIngredientMappingRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="IngredientMappingService"/> class.
        /// </summary>
        /// <param name="repository">The ingredient mapping repository to use for data access.</param>
        public IngredientMappingService(IIngredientMappingRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Converts the mapped <see cref="Good"/> entity to a <see cref="GoodDTOLight"/> for API response.
        /// </remarks>
        public async Task<GoodDTOLight?> CreateMappingAsync(IngredientMappingCreationDTO mappingDto)
        {
            var good = await _repository.CreateOrGetMappingAsync(mappingDto);
            return good != null ? new GoodDTOLight(good) : null;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Converts the mapped <see cref="Good"/> entity to a <see cref="GoodDTOLight"/> for API response.
        /// </remarks>
        public async Task<GoodDTOLight?> GetUserMappingForIngredientAsync(int userId, int ingredientId)
        {
            var good = await _repository.GetUserMappingForIngredientAsync(userId, ingredientId);
            return good != null ? new GoodDTOLight(good) : null;
        }
    }
}
