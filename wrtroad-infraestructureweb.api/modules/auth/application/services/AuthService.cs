using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using wrtroad_infraestructureweb.api.core.application.validators;
using wrtroad_infraestructureweb.api.core.domain.entities;
using wrtroad_infraestructureweb.api.modules.auth.application.helpers.password;
using wrtroad_infraestructureweb.api.modules.auth.domain.entities;
using wrtroad_infraestructureweb.api.webapi.models.request;
using wrtroad_infraestructureweb.api.webapi.models.response;

namespace wrtroad_infraestructureweb.api
{
    public partial class ApplicationService : IApplicationService
    {
        public async Task<RegisterRequestModel> RegisterAsync(RegisterRequestModel newRegister)
        {
            var userEntity = _mapper.Map<UserEntity>(newRegister);
            ValidatorsGenerics.NotNull(userEntity, "The user object cannot be null.");
            ValidatorsGenerics.NotNullOrWhiteSpace(userEntity.Username, "The username cannot be empty or whitespace.");
            ValidatorsGenerics.NotNullOrWhiteSpace(userEntity.Email, "Email is required. Please provide a valid email address.");
            ValidatorsGenerics.NotNullOrWhiteSpace(userEntity.Password, "The password cannot be empty or whitespace.");
            ValidatorsGenerics.IsValidEmail(userEntity.Email, "Email is required and must be in a valid format (e.g., example@domain.com).");
            ValidatorsGenerics.IsValidPassword(userEntity.Password, "Password is required and must meet security requirements (e.g., minimum length, special characters, etc.).");
            await ValidatorsGenerics.EnsureUniqueAsync(
                () => _dbContext.Users.AnyAsync(u => u.Email == userEntity.Email),
                "This email is already in use. Please use a different email address.");
            await ValidatorsGenerics.EnsureUniqueAsync(
                () => _dbContext.Users.AnyAsync(u => u.Username == userEntity.Username),
                "This username is already taken. Please choose a different one.");
            string salt = GenerateHashPassUtils.GenerateSalt();
            string passwordEncrypt = GenerateHashPassUtils.HashPassword(userEntity.Password, salt);
            try
            {
                var userRegister = new UserEntity
                {
                    Username = userEntity.Username,
                    Email = userEntity.Email,
                    Password = passwordEncrypt,
                    Salt = salt,
                    DateRegister = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                };
                await _authRepository.AddRegisterUserAsync(userRegister);
                var userRoleRegister = new UserRoleEntity
                {
                    UserId = userRegister.Id
                };
                await _authRepository.AddUserRolesAsync(userRoleRegister);
                await Task.Run(() => SendVerificationEmailAsync(userRegister));
                return _mapper.Map<RegisterRequestModel>(userRegister);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel login)
        {
            ValidatorsGenerics.NotNull(login, "The login object cannot be null. Please provide a valid login object.");
            ValidatorsGenerics.NotNullOrWhiteSpace(login.Email, "Email is required. Please provide a valid email address.");
            ValidatorsGenerics.NotNullOrWhiteSpace(login.Password, "Password cannot be empty.");
            ValidatorsGenerics.IsValidEmail(login.Email, "Email is required and must be in a valid format (e.g., example@domain.com).");
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            ValidatorsGenerics.EnsureState(user == null || !GenerateHashPassUtils.VerifyHashedPassword(login.Password, user.Salt, user.Password), "User not found. Please check your email and password.");
            try
            {
                var userRoles = await _authRepository.GetUserRolesAsync(user.Id);
                var token = GenerateJwtToken(user, userRoles);
                return new LoginResponseModel
                {
                    Success = true,
                    Message = "Token generate.",
                    Token = token,
                    TokenExpiration = DateTime.Parse(DateTime.Now.AddHours(1).ToString("yyyy/MM/dd HH:mm:ss"))
                };
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
        private string GenerateJwtToken(UserEntity user, IEnumerable<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            int expiresInHours = int.Parse(_configuration["JwtConfig:ExpiresInHours"]);
            int expiresInMinutes = expiresInHours * 60;
            var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["JwtConfig:Issuer"],
                _configuration["JwtConfig:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
