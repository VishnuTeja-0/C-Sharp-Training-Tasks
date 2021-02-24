using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement.Models
{
    public class BankAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }
        public List<string> Transactions { get; set; }

    }
}
