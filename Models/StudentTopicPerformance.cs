using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinhocOnline.Models
{
    public class StudentTopicPerformance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string TopicName { get; set; } = string.Empty;

        [Required]
        public int TotalAttempts { get; set; }

        [Required]
        public int TotalQuestions { get; set; }

        [Required]
        public int CorrectAnswers { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal AccuracyPercentage { get; set; }

        [Required]
        public DateTime LastAttemptDate { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
