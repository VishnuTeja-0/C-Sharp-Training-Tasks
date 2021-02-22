using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement.Models
{
    public class Currency
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double ExchangeRate { get; set; }
    }
}
