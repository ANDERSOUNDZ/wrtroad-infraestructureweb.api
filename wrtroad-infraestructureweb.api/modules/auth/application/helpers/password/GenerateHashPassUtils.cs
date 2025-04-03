using System.Security.Cryptography;
using System.Text;

namespace wrtroad_infraestructureweb.api.modules.auth.application.helpers.password
{
    public static class GenerateHashPassUtils
    {
        public static string GenerateSalt()
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                return Convert.ToBase64String(hmac.Key);
            }
        }
        public static string HashPassword(string password, string salt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(Convert.FromBase64String(salt)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
        public static bool VerifyHashedPassword(string password, string salt, string hashedPassword)
        {
            string computedHash = HashPassword(password, salt);
            return hashedPassword == computedHash;
        }
    }
}
