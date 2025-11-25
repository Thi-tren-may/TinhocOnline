using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinhocOnline.Models
{
    [Table("StudentExams")]
    public class StudentExam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentExamId { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? SubmittedAt { get; set; } // Thời điểm nộp bài

        public int? DurationTaken { get; set; } // Thời gian làm bài thực tế (phút)

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Score { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "in_progress";

        // Navigation properties
        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        [ForeignKey("StudentId")]
        public virtual User Student { get; set; }

        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; }

        public virtual ICollection<StudentExamQuestion> StudentExamQuestions { get; set; }
    }
}
