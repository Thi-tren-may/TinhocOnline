using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinhocOnline.Models
{
    public class StudentExamAnalysis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentExamId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Required]
        public DateTime AnalyzedAt { get; set; }

        [Required]
        public int TotalQuestions { get; set; }

        [Required]
        public int CorrectAnswers { get; set; }

        [Required]
        public int WrongAnswers { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal AccuracyPercentage { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string AnalysisResultJson { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("StudentExamId")]
        public StudentExam? StudentExam { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("ExamId")]
        public Exam? Exam { get; set; }
    }
}
