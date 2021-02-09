using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentManagement.Models;

namespace StudentManagement
{
    class Helper
    {
        public void PrintMenu(School school)
        {
            Console.WriteLine("Welcome to " + school.GetSchoolName() + " School Student Information Management");
            Console.WriteLine(new String('-', 50));
            Console.WriteLine("1. Add student\n2. Add marks for student\n3. Show student progress card\n4. Quit Program\n");
            Console.WriteLine("Please provide valid input from menu options :");
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
                        Console.WriteLine("Invalid Input. Input can only be alphabetical. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please enter alphabetical input.");
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
                        Console.WriteLine("Invalid Input. Input can only be numerical. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please enter numerical input.");
                }
            }
        }
    }
}
