using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDSU_JournalMS.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string ReviewerEmail { get; set; }
        public JournalEditor? JournalEditors { get; set; }
        [ForeignKey("EDSUJournals")]
        public int JournalId { get; set; }
        public EDSUJournal? Journals { get; set; }
        public string? Title { get; set; }
        public string? Abstract { get; set; }
        public string ReviewerNo { get; set; }
        [StringLength(2500)]
        [Column(TypeName = "varchar")]
        public string? CommentBody { get; set; }
       
        public DateTime CommentDate { get; set; }=DateTime.Now;

    }
}
