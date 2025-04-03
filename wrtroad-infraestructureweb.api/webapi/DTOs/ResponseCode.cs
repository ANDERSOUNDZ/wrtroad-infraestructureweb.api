using System.ComponentModel;

namespace wrtroad_infraestructureweb.api.webapi.DTOs
{
    public enum ResponseCode : int
    {
        OK = 200,
        BAD_REQUEST = 400,
        INTERNAL_SERVER_ERROR = 500
    }
}
