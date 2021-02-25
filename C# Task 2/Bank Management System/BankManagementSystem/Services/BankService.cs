using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankManagement.Models;
using static BankManagement.EnumsClass;

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
                Id = $"{bankName.Substring(0, 3)}{DateTime.Today:d}",
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

        public void CreateAccount(string bankId, string newName, string newUsername, string newPassword, double initialDeposit)
        {
            BankAccount bankAccount = new BankAccount
            {
                Id = $"{newName.Substring(0, 3)}{DateTime.Today:d}",
                Name = newName,
                Username = newUsername,
                Password = newPassword,
                Balance = initialDeposit,
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

        public void RevertTransaction(string bankId, string accountId, string transactionId)
        {
            Bank bank = _banks.GetBankById(bankId);
            BankAccount account = bank.Accounts.GetAccountById(accountId);
            Transaction transaction = account.Transactions.FirstOrDefault(i => i.Id == transactionId);
            if (transaction.Type == (TransactionTypes) 1)
            {
                Bank recipientBank = _banks.GetBankById(transaction.ReceiverBankId);
                BankAccount recipientAccount = recipientBank.Accounts.GetAccountById(transaction.ReceiverId);
                double amount = transaction.Amount;
                double serviceCharges;
                if (recipientBank == bank)
                {
                    serviceCharges = amount * (bank.SameRTGS + bank.SameIMPS);
                }
                else
                {
                    serviceCharges = amount * (bank.DiffRTGS + bank.DiffIMPS);
                }
                account.Balance += amount + serviceCharges;
                recipientAccount.Balance -= amount;
                account.Transactions.Remove(transaction);
                recipientAccount.Transactions.Remove(transaction);
            }
            else if (transaction.Type == (TransactionTypes) 2)
            {
                Bank senderBank = _banks.GetBankById(transaction.SenderBankId);
                BankAccount senderAccount = senderBank.Accounts.GetAccountById(transaction.SenderId);
                double amount = transaction.Amount;
                double serviceCharges;
                if (senderBank == bank)
                {
                    serviceCharges = amount * (senderBank.SameRTGS + senderBank.SameIMPS);
                }
                else
                {
                    serviceCharges = amount * (senderBank.DiffRTGS + senderBank.DiffIMPS);
                }
                account.Balance -= amount;
                senderAccount.Balance += amount + serviceCharges;
                account.Transactions.Remove(transaction);
                senderAccount.Transactions.Remove(transaction);
            }
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

        public void TransferFunds(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount)
        {
            Bank senderBank = _banks.GetBankById(senderBankId);
            Bank recipientBank = _banks.GetBankById(recipientBankId);
            BankAccount senderAccount = senderBank.Accounts.GetAccountById(senderAccountId);
            BankAccount recipientAccount = recipientBank.Accounts.GetAccountById(recipientAccountId);
            double serviceCharges;
            if (senderBank == recipientBank)
            {
                serviceCharges = amount * (senderBank.SameRTGS + senderBank.SameIMPS);
            }
            else
            {
                serviceCharges = amount * (senderBank.DiffRTGS + senderBank.DiffIMPS);
            }
            senderAccount.Balance -= amount + serviceCharges;
            recipientAccount.Balance += amount;
            Transaction senderTransaction = new Transaction
            {
                Type = (TransactionTypes) 1,
                Id = $"TXN{senderBankId}{senderAccountId}{DateTime.Today:d}",
                SenderBankId = senderBankId,
                SenderId = senderAccountId,
                ReceiverBankId = recipientBankId,
                ReceiverId = recipientAccountId,
                Amount = amount + serviceCharges,
                Time = DateTime.Now.ToString()
            };
            senderAccount.Transactions.Add(senderTransaction);
            Transaction recipientTransaction = new Transaction
            {
                Type = (TransactionTypes) 2,
                Id = $"TXN{recipientBankId}{recipientAccountId}{DateTime.Today:d}",
                SenderBankId = senderBankId,
                SenderId = senderAccountId,
                ReceiverBankId = recipientBankId,
                ReceiverId = recipientAccountId,
                Amount = amount,
                Time = DateTime.Now.ToString()
            };
            recipientAccount.Transactions.Add(recipientTransaction);
        }

        public List<Transaction> GetTransactions(string bankId, string accountId)
        {
            BankAccount account = _banks.GetBankById(bankId).Accounts.GetAccountById(accountId);
            return account.Transactions;
        }

        // Service Functions

        public bool IsBankAvailable(string bankName)
        {
            return _banks.Any(i => i.Name == bankName);
        }

        public string GetBankId(string bankName)
        {
            return _banks.FirstOrDefault(i => i.Name == bankName).Id;
        }

        public string GetBankName(string bankId)
        {
            return _banks.GetBankById(bankId).Name;
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
            return _banks.GetBankById(bankId).Accounts.FirstOrDefault(i => i.Username == username).Id;
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

        public bool IsTransactionAvailable(string bankId, string accountId, string transactionId)
        {
            return _banks.GetBankById(bankId).Accounts.GetAccountById(accountId).Transactions.Any(i => i.Id == transactionId);
        }

    }
}
