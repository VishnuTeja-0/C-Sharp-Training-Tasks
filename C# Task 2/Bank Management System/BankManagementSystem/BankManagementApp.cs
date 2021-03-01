using System;
using System.Collections.Generic;
using BankManagement.Models;
using BankManagement.Services;

namespace BankManagement
{
    public class BankManagementApp
    {
        private BankService _bankService;

        public void DepositRequest(string bankId, string accountId)
        {
            ("Enter 3-letter currency code : ").Display();
            string code = Helper.GetCurrencyCodeInput();
            if (_bankService.IsCurrencyAvailable(bankId, accountId, code))
            {
                ("Enter deposit Amount : ").Display();
                double amount = Helper.GetDecimalInput();
                _bankService.DepositAmount(bankId, accountId, amount, code);
                ($"Amount deposited successfully! Current Balance : {_bankService.GetAccountBalance(bankId, accountId)}").Display();
            }
            else
            {
                ("Given currency code is incorrect or not supported by bank. Please try again.").Display();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void WithdrawRequest(string bankId, string accountId)
        {
            ("Enter withdraw amount : ").Display();
            double amount = Helper.GetDecimalInput();
            if (amount <= _bankService.GetAccountBalance(bankId, accountId))
            {
                _bankService.WithdrawAmount(bankId, accountId, amount);
                ($"Amount deposited succesfully! Current balance : {_bankService.GetAccountBalance(bankId, accountId)}").Display();
            }
            else
            {
                ("Insufficient balance for withdrawal. Please try again.").Display();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void TransferRequest(string bankId, string accountId)
        {
            ("Enter bank name of recipient account").Display();
            string recipientBankId = SelectBank();
            ("Enter recipient account ID : ").Display();
            string recipientAccountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(recipientBankId, recipientAccountId))
            {
                ("Enter transfer amount : ").Display();
                double amount = Helper.GetDecimalInput();
                if (amount <= _bankService.GetAccountBalance(bankId, accountId))
                {
                    _bankService.TransferFunds(bankId, accountId, recipientBankId, recipientAccountId, amount);
                    ($"Amount transferred succesfully! Current balance : {_bankService.GetAccountBalance(bankId, accountId)}").Display();
                }
                else
                {
                    ("Insufficient balance for transfer. Please try again.").Display();
                }
            }
            else
            {
                ("No account by given ID exists. Please try again").Display();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void ViewTransactionRequest(string bankId, string accountId)
        {
            List<Transaction> transactions = _bankService.GetTransactions(bankId, accountId);
            ($"{"Transaction ",41}{"Sender_Id ",14}{"Sender_Bank ",10}{"Sender_Name ",10}{"Recipient_ID ",14}{"Recipient_Bank ",10}{"Recipient_Name ",10}{"Amount ",8}{"Date_and_Time",21}").Display();
            foreach (Transaction transaction in transactions)
            {
                Console.Write($"{transaction.Id,40} ");
                Console.Write($"{transaction.SenderId,14} ");
                Console.Write($"{_bankService.GetBankName(transaction.SenderBankId),11} ");
                Console.Write($"{_bankService.GetAccountName(transaction.SenderBankId, transaction.SenderId),11} ");
                Console.Write($"{transaction.ReceiverId,14} ");
                Console.Write($"{_bankService.GetBankName(transaction.ReceiverBankId),11} ");
                Console.Write($"{_bankService.GetAccountName(transaction.ReceiverBankId, transaction.ReceiverId),11} ");
                Console.Write($"{transaction.Amount,8} ");
                ($"{transaction.Time,21} \n").Display();
            }
        }

        public void ReturnToAccountMenu(string bankId, string accountId)
        {
            ("\nPress any key to continue").Display();
            Console.ReadKey(false);
            DisplayAccountMenu(bankId, accountId);
        }

        public void DisplayAccountMenu(string bankId, string accountId)
        {
            Console.Clear();
            string bankName = _bankService.GetBankName(bankId);
            string accountName = _bankService.GetAccountName(bankId, accountId);
            ($"Welcome to {bankName} Bank! You are logged in as account holder, {accountName}.\n").Display();
            ("1. Deposit amount\n2. Withdraw amount (INR only)\n3. Transfer Funds (INR only)").Display();
            ("4. View Account Transaction History\n5. Logout").Display();
            ("Please provide valid input from given options : ").Display();
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
                        DepositRequest(bankId, accountId);
                        break;
                    case AccountOperations.WithdrawAmount:
                        WithdrawRequest(bankId, accountId);
                        break;
                    case AccountOperations.TransferFunds:
                        TransferRequest(bankId, accountId);
                        break;
                    case AccountOperations.ViewTransactionHistory:
                        ViewTransactionRequest(bankId, accountId);
                        ReturnToAccountMenu(bankId, accountId);
                        break;
                    case AccountOperations.Logout:
                        isLoggedIn = false;
                        break;
                    default:
                        ("Out of bounds. Please enter valid option from menu.").Display();
                        break;
                }
            }
            ("Successfully logged out").Display();
        }

        public void CreateAccountRequest(string bankId)
        {
            Console.Clear();
            ("Enter name of account holder : ").Display();
            string newName = Helper.GetTextInput();
            ("Enter new username : ").Display();
            string newUsername = Console.ReadLine();
            if (! _bankService.IsAccountHolder(bankId, newUsername))
            {
                ("Enter new password : ").Display();
                string newPassword = Console.ReadLine();
                ("Enter initial deposit : ").Display();
                double initialDeposit = Helper.GetDecimalInput();
                _bankService.CreateAccount(bankId, newName, newUsername, newPassword, initialDeposit);
                ("Account successfuly created!").Display();
            }
            else
            {
                ("Username already exists. Please try again.").Display();
            }
            ReturnToStaffMenu(bankId);
        }

        public void UpdateAccountRequest(string bankId)
        {
            Console.Clear();
            ("Enter account ID to update : ").Display();
            string accountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(bankId, accountId))
            {
                ("Enter name update : ").Display();
                string updatedName = Helper.GetTextInput();
                ("Enter username update : ").Display();
                string updatedUsername = Console.ReadLine();
                if (!_bankService.IsAccountHolder(bankId, updatedUsername))
                {
                    ("Enter password update : ").Display();
                    string updatedPassword = Console.ReadLine();
                    _bankService.UpdateAccount(bankId, accountId, updatedName, updatedUsername, updatedPassword);
                    ("Account details successfully updated!").Display();
                }
                else
                {
                    ("Username already exists. Please try again.").Display();
                }

            }
            else
            {
                ("Account with given ID does not exist. Please try again.").Display();
            }
            ReturnToStaffMenu(bankId);
        }

        public void DeleteAccountRequest(string bankId)
        {
            Console.Clear();
            ("Enter account ID to delete : ").Display();
            string accountId = Console.ReadLine();
            if(_bankService.IsAccountAvailable(bankId, accountId))
            {
                _bankService.DeleteAccount(bankId, accountId);
            }
            else
            {
                ("Account with given ID does not exist. Please try again.").Display();
            }
            ReturnToStaffMenu(bankId);
        }

        public void AddCurrencyRequest(string bankId)
        {
            Console.Clear();
            ("Enter Name of Currency : ").Display();
            string name = Helper.GetTextInput();
            ("Enter 3-Letter Currency Code : ").Display();
            string code = Helper.GetCurrencyCodeInput();
            ("Enter exchange rate with respect to Indian Rupee (INR)").Display();
            double exchangeRate = Helper.GetDecimalInput();
            _bankService.AddCurrency(bankId, name, code, exchangeRate);
            Console.Write("Currency successfully added!");
            ReturnToStaffMenu(bankId);
        }

        public void UpdateChargesRequest(string bankId)
        {
            Console.Clear();
            ("Enter new RTGS percentage for transaction to same bank : ").Display();
            double newSameRTGS = Helper.GetNumberInput() / 100;
            ("Enter new IMPS percentage for transaction to same bank : ").Display();
            double newSameIMPS = Helper.GetNumberInput() / 100;
            ("Enter new RTGS percentage for transaction to different bank : ").Display();
            double newDiffRTGS = Helper.GetNumberInput() / 100;
            ("Enter new IMPS percentage for transaction to different bank : ").Display();
            double newDiffIMPS = Helper.GetNumberInput() / 100;
            _bankService.UpdateServiceCharges(bankId, newSameRTGS, newSameIMPS, newDiffRTGS, newDiffIMPS);
            ("Service charges successfully updated!").Display();
            ReturnToStaffMenu(bankId);
        }

        public void RevertTransactionRequest(string bankId)
        {
            ("Enter account ID : ").Display();
            string accountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(bankId, accountId))
            {
                ("Enter transacton ID to revert transaction : ").Display();
                string transactionId = Console.ReadLine();
                if (_bankService.IsTransactionAvailable(bankId, accountId, transactionId))
                {
                    _bankService.RevertTransaction(bankId, accountId, transactionId);
                    ("Transaction succesfully reverted").Display();
                }
                else
                {
                    ("Transaction with given ID does not exist. Please try again.").Display();
                }
            }
            else
            {
                ("Account with given ID does not exist. Please try again.").Display();
            }
            ReturnToStaffMenu(bankId);
        }

        public void ReturnToStaffMenu(string bankId)
        {
            ("\nPress any key to continue").Display();
            Console.ReadKey(false);
            DisplayStaffMenu(bankId);
        }

        public void DisplayStaffMenu(string bankId)
        {
            Console.Clear();
            string bankName = _bankService.GetBankName(bankId);
            ($"Welcome to {bankName} Bank! You are logged in as Bank Staff.\n").Display();
            ("1. Create new account\n2. Update account\n3. Delete account\n4. Add new accepted currency\n5. Update service charges").Display();
            ("6. View account transaction history\n7. Revert a transaction\n8. Logout").Display();
            ("Please provide valid input from given options : ").Display();
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
                        CreateAccountRequest(bankId);
                        break;
                    case StaffOperations.UpdateAccount:
                        UpdateAccountRequest(bankId);
                        break;
                    case StaffOperations.DeleteAccount:
                        DeleteAccountRequest(bankId);
                        break;
                    case StaffOperations.AddCurrency:
                        AddCurrencyRequest(bankId);
                        break;
                    case StaffOperations.UpdateServiceCharges:
                        UpdateChargesRequest(bankId);
                        break;
                    case StaffOperations.ViewTransactionHistory:
                        ("Enter account ID : ").Display();
                        string accountId = Console.ReadLine();
                        ViewTransactionRequest(bankId, accountId);
                        ReturnToStaffMenu(bankId);
                        break;
                    case StaffOperations.RevertTransaction:
                        RevertTransactionRequest(bankId);
                        break;
                    case StaffOperations.Logout:
                        isLoggedIn = false;
                        break;
                    default:
                        ("Out of bounds. Please enter valid option from menu.").Display();
                        break;
                }
            }
            ("Successfully logged out").Display();
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
                    ("Given bank is not available in system. Please try again with a different bank").Display();
                }
            }      
        }

        public void Login()
        {
            ("Enter bank name : ").Display();
            string bankId = SelectBank();
            ("Enter Staff / Account Username : ").Display();
            string username = Console.ReadLine();
            if (_bankService.IsStaff(bankId, username))
            {
                ("Enter Password : ").Display();
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
                        ("Password does not match username credentials. Please try again.").Display();
                    }
                }    
            }
            else if(_bankService.IsAccountHolder(bankId, username))
            {
                ("Enter Password : ").Display();
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
                        ("Password does not match username credentials. Please try again.").Display();
                    }
                }
            }
            else
            {
                ("Invalid username credentials. Please try again.").Display();
            }
            ReturnToMainMenu();
        }

        public void SetupBank()
        {
            ("Enter Bank Name : ").Display();
            string bankName = Helper.GetTextInput();
            if (!_bankService.IsBankAvailable(bankName))
            {
                ("Enter Staff Username : ").Display();
                string staffUsername = Console.ReadLine();
                ("Enter Staff Password : ").Display();
                string staffPassword = Console.ReadLine();
                _bankService.CreateBank(bankName, staffUsername, staffPassword);
                ("New bank successfully created!").Display();
            }
            else
            {
                ("Bank already available in system. Please try again.").Display();
            }
            ReturnToMainMenu();
        }

        public void ReturnToMainMenu()
        {
            ("\nPress any key to continue").Display();
            Console.ReadKey(false);
            DisplayMenu();
        }

        public void DisplayMenu()
        {
            Console.Clear();
            ("Welcome to Bank Management System\n").Display();
            ("1. Setup New Bank\n2. Login as account holder / bank staff\n3. Exit\n").Display();
            ("Please select valid option : ").Display();
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
                        ("Out of bounds. Please enter a number among the given options.").Display();
                        break;
                }
            }          

        }

        public void Startup()
        {
            BankService service = new BankService();
            _bankService = service;
            MainMenu();
        }
    }
}
