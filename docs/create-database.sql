-- =============================================
-- Database: TinhocOnlineDB
-- Hệ thống Ngân hàng Đề thi Trực tuyến
-- Created: 2025-10-27
-- =============================================

-- Tạo database
CREATE DATABASE TinhocOnlineDB;
GO

USE TinhocOnlineDB;
GO

-- =============================================
-- 1. Bảng Users (Người dùng)
-- =============================================
CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    full_name NVARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL,
    role VARCHAR(20) NOT NULL CHECK (role IN ('admin', 'teacher', 'student')),
    status VARCHAR(20) DEFAULT 'active' CHECK (status IN ('active', 'inactive'))
);
GO

-- =============================================
-- 2. Bảng Subjects (Môn học)
-- =============================================
CREATE TABLE Subjects (
    subject_id INT IDENTITY(1,1) PRIMARY KEY,
    subject_name NVARCHAR(100) NOT NULL,
    subject_code VARCHAR(20) NOT NULL,
    status VARCHAR(20) DEFAULT 'active' CHECK (status IN ('active', 'inactive'))
);
GO

-- =============================================
-- 3. Bảng Questions (Câu hỏi)
-- =============================================
CREATE TABLE Questions (
    question_id INT IDENTITY(1,1) PRIMARY KEY,
    subject_id INT NOT NULL,
    question_text NVARCHAR(MAX) NOT NULL,
    difficulty_level VARCHAR(20) NOT NULL CHECK (difficulty_level IN ('easy', 'medium', 'hard')),
    created_by INT NOT NULL,
    status VARCHAR(20) DEFAULT 'active' CHECK (status IN ('active', 'inactive')),
    
    -- Foreign Keys
    CONSTRAINT FK_Questions_Subjects FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id),
    CONSTRAINT FK_Questions_Users FOREIGN KEY (created_by) REFERENCES Users(user_id)
);
GO

-- =============================================
-- 4. Bảng Answers (Đáp án)
-- =============================================
CREATE TABLE Answers (
    answer_id INT IDENTITY(1,1) PRIMARY KEY,
    question_id INT NOT NULL,
    answer_text NVARCHAR(MAX) NOT NULL,
    is_correct BIT NOT NULL,
    answer_order CHAR(1) NOT NULL CHECK (answer_order IN ('A', 'B', 'C', 'D')),
    
    -- Foreign Keys
    CONSTRAINT FK_Answers_Questions FOREIGN KEY (question_id) REFERENCES Questions(question_id) ON DELETE CASCADE
);
GO

-- =============================================
-- 5. Bảng Exams (Đề thi)
-- =============================================
CREATE TABLE Exams (
    exam_id INT IDENTITY(1,1) PRIMARY KEY,
    subject_id INT NOT NULL,
    exam_name NVARCHAR(200) NOT NULL,
    duration INT NOT NULL,
    total_questions INT DEFAULT 50,
    easy_percentage DECIMAL(5,2) DEFAULT 60,
    medium_percentage DECIMAL(5,2) DEFAULT 30,
    hard_percentage DECIMAL(5,2) DEFAULT 10,
    created_by INT NOT NULL,
    status VARCHAR(20) DEFAULT 'draft' CHECK (status IN ('draft', 'published')),
    
    -- Foreign Keys
    CONSTRAINT FK_Exams_Subjects FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id),
    CONSTRAINT FK_Exams_Users FOREIGN KEY (created_by) REFERENCES Users(user_id)
);
GO

-- =============================================
-- 6. Bảng Exam_Questions (Câu hỏi trong đề thi)
-- =============================================
CREATE TABLE Exam_Questions (
    exam_question_id INT IDENTITY(1,1) PRIMARY KEY,
    exam_id INT NOT NULL,
    question_id INT NOT NULL,
    question_order INT NOT NULL,
    
    -- Foreign Keys
    CONSTRAINT FK_ExamQuestions_Exams FOREIGN KEY (exam_id) REFERENCES Exams(exam_id) ON DELETE CASCADE,
    CONSTRAINT FK_ExamQuestions_Questions FOREIGN KEY (question_id) REFERENCES Questions(question_id)
);
GO

-- =============================================
-- 7. Bảng Student_Exams (Bài thi của học sinh)
-- =============================================
CREATE TABLE Student_Exams (
    student_exam_id INT IDENTITY(1,1) PRIMARY KEY,
    exam_id INT NOT NULL,
    student_id INT NOT NULL,
    start_time DATETIME NOT NULL,
    end_time DATETIME NULL,
    score DECIMAL(5,2) NULL,
    status VARCHAR(20) DEFAULT 'in_progress' CHECK (status IN ('in_progress', 'completed')),
    
    -- Foreign Keys
    CONSTRAINT FK_StudentExams_Exams FOREIGN KEY (exam_id) REFERENCES Exams(exam_id),
    CONSTRAINT FK_StudentExams_Users FOREIGN KEY (student_id) REFERENCES Users(user_id)
);
GO

-- =============================================
-- 8. Bảng Student_Answers (Câu trả lời của học sinh)
-- =============================================
CREATE TABLE Student_Answers (
    student_answer_id INT IDENTITY(1,1) PRIMARY KEY,
    student_exam_id INT NOT NULL,
    question_id INT NOT NULL,
    answer_id INT NULL,
    is_correct BIT NULL,
    
    -- Foreign Keys
    CONSTRAINT FK_StudentAnswers_StudentExams FOREIGN KEY (student_exam_id) REFERENCES Student_Exams(student_exam_id) ON DELETE CASCADE,
    CONSTRAINT FK_StudentAnswers_Questions FOREIGN KEY (question_id) REFERENCES Questions(question_id),
    CONSTRAINT FK_StudentAnswers_Answers FOREIGN KEY (answer_id) REFERENCES Answers(answer_id)
);
GO

