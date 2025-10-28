# Business Requirements - Hệ thống Ngân hàng Đề thi

## Tổng quan Dự án

Hệ thống quản lý ngân hàng đề thi trực tuyến, cho phép giáo viên tạo và quản lý câu hỏi, tự động sinh đề thi, và học sinh thực hiện bài thi trực tuyến với chấm điểm tự động.

---

## Mục tiêu

### Mục tiêu Chính
1. Tự động hóa quy trình tạo đề thi và chấm điểm
2. Xây dựng ngân hàng câu hỏi có phân loại theo độ khó
3. Cung cấp nền tảng thi trực tuyến thuận tiện cho học sinh
4. Theo dõi và thống kê kết quả học tập

### Lợi ích
- **Giáo viên**: Tiết kiệm thời gian tạo đề và chấm bài
- **Học sinh**: Làm bài mọi lúc mọi nơi, nhận kết quả ngay lập tức
- **Quản trị**: Quản lý tập trung, thống kê toàn diện

---

## Vai trò và Quyền hạn

### 1. QUẢN TRỊ VIÊN (Admin)

#### 1.1. Quản lý Người dùng
**Chức năng:**
- ✅ Thêm tài khoản mới (Giáo viên/Học sinh)
- ✅ Chỉnh sửa thông tin người dùng
- ✅ Xóa tài khoản (soft delete)
- ✅ Kích hoạt/Vô hiệu hóa tài khoản
- ✅ Reset mật khẩu
- ✅ Phân quyền và quản lý vai trò
- ✅ Xem danh sách tất cả người dùng (phân trang, tìm kiếm, lọc)

**Màn hình:**
- Danh sách người dùng
- Form thêm/sửa người dùng
- Chi tiết người dùng

#### 1.2. Quản lý Môn học
**Chức năng:**
- ✅ Thêm/Sửa/Xóa môn học
- ✅ Quản lý mã môn học
- ✅ Kích hoạt/Vô hiệu hóa môn học

**Màn hình:**
- Danh sách môn học
- Form thêm/sửa môn học

#### 1.3. Thống kê và Báo cáo
**Chức năng:**
- ✅ Dashboard tổng quan:
  - Tổng số người dùng (Admin/Giáo viên/Học sinh)
  - Tổng số câu hỏi (theo môn học, độ khó)
  - Tổng số đề thi (theo trạng thái)
  - Số lượt thi trong tháng
- ✅ Báo cáo chi tiết:
  - Thống kê câu hỏi theo môn học
  - Thống kê đề thi theo giáo viên
  - Thống kê kết quả thi theo lớp/môn
- ✅ Export báo cáo (PDF, Excel)

**Màn hình:**
- Dashboard
- Trang báo cáo chi tiết

#### 1.4. Quản lý Dữ liệu
**Chức năng:**
- ✅ Sao lưu database
- ✅ Khôi phục dữ liệu
- ✅ Xóa dữ liệu không còn sử dụng (câu hỏi inactive, đề thi cũ)
- ✅ Xem lịch sử hoạt động hệ thống (audit log)

---

### 2. GIÁO VIÊN (Teacher)

#### 2.1. Quản lý Câu hỏi

**Thêm câu hỏi mới:**
- ✅ Nhập nội dung câu hỏi (WYSIWYG editor, hỗ trợ ảnh)
- ✅ Chọn môn học từ dropdown
- ✅ Nhập 4 đáp án (A, B, C, D)
- ✅ Chọn đáp án đúng (radio button)
- ✅ Chọn độ khó (Easy/Medium/Hard)
- ✅ Preview câu hỏi trước khi lưu
- ✅ Lưu nháp hoặc xuất bản ngay

**Sửa câu hỏi:**
- ✅ Chỉnh sửa nội dung câu hỏi và đáp án
- ✅ Thay đổi độ khó, môn học
- ✅ Xem lịch sử chỉnh sửa

**Xóa câu hỏi:**
- ✅ Soft delete (chuyển status = inactive)
- ✅ Cảnh báo nếu câu hỏi đang được sử dụng trong đề thi

