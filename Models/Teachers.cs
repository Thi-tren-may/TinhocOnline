using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinhocOnline.Models
{
    [Table("Teachers")]
    public class Teacher
    {
        [Key]
        public int TeacherID { get; set; }

        // Liên kết đến User (bảng Users)
        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public Users? User { get; set; }

        public string? Department { get; set; }
        public bool? CanShareQuestions { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }


    }
}
