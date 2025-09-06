using PriceCheck.DB.Persistence.Entities;

namespace PriceCheck.DB.DTOs
{
    public record GoodDTOLight
    {
        public GoodDTOLight(Good good)
        {
            Id = good.Id;
            FriendlyName = good.FriendlyName;
        }

        public GoodDTOLight()
        {
        }

        public int Id { get; set; }

        public string FriendlyName { get; set; }
    }


    /// <summary>
    /// Include costing information for a good
    /// </summary>
    public record GoodDTOCost : GoodDTOLight
    {



    }
}