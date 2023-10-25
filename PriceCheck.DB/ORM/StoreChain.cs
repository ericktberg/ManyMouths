using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace PriceCheck.DB.ORM
{
    [Resource]
    [Table("store_chain")]
    public class StoreChain : Identifiable<Guid>
    {
        [Attr]
        [Column("chain_name")]
        public string ChainName { get; set; } = "";

        #region Navigation

        [HasMany]
        public ICollection<StoreLocation> StoreLocations { get; } = new List<StoreLocation>();

        #endregion Navigation
    }
}