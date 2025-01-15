using System.Collections.Generic;
namespace Bosnet.Models
{
    public class TransferRequest
    {
        public required string SourceAccountId { get; set; }
        public required string CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public required List<string> TargetAccountIds { get; set; }
        public required string Note { get; set; }
    }
}
