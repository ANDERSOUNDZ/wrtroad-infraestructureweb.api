using System.Text.RegularExpressions;

namespace wrtroad_infraestructureweb.api.modules.auth.application.validators.password
{
    public static class PasswordValidator
    {
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            bool hasMinimumLength = password.Length >= 8;
            bool hasUpperCase = Regex.IsMatch(password, "[A-Z]");
            bool hasLowerCase = Regex.IsMatch(password, "[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");
            bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]");
            return hasMinimumLength && hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }
    }
}
