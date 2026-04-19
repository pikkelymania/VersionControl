using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnitTestExample.Abstractions;
using UnitTestExample.Entities;
using UnitTestExample.Services;

namespace UnitTestExample.Controllers
{
    public class AccountController
    {        
        public IAccountManager AccountManager { get; set; }

        public AccountController()
        {
            AccountManager = new AccountManager();
        }

        public Account Register(string email, string password)
        {
            if (!ValidateEmail(email))
            {
                throw new ValidationException("Invalid email address!");
            }

            if (!ValidatePassword(password))
            {
                throw new ValidationException("A jelszónak legalább 8 karakter hosszúnak kell lennie, és tartalmaznia kell kisbetűt, nagybetűt, valamint számot is!");
            }

            var newAccount = new Account()
            {
                ID = Guid.NewGuid(),
                Email = email,
                Password = password
            };

            AccountManager.CreateAccount(newAccount);

            return newAccount;
        }

        public bool ValidateEmail(string email)
        {            
            return Regex.IsMatch(
                email, 
                @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        }

        public bool ValidatePassword(string password)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"^[a-zA-Z0-9]{8,}$"))
            {
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]"))
            {
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]"))
            {
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]"))
            {
                return false;
            }

            return true;
        }
    }
}
