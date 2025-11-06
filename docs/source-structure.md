# 📁 CẤU TRÚC SOURCE CODE - TinhocOnline

## 📊 TIẾN ĐỘ HIỆN TẠI

| Module | Tình trạng | Tiến độ |
|--------|------------|---------|
| Authentication (Login/Register) | ✅ Hoàn thành | 100% |
| Admin - Quản lý Topics | ✅ Hoàn thành | 100% |
| Admin - Quản lý Users | 🔧 Đang triển khai | 80% |
| Teacher - Quản lý Questions | ✅ Hoàn thành | 100% |
| Teacher - Tạo đề thi | ❌ Chưa triển khai | 0% |
| Teacher - Xem kết quả | ❌ Chưa triển khai | 0% |
| Student - Làm bài thi | ❌ Chưa triển khai | 0% |
| Student - Xem kết quả | ❌ Chưa triển khai | 0% |
| Dashboard & Thống kê | 🔧 Đang triển khai | 20% |

**Tổng tiến độ:** ~40%

---

## 📂 CẤU TRÚC THƯ MỤC

```
TinhocOnline/
│
├── 📄 Program.cs                    # Entry point, cấu hình Services & Middleware
├── 📄 TinhocOnline.csproj           # Project file, NuGet packages
├── 📄 TinhocOnline.sln              # Solution file
├── 📄 appsettings.json              # ConnectionString, Logging config
├── 📄 appsettings.Development.json  # Dev environment settings
├── 📄 README.md                     # Project documentation
│
├── 📁 Controllers/                  # Controllers công khai
│   ├── AuthController.cs            # Login, Register
│   └── HomeController.cs            # Landing page, Privacy
│
├── 📁 Areas/                        # Module theo vai trò người dùng
│   ├── 📁 Admin/                    # Module Quản trị viên
│   │   ├── Controllers/
│   │   │   ├── DashboardController.cs
│   │   │   ├── StudentManagerController.cs
│   │   │   ├── TeacherManagerController.cs
│   │   │   └── TopicManagerController.cs
│   │   └── Views/
│   │       ├── Dashboard/
│   │       ├── StudentManager/
│   │       ├── TeacherManager/
│   │       ├── TopicManager/
│   │       └── Shared/
│   │
│   ├── 📁 Teacher/                  # Module Giáo viên
│   │   ├── Controllers/
│   │   │   ├── DashboardController.cs
│   │   │   └── QuestionManagerController.cs
│   │   └── Views/
│   │       ├── Dashboard/
│   │       ├── QuestionManager/
│   │       └── Shared/
│   │
│   └── 📁 Student/                  # Module Học sinh
│       ├── Controllers/
│       │   └── DashboardController.cs
│       └── Views/
│           ├── Dashboard/
│           └── Shared/
│
├── 📁 Models/                       # Domain Models & DbContext
│   ├── User.cs                      # Người dùng (Admin/Teacher/Student)
│   ├── Topic.cs                     # Chủ đề (7 topics cố định)
│   ├── Question.cs                  # Câu hỏi
│   ├── Answer.cs                    # Đáp án (4 đáp án/câu)
│   ├── Exam.cs                      # Đề thi
│   ├── ExamTopic.cs                 # Exam ↔ Topic (Many-to-Many)
│   ├── ExamQuestion.cs              # Exam ↔ Question (Many-to-Many)
│   ├── StudentExam.cs               # Bài thi của học sinh
│   ├── StudentAnswer.cs             # Câu trả lời của học sinh
│   ├── DataContext.cs               # EF Core DbContext
│   └── ViewModels/
│       ├── AnswerDto.cs
│       └── QuestionWithAnswersViewModel.cs
│
├── 📁 Migrations/                   # EF Core Migrations
│   ├── 20251027163606_InitialCreate.cs
│   ├── 20251030124026_AddTopicsAndExamTopics.cs
│   ├── 20251030125238_UpdateTopicRemoveSubjectId.cs
│   ├── 20251031091809_RemoveSubjectTable.cs
│   ├── 20251031093123_RemoveSubjectAndTopicCode.cs
│   └── DataContextModelSnapshot.cs
│
├── 📁 Views/                        # Razor Views (Public)
│   ├── Auth/
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── _Layout.cshtml.css
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
│
├── 📁 wwwroot/                      # Static Files
│   ├── assets/                      # Admin template (NiceAdmin)
│   │   ├── css/
│   │   ├── js/
│   │   ├── img/
│   │   ├── scss/
│   │   └── vendor/                  # Bootstrap 5, jQuery, DataTables, etc.
│   ├── public-assets/               # Public template
│   │   ├── css/
│   │   ├── js/
│   │   ├── img/
│   │   └── lib/
│   └── lib/
│       └── jquery/
│
├── 📁 docs/                         # Documentation
│   ├── business-requirements.md     # Yêu cầu nghiệp vụ chi tiết
│   ├── database-schema.md           # Thiết kế database
│   ├── rule.md                      # Quy tắc tạo đề thi
│   └── db/
│       ├── question.sql             # Sample questions
│       └── topic_script.sql         # 7 topics Tin học
│
├── 📁 Properties/
│   └── launchSettings.json          # Launch configuration
│
├── 📁 bin/                          # Binary output
└── 📁 obj/                          # Build artifacts
```

