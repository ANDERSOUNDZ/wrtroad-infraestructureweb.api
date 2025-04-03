using System.ComponentModel;

namespace wrtroad_infraestructureweb.api.webapi.DTOs
{
    public enum ResponseCodeText
    {
        [Description("OK")]
        OK,
        [Description("SUCCESS")]
        SUCCESS,
        [Description("BAD_REQUEST")]
        BAD_REQUEST,
        [Description("INTERNAL_SERVER_ERROR")]
        INTERNAL_SERVER_ERROR
    }
}
