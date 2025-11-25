using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TinhocOnline.Models
{
    [Table("StudentExamQuestions")]
    public class StudentExamQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentExamQuestionId { get; set; }

        [Required]
        public int StudentExamId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int QuestionOrder { get; set; } // Thứ tự câu hỏi trong bài thi của học sinh này

        // Navigation properties
        [ValidateNever]
        [ForeignKey("StudentExamId")]
        public virtual StudentExam StudentExam { get; set; }

        [ValidateNever]
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
    }
}
