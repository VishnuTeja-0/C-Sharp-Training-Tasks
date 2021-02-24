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

        public void CreateBank(string bankName, string staffUsername, string staffPassword)
        {
            Bank bank = new Bank
            {
                Name = bankName,
                Id = $"{bankName.Substring(0, 3)}{DateTime.Now}",
                SameRTGS = 0,
                SameIMPS = 0.05,
                DiffRTGS = 0.02,
                DiffIMPS = 0.06,
                Accounts = new List<BankAccount>(),
                Currencies = new List<Currency>(),
                StaffUsername = staffUsername,
                StaffPassword = staffPassword
            };
            _banks.Add(bank);
            AddCurrency(bank.Id, "Indian Rupee", "INR", 1);

        }

        // Staff Functions

        public void CreateAccount(string bankId, string newName, string newUsername, string newPassword)
        {
            BankAccount bankAccount = new BankAccount
            {
                Id = $"{newName.Substring(0, 3)}{DateTime.Now}",
                Name = newName,
                Username = newUsername,
                Password = newPassword,
                Transactions = new List<Transaction>()
            };
            _banks.GetBankById(bankId).Accounts.Add(bankAccount);
        }

        public void UpdateAccount(string bankId, string accountId, string newName, string newUsername, string newPassword)
        {
            BankAccount account = _banks.GetBankById(bankId).Accounts.GetAccountById(accountId);
            account.Name = newName;
            account.Username = newUsername;
            account.Password = newPassword;
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            _banks.GetBankById(bankId).Accounts.RemoveAll(i => i.Id == accountId);
        }

        public void AddCurrency(string bankId, string currencyName, string currencyCode, double exchangeRate)
        {
            Currency curr = new Currency
            {
                Name = currencyName,
                Code = currencyCode,
                ExchangeRate = exchangeRate
            };
            _banks.GetBankById(bankId).Currencies.Add(curr);
        }

        public void UpdateServiceCharges(string bankId, double newSameRTGS, double newSameIMPS, double newDiffRTGS, double newDiffIMPS)
        {
            Bank bank = _banks.GetBankById(bankId);
            bank.SameRTGS = newSameRTGS;
            bank.SameIMPS = newSameIMPS;
            bank.DiffRTGS = newDiffRTGS;
            bank.DiffIMPS = newDiffIMPS;
        }

        // Account Functions

        public void DepositAmount(string bankId, string accountId, double amount, string currencyCode)
        {
            Bank bank = _banks.GetBankById(bankId);
            if (currencyCode != "INR")
            {
                amount *= bank.Currencies.FirstOrDefault(i => i.Code == currencyCode).ExchangeRate;
            }
            BankAccount account = bank.Accounts.GetAccountById(accountId);
            account.Balance += amount;
        }

        public void WithdrawAmount(string bankId, string accountId, double amount)
        {
            BankAccount account = _banks.GetBankById(bankId).Accounts.GetAccountById(accountId);
            account.Balance -= amount;
        }

        // Service Functions

        public bool IsBankAvailable(string bankName)
        {
            return _banks.Any(i => i.Name == bankName);
        }

        public List<string> GetBanks()
        {
            List<string> bankNames = new List<string>();
            foreach(Bank bank in _banks)
            {
                bankNames.Add(bank.Name);
            }
            return bankNames;
        }

        public string GetBanks(string bankId)
        {
            return _banks.GetBankById(bankId).Name;
        }

        public string GetBankId(int bankOption)
        {
            return _banks[bankOption - 1].Id;
        }

        public bool IsStaff(string bankId, string usernameInput)
        {
            return _banks.GetBankById(bankId).StaffUsername == usernameInput;
        }
        public bool IsValidStaffPassword(string bankId, string passwordInput)
        {
            return _banks.GetBankById(bankId).StaffPassword == passwordInput;
        }

        public bool IsAccountHolder(string bankId, string usernameInput)
        {
            return _banks.GetBankById(bankId).Accounts.Any(i => i.Username == usernameInput);
        }

        public bool IsValidAccountPassword(string bankId, string usernameInput, string passwordInput)
        {
            BankAccount bankAccount = _banks.GetBankById(bankId).Accounts.FirstOrDefault(i => i.Username == usernameInput);
            return bankAccount.Password == passwordInput;
        }

        public bool IsAccountAvailable(string bankId, string accountId)
        {
            return _banks.GetBankById(bankId).Accounts.Any(i => i.Id == accountId);
        }

        public string GetAccountId(string bankId, string username)
        {
            return _banks.GetBankById(bankId).Accounts.FirstOrDefault(i => i.Name == username).Id;
        }

        public string GetAccountName(string bankId, string accountId)
        {
            return _banks.GetBankById(bankId).Accounts.GetAccountById(accountId).Name;
        }

        public bool IsCurrencyAvailable(string bankId, string accountId, string currencyCode)
        {
            return _banks.GetBankById(bankId).Currencies.Any(i => i.Code == currencyCode);
        }

        public double GetAccountBalance(string bankId, string accountId)
        {
            return _banks.GetBankById(bankId).Accounts.GetAccountById(accountId).Balance;
        }

    }
}
