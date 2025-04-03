using Microsoft.EntityFrameworkCore;
using wrtroad_infraestructureweb.api.core.domain.entities;
using wrtroad_infraestructureweb.api.core.infrastructure.data.context;
using wrtroad_infraestructureweb.api.modules.auth.domain.entities;
using wrtroad_infraestructureweb.api.modules.auth.domain.Irepositories;

namespace wrtroad_infraestructureweb.api.modules.auth.infrastructure.repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly WrtRoadDbContext _dbContext;
        public AuthRepository(WrtRoadDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EmailVerificationEntity> GetByTokenAsync(string token)
        {
            try
            {
                return await _dbContext.EmailVerifications.FirstOrDefaultAsync(ev => ev.VerificationToken == token);
            }
            catch (Exception ex)
            {
                throw new Exception("Error to find token. Please try again later.", ex);
            }
        }
        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            var roles = await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
            return roles;
        }
        public async Task<UserEntity> AddRegisterUserAsync(UserEntity register)
        {
            try
            {
                await _dbContext.Users.AddAsync(register);
                await _dbContext.SaveChangesAsync();
                return register;
            }
            catch (Exception ex)
            {
                throw new Exception("Error authentication register user. Please try again later.", ex);
            }
        }
        public async Task<UserRoleEntity> AddUserRolesAsync(UserRoleEntity userRole)
        {
            try
            {
                await _dbContext.UserRoles.AddAsync(userRole);
                await _dbContext.SaveChangesAsync();
                return userRole;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all user roles. Please try again later.", ex);
            }
        }
        public async Task AddEmailVerificationAsync(EmailVerificationEntity entity)
        {
            try
            {
                await _dbContext.EmailVerifications.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error add token. Please try again later.", ex);
            }
        }
        public async Task UpdateEmailVerificationAsync(EmailVerificationEntity entity)
        {
            try
            {
                _dbContext.EmailVerifications.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error update token. Please try again later.", ex);
            }
        }
    }
}
