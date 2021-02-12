using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class Student
    {
        private int _rollNo;

        public int RollNo
        {
            get { return _rollNo; }
            set { _rollNo = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<Subject> _subjects;

        public List<Subject> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; }
        }

    }
}

