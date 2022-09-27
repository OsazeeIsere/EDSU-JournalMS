using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EDSU_JournalMS.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDSU_JournalMS.Models
{
    [Table("JournalEditors")]
    public class JournalEditor
    {
        
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? OtherName { get; set; }
        public Title Title { get; set; }
        public string Role { get; set; }
        public string? AreaOfSpecialization { get; set; }
        public string? EditorEmail { get; set; }
        public string? Tel { get; set; }
        public string? Image { get; set; }
        public string? Institution { get; set; }
       // public AspNetUsers Role { get; set; }
    }
}
