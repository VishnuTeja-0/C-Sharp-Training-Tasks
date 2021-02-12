using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentManagement
{
    public class InputService
    {
        private SchoolService _schoolService;

        public string InputSchoolName()
        {
            Console.WriteLine("Enter The School Name: ");
            string newSchoolName = Helper.TakeValidTextInput();
            SchoolService service = new SchoolService();
            service.AddSchool(newSchoolName);
            _schoolService = service;
            return newSchoolName;
        }

        public void InputStudentDetails()
        {
            Console.WriteLine("Enter Student Roll number : ");
            int rollNo = Helper.TakeValidNumberInput();
            if (!_schoolService.IsStudentExisting(rollNo))
            {
                Console.WriteLine("Enter Student Name : ");
                string name = Helper.TakeValidTextInput();
                _schoolService.AddStudentDetails(rollNo, name);
                Console.WriteLine("Student details are added successfully\n");
            }
            else
            {
                Console.WriteLine("Student with give roll number already exists. Please try again.\n");
            }
        }

        public void InputStudentMarks()
        {
            Console.WriteLine("Enter Student Roll number : ");
            int rollNo = Helper.TakeValidNumberInput();
            if (_schoolService.IsStudentExisting(rollNo))
            {
                List<Subject> subjects = new List<Subject>();
                foreach (string subjectName in _schoolService.GetSubjects())
                {
                    Subject subject = new Subject();
                    subject.Name = subjectName;
                    Console.WriteLine($"Enter Marks scored in {subjectName} : ");
                    subject.Marks = Helper.TakeValidNumberInput();
                    subjects.Add(subject);
                }
                _schoolService.AddStudentMarks(rollNo, subjects);
                Console.WriteLine("Student marks are added successfully\n");
            }
            else
            {
                Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
            }
        }

        public void RequestProgressCard()
        {
            Console.WriteLine("Enter Student Roll Number : ");
            int rollNo = Helper.TakeValidNumberInput();
            if (_schoolService.IsStudentExisting(rollNo))
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
        }
    }
}
