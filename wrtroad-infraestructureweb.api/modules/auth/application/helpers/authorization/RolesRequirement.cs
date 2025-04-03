using Microsoft.AspNetCore.Authorization;

namespace wrtroad_infraestructureweb.api.modules.auth.application.helpers.authorization
{
    public class RolesRequirement : IAuthorizationRequirement
    {
        public string[] AllowedRoles { get; }

        public RolesRequirement(params string[] allowedRoles)
        {
            AllowedRoles = allowedRoles;
        }
    }
}
