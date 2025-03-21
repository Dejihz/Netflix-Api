using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace project6._1Api.Entities
{
    public partial class Transactions
    {
        public Transactions() { 
        
        }

        public string Id { get; set; }
        public int? User_id { get; set; }

        public string Account { get; set; }

        public string ClosingAvailableBalance { get; set; }

        public string ClosingBalance { get; set; }

        public string Description { get; set; }

        public string CustomDescription { get; set; }

        public string ForwardAvailableBalance { get; set; }

        public string OpeningBalance { get; set; }

        public string RelatedMessage { get; set; }

        public string SequenceNumber { get; set; }

        public string StatementNumber { get; set; }

        public string TransactionReference { get; set; }
        public string TransactionDate { get; set; }
        public string Category { get; set; }
        public string DebitCredit { get; set; }
        public string? Date_of_transaction { get; set; }
    }

}
