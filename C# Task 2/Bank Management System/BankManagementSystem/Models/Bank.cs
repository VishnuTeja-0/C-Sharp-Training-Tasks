using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public double SameRTGS { get; set; } = 0;
        public double SameIMPS { get; set; } = 0.05;
        public double DiffRTGS { get; set; } = 0.02;
        public double DiffIMPS { get; set; } = 0.06;
        public List<BankAccount> Accounts { get; set; } = new List<BankAccount>();

        public List<Currency> Currencies { get; set; } = new List<Currency>();

        public string StaffUsername { get; set; }
        public string StaffPassword { get; set; }

    }
}
