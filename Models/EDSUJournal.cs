using System.ComponentModel.DataAnnotations.Schema;
using static EDSU_JournalMS.Models.Enum;

namespace EDSU_JournalMS.Models
{
    [Table("EDSUJournals")]

    public class EDSUJournal
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Authors { get; set; }
        public string? Keywords { get; set; }
        public string? Abstract { get; set; }
        public string? NoOfPages { get; set; }
        public string? Upload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UploadedBy { get; set; }
        public ArticleStatus Status { get; set; }

        [ForeignKey("JournalEditors")]
        public int? JournalEditorId { get; set; }
        public JournalEditor? JournalEditors { get; set; }
        public string? UploaderId { get; set; }
        public string? FirstReviewer { get; set; }
        public string? SecondReviewer { get; set; }
        public string? ThirdReviewer { get; set; }
        public string? FourthReviewer { get; set; }
        public string? FifthReviewer { get; set; }

    }
}
