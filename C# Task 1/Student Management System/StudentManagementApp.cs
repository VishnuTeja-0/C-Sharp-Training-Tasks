using System;
using System.Collections.Generic;
using System.Linq;

namespace Student_Management_System
{
    class StudentManagementApp
    {
        // To perform operation of printing progress card of existing student with existing marks record
        private void ShowProgress(School school, Helper helper)
        {
            Operations op = new Operations();
            op.GenerateReportCard(school);
            Console.ReadKey(false);
            Console.Clear();
            helper.PrintMenu(school);
        }

        // To perform operation for adding marks for existing student
        private void AddMarks(School school, Helper helper)
        {
            Operations ops = new Operations();
            ops.AddMarks(school);
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey(false);
            Console.Clear();
            helper.PrintMenu(school);
        }

        // To perform operation of adding new student details
        private void AddStudent(School school, Helper helper)
        {
            Student student = new Student();
            student.SetStudentDetails(school);
            school.GetStudentList().Add(student);
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey(false);
            Console.Clear();
            helper.PrintMenu(school);
        }

        // For printing main menu
        

        // For main student management options
        private void MainMenu(School school, Helper helper)
        {
            Console.Clear();
            helper.PrintMenu(school);
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsDigit))
                {
                    int opt = Convert.ToInt32(strin);
                    switch (opt)
                    {
                        case 1:
                            AddStudent(school, helper);
                            break;
                        case 2:
                            AddMarks(school, helper);
                            break;
                        case 3:
                            ShowProgress(school, helper);
                            break;
                        default:
                            Console.WriteLine("Out of bounds. Please enter a number among the given options.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input type. Please enter numerical input.");
                }
            }
        }

        // For startup message and prompt
        public void StartUp()
        {
            School school = new School();
            school.SetSchoolName();
            Helper helper = new Helper();
            MainMenu(school, helper);
        }

        static void Main()
        {
            StudentManagementApp app = new StudentManagementApp();
            app.StartUp();
        }
    }
}
