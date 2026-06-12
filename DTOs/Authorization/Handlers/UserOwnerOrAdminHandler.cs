using HospitalSystem.Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HospitalSystem.Infrastructure.Authorization.Handlers
{
    public class UserOwnerOrAdminHandler : AuthorizationHandler<UserOwnerOrAdminRequirement, int>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserOwnerOrAdminRequirement requirement,
        int userID)
        {
            // Admin override
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Ownership check
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int authenticatedStudentId) &&
                authenticatedStudentId == userID)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}