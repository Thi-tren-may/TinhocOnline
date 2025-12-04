using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TinhocOnline.Services.DTOs;

namespace TinhocOnline.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _defaultModel;

        public GeminiService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}
