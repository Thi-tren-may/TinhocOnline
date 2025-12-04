namespace TinhocOnline.Services.DTOs
{
    public class ExamAnalysisRequest
    {
        public List<TopicDto> Topics { get; set; }
        public List<QuestionDto> Questions { get; set; }
        public List<UserAnswerDto> UserAnswers { get; set; }
        public string Model { get; set; }
    }

    public class TopicDto
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
    }

    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string TopicName { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class UserAnswerDto
    {
        public int QuestionId { get; set; }
        public string UserAnswer { get; set; }
    }

    public class ExamAnalysisResponse
    {
        public List<TopicSummary> TopicSummary { get; set; }
        public List<QuestionFeedback> FeedbackPerQuestion { get; set; }
        public List<string> StudyRecommendations { get; set; }
        public List<string> NextLearningSteps { get; set; }
    }

    public class TopicSummary
    {
        public string Topic { get; set; }
        public int TotalQuestions { get; set; }
        public int Correct { get; set; }
        public int Wrong { get; set; }
        public double Accuracy { get; set; }
    }

    public class QuestionFeedback
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public string Feedback { get; set; }
    }
}
