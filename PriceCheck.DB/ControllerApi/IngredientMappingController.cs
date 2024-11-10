using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PriceCheck.DB.ORM;

namespace PriceCheck.DB.Controllers
{
    public record IngredientMappingCreationDTO
    {
        public int IngredientId { get; set; }

        public int GoodId { get; set; }

        public int UserId { get; set; }
    }

    public record GoodDTOLight
    {
        public GoodDTOLight(Good good)
        {
            Id = good.Id;
            FriendlyName = good.FriendlyName;
        }

        public GoodDTOLight()
        {
        }

        public int Id { get; set; }

        public string FriendlyName { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class IngredientMappingController : ControllerBase
    {
        private readonly ManyMouthsContext _context;

        public IngredientMappingController(ManyMouthsContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMapping(IngredientMappingCreationDTO mapping)
        {
            var ingredient = _context.Ingredients.Find(mapping.IngredientId);
            if (ingredient == null)
            {
                return BadRequest("Ingredient not found");
            }

            var good = _context.Goods.Find(mapping.GoodId);
            if (good == null)
            {
                return BadRequest("Good not found");
            }

            var user = _context.Users.Find(mapping.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            /* Insert the mapping if it does not exists */
            var existingMapping = _context.IngredientMappings
                .FirstOrDefault(im => im.IngredientId == mapping.IngredientId && im.GoodId == mapping.GoodId);

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
                _context.SaveChanges();
            }

            /* Select the mapping for our user. Update the current record if it exists,
             * otherwise add a new one.
             */
            var currentSelection = _context.IngredientMappingSelections
                .Find(mapping.UserId, mapping.IngredientId);

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

            return Ok(new GoodDTOLight(mappingEntity.Good));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserMappingForIngredient(int ingredientId)
        {
            int userId = 1;  // Hard-code to 1 for now
            var mapping = await _context.IngredientMappingSelections
                .Include(s => s.Mapping)
                .ThenInclude(m => m.Good)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.IngredientId == ingredientId);

            if (mapping is null)
            {
                return NotFound();
            }

            return Ok(new GoodDTOLight(mapping.Mapping.Good));
        }
    }
}