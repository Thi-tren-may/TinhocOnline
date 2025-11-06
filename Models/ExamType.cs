using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TinhocOnline.Models
{
    [Table("ExamTypes")]
    public class ExamType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; } // "Kiểm tra 15 phút", "Giữa kỳ"...

        [Required]
        [StringLength(20)]
        public string TypeCode { get; set; } // "quiz_15min", "midterm", "final"

        [Required]
        public int DefaultDuration { get; set; } // Thời gian mặc định (phút)

        [Required]
        public int DefaultTotalQuestions { get; set; } // Số câu mặc định

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ValidateNever]
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