**Tìm kiếm và Lọc:**
- ✅ Tìm theo từ khóa trong nội dung câu hỏi
- ✅ Lọc theo môn học
- ✅ Lọc theo độ khó
- ✅ Lọc theo trạng thái (active/inactive)
- ✅ Lọc theo người tạo
- ✅ Sắp xếp theo ngày tạo, độ khó

**Màn hình:**
- Danh sách câu hỏi (table view với pagination)
- Form thêm/sửa câu hỏi
- Preview câu hỏi
- Import câu hỏi từ Excel/CSV (optional)

#### 2.2. Quản lý Đề thi

**Tạo đề thi Tự động:**

*Bước 1: Cấu hình đề thi*
- ✅ Nhập tên đề thi
- ✅ Chọn môn học
- ✅ Chọn thời gian làm bài (phút)
- ✅ Chọn số lượng câu hỏi (mặc định 50)
- ✅ Cấu hình tỷ lệ độ khó:
  - Easy: 60% (30 câu)
  - Medium: 30% (15 câu)
  - Hard: 10% (5 câu)
- ✅ Tùy chọn:
  - Trộn câu hỏi (shuffle questions)
  - Trộn đáp án (shuffle answers)
  - Hiển thị đáp án sau khi nộp
  - Điểm đạt (passing score)

*Bước 2: Sinh đề tự động*
- ✅ Hệ thống tự động chọn câu hỏi theo thuật toán:
  ```
  INPUT:
  - subject_id
  - total_questions = 50
  - easy% = 60, medium% = 30, hard% = 10
  
  PROCESS:
  1. Tính số câu mỗi loại:
     - easy_count = 50 × 0.6 = 30
     - medium_count = 50 × 0.3 = 15
     - hard_count = 50 × 0.1 = 5
  
  2. Truy vấn ngẫu nhiên:
     SELECT TOP 30 * FROM Questions 
     WHERE subject_id = @subject_id 
       AND difficulty_level = 'easy' 
       AND status = 'active'
     ORDER BY NEWID()
     
     (Tương tự cho medium và hard)
  
  3. Kiểm tra đủ số lượng:
     IF (total_selected < 50) 
       THEN show warning "Không đủ câu hỏi"
  
  4. Trộn thứ tự (nếu shuffle_questions = true):
     ORDER BY NEWID()
  
  5. Gán thứ tự: question_order = 1 to 50
  
  6. Lưu vào Exam_Questions
  
  OUTPUT: exam_id
  ```

*Bước 3: Xem trước và Lưu*
- ✅ Hiển thị preview đề thi đầy đủ
- ✅ Kiểm tra số lượng câu theo độ khó
- ✅ Cho phép thay đổi câu hỏi cụ thể
- ✅ Lưu nháp (draft) hoặc xuất bản (published)

**Tạo đề thi Thủ công:**
- ✅ Chọn môn học
- ✅ Tìm kiếm và chọn từng câu hỏi cụ thể
- ✅ Sắp xếp thứ tự câu hỏi (drag & drop)
- ✅ Xem preview và lưu

**Sửa đề thi:**
- ✅ Chỉnh sửa thông tin đề (tên, thời gian, cấu hình)
- ✅ Thêm/Xóa/Thay thế câu hỏi
- ✅ Chỉ được sửa đề có status = 'draft'
- ✅ Đề đã xuất bản có thể archive nhưng không sửa

**Xóa đề thi:**
- ✅ Soft delete (status = 'archived')
- ✅ Không được xóa nếu đã có học sinh làm bài

**Xuất bản đề thi:**
- ✅ Chuyển status từ 'draft' → 'published'
- ✅ Ghi nhận published_at = thời gian xuất bản
- ✅ Sau khi xuất bản, học sinh mới thấy được đề

**Màn hình:**
- Danh sách đề thi
- Wizard tạo đề thi (multi-step form)
- Preview đề thi
- Quản lý câu hỏi trong đề (drag & drop)

#### 2.3. Chấm điểm và Xem kết quả

