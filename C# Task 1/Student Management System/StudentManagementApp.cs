using System;
using StudentManagement.Models;


namespace StudentManagement
{
    public class StudentManagementApp
    {
        // To perform operation of printing progress card of existing student with existing marks record
        private void ShowProgress(string schoolName, InputService inputService)
        {
            inputService.RequestProgressCard();
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey(false);
            DisplayMenu(schoolName);
        }

        // To perform operation for adding marks for existing student
        private void AddMarks(string schoolName, InputService inputService)
        {
            inputService.InputStudentMarks();
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey(false);
            DisplayMenu(schoolName);
        }

        // To perform operation of adding new student details
        private void AddStudent(string schoolName, InputService inputService)
        {

            inputService.InputStudentDetails();
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey(false);
            DisplayMenu(schoolName);
        }

        // For printing main menu
        

        // For main student management options
        private void MainMenu(string schoolName, InputService inputService)
        {
            DisplayMenu(schoolName);
            while (true)
            {
                int option = Helper.TakeValidNumberInput();
                switch (option)
                {
                    case 1:
                        AddStudent(schoolName, inputService);
                        break;
                    case 2:
                        AddMarks(schoolName, inputService);
                        break;
                    case 3:
                        ShowProgress(schoolName, inputService);
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Out of bounds. Please enter a number among the given options.");
                        break;
                }
            }
        }

        public static void DisplayMenu(string schoolName)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to {schoolName} School Student Information Management");
            Console.WriteLine(new String('-', 50));
            Console.WriteLine("1. Add student\n2. Add marks for student\n3. Show student progress card\n4. Quit Program\n");
            Console.WriteLine("Please provide valid input from menu options :");
        }

        // For startup message and prompt
        public void StartUp()
        {
            InputService inputService = new InputService();
            string schoolName = inputService.InputSchoolName();
            MainMenu(schoolName, inputService);
        }

        static void Main()
        {
            StudentManagementApp app = new StudentManagementApp();
            app.StartUp();
        }
    }
}
