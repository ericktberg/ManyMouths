using PriceCheck.DB.ORM;

namespace PriceCheck.DB.DTOs
{
    public record UserDTO
    {
        public int UserId { get; init; }

        public List<int> RecipeIds { get; init; } = new();

        public static UserDTO FromUserOrm(User user)
        {
            return new UserDTO()
            {
                UserId = user.UserId,
                RecipeIds = user.OwnedRecipes.Select(ro => ro.RecipeId).ToList()
            };
        }
    }
}