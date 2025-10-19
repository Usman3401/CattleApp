using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Services
{
    public static class AuthenticationService
    {
        public static bool ValidateUser(string username, string password)
        {
            // Mock: Replace with DB check
            return username == "admin" && password == "1";
        }
    }
}
