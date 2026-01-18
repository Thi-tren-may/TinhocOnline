using System.ComponentModel.DataAnnotations;

namespace TinhocOnline.Models.ViewModels
{
    public class CreateExamViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đề thi")]
        [StringLength(200, ErrorMessage = "Tên đề thi không quá 200 ký tự")]
        public string ExamName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn loại đề thi")]
        public int ExamTypeId { get; set; }

        public string? GradeLevel { get; set; } // "10", "11", "12"

        [Required(ErrorMessage = "Vui lòng nhập thời gian làm bài")]
        [Range(1, 300, ErrorMessage = "Thời gian làm bài từ 1-300 phút")]
        public int Duration { get; set; } = 45;

        [Required(ErrorMessage = "Vui lòng nhập tổng số câu hỏi")]
        [Range(10, 100, ErrorMessage = "Số câu hỏi từ 10-100")]
        public int TotalQuestions { get; set; } = 50;

        // Ma trận độ khó
        [Required(ErrorMessage = "Vui lòng nhập tỷ lệ câu dễ")]
        [Range(0, 100, ErrorMessage = "Tỷ lệ từ 0-100%")]
        public decimal EasyPercentage { get; set; } = 40;

        [Required(ErrorMessage = "Vui lòng nhập tỷ lệ câu trung bình")]
        [Range(0, 100, ErrorMessage = "Tỷ lệ từ 0-100%")]
        public decimal MediumPercentage { get; set; } = 30;

        [Required(ErrorMessage = "Vui lòng nhập tỷ lệ câu khó")]
        [Range(0, 100, ErrorMessage = "Tỷ lệ từ 0-100%")]
        public decimal HardPercentage { get; set; } = 30;

        // Ma trận chủ đề - Chọn topics bằng checkbox (List TopicIds)
        public List<int> SelectedTopicIds { get; set; } = new List<int>();

        // Cấu hình
        public bool ShuffleQuestions { get; set; } = false;
        public bool ShuffleAnswers { get; set; } = false;

        [Range(0, 10, ErrorMessage = "Điểm đạt từ 0-10")]
        public decimal PassingScore { get; set; } = 5.0M;

        public DateTime? StartDate { get; set; } // Thời gian bắt đầu cho phép thi

        public DateTime? EndDate { get; set; } // Thời gian kết thúc kỳ thi

        public string Status { get; set; } = "draft"; // draft, published

        public int CreatedBy { get; set; }

        // Chế độ tạo đề
        public string CreateMode { get; set; } = "quick"; // "quick" hoặc "custom"

        // Custom mode: Danh sách câu hỏi được chọn thủ công
        public List<int> SelectedQuestionIds { get; set; } = new List<int>();

        // Checkbox selections từ trang hiện tại (dùng để merge)
        public List<int>? CurrentPageSelections { get; set; }

        // State phân trang và lọc (cho Custom mode)
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages { get; set; }
        public int TotalQuestionItems { get; set; }
        
        // Bộ lọc
        public int? FilterTopicId { get; set; }
        public string? FilterDifficulty { get; set; }
        public string? FilterGradeLevel { get; set; }

        // Danh sách câu hỏi hiện tại (cho hiển thị)
        public List<QuestionItemDto>? Questions { get; set; }

        // Action button được nhấn
        public string? ActionButton { get; set; }
    }

    // DTO cho câu hỏi
    public class QuestionItemDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public string? GradeLevel { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
