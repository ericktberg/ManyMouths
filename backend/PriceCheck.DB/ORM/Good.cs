using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCheck.DB.ORM
{
    /// <summary>
    /// Represents a purchasable good at a store.
    /// Does not directly represent an ingredient, but is used to map ingredients to something that can be purchased.
    /// </summary>
    [Table("good")]
    public class Good
    {
        /// <summary>
        /// How to interpret <see cref="StoreCode"/>
        /// </summary>
        [Column("code_type")]
        public CodeTypes CodeType { get; set; }

        [Column("friendly_name")]
        public string FriendlyName { get; set; }

        [Column("good_id")]
        public int Id { get; set; }

        /// <summary>
        /// The PlU or UPC code associated with the good
        /// </summary>
        [Column("store_code")]
        public int? StoreCode { get; set; }

        #region Navigation

        public ICollection<GoodTransaction> GoodTransactions { get; } = new List<GoodTransaction>();

        public ICollection<IngredientMapping> IngredientMappings { get; } = new List<IngredientMapping>();

        #endregion Navigation
    }
}