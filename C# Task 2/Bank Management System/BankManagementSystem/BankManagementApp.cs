using System;
using System.Collections.Generic;
using System.Linq;
using BankManagement.Services;
using static BankManagement.EnumsClass;

namespace BankManagement
{
    class BankManagementApp
    {
        private BankService _bankService;

        public void DepositRequest(string bankId, string accountId)
        {
            Console.WriteLine("Enter 3-letter currency code : ");
            string code = Helper.GetCurrencyCodeInput();
            if (_bankService.IsCurrencyAvailable(bankId, accountId, code))
            {
                Console.WriteLine("Enter deposit Amount : ");
                double amount = Helper.GetDecimalInput();
                _bankService.DepositAmount(bankId, accountId, amount, code);
                Console.WriteLine($"Amount deposited successfully! Current Balance : {_bankService.GetAccountBalance(bankId, accountId)}");
            }
            else
            {
                Console.WriteLine("Given currency code is incorrect or not supported by bank. Please try again.");
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void WithdrawRequest(string bankId, string accountId)
        {
            Console.WriteLine("Enter withdraw amount : ");
            double amount = Helper.GetDecimalInput();
            if (amount <= _bankService.GetAccountBalance(bankId, accountId))
            {
                _bankService.WithdrawAmount(bankId, accountId, amount);
                Console.WriteLine($"Amount deposited succesfully! Current balance : {_bankService.GetAccountBalance(bankId, accountId)}");
            }
            else
            {
                Console.WriteLine("Insufficient balance for withdrawal. Please try again.");
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void TransferRequest(string bankId, string accountId)
        {

        }

        public void ReturnToAccountMenu(string bankId, string accountId)
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey(false);
            DisplayAccountMenu(bankId, accountId);
        }

        public void DisplayAccountMenu(string bankId, string accountId)
        {
            Console.Clear();
            string bankName = _bankService.GetBanks(bankId);
            string accountName = _bankService.GetAccountName(bankId, accountId);
            Console.WriteLine($"Welcome to {bankName} Bank! You are logged in as account holder, {accountName}.\n");
            Console.WriteLine("1. Deposit amount\n2. Withdraw amount (INR only)\n3. Transfer Funds (INR only)\n");
            Console.WriteLine("4. View Account Transaction History\n5. Logout");
            Console.WriteLine("Please provide valid input from given options : ");
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
                        break;
                    case AccountOperations.Logout:
                        isLoggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Out of bounds. Please enter valid option from menu.");
                        break;
                }
            }
            Console.WriteLine("Successfully logged out");
        }

        public void CreateAccountRequest(string bankId)
        {
            Console.Clear();
            Console.WriteLine("Enter name of account holder : ");
            string newName = Helper.GetTextInput();
            Console.WriteLine("Enter new username : ");
            string newUsername = Console.ReadLine();
            if (! _bankService.IsAccountHolder(bankId, newUsername))
            {
                Console.WriteLine("Enter new password : ");
                string newPassword = Console.ReadLine();
                _bankService.CreateAccount(bankId, newName, newUsername, newPassword);
                Console.WriteLine("Account successfuly created!");
            }
            else
            {
                Console.WriteLine("Username already exists. Please try again.");
            }
            ReturnToStaffMenu(bankId);
        }

        public void UpdateAccountRequest(string bankId)
        {
            Console.Clear();
            Console.WriteLine("Enter account ID to update : ");
            string accountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(bankId, accountId))
            {
                Console.WriteLine("Enter name update : ");
                string updatedName = Helper.GetTextInput();
                Console.WriteLine("Enter username update : ");
                string updatedUsername = Console.ReadLine();
                if (!_bankService.IsAccountHolder(bankId, updatedUsername))
                {
                    Console.WriteLine("Enter password update : ");
                    string updatedPassword = Console.ReadLine();
                    _bankService.UpdateAccount(bankId, accountId, updatedName, updatedUsername, updatedPassword);
                    Console.WriteLine("Account details successfully updated!");
                }
                else
                {
                    Console.WriteLine("Username already exists. Please try again.");
                }

            }
            else
            {
                Console.WriteLine("Account with given ID does not exist. Please try again.");
            }
            ReturnToStaffMenu(bankId);
        }

        public void DeleteAccountRequest(string bankId)
        {
            Console.Clear();
            Console.WriteLine("Enter account ID to delete : ");
            string accountId = Console.ReadLine();
            if(_bankService.IsAccountAvailable(bankId, accountId))
            {
                _bankService.DeleteAccount(bankId, accountId);
            }
            else
            {
                Console.WriteLine("Account with given ID does not exist. Please try again.");
            }
            ReturnToStaffMenu(bankId);
        }

