namespace PriceCheck.DB.DTOs
{
    public class ReceiptDto
    {
        public string StoreChain { get; set; }
        public string StoreAddress { get; set; }
        public List<GoodTransactionDto> Goods { get; set; }
    }
}