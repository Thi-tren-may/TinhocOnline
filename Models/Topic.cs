using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TinhocOnline.Models
{
    [Table("Topics")]
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicId { get; set; }

        [Required]
        [StringLength(10)]
        public string TopicCode { get; set; } // A, B, C, D, E, F, G

        [Required]
        [StringLength(200)]
        public string TopicName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "active";

        // Navigation properties
        [ValidateNever]
        public virtual ICollection<Question> Questions { get; set; }
        
        [ValidateNever]
        public virtual ICollection<ExamTopic> ExamTopics { get; set; }
    }
}
