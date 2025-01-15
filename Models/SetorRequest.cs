namespace Bosnet.Models
{
    public class SetorRequest
    {
        public required string AccountId { get; set; }
        public required string CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public required string Note { get; set; } = "SETOR"; 
        public SetorRequest()
        {
            Note = "SETOR";
        }
    }
}
