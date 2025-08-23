using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCheck.DB.ORM
{
    [Table("store_location")]
    public class StoreLocation
    {
        [Column("store_location_id")]
        public int StoreLocationId { get; set; }

        [Column("store_chain_id")]
        public int StoreChainId { get; set; }

        [Column("location_address")]
        public string LocationAddress { get; set; } = "";

        [Column("location_number")]
        public int LocationNumber { get; set; }

        #region Navigation

        public StoreChain Chain { get; set; }

        public ICollection<GoodTransaction> GoodTransactions { get; } = new List<GoodTransaction>();

        #endregion
    }
}