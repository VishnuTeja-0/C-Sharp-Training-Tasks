using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Student_Management_System
{
    class Student
    {
        private int _rollNo;
        private string _name;
        Dictionary<string, int> _subjectMarks = new Dictionary<string, int>();

        public void SetStudentDetails(School school)
        {
            Console.WriteLine("Enter Student Roll number : ");
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsDigit))
                {
                    int num = Int32.Parse(strin);
                    if (school.GetStudentList().Any(i => i.GetRollNumber() == num))
                    {
                        Console.WriteLine("Student with given roll number already exists. Please try again\n");
                        return;
                    }
                    else
                    {
                        _rollNo = num;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter numerical input.");
                }
            }
            Console.WriteLine("Enter Student Name : ");
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsLetter))
                {
                    _name = strin;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Student Name can only be alphabetical. Please try again.");
                }
            }
            Console.WriteLine("Student details are added successfully\n");
        }

        public void SetSubjectMarks(KeyValuePair<string, int> k)
        {
            _subjectMarks.Add(k.Key, k.Value);
        }

        public int GetRollNumber()
        {
            return _rollNo;
        }

        public string GetName()
        {
            return _name;
        }

        public Dictionary<string, int> GetSubjectMarks()
        {
            return _subjectMarks;
        }
    }
}

