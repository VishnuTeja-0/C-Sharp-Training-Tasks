using System;
using System.Collections.Generic;

#nullable disable

namespace BankManagement.Models.EntityModels
{
    public partial class Bank
    {
        public Bank()
        {
            Accounts = new HashSet<Account>();
        }

        public string BankId { get; set; }
        public string BankName { get; set; }
        public decimal SameRtgs { get; set; }
        public decimal SameImps { get; set; }
        public decimal DiffRtgs { get; set; }
        public decimal DiffImps { get; set; }
        public string SupportedCurrencies { get; set; }
        public string StaffUsername { get; set; }
        public string StaffPassword { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
