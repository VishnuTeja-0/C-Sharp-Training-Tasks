
namespace StudentManagement.Models
{
    public class Subject
    {
        private string _subjectName;
        private int _marks;

        public void SetName(string name)
        {
            _subjectName = name;
        }

        public void SetMarks(int marks)
        {
            _marks = marks;
        }

        public string GetName()
        {
            return _subjectName;
        }

        public int GetMarks()
        {
            return _marks;
        }
    }
}
