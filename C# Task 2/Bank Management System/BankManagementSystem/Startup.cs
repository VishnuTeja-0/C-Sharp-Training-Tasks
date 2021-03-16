using BankManagement.Services;
using System;

namespace BankManagement
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            try
            {
                BankManagementApp app = new BankManagementApp(new BankService());
                app.MainMenu();
            }
            catch(Exception ex)
            {
                ex.Message.DisplayLine();
            }
        }
    }
}