---

## 🎭 CHI TIẾT THEO MODULE

### 1️⃣ ROOT LEVEL

| File | Chức năng |
|------|-----------|
| `Program.cs` | • Cấu hình DbContext với SQL Server<br>• Cấu hình Session (timeout 30 phút)<br>• Đăng ký MVC Services<br>• Route mapping cho Areas |
| `appsettings.json` | • Connection string: `PCHAILAM\SQLEXPRESS`<br>• Database: `TinhocOnline`<br>• Logging configuration |

---

### 2️⃣ CONTROLLERS (Public)

| Controller | Endpoint | Chức năng |
|------------|----------|-----------|
| `AuthController` | `/Auth/Login`<br>`/Auth/Register` | • Xác thực username/password<br>• Phân quyền theo role<br>• Lưu Session<br>• Redirect về Dashboard tương ứng |
| `HomeController` | `/Home/Index`<br>`/Home/Privacy` | • Landing page<br>• Privacy policy |

---

### 3️⃣ AREAS - ADMIN

**Prefix URL:** `/Admin/...`

| Controller | Chức năng hiện tại | Chức năng cần triển khai |
|------------|-------------------|-------------------------|
| `DashboardController` | • View cơ bản | • Thống kê tổng quan<br>• Biểu đồ người dùng<br>• Thống kê đề thi |
| `TopicManagerController` | ✅ **CRUD Topics đầy đủ**<br>• Index (danh sách)<br>• Create<br>• Edit<br>• Delete<br>• Details | • Export danh sách Topics |
| `StudentManagerController` | 🔧 Đang triển khai | • CRUD Students<br>• Import từ Excel<br>• Reset mật khẩu<br>• Kích hoạt/vô hiệu hóa |
| `TeacherManagerController` | 🔧 Đang triển khai | • CRUD Teachers<br>• Thống kê câu hỏi của GV<br>• Thống kê đề thi của GV |

**Views:**
- Bootstrap 5 + DataTables
- Template: NiceAdmin

---

### 4️⃣ AREAS - TEACHER

**Prefix URL:** `/Teacher/...`

| Controller | Chức năng hiện tại | Chức năng cần triển khai |
|------------|-------------------|-------------------------|
| `DashboardController` | • View cơ bản | • Thống kê câu hỏi đã tạo<br>• Thống kê đề thi<br>• Kết quả học sinh |
| `QuestionManagerController` | ✅ **CRUD Questions đầy đủ**<br>• Index với phân trang (20/page)<br>• Create (câu hỏi + 4 đáp án)<br>• Edit<br>• Delete<br>• Details<br>• Preview | • Filter theo Topic<br>• Filter theo Difficulty<br>• Export câu hỏi<br>• Import từ Excel |
| `ExamManagerController` | ❌ Chưa có | • Tạo đề tự động<br>• Tạo đề thủ công<br>• Cấu hình ma trận đề<br>• Xuất bản đề thi<br>• Xem danh sách đề |

