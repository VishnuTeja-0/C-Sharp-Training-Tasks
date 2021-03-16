using System;
using System.Collections.Generic;
using BankManagement.Models;
using BankManagement.Contracts;

namespace BankManagement
{
    public class BankManagementApp
    {
        private IBankService _bankService;

        public void DepositAmount(string bankId, string accountId)
        {
            Constants.CurrencyCodePrompt.DisplayLine();
            string code = Helper.GetCurrencyCodeInput();
            if (_bankService.IsCurrencyAvailable(bankId, code))
            {
                Constants.Account.DepositAmountPrompt.DisplayLine();
                decimal amount = Helper.GetDecimalInput();
                _bankService.DepositAmount(bankId, accountId, amount, code);
                ($"Amount deposited successfully! Current Balance : {_bankService.GetAccountBalance(accountId)}").DisplayLine();
            }
            else
            {
                Constants.Account.InvalidCurrencyCode.DisplayLine();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void WithdrawAmount(string bankId, string accountId)
        {
            Constants.Account.WithdrawAmountPrompt.DisplayLine();
            decimal amount = Helper.GetDecimalInput();
            if (amount <= _bankService.GetAccountBalance(accountId))
            {
                _bankService.WithdrawAmount(accountId, amount);
                ($"Amount withdrawn succesfully! Current balance : {_bankService.GetAccountBalance(accountId)}").DisplayLine();
            }
            else
            {
                Constants.Account.InsufficientBalanceMessage.DisplayLine();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void TransferFunds(string bankId, string accountId)
        {
            Constants.Account.RecipientAccountBankPrompt.DisplayLine();
            string recipientBankId = SelectBank();
            Constants.Account.RecipientAccountIdPrompt.DisplayLine();
            string recipientAccountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(recipientBankId, recipientAccountId))
            {
                Constants.Account.TransferAmountPrompt.DisplayLine();
                decimal amount = Helper.GetDecimalInput();
                if (amount <= _bankService.GetAccountBalance(accountId))
                {
                    _bankService.TransferFunds(bankId, accountId, recipientBankId, recipientAccountId, amount);
                    ($"Amount transferred succesfully! Current balance : {_bankService.GetAccountBalance(accountId)}").DisplayLine();
                }
                else
                {
                    Constants.Account.InsufficientBalanceMessage.DisplayLine();
                }
            }
            else
            {
                Constants.AccountNotAvailableMessage.DisplayLine();
            }
            ReturnToAccountMenu(bankId, accountId);
        }

        public void ViewTransaction(string bankId, string accountId)
        {
            List<Models.EntityModels.Transaction> transactions = _bankService.GetTransactions(accountId);
            Constants.Account.TransactionTableHeader.DisplayLine();
            foreach (Models.EntityModels.Transaction transaction in transactions)
            {
                ($"{transaction.TransactionId,40} ").Display();
                ($"{transaction.SenderAccountId,14} ").Display();
                ($"{_bankService.GetBankName(_bankService.GetAccountBank(transaction.SenderAccountId)),11} ").Display();
                ($"{_bankService.GetAccountName(transaction.SenderAccountId)} ").Display();
                ($"{transaction.RecipientAccountId,14} ").Display();
                ($"{_bankService.GetBankName(_bankService.GetAccountBank(transaction.RecipientAccountId)),11} ").Display();
                ($"{_bankService.GetAccountName(transaction.RecipientAccountId),11} ").Display();
                ($"{transaction.Amount,8} ").Display();
                ($"{transaction.TransactionDateTime,21} \n").DisplayLine();
            }
        }

        public void ReturnToAccountMenu(string bankId, string accountId)
        {
            Constants.PressKeyPrompt.DisplayLine();
            Console.ReadKey(false);
            DisplayAccountMenu(bankId, accountId);
        }

        public void DisplayAccountMenu(string bankId, string accountId)
        {
            Console.Clear();
            string bankName = _bankService.GetBankName(bankId);
            string accountName = _bankService.GetAccountName(accountId);
            ($"Welcome to {bankName} Bank! You are logged in as account holder, {accountName}.\n").DisplayLine();
            Constants.Account.Menu.DisplayLine();
            Constants.OptionSelectionPrompt.DisplayLine();
        }

        public void AccountLoginOptions(string bankId, string accountId)
        {
            DisplayAccountMenu(bankId, accountId);
            bool isLoggedIn = true;
            while (isLoggedIn)
            {
                AccountOperations option = (AccountOperations)Helper.GetNumberInput();
                try
                {
                    switch (option)
                    {
                        case AccountOperations.DepositAmount:
                            DepositAmount(bankId, accountId);
                            break;
                        case AccountOperations.WithdrawAmount:
                            WithdrawAmount(bankId, accountId);
                            break;
                        case AccountOperations.TransferFunds:
                            TransferFunds(bankId, accountId);
                            break;
                        case AccountOperations.ViewTransactionHistory:
                            ViewTransaction(bankId, accountId);
                            ReturnToAccountMenu(bankId, accountId);
                            break;
                        case AccountOperations.Logout:
                            isLoggedIn = false;
                            break;
                        default:
                            Constants.OutOfBoundsMessage.DisplayLine();
                            break;
                    }
                }
                catch(Microsoft.EntityFrameworkCore.DbUpdateException UpEx)
                {
                    Constants.DBUpdateExceptionMessage.DisplayLine();
                    UpEx.ToString().DisplayLine();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException EnEx)
                {
                    Constants.DBUpdateExceptionMessage.DisplayLine();
                    EnEx.ToString().DisplayLine();
                }
                finally
                {
                    ReturnToAccountMenu(bankId, accountId);
                }

            }
            Constants.LogOutMessage.DisplayLine();
        }

        public void CreateAccount(string bankId)
        {
            Console.Clear();
            Constants.Staff.NewNamePrompt.DisplayLine();
            string newName = Helper.GetTextInput();
            Constants.Staff.NewUsernamePrompt.DisplayLine();
            string newUsername = Console.ReadLine();
            if (! _bankService.IsAccountHolder(newUsername))
            {
               Constants.PasswordPrompt.DisplayLine();
                string newPassword = Helper.GetPasswordInput();
                Constants.Staff.InitialDepositPrompt.DisplayLine();
                decimal initialDeposit = Helper.GetDecimalInput();
                _bankService.CreateAccount(bankId, newName, newUsername, newPassword, initialDeposit);
                Constants.Staff.CreateAccountSuccessMessage.DisplayLine();
            }
            else
            {
                Constants.Staff.UsernameNotAvailableMessage.DisplayLine();
            }
            ReturnToStaffMenu(bankId);
        }

        public void UpdateAccount(string bankId)
        {
            Console.Clear();
            Constants.Staff.UpdateAccountIdPrompt.DisplayLine();
            string accountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(bankId, accountId))
            {
                Constants.Staff.UpdateNamePrompt.DisplayLine();
                string updatedName = Helper.GetTextInput();
                Constants.Staff.UpdateUsernamePrompt.DisplayLine();
                string updatedUsername = Console.ReadLine();
                if (!_bankService.IsAccountHolder(updatedUsername))
                {
                    Constants.Staff.UpdatePasswordPrompt.DisplayLine();
                    string updatedPassword = Helper.GetPasswordInput();
                    _bankService.UpdateAccount(accountId, updatedName, updatedUsername, updatedPassword);
                    Constants.Staff.UpdateAccountSuccessMessage.DisplayLine();
                }
                else
                {
                    Constants.Staff.UsernameNotAvailableMessage.DisplayLine();
                }

            }
            else
            {
                Constants.AccountNotAvailableMessage.DisplayLine();
            }
            ReturnToStaffMenu(bankId);
        }

        public void DeleteAccount(string bankId)
        {
            Console.Clear();
            Constants.Staff.DeleteAccountIdPrompt.DisplayLine();
            string accountId = Console.ReadLine();
            if(_bankService.IsAccountAvailable(bankId, accountId))
            {
                _bankService.DeleteAccount(bankId, accountId);
                Constants.Staff.DeleteAccountSuccessMessage.Display();
            }
            else
            {
                Constants.AccountNotAvailableMessage.DisplayLine();
            }
            ReturnToStaffMenu(bankId);
        }

        public void AddCurrency(string bankId)
        {
            Console.Clear();
            Constants.Staff.CurrencyNamePrompt.DisplayLine();
            string name = Helper.GetTextInput();
            Constants.CurrencyCodePrompt.DisplayLine();
            string code = Helper.GetCurrencyCodeInput();
            Constants.Staff.ExchangeRatePrompt.DisplayLine();
            decimal exchangeRate = Helper.GetDecimalInput();
            _bankService.AddCurrency(bankId, name, code, exchangeRate);
            Constants.Staff.AddCurrencySuccessMessage.Display();
            ReturnToStaffMenu(bankId);
        }

        public void UpdateCharges(string bankId)
        {
            Console.Clear();
            Constants.Staff.SameRtgsPrompt.DisplayLine();
            decimal newSameRTGS = Helper.GetNumberInput() / 100;
            Constants.Staff.SameImpsPrompt.DisplayLine();
            decimal newSameIMPS = Helper.GetNumberInput() / 100;
            Constants.Staff.DiffRtgsPrompt.DisplayLine();
            decimal newDiffRTGS = Helper.GetNumberInput() / 100;
            Constants.Staff.DiffImpsPrompt.DisplayLine();
            decimal newDiffIMPS = Helper.GetNumberInput() / 100;
            _bankService.UpdateServiceCharges(bankId, newSameRTGS, newSameIMPS, newDiffRTGS, newDiffIMPS);
            Constants.Staff.UpdateChargesSuccessMessage.DisplayLine();
            ReturnToStaffMenu(bankId);
        }

        public void RevertTransaction(string bankId)
        {
            Constants.Staff.AccountIdPrompt.DisplayLine();
            string accountId = Console.ReadLine();
            if (_bankService.IsAccountAvailable(bankId, accountId))
            {
                Constants.Staff.TransactionIdPrompt.DisplayLine();
                string transactionId = Console.ReadLine();
                if (_bankService.IsTransactionAvailable(transactionId))
                {
                    _bankService.RevertTransaction(transactionId);
                    Constants.Staff.RevertTransactionSuccessMessage.DisplayLine();
                }
                else
                {
                    Constants.Staff.TransactionNotAvailableMessage.DisplayLine();
                }
            }
            else
            {
                Constants.AccountNotAvailableMessage.DisplayLine();
            }
            ReturnToStaffMenu(bankId);
        }

        public void ReturnToStaffMenu(string bankId)
        {
            Constants.PressKeyPrompt.DisplayLine();
            Console.ReadKey(false);
            DisplayStaffMenu(bankId);
        }

        public void DisplayStaffMenu(string bankId)
        {
            Console.Clear();
            string bankName = _bankService.GetBankName(bankId);
            ($"Welcome to {bankName} Bank! You are logged in as Bank Staff.\n").DisplayLine();
            Constants.Staff.Menu.DisplayLine();
            Constants.OptionSelectionPrompt.DisplayLine();
        }

        public void StaffLoginOptions(string bankId)
        {
            DisplayStaffMenu(bankId);
            bool isLoggedIn = true;
            while (isLoggedIn)
            {
                StaffOperations option = (StaffOperations)Helper.GetNumberInput();
                try
                {
                    switch (option)
                    {
                        case StaffOperations.CreateAccount:
                            CreateAccount(bankId);
                            break;
                        case StaffOperations.UpdateAccount:
                            UpdateAccount(bankId);
                            break;
                        case StaffOperations.DeleteAccount:
                            DeleteAccount(bankId);
                            break;
                        case StaffOperations.AddCurrency:
                            AddCurrency(bankId);
                            break;
                        case StaffOperations.UpdateServiceCharges:
                            UpdateCharges(bankId);
                            break;
                        case StaffOperations.ViewTransactionHistory:
                            Constants.Staff.AccountIdPrompt.DisplayLine();
                            string accountId = Console.ReadLine();
                            ViewTransaction(bankId, accountId);
                            ReturnToStaffMenu(bankId);
                            break;
                        case StaffOperations.RevertTransaction:
                            RevertTransaction(bankId);
                            break;
                        case StaffOperations.Logout:
                            isLoggedIn = false;
                            break;
                        default:
                            Constants.OutOfBoundsMessage.DisplayLine();
                            break;
                    }
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException UpEx)
                {
                    Constants.DBUpdateExceptionMessage.DisplayLine();
                    UpEx.ToString().DisplayLine();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException EnEx)
                {
                    Constants.DBUpdateExceptionMessage.DisplayLine();
                    EnEx.ToString().DisplayLine();
                }
                finally
                {
                    ReturnToStaffMenu(bankId);
                }
            }
            Constants.LogOutMessage.DisplayLine();
        }

        public string SelectBank()
        {
            while (true)
            {
                string bankName = Helper.GetTextInput();
                if (_bankService.IsBankAvailable(bankName))
                {
                    return _bankService.GetBankId(bankName);
                }
                else
                {
                    Constants.MainMenu.BankNotAvailableMessage.DisplayLine();
                }
            }      
        }

        public void Login()
        {
            Constants.MainMenu.BankNamePrompt.DisplayLine();
            string bankId = SelectBank();
            Constants.MainMenu.LoginUsernamePrompt.DisplayLine();
            string username = Console.ReadLine();
            if (_bankService.IsStaff(bankId, username))
            {
                Constants.PasswordPrompt.DisplayLine();
                while (true)
                {
                    string password = Console.ReadLine();
                    if (_bankService.IsValidStaffPassword(bankId, password))
                    {

                        StaffLoginOptions(bankId);
                        break;
                    }
                    else
                    {
                        Constants.MainMenu.InvalidPasswordMessage.DisplayLine();
                    }
                }    
            }
            else if(_bankService.IsAccountHolder(username))
            {
                Constants.PasswordPrompt.DisplayLine();
                while (true)
                {
                    string password = Console.ReadLine();
                    if (_bankService.IsValidAccountPassword(username, password))
                    {
                        string accountId = _bankService.GetAccountId(bankId, username);
                        AccountLoginOptions(bankId, accountId);
                        break;
                    }
                    else
                    {
                        Constants.MainMenu.InvalidPasswordMessage.DisplayLine();
                    }
                }
            }
            else
            {
                Constants.MainMenu.InvalidUsernameMessage.DisplayLine();
            }
            ReturnToMainMenu();
        }

        public void CreateBank()
        {
            Constants.MainMenu.BankNamePrompt.DisplayLine();
            string bankName = Helper.GetTextInput();
            if (!_bankService.IsBankAvailable(bankName))
            {
                Constants.MainMenu.StaffUsernamePrompt.DisplayLine();
                string staffUsername = Console.ReadLine();
                Constants.PasswordPrompt.DisplayLine();
                string staffPassword = Helper.GetPasswordInput();
                _bankService.CreateBank(bankName, staffUsername, staffPassword);
                Constants.MainMenu.BankCreationSuccessMessage.DisplayLine();
            }
            else
            {
                Constants.MainMenu.BankAvailableMessage.DisplayLine();
            }
            ReturnToMainMenu();
        }

        public void ReturnToMainMenu()
        {
            Constants.PressKeyPrompt.DisplayLine();
            Console.ReadKey(false);
            DisplayMenu();
        }

        public void DisplayMenu()
        {
            Console.Clear();
            Constants.MainMenu.WelcomeMessage.DisplayLine();
            Constants.MainMenu.Menu.DisplayLine();
            Constants.OptionSelectionPrompt.DisplayLine();
        }

        public void MainMenu()
        {
            DisplayMenu();
            while (true)
            {
                BankOperations option = (BankOperations)Helper.GetNumberInput();
                try
                {
                    switch (option)
                    {
                        case BankOperations.SetupBank:
                            CreateBank();
                            break;
                        case BankOperations.Login:
                            Login();
                            break;
                        case BankOperations.Exit:
                            Environment.Exit(0);
                            break;
                        default:
                            Constants.OutOfBoundsMessage.DisplayLine();
                            break;
                    }
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException UpEx)
                {
                    Constants.DBUpdateExceptionMessage.DisplayLine();
                    UpEx.ToString().DisplayLine();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException EnEx)
                {
                    Constants.DBUpdateExceptionMessage.DisplayLine();
                    EnEx.ToString().DisplayLine();
                }
                finally
                {
                    ReturnToMainMenu();
                }
            }          

        }

        public BankManagementApp(IBankService bankService)
        {
            _bankService = bankService;
        }
    }
}
