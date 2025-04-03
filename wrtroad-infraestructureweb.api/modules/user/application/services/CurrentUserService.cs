namespace wrtroad_infraestructureweb.api
{
    public partial class ApplicationService : IApplicationService
    {
        public string? UserId => _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        public List<string> Roles => _httpContextAccessor.HttpContext?.Items["UserRoles"] as List<string> ?? new List<string>();
        public bool IsAdmin => Roles.Contains("super-admin");
    }
}
