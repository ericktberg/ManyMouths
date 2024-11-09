namespace PriceCheck.DB.DTOs
{
    public class GoodTransactionDto
    {
        public string GoodName { get; set; }
        public int CodeType { get; set; }
        public int? StoreCode { get; set; }
        public int Price { get; set; }
        public string Unit { get; set; }
    }
}