using System;
using StudentManagement.Models;


namespace StudentManagement
{
    public class StudentManagementApp : Operations
    {
        // To perform operation of printing progress card of existing student with existing marks record
        private void ShowProgress(School school)
        {
            GenerateReportCard(school.GetStudentList());
            DisplayMenu(school.GetSchoolName());
        }

        // To perform operation for adding marks for existing student
        private void AddMarks(School school)
        {
            AddStudentMarks(school.GetStudentList(), school.GetSubjects());
            cw("Press Any Key to Continue");
            DisplayMenu(school.GetSchoolName());
        }

        // To perform operation of adding new student details
        private void AddStudent(School school)
        {
            Student student = new Student();
            SetStudentDetails(school.GetStudentList(), student);
            school.AddStudent(student);
            cw("Press Any Key to Continue");
            DisplayMenu(school.GetSchoolName());
        }

        // For printing main menu
        

        // For main student management options
        private void MainMenu(School school)
        {
            Console.Clear();
            PrintMenu(school.GetSchoolName());
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
