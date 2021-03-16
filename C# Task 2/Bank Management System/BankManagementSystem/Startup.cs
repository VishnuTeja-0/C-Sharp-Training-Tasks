using BankManagement.Services;

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
            catch(System.Data.SqlClient.SqlException SQLEx)
            {
                Constants.SQLExceptionMessage.DisplayLine();
                SQLEx.ToString().DisplayLine();
            }
        }
    }
}
