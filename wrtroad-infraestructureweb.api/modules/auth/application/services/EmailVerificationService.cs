using System.ComponentModel.DataAnnotations;
using System.Text;
using wrtroad_infraestructureweb.api.core.application.validators;
using wrtroad_infraestructureweb.api.core.domain.entities;
using wrtroad_infraestructureweb.api.modules.auth.domain.entities;

namespace wrtroad_infraestructureweb.api
{
    public partial class ApplicationService : IApplicationService
    {
        private readonly string _emailVerificationUrl = "https://localhost:7035/api/Auth/verify-email";
        public async Task SendVerificationEmailAsync(UserEntity user)
        {
            ValidatorsGenerics.NotNull(user, "The user object cannot be null.");
            ValidatorsGenerics.NotNullOrWhiteSpace(user.Email, "The email cannot be empty or whitespace.");
            try
            {
                var emailVerification = new EmailVerificationEntity
                {
                    UserId = user.Id,
                    VerificationToken = Guid.NewGuid().ToString(),
                    ExpirationDate = DateTime.Parse(DateTime.Now.AddHours(24).ToString("yyyy/MM/dd HH:mm:ss"))
                };
                await _authRepository.AddEmailVerificationAsync(emailVerification);
                var emailBody = new StringBuilder();
                emailBody.AppendLine($"<h1>Hello {user.Username}!</h1>");
                emailBody.AppendLine("<p>Thank you for signing up with WRTROAD.</p>");
                emailBody.AppendLine("<p>To verify your email address, please click the link below:</p>");
                emailBody.AppendLine($"<a href='{_emailVerificationUrl}?token={emailVerification.VerificationToken}'>Verify email address</a>");
                emailBody.AppendLine("<p>If you did not request this email, please ignore it.</p>");
                await _emailSenderRepository.SendEmailAsync(user.Email, "Email Verification", emailBody.ToString());
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
        public async Task<bool> VerifyEmailAsync(string token)
        {
            ValidatorsGenerics.NotNullOrWhiteSpace(token, "The token cannot be empty or whitespace.");
            var verification = await _authRepository.GetByTokenAsync(token);
            ValidatorsGenerics.NotNull(verification, "The token does not exist. Please provide a valid token.");
            ValidatorsGenerics.EnsureState(verification.IsVerified, "The user has already activated the account.");
            await ValidatorsGenerics.EnsureDateConditionWithActionAsync(
                DateTime.Now > verification.ExpirationDate.AddHours(10),
                async () => await HandleExpiredTokenAsync(verification),
                "The email verification token has expired. A new activation link has been sent to your email."
                );
            try
            {
                verification.IsVerified = true;
                await _authRepository.UpdateEmailVerificationAsync(verification);
                return true;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
        private async Task HandleExpiredTokenAsync(EmailVerificationEntity verification)
        {
            var newToken = Guid.NewGuid().ToString();
            verification.VerificationToken = newToken;
            verification.ExpirationDate = DateTime.Parse(DateTime.Now.AddHours(1).ToString("yyyy/MM/dd HH:mm:ss"));
            await _authRepository.UpdateEmailVerificationAsync(verification);
            var user = await _dbContext.Users.FindAsync(verification.UserId);
            ValidatorsGenerics.NotNull(user, "No user was found associated with the provided token.");
            var emailBody = new StringBuilder();
            emailBody.AppendLine($"<h1>Hello {user.Username}!</h1>");
            emailBody.AppendLine("<p>Thank you for signing up with WRTROAD.</p>");
            emailBody.AppendLine("<p>To verify your email address, please click the link below:</p>");
            emailBody.AppendLine($"<a href='{_emailVerificationUrl}?token={newToken}'>Verify email address</a>");
            emailBody.AppendLine("<p>If you did not request this email, please ignore it.</p>");
            await _emailSenderRepository.SendEmailAsync(user.Email, "Email Verification", emailBody.ToString());
        }
    }
}