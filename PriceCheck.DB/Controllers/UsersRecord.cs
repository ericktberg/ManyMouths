using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace PriceCheck.DB.Controllers
{
    [PrimaryKey(nameof(UserId))]
    [Table("user")]
    public class UserRecord
    {
        public UserRecord(int userId)
        {
            UserId = userId;
        }

        [Column("user_id")]
        public int UserId { get; set; }
    }
}