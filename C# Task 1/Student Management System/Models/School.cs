using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class School
    {
        private string _name;
        private List<Student> _students;
        private List<string> _subjects = new List<string> { "Telugu", "Hindi", "English", "Maths", "Science", "Social" };

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<Student> Students
        {
            get { return _students; }
            set { _students = value;  }
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);
        }

        public List<string> Subjects
        {
            get { return _subjects; }
        }

        public void AddSubject(string subject)
        {
            _subjects.Add(subject);
        }


    }
}
