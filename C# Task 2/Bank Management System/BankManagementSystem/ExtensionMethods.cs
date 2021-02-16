using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankManagement
{
    public static class ExtensionMethods
    {
        public static bool IsAlphabetical(this string str)
        {
            return str.All(char.IsLetter);
        }
    }
}