**Chấm điểm Tự động:**
```
TRIGGER: Khi học sinh nộp bài (student submits exam)

PROCESS:
1. Lấy danh sách câu trả lời:
   SELECT sa.question_id, sa.answer_id
   FROM Student_Answers sa
   WHERE sa.student_exam_id = @student_exam_id

2. So sánh với đáp án đúng:
   UPDATE Student_Answers
   SET is_correct = CASE 
     WHEN sa.answer_id = (
       SELECT answer_id FROM Answers 
       WHERE question_id = sa.question_id 
       AND is_correct = 1
     ) THEN 1
     ELSE 0
   END

3. Tính điểm:
   total_correct = COUNT(is_correct = 1)
   total_wrong = COUNT(is_correct = 0)
   total_unanswered = 50 - (total_correct + total_wrong)
   
   score = (total_correct / 50) × 10
   (hoặc tính theo points cụ thể của từng câu)

4. Cập nhật Student_Exams:
   UPDATE Student_Exams
   SET score = @score,
       total_correct = @total_correct,
       total_wrong = @total_wrong,
       total_unanswered = @total_unanswered,
       status = 'completed',
       submitted_at = GETDATE()

OUTPUT: score
```

**Xem danh sách Bài thi:**
- ✅ Danh sách học sinh đã làm bài theo đề thi
- ✅ Hiển thị: Tên, Điểm, Thời gian nộp, Trạng thái
- ✅ Sắp xếp theo điểm (cao/thấp), thời gian
- ✅ Lọc theo trạng thái (completed/in_progress)
- ✅ Tìm kiếm học sinh

**Xem chi tiết Bài làm:**
- ✅ Thông tin bài thi:
  - Tên học sinh
  - Thời gian làm bài
  - Điểm số
  - Số câu đúng/sai/chưa làm
- ✅ Chi tiết từng câu:
  - Câu hỏi
  - 4 đáp án (highlight đáp án đúng và đáp án học sinh chọn)
  - Giải thích (nếu có)
  - Trạng thái: Đúng ✅ / Sai ❌ / Chưa làm ⚪

**Thống kê:**
- ✅ Điểm trung bình của lớp/đề thi
- ✅ Phân bố điểm số (histogram)
- ✅ Tỷ lệ đạt/không đạt
- ✅ Câu hỏi có tỷ lệ sai cao nhất (cần review)
- ✅ Thời gian làm bài trung bình
- ✅ Export kết quả (Excel, PDF)

**Màn hình:**
- Danh sách bài thi theo đề
- Chi tiết bài làm của học sinh
- Trang thống kê và báo cáo

---

### 3. HỌC SINH (Student)

#### 3.1. Làm bài thi

**Xem danh sách Đề thi:**
- ✅ Hiển thị các đề thi đã xuất bản (status = 'published')
- ✅ Thông tin: Tên đề, Môn học, Số câu, Thời gian, Điểm đạt
- ✅ Trạng thái: Chưa làm / Đã làm / Đang làm
- ✅ Lọc theo môn học
- ✅ Tìm kiếm theo tên đề

**Bắt đầu làm bài:**

*Bước 1: Xác nhận bắt đầu*
- ✅ Hiển thị thông tin đề thi
- ✅ Lưu ý về thời gian và quy định
- ✅ Nút "Bắt đầu" → Tạo record Student_Exams:
  ```sql
  INSERT INTO Student_Exams 
  (exam_id, student_id, start_time, end_time, status)
  VALUES 
  (@exam_id, @student_id, GETDATE(), 
   DATEADD(MINUTE, @duration, GETDATE()), 
   'in_progress')
  ```

*Bước 2: Giao diện làm bài*
- ✅ Hiển thị câu hỏi theo thứ tự
- ✅ Navigation:
  - Danh sách câu hỏi (1-50) dạng grid
  - Nút Previous/Next
  - Nhấn số câu để jump
- ✅ Chọn đáp án (radio button A/B/C/D)
- ✅ Đếm ngược thời gian (countdown timer)
- ✅ Đánh dấu câu cần xem lại (flag)
- ✅ Hiển thị trạng thái:
  - Đã làm: màu xanh
  - Chưa làm: màu trắng
  - Đã đánh dấu: màu vàng
- ✅ Auto-save câu trả lời mỗi 30s:
  ```sql
  INSERT INTO Student_Answers 
  (student_exam_id, question_id, answer_id, answered_at)
  VALUES (...)
  ON CONFLICT UPDATE ...
  ```

