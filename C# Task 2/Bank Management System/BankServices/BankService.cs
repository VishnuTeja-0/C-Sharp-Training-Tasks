using System;
using System.Collections.Generic;
using System.Linq;
using BankManagement.Contracts;
using BankManagement.Models.EntityModels;

namespace BankManagement.Services
{
    public class BankService : IBankService
    {

        public BankService()
        {
            using (var context = new BankDBContext())
            {
                context.Currencies.Add
                (
                    new Currency
                    {
                        CurrencyCode = Constants.CurrencyCode,
                        CurrencyName = Constants.CurrencyName,
                        ExchangeRate = Constants.ExchangeRate
                    }

                );
            }
        }

        public void CreateBank(string bankName, string staffUsername, string staffPassword)
        {
            using (var context = new BankDBContext())
            {
                context.Banks.Add
                (
                    new Bank
                    {
                        BankName = bankName,
                        BankId = GenerateId(bankName),
                        SameRtgs = Constants.SameRTGS,
                        SameImps = Constants.SameIMPS,
                        DiffRtgs = Constants.DiffRTGS,
                        DiffImps = Constants.DiffIMPS,
                        SupportedCurrencies = $"{Constants.CurrencyCode}, ",
                        StaffUsername = staffUsername,
                        StaffPassword = staffPassword
                    }
                );
                context.SaveChanges();
            }
        }

        #region Staff Functions

        public void CreateAccount(string bankId, string newName, string newUsername, string newPassword, decimal initialDeposit)
        {
            using (var context = new BankDBContext())
            {
                context.Accounts.Add
                (
                    new Account
                    {
                        AccountId = GenerateId(newName),
                        BankId = bankId,
                        HolderName = newName,
                        AccountUsername = newUsername,
                        AccountPassword = newPassword,
                        Balance = initialDeposit
                    }
                );
                context.SaveChanges();
            }
        }

        public void UpdateAccount(string accountId, string newName, string newUsername, string newPassword)
        {   
            using (var context = new BankDBContext())
            {
                Account account = context.Accounts.FirstOrDefault(i => i.AccountId == accountId);
                account.HolderName = newName;
                account.AccountUsername = newUsername;
                account.AccountPassword = newPassword;
                context.SaveChanges();
            }
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            using (var context = new BankDBContext())
            {
                Account account = context.Accounts.FirstOrDefault(s => s.AccountId == accountId);
                context.Accounts.Remove(account);
                context.SaveChanges();
            }
        }

        public void AddCurrency(string bankId, string currencyName, string currencyCode, decimal exchangeRate)
        {
            using (var context = new BankDBContext())
            {
                Bank bank = context.Banks.FirstOrDefault(i => i.BankId == bankId);
                bank.SupportedCurrencies += $"{currencyCode}, ";
                context.Currencies.Add
                (
                    new Currency
                    {
                        CurrencyName = currencyName,
                        CurrencyCode = currencyCode,
                        ExchangeRate = exchangeRate
                    }
                );
                context.SaveChanges();
            }
        }

        public void UpdateServiceCharges(string bankId, decimal newSameRTGS, decimal newSameIMPS, decimal newDiffRTGS, decimal newDiffIMPS)
        { 
            using (var context = new BankDBContext())
            {
                Bank bank = context.Banks.FirstOrDefault(i => i.BankId == bankId);
                bank.SameRtgs = newSameRTGS;
                bank.SameImps = newSameIMPS;
                bank.DiffRtgs = newDiffRTGS;
                bank.DiffImps = newDiffIMPS;
                context.SaveChanges();
            }
        }

        public void RevertTransaction(string transactionId)
        {
            using (var context = new BankDBContext())
            {
                Transaction transaction = context.Transactions.FirstOrDefault(i => i.TransactionId == transactionId);
                Account senderAccount = context.Accounts.FirstOrDefault(i => i.AccountId == transaction.SenderAccountId);
                Account recipientAccount = context.Accounts.FirstOrDefault(i => i.AccountId == transaction.RecipientAccountId);
                Bank senderBank = context.Banks.FirstOrDefault(i => i.BankId == senderAccount.BankId);
                Bank recipientBank = context.Banks.FirstOrDefault(i => i.BankId == recipientAccount.BankId);
                decimal serviceCharges = GetServiceCharges(senderBank.BankId, recipientBank.BankId, transaction.Amount, senderBank.SameRtgs, senderBank.SameImps, senderBank.DiffRtgs, senderBank.DiffImps);
                senderAccount.Balance += (transaction.Amount + serviceCharges);
                recipientAccount.Balance -= transaction.Amount;
                context.Transactions.Remove(transaction);
                context.SaveChanges();
            }
        }

        #endregion End of Staff Functions

        #region Account Functions

        public void DepositAmount(string accountId, decimal amount, string currencyCode)
        {
            using (var context = new BankDBContext())
            {
                if (currencyCode != "INR")
                {
                    amount *= context.Currencies.FirstOrDefault(i => i.CurrencyCode == currencyCode).ExchangeRate;
                }
                Account account = context.Accounts.FirstOrDefault(i => i.AccountId == accountId);
                account.Balance += amount;
                context.SaveChanges();
            }

        }

        public void WithdrawAmount(string accountId, decimal amount)
        {
            using (var context = new BankDBContext())
            {
                Account account = context.Accounts.FirstOrDefault(i => i.AccountId == accountId);
                account.Balance -= amount;
                context.SaveChanges();
            }
        }

