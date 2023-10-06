using Microsoft.AspNetCore.Identity;
using sib_love_site.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sib_love_site.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Content { get; set; }

        //Relations

        [Required]
        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? User { get; set; }

        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }


    }
}
