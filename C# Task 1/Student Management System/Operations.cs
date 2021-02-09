using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentManagement.Models;

namespace StudentManagement
{
    class Operations : Helper
    {
        public void EnterSchoolName(School school)
        {
            Console.WriteLine("Enter The School Name: ");
            string userInput = ValidTextInput();
            school.SetSchoolName(userInput);
        }

        public void SetStudentDetails(School school, Student student)
        {
            Console.WriteLine("Enter Student Roll number : ");
            int rollNo = ValidNumberInput();
            if (!school.GetStudentList().Any(i => i.GetRollNumber() == rollNo)){
                Console.WriteLine("Enter Student Name : ");
                string name = ValidTextInput();
                student.SetStudentDetails(rollNo, name);
                Console.WriteLine("Student details are added successfully\n");
            }
            else
            {
                Console.WriteLine("Student with given roll number already exists. Please try again.");
                return;
            }
            
        }

        public void AddStudentMarks(School school)
        {
            Console.WriteLine("Enter Student Roll number : ");
            int rollNo = ValidNumberInput();
            if (!school.GetStudentList().Any(i => i.GetRollNumber() == rollNo))
            {
                Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
                return;
            }
            else
            {
                Student student = school.GetStudentList().FirstOrDefault(i => i.GetRollNumber() == rollNo);
                foreach (string subjectname in school.GetSubjects())
                {
                    Subject subject = new Subject();
                    subject.SetName(subjectname);
                    subject.SetMarks(EnterSubjectMarks(subjectname));
                    student.SetSubjectMarks(subject);
                }
            }
            Console.WriteLine("Student marks are added successfully\n");
        }

        public int EnterSubjectMarks(string subject)
        {
            Console.WriteLine("Enter Marks scored in " + subject + " : ");
            int marks;
            while (true)
            {
                marks = ValidNumberInput();
                if(marks < 100)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter numerical input out of 100");
                }
            }
            return marks;
        }

        public void GenerateReportCard(School school)
        {
            Console.WriteLine("Enter Student Roll Number : ");
            int rollNo = ValidNumberInput();
            if (school.GetStudentList().Any(i => i.GetRollNumber() == rollNo))
            {
                Student student = school.GetStudentList().FirstOrDefault(i => i.GetRollNumber() == rollNo);
                if (student.GetSubjectMarks().Count() > 0)
                {
                    Console.WriteLine("Student Roll Number : " + rollNo);
                    Console.WriteLine("Student Name : " + student.GetName());
                    Console.WriteLine("Student Marks");
                    Console.WriteLine("---------------");
                    List<int> setOfMarks = new List<int>();
                    foreach(Subject subject in student.GetSubjectMarks())
                    {
                        Console.WriteLine(subject.GetName() + " : " + subject.GetMarks());
                        setOfMarks.Add(subject.GetMarks());
                    }
                    Console.WriteLine("---------------\n");
                    Console.WriteLine("Total Marks : " + setOfMarks.Sum());
                    double percentage = setOfMarks.Average();
                    Console.WriteLine("Percentage : " + percentage);
                    Console.WriteLine("---------------\n");
                }
                else
                {
                    Console.WriteLine("Student with given roll number does not have record of marks. Please try again\n");
                    Console.WriteLine("Press Any Key to Continue");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
                Console.WriteLine("Press Any Key to Continue");
                return;
            }
            Console.WriteLine("Press any key to continue");
        }
    }
}
