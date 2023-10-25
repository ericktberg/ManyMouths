using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace PriceCheck.DB.ORM
{
    [Resource]
    [Table("store_location")]
    public class StoreLocation : Identifiable<Guid>
    {
        [Column("store_chain_id")]
        public Guid StoreChainId { get; set; }

        [Attr]
        [Column("location_address")]
        public string LocationAddress { get; set; } = "";

        [Attr]
        [Column("location_number")]
        public int LocationNumber { get; set; }

        #region Navigation

        [HasOne]
        public StoreChain Chain { get; set; }

        #endregion
    }
}