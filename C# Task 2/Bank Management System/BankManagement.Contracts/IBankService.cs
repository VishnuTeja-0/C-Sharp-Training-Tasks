using System.Collections.Generic;
using BankManagement.Models;

namespace BankManagement.Contracts
{
    public interface IBankService
    {
        public void CreateBank(string bankName, string staffUsername, string staffPassword);

        #region  Staff Functions
        public void CreateAccount(string bankId, string newName, string newUsername, string newPassword, double initialDeposit);
        public void UpdateAccount(string bankId, string accountId, string newName, string newUsername, string newPassword);
        public void DeleteAccount(string bankId, string accountId);
        public void AddCurrency(string bankId, string currencyName, string currencyCode, double exchangeRate);
        public void UpdateServiceCharges(string bankId, double newSameRTGS, double newSameIMPS, double newDiffRTGS, double newDiffIMPS);
        public void RevertTransaction(string bankId, string accountId, string transactionId);
        #endregion End of Staff Functions

        #region Account Functions
        public void DepositAmount(string bankId, string accountId, double amount, string currencyCode);
        public void WithdrawAmount(string bankId, string accountId, double amount);
        public void TransferFunds(string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount);
        public List<Transaction> GetTransactions(string bankId, string accountId);
        #endregion End of Account Functions

        #region Service Functions
        public Bank GetBankById(string bankId);
        public BankAccount GetAccountById(string bankId, string accountId);
        public bool IsBankAvailable(string bankName);
        public string GetBankId(string bankName);
        public string GetBankName(string bankId);
        public bool IsStaff(string bankId, string usernameInput);
        public bool IsValidStaffPassword(string bankId, string passwordInput);
        public bool IsAccountHolder(string bankId, string usernameInput);
        public bool IsValidAccountPassword(string bankId, string usernameInput, string passwordInput);
        public bool IsAccountAvailable(string bankId, string accountId);
        public string GetAccountId(string bankId, string username);
        public string GetAccountName(string bankId, string accountId);
        public bool IsCurrencyAvailable(string bankId, string accountId, string currencyCode);
        public double GetAccountBalance(string bankId, string accountId);
        public bool IsTransactionAvailable(string bankId, string accountId, string transactionId);
        public string CreateId(string name);
        public void CreateTransaction(TransactionTypes type, string bankId, string accountId, string senderBankId, string senderAccountId, string recipientBankId, string recipientAccountId, double amount);
        public string CreateTransactionId(string bankId, string accountId);
        public double GetServiceCharges(string senderBankId, string receiverBankId, double amount, double sameRTGS, double sameIMPS, double diffRTGS, double diffIMPS);
        #endregion End of Staff Functions
    }
}
