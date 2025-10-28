# TinhocOnline - Hệ thống Ngân hàng Đề thi Trực tuyến

## Mô tả
Hệ thống quản lý ngân hàng đề thi trực tuyến, cho phép giáo viên tạo và quản lý câu hỏi, tự động sinh đề thi, và học sinh thực hiện bài thi trực tuyến với chấm điểm tự động.

## Công nghệ sử dụng
- **Backend:** ASP.NET Core 8.0 MVC
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Frontend:** HTML, CSS, Bootstrap, JavaScript

---

## Hướng dẫn Cài đặt và Chạy Project

### Bước 1: Tạo Connection String

1. Mở file `appsettings.json` trong thư mục gốc của project
2. Thêm connection string vào trong file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TinhocOnlineDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Lưu ý:** 
- Thay `localhost` bằng tên SQL Server của bạn nếu khác
- Nếu sử dụng SQL Server Authentication, thay `Trusted_Connection=True` bằng:
  ```
  User Id=your_username;Password=your_password;
  ```

### Bước 2: Cấu hình DbContext trong Program.cs

Mở file `Program.cs` và thêm cấu hình sau:

```csharp
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### Bước 3: Chạy Migration để tạo Database

Mở **Terminal** hoặc **Package Manager Console** và chạy các lệnh sau:

#### 3.1. Thêm Migration
```bash
dotnet ef migrations add InitialCreate
```

Lệnh này sẽ tạo folder `Migrations` chứa các file migration.

#### 3.2. Cập nhật Database
```bash
dotnet ef database update
```

Lệnh này sẽ:
- Tạo database `TinhocOnlineDB` (nếu chưa có)
- Tạo tất cả các bảng theo schema đã định nghĩa

#### 3.3. Kiểm tra Database

Mở **SQL Server Management Studio (SSMS)** và kiểm tra:
- Database `TinhocOnlineDB` đã được tạo
- 8 bảng: Users, Subjects, Questions, Answers, Exams, Exam_Questions, Student_Exams, Student_Answers

---

## Lưu ý

### Nếu chưa cài đặt EF Core Tools:
```bash
dotnet tool install --global dotnet-ef
```

### Nếu thiếu package (nuget 9.0.0):
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
```

### Các lệnh Migration hữu ích khác:

```bash
# Xem danh sách migrations
dotnet ef migrations list

# Xóa migration cuối cùng (nếu chưa update database)
dotnet ef migrations remove

# Rollback database về migration trước
dotnet ef database update <MigrationName>

# Tạo SQL script từ migration
dotnet ef migrations script

# Xóa database
dotnet ef database drop
```

---

## Cấu trúc Database

### Danh sách Bảng:
1. **Users** - Quản lý người dùng (Admin, Teacher, Student)
2. **Subjects** - Quản lý môn học
3. **Questions** - Ngân hàng câu hỏi
4. **Answers** - Đáp án cho câu hỏi (4 đáp án A/B/C/D)
5. **Exams** - Đề thi
6. **Exam_Questions** - Câu hỏi trong đề thi (quan hệ nhiều-nhiều)
7. **Student_Exams** - Bài thi của học sinh
8. **Student_Answers** - Câu trả lời của học sinh

### Quy tắc Tính điểm:
- Mỗi đề thi: **50 câu hỏi**
- Mỗi câu đúng: **0.2 điểm**
- Tổng điểm tối đa: **10 điểm**
- Công thức: `Điểm = (Số câu đúng / 50) × 10`

---

## Tài liệu Tham khảo

- **Database Schema:** [docs/database-schema.md](docs/database-schema.md)
- **Business Requirements:** [docs/business-requirements.md](docs/business-requirements.md)
- **SQL Script:** [docs/create-database.sql](docs/create-database.sql)

---

## Tiếp theo

Sau khi hoàn thành migration, tiếp tục với:
- [ ] Tạo Controllers (Admin, Teacher, Student)
- [ ] Tạo Views
- [ ] Implement Authentication & Authorization
- [ ] Implement Business Logic (Tạo đề thi tự động, Chấm điểm)
- [ ] Testing

---