**Đặc điểm QuestionManager:**
- Include Topic, Answers, Creator
- Pagination: Server-side
- Editor: WYSIWYG (textarea)
- 4 đáp án A, B, C, D
- Chọn đáp án đúng bằng radio button

---

### 5️⃣ AREAS - STUDENT

**Prefix URL:** `/Student/...`

| Controller | Chức năng cần triển khai |
|------------|-------------------------|
| `DashboardController` | • Thống kê số bài thi đã làm<br>• Điểm trung bình<br>• Xếp hạng |
| `ExamController` | • Danh sách đề thi available<br>• Làm bài thi (timer)<br>• Submit bài thi<br>• Xem kết quả chi tiết<br>• Lịch sử thi |

---

### 6️⃣ MODELS (Domain Layer)

#### Core Entities

| Entity | Mô tả | Quan hệ |
|--------|-------|---------|
| `User` | Người dùng (Admin/Teacher/Student) | • 1 User → N Questions (Creator)<br>• 1 User → N Exams (Creator)<br>• 1 User → N StudentExams |
| `Topic` | 7 chủ đề Tin học cố định | • 1 Topic → N Questions<br>• N Topics ↔ N Exams (qua ExamTopics) |
| `Question` | Câu hỏi (có TopicId, DifficultyLevel) | • 1 Question → 4 Answers<br>• N Questions ↔ N Exams (qua ExamQuestions) |
| `Answer` | Đáp án (A/B/C/D) | • N Answers → 1 Question<br>• 1 Answer có `is_correct` flag |
| `Exam` | Đề thi (50 câu, duration, difficulty %) | • 1 Exam → N ExamQuestions<br>• 1 Exam → N ExamTopics<br>• 1 Exam → N StudentExams |
| `ExamTopic` | Junction table (Exam ↔ Topic) | Lưu tỷ lệ % từng Topic trong đề |
| `ExamQuestion` | Junction table (Exam ↔ Question) | Lưu thứ tự câu hỏi trong đề |
| `StudentExam` | Bài thi của học sinh | Lưu start_time, end_time, score |
| `StudentAnswer` | Câu trả lời của học sinh | Lưu answer đã chọn, đúng/sai |

#### ViewModels

| ViewModel | Mục đích |
|-----------|----------|
| `AnswerDto` | Transfer Answer data |
| `QuestionWithAnswersViewModel` | Hiển thị Question + 4 Answers |

#### DataContext Configuration

```csharp
- DbSet<User> Users
- DbSet<Topic> Topics
- DbSet<Question> Questions
- DbSet<Answer> Answers
- DbSet<Exam> Exams
- DbSet<ExamQuestion> ExamQuestions
- DbSet<ExamTopic> ExamTopics
- DbSet<StudentExam> StudentExams
- DbSet<StudentAnswer> StudentAnswers
```

**Constraints:**
- `Username`: Unique index
- `Email`: Unique index
- `Answer.QuestionId`: ON DELETE CASCADE
- Tất cả FK có ràng buộc referential integrity

---

### 7️⃣ MIGRATIONS

| Migration | Ngày | Mô tả |
|-----------|------|-------|
| `InitialCreate` | 27/10/2025 | Tạo 9 bảng ban đầu |
| `AddTopicsAndExamTopics` | 30/10/2025 | Thêm Topics + ExamTopics |
| `UpdateTopicRemoveSubjectId` | 30/10/2025 | Xóa SubjectId khỏi Topic |
| `RemoveSubjectTable` | 31/10/2025 | Xóa bảng Subject (chỉ 1 môn Tin học) |
| `RemoveSubjectAndTopicCode` | 31/10/2025 | Xóa TopicCode (dùng TopicId) |

**Current Schema:** 9 bảng (Users, Topics, Questions, Answers, Exams, ExamTopics, ExamQuestions, StudentExams, StudentAnswers)

