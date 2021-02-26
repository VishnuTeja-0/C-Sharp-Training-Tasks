using System;
using System.Collections.Generic;
using System.Linq;
using BankManagement.Models;

namespace BankManagement.Services
{
    public class BankService
    {
        List<Bank> _banks = new List<Bank>();

        public void CreateBank(string bankName, string staffUsername, string staffPassword)
        {
            Bank bank = new Bank
            {
                Name = bankName,
                Id = $"{bankName.Substring(0, 3)}{DateTime.Today:d}",
                SameRTGS = DefaultValues.sameRTGS,
                SameIMPS = DefaultValues.sameIMPS,
                DiffRTGS = DefaultValues.diffRTGS,
                DiffIMPS = DefaultValues.diffIMPS,
                Accounts = new List<BankAccount>(),
                Currencies = new List<Currency>{new Currency { Code = DefaultValues.currencyCode, 
                                                               ExchangeRate = DefaultValues.exchangeRate, 
                                                               Name = DefaultValues.currencyName }},
                StaffUsername = staffUsername,
                StaffPassword = staffPassword
            };
            _banks.Add(bank);
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
            GetBank(bankId).Accounts.Add(bankAccount);
        }

        public void UpdateAccount(string bankId, string accountId, string newName, string newUsername, string newPassword)
        {
            BankAccount account = GetAccount(bankId, accountId);
            account.Name = newName;
            account.Username = newUsername;
            account.Password = newPassword;
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            BankAccount account = GetAccount(bankId, accountId);
            GetBank(bankId).Accounts.Remove(account);
        }

        public void AddCurrency(string bankId, string currencyName, string currencyCode, double exchangeRate)
        {
            Currency curr = new Currency
            {
                Name = currencyName,
                Code = currencyCode,
                ExchangeRate = exchangeRate
            };
            GetBank(bankId).Currencies.Add(curr);
        }

        public void UpdateServiceCharges(string bankId, double newSameRTGS, double newSameIMPS, double newDiffRTGS, double newDiffIMPS)
        {
            Bank bank = GetBank(bankId);
            bank.SameRTGS = newSameRTGS;
            bank.SameIMPS = newSameIMPS;
            bank.DiffRTGS = newDiffRTGS;
            bank.DiffIMPS = newDiffIMPS;
        }

        public void RevertTransaction(string bankId, string accountId, string transactionId)
        {
            Bank bank = GetBank(bankId);
            BankAccount account = GetAccount(bankId, accountId);
            Transaction transaction = GetTransactions(bankId, accountId).FirstOrDefault(i => i.Id == transactionId);
            double amount = transaction.Amount;
            if (transaction.Type == "Sent")
            {
                Bank recipientBank = GetBank(transaction.ReceiverBankId);
                BankAccount recipientAccount = GetAccount(recipientBank.Id, transaction.ReceiverId);
                double serviceCharges = (recipientBank == bank) ? amount * (bank.SameRTGS + bank.SameIMPS) : amount * (bank.DiffRTGS + bank.DiffIMPS);
                account.Balance += amount + serviceCharges;
                recipientAccount.Balance -= amount;
                recipientAccount.Transactions.Remove(transaction);
            }
            else if (transaction.Type == "Received")
            {
                Bank senderBank = GetBank(transaction.SenderBankId);
                BankAccount senderAccount = GetAccount(senderBank.Id, transaction.SenderId);
                
                double serviceCharges = (senderBank == bank) ? amount * (senderBank.SameRTGS + senderBank.SameIMPS) : amount * (senderBank.DiffRTGS + senderBank.DiffIMPS);
                account.Balance -= amount;
                senderAccount.Balance += amount + serviceCharges;
                senderAccount.Transactions.Remove(transaction);
            }
            account.Transactions.Remove(transaction);
        }

        // Account Functions

        public void DepositAmount(string bankId, string accountId, double amount, string currencyCode)
        {
            Bank bank = GetBank(bankId);
            if (currencyCode != "INR")
            {
                amount *= bank.Currencies.FirstOrDefault(i => i.Code == currencyCode).ExchangeRate;
            }
            BankAccount account = GetAccount(bankId, accountId);
            account.Balance += amount;
        }

