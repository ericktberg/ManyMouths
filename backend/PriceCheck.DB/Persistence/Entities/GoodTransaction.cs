using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCheck.DB.Persistence.Entities
{
    /// <summary>
    /// Represents a transaction to purchase a good at a certain store location
    /// </summary>
    [Table("good_transaction")]
    public class GoodTransaction
    {
        [Column("good_id")]
        public int GoodId { get; set; }

        [Column("price")]
        public int Price { get; set; }

        [Column("store_location_id")]
        public int StoreLocationId { get; set; }

        [Column("good_transaction_id")]
        public int Id { get; set; }

        [Column("unit")]
        public string Unit { get; set; }

        #region Navigation

        public Good Good { get; set; }

        public StoreLocation StoreLocation { get; set; }

        #endregion Navigation
    }
}