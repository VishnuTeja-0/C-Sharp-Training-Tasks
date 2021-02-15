
namespace StudentManagement.Models
{
    public class Subject
    {

        private School.Subjects _name;

        public School.Subjects Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _marks;

        public int Marks
        {
            get { return _marks; }
            set { _marks = value; }
        }
    }
}
