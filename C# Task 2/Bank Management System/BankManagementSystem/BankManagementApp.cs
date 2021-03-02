using System;
using System.Collections.Generic;
using BankManagement.Models;
using BankManagement.Services;

namespace BankManagement
{
    public class BankManagementApp
    {
        private BankService _bankService;

        public void DepositAmount(string bankId, string accountId)
        {
            ("Enter 3-letter currency code : ").DisplayLine();
            string code = Helper.GetCurrencyCodeInput();
            if (_bankService.IsCurrencyAvailable(bankId, accountId, code))
            {
                ("Enter deposit Amount : ").DisplayLine();
                double amount = Helper.GetDecimalInput();
                _bankService.DepositAmount(bankId, accountId, amount, code);
                ($"Amount deposited successfully! Current Balance : {_bankService.GetAccountBalance(bankId, accountId)}").DisplayLine();
            }
            else
            {
                ("Given currency code is incorrect or not supported by bank. Please try again.").DisplayLine();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void WithdrawAmount(string bankId, string accountId)
        {
            ("Enter withdraw amount : ").DisplayLine();
            double amount = Helper.GetDecimalInput();
            if (amount <= _bankService.GetAccountBalance(bankId, accountId))
            {
                _bankService.WithdrawAmount(bankId, accountId, amount);
                ($"Amount withdrawn succesfully! Current balance : {_bankService.GetAccountBalance(bankId, accountId)}").DisplayLine();
            }
            else
            {
                ("Insufficient balance for withdrawal. Please try again.").DisplayLine();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void TransferFunds(string bankId, string accountId)
        {
            ("Enter bank name of recipient account").DisplayLine();
            string recipientBankId = SelectBank();
            ("Enter recipient account ID : ").DisplayLine();
            string recipientAccountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(recipientBankId, recipientAccountId))
            {
                ("Enter transfer amount : ").DisplayLine();
                double amount = Helper.GetDecimalInput();
                if (amount <= _bankService.GetAccountBalance(bankId, accountId))
                {
                    _bankService.TransferFunds(bankId, accountId, recipientBankId, recipientAccountId, amount);
                    ($"Amount transferred succesfully! Current balance : {_bankService.GetAccountBalance(bankId, accountId)}").DisplayLine();
                }
                else
                {
                    ("Insufficient balance for transfer. Please try again.").DisplayLine();
                }
            }
            else
            {
                ("No account by given ID exists. Please try again").DisplayLine();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void ViewTransaction(string bankId, string accountId)
        {
            List<Transaction> transactions = _bankService.GetTransactions(bankId, accountId);
            ($"{"Transaction ",41}{"Sender_Id ",14}{"Sender_Bank ",10}{"Sender_Name ",10}{"Recipient_ID ",14}{"Recipient_Bank ",10}{"Recipient_Name ",10}{"Amount ",8}{"Date_and_Time",21}").DisplayLine();
            foreach (Transaction transaction in transactions)
            {
                ($"{transaction.Id,40} ").Display();
                ($"{transaction.SenderId,14} ").Display();
                ($"{_bankService.GetBankName(transaction.SenderBankId),11} ").Display();
                ($"{_bankService.GetAccountName(transaction.SenderBankId, transaction.SenderId),11} ").Display();
                ($"{transaction.ReceiverId,14} ").Display();
                ($"{_bankService.GetBankName(transaction.ReceiverBankId),11} ").Display();
                ($"{_bankService.GetAccountName(transaction.ReceiverBankId, transaction.ReceiverId),11} ").Display();
                ($"{transaction.Amount,8} ").Display();
                ($"{transaction.Time,21} \n").DisplayLine();
            }
        }

        public void ReturnToAccountMenu(string bankId, string accountId)
        {
            ("\nPress any key to continue").DisplayLine();
            Console.ReadKey(false);
            DisplayAccountMenu(bankId, accountId);
        }

        public void DisplayAccountMenu(string bankId, string accountId)
        {
            Console.Clear();
            string bankName = _bankService.GetBankName(bankId);
            string accountName = _bankService.GetAccountName(bankId, accountId);
            ($"Welcome to {bankName} Bank! You are logged in as account holder, {accountName}.\n").DisplayLine();
            ("1. Deposit amount\n2. Withdraw amount (INR only)\n3. Transfer Funds (INR only)").DisplayLine();
            ("4. View Account Transaction History\n5. Logout").DisplayLine();
            ("Please provide valid input from given options : ").DisplayLine();
        }

        public void AccountLoginOptions(string bankId, string accountId)
        {
            DisplayAccountMenu(bankId, accountId);
            bool isLoggedIn = true;
            while (isLoggedIn)
            {
                AccountOperations option = (AccountOperations)Helper.GetNumberInput();
                switch (option)
                {
                    case AccountOperations.DepositAmount:
                        DepositAmount(bankId, accountId);
                        break;
                    case AccountOperations.WithdrawAmount:
                        WithdrawAmount(bankId, accountId);
                        break;
                    case AccountOperations.TransferFunds:
                        TransferFunds(bankId, accountId);
                        break;
                    case AccountOperations.ViewTransactionHistory:
                        ViewTransaction(bankId, accountId);
                        ReturnToAccountMenu(bankId, accountId);
                        break;
                    case AccountOperations.Logout:
                        isLoggedIn = false;
                        break;
                    default:
                        ("Out of bounds. Please enter valid option from menu.").DisplayLine();
                        break;
                }
            }
            ("Successfully logged out").DisplayLine();
        }

        public void CreateAccount(string bankId)
        {
            Console.Clear();
            ("Enter name of account holder : ").DisplayLine();
            string newName = Helper.GetTextInput();
            ("Enter new username : ").DisplayLine();
            string newUsername = Console.ReadLine();
            if (! _bankService.IsAccountHolder(bankId, newUsername))
            {
                ("Enter Staff Password (Password must have atleast 8 characters, and atleast 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character) : ").DisplayLine();
                string newPassword = Helper.GetPasswordInput();
                ("Enter initial deposit : ").DisplayLine();
                double initialDeposit = Helper.GetDecimalInput();
                _bankService.CreateAccount(bankId, newName, newUsername, newPassword, initialDeposit);
                ("Account successfuly created!").DisplayLine();
            }
            else
            {
                ("Username already exists. Please try again.").DisplayLine();
            }
            ReturnToStaffMenu(bankId);
        }

        public void UpdateAccount(string bankId)
        {
            Console.Clear();
            ("Enter account ID to update : ").DisplayLine();
            string accountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(bankId, accountId))
            {
                ("Enter name update : ").DisplayLine();
                string updatedName = Helper.GetTextInput();
                ("Enter username update : ").DisplayLine();
                string updatedUsername = Console.ReadLine();
                if (!_bankService.IsAccountHolder(bankId, updatedUsername))
                {
                    ("Enter Staff Password (Password must have atleast 8 characters, and atleast 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character) : ").DisplayLine();
                    string updatedPassword = Helper.GetPasswordInput();
                    _bankService.UpdateAccount(bankId, accountId, updatedName, updatedUsername, updatedPassword);
                    ("Account details successfully updated!").DisplayLine();
                }
                else
                {
                    ("Username already exists. Please try again.").DisplayLine();
                }

            }
            else
            {
                ("Account with given ID does not exist. Please try again.").DisplayLine();
            }
            ReturnToStaffMenu(bankId);
        }

        public void DeleteAccount(string bankId)
        {
            Console.Clear();
            ("Enter account ID to delete : ").DisplayLine();
            string accountId = Console.ReadLine();
            if(_bankService.IsAccountAvailable(bankId, accountId))
            {
                _bankService.DeleteAccount(bankId, accountId);
            }
            else
            {
                ("Account with given ID does not exist. Please try again.").DisplayLine();
            }
            ReturnToStaffMenu(bankId);
        }

        public void AddCurrency(string bankId)
        {
            Console.Clear();
            ("Enter Name of Currency : ").DisplayLine();
            string name = Helper.GetTextInput();
            ("Enter 3-Letter Currency Code : ").DisplayLine();
            string code = Helper.GetCurrencyCodeInput();
            ("Enter exchange rate with respect to Indian Rupee (INR)").DisplayLine();
            double exchangeRate = Helper.GetDecimalInput();
            _bankService.AddCurrency(bankId, name, code, exchangeRate);
            ("Currency successfully added!").Display();
            ReturnToStaffMenu(bankId);
        }

        public void UpdateCharges(string bankId)
        {
            Console.Clear();
            ("Enter new RTGS percentage for transaction to same bank : ").DisplayLine();
            double newSameRTGS = Helper.GetNumberInput() / 100;
            ("Enter new IMPS percentage for transaction to same bank : ").DisplayLine();
            double newSameIMPS = Helper.GetNumberInput() / 100;
            ("Enter new RTGS percentage for transaction to different bank : ").DisplayLine();
            double newDiffRTGS = Helper.GetNumberInput() / 100;
            ("Enter new IMPS percentage for transaction to different bank : ").DisplayLine();
            double newDiffIMPS = Helper.GetNumberInput() / 100;
            _bankService.UpdateServiceCharges(bankId, newSameRTGS, newSameIMPS, newDiffRTGS, newDiffIMPS);
            ("Service charges successfully updated!").DisplayLine();
            ReturnToStaffMenu(bankId);
        }

        public void RevertTransaction(string bankId)
        {
            ("Enter account ID : ").DisplayLine();
            string accountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(bankId, accountId))
            {
                ("Enter transacton ID to revert transaction : ").DisplayLine();
                string transactionId = Console.ReadLine();
                if (_bankService.IsTransactionAvailable(bankId, accountId, transactionId))
                {
                    _bankService.RevertTransaction(bankId, accountId, transactionId);
                    ("Transaction succesfully reverted").DisplayLine();
                }
                else
                {
                    ("Transaction with given ID does not exist. Please try again.").DisplayLine();
                }
            }
            else
            {
                ("Account with given ID does not exist. Please try again.").DisplayLine();
            }
            ReturnToStaffMenu(bankId);
        }

        public void ReturnToStaffMenu(string bankId)
        {
            ("\nPress any key to continue").DisplayLine();
            Console.ReadKey(false);
            DisplayStaffMenu(bankId);
        }

        public void DisplayStaffMenu(string bankId)
        {
            Console.Clear();
            string bankName = _bankService.GetBankName(bankId);
            ($"Welcome to {bankName} Bank! You are logged in as Bank Staff.\n").DisplayLine();
            ("1. Create new account\n2. Update account\n3. Delete account\n4. Add new accepted currency\n5. Update service charges").DisplayLine();
            ("6. View account transaction history\n7. Revert a transaction\n8. Logout").DisplayLine();
            ("Please provide valid input from given options : ").DisplayLine();
        }

        public void StaffLoginOptions(string bankId)
        {
            DisplayStaffMenu(bankId);
            bool isLoggedIn = true;
            while (isLoggedIn)
            {
                StaffOperations option = (StaffOperations)Helper.GetNumberInput();
                switch (option)
                {
                    case StaffOperations.CreateAccount:
                        CreateAccount(bankId);
                        break;
                    case StaffOperations.UpdateAccount:
                        UpdateAccount(bankId);
                        break;
                    case StaffOperations.DeleteAccount:
                        DeleteAccount(bankId);
                        break;
                    case StaffOperations.AddCurrency:
                        AddCurrency(bankId);
                        break;
                    case StaffOperations.UpdateServiceCharges:
                        UpdateCharges(bankId);
                        break;
                    case StaffOperations.ViewTransactionHistory:
                        ("Enter account ID : ").DisplayLine();
                        string accountId = Console.ReadLine();
                        ViewTransaction(bankId, accountId);
                        ReturnToStaffMenu(bankId);
                        break;
                    case StaffOperations.RevertTransaction:
                        RevertTransaction(bankId);
                        break;
                    case StaffOperations.Logout:
                        isLoggedIn = false;
                        break;
                    default:
                        ("Out of bounds. Please enter valid option from menu.").DisplayLine();
                        break;
                }
            }
            ("Successfully logged out").DisplayLine();
        }

        public string SelectBank()
        {
            while (true)
            {
                string bankName = Helper.GetTextInput();
                if (_bankService.IsBankAvailable(bankName))
                {
                    return _bankService.GetBankId(bankName);
                }
                else
                {
                    ("Given bank is not available in system. Please try again with a different bank").DisplayLine();
                }
            }      
        }

        public void Login()
        {
            ("Enter bank name : ").DisplayLine();
            string bankId = SelectBank();
            ("Enter Staff / Account Username : ").DisplayLine();
            string username = Console.ReadLine();
            if (_bankService.IsStaff(bankId, username))
            {
                ("Enter Password : ").DisplayLine();
                while (true)
                {
                    string password = Console.ReadLine();
                    if (_bankService.IsValidStaffPassword(bankId, password))
                    {

                        StaffLoginOptions(bankId);
                        break;
                    }
                    else
                    {
                        ("Password does not match username credentials. Please try again.").DisplayLine();
                    }
                }    
            }
            else if(_bankService.IsAccountHolder(bankId, username))
            {
                ("Enter Password : ").DisplayLine();
                while (true)
                {
                    string password = Console.ReadLine();
                    if (_bankService.IsValidAccountPassword(bankId, username, password))
                    {
                        string accountId = _bankService.GetAccountId(bankId, username);
                        AccountLoginOptions(bankId, accountId);
                        break;
                    }
                    else
                    {
                        ("Password does not match username credentials. Please try again.").DisplayLine();
                    }
                }
            }
            else
            {
                ("Invalid username credentials. Please try again.").DisplayLine();
            }
            ReturnToMainMenu();
        }

        public void SetupBank()
        {
            ("Enter Bank Name : ").DisplayLine();
            string bankName = Helper.GetTextInput();
            if (!_bankService.IsBankAvailable(bankName))
            {
                ("Enter Staff Username : ").DisplayLine();
                string staffUsername = Console.ReadLine();
                ("Enter Staff Password (Password must have atleast 8 characters, and atleast 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character) : ").DisplayLine();
                string staffPassword = Helper.GetPasswordInput();
                _bankService.CreateBank(bankName, staffUsername, staffPassword);
                ("New bank successfully created!").DisplayLine();
            }
            else
            {
                ("Bank already available in system. Please try again.").DisplayLine();
            }
            ReturnToMainMenu();
        }

        public void ReturnToMainMenu()
        {
            ("\nPress any key to continue").DisplayLine();
            Console.ReadKey(false);
            DisplayMenu();
        }

        public void DisplayMenu()
        {
            Console.Clear();
            ("Welcome to Bank Management System\n").DisplayLine();
            ("1. Setup New Bank\n2. Login as account holder / bank staff\n3. Exit\n").DisplayLine();
            ("Please select valid option : ").DisplayLine();
        }

        public void MainMenu()
        {
            DisplayMenu();
            while (true)
            {
                BankOperations option = (BankOperations)Helper.GetNumberInput();
                switch (option)
                {
                    case BankOperations.SetupBank:
                        SetupBank();
                        break;
                    case BankOperations.Login:
                        Login();
                        break;
                    case BankOperations.Exit:
                        Environment.Exit(0);
                        break;
                    default:
                        ("Out of bounds. Please enter a number among the given options.").DisplayLine();
                        break;
                }
            }          

        }

        public void Startup()
        {
            _bankService = new BankService();
            MainMenu();
        }
    }
}
