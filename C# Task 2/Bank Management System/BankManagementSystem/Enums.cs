using System;
using System.Collections.Generic;
using System.Text;

namespace BankManagement
{
    public class Enums
    {

        public enum BankOperations
        {
            SetupBank = 1,
            Login,
            Exit
        }


        public enum StaffOperations
        {
            CreateAccount = 1,
            UpdateAccount,
            DeleteAccount,
            AddCurrency,
            UpdateServiceCharges,
            ViewTransactionHistory,
            RevertTransaction,
            Logout
        }

        public enum AccountOperations
        {
            DepositAmount = 1,
            WithdrawAmount,
            TransferFunds,
            ViewTransactionHistory,
            Logout
        }
    }
}
