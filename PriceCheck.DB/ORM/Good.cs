using System.ComponentModel.DataAnnotations.Schema;

using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace PriceCheck.DB.ORM
{
    [Table("good")]
    [Resource]
    public class Good : Identifiable<Guid>
    {
        /// <summary>
        /// How to interpret <see cref="StoreCode"/>
        /// </summary>
        [Attr]
        [Column("code_type")]
        public CodeTypes CodeType { get; set; }

        [Attr(PublicName = "Name")]
        [Column("friendly_name")]
        public string FriendlyName { get; set; } = "";

        /// <summary>
        /// The PlU or UPC code associated with the good
        /// </summary>
        [Attr]
        [Column("store_code")]
        public int StoreCode { get; set; }

        #region Navigation

        [HasMany]
        public ICollection<IngredientMapping> IngredientMappings { get; } = new List<IngredientMapping>();

        #endregion Navigation
    }
}