using System;
using System.Collections.Generic;

#nullable disable

namespace BankManagement.Services.EntityModels
{
    public partial class Transaction
    {
        public string TransactionId { get; set; }
        public string AccountId { get; set; }
        public string TransactionType { get; set; }
        public string AssociatedAccountId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TransactionDateTime { get; set; }

        public virtual Account Account { get; set; }
        public virtual Account AssociatedAccount { get; set; }
    }
}
