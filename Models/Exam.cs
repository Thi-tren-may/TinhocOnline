using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinhocOnline.Models
{
    [Table("Exams")]
    public class Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamId { get; set; }

        [Required]
        public int SubjectId { get; set; }

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

        [Required]
        public int CreatedBy { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "draft";

        // Navigation properties
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User Creator { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<StudentExam> StudentExams { get; set; }
    }
}
