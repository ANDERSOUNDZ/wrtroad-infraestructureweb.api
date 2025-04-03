using wrtroad_infraestructureweb.api.core.domain.entities;

namespace wrtroad_infraestructureweb.api
{
    partial interface IApplicationService
    {
        Task SendVerificationEmailAsync(UserEntity user);
        Task<bool> VerifyEmailAsync(string token);
    }
}
