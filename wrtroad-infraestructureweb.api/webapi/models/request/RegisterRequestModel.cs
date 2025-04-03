namespace wrtroad_infraestructureweb.api.webapi.models.request
{
    public class RegisterRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
