using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement.Models
{
    public class Bank
    {
        private int _name { get; set; }
        private int _id { get; set; }
        private int _sameRTGS { get; set; }
        private int _sameIMPS { get; set; }
        private int _diffRTGS { get; set; }
        private int _diffIMPS { get; set; }
        private List<BankAccount> _accounts { get; set; }
        private List<Currency> _currencies { get; set; }
        private string _staffUsername { get; set; }
        private string _staffPassword { get; set; }

    }
}
