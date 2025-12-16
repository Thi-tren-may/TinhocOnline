using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TinhocOnline.Services.DTOs;
using TinhocOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace TinhocOnline.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _defaultModel;
        private readonly DataContext _context;

        public GeminiService(IConfiguration configuration, HttpClient httpClient, DataContext context)
        {
            _httpClient = httpClient;
            _context = context;
            _apiKey = configuration["Gemini:ApiKey"] ?? throw new ArgumentNullException("Gemini API Key not configured");
            _baseUrl = configuration["Gemini:BaseUrl"] ?? "https://generativelanguage.googleapis.com/v1beta/models";
            _defaultModel = configuration["Gemini:DefaultModel"] ?? "gemini-2.0-flash";
        }

        public async Task<string> GenerateContentAsync(string prompt, string model = null)
        {
            try
            {
                var selectedModel = model ?? _defaultModel;
                
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var requestUrl = $"{_baseUrl}/{selectedModel}:generateContent?key={_apiKey}";
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Gemini API error: {response.StatusCode} - {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                // Extract text from response
                var text = jsonResponse
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling Gemini API: {ex.Message}", ex);
            }
        }

        public async Task<string> AnalyzeExamResultAsync(ExamAnalysisRequest request)
        {
            // Format dữ liệu để gửi cho AI
            var topicsText = string.Join("\n", request.Topics.Select(t => $"- ID: {t.TopicId}, Tên: {t.TopicName}"));
            
            var questionsText = string.Join("\n", request.Questions.Select(q => 
                $"- ID: {q.QuestionId}, Chủ đề: {q.TopicName}, Câu hỏi: {q.QuestionText}, Đáp án đúng: {q.CorrectAnswer}"));
            
            var userAnswersText = string.Join("\n", request.UserAnswers.Select(ua => 
                $"- Câu {ua.QuestionId}: Trả lời '{ua.UserAnswer}'"));

            var prompt = $@"
            Bạn là hệ thống chấm thi thông minh. 
            YÊU CẦU QUAN TRỌNG: **Trả lời NGẮN NHẤT có thể nhưng vẫn đầy đủ thông tin. Không giải thích dài dòng. Không kể chuyện. Không đưa thông tin thừa.**

            Nhiệm vụ:

            1. Phân tích bài thi:
            - Xác định đúng/sai.
            - Tổng kết theo chủ đề (đúng, sai, độ chính xác).

            2. Feedback cho từng câu sai (ngắn gọn tối đa 1–2 câu):
            - Trả về đầy đủ text câu hỏi (questionText).
            - Vì sao sai.
            - Kiến thức cần nhớ.

            3. Recommend chủ đề cần ôn:
            - Chỉ được chọn từ 7 chủ đề sau:
                - Máy tính và xã hội tri thức
                - Mạng máy tính và Internet
                - Tổ chức lưu trữ, tìm kiếm và trao đổi thông tin
                - Đạo đức, pháp luật và văn hóa trong môi trường số
                - Ứng dụng tin học
                - Giải quyết vấn đề với sự trợ giúp của máy tính
                - Hướng nghiệp với tin học
            - Tối đa 3 chủ đề.
            - Mô tả cực ngắn (tối đa 1 câu).

            4. Gợi ý 3 bước học tiếp theo (rất ngắn).

            DỮ LIỆU ĐẦU VÀO:
            ----------------
            Danh sách chủ đề:
            {topicsText}

            Danh sách câu hỏi:
            {questionsText}

            Bài làm của học sinh:
            {userAnswersText}

            YÊU CẦU TRẢ VỀ JSON NGẮN GỌN:
            ------------------------------
            {{
            ""topicSummary"": [
                {{
                ""topic"": ""Tên chủ đề"",
                ""totalQuestions"": 0,
                ""correct"": 0,
                ""wrong"": 0,
                ""accuracy"": 0
                }}
            ],
            ""feedbackPerQuestion"": [
                {{
                ""questionId"": 1,
                ""questionText"": ""Text đầy đủ của câu hỏi"",
                ""userAnswer"": """",
                ""correctAnswer"": """",
                ""isCorrect"": false,
                ""feedback"": ""Ngắn gọn 1–2 câu""
                }}
            ],
            ""studyRecommendations"": [
                ""Tên chủ đề 1"",
                ""Tên chủ đề 2""
            ],
            ""nextLearningSteps"": [
                ""Bước 1"",
                ""Bước 2"",
                ""Bước 3""
            ]
            }}
            \";

            return await GenerateContentAsync(prompt, request.Model);
        }

        public async Task<StudentExamAnalysis> SaveAnalysisResultAsync(
            int studentExamId, 
            int userId, 
            int examId, 
            string analysisJson)
        {
            try
            {
                // Clean JSON response (loại bỏ markdown code blocks nếu có)
                string cleanedJson = CleanJsonResponse(analysisJson);

                // Parse JSON từ AI
                var analysisResult = JsonSerializer.Deserialize<AnalysisResultDto>(cleanedJson);
                if (analysisResult == null)
                {
                    throw new Exception("Failed to parse analysis result");
                }

                // Tính toán thống kê tổng quan
                var totalQuestions = analysisResult.FeedbackPerQuestion.Count;
                var correctAnswers = analysisResult.FeedbackPerQuestion.Count(q => q.IsCorrect);
                var wrongAnswers = totalQuestions - correctAnswers;
                var accuracy = totalQuestions > 0 ? (decimal)correctAnswers / totalQuestions * 100 : 0;

                // Lưu StudentExamAnalysis
                var examAnalysis = new StudentExamAnalysis
                {
                    StudentExamId = studentExamId,
                    UserId = userId,
                    ExamId = examId,
                    AnalyzedAt = DateTime.Now,
                    TotalQuestions = totalQuestions,
                    CorrectAnswers = correctAnswers,
                    WrongAnswers = wrongAnswers,
                    AccuracyPercentage = Math.Round(accuracy, 2),
                    AnalysisResultJson = analysisJson
                };

                _context.StudentExamAnalyses.Add(examAnalysis);
                await _context.SaveChangesAsync();

                // Cập nhật hoặc tạo mới StudentTopicPerformance
                await UpdateTopicPerformanceAsync(userId, analysisResult.TopicSummary);

                return examAnalysis;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving analysis result: {ex.Message}", ex);
            }
        }

        private async Task UpdateTopicPerformanceAsync(int userId, List<TopicSummaryDto> topicSummaries)
        {
            foreach (var topic in topicSummaries)
            {
                // Tìm performance hiện tại
                var performance = await _context.StudentTopicPerformances
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.TopicName == topic.Topic);

                if (performance != null)
                {
                    // Cập nhật
                    performance.TotalAttempts += 1;
                    performance.TotalQuestions += topic.TotalQuestions;
                    performance.CorrectAnswers += topic.Correct;
                    performance.AccuracyPercentage = performance.TotalQuestions > 0
                        ? Math.Round((decimal)performance.CorrectAnswers / performance.TotalQuestions * 100, 2)
                        : 0;
                    performance.LastAttemptDate = DateTime.Now;
                }
                else
                {
                    // Tạo mới
                    performance = new StudentTopicPerformance
                    {
                        UserId = userId,
                        TopicName = topic.Topic,
                        TotalAttempts = 1,
                        TotalQuestions = topic.TotalQuestions,
                        CorrectAnswers = topic.Correct,
                        AccuracyPercentage = Math.Round(topic.Accuracy, 2),
                        LastAttemptDate = DateTime.Now
                    };
                    _context.StudentTopicPerformances.Add(performance);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentTopicPerformance>> GetUserTopicPerformanceAsync(int userId)
        {
            return await _context.StudentTopicPerformances
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.AccuracyPercentage)
                .ToListAsync();
        }

        public async Task<StudentExamAnalysis?> GetExamAnalysisAsync(int studentExamId)
        {
            return await _context.StudentExamAnalyses
                .FirstOrDefaultAsync(a => a.StudentExamId == studentExamId);
        }

        public async Task<string> AnalyzeOverallProgressAsync(int userId)
        {
            // Lấy tất cả phân tích của học sinh
            var analyses = await _context.StudentExamAnalyses
                .Where(a => a.UserId == userId)
                .Include(a => a.Exam)
                .OrderBy(a => a.AnalyzedAt)
                .ToListAsync();

            if (!analyses.Any())
            {
                return string.Empty;
            }

            // Tạo payload ngắn gọn
            var summaryText = string.Join("\n", analyses.Select((a, index) => 
                $"{index + 1}. {a.Exam?.ExamName ?? "N/A"} - {a.AnalyzedAt:dd/MM/yyyy} - {a.CorrectAnswers}/{a.TotalQuestions} ({a.AccuracyPercentage}%)"));

            // Lấy topic performance
            var topicPerformances = await _context.StudentTopicPerformances
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.AccuracyPercentage)
                .ToListAsync();

            var topicText = string.Join("\n", topicPerformances.Select(tp => 
                $"- {tp.TopicName}: {tp.AccuracyPercentage}% ({tp.CorrectAnswers}/{tp.TotalQuestions} câu, {tp.TotalAttempts} lần)"));

            var prompt = $@"
Bạn là trợ lý AI đánh giá tiến độ học tập. Dựa trên lịch sử thi của học sinh, hãy đưa ra đánh giá NGẮN GỌN.

LỊCH SỬ BÀI THI:
{summaryText}

HIỆU SUẤT THEO CHỦ ĐỀ:
{topicText}

YÊU CẦU:
1. Đánh giá xu hướng (tiến bộ/thoái lui/ổn định) - 1 câu
2. Điểm mạnh chính - 1 câu
3. Điểm yếu cần cải thiện - 1 câu
4. 3 khuyến nghị ưu tiên (mỗi khuyến nghị 1 câu ngắn)

TRẢ VỀ CHỈ JSON THUẦN TÚY, KHÔNG CÓ MARKDOWN:
{{
  ""trend"": ""Mô tả xu hướng"",
  ""strengths"": ""Điểm mạnh"",
  ""weaknesses"": ""Điểm yếu"",
  ""recommendations"": [
    ""Khuyến nghị 1"",
    ""Khuyến nghị 2"",
    ""Khuyến nghị 3""
  ]
}}";

            var response = await GenerateContentAsync(prompt);
            return CleanJsonResponse(response);
        }

        private string CleanJsonResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return response;

            // Loại bỏ markdown code blocks
            var cleaned = response.Trim();
            
            // Nếu bắt đầu với ```json hoặc ```
            if (cleaned.StartsWith("```json"))
            {
                cleaned = cleaned.Substring(7); // Remove ```json
            }
            else if (cleaned.StartsWith("```"))
            {
                cleaned = cleaned.Substring(3); // Remove ```
            }

            // Nếu kết thúc với ```
            if (cleaned.EndsWith("```"))
            {
                cleaned = cleaned.Substring(0, cleaned.Length - 3);
            }

            return cleaned.Trim();
        }
    }
}
