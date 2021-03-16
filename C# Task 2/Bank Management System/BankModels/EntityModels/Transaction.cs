using System;
using System.Collections.Generic;

#nullable disable

namespace BankManagement.Models.EntityModels
{
    public partial class Transaction
    {
        public string TransactionId { get; set; }
        public string SenderAccountId { get; set; }
        public string RecipientAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }

        public virtual Account RecipientAccount { get; set; }
        public virtual Account SenderAccount { get; set; }
    }
}
