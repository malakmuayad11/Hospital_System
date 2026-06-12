using Microsoft.AspNetCore.Authorization;

namespace HospitalSystem.Infrastructure.Authorization.Requirements
{
    public class HasUserPermissionsRequirement : IAuthorizationRequirement
    {
        public int Permissions { get; }

        public HasUserPermissionsRequirement(int Permissions) => this.Permissions = Permissions;
    }
}