*Bước 3: Nộp bài*
- ✅ Kiểm tra câu chưa làm → Hiển thị cảnh báo
- ✅ Xác nhận nộp bài (modal confirmation)
- ✅ Nút "Nộp bài" → Trigger chấm điểm tự động
- ✅ Tự động nộp khi hết giờ:
  ```javascript
  if (remaining_time <= 0) {
    autoSubmit();
  }
  ```

**Màn hình:**
- Danh sách đề thi
- Trang làm bài (exam taking interface)
- Modal xác nhận nộp

#### 3.2. Xem kết quả

**Xem điểm ngay sau khi nộp:**
- ✅ Hiển thị popup kết quả:
  - Tổng điểm: X/10
  - Số câu đúng: X/50
  - Số câu sai: X/50
  - Phần trăm hoàn thành: X%
  - Trạng thái: Đạt ✅ / Không đạt ❌
- ✅ Nút "Xem chi tiết" → Chuyển đến trang kết quả chi tiết

**Xem chi tiết Bài làm:**
- ✅ Thông tin chung:
  - Tên đề thi
  - Thời gian làm bài
  - Điểm số
- ✅ Danh sách câu hỏi:
  - Câu hỏi
  - Đáp án đã chọn (highlight)
  - Đáp án đúng (nếu show_answers = true)
  - Giải thích
  - Icon: ✅ Đúng / ❌ Sai
- ✅ Navigation giữa các câu
- ✅ Lọc: Tất cả / Đúng / Sai / Chưa làm

**Lịch sử Bài thi:**
- ✅ Danh sách các bài thi đã làm
- ✅ Hiển thị: Tên đề, Môn, Điểm, Ngày thi
- ✅ Sắp xếp theo ngày thi (mới nhất)
- ✅ Lọc theo môn học
- ✅ Xem lại chi tiết mỗi bài

**Màn hình:**
- Popup kết quả
- Trang chi tiết bài làm
- Trang lịch sử bài thi

#### 3.3. Quản lý Cá nhân

**Thông tin Cá nhân:**
- ✅ Xem thông tin: Tên, Email, SĐT, Ngày sinh
- ✅ Chỉnh sửa thông tin (trừ username)
- ✅ Đổi mật khẩu:
  - Nhập mật khẩu cũ
  - Nhập mật khẩu mới (min 8 ký tự)
  - Xác nhận mật khẩu mới

**Thống kê Cá nhân:**
- ✅ Tổng số bài thi đã làm
- ✅ Điểm trung bình
- ✅ Điểm cao nhất/thấp nhất
- ✅ Biểu đồ điểm theo thời gian
- ✅ Phân bố điểm theo môn học

**Màn hình:**
- Trang hồ sơ cá nhân
- Form đổi mật khẩu
- Dashboard thống kê cá nhân

---

## Quy trình Nghiệp vụ

### Quy trình 1: Giáo viên Tạo đề thi

```
[START]
  ↓
1. Giáo viên đăng nhập
  ↓
2. Tạo/Nhập câu hỏi vào ngân hàng
   - Phân loại độ khó (Easy/Medium/Hard)
   - Gán môn học
  ↓
3. Tạo đề thi mới
   ├─→ Tự động: Chọn môn, số câu, tỷ lệ → Hệ thống sinh đề
   └─→ Thủ công: Chọn từng câu cụ thể
  ↓
4. Xem preview đề thi
  ↓
5. Chỉnh sửa nếu cần
  ↓
6. Lưu nháp hoặc Xuất bản
  ↓
[END]
```

### Quy trình 2: Học sinh Làm bài và Nhận kết quả

```
[START]
  ↓
1. Học sinh đăng nhập
  ↓
2. Xem danh sách đề thi available
  ↓
3. Chọn đề thi → Bắt đầu
   - Hệ thống tạo Student_Exams
   - Bắt đầu đếm ngược thời gian
  ↓
4. Làm bài
   - Chọn đáp án cho mỗi câu
   - Auto-save định kỳ
   - Di chuyển giữa các câu
  ↓
5. Nộp bài (hoặc hết giờ tự động nộp)
   - Hệ thống chấm điểm tự động
  ↓
6. Xem điểm ngay lập tức
  ↓
7. Xem chi tiết đáp án (nếu được phép)
  ↓
[END]
```

### Quy trình 3: Giáo viên Xem và Phân tích Kết quả

