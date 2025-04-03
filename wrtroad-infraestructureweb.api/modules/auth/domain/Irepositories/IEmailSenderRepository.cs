namespace wrtroad_infraestructureweb.api.modules.auth.domain.Irepositories
{
    public interface IEmailSenderRepository
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
