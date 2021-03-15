using BankManagement.Services;

namespace BankManagement
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            BankManagementApp app = new BankManagementApp(new BankService());
            app.MainMenu();
        }
    }
}
