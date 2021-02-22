using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }

    }
}
