using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Student_Management_System
{
    class School
    {
        private string _schoolName;
        private List<Student> _students;
        private List<string> _subjects = new List<string> { "Telugu", "Hindi", "English", "Maths", "Science", "Social" };

        public School()
        {
            _students = new List<Student>();
        }

        public void SetSchoolName()
        {
            Console.WriteLine("Enter The School Name: ");
            while (true)
            {
                string strIn = Console.ReadLine();
                if (strIn.All(char.IsLetter))
                {
                    _schoolName = strIn;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input. School Name can only be alphabetical. Please try again.");
                }
            }
        }

        public List<Student> GetStudentList()
        {
            return _students;
        }

        public string GetSchoolName()
        {
            return _schoolName;
        }

        public List<string> GetSubjects()
        {
            return _subjects;
        }

    }
}
