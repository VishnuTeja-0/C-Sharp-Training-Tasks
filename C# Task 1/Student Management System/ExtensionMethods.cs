using System.Collections.Generic;
using System.Linq;

namespace StudentManagement
{
    public static class ExtensionMethods
    {
        public static bool StudentExists(this List<Models.Student> studentList, int rollNo)
        {
            return studentList.Any(i => i.GetRollNumber() == rollNo);
        }

        public static Models.Student GetStudentByRollNumber(this List<Models.Student> studentList, int rollNo)
        {
            return studentList.FirstOrDefault(i => i.GetRollNumber() == rollNo);
        }

        public static bool IsAlphabetical(this string str)
        {
            return str.All(char.IsLetter);
        }

        public static bool IsNumerical(this string str)
        {
            return str.All(char.IsDigit);
        }
    }
}
