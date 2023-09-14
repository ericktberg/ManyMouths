namespace PriceCheck.DB
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Global_Trade_Item_Number
    /// </summary>
    public readonly struct Gtin
    {
        public Gtin(ulong value)
        {
            Value = value;
        }

        public readonly ulong Value { get; }

        public static Gtin Parse(string upcString)
        {
            ulong value = ulong.Parse(upcString);
            return new Gtin(value);
        }

        public string Gtin12()
        {
            return Value.ToString("D12");
        }

        public string Gtin14()
        {
            return Value.ToString("D14");
        }
    }
}