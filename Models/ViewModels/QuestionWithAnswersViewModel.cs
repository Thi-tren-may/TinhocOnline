using System.ComponentModel.DataAnnotations;

namespace TinhocOnline.Models.ViewModels
{
    public class QuestionWithAnswersViewModel
    {
        // Question properties
        [Required(ErrorMessage = "Vui lòng chọn chủ đề")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung câu hỏi")]
        public string QuestionText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn độ khó")]
        public string DifficultyLevel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn lớp")]

        public string? GradeLevel { get; set; } // "10", "11", "12"

        [Required(ErrorMessage = "Vui lòng chọn người tạo")]
        public int CreatedBy { get; set; }

        public string Status { get; set; } = "active";

        // Answers properties
        [Required(ErrorMessage = "Vui lòng nhập đáp án A")]
        public string AnswerA { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập đáp án B")]
        public string AnswerB { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập đáp án C")]
        public string AnswerC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập đáp án D")]
        public string AnswerD { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn đáp án đúng")]
        public string CorrectAnswer { get; set; } = string.Empty; // A, B, C, or D
    }
}
