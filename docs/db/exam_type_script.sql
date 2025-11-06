-- =============================================
-- Insert ExamTypes (Loại đề thi)
-- =============================================

INSERT INTO ExamTypes (TypeName, TypeCode, DefaultDuration, DefaultTotalQuestions, Description, Status)
VALUES 
(N'Kiểm tra 15 phút', 'quiz_15min', 15, 15, N'Kiểm tra ngắn, 15 câu trắc nghiệm', 'active'),
(N'Kiểm tra 1 tiết (45 phút)', 'quiz_45min', 45, 30, N'Kiểm tra 1 tiết, 30 câu trắc nghiệm', 'active'),
(N'Kiểm tra giữa kỳ', 'midterm', 45, 50, N'Kiểm tra giữa học kỳ, 50 câu trắc nghiệm', 'active'),
(N'Kiểm tra cuối kỳ', 'final', 90, 50, N'Kiểm tra cuối học kỳ, 50 câu trắc nghiệm', 'active'),
(N'Ôn tập tự do', 'practice', 60, 50, N'Đề thi ôn tập không giới hạn thời gian', 'active');
