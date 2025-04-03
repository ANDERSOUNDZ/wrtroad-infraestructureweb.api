using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using wrtroad_infraestructureweb.api.modules.auth.application.validators.email;
using wrtroad_infraestructureweb.api.modules.auth.application.validators.password;

namespace wrtroad_infraestructureweb.api.core.application.validators
{
    public static class ValidatorsGenerics
    {
        public static void NotNull<T>(T value, string message)
        {
            if (value == null)
            {
                throw new ValidationException(message);
            }
        }
        public static void NotNullOrWhiteSpace(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException(message);
            }
        }
        public static void IsValidEmail(string email, string message)
        {
            if (string.IsNullOrWhiteSpace(email) || !EmailValidator.IsValidEmail(email))
            {
                throw new ValidationException(message);
            }
        }
        public static void IsValidPassword(string password, string message)
        {
            if (string.IsNullOrWhiteSpace(password) || !PasswordValidator.IsValidPassword(password))
            {
                throw new ValidationException(message);
            }
        }
        public static async Task EnsureUniqueAsync(Func<Task<bool>> condition, string message)
        {
            if (await condition())
            {
                throw new ValidationException(message);
            }
        }
        public static void EnsureState(bool condition, string message)
        {
            if (condition)
            {
                throw new ValidationException(message);
            }
        }
        public static async Task EnsureDateConditionWithActionAsync(bool condition, Func<Task> action, string message)
        {
            if (condition)
            {
                if (action != null)
                {
                    await action();
                }
                throw new ValidationException(message);
            }
        }
    }
}
