using System;

namespace StudentManagement
{
    public class Helper
    {
        public Action<string> cw = Console.WriteLine;

        public void PrintMenu(string schoolName)
        {
            cw($"Welcome to {schoolName} School Student Information Management");
            cw(new String('-', 50));
            cw("1. Add student\n2. Add marks for student\n3. Show student progress card\n4. Quit Program\n");
            cw("Please provide valid input from menu options :");
        }

        public string ValidTextInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput))
                {
                    if (userInput.IsAlphabetical())
                    {
                        return userInput;
                    }
                    else
                    {
                        cw("Invalid Input. Input can only be alphabetical. Please try again.");
                    }
                }
                else
                {
                    cw("Invalid Input. Please enter alphabetical input.");
                }
            }
        }

        public int ValidNumberInput()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput))
                {
                    if (userInput.IsNumerical())
                    {
                        return Int32.Parse(userInput);
                    }
                    else
                    {
                        cw("Invalid Input. Input can only be numerical. Please try again.");
                    }
                }
                else
                {
                    cw("Invalid Input. Please enter numerical input.");
                }
            }
        }

        public void DisplayMenu(string schoolName)
        {
            Console.ReadKey(false);
            Console.Clear();
            PrintMenu(schoolName);
        }
    }
}
