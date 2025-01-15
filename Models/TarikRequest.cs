namespace Bosnet.Models
{
    public class TarikRequest
    {
        public required string AccountId { get; set; }
        public required string CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public required string Note { get; set; } = "TARIK"; 
        public TarikRequest()
        {
            Note = "TARIK"; // Default value if not provided
        }
    }
}
