using System;
using System.Collections.Generic;
using System.Linq;
using BankManagement.Contracts;
using BankManagement.Models;

namespace BankManagement.Services
{
    public class BankService : IBankService
    {
        public List<Bank> _banks;
        public BankService()
        {
            _banks = new List<Bank>();
        }

        public void CreateBank(string bankName, string staffUsername, string staffPassword)
        {
            Bank bank = new Bank
            {
                Name = bankName,
                Id = CreateId(bankName),
                SameRTGS = Constants.sameRTGS,
                SameIMPS = Constants.sameIMPS,
                DiffRTGS = Constants.diffRTGS,
                DiffIMPS = Constants.diffIMPS,
                Accounts = new List<BankAccount>(),
                Currencies = new List<Currency>
                {
                    new Currency 
                    { 
                        Code = Constants.currencyCode, 
                        ExchangeRate = Constants.exchangeRate, 
                        Name = Constants.currencyName 
                    }
                },
                StaffUsername = staffUsername,
                StaffPassword = staffPassword
            };
            _banks.Add(bank);
        }

        #region Staff Functions

        public void CreateAccount(string bankId, string newName, string newUsername, string newPassword, double initialDeposit)
        {
            BankAccount bankAccount = new BankAccount
            {
                Id = CreateId(newName),
                Name = newName,
                Username = newUsername,
                Password = newPassword,
                Balance = initialDeposit,
                Transactions = new List<Transaction>()
            };
            GetBankById(bankId).Accounts.Add(bankAccount);
        }

        public void UpdateAccount(string bankId, string accountId, string newName, string newUsername, string newPassword)
        {
            BankAccount account = GetAccountById(bankId, accountId);
            account.Name = newName;
            account.Username = newUsername;
            account.Password = newPassword;
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            BankAccount account = GetAccountById(bankId, accountId);
            GetBankById(bankId).Accounts.Remove(account);
        }

        public void AddCurrency(string bankId, string currencyName, string currencyCode, double exchangeRate)
        {
            Currency curr = new Currency
            {
                Name = currencyName,
                Code = currencyCode,
                ExchangeRate = exchangeRate
            };
            GetBankById(bankId).Currencies.Add(curr);
        }

        public void UpdateServiceCharges(string bankId, double newSameRTGS, double newSameIMPS, double newDiffRTGS, double newDiffIMPS)
        {
            Bank bank = GetBankById(bankId);
            bank.SameRTGS = newSameRTGS;
            bank.SameIMPS = newSameIMPS;
            bank.DiffRTGS = newDiffRTGS;
            bank.DiffIMPS = newDiffIMPS;
        }

        public void RevertTransaction(string bankId, string accountId, string transactionId)
        {
            Bank bank = GetBankById(bankId);
            BankAccount account = GetAccountById(bankId, accountId);
            Transaction transaction = GetTransactions(bankId, accountId).FirstOrDefault(i => i.Id == transactionId);
            double amount = transaction.Amount;
            double serviceCharges;
            switch (transaction.Type)
            {
                case TransactionTypes.Sent:
                    Bank recipientBank = GetBankById(transaction.ReceiverBankId);
                    BankAccount recipientAccount = GetAccountById(recipientBank.Id, transaction.ReceiverId);
                    serviceCharges = GetServiceCharges(bankId, recipientAccount.Id, amount, bank.SameRTGS, bank.SameIMPS, bank.DiffRTGS, bank.DiffIMPS);
                    account.Balance += amount + serviceCharges;
                    recipientAccount.Balance -= amount;
                    recipientAccount.Transactions.Remove(transaction);
                    break;
                case TransactionTypes.Received:
                    Bank senderBank = GetBankById(transaction.SenderBankId);
                    BankAccount senderAccount = GetAccountById(senderBank.Id, transaction.SenderId);
                    serviceCharges = GetServiceCharges(senderBank.Id, bankId, amount, senderBank.SameRTGS, senderBank.SameIMPS, senderBank.DiffRTGS, senderBank.DiffIMPS);
                    account.Balance -= amount;
                    senderAccount.Balance += amount + serviceCharges;
                    senderAccount.Transactions.Remove(transaction);
                    break;
            }
            account.Transactions.Remove(transaction);
        }

        #endregion End of Staff Functions

        #region Account Functions

        public void DepositAmount(string bankId, string accountId, double amount, string currencyCode)
        {
            Bank bank = GetBankById(bankId);
            if (currencyCode != "INR")
            {
                amount *= bank.Currencies.FirstOrDefault(i => i.Code == currencyCode).ExchangeRate;
            }
            BankAccount account = GetAccountById(bankId, accountId);
            account.Balance += amount;
        }

