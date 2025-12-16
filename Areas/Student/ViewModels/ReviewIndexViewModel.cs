namespace TinhocOnline.Areas.Student.ViewModels
{
    public class ReviewIndexViewModel
    {
        public StudentProgress Progress { get; set; } = new();
        public List<TopicRecommendation> RecommendedTopics { get; set; } = new();
        public List<RecentAnalysis> RecentAnalyses { get; set; } = new();
        public OverallAssessment? Assessment { get; set; }
    }

    public class StudentProgress
    {
        public int TotalExamsTaken { get; set; }
        public decimal AverageAccuracy { get; set; }
        public int TotalQuestionsAnswered { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public DateTime? LastExamDate { get; set; }
    }

    public class TopicRecommendation
    {
        public string TopicName { get; set; } = string.Empty;
        public decimal AccuracyPercentage { get; set; }
        public int TotalAttempts { get; set; }
        public int TotalQuestions { get; set; }
        public string Priority { get; set; } = string.Empty; // "High", "Medium", "Low"
    }

    public class RecentAnalysis
    {
        public int StudentExamId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public DateTime AnalyzedAt { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal AccuracyPercentage { get; set; }
    }

    public class OverallAssessment
    {
        public string Trend { get; set; } = string.Empty;
        public string Strengths { get; set; } = string.Empty;
        public string Weaknesses { get; set; } = string.Empty;
        public List<string> Recommendations { get; set; } = new();
    }
}
