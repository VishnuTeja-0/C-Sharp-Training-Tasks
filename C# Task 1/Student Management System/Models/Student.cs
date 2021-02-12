using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class Student
    {
        private int _rollNo;
        private string _name;
        private List<Subject> _subjects;

        public int RollNo
        {
            get { return _rollNo; }
            set { _rollNo = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<Subject> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; }
        }

    }
}

