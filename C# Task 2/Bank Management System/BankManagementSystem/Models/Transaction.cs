using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string senderId { get; set; }
        public string receiverId { get; set; }
        public double amount { get; set; }
    }
}
