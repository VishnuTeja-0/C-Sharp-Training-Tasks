using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankManagement.Models;

namespace BankManagement.Services
{
    class BankService
    {
        List<Bank> _banks = new List<Bank>();

        public void SetupBank(string bankName, string staffUsername, string staffPassword)
        {
            Bank bank = new Bank();
            bank.Name = bankName;
            bank.StaffUsername = staffUsername;
            bank.StaffPassword = staffPassword;
            _banks.Add(bank);
            AddCurrency(bankName, "Indian Rupee", "INR", 1);

        }

        public void CreateAccount(string bankName, string newUsername, string newPassword)
        {
            BankAccount bankAccount = new BankAccount
            {
                Username = newUsername,
                Password = newPassword
            };
            _banks.GetBankByName(bankName).Accounts.Add(bankAccount);
        }

        public void UpdateAccount(string bankName, string newUsername, string newPassword)
        {

        }



        public void AddCurrency(string bankName, string currencyName, string currencyCode, int exchangeRate)
        {
            Currency curr = new Currency
            {
                Name = currencyName,
                Code = currencyCode,
                ExchangeRate = exchangeRate
            };
            _banks.GetBankByName(bankName).Currencies.Add(curr);
        }

        public bool IsBankAvailable(string bankName)
        {
            return _banks.Any(i => i.Name == bankName);
        }

        public string GetBankName(int bankOption)
        {
            return _banks[bankOption - 1].Name;
        }

        public bool IsStaff(int bankOption, string usernameInput)
        {
            return _banks[bankOption - 1].StaffUsername == usernameInput;
        }
        public bool IsValidStaffPassword(int bankOption, string passwordInput)
        {
            return _banks[bankOption - 1].StaffPassword == passwordInput;
        }

        public bool IsAccountHolder(string bankName, string usernameInput)
        {
            Bank bank = _banks.GetBankByName(bankName);
            return bank.Accounts.Any(i => i.Username == usernameInput);
        }

        public bool IsValidAccountPassword(int bankOption, string usernameInput, string passwordInput)
        {
            BankAccount bankAccount = _banks[bankOption - 1].Accounts.GetAccount(usernameInput);
            return bankAccount.Password == passwordInput;
        }


    }
}
