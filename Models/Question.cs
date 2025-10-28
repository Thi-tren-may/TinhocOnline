using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinhocOnline.Models
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string QuestionText { get; set; }

        [Required]
        [StringLength(20)]
        public string DifficultyLevel { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "active";

        // Navigation properties
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
