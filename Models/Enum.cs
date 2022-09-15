namespace EDSU_JournalMS.Models
{
    public class Enum
    {
        public enum ArticleStatus
        {
            Pending,
            Accepted,
            Rejected
        }
        public enum Title
        {
            Mr,
            Mrs,
            Ms,
            Dr,
            Prof
        }
        //Post Graduate Progress Report Ranking
        public enum Ranking
        {
            Pending,
            Satisfied,
            Partially_Satisfied,
            Not_Satisfied
        }
        public enum PgProgram
        {
            PGD,
            MSc,
            MPhil,
            MPhil_PhD,
            PhD
        }
        public enum WorksStatus
        {
            Pending,
            In_Progress,
            Completed
        }
        public enum EditorRole
        {
            ArticleReviewerRole,
            ArticleSuperAdminRole
        }
    }
}
