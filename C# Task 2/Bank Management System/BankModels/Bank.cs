using System.Collections.Generic;

namespace BankManagement.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public double SameRTGS { get; set; }
        public double SameIMPS { get; set; }
        public double DiffRTGS { get; set; }
        public double DiffIMPS { get; set; }

        public string Currencies { get; set; }

        public string StaffUsername { get; set; }
        public string StaffPassword { get; set; }

    }
}
