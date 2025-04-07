using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wrtroad_infraestructureweb.api.webapi.models.response;

namespace wrtroad_infraestructureweb.api.webapi.controllers.users
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly IApplicationService _applicationService;
        public UserController(IApplicationService applicationService) : base(applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpGet]
        [Route("")]
        [Authorize(Policy = "Restricted")]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> ObtenerTodos()
        {
            try
            {
                return Ok("hola");
                //var usersResponseList = await _applicationService.GetAllUsersListAsync();
                //return Success("Users response successfully.", usersResponseList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
