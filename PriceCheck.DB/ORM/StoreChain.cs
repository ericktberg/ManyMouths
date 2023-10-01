using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCheck.DB.ORM
{
    [Table("store_chain")]
    public class StoreChain
    {
        [Column("chain_name")]
        public string ChainName { get; set; }

        [Column("store_chain_id")]
        public int StoreChainId { get; set; }

        #region Navigation

        public ICollection<StoreLocation> StoreLocations { get; } = new List<StoreLocation>();

        #endregion Navigation
    }
}