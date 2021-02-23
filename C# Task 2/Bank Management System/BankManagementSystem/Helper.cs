using System;

namespace BankManagement
{
    public static class Helper
    {

        public static string TextInput()
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

        public static int NumberInput()
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

        public static double DecimalInput()
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
    }
}
