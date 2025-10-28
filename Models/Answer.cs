using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinhocOnline.Models
{
    [Table("Answers")]
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string AnswerText { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        [StringLength(1)]
        public string AnswerOrder { get; set; }

        // Navigation properties
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
    }
}
