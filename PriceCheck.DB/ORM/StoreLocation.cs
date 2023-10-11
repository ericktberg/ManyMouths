using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCheck.DB.ORM
{
    [Table("store_location")]
    public class StoreLocation
    {
        public Guid StoreLocationId { get; set; }

        public Guid StoreChainId { get; set; }

        public string LocationAddress { get; set; } = "";

        public int LocationNumber { get; set; }

        #region Navigation

        public StoreChain Chain { get; set; }

        #endregion
    }
}