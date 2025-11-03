# 🧩 Mô tả nghiệp vụ: Tạo đề thi

## 1. **Thực thể tham gia**

| Thực thể           | Vai trò                     | Mô tả                                                                                                                                      |
| ------------------ | --------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
| **Admin**          | Quản trị hệ thống           | - Cấu hình ma trận mặc định (tỉ lệ chủ đề, mức độ khó).                                                                                    |
| **Giáo viên (GV)** | Người tạo và quản lý đề thi | - Tạo ma trận đề (công khai hoặc riêng). <br> - Tùy chỉnh tỉ lệ chủ đề, số lượng câu hỏi, mức độ khó. <br> - Xuất bản đề cho học sinh làm. |
| **Học sinh (HS)**  | Người làm và ôn luyện đề    | - Có thể tạo đề để tự ôn. <br> - Có thể chọn tạo đề theo ma trận GV công bố hoặc tùy chỉnh theo ý muốn.                                    |

---

## 2. **Cấu trúc ma trận đề**

- **Tổng số câu:** 50 câu/đề
- **Tổng số chủ đề:** 7 chủ đề (A, B, C, D, E, F, G)
- **Mức độ câu hỏi:**

  - Nhận biết (dễ): **40%**
  - Thông hiểu (trung bình): **30%**
  - Vận dụng (khó): **30%**

- **Tỉ lệ chủ đề (ví dụ):**

  | Chủ đề   | Tỉ lệ (%) | Số câu (trên 50) |
  | -------- | --------- | ---------------- |
  | A        | 15%       | 6                |
  | B        | 20%       | 10               |
  | C        | 10%       | 5                |
  | D        | 15%       | 7                |
  | E        | 15%       | 7                |
  | F        | 10%       | 5                |
  | G        | 15%       | 7                |
  | **Tổng** | **100%**  | **50**           |

> 📘 _Ma trận đề có thể được công khai để học sinh tự ôn tập theo tỉ lệ chuẩn mà giáo viên cung cấp._

---

## 3. **Chức năng tạo đề thi**

Khi **GV hoặc HS** bấm nút **“Tạo đề”**, hệ thống cung cấp **2 lựa chọn chính**:

### ➤ **Lựa chọn 1 – Tạo nhanh (Mặc định)**

- Người dùng **chỉ nhập tổng số câu hỏi** (ví dụ: 50 câu).
- Hệ thống **sử dụng ma trận mặc định** để sinh đề:

  - Tỉ lệ chủ đề theo cấu hình hệ thống hoặc GV.
  - Tỉ lệ mức độ khó: 40% - 30% - 30%.

### ➤ **Lựa chọn 2 – Tạo tùy chỉnh**

- Người dùng được phép:

  - Chọn **tỉ lệ chủ đề** mong muốn (VD: chỉ ôn 1 chủ đề = 100%).
  - Chọn **tổng số câu hỏi**.

- Hệ thống sinh đề dựa trên thông số tùy chỉnh, vẫn đảm bảo tỉ lệ mức độ khó (NB, TH, VD).

---

## 4. **Kết quả đầu ra**

- Đề thi được sinh **ngẫu nhiên** từ ngân hàng câu hỏi dựa theo:

  - Tỉ lệ chủ đề.
  - Tỉ lệ mức độ khó.
  - Tổng số câu yêu cầu.

- Đề được **lưu lại**:

  - Nếu **GV** tạo → dùng cho kiểm tra chính thức.
  - Nếu **HS** tạo → dùng để ôn luyện cá nhân.

---

## 5. **Quy tắc nghiệp vụ (Business Rules)**

1. **HS** không được phép thay đổi ma trận gốc của GV.
2. **Tỉ lệ mức độ câu hỏi (NB, TH, VD)** mặc định là 40% - 30% - 30%.

   - Chỉ **Admin hoặc GV** có quyền thay đổi tỉ lệ này.

3. Hệ thống luôn đảm bảo **tổng tỉ lệ chủ đề = 100%** trước khi sinh đề.
4. Mỗi câu hỏi khi gen vào đề phải **khớp với chủ đề và mức độ tương ứng**.