-- =============================================
-- INSERT DỮ LIỆU MẪU
-- =============================================

-- 1. Thêm Admin mặc định
INSERT INTO Users (username, password, full_name, email, role, status)
VALUES 
    ('admin', 'Admin@123', N'Quản trị viên', 'admin@tinhoc.vn', 'admin', 'active'),
    ('gv_nguyen', 'Teacher@123', N'Nguyễn Văn A', 'nguyenvana@tinhoc.vn', 'teacher', 'active'),
    ('hs_tran', 'Student@123', N'Trần Thị B', 'tranthib@tinhoc.vn', 'student', 'active');
GO

-- 2. Thêm Môn học
INSERT INTO Subjects (subject_name, subject_code, status)
VALUES 
    (N'Tin học căn bản', 'THCB', 'active'),
    (N'Lập trình C#', 'LTCS', 'active'),
    (N'Cơ sở dữ liệu', 'CSDL', 'active'),
    (N'Lập trình Web', 'LTW', 'active'),
    (N'Mạng máy tính', 'MMT', 'active');
GO

-- 3. Thêm Câu hỏi mẫu (Tin học căn bản)
-- Câu hỏi dễ
INSERT INTO Questions (subject_id, question_text, difficulty_level, created_by, status)
VALUES 
    (1, N'CPU là viết tắt của từ gì?', 'easy', 2, 'active'),
    (1, N'RAM là bộ nhớ gì?', 'easy', 2, 'active'),
    (1, N'HDD là thiết bị gì?', 'easy', 2, 'active');
GO

-- Câu hỏi trung bình
INSERT INTO Questions (subject_id, question_text, difficulty_level, created_by, status)
VALUES 
    (1, N'Hệ điều hành nào là mã nguồn mở?', 'medium', 2, 'active'),
    (1, N'Giao thức nào được sử dụng để truyền tải trang web?', 'medium', 2, 'active');
GO

-- Câu hỏi khó
INSERT INTO Questions (subject_id, question_text, difficulty_level, created_by, status)
VALUES 
    (1, N'Thuật toán sắp xếp nào có độ phức tạp tốt nhất là O(n log n)?', 'hard', 2, 'active');
GO

-- 4. Thêm Đáp án
-- Đáp án cho câu 1: CPU là viết tắt của từ gì?
INSERT INTO Answers (question_id, answer_text, is_correct, answer_order)
VALUES 
    (1, N'Central Processing Unit', 1, 'A'),
    (1, N'Computer Personal Unit', 0, 'B'),
    (1, N'Central Personal Unit', 0, 'C'),
    (1, N'Computer Processing Unit', 0, 'D');
GO

-- Đáp án cho câu 2: RAM là bộ nhớ gì?
INSERT INTO Answers (question_id, answer_text, is_correct, answer_order)
VALUES 
    (2, N'Bộ nhớ trong', 1, 'A'),
    (2, N'Bộ nhớ ngoài', 0, 'B'),
    (2, N'Bộ nhớ cache', 0, 'C'),
    (2, N'Bộ nhớ ROM', 0, 'D');
GO

-- Đáp án cho câu 3: HDD là thiết bị gì?
INSERT INTO Answers (question_id, answer_text, is_correct, answer_order)
VALUES 
    (3, N'Ổ cứng', 1, 'A'),
    (3, N'Bộ nhớ RAM', 0, 'B'),
    (3, N'Màn hình', 0, 'C'),
    (3, N'Bàn phím', 0, 'D');
GO

-- Đáp án cho câu 4: Hệ điều hành nào là mã nguồn mở?
INSERT INTO Answers (question_id, answer_text, is_correct, answer_order)
VALUES 
    (4, N'Linux', 1, 'A'),
    (4, N'Windows', 0, 'B'),
    (4, N'MacOS', 0, 'C'),
    (4, N'iOS', 0, 'D');
GO

-- Đáp án cho câu 5: Giao thức nào được sử dụng để truyền tải trang web?
INSERT INTO Answers (question_id, answer_text, is_correct, answer_order)
VALUES 
    (5, N'HTTP/HTTPS', 1, 'A'),
    (5, N'FTP', 0, 'B'),
    (5, N'SMTP', 0, 'C'),
    (5, N'SSH', 0, 'D');
GO

-- Đáp án cho câu 6: Thuật toán sắp xếp nào có độ phức tạp tốt nhất là O(n log n)?
INSERT INTO Answers (question_id, answer_text, is_correct, answer_order)
VALUES 
    (6, N'Quick Sort', 1, 'A'),
    (6, N'Bubble Sort', 0, 'B'),
    (6, N'Selection Sort', 0, 'C'),
    (6, N'Insertion Sort', 0, 'D');
GO

-- =============================================
-- QUERY KIỂM TRA
-- =============================================

-- Xem tất cả users
SELECT * FROM Users;

-- Xem tất cả môn học
SELECT * FROM Subjects;

-- Xem câu hỏi và đáp án
SELECT 
    q.question_id,
    s.subject_name,
    q.question_text,
    q.difficulty_level,
    a.answer_order,
    a.answer_text,
    a.is_correct
FROM Questions q
JOIN Subjects s ON q.subject_id = s.subject_id
JOIN Answers a ON q.question_id = a.question_id
ORDER BY q.question_id, a.answer_order;

-- =============================================
-- HẾT
-- =============================================
