using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentManagement.Models;

namespace StudentManagement
{
    class Helper
    {
        public Action<string> cw = Console.WriteLine;

        public void PrintMenu(School school)
        {
            cw($"Welcome to {school.GetSchoolName()} School Student Information Management");
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
                    if (userInput.All(char.IsLetter))
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
                    if (userInput.All(char.IsDigit))
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
    }
}
