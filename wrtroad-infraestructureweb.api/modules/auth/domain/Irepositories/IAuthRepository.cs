using wrtroad_infraestructureweb.api.core.domain.entities;
using wrtroad_infraestructureweb.api.modules.auth.domain.entities;

namespace wrtroad_infraestructureweb.api.modules.auth.domain.Irepositories
{
    public interface IAuthRepository
    {
        Task<EmailVerificationEntity> GetByTokenAsync(string token);
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<UserEntity> AddRegisterUserAsync(UserEntity register);
        Task<UserRoleEntity> AddUserRolesAsync(UserRoleEntity userRole);
        Task AddEmailVerificationAsync(EmailVerificationEntity entity);
        Task UpdateEmailVerificationAsync(EmailVerificationEntity entity);
    }
}
