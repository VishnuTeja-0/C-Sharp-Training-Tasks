using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Student_Management_System
{
    class Helper
    {
        public void PrintMenu(School school)
        {
            Console.WriteLine("Welcome to " + school.GetSchoolName() + " School Student Information Management");
            Console.WriteLine(new String('-', 50));
            Console.WriteLine("1. Add student\n2. Add marks for student\n3. Show student progress card");
            Console.WriteLine("Please provide valid input from menu options :");
        }
    }
}
