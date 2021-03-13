using System;
using System.Collections.Generic;

#nullable disable

namespace BankManagement.Services.EntityModels
{
    public partial class Currency
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
