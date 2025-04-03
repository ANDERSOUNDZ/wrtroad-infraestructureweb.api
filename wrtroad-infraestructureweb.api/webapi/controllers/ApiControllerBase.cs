using Microsoft.AspNetCore.Mvc;
using wrtroad_infraestructureweb.api.webapi.DTOs;

namespace wrtroad_infraestructureweb.api.webapi.controllers
{
    public class ApiControllerBase : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        protected ApiControllerBase(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        
        protected ActionResult Success(string message, object data = null, ResponseCodeText codeText = ResponseCodeText.SUCCESS)
        {
            return Ok(new ResponseBase()
            {
                Message = message,
                Code = ResponseCode.OK,
                Data = data,
                CodeText = codeText.ToString()
            });
        }
        protected ActionResult BadRequest(string message, object data = null, ResponseCodeText codeText = ResponseCodeText.BAD_REQUEST)
        {
            return BadRequest(new ResponseBase()
            {
                Message = message,
                Code = ResponseCode.BAD_REQUEST,
                Data = data,
                CodeText = codeText.ToString()
            });
        }
    }
}
