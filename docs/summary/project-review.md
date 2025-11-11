# 🧭 Project Summary: TinhocOnline

## 🧩 Tổng quan

**TinhocOnline** là hệ thống **ngân hàng đề thi trực tuyến môn Tin học cấp 3**, cho phép:

- Giáo viên tạo và quản lý câu hỏi, sinh đề thi tự động.
- Học sinh làm bài thi trực tuyến, chấm điểm tự động.
- Quản trị viên giám sát người dùng, dữ liệu và thống kê kết quả.

### Công nghệ

- **Backend:** ASP.NET Core 8.0 MVC
- **ORM:** Entity Framework Core
- **Database:** SQL Server 2022
- **Frontend:** Razor Pages, Bootstrap 5
- **Auth:** ASP.NET Identity (Role-based)
- **Template:** NiceAdmin (Bootstrap theme)

---

## 🧱 Cấu trúc Dự án

```
TinhocOnline/
├── Controllers/ → Auth & Public controllers
├── Areas/ → Modules theo vai trò (Admin, Teacher, Student)
├── Models/ → Entity + ViewModels
├── Migrations/ → EF Core schema tracking
├── Views/ → Razor Pages cho frontend
├── wwwroot/ → Static assets (JS, CSS, Images)
├── docs/ → Yêu cầu nghiệp vụ & database schema
```

---

## 👑 Vai trò & Nghiệp vụ

### **ADMIN**

- Quản lý phân quyền.
- Quản lý chủ đề.
- Thống kê tổng quan: số đề, câu hỏi, lượt thi, người dùng.

### **GIÁO VIÊN**

- Quản lý câu hỏi (CRUD, soft delete, lọc theo độ khó/chủ đề).
  - Câu hỏi chia theo lớp THPT (lớp 10, 11, 12)
- Tạo đề thi tự động hoặc thủ công:
  - Có các loại đề thi khác nhau (giữ kì, cuối kì, 15p).
  - Tương ứng với từng loại đề sẽ có thời gian làm bài khác nhau.
  - Tự động sinh câu hỏi theo ma trận (40% dễ, 30% TB, 30% khó).
  - Cho phép trộn câu hỏi và đáp án.
- Xem kết quả học sinh, thống kê điểm và tỷ lệ đạt.

### **HỌC SINH**

- Học sinh sẽ có thể:
  - Chọn tạo đề (chọn lớp, topic) tự động với các điều kiện.
  - Thi đề giáo viên đưa.
- Xem danh sách đề thi công khai (published).
- Làm bài trực tuyến với countdown.
- Nhận kết quả ngay sau khi nộp bài (auto-grade).
- Xem chi tiết câu đúng/sai, lịch sử thi và tiến bộ cá nhân.

---

## ⚙️ Cấu trúc CSDL Chính (theo Entity Framework)

| Entity            | Mô tả                                         |
| ----------------- | --------------------------------------------- |
| **User**          | Người dùng hệ thống (Admin, Teacher, Student) |
| **Topic**         | 7 chủ đề Tin học (A–G)                        |
| **Question**      | Câu hỏi có độ khó và chủ đề                   |
| **Answer**        | 4 đáp án mỗi câu, 1 đáp án đúng               |
| **Exam**          | Đề thi (50 câu)                               |
| **ExamTopic**     | Liên kết đề ↔ chủ đề (Many-to-Many)           |
| **ExamQuestion**  | Liên kết đề ↔ câu hỏi (Many-to-Many)          |
| **StudentExam**   | Bài thi của học sinh                          |
| **StudentAnswer** | Câu trả lời học sinh chọn                     |

**Quy tắc điểm:**  
Mỗi câu đúng = 0.2 điểm → Tổng điểm = (Số câu đúng / 50) × 10.

---

## 📊 Ma Trận Đề thi

| Mức độ              | Tỉ lệ (%) | Số câu (trên 50) |
| ------------------- | --------- | ---------------- |
| Nhận biết (Easy)    | 40%       | 20               |
| Thông hiểu (Medium) | 30%       | 15               |
| Vận dụng (Hard)     | 30%       | 15               |

| Chủ đề | Tỉ lệ (%) | Ví dụ                           |
| ------ | --------- | ------------------------------- |
| A      | 15%       | Máy tính và xã hội tri thức     |
| B      | 20%       | Mạng máy tính và Internet       |
| C      | 10%       | Lưu trữ và tìm kiếm thông tin   |
| D      | 15%       | Đạo đức, pháp luật, văn hóa số  |
| E      | 15%       | Ứng dụng tin học                |
| F      | 10%       | Giải quyết vấn đề bằng máy tính |
| G      | 15%       | Hướng nghiệp với Tin học        |

---

## 📘 Luật Nghiệp vụ Chính

### Tạo Đề

- Tổng câu = 50
- Hệ thống chọn ngẫu nhiên câu hỏi phù hợp chủ đề & độ khó
- Tổng tỉ lệ chủ đề phải = 100%
- Nếu không đủ câu hỏi → cảnh báo “Không đủ dữ liệu để sinh đề”

### Tạo Câu hỏi

- Mỗi câu thuộc 1 chủ đề (A–G) và 1 độ khó
- Phải có 4 đáp án (A–D) và ít nhất 1 đáp án đúng
- Không được xóa câu hỏi đang nằm trong đề thi published

### Làm Bài

- Học sinh chỉ được thi 1 lần / đề chính thức
- Tự động lưu câu trả lời 30s/lần
- Hết giờ tự động nộp bài và chấm điểm

---

## 📂 Mapping Source → Feature

| Module  | Controller                                                                                                          | Chức năng chính                |
| ------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------ |
| Auth    | `AuthController.cs`                                                                                                 | Login / Register               |
| Admin   | `StudentManagerController.cs`, `TeacherManagerController.cs`, `TopicManagerController.cs`, `DashboardController.cs` | Quản lý user, chủ đề, thống kê |
| Teacher | `QuestionManagerController.cs`, `DashboardController.cs`                                                            | CRUD câu hỏi, tạo đề thi       |
| Student | `DashboardController.cs`                                                                                            | Làm bài thi, xem kết quả       |

---

## 📚 Tài liệu liên quan

- `/docs/business-requirements.md` → Toàn bộ yêu cầu nghiệp vụ
- `/docs/rule.md` → Quy tắc tạo đề & ma trận
- `/docs/database-schema.md` → Thiết kế CSDL
- `/summary/project-overview.md` → Bản tóm tắt cho agent đọc

---

## 🧠 Gợi ý cho Agent

Khi đọc project này, hãy nắm:

1. Ba vai trò chính (Admin, Teacher, Student) và chức năng tương ứng.
2. Logic sinh đề dựa trên **ma trận chủ đề + độ khó**.
3. Entity Framework là trung tâm truy cập dữ liệu (`DataContext.cs`).
4. Dự án theo mô hình **MVC + Areas**, tách rõ module.
5. Hệ thống dùng **SQL Server**, **EF Core Migration**, **Bootstrap** UI.
