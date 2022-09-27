using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using EDSU_JournalMS.Models;

namespace EDSU_JournalMS.Authorization
{
    public class ArticleCreatorAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, EDSUJournal>
    {
        UserManager<IdentityUser> _userManager;
        public ArticleCreatorAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, EDSUJournal Journal)
        {
            if (context.User == null || Journal == null)
                return Task.CompletedTask;

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName ||
                requirement.Name == Constants.DeleteOperationName ||
                requirement.Name == Constants.RejectedOperationName ||
                requirement.Name == Constants.AcceptedForReviewOperationName)
            {
                return Task.CompletedTask;
            }
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
