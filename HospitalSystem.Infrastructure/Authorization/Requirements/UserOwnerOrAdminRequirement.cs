using Microsoft.AspNetCore.Authorization;

namespace HospitalSystem.Infrastructure.Authorization.Requirements
{
    public class UserOwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}