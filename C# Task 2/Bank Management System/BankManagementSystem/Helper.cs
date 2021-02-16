using System;

namespace BankManagement
{
    public static class Helper
    {

        public static string ValidateTextInput()
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

        public static int ValidateNumberInput()
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
    }
}
