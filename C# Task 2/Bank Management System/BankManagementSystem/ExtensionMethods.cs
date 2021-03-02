using System;
using System.Collections.Generic;
using System.Linq;

namespace BankManagement
{
    public static class ExtensionMethods
    {
        public static void DisplayLine(this string str)
        {
            Console.WriteLine(str);
        }

        public static void Display(this string str)
        {
            Console.Write(str);
        }

        public static bool IsAlphabetical(this string str)
        {
            return str.All(char.IsLetter);
        }

    }
}
