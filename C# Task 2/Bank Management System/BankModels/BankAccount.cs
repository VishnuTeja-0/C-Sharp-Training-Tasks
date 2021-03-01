using System.Collections.Generic;

namespace BankManagement.Models
{
    public class BankAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}
