using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace EDSU_JournalMS.Authorization
{
    public class ArticleOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
        public static OperationAuthorizationRequirement Accepted =
            new OperationAuthorizationRequirement { Name = Constants.AcceptedOperationName };
        public static OperationAuthorizationRequirement Rejected =
            new OperationAuthorizationRequirement { Name = Constants.RejectedOperationName };

    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";

        public static readonly string AcceptedOperationName = "Accepted";
        public static readonly string RejectedOperationName = "Rejected";

        public static readonly string ArticleReviewerRole = "ArticleReviewer";
        public static readonly string ArticleSuperAdminRole = "ArticleSuperAdmin";

    }
}
