
namespace BankManagement.Models
{
    public enum BankOperations
    {
        SetupBank = 1,
        Login,
        Exit
    }

    public enum StaffOperations
    {
        CreateAccount = 1,
        UpdateAccount,
        DeleteAccount,
        AddCurrency,
        UpdateServiceCharges,
        ViewTransactionHistory,
        RevertTransaction,
        Logout
    }

    public enum AccountOperations
    {
        DepositAmount = 1,
        WithdrawAmount,
        TransferFunds,
        ViewTransactionHistory,
        Logout
    }

    public enum TransactionTypes
    {
        Sent,
        Received
    }
}
