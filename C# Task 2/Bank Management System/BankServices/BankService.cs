using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BankManagement.Contracts;
using BankManagement.Models;
using BankManagement.EntityModels;

namespace BankManagement.Services
{
    public class BankService : IBankService
    {

        public void CreateBank(string bankName, string staffUsername, string staffPassword)
        {
            EntityModels.Bank bank = new EntityModels.Bank
            {
                BankName = bankName,
                BankId = CreateId(bankName),
                SameRtgs = (decimal)Constants.sameRTGS,
                SameImps = (decimal)Constants.sameIMPS,
                DiffRtgs = (decimal)Constants.diffRTGS,
                DiffImps = (decimal)Constants.diffIMPS,
                SupportedCurrencies = $"{Constants.currencyCode}, ",
                StaffUsername = staffUsername,
                StaffPassword = staffPassword
            };

            EntityModels.Currency defaultCurrency = new EntityModels.Currency
            {
                CurrencyCode = Constants.currencyCode,
                CurrencyName = Constants.currencyName,
                ExchangeRate = (decimal)Constants.exchangeRate
            };

            using (var context = new BankDBContext())
            {
                context.Banks.Add(bank);
                context.Currencies.Add(defaultCurrency);
                context.SaveChanges();
            }
        }

        #region Staff Functions

        public void CreateAccount(string bankId, string newName, string newUsername, string newPassword, double initialDeposit)
        {
            EntityModels.Account bankAccount = new EntityModels.Account
            {
                AccountId = CreateId(newName),
                BankId = bankId,
                HolderName = newName,
                AccountUsername = newUsername,
                AccountPassword = newPassword,
                Balance = (decimal)initialDeposit
            };
            
            using (var context = new BankDBContext())
            {
                context.Accounts.Add(bankAccount);
                context.SaveChanges();
            }
        }

        public void UpdateAccount(string accountId, string newName, string newUsername, string newPassword)
        {
            EntityModels.Account account = GetAccountById(accountId);
            account.HolderName = newName;
            account.AccountUsername = newUsername;
            account.AccountPassword = newPassword;
            using (var context = new BankDBContext())
            {
                context.Accounts.Attach(account);
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }      
        }

        public void DeleteAccount(string bankId, string accountId)
        {
            EntityModels.Account account = GetAccountById(accountId);
            using (var context = new BankDBContext())
            {
                var entry = context.Entry(account);
                context.Accounts.Attach(account);
                context.Accounts.Remove(account);
                context.SaveChanges();
            }
        }

        public void AddCurrency(string bankId, string currencyName, string currencyCode, double exchangeRate)
        {
            EntityModels.Currency curr = new EntityModels.Currency
            {
                CurrencyName = currencyName,
                CurrencyCode = currencyCode,
                ExchangeRate = (decimal)exchangeRate
            };

            EntityModels.Bank bank = GetBankById(bankId);
            bank.SupportedCurrencies += $"{currencyCode}, ";

            using (var context = new BankDBContext())
            {
                context.Banks.Attach(bank);
                context.Entry(bank).State = EntityState.Modified;
                context.Currencies.Add(curr);
                context.SaveChanges();
            }          
        }