```
[START]
  ↓
1. Giáo viên đăng nhập
  ↓
2. Vào trang "Quản lý Đề thi"
  ↓
3. Chọn đề thi cần xem kết quả
  ↓
4. Xem danh sách học sinh đã làm
   - Điểm số
   - Thời gian nộp
  ↓
5. Xem chi tiết bài làm từng học sinh
   - Câu trả lời
   - Câu đúng/sai
  ↓
6. Xem thống kê tổng quan
   - Điểm TB
   - Phân bố điểm
   - Câu hỏi khó nhất
  ↓
7. Export báo cáo (optional)
  ↓
[END]
```

---

## Tính năng Đặc biệt

### 1. Tự động Sinh Đề thi

**Mô tả:** Hệ thống tự động chọn câu hỏi theo tỷ lệ độ khó được cấu hình.

**Input:**
- Môn học (subject_id)
- Tổng số câu (default: 50)
- Tỷ lệ độ khó: Easy 60%, Medium 30%, Hard 10%

**Output:** Đề thi với 50 câu được chọn ngẫu nhiên theo đúng tỷ lệ

**Ưu điểm:**
- Tiết kiệm thời gian cho giáo viên
- Đảm bảo cân đối độ khó
- Mỗi lần sinh ra đề khác nhau (random)

### 2. Chấm điểm Tự động

**Mô tả:** Hệ thống tự động chấm điểm ngay khi học sinh nộp bài.

**Process:**
1. So sánh câu trả lời với đáp án đúng
2. Tính số câu đúng/sai
3. Tính điểm: `score = (correct / total) × 10`
4. Lưu kết quả vào database

**Ưu điểm:**
- Học sinh nhận kết quả ngay lập tức
- Giáo viên không cần chấm thủ công
- Giảm thiểu sai sót

### 3. Auto-save Câu trả lời

**Mô tả:** Tự động lưu câu trả lời của học sinh mỗi 30 giây.

**Lợi ích:**
- Tránh mất dữ liệu khi mất kết nối
- Học sinh có thể tiếp tục làm bài sau khi reload
- Giảm lo lắng về việc mất công làm bài

### 4. Countdown Timer với Auto-submit

**Mô tả:** Đếm ngược thời gian, tự động nộp bài khi hết giờ.

**Features:**
- Hiển thị thời gian còn lại
- Cảnh báo khi còn 5 phút
- Tự động nộp khi countdown = 0
- Khóa không cho chỉnh sửa sau khi hết giờ

### 5. Trộn Câu hỏi và Đáp án

**Mô tả:** Random thứ tự câu hỏi và đáp án để tránh gian lận.

**Cấu hình:**
- `shuffle_questions`: Trộn thứ tự câu hỏi
- `shuffle_answers`: Trộn thứ tự đáp án (A/B/C/D)

### 6. Thống kê và Phân tích

**Chức năng:**
- Điểm trung bình theo lớp/môn/đề
- Phân bố điểm (histogram)
- Câu hỏi có tỷ lệ sai cao → Cần review
- Xu hướng điểm theo thời gian
- So sánh giữa các lớp/kỳ thi

---

## Yêu cầu Phi Chức năng

### 1. Hiệu năng (Performance)
- ✅ Thời gian tải trang < 2s
- ✅ Hỗ trợ đồng thời 100+ học sinh làm bài
- ✅ Database query optimization (indexes)
- ✅ Caching cho dữ liệu tĩnh (subjects, published exams)

### 2. Bảo mật (Security)
- ✅ Mã hóa mật khẩu (BCrypt)
- ✅ Authentication: ASP.NET Identity
- ✅ Authorization: Role-based access control
- ✅ HTTPS only
- ✅ XSS, CSRF protection
- ✅ SQL Injection prevention (EF Core parameterized queries)
- ✅ Session timeout: 30 phút không hoạt động

### 3. Độ tin cậy (Reliability)
- ✅ Auto-save để tránh mất dữ liệu
- ✅ Transaction cho các thao tác quan trọng
- ✅ Error handling và logging
- ✅ Backup database hàng ngày

### 4. Khả năng mở rộng (Scalability)
- ✅ Thiết kế database chuẩn hóa
- ✅ Pagination cho danh sách dài
- ✅ Lazy loading cho ảnh/media
- ✅ Có thể scale horizontal (thêm server)

