using System;
using System.Collections.Generic;
using System.Text;

namespace Project_02_nov.Service
{
    internal class AdminService
    {
        private const string AdminPassword = "admin";

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            return password == AdminPassword;
        }
    }
}
