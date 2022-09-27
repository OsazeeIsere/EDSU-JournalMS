using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using EDSU_JournalMS.Models;

namespace EDSU_JournalMS.Authorization
{
    public class ArticleSuperAdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, EDSUJournal>
    {
        protected override Task HandleRequirementAsync(
           AuthorizationHandlerContext context,
           OperationAuthorizationRequirement requirement,
           EDSUJournal Journal)
        {

            if (context.User == null || Journal == null)
                return Task.CompletedTask;

            if (requirement.Name != Constants.AcceptedForReviewOperationName &&
                requirement.Name != Constants.RejectedOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(Constants.ArticleSuperAdminRole))
                context.Succeed(requirement);

            return Task.CompletedTask;

        }
    }
}
