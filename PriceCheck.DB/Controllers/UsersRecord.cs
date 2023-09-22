namespace PriceCheck.DB.Controllers
{
    public class UsersRecord
    {
        public UsersRecord(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}