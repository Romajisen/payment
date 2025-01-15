namespace Bosnet.Models
{
    public class TransactionHistory
    {
        public required string TransactionId { get; set; }
        public required string SourceAccountId { get; set; }
        public required string AccountId { get; set; }
        public required string CurrencyId { get; set; }
        public DateTime TransactionDate { get; set; }
        public required decimal Amount { get; set; }
        public required string Note { get; set; }
    }
}
