using System;
using System.Text.RegularExpressions;

namespace BankManagement
{
    public static class Helper
    {

        public static string GetTextInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput) && userInput.IsAlphabetical())
                {
                    return userInput;
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please enter alphabetical input.");
                }
            }
        }

        public static int GetNumberInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput) && Int32.TryParse(userInput, out int num))
                {
                    return num;
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please enter numerical input.");
                }
            }
        }

        public static double GetDecimalInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput) && Double.TryParse(userInput, out double num))
                {
                    return num;
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please enter numerical / decimal input.");
                }
            }
        }

        public static string GetCurrencyCodeInput()
        {
            while (true)
            {
                string code = Helper.GetTextInput();
                if (code.Length == 3)
                {
                    return code;
                }
                else
                {
                    Console.WriteLine("Incorrect input. Code should be consist of three letters only. Please try again.");
                }
            }
        }

        public static string GetPasswordInput()
        {
            Regex re = new Regex("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]){8,}");
            while (true)
            {
                string password = Console.ReadLine();
                if (password.Length >= 8 && re.IsMatch(password))
                {
                    return password;
                }
                else
                {
                    ("Invalid password").DisplayLine();
                }
            }
        }
    }
}