        public void TransferFunds(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, decimal amount)
        {
            Bank senderBank = GetBankById(senderBankId);
            Bank recipientBank = GetBankById(recipientBankId);
            decimal serviceCharges = GetServiceCharges(senderBank.BankId, recipientBank.BankId, amount, senderBank.SameRtgs, senderBank.SameImps, senderBank.DiffRtgs, senderBank.DiffImps);
            using (var context = new BankDBContext())
            {
                Account senderAccount = context.Accounts.FirstOrDefault(i => i.AccountId == senderAccountId);
                Account recipientAccount = context.Accounts.FirstOrDefault(i => i.AccountId == recipientAccountId);
                senderAccount.Balance -= (amount + serviceCharges);
                recipientAccount.Balance += amount;
                context.SaveChanges();
            }
            CreateTransaction(senderBankId, senderAccountId, recipientBankId, recipientAccountId, amount);
        }

        public List<Transaction> GetTransactions(string accountId)
        {
            using (var context = new BankDBContext())
            {
                return context.Transactions.Where(i => i.SenderAccountId == accountId || i.RecipientAccountId == accountId).ToList();
            }
        }

        #endregion End of Account Functions

        #region Service Functions

        public Bank GetBankById(string bankId)
        {
            using (var context = new BankDBContext())
            {
                return context.Banks.FirstOrDefault(i => i.BankId == bankId);
            }
        }

        public Account GetAccountById(string accountId)
        {
            using (var context = new BankDBContext())
            {
                return context.Accounts.FirstOrDefault(i => i.AccountId == accountId);
            }
        }

        public Transaction GetTransactionById(string transactionId)
        {
            using (var context = new BankDBContext())
            {
                return context.Transactions.FirstOrDefault(i => i.TransactionId == transactionId);
            }
        }

        public bool IsBankAvailable(string bankName)
        {
            using (var context = new BankDBContext())
            {
                return context.Banks.Any(i => i.BankName == bankName);
            }
        }

        public string GetBankId(string bankName)
        {
            using (var context = new BankDBContext())
            {
                return context.Banks.FirstOrDefault(i => i.BankName == bankName).BankId;
            }
        }

        public string GetBankName(string bankId)
        {
            return GetBankById(bankId).BankName;
        }

        public bool IsStaff(string bankId, string usernameInput)
        {
            return GetBankById(bankId).StaffUsername == usernameInput;
        }

        public bool IsValidStaffPassword(string bankId, string passwordInput)
        {
            return GetBankById(bankId).StaffPassword == passwordInput;
        }

        public bool IsAccountHolder(string usernameInput)
        {
            using (var context = new BankDBContext())
            {
                return context.Accounts.Any(i => i.AccountUsername == usernameInput);
            }
        }

        public bool IsValidAccountPassword(string usernameInput, string passwordInput)
        {
            using (var context = new BankDBContext())
            {
                Account bankAccount = context.Accounts.FirstOrDefault(i => i.AccountUsername == usernameInput);
                return bankAccount.AccountPassword == passwordInput;
            }

        }

        public bool IsAccountAvailable(string bankId, string accountId)
        {
            using (var context = new BankDBContext())
            {
                return context.Accounts.Any(i => i.BankId == bankId && i.AccountId == accountId);
            }
        }

        public string GetAccountId(string bankId, string username)
        {
            using (var context = new BankDBContext())
            {
                return context.Accounts.FirstOrDefault(i => i.BankId == bankId && i.AccountUsername == username).AccountId;
            }
        }

        public string GetAccountName(string accountId)
        {
            return GetAccountById(accountId).HolderName;
        }

        public bool IsCurrencyAvailable(string bankId, string currencyCode)
        {
            return GetBankById(bankId).SupportedCurrencies.Contains($" {currencyCode},");
        }

        public decimal GetAccountBalance(string accountId)
        {
            return GetAccountById(accountId).Balance;
        }

        public bool IsTransactionAvailable(string transactionId)
        {
            using (var context = new BankDBContext())
            {
                return context.Transactions.Any(i => i.TransactionId == transactionId);
            }
        }

        public string GenerateId(string name)
        {
            return $"{name.Substring(0, 3)}{DateTime.Now}";
        }

        public void CreateTransaction(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, decimal amount)
        {
            using (var context = new BankDBContext())
            {
                context.Transactions.Add
                (
                    new Transaction
                    {
                        TransactionId = GenerateTransactionId(senderBankId, senderAccountId),
                        SenderAccountId = senderAccountId,
                        RecipientAccountId = recipientAccountId,
                        Amount = amount,
                        TransactionDateTime = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }

        public string GenerateTransactionId(string bankId, string accountId)
        {
            return $"TXN{bankId}{accountId}{DateTime.Now}";
        }

        public decimal GetServiceCharges(string senderBankId, string receiverBankId, decimal amount, decimal sameRTGS, decimal sameIMPS, decimal diffRTGS, decimal diffIMPS)
        {
            return (senderBankId == receiverBankId) ? amount * (sameRTGS + sameIMPS) : amount * (diffRTGS + diffIMPS);
        }

        public string GetAccountBank(string accountId)
        {
            return GetAccountById(accountId).BankId;
        }

        #endregion End of Service Functions

    }
}