        public void AddCurrencyRequest(string bankId)
        {
            Console.Clear();
            Console.WriteLine("Enter Name of Currency : ");
            string name = Helper.GetTextInput();
            Console.WriteLine("Enter 3-Letter Currency Code : ");
            string code = Helper.GetCurrencyCodeInput();
            Console.WriteLine("Enter exchange rate with respect to Indian Rupee (INR)");
            double exchangeRate = Helper.GetDecimalInput();
            _bankService.AddCurrency(bankId, name, code, exchangeRate);
            Console.Write("Currency successfully added!");
            ReturnToStaffMenu(bankId);
        }

        public void UpdateChargesRequest(string bankId)
        {
            Console.Clear();
            Console.WriteLine("Enter new RTGS percentage for transaction to same bank : ");
            double newSameRTGS = Helper.GetNumberInput() / 100;
            Console.WriteLine("Enter new IMPS percentage for transaction to same bank : ");
            double newSameIMPS = Helper.GetNumberInput() / 100;
            Console.WriteLine("Enter new RTGS percentage for transaction to different bank : ");
            double newDiffRTGS = Helper.GetNumberInput() / 100;
            Console.WriteLine("Enter new IMPS percentage for transaction to different bank : ");
            double newDiffIMPS = Helper.GetNumberInput() / 100;
            _bankService.UpdateServiceCharges(bankId, newSameRTGS, newSameIMPS, newDiffRTGS, newDiffIMPS);
            Console.WriteLine("Service charges successfully updated!");
            ReturnToStaffMenu(bankId);
        }

        public void ReturnToStaffMenu(string bankId)
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey(false);
            DisplayStaffMenu(bankId);
        }

        public void DisplayStaffMenu(string bankId)
        {
            Console.Clear();
            string bankName = _bankService.GetBanks(bankId);
            Console.WriteLine($"Welcome to {bankName} Bank! You are logged in as Bank Staff.\n");
            Console.WriteLine("1. Create new account\n2. Update account\n3. Delete account\n4. Add new accepted currency\n5. Update service charges");
            Console.WriteLine("6. View account transaction history\n7. Revert a transaction\n8. Logout");
            Console.WriteLine("Please provide valid input from given options : ");
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
                        ViewTransactionRequest(bankId);
                        break;
                    case StaffOperations.RevertTransaction:
                        RevertTransactionRequest(bankId);
                        break;
                    case StaffOperations.Logout:
                        isLoggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Out of bounds. Please enter valid option from menu.");
                        break;
                }
            }
            Console.WriteLine("Successfully logged out");
        }

        public string SelectBank()
        {
            Console.WriteLine("Select Bank\n");
            List<string> bankNames = _bankService.GetBanks();
            for (int i = 0; i < bankNames.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {bankNames[i]}\n");
            }
            Console.WriteLine("Please enter valid menu option : ");
            int bankOption = Helper.GetNumberInput();
            return _bankService.GetBankId(bankOption);
        }

        public void Login()
        {
            string bankId = SelectBank();
            Console.WriteLine("Enter Staff / Account Username : ");
            string username = Console.ReadLine();
            if (_bankService.IsStaff(bankId, username))
            {
                Console.WriteLine("Enter Password : ");
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
                        Console.WriteLine("Password does not match username credentials. Please try again.");
                    }
                }    
            }
            else if(_bankService.IsAccountHolder(bankId, username))
            {
                Console.WriteLine("Enter Password : ");
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
                        Console.WriteLine("Password does not match username credentials. Please try again.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid username credentials. Please try again.");
            }
            ReturnToMainMenu();
        }

        public void SetupBank()
        {
            Console.WriteLine("Enter Bank Name: ");
            string bankName = Helper.GetTextInput();
            if (!_bankService.IsBankAvailable(bankName))
            {
                Console.WriteLine("Enter Staff Username");
                string staffUsername = Console.ReadLine();
                Console.WriteLine("Enter Staff Password");
                string staffPassword = Console.ReadLine();
                _bankService.CreateBank(bankName, staffUsername, staffPassword);
                Console.WriteLine("New bank successfully created!");
            }
            else
            {
                Console.WriteLine("Bank already available in system. Please try again.");
            }
            ReturnToMainMenu();
        }

        public void ReturnToMainMenu()
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey(false);
            DisplayMenu();
        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Bank Management System\n");
            Console.WriteLine("1. Setup New Bank\n2. Login as account holder / bank staff\n3. Exit\n");
            Console.WriteLine("Please select valid option : ");
        }

        

        public void MainMenu()
        {
            while (true)
            {
                DisplayMenu();
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
                        Console.WriteLine("Out of bounds. Please enter a number among the given options.");
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
