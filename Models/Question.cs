using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TinhocOnline.Models
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        // Câu hỏi thuộc chủ đề nào
        [Required]
        public int TopicId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string QuestionText { get; set; }

        [Required]
        [StringLength(20)]
        public string DifficultyLevel { get; set; }

        [StringLength(10)]
        public string? GradeLevel { get; set; } // "10", "11", "12"

        [Required]
        public int CreatedBy { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "active";

        // Navigation properties
        [ValidateNever]
        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }

        [ValidateNever]
        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }

        [ValidateNever]
        public virtual ICollection<Answer> Answers { get; set; }
        
        [ValidateNever]
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        
        [ValidateNever]
        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; }
    }
}
