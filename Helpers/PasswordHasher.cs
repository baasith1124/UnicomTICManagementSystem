using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "PasswordHasher.HashPassword");
                throw new Exception("Error occurred while hashing the password.");
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "PasswordHasher.VerifyPassword");
                return false; // Safe fallback – deny login if verification fails
            }
        }
    }
}
