using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using System.Security.Authentication;
using wrtroad_infraestructureweb.api.modules.auth.domain.Irepositories;

namespace wrtroad_infraestructureweb.api.modules.auth.infrastructure.repositories
{
    public class EmailSenderRepository : IEmailSenderRepository
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        public EmailSenderRepository(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null.");
            }
            _smtpServer = configuration["Smtp:Server"] ?? throw new ArgumentException("SMTP server configuration is missing or empty.", nameof(configuration));
            _smtpUser = configuration["Smtp:User"] ?? throw new ArgumentException("SMTP user configuration is missing or empty.", nameof(configuration));
            _smtpPass = configuration["Smtp:Password"] ?? throw new ArgumentException("SMTP password configuration is missing or empty.", nameof(configuration));
            if (!int.TryParse(configuration["Smtp:Port"], out _smtpPort) || _smtpPort <= 0 || _smtpPort > 65535)
            {
                throw new ArgumentException("Invalid SMTP port configuration.", nameof(configuration));
            }
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException("Recipient email address cannot be null or empty.", nameof(to));
            }
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("Email subject cannot be null or empty.", nameof(subject));
            }
            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException("Email body cannot be null or empty.", nameof(body));
            }
            if (!IsValidEmail(to))
            {
                throw new ArgumentException("Invalid recipient email address.", nameof(to));
            }
            if (body.Length > 1024 * 1024) // 1 MB
            {
                throw new ArgumentException("Email body is too large.", nameof(body));
            }
            if (body.Contains("<script>", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Email body contains potentially malicious content.", nameof(body));
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your application", _smtpUser));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();
            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Timeout = 5000;
            try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(message);
            }
            catch (SmtpCommandException ex)
            {
                throw new ApplicationException("Error sending email: SMTP command failed.", ex);
            }
            catch (SmtpProtocolException ex)
            {
                throw new ApplicationException("Error sending email: SMTP protocol error.", ex);
            }
            catch (AuthenticationException ex)
            {
                throw new ApplicationException("Error sending email: Authentication failed.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while sending the email.", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }        
    }
}
