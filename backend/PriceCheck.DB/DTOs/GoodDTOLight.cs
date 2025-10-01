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

    public class CreateGoodDTO
    {
        public string FriendlyName { get; set; }
        public string? StoreCode { get; set; }
    }

    public class AddPriceDTO
    {
        public static readonly HashSet<string> AllowedUnits = new() { "each", "lb", "pound", "oz", "kg", "g", "dozen", "pack", "liter", "l", "ml", "quart", "qt", "gallon", "gal" };

        public decimal Price { get; set; }
        private string _unit;
        public string Unit
        {
            get => _unit;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Unit cannot be empty.");
                var normalized = value.Trim().ToLowerInvariant();
                if (!AllowedUnits.Contains(normalized))
                    throw new ArgumentException($"Unit '{value}' is not allowed. Allowed: {string.Join(", ", AllowedUnits)}");
                _unit = normalized;
            }
        }
        public int? StoreLocationId { get; set; }
    }

    public class GoodTransactionDTO
    {
        public int Id { get; set; }
        
        public decimal Price { get; set; }

        public string Unit { get; set; }

        public GoodTransactionDTO(GoodTransaction transaction)
        {
            Id = transaction.Id;
            Price = transaction.Price;
            Unit = transaction.Unit;
        }
    }
}