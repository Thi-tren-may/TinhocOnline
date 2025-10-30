# Database Schema - Hệ thống Ngân hàng Đề thi

## Tổng quan
Database được thiết kế cho hệ thống thi trực tuyến với 3 vai trò chính: Admin, Teacher, Student.

**Lưu ý:** Mỗi đề thi luôn có 50 câu hỏi, mỗi câu chiếm 0.2 điểm (tổng 10 điểm).

---

## Danh sách Bảng

### 1. Users (Người dùng)
Lưu trữ thông tin tất cả người dùng trong hệ thống.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `user_id` | INT | PK, IDENTITY(1,1) | ID người dùng |
| `username` | VARCHAR(50) | UNIQUE, NOT NULL | Tên đăng nhập |
| `password` | VARCHAR(255) | NOT NULL | Mật khẩu (đã mã hóa) |
| `full_name` | NVARCHAR(100) | NOT NULL | Họ và tên |
| `email` | VARCHAR(100) | NOT NULL | Email |
| `role` | VARCHAR(20) | NOT NULL | Vai trò: 'admin', 'teacher', 'student' |
| `status` | VARCHAR(20) | DEFAULT 'active' | Trạng thái: 'active', 'inactive' |

---

### 2. Subjects (Môn học)
Quản lý danh mục các môn học.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `subject_id` | INT | PK, IDENTITY(1,1) | ID môn học |
| `subject_name` | NVARCHAR(100) | NOT NULL | Tên môn học |
| `subject_code` | VARCHAR(20) | NOT NULL | Mã môn học |
| `status` | VARCHAR(20) | DEFAULT 'active' | Trạng thái: 'active', 'inactive' |

---

### 3. Topics (Chủ đề)
Quản lý các chủ đề theo chương trình Tin học phổ thông.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `topic_id` | INT | PK, IDENTITY(1,1) | ID chủ đề |
| `topic_code` | VARCHAR(10) | UNIQUE, NOT NULL | Mã chủ đề: A, B, C, D, E, F, G |
| `topic_name` | NVARCHAR(200) | NOT NULL | Tên chủ đề |
| `description` | NVARCHAR(500) | NULL | Mô tả chủ đề |
| `status` | VARCHAR(20) | DEFAULT 'active' | Trạng thái: 'active', 'inactive' |

**Danh sách Topics cố định:**
- **A**: Máy tính và xã hội tri thức
- **B**: Mạng máy tính và Internet
- **C**: Tổ chức lưu trữ, tìm kiếm và trao đổi thông tin
- **D**: Đạo đức, pháp luật và văn hóa trong môi trường số
- **E**: Ứng dụng tin học
- **F**: Giải quyết vấn đề với sự trợ giúp của máy tính
- **G**: Hướng nghiệp với tin học

---

### 4. Questions (Câu hỏi)
Ngân hàng câu hỏi của hệ thống.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `question_id` | INT | PK, IDENTITY(1,1) | ID câu hỏi |
| `subject_id` | INT | FK, NOT NULL | ID môn học |
| `topic_id` | INT | FK, NOT NULL | ID chủ đề (A-G) |
| `question_text` | NVARCHAR(MAX) | NOT NULL | Nội dung câu hỏi |
| `difficulty_level` | VARCHAR(20) | NOT NULL | Độ khó: 'easy', 'medium', 'hard' |
| `created_by` | INT | FK, NOT NULL | ID giáo viên tạo |
| `status` | VARCHAR(20) | DEFAULT 'active' | Trạng thái: 'active', 'inactive' |

**Foreign Keys:**
- `subject_id` REFERENCES `Subjects(subject_id)`
- `topic_id` REFERENCES `Topics(topic_id)`
- `created_by` REFERENCES `Users(user_id)`

---

### 5. Answers (Đáp án)
Lưu trữ các đáp án cho mỗi câu hỏi.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `answer_id` | INT | PK, IDENTITY(1,1) | ID đáp án |
| `question_id` | INT | FK, NOT NULL | ID câu hỏi |
| `answer_text` | NVARCHAR(MAX) | NOT NULL | Nội dung đáp án |
| `is_correct` | BIT | NOT NULL | Đúng (1) / Sai (0) |
| `answer_order` | CHAR(1) | NOT NULL | Thứ tự: 'A', 'B', 'C', 'D' |

**Foreign Keys:**
- `question_id` REFERENCES `Questions(question_id)` ON DELETE CASCADE

**Lưu ý:** Mỗi câu hỏi phải có đúng 4 đáp án (A, B, C, D) và ít nhất 1 đáp án đúng.

---

