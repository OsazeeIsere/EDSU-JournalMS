using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using EDSU_JournalMS.Models;

namespace EDSU_JournalMS.Authorization
{
    public class ArticleManagerAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, EDSUJournal>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            EDSUJournal article)
        {

            if (context.User == null || article == null)
                return Task.CompletedTask;

            if (requirement.Name != Constants.AcceptedForReviewOperationName &&
                requirement.Name != Constants.RejectedOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(Constants.ArticleReviewerRole))
                context.Succeed(requirement);

            return Task.CompletedTask;

        }


    }
}
