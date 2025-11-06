using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinhocOnline.Models
{
    [Table("StudentAnswers")]
    public class StudentAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentAnswerId { get; set; }

        [Required]
        public int StudentExamId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public int? AnswerId { get; set; }

        public bool? IsCorrect { get; set; }

        // Navigation properties
        [ForeignKey("StudentExamId")]
        public virtual StudentExam StudentExam { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        [ForeignKey("AnswerId")]
        public virtual Answer Answer { get; set; }
    }
}