        public void WithdrawAmount(string bankId, string accountId, double amount)
        {
            BankAccount account = GetAccountById(bankId, accountId);
            account.Balance -= amount;
        }

        public void TransferFunds(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount)
        {
            Bank senderBank = GetBankById(senderBankId);
            Bank recipientBank = GetBankById(recipientBankId);
            BankAccount senderAccount = GetAccountById(senderBankId, senderAccountId);
            BankAccount recipientAccount = GetAccountById(recipientBankId, recipientAccountId);
            double serviceCharges = GetServiceCharges(senderBank.Id, recipientBank.Id, amount, senderBank.SameRTGS, senderBank.SameIMPS, senderBank.DiffRTGS, senderBank.DiffIMPS); ;
            senderAccount.Balance -= amount + serviceCharges;
            recipientAccount.Balance += amount;
            CreateTransaction(TransactionTypes.Sent, senderBankId, senderAccountId, senderBankId, senderAccountId, recipientBankId, recipientAccountId, amount + serviceCharges);
            CreateTransaction(TransactionTypes.Received, recipientBankId, recipientAccountId, senderBankId, senderAccountId, recipientBankId, recipientAccountId, amount);
        }

        public List<Transaction> GetTransactions(string bankId, string accountId)
        {
            BankAccount account = GetAccountById(bankId, accountId);
            return account.Transactions;
        }

        #endregion End of Account Functions

        #region Service Functions

        public Bank GetBankById(string bankId)
        {
            return _banks.FirstOrDefault(i => i.Id == bankId);
        }

        public BankAccount GetAccountById(string bankId, string accountId)
        {
            return GetBankById(bankId).Accounts.FirstOrDefault(i => i.Id == accountId);
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
            return GetBankById(bankId).Name;
        }

        public bool IsStaff(string bankId, string usernameInput)
        {
            return GetBankById(bankId).StaffUsername == usernameInput;
        }

        public bool IsValidStaffPassword(string bankId, string passwordInput)
        {
            return GetBankById(bankId).StaffPassword == passwordInput;
        }

        public bool IsAccountHolder(string bankId, string usernameInput)
        {
            return GetBankById(bankId).Accounts.Any(i => i.Username == usernameInput);
        }

        public bool IsValidAccountPassword(string bankId, string usernameInput, string passwordInput)
        {
            BankAccount bankAccount = GetBankById(bankId).Accounts.FirstOrDefault(i => i.Username == usernameInput);
            return bankAccount.Password == passwordInput;
        }

        public bool IsAccountAvailable(string bankId, string accountId)
        {
            return GetBankById(bankId).Accounts.Any(i => i.Id == accountId);
        }

        public string GetAccountId(string bankId, string username)
        {
            return GetBankById(bankId).Accounts.FirstOrDefault(i => i.Username == username).Id;
        }

        public string GetAccountName(string bankId, string accountId)
        {
            return GetAccountById(bankId, accountId).Name;
        }

        public bool IsCurrencyAvailable(string bankId, string accountId, string currencyCode)
        {
            return GetBankById(bankId).Currencies.Any(i => i.Code == currencyCode);
        }

        public double GetAccountBalance(string bankId, string accountId)
        {
            return GetAccountById(bankId, accountId).Balance;
        }

        public bool IsTransactionAvailable(string bankId, string accountId, string transactionId)
        {
            return GetAccountById(bankId, accountId).Transactions.Any(i => i.Id == transactionId);
        }

        public string CreateId(string name)
        {
            return $"{name.Substring(0, 3)}{DateTime.Now}";
        }

        public void CreateTransaction(TransactionTypes type, string bankId, string accountId, string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount)
        {
            Transaction transaction = new Transaction
            {
                Type = type,
                Id = CreateTransactionId(bankId, accountId),
                SenderBankId = senderBankId,
                SenderId = senderAccountId,
                ReceiverBankId = recipientBankId,
                ReceiverId = recipientAccountId,
                Amount = amount,
                Time = DateTime.Now.ToString()
            };
            GetAccountById(bankId, accountId).Transactions.Add(transaction);
        }

        public string CreateTransactionId(string bankId, string accountId)
        {
            return $"TXN{bankId}{accountId}{DateTime.Now}";
        }

        public double GetServiceCharges(string senderBankId, string receiverBankId, double amount, double sameRTGS, double sameIMPS, double diffRTGS, double diffIMPS)
        {
            return (senderBankId == receiverBankId) ? amount * (sameRTGS + sameIMPS) : amount * (diffRTGS + diffIMPS);
        }

        #endregion End of Service Functions

    }
}