### 6. Exams (Đề thi)
Thông tin các đề thi.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `exam_id` | INT | PK, IDENTITY(1,1) | ID đề thi |
| `subject_id` | INT | FK, NOT NULL | ID môn học |
| `exam_name` | NVARCHAR(200) | NOT NULL | Tên đề thi |
| `duration` | INT | NOT NULL | Thời gian làm bài (phút) |
| `total_questions` | INT | DEFAULT 50 | Tổng số câu (mặc định 50) |
| `easy_percentage` | DECIMAL(5,2) | DEFAULT 60 | % câu dễ (mặc định 60) |
| `medium_percentage` | DECIMAL(5,2) | DEFAULT 30 | % câu vừa (mặc định 30) |
| `hard_percentage` | DECIMAL(5,2) | DEFAULT 10 | % câu khó (mặc định 10) |
| `created_by` | INT | FK, NOT NULL | ID giáo viên tạo |
| `status` | VARCHAR(20) | DEFAULT 'draft' | Trạng thái: 'draft', 'published' |

**Foreign Keys:**
- `subject_id` REFERENCES `Subjects(subject_id)`
- `created_by` REFERENCES `Users(user_id)`

---

### 7. Exam_Topics (Chủ đề trong đề thi)
Quan hệ nhiều-nhiều giữa Exams và Topics. Một đề thi có thể bao gồm nhiều chủ đề.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `exam_topic_id` | INT | PK, IDENTITY(1,1) | ID |
| `exam_id` | INT | FK, NOT NULL | ID đề thi |
| `topic_id` | INT | FK, NOT NULL | ID chủ đề |
| `question_count` | INT | NOT NULL | Số câu hỏi từ chủ đề này |

**Foreign Keys:**
- `exam_id` REFERENCES `Exams(exam_id)` ON DELETE CASCADE
- `topic_id` REFERENCES `Topics(topic_id)`

**Ví dụ:** 
- Đề thi "Giữa kỳ Tin học" có 50 câu:
  - 10 câu từ chủ đề A (Máy tính và xã hội tri thức)
  - 15 câu từ chủ đề B (Mạng máy tính)
  - 15 câu từ chủ đề C (Lưu trữ thông tin)
  - 10 câu từ chủ đề E (Ứng dụng tin học)

---

### 8. Exam_Questions (Câu hỏi trong đề thi)
Quan hệ nhiều-nhiều giữa Exams và Questions.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `exam_question_id` | INT | PK, IDENTITY(1,1) | ID |
| `exam_id` | INT | FK, NOT NULL | ID đề thi |
| `question_id` | INT | FK, NOT NULL | ID câu hỏi |
| `question_order` | INT | NOT NULL | Thứ tự câu trong đề (1-50) |

**Foreign Keys:**
- `exam_id` REFERENCES `Exams(exam_id)` ON DELETE CASCADE
- `question_id` REFERENCES `Questions(question_id)`

---

### 9. Student_Exams (Bài thi của học sinh)
Lưu trữ thông tin bài làm của học sinh.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `student_exam_id` | INT | PK, IDENTITY(1,1) | ID bài thi |
| `exam_id` | INT | FK, NOT NULL | ID đề thi |
| `student_id` | INT | FK, NOT NULL | ID học sinh |
| `start_time` | DATETIME | NOT NULL | Thời gian bắt đầu |
| `end_time` | DATETIME | NULL | Thời gian kết thúc |
| `score` | DECIMAL(5,2) | NULL | Điểm số |
| `status` | VARCHAR(20) | DEFAULT 'in_progress' | Trạng thái: 'in_progress', 'completed' |

**Foreign Keys:**
- `exam_id` REFERENCES `Exams(exam_id)`
- `student_id` REFERENCES `Users(user_id)`

---

### 10. Student_Answers (Câu trả lời của học sinh)
Lưu trữ từng câu trả lời của học sinh.

| Tên cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---------|-------------|-----------|-------|
| `student_answer_id` | INT | PK, IDENTITY(1,1) | ID |
| `student_exam_id` | INT | FK, NOT NULL | ID bài thi |
| `question_id` | INT | FK, NOT NULL | ID câu hỏi |
| `answer_id` | INT | FK, NULL | ID đáp án đã chọn (NULL = chưa trả lời) |
| `is_correct` | BIT | NULL | Đúng/Sai (NULL = chưa chấm) |

**Foreign Keys:**
- `student_exam_id` REFERENCES `Student_Exams(student_exam_id)` ON DELETE CASCADE
- `question_id` REFERENCES `Questions(question_id)`
- `answer_id` REFERENCES `Answers(answer_id)`

---

## Quy tắc Tính điểm

- Mỗi đề thi có **50 câu hỏi**
- Mỗi câu đúng = **0.2 điểm**
- Tổng điểm tối đa = **10 điểm**
- Công thức: `Điểm = (Số câu đúng / 50) × 10`