        public void UpdateServiceCharges(string bankId, double newSameRTGS, double newSameIMPS, double newDiffRTGS, double newDiffIMPS)
        {
            EntityModels.Bank bank = GetBankById(bankId);
            bank.SameRtgs = (decimal)newSameRTGS;
            bank.SameImps = (decimal)newSameIMPS;
            bank.DiffRtgs = (decimal)newDiffRTGS;
            bank.DiffImps = (decimal)newDiffIMPS;

            using (var context = new BankDBContext())
            {
                context.Banks.Attach(bank);
                context.Entry(bank).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void RevertTransaction(string transactionId)
        {
            EntityModels.Transaction transaction = GetTransactionById(transactionId);
            double amount = (double)transaction.Amount;

            EntityModels.Account senderAccount = GetAccountById(transaction.SenderAccountId);
            EntityModels.Account recipientAccount = GetAccountById(transaction.RecipientAccountId);
            EntityModels.Bank senderBank = GetBankById(senderAccount.BankId);
            EntityModels.Bank recipientBank = GetBankById(recipientAccount.BankId);
            double serviceCharges = GetServiceCharges(senderBank.BankId, recipientBank.BankId, amount, (double)senderBank.SameRtgs, (double)senderBank.SameImps, (double)senderBank.DiffRtgs, (double)senderBank.DiffImps);
            senderAccount.Balance += (decimal)(amount + serviceCharges);
            recipientAccount.Balance -= (decimal)amount;

            using (var context = new BankDBContext())
            {
                context.Accounts.Attach(senderAccount);
                context.Accounts.Attach(recipientAccount);
                context.Transactions.Attach(transaction);
                context.Entry(senderAccount).State = EntityState.Modified;
                context.Entry(recipientAccount).State = EntityState.Modified;
                context.Transactions.Remove(transaction);
                context.SaveChanges();
            }
        }

        #endregion End of Staff Functions

        #region Account Functions

        public void DepositAmount(string bankId, string accountId, double amount, string currencyCode)
        {
            EntityModels.Bank bank = GetBankById(bankId);
            using (var context = new BankDBContext())
            {
                if (currencyCode != "INR")
                {
                    amount *= (double)context.Currencies.Where(i => i.CurrencyCode == currencyCode).FirstOrDefault().ExchangeRate;
                }
                EntityModels.Account account = GetAccountById(accountId);
                account.Balance += (decimal)amount;

                context.Accounts.Attach(account);
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
            
        }

        public void WithdrawAmount(string accountId, double amount)
        {
            EntityModels.Account account = GetAccountById(accountId);
            account.Balance -= (decimal)amount;

            using (var context = new BankDBContext())
            {
                context.Accounts.Attach(account);
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void TransferFunds(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount)
        {
            EntityModels.Bank senderBank = GetBankById(senderBankId);
            EntityModels.Bank recipientBank = GetBankById(recipientBankId);
            EntityModels.Account senderAccount = GetAccountById(senderAccountId);
            EntityModels.Account recipientAccount = GetAccountById(recipientAccountId);
            double serviceCharges = GetServiceCharges(senderBank.BankId, recipientBank.BankId, amount, (double)senderBank.SameRtgs, (double)senderBank.SameImps, (double)senderBank.DiffRtgs, (double)senderBank.DiffImps); ;
            senderAccount.Balance -= (decimal)(amount + serviceCharges);
            recipientAccount.Balance += (decimal)amount;
            CreateTransaction(senderBankId, senderAccountId, recipientBankId, recipientAccountId, amount);
        }

        public List<EntityModels.Transaction> GetTransactions(string accountId)
        {
            EntityModels.Account account = GetAccountById(accountId);
            using (var context = new BankDBContext())
            {
                return context.Transactions.Where(i => i.SenderAccountId == accountId || i.RecipientAccountId == accountId).ToList();
            }
        }

        #endregion End of Account Functions

        #region Service Functions

        public EntityModels.Bank GetBankById(string bankId)
        {
            using (var context = new BankDBContext())
            {
                return context.Banks.Find(bankId);
            }
        }

        public EntityModels.Account GetAccountById(string accountId)
        {
            using (var context = new BankDBContext())
            {
                return context.Accounts.Find(accountId);
            }
        }

        public EntityModels.Transaction GetTransactionById(string transactionId)
        {
            using (var context = new BankDBContext())
            {
                return context.Transactions.Find(transactionId);
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
                return context.Banks.Where(i => i.BankName == bankName).FirstOrDefault().BankId;
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
                EntityModels.Account bankAccount = context.Accounts.Where(i => i.AccountUsername == usernameInput).FirstOrDefault();
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
                return context.Accounts.Where(i => i.BankId == bankId && i.AccountUsername == username).FirstOrDefault().AccountId;
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

        public double GetAccountBalance(string accountId)
        {
            return (double)GetAccountById(accountId).Balance;
        }

        public bool IsTransactionAvailable(string transactionId)
        {
            using (var context = new BankDBContext())
            {
                return context.Transactions.Any(i => i.TransactionId == transactionId);
            }
        }

        public string CreateId(string name)
        {
            return $"{name.Substring(0, 3)}{DateTime.Now}";
        }

        public void CreateTransaction(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount)
        {
            EntityModels.Transaction transaction = new EntityModels.Transaction
            {
                TransactionId = CreateTransactionId(senderBankId, senderAccountId),
                SenderAccountId = senderAccountId,
                RecipientAccountId = recipientAccountId,
                Amount = (decimal)amount,
                TransactionDateTime = DateTime.Now
            };
            
            using (var context = new BankDBContext())
            {
                context.Transactions.Add(transaction);
                context.SaveChanges();
            }
        }

        public string CreateTransactionId(string bankId, string accountId)
        {
            return $"TXN{bankId}{accountId}{DateTime.Now}";
        }

        public double GetServiceCharges(string senderBankId, string receiverBankId, double amount, double sameRTGS, double sameIMPS, double diffRTGS, double diffIMPS)
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
