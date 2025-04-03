namespace wrtroad_infraestructureweb.api.webapi.models.response
{
    public class LoginResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
