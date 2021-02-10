using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class Student
    {
        private int _rollNo;
        private string _name;
        List<Subject> _subjectMarks;

        public Student()
        {
            _subjectMarks = new List<Subject>();
        }

        public void SetStudentDetails(int rollNo, string name)
        {
            _rollNo = rollNo;
            _name = name;
        }

        public void SetSubjectMarks(Subject subject)
        {
            _subjectMarks.Add(subject);
        }

        public int GetRollNumber()
        {
            return _rollNo;
        }

        public string GetName()
        {
            return _name;
        }

        public List<Subject> GetSubjectMarks()
        {
            return _subjectMarks;
        }
    }
}

