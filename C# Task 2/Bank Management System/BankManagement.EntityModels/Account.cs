using System;
using System.Collections.Generic;

#nullable disable

namespace BankManagement.EntityModels
{
    public partial class Account
    {
        public Account()
        {
            TransactionAccounts = new HashSet<Transaction>();
            TransactionAssociatedAccounts = new HashSet<Transaction>();
        }

        public string AccountId { get; set; }
        public string BankId { get; set; }
        public string HolderName { get; set; }
        public decimal? Balance { get; set; }
        public string AccountUsername { get; set; }
        public string AccountPassword { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual ICollection<Transaction> TransactionAccounts { get; set; }
        public virtual ICollection<Transaction> TransactionAssociatedAccounts { get; set; }
    }
}
