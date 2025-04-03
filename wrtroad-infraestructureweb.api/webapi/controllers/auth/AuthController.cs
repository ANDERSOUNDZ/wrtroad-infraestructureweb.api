using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wrtroad_infraestructureweb.api.webapi.models.request;

namespace wrtroad_infraestructureweb.api.webapi.controllers.auth
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IApplicationService _applicationService;
        public AuthController(IApplicationService applicationService) : base(applicationService)
        {
            _applicationService = applicationService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestModel registerUser)
        {
            try
            {
                var userAuthCreate = await _applicationService.RegisterAsync(registerUser);
                return Success("User created successfully. Please review your email for your link activated account.", userAuthCreate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            try
            {
                var userAuthCreate = await _applicationService.VerifyEmailAsync(token);
                return Success("Account activated succesfull.", userAuthCreate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel login)
        {
            try
            {
                var userAuthLogin = await _applicationService.LoginAsync(login);
                return Success("User login successfully.", userAuthLogin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
