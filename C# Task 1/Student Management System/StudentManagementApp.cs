using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagement.Models;


namespace StudentManagement
{
    public class StudentManagementApp
    {
        private SchoolService _schoolService;

        // To perform operation of printing progress card of existing student with existing marks record
        private void ShowProgress(string schoolName)
        {
            Console.WriteLine("Enter Student Roll Number : ");
            int rollNo = Helper.ValidateNumberInput();
            if (_schoolService.IsStudentAvailable(rollNo))
            {
                Student student = _schoolService.GetStudent(rollNo);
                if (student.Subjects.Count > 0)
                {
                    Console.WriteLine($"Student Roll Number : {rollNo}");
                    Console.WriteLine($"Student Name : {student.Name}");
                    Console.WriteLine("Student Marks");
                    Console.WriteLine("---------------");
                    List<Subject> subjects = _schoolService.GenerateReportCard(rollNo);
                    List<int> marks = new List<int>();
                    foreach (Subject subject in subjects)
                    {
                        Console.WriteLine($"{subject.Name} : {subject.Marks}");
                        marks.Add(subject.Marks);
                    }
                    Console.WriteLine("---------------\n");
                    Console.WriteLine($"Total Marks : {marks.Sum()}");
                    Console.WriteLine($"Percentage : { marks.Average()}");
                    Console.WriteLine("---------------\n");
                }
                else
                {
                    Console.WriteLine("Student with given roll number does not have record of marks. Please try again\n");
                }
            }
            else
            {
                Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
            }
            AfterOperation(schoolName);
        }

        // To perform operation for adding marks for existing student
        private void AddMarks(string schoolName)
        {
            Console.WriteLine("Enter Student Roll number : ");
            int rollNo = Helper.ValidateNumberInput();
            if (_schoolService.IsStudentAvailable(rollNo))
            {
                List<Subject> subjects = new List<Subject>();
                foreach (string subjectName in _schoolService.GetSubjects())
                {
                    Subject subject = new Subject();
                    subject.Name = subjectName;
                    Console.WriteLine($"Enter Marks scored in {subjectName} : ");
                    subject.Marks = Helper.ValidateNumberInput();
                    subjects.Add(subject);
                }
                _schoolService.AddStudentMarks(rollNo, subjects);
                Console.WriteLine("Student marks are added successfully\n");
            }
            else
            {
                Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
            }
            AfterOperation(schoolName);
        }

        // To perform operation of adding new student details
        private void AddStudent(string schoolName)
        {

            Console.WriteLine("Enter Student Roll number : ");
            int rollNo = Helper.ValidateNumberInput();
            if (!_schoolService.IsStudentAvailable(rollNo))
            {
                Console.WriteLine("Enter Student Name : ");
                string name = Helper.ValidateTextInput();
                _schoolService.AddStudentDetails(rollNo, name);
                Console.WriteLine("Student details are added successfully\n");
            }
            else
            {
                Console.WriteLine("Student with give roll number already exists. Please try again.\n");
            }
            AfterOperation(schoolName);
        }

        // For printing main menu
        

        // For main student management options
        private void MainMenu(string schoolName)
        {
            DisplayMenu(schoolName);
            while (true)
            {
                int option = Helper.ValidateNumberInput();
                switch (option)
                {
                    case 1:
                        AddStudent(schoolName);
                        break;
                    case 2:
                        AddMarks(schoolName);
                        break;
                    case 3:
                        ShowProgress(schoolName);
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

        public void DisplayMenu(string schoolName)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to {schoolName} School Student Information Management");
            Console.WriteLine(new String('-', 50));
            Console.WriteLine("1. Add student\n2. Add marks for student\n3. Show student progress card\n4. Quit Program\n");
            Console.WriteLine("Please provide valid input from menu options :");
        }

        public void AfterOperation(string schoolName)
        {
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey(false);
            DisplayMenu(schoolName);
        }

        // For startup message and prompt
        public void StartUp()
        {
            Console.WriteLine("Enter The School Name: ");
            string newSchoolName = Helper.ValidateTextInput();
            SchoolService service = new SchoolService();
            service.AddSchool(newSchoolName);
            _schoolService = service;
            MainMenu(newSchoolName);
        }
    }
}
