using System.Security.Cryptography;
using System.Text;

namespace PeakeyeBackendCase.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string storedHash, string inputPassword)
        {
            return inputPassword == storedHash;

        }
    }
}
