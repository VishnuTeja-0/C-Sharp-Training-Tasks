using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class School
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<Student> _students;

        public List<Student> Students
        {
            get { return _students; }
            set { _students = value; }
        }

        private List<string> _subjects = new List<string> { "Telugu", "Hindi", "English", "Maths", "Science", "Social" };

        public List<string> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; }
        }
    }
}
