using System;

namespace BankManagement
{
    class BankManagementApp
    {
        public void SetupBank()
        {

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
            Login
        }

        public void MainMenu()
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
                default:
                    Console.WriteLine("Out of bounds. Please enter a number among the given options.");
                    break;
            }

        }

        public void Startup()
        {
            
        }
    }
}
