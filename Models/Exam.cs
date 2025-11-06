using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TinhocOnline.Models
{
    [Table("Exams")]
    public class Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamId { get; set; }

        [Required]
        [StringLength(200)]
        public string ExamName { get; set; }

        [Required]
        public int Duration { get; set; }

        public int TotalQuestions { get; set; } = 50;

        [Column(TypeName = "decimal(5,2)")]
        public decimal EasyPercentage { get; set; } = 60;

        [Column(TypeName = "decimal(5,2)")]
        public decimal MediumPercentage { get; set; } = 30;

        [Column(TypeName = "decimal(5,2)")]
        public decimal HardPercentage { get; set; } = 10;

        [StringLength(10)]
        public string? GradeLevel { get; set; } // "10", "11", "12"

        public int? ExamTypeId { get; set; } // FK to ExamTypes

        public bool ShuffleQuestions { get; set; } = false; // Trộn câu hỏi

        public bool ShuffleAnswers { get; set; } = false; // Trộn đáp án

        [Column(TypeName = "decimal(4,2)")]
        public decimal PassingScore { get; set; } = 5.0M; // Điểm đạt (mặc định 5/10)

        [Required]
        public int CreatedBy { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "draft";

        // Navigation properties
        [ValidateNever]
        [ForeignKey("ExamTypeId")]
        public virtual ExamType? ExamType { get; set; }

        [ValidateNever]
        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }

        [ValidateNever]
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        
        [ValidateNever]
        public virtual ICollection<StudentExam> StudentExams { get; set; }
        
        [ValidateNever]
        public virtual ICollection<ExamTopic> ExamTopics { get; set; }
    }
}
