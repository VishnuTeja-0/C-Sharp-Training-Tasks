using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagement.Models;


namespace StudentManagement
{
    class StudentManagementApp : Operations
    {
        // To perform operation of printing progress card of existing student with existing marks record
        private void ShowProgress(School school)
        {
            GenerateReportCard(school);
            Console.ReadKey(false);
            Console.Clear();
            PrintMenu(school);
        }

        // To perform operation for adding marks for existing student
        private void AddMarks(School school)
        {
            AddStudentMarks(school);
            cw("Press Any Key to Continue");
            Console.ReadKey(false);
            Console.Clear();
            PrintMenu(school);
        }

        // To perform operation of adding new student details
        private void AddStudent(School school)
        {
            Student student = new Student();
            SetStudentDetails(school, student);
            school.AddStudent(student);
            cw("Press Any Key to Continue");
            Console.ReadKey(false);
            Console.Clear();
            PrintMenu(school);
        }

        // For printing main menu
        

        // For main student management options
        private void MainMenu(School school)
        {
            Console.Clear();
            PrintMenu(school);
            while (true)
            {
                int option = ValidNumberInput();
                switch (option)
                {
                    case 1:
                        AddStudent(school);
                        break;
                    case 2:
                        AddMarks(school);
                        break;
                    case 3:
                        ShowProgress(school);
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        cw("Out of bounds. Please enter a number among the given options.");
                        break;
                }
            }
        }

        // For startup message and prompt
        public void StartUp()
        {
            School school = new School();
            EnterSchoolName(school);
            MainMenu(school);
        }

        static void Main()
        {
            StudentManagementApp app = new StudentManagementApp();
            app.StartUp();
        }
    }
}