### 5. Khả năng sử dụng (Usability)
- ✅ Giao diện thân thiện, responsive
- ✅ Hỗ trợ mobile (tablet/smartphone)
- ✅ Thông báo rõ ràng (success/error messages)
- ✅ Validation ngay trên form
- ✅ Help/Tutorial cho người dùng mới

### 6. Tương thích (Compatibility)
- ✅ Browsers: Chrome, Firefox, Edge, Safari (latest versions)
- ✅ Devices: Desktop, Tablet, Mobile
- ✅ Screen resolutions: >= 1024x768

---

## User Stories

### Admin
- Là Admin, tôi muốn thêm tài khoản giáo viên/học sinh để quản lý người dùng hệ thống
- Là Admin, tôi muốn xem thống kê tổng quan để nắm bắt tình hình hoạt động
- Là Admin, tôi muốn quản lý môn học để tổ chức ngân hàng câu hỏi

### Giáo viên
- Là Giáo viên, tôi muốn thêm câu hỏi vào ngân hàng để xây dựng bộ đề
- Là Giáo viên, tôi muốn tạo đề thi tự động để tiết kiệm thời gian
- Là Giáo viên, tôi muốn xem kết quả học sinh để đánh giá năng lực
- Là Giáo viên, tôi muốn xem câu hỏi có tỷ lệ sai cao để cải thiện chất lượng

### Học sinh
- Là Học sinh, tôi muốn xem danh sách đề thi để chọn bài làm
- Là Học sinh, tôi muốn làm bài thi trực tuyến một cách thuận tiện
- Là Học sinh, tôi muốn xem điểm ngay sau khi nộp để biết kết quả
- Là Học sinh, tôi muốn xem lại đáp án để học hỏi từ sai lầm
- Là Học sinh, tôi muốn xem lịch sử bài thi để theo dõi tiến bộ

---

## Roadmap và Ưu tiên

### Phase 1: MVP (Minimum Viable Product) - 2 tháng
- ✅ Đăng nhập/Đăng ký (3 vai trò)
- ✅ CRUD Câu hỏi (Giáo viên)
- ✅ Tạo đề thi tự động (Giáo viên)
- ✅ Làm bài thi (Học sinh)
- ✅ Chấm điểm tự động
- ✅ Xem kết quả cơ bản

### Phase 2: Enhancement - 1 tháng
- ✅ Quản lý người dùng (Admin)
- ✅ Thống kê và báo cáo
- ✅ Tạo đề thi thủ công
- ✅ Auto-save câu trả lời
- ✅ Countdown timer
- ✅ Responsive design

### Phase 3: Advanced Features - 1 tháng
- ✅ Import/Export câu hỏi (Excel, CSV)
- ✅ Rich text editor cho câu hỏi (hỗ trợ ảnh, công thức)
- ✅ Quản lý lớp học
- ✅ Phân công đề thi cho lớp
- ✅ Lịch sử hoạt động (audit log)
- ✅ Email notification

### Phase 4: Optimization - Ongoing
- ✅ Performance tuning
- ✅ Security hardening
- ✅ UI/UX improvements
- ✅ Bug fixes

---

## Công nghệ Sử dụng

### Backend
- **Framework:** ASP.NET Core 8.0 MVC
- **ORM:** Entity Framework Core
- **Database:** SQL Server 2022
- **Authentication:** ASP.NET Identity
- **Logging:** Serilog

### Frontend
- **HTML5/CSS3**
- **Bootstrap 5** (responsive design)
- **JavaScript/jQuery**
- **AJAX** (for auto-save, dynamic loading)
- **Chart.js** (for statistics)

### Tools
- **IDE:** Visual Studio 2022
- **Version Control:** Git
- **Database Management:** SQL Server Management Studio (SSMS)
- **API Testing:** Postman

---

## Tài liệu Tham khảo
- [ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Bootstrap Documentation](https://getbootstrap.com/docs/)

---

## Ghi chú
- Tài liệu này có thể được cập nhật theo quá trình phát triển
- Mọi thay đổi yêu cầu cần được approve bởi Product Owner
- Version: 1.0
- Last updated: 2025-10-27
