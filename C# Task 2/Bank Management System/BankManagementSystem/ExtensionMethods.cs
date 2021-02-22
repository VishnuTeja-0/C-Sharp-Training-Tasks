using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankManagement
{
    public static class ExtensionMethods
    {
        public static bool IsAlphabetical(this string str)
        {
            return str.All(char.IsLetter);
        }

        public static Models.Bank GetBankByName(this List<Models.Bank> banks, string bankName)
        {
            return banks.FirstOrDefault(i => i.Name == bankName);
        }

        public static Models.BankAccount GetAccount(this List<Models.BankAccount> bankAccounts, string username)
        {
            return bankAccounts.FirstOrDefault(i => i.Username == username);
        }
    }
}
