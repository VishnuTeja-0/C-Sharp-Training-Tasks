using System;

namespace StudentManagement
{
    public static class Helper
    {

        public static string TakeValidTextInput()
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

        public static int TakeValidNumberInput()
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
                    Console.WriteLine("Invalid Input. Please enter alphabetical input.");
                }
            }
        }
    }
}
