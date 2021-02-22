using System;
using BankManagement.Services;

namespace BankManagement
{
    class BankManagementApp
    {
        private BankService _bankService;

        public void CreateAccountRequest(int bankOption)
        {
            string bankName = _bankService.GetBankName(bankOption);
            Console.WriteLine("Enter new username : ");
            string newUsername = Console.ReadLine();
            if (! _bankService.IsAccountHolder(bankName, newUsername))
            {
                Console.WriteLine("Enter new password : ");
                string newPassword = Console.ReadLine();
                _bankService.CreateAccount(bankName, newUsername, newPassword);
            }
            else
            {
                Console.WriteLine("Username already exists. Please try again.");
            }
        }

        public void UpdateAccountRequest(int bankOption)
        {

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

        public void StaffLoginOptions(int bankOption)
        {
            Console.Clear();
            bool isLoggedIn = true;
            string bankName = _bankService.GetName(bankOption);
            Console.WriteLine($"Welcome to {bankName} Bank! You are logged in as Bank Staff.\n");
            Console.WriteLine("1. Create new account\n2. Update account\n3. Delete account\n4. Add new accepted currency\n5. Update service charges");
            Console.WriteLine("6. View account transaction history\n7. Revert a transaction\n8. Logout");
            Console.WriteLine("Please provide valid input from given options : ");
            while (isLoggedIn)
            {
                StaffOperations option = (StaffOperations)Helper.ValidateNumberInput();
                switch (option)
                {
                    case StaffOperations.CreateAccount:
                        CreateAccountRequest(bankOption);
                        break;
                    case StaffOperations.UpdateAccount:
                        UpdateAccountRequest(bankOption);
                        break;
                    case StaffOperations.DeleteAccount:
                        DeleteAccountRequest(bankOption);
                        break;
                    case StaffOperations.AddCurrency:
                        AddCurrencyRequest(bankOption);
                        break;
                    case StaffOperations.UpdateServiceCharges:
                        UpdateChargesRequest(bankOption);
                        break;
                    case StaffOperations.ViewTransactionHistory:
                        ViewTransactionRequest(bankOption);
                        break;
                    case StaffOperations.RevertTransaction:
                        RevertTransactionRequest(bankOption);
                        break;
                    case StaffOperations.Logout:
                        isLoggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Out of bounds. Please enter valid option from menu.");
                        break;
                }
            }
        }

        public void Login()
        {
            Console.WriteLine("Select Bank\n");
            
            int bankOption = Helper.ValidateNumberInput();
            Console.WriteLine("Enter Staff / Account Username : ");
            string username = Console.ReadLine();
            if (_bankService.IsStaff(bankOption, username))
            {
                Console.WriteLine("Enter Password : ");
                while (true)
                {
                    string password = Console.ReadLine();
                    if (_bankService.IsValidStaffPassword(bankOption, password))
                    {

                        StaffLoginOptions(bankOption);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Password does not match username credentials. Please try again.");
                    }
                }    
            }
            else if(_bankService.IsAccountHolder(bankOption, username))
            {
                Console.WriteLine("Enter Password : ");
                while (true)
                {
                    string password = Console.ReadLine();
                    if (_bankService.IsValidAccountPassword(bankOption, username, password))
                    {
                        AccountLoginOptions(bankOption);
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
        }

        public void SetupBank()
        {
            Console.WriteLine("Enter Bank Name: ");
            string bankName = Helper.ValidateTextInput();
            if (!_bankService.IsBankAvailable(bankName))
            {
                Console.WriteLine("Enter Staff Username");
                string staffUsername = Console.ReadLine();
                Console.WriteLine("Enter Staff Password");
                string staffPassword = Console.ReadLine();
                _bankService.SetupBank(bankName, staffUsername, staffPassword);
            }
            else
            {
                Console.WriteLine("Bank already available in system. Please try again.");
            }
        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Bank Management System\n");
            Console.WriteLine("1. Setup New Bank\n2. Login as account holder / bank staff\n3. Exit\n");
        }

        public enum BankOperations
        {
            SetupBank = 1,
            Login,
            Exit
        }

        public void MainMenu()
        {
            while (true)
            {
                DisplayMenu();
                Console.WriteLine("Please select valid option : ");
                BankOperations option = (BankOperations)Helper.ValidateNumberInput();
                switch (option)
                {
                    case BankOperations.SetupBank:
                        SetupBank();
                        break;
                    case BankOperations.Login:
                        LoginOptions();
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
