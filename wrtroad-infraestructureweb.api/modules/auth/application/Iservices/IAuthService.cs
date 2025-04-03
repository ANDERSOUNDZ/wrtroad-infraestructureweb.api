using wrtroad_infraestructureweb.api.webapi.models.request;
using wrtroad_infraestructureweb.api.webapi.models.response;

namespace wrtroad_infraestructureweb.api
{
    public partial interface IApplicationService
    {
        Task<RegisterRequestModel> RegisterAsync(RegisterRequestModel register);
        Task<LoginResponseModel> LoginAsync(LoginRequestModel login);
    }
}
