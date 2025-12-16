using System.Text.Json.Serialization;

namespace TinhocOnline.Services.DTOs
{
    public class AnalysisResultDto
    {
        [JsonPropertyName("topicSummary")]
        public List<TopicSummaryDto> TopicSummary { get; set; } = new();

        [JsonPropertyName("feedbackPerQuestion")]
        public List<QuestionFeedbackDto> FeedbackPerQuestion { get; set; } = new();

        [JsonPropertyName("studyRecommendations")]
        public List<string> StudyRecommendations { get; set; } = new();

        [JsonPropertyName("nextLearningSteps")]
        public List<string> NextLearningSteps { get; set; } = new();
    }

    public class TopicSummaryDto
    {
        [JsonPropertyName("topic")]
        public string Topic { get; set; } = string.Empty;

        [JsonPropertyName("totalQuestions")]
        public int TotalQuestions { get; set; }

        [JsonPropertyName("correct")]
        public int Correct { get; set; }

        [JsonPropertyName("wrong")]
        public int Wrong { get; set; }

        [JsonPropertyName("accuracy")]
        public decimal Accuracy { get; set; }
    }

    public class QuestionFeedbackDto
    {
        [JsonPropertyName("questionId")]
        public int QuestionId { get; set; }

        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; } = string.Empty;

        [JsonPropertyName("userAnswer")]
        public string UserAnswer { get; set; } = string.Empty;

        [JsonPropertyName("correctAnswer")]
        public string CorrectAnswer { get; set; } = string.Empty;

        [JsonPropertyName("isCorrect")]
        public bool IsCorrect { get; set; }

        [JsonPropertyName("feedback")]
        public string Feedback { get; set; } = string.Empty;
    }
}
