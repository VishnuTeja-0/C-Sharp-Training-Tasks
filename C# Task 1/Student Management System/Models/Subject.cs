
namespace StudentManagement.Models
{
    public class Subject
    {

        private string _name;
        private int _marks;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Marks
        {
            get { return _marks; }
            set { _marks = value; }
        }
    }
}
