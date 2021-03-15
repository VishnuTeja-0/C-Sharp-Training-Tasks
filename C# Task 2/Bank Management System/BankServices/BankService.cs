using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BankManagement.Contracts;
using BankManagement.Models.EntityModels;

namespace BankManagement.Services
{
    public class BankService : IBankService
    {

        public void CreateBank(string bankName, string staffUsername, string staffPassword)
        {
            Bank bank = new Bank
            {
                BankName = bankName,
                BankId = CreateId(bankName),
                SameRtgs = Constants.sameRTGS,
                SameImps = Constants.sameIMPS,
                DiffRtgs = Constants.diffRTGS,
                DiffImps = Constants.diffIMPS,
                SupportedCurrencies = $"{Constants.currencyCode}, ",
                StaffUsername = staffUsername,
                StaffPassword = staffPassword
            };

            Currency defaultCurrency = new Currency
            {
                CurrencyCode = Constants.currencyCode,
                CurrencyName = Constants.currencyName,
                ExchangeRate = Constants.exchangeRate
            };

            using (var context = new BankDBContext())
            {
                context.Banks.Add(bank);
                context.Currencies.Add(defaultCurrency);
                context.SaveChanges();
            }
        }

        #region Staff Functions

        public void CreateAccount(string bankId, string newName, string newUsername, string newPassword, decimal initialDeposit)
        {
            Account bankAccount = new Account
            {
                AccountId = CreateId(newName),
                BankId = bankId,
                HolderName = newName,
                AccountUsername = newUsername,
                AccountPassword = newPassword,
                Balance = initialDeposit
            };
            
            using (var context = new BankDBContext())
            {
                context.Accounts.Add(bankAccount);
                context.SaveChanges();
            }
        }

        public void UpdateAccount(string accountId, string newName, string newUsername, string newPassword)
        {
            Account account = GetAccountById(accountId);
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
            Account account = GetAccountById(accountId);
            using (var context = new BankDBContext())
            {
                var entry = context.Entry(account);
                context.Accounts.Attach(account);
                context.Accounts.Remove(account);
                context.SaveChanges();
            }
        }

        public void AddCurrency(string bankId, string currencyName, string currencyCode, decimal exchangeRate)
        {
            Currency curr = new Currency
            {
                CurrencyName = currencyName,
                CurrencyCode = currencyCode,
                ExchangeRate = exchangeRate
            };

            Bank bank = GetBankById(bankId);
            bank.SupportedCurrencies += $"{currencyCode}, ";

            using (var context = new BankDBContext())
            {
                context.Banks.Attach(bank);
                context.Entry(bank).State = EntityState.Modified;
                context.Currencies.Add(curr);
                context.SaveChanges();
            }          
        }

        public void UpdateServiceCharges(string bankId, decimal newSameRTGS, decimal newSameIMPS, decimal newDiffRTGS, decimal newDiffIMPS)
        {
            Bank bank = GetBankById(bankId);
            bank.SameRtgs = newSameRTGS;
            bank.SameImps = newSameIMPS;
            bank.DiffRtgs = newDiffRTGS;
            bank.DiffImps = newDiffIMPS;

            using (var context = new BankDBContext())
            {
                context.Banks.Attach(bank);
                context.Entry(bank).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void RevertTransaction(string transactionId)
        {
            Transaction transaction = GetTransactionById(transactionId);
            decimal amount = transaction.Amount;

            Account senderAccount = GetAccountById(transaction.SenderAccountId);
            Account recipientAccount = GetAccountById(transaction.RecipientAccountId);
            Bank senderBank = GetBankById(senderAccount.BankId);
            Bank recipientBank = GetBankById(recipientAccount.BankId);
            decimal serviceCharges = GetServiceCharges(senderBank.BankId, recipientBank.BankId, amount, senderBank.SameRtgs, senderBank.SameImps, senderBank.DiffRtgs, senderBank.DiffImps);
            senderAccount.Balance += (amount + serviceCharges);
            recipientAccount.Balance -= amount;

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

        public void DepositAmount(string bankId, string accountId, decimal amount, string currencyCode)
        {
            Bank bank = GetBankById(bankId);
            using (var context = new BankDBContext())
            {
                if (currencyCode != "INR")
                {
                    amount *= context.Currencies.Where(i => i.CurrencyCode == currencyCode).FirstOrDefault().ExchangeRate;
                }
                Account account = GetAccountById(accountId);
                account.Balance += amount;

                context.Accounts.Attach(account);
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
            
        }

        public void WithdrawAmount(string accountId, decimal amount)
        {
            Account account = GetAccountById(accountId);
            account.Balance -= amount;

            using (var context = new BankDBContext())
            {
                context.Accounts.Attach(account);
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void TransferFunds(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, decimal amount)
        {
            Bank senderBank = GetBankById(senderBankId);
            Bank recipientBank = GetBankById(recipientBankId);
            Account senderAccount = GetAccountById(senderAccountId);
            Account recipientAccount = GetAccountById(recipientAccountId);
            decimal serviceCharges = GetServiceCharges(senderBank.BankId, recipientBank.BankId, amount, (decimal)senderBank.SameRtgs, (decimal)senderBank.SameImps, (decimal)senderBank.DiffRtgs, (decimal)senderBank.DiffImps); ;
            senderAccount.Balance -= (amount + serviceCharges);
            recipientAccount.Balance += amount;
            CreateTransaction(senderBankId, senderAccountId, recipientBankId, recipientAccountId, amount);
        }

        public List<Transaction> GetTransactions(string accountId)
        {
            Account account = GetAccountById(accountId);
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
                return context.Banks.Find(bankId);
            }
        }

        public Account GetAccountById(string accountId)
        {
            using (var context = new BankDBContext())
            {
                return context.Accounts.Find(accountId);
            }
        }

        public Transaction GetTransactionById(string transactionId)
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
                Account bankAccount = context.Accounts.Where(i => i.AccountUsername == usernameInput).FirstOrDefault();
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

        public decimal GetAccountBalance(string accountId)
        {
            return (decimal)GetAccountById(accountId).Balance;
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

        public void CreateTransaction(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, decimal amount)
        {
            Transaction transaction = new Transaction
            {
                TransactionId = CreateTransactionId(senderBankId, senderAccountId),
                SenderAccountId = senderAccountId,
                RecipientAccountId = recipientAccountId,
                Amount = amount,
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
