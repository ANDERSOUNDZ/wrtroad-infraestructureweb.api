namespace wrtroad_infraestructureweb.api
{
    public partial interface IApplicationService
    {
        string? UserId { get; }
        List<string> Roles { get; }
        bool IsAdmin { get; }
    }
}
