using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TinhocOnline.Models
{
    [Table("ExamTopics")]
    public class ExamTopic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamTopicId { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Required]
        public int TopicId { get; set; }

        // Số câu hỏi từ chủ đề này trong đề thi
        public int QuestionCount { get; set; }

        // Navigation properties
        [ValidateNever]
        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        [ValidateNever]
        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
    }
}