---

### 8️⃣ VIEWS (UI Layer)

#### Public Views (`/Views`)
- `Auth/Login.cshtml`: Form đăng nhập (username, password, role dropdown)
- `Auth/Register.cshtml`: Form đăng ký
- `Home/Index.cshtml`: Landing page
- `Shared/_Layout.cshtml`: Master layout (Bootstrap 5)

#### Admin Views (`/Areas/Admin/Views`)
- `TopicManager/Index.cshtml`: DataTable với CRUD buttons
- `TopicManager/Create.cshtml`: Form thêm chủ đề
- `TopicManager/Edit.cshtml`: Form sửa chủ đề
- `TopicManager/Details.cshtml`: Chi tiết chủ đề

#### Teacher Views (`/Areas/Teacher/Views`)
- `QuestionManager/Index.cshtml`: DataTable với pagination, filter
- `QuestionManager/Create.cshtml`: Form câu hỏi + 4 đáp án
- `QuestionManager/Edit.cshtml`: Form sửa
- `QuestionManager/Details.cshtml`: Preview câu hỏi

---

### 9️⃣ WWWROOT (Static Assets)

#### `/wwwroot/assets/` - Admin Template (NiceAdmin)
```
vendor/
├── bootstrap/             # Bootstrap 5.3.0
├── bootstrap-icons/       # Icons
├── apexcharts/           # Charts library
├── datatables/           # DataTables plugin
├── quill/                # Rich text editor
├── simple-datatables/    # Lightweight DataTables
└── tinymce/              # WYSIWYG editor
```

#### `/wwwroot/public-assets/` - Public Template
- CSS, JS, Images cho landing page
- Responsive design

---

## 🔧 DEPENDENCIES

### NuGet Packages

| Package | Version | Mục đích |
|---------|---------|----------|
| `Microsoft.EntityFrameworkCore.SqlServer` | 9.0.0 | SQL Server provider |
| `Microsoft.EntityFrameworkCore.Tools` | 9.0.0 | Migration CLI tools |
| `Microsoft.EntityFrameworkCore.Design` | 9.0.0 | Design-time support |
| `Microsoft.VisualStudio.Web.CodeGeneration.Design` | 8.0.7 | Scaffolding MVC |

### Client-side Libraries

| Library | Version | Mục đích |
|---------|---------|----------|
| Bootstrap | 5.3.0 | UI Framework |
| jQuery | 3.x | DOM manipulation |
| DataTables | Latest | Table with pagination, sort, filter |
| ApexCharts | Latest | Charts & graphs |
| TinyMCE | Latest | Rich text editor (for questions) |

---

## 🗄️ DATABASE

**Server:** `PCHAILAM\SQLEXPRESS`  
**Database:** `TinhocOnline`  
**Authentication:** Integrated Security (Windows Auth)  
**Trust Server Certificate:** True

### Bảng chính:
1. **Users** - Lưu Admin, Teacher, Student
2. **Topics** - 7 chủ đề Tin học cố định
3. **Questions** - Ngân hàng câu hỏi
4. **Answers** - 4 đáp án/câu
5. **Exams** - Đề thi (50 câu)
6. **ExamTopics** - Tỷ lệ Topics trong đề
7. **ExamQuestions** - Câu hỏi trong đề
8. **StudentExams** - Bài thi của HS
9. **StudentAnswers** - Câu trả lời của HS

---

## 🚀 CHẠY DỰ ÁN

### Yêu cầu:
- .NET 8.0 SDK
- SQL Server 2022 hoặc SQL Server Express
- Visual Studio 2022 hoặc VS Code

### Các bước:

1. **Clone repository:**
   ```bash
   git clone <repo-url>
   cd TinhocOnline
   ```

2. **Restore packages:**
   ```bash
   dotnet restore
   ```

3. **Cập nhật connection string:**
   Sửa `appsettings.json` với SQL Server của bạn

4. **Apply migrations:**
   ```bash
   dotnet ef database update
   ```

