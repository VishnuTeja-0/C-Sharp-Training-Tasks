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

        public static Models.Bank GetBankById(this List<Models.Bank> banks, string bankId)
        {
            return banks.FirstOrDefault(i => i.Id == bankId);
        }

        public static Models.BankAccount GetAccountById(this List<Models.BankAccount> bankAccounts, string accountId)
        {
            return bankAccounts.FirstOrDefault(i => i.Id == accountId);
        }
    }
}
