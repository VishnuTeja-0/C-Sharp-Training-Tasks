using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentManagement.Models
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

        public void SetSchoolName(string name)
        {
            _schoolName = name;
        }

        public void AddSubject(string subject)
        {
            _subjects.Add(subject);
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);
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