        public void WithdrawAmount(string bankId, string accountId, double amount)
        {
            BankAccount account = GetAccount(bankId, accountId);
            account.Balance -= amount;
        }

        public void TransferFunds(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount)
        {
            Bank senderBank = GetBank(senderBankId);
            Bank recipientBank = GetBank(recipientBankId);
            BankAccount senderAccount = GetAccount(senderBankId, senderAccountId);
            BankAccount recipientAccount = GetAccount(recipientBankId, recipientAccountId);
            double serviceCharges = (senderBank == recipientBank) ? amount * (senderBank.SameRTGS + senderBank.SameIMPS) : amount * (senderBank.DiffRTGS + senderBank.DiffIMPS);
            senderAccount.Balance -= amount + serviceCharges;
            recipientAccount.Balance += amount;
            CreateTransaction("Sent", senderBankId, senderAccountId, senderBankId, senderAccountId, recipientBankId, recipientAccountId, amount + serviceCharges);
            CreateTransaction("Received", recipientBankId, recipientAccountId, senderBankId, senderAccountId, recipientBankId, recipientAccountId, amount);
        }

        public List<Transaction> GetTransactions(string bankId, string accountId)
        {
            BankAccount account = GetAccount(bankId, accountId);
            return account.Transactions;
        }

        // Service Functions

        public Bank GetBank(string bankId)
        {
            return _banks.GetBankById(bankId);
        }

        public BankAccount GetAccount(string bankId, string accountId)
        {
            return GetBank(bankId).Accounts.GetAccountById(accountId);
        }

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
            return GetBank(bankId).Name;
        }

        public bool IsStaff(string bankId, string usernameInput)
        {
            return GetBank(bankId).StaffUsername == usernameInput;
        }

        public bool IsValidStaffPassword(string bankId, string passwordInput)
        {
            return GetBank(bankId).StaffPassword == passwordInput;
        }

        public bool IsAccountHolder(string bankId, string usernameInput)
        {
            return GetBank(bankId).Accounts.Any(i => i.Username == usernameInput);
        }

        public bool IsValidAccountPassword(string bankId, string usernameInput, string passwordInput)
        {
            BankAccount bankAccount = GetBank(bankId).Accounts.FirstOrDefault(i => i.Username == usernameInput);
            return bankAccount.Password == passwordInput;
        }

        public bool IsAccountAvailable(string bankId, string accountId)
        {
            return GetBank(bankId).Accounts.Any(i => i.Id == accountId);
        }

        public string GetAccountId(string bankId, string username)
        {
            return GetBank(bankId).Accounts.FirstOrDefault(i => i.Username == username).Id;
        }

        public string GetAccountName(string bankId, string accountId)
        {
            return GetAccount(bankId, accountId).Name;
        }

        public bool IsCurrencyAvailable(string bankId, string accountId, string currencyCode)
        {
            return GetBank(bankId).Currencies.Any(i => i.Code == currencyCode);
        }

        public double GetAccountBalance(string bankId, string accountId)
        {
            return GetAccount(bankId, accountId).Balance;
        }

        public bool IsTransactionAvailable(string bankId, string accountId, string transactionId)
        {
            return GetAccount(bankId, accountId).Transactions.Any(i => i.Id == transactionId);
        }

        public void CreateTransaction(string type, string idBank, string idAccount, string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount)
        {
            Transaction transaction = new Transaction
            {
                Type = type,
                Id = $"TXN{idBank}{idAccount}{DateTime.Today:d}",
                SenderBankId = senderBankId,
                SenderId = senderAccountId,
                ReceiverBankId = recipientBankId,
                ReceiverId = recipientAccountId,
                Amount = amount,
                Time = DateTime.Now.ToString()
            };
            GetAccount(idBank, idAccount).Transactions.Add(transaction);
        }

    }
}