5. **Run project:**
   ```bash
   dotnet run
   ```

6. **Truy cập:**
   - HTTPS: `https://localhost:5001`
   - HTTP: `http://localhost:5000`

---

## 📋 CHỨC NĂNG ĐÃ TRIỂN KHAI

### ✅ Authentication
- [x] Login với username/password/role
- [x] Session management (30 phút)
- [x] Redirect theo role (Admin/Teacher/Student)
- [ ] Register (UI có nhưng chưa hoạt động)
- [ ] Forgot password
- [ ] Change password

### ✅ Admin Module
- [x] CRUD Topics (100%)
- [x] Layout Dashboard
- [ ] CRUD Users (Student/Teacher)
- [ ] Thống kê tổng quan
- [ ] Báo cáo

### ✅ Teacher Module
- [x] CRUD Questions với 4 Answers (100%)
- [x] Pagination (20 câu/page)
- [x] Include Topic, DifficultyLevel
- [ ] Filter Questions
- [ ] Tạo đề thi tự động
- [ ] Tạo đề thi thủ công
- [ ] Xem kết quả học sinh

### ❌ Student Module
- [ ] Xem danh sách đề thi
- [ ] Làm bài thi (timer)
- [ ] Submit bài thi
- [ ] Xem kết quả
- [ ] Lịch sử thi

---

## 🎯 ROADMAP

### Phase 1: Core Features (Hiện tại - 40%)
- ✅ Database design
- ✅ Authentication
- ✅ CRUD Topics
- ✅ CRUD Questions

### Phase 2: Exam Management (0%)
- [ ] Thuật toán tạo đề tự động
- [ ] Cấu hình ma trận đề (theo rule.md)
- [ ] Tạo đề thủ công
- [ ] Preview đề thi

### Phase 3: Student Features (0%)
- [ ] UI làm bài thi
- [ ] Timer countdown
- [ ] Auto-submit khi hết giờ
- [ ] Chấm điểm tự động
- [ ] Xem đáp án đúng/sai

### Phase 4: Statistics & Reports (0%)
- [ ] Dashboard Admin
- [ ] Dashboard Teacher
- [ ] Dashboard Student
- [ ] Export PDF/Excel
- [ ] Biểu đồ thống kê

### Phase 5: Advanced Features (Future)
- [ ] Import Questions từ Excel
- [ ] Ngân hàng đề thi mẫu
- [ ] Xếp hạng học sinh
- [ ] Email notification
- [ ] Mobile responsive

---

## 🐛 VẤN ĐỀ CẦN LƯU Ý

1. **Security:**
   - ⚠️ Password đang lưu plain text (chưa hash)
   - ⚠️ Chưa có CSRF protection đầy đủ
   - ⚠️ Session timeout cần test kỹ

2. **Performance:**
   - ⚠️ QuestionManager load toàn bộ Answers (N+1 query)
   - ✅ Đã có pagination ở server-side

3. **Business Logic:**
   - ❌ Chưa có validation tỷ lệ độ khó (60%-30%-10%)
   - ❌ Chưa có validation 7 Topics = 100%
   - ❌ Chưa implement thuật toán gen đề

4. **UI/UX:**
   - ⚠️ Chưa có loading indicator
   - ⚠️ Chưa có error handling UI
   - ⚠️ Form validation còn cơ bản

---

## 👥 TEAM & ROLES

**Dự án:** Hệ thống Thi trực tuyến Tin học  
**Đối tượng:** Học sinh THPT  
**Môn học:** Tin học (7 chủ đề A-G)  
**Quy mô:** 50 câu/đề, 0.2 điểm/câu

---

## 📞 HỖ TRỢ

Nếu cần hỗ trợ, tham khảo:
- `docs/business-requirements.md` - Yêu cầu nghiệp vụ
- `docs/database-schema.md` - Thiết kế database
- `docs/rule.md` - Quy tắc tạo đề thi

---

**Cập nhật lần cuối:** 07/11/2025  
**Branch hiện tại:** `refactor`
