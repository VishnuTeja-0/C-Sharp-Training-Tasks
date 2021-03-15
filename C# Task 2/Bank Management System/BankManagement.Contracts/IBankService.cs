using System.Collections.Generic;
using BankManagement.Models.EntityModels;

namespace BankManagement.Contracts
{
    public interface IBankService
    {
        public void CreateBank(string bankName, string staffUsername, string staffPassword);

        #region  Staff Functions
        public void CreateAccount(string bankId, string newName, string newUsername, string newPassword, decimal initialDeposit);
        public void UpdateAccount(string accountId, string newName, string newUsername, string newPassword);
        public void DeleteAccount(string bankId, string accountId);
        public void AddCurrency(string bankId, string currencyName, string currencyCode, decimal exchangeRate);
        public void UpdateServiceCharges(string bankId, decimal newSameRTGS, decimal newSameIMPS, decimal newDiffRTGS, decimal newDiffIMPS);
        public void RevertTransaction(string transactionId);
        #endregion End of Staff Functions

        #region Account Functions
        public void DepositAmount(string bankId, string accountId, decimal amount, string currencyCode);
        public void WithdrawAmount(string accountId, decimal amount);
        public void TransferFunds(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, decimal amount);
        public List<Transaction> GetTransactions(string accountId);
        #endregion End of Account Functions

        #region Service Functions
        public Bank GetBankById(string bankId);
        public Account GetAccountById(string accountId);
        public Transaction GetTransactionById(string transactionId);
        public bool IsBankAvailable(string bankName);
        public string GetBankId(string bankName);
        public string GetBankName(string bankId);
        public bool IsStaff(string bankId, string usernameInput);
        public bool IsValidStaffPassword(string bankId, string passwordInput);
        public bool IsAccountHolder(string usernameInput);
        public bool IsValidAccountPassword(string usernameInput, string passwordInput);
        public bool IsAccountAvailable(string bankId, string accountId);
        public string GetAccountId(string bankId, string username);
        public string GetAccountName(string accountId);
        public bool IsCurrencyAvailable(string bankId, string currencyCode);
        public decimal GetAccountBalance(string accountId);
        public bool IsTransactionAvailable(string transactionId);
        public string GenerateId(string name);
        public void CreateTransaction(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, decimal amount);
        public string GenerateTransactionId(string bankId, string accountId);
        public decimal GetServiceCharges(string senderBankId, string receiverBankId, decimal amount, decimal sameRTGS, decimal sameIMPS, decimal diffRTGS, decimal diffIMPS);
        public string GetAccountBank(string accountId);
        #endregion End of Service Functions
    }
}
