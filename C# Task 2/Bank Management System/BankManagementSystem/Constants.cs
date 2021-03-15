using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement
{
    public static class Constants
    {
        public static string OutOfBoundsMessage = "Out of bounds. Please enter a number among the given options.";
        public static string OptionSelectionPrompt = "Please select valid option : ";
        public static string PressKeyPrompt = "\nPress any key to continue";
        public static string PasswordPrompt = "Enter Password (Password must have atleast 8 characters, and atleast 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character) : ";

        public static class MainMenu
        {
            public static string WelcomeMessage = "Welcome to Bank Management System\n";
            public static string Menu = "1. Setup New Bank\n2. Login as account holder / bank staff\n3. Exit\n";
            public static string BankNamePrompt = "Enter Bank Name : ";
            public static string StaffUsernamePrompt = "Enter Staff Username : ";
            public static string LoginUsernamePrompt = "Enter Staff / Account Username : ";
            public static string InvalidUsernameMessage = "Invalid username credentials. Please try again.";
            public static string InvalidPasswordMessage = "Password does not match username credentials. Please try again.";
            public static string BankCreationSuccessMessage = "New bank successfully created!";
            public static string BankAvailableMessage = "Bank already available in system. Please try again.";
            public static string BankNotAvailableMessage = "Given bank is not available in system.Please try again with a different bank";
        }

        public static string LogOutMessage = "Successfully logged out";
        public static string AccountNotAvailableMessage = "Account with given ID does not exist. Please try again.";
        public static string CurrencyCodePrompt = "Enter 3-Letter Currency Code : ";

        public static class Staff
        {
            public static string Menu = "1. Create new account\n2. Update account\n3. Delete account\n4. Add new accepted currency\n5. Update service charges\n6. View account transaction history\n7. Revert a transaction\n8. Logout";
            public static string AccountIdPrompt = "Enter account ID : ";
            public static string TransactionIdPrompt = "Enter transacton ID to revert transaction : ";
            public static string RevertTransactionSuccessMessage = "Transaction succesfully reverted!";
            public static string TransactionNotAvailableMessage = "Transaction with given ID does not exist. Please try again.";
            public static string SameRtgsPrompt = "Enter new RTGS percentage for transaction to same bank : ";
            public static string SameImpsPrompt = "Enter new IMPS percentage for transaction to same bank : ";
            public static string DiffRtgsPrompt = "Enter new RTGS percentage for transaction to different bank : ";
            public static string DiffImpsPrompt = "Enter new IMPS percentage for transaction to different bank : ";
            public static string UpdateChargesSuccessMessage = "Service charges successfully updated!";
            public static string CurrencyNamePrompt = "Enter Name of Currency : ";
            public static string ExchangeRatePrompt = "Enter exchange rate with respect to Indian Rupee (INR) : ";
            public static string AddCurrencySuccessMessage = "Currency successfully added!";
            public static string UsernameNotAvailableMessage = "Username already exists. Please try again.";
            public static string DeleteAccountIdPrompt = "Enter account ID to delete : ";
            public static string UpdateAccountIdPrompt = "Enter account ID to update : ";
            public static string UpdateAccountSuccessMessage = "Account details successfully updated!";
            public static string DeleteAccountSuccessMessage = "Account successfully deleted!";
            public static string CreateAccountSuccessMessage = "Account successfuly created!";
            public static string UpdateNamePrompt = "Enter name update : ";
            public static string UpdateUsernamePrompt = "Enter username update : ";
            public static string UpdatePasswordPrompt = "Enter password update : ";
            public static string NewNamePrompt = "Enter name of account holder : ";
            public static string NewUsernamePrompt = "Enter new username : ";
            public static string InitialDepositPrompt = "Enter initial deposit : ";
        }

        public static class Account
        {
            public static string Menu = "1. Deposit amount\n2. Withdraw amount (INR only)\n3. Transfer Funds (INR only)\n4. View Account Transaction History\n5. Logout";
            public static string TransactionTableHeader = $"{"Transaction ",41}{"Sender_Id ",14}{"Sender_Bank ",10}{"Sender_Name ",10}{"Recipient_ID ",14}{"Recipient_Bank ",10}{"Recipient_Name ",10}{"Amount ",8}{"Date_and_Time",21}";
            public static string InsufficientBalanceMessage = "Insufficient balance for action. Please try again.";
            public static string RecipientAccountBankPrompt = "Enter bank name of recipient account : ";
            public static string RecipientAccountIdPrompt = "Enter recipient account ID : ";
            public static string TransferAmountPrompt = "Enter transfer amount : ";
            public static string WithdrawAmountPrompt = "Enter withdraw amount : ";
            public static string DepositAmountPrompt = "Enter deposit Amount : ";
            public static string InvalidCurrencyCode = "Given currency code is incorrect or not supported by bank. Please try again.";
        }
    }
}
