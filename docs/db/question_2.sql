-- =============================================
-- Bổ sung Questions và Answers (Phần 2)
-- Mỗi topic thêm 7 câu hỏi: 3 easy, 2 medium, 2 hard
-- Để tổng mỗi topic có 16 câu (6 easy, 5 medium, 5 hard)
-- Đủ để tạo đề thi 100 câu
-- =============================================

-- =============================================
-- TOPIC 1: Máy tính và xã hội tri thức (Thêm 7 câu: 64-70)
-- =============================================
-- Easy Questions (thêm 3 câu)
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(1, N'Hệ điều hành nào sau đây là phổ biến nhất hiện nay?', 'easy', '10', 1, 'active'),
(1, N'RAM có chức năng gì trong máy tính?', 'easy', '10', 1, 'active'),
(1, N'Browser là gì?', 'easy', '10', 1, 'active');

-- Medium Questions (thêm 2 câu)
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(1, N'Blockchain được ứng dụng trong lĩnh vực nào ngoài tiền mã hóa?', 'medium', '11', 1, 'active'),
(1, N'5G khác biệt với 4G như thế nào?', 'medium', '11', 1, 'active');

-- Hard Questions (thêm 2 câu)
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(1, N'Quantum Computing có tiềm năng ứng dụng gì trong tương lai?', 'hard', '12', 1, 'active'),
(1, N'Edge Computing khác Cloud Computing như thế nào?', 'hard', '12', 1, 'active');

-- Answers cho Topic 1 (7 câu x 4 đáp án = 28 đáp án)
-- Câu 64
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(64, N'Windows, macOS, Linux', 1),
(64, N'Chỉ có Windows', 0),
(64, N'Chỉ có macOS', 0),
(64, N'MS-DOS', 0);

-- Câu 65
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(65, N'Lưu trữ dữ liệu tạm thời khi máy hoạt động', 1),
(65, N'Lưu trữ vĩnh viễn', 0),
(65, N'Chỉ chạy hệ điều hành', 0),
(65, N'Kết nối Internet', 0);

-- Câu 66
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(66, N'Trình duyệt web', 1),
(66, N'Hệ điều hành', 0),
(66, N'Phần mềm diệt virus', 0),
(66, N'Ổ cứng', 0);

-- Câu 67
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(67, N'Quản lý chuỗi cung ứng, y tế, bất động sản', 1),
(67, N'Chỉ dùng cho Bitcoin', 0),
(67, N'Không có ứng dụng khác', 0),
(67, N'Chỉ trong game', 0);

-- Câu 68
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(68, N'Tốc độ nhanh hơn 10-100 lần, độ trễ thấp hơn', 1),
(68, N'Chỉ khác tên gọi', 0),
(68, N'5G chậm hơn 4G', 0),
(68, N'Không có sự khác biệt', 0);

-- Câu 69
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(69, N'Mã hóa an ninh, mô phỏng phân tử, tối ưu hóa phức tạp', 1),
(69, N'Chỉ chơi game', 0),
(69, N'Thay thế máy tính cá nhân', 0),
(69, N'Không có ứng dụng', 0);

-- Câu 70
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(70, N'Edge xử lý gần nguồn dữ liệu, Cloud xử lý tập trung', 1),
(70, N'Giống nhau hoàn toàn', 0),
(70, N'Edge chậm hơn Cloud', 0),
(70, N'Cloud rẻ hơn Edge', 0);

-- =============================================
-- TOPIC 2: Mạng máy tính và Internet (Thêm 7 câu: 71-77)
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(2, N'URL là viết tắt của gì?', 'easy', '10', 1, 'active'),
(2, N'Modem có chức năng gì?', 'easy', '10', 1, 'active'),
(2, N'WiFi là gì?', 'easy', '10', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(2, N'Router và Switch khác nhau như thế nào?', 'medium', '11', 1, 'active'),
(2, N'Proxy Server có vai trò gì?', 'medium', '11', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(2, N'Load Balancer hoạt động theo cơ chế nào?', 'hard', '12', 1, 'active'),
(2, N'CDN (Content Delivery Network) tối ưu tốc độ như thế nào?', 'hard', '12', 1, 'active');

-- Answers cho Topic 2
-- Câu 71
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(71, N'Uniform Resource Locator', 1),
(71, N'Universal Resource Link', 0),
(71, N'Unique Resource Locator', 0),
(71, N'United Resource Link', 0);

-- Câu 72
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(72, N'Chuyển đổi tín hiệu analog/digital', 1),
(72, N'Chỉ lưu trữ dữ liệu', 0),
(72, N'Bảo mật mạng', 0),
(72, N'Tăng tốc Internet', 0);

-- Câu 73
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(73, N'Mạng không dây dùng sóng radio', 1),
(73, N'Dây cáp mạng', 0),
(73, N'Hệ điều hành', 0),
(73, N'Phần mềm diệt virus', 0);

-- Câu 74
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(74, N'Router định tuyến giữa các mạng, Switch kết nối trong cùng mạng', 1),
(74, N'Giống nhau hoàn toàn', 0),
(74, N'Switch nhanh hơn Router', 0),
(74, N'Không có sự khác biệt', 0);

-- Câu 75
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(75, N'Trung gian giữa client và server, cache, lọc nội dung', 1),
(75, N'Chỉ lưu trữ dữ liệu', 0),
(75, N'Không có vai trò', 0),
(75, N'Chỉ tăng tốc độ', 0);

-- Câu 76
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(76, N'Phân phối tải đều trên nhiều server', 1),
(76, N'Chỉ lưu trữ dữ liệu', 0),
(76, N'Không có chức năng', 0),
(76, N'Chỉ bảo mật', 0);

-- Câu 77
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(77, N'Phân phối nội dung từ server gần người dùng nhất', 1),
(77, N'Chỉ lưu trữ tập trung', 0),
(77, N'Không tối ưu được', 0),
(77, N'Chỉ tăng giá dịch vụ', 0);

-- =============================================
-- TOPIC 3: Tổ chức lưu trữ, tìm kiếm (Thêm 7 câu: 78-84)
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(3, N'1 TB bằng bao nhiêu GB?', 'easy', '10', 1, 'active'),
(3, N'File .xlsx là loại file gì?', 'easy', '10', 1, 'active'),
(3, N'Backup dữ liệu có mục đích gì?', 'easy', '10', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(3, N'RAID là gì và có mấy loại phổ biến?', 'medium', '11', 1, 'active'),
(3, N'Index trong database có tác dụng gì?', 'medium', '11', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(3, N'Sharding trong database là gì?', 'hard', '12', 1, 'active'),
(3, N'Hash function được sử dụng trong lưu trữ như thế nào?', 'hard', '12', 1, 'active');

-- Answers cho Topic 3
-- Câu 78
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(78, N'1024 GB', 1),
(78, N'1000 GB', 0),
(78, N'512 GB', 0),
(78, N'2048 GB', 0);

-- Câu 79
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(79, N'File Excel', 1),
(79, N'File Word', 0),
(79, N'File PDF', 0),
(79, N'File PowerPoint', 0);

-- Câu 80
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(80, N'Sao lưu để phục hồi khi mất dữ liệu', 1),
(80, N'Tăng tốc máy tính', 0),
(80, N'Giảm dung lượng ổ cứng', 0),
(80, N'Không có mục đích', 0);

-- Câu 81
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(81, N'Hệ thống đĩa dự phòng: RAID 0, 1, 5, 10', 1),
(81, N'Chỉ có 1 loại', 0),
(81, N'Phần mềm diệt virus', 0),
(81, N'Hệ điều hành', 0);

-- Câu 82
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(82, N'Tăng tốc độ truy vấn dữ liệu', 1),
(82, N'Giảm dung lượng', 0),
(82, N'Bảo mật dữ liệu', 0),
(82, N'Không có tác dụng', 0);

-- Câu 83
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(83, N'Phân chia dữ liệu thành nhiều phần trên nhiều server', 1),
(83, N'Sao lưu dữ liệu', 0),
(83, N'Mã hóa dữ liệu', 0),
(83, N'Xóa dữ liệu cũ', 0);

-- Câu 84
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(84, N'Tạo key duy nhất, kiểm tra toàn vẹn, phân phối dữ liệu', 1),
(84, N'Chỉ mã hóa mật khẩu', 0),
(84, N'Không dùng trong lưu trữ', 0),
(84, N'Chỉ nén dữ liệu', 0);

-- =============================================
-- TOPIC 4: Đạo đức, pháp luật (Thêm 7 câu: 85-91)
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(4, N'Phishing là gì?', 'easy', '10', 1, 'active'),
(4, N'Password mạnh cần có yếu tố nào?', 'easy', '10', 1, 'active'),
(4, N'Two-factor authentication là gì?', 'easy', '10', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(4, N'Open Source License có loại nào?', 'medium', '11', 1, 'active'),
(4, N'Data Privacy cần bảo vệ những thông tin gì?', 'medium', '11', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(4, N'Zero-day vulnerability là gì và nguy hiểm như thế nào?', 'hard', '12', 1, 'active'),
(4, N'Ransomware hoạt động theo cơ chế nào?', 'hard', '12', 1, 'active');

-- Answers cho Topic 4
-- Câu 85
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(85, N'Lừa đảo qua email/website giả mạo để lấy thông tin', 1),
(85, N'Câu cá', 0),
(85, N'Chơi game', 0),
(85, N'Mua sắm online', 0);

-- Câu 86
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(86, N'Chữ hoa, chữ thường, số, ký tự đặc biệt, độ dài >= 8', 1),
(86, N'Chỉ cần số', 0),
(86, N'Chỉ cần chữ', 0),
(86, N'Ngắn và đơn giản', 0);

-- Câu 87
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(87, N'Xác thực 2 bước: mật khẩu + mã OTP/SMS', 1),
(87, N'Dùng 2 mật khẩu', 0),
(87, N'Đăng nhập 2 lần', 0),
(87, N'Có 2 tài khoản', 0);

-- Câu 88
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(88, N'GPL, MIT, Apache, BSD', 1),
(88, N'Chỉ có 1 loại', 0),
(88, N'Không có phân loại', 0),
(88, N'Tất cả đều giống nhau', 0);

-- Câu 89
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(89, N'Tên, địa chỉ, số điện thoại, email, CMND, dữ liệu tài chính', 1),
(89, N'Chỉ tên', 0),
(89, N'Chỉ email', 0),
(89, N'Không cần bảo vệ', 0);

-- Câu 90
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(90, N'Lỗ hổng chưa được công bố, chưa có bản vá', 1),
(90, N'Lỗi thông thường', 0),
(90, N'Không nguy hiểm', 0),
(90, N'Chỉ ảnh hưởng nhỏ', 0);

-- Câu 91
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(91, N'Mã hóa dữ liệu và đòi tiền chuộc để giải mã', 1),
(91, N'Xóa dữ liệu ngay', 0),
(91, N'Chỉ làm chậm máy', 0),
(91, N'Không gây hại', 0);

-- =============================================
-- TOPIC 5: Ứng dụng tin học (Thêm 7 câu: 92-98)
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(5, N'Gmail là loại ứng dụng gì?', 'easy', '10', 1, 'active'),
(5, N'Google Drive dùng để làm gì?', 'easy', '10', 1, 'active'),
(5, N'Canva là phần mềm gì?', 'easy', '10', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(5, N'Slack và Discord thuộc nhóm ứng dụng gì?', 'medium', '11', 1, 'active'),
(5, N'Figma được sử dụng để làm gì?', 'medium', '11', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(5, N'Docker được sử dụng trong lĩnh vực nào?', 'hard', '12', 1, 'active'),
(5, N'Kubernetes có chức năng gì trong DevOps?', 'hard', '12', 1, 'active');

-- Answers cho Topic 5
-- Câu 92
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(92, N'Email', 1),
(92, N'Mạng xã hội', 0),
(92, N'Game', 0),
(92, N'Trình duyệt', 0);

-- Câu 93
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(93, N'Lưu trữ và chia sẻ file trực tuyến', 1),
(93, N'Chỉnh sửa ảnh', 0),
(93, N'Chơi game', 0),
(93, N'Gọi điện', 0);

-- Câu 94
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(94, N'Thiết kế đồ họa trực tuyến', 1),
(94, N'Soạn thảo văn bản', 0),
(94, N'Lập trình', 0),
(94, N'Quản lý dự án', 0);

-- Câu 95
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(95, N'Ứng dụng giao tiếp, chat nhóm', 1),
(95, N'Chỉnh sửa video', 0),
(95, N'Thiết kế website', 0),
(95, N'Lập trình game', 0);

-- Câu 96
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(96, N'Thiết kế UI/UX, prototype', 1),
(96, N'Soạn thảo văn bản', 0),
(96, N'Tạo bảng tính', 0),
(96, N'Chỉnh sửa video', 0);

-- Câu 97
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(97, N'Container hóa ứng dụng, triển khai phần mềm', 1),
(97, N'Chỉnh sửa ảnh', 0),
(97, N'Soạn thảo văn bản', 0),
(97, N'Chơi game', 0);

-- Câu 98
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(98, N'Quản lý và điều phối container', 1),
(98, N'Chỉ lưu trữ dữ liệu', 0),
(98, N'Thiết kế giao diện', 0),
(98, N'Chỉnh sửa video', 0);

-- =============================================
-- TOPIC 6: Giải quyết vấn đề (Thêm 7 câu: 99-105)
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(6, N'Biến (variable) trong lập trình là gì?', 'easy', '10', 1, 'active'),
(6, N'Hàm (function) có tác dụng gì?', 'easy', '10', 1, 'active'),
(6, N'Câu lệnh if-else dùng để làm gì?', 'easy', '10', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(6, N'Array và List khác nhau như thế nào?', 'medium', '11', 1, 'active'),
(6, N'Exception handling là gì?', 'medium', '11', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(6, N'Dynamic Programming khác với Greedy Algorithm như thế nào?', 'hard', '12', 1, 'active'),
(6, N'Graph traversal có những thuật toán nào?', 'hard', '12', 1, 'active');

-- Answers cho Topic 6
-- Câu 99
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(99, N'Vùng nhớ lưu trữ giá trị có thể thay đổi', 1),
(99, N'Hàm xử lý', 0),
(99, N'Thuật toán', 0),
(99, N'Câu lệnh điều kiện', 0);

-- Câu 100
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(100, N'Nhóm các câu lệnh có thể tái sử dụng', 1),
(100, N'Biến số', 0),
(100, N'Vòng lặp', 0),
(100, N'Kiểu dữ liệu', 0);

-- Câu 101
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(101, N'Kiểm tra điều kiện và thực thi code tương ứng', 1),
(101, N'Lặp lại code', 0),
(101, N'Khai báo biến', 0),
(101, N'Xuất dữ liệu', 0);

-- Câu 102
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(102, N'Array cố định kích thước, List linh hoạt', 1),
(102, N'Giống nhau hoàn toàn', 0),
(102, N'Array nhanh hơn mọi trường hợp', 0),
(102, N'Không có sự khác biệt', 0);

-- Câu 103
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(103, N'Xử lý lỗi runtime để tránh crash', 1),
(103, N'Tạo lỗi', 0),
(103, N'Tối ưu code', 0),
(103, N'Debug code', 0);

-- Câu 104
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(104, N'DP lưu kết quả con, Greedy chọn tối ưu cục bộ', 1),
(104, N'Giống nhau', 0),
(104, N'DP luôn nhanh hơn', 0),
(104, N'Greedy luôn chính xác hơn', 0);

-- Câu 105
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(105, N'BFS (Breadth-First Search), DFS (Depth-First Search)', 1),
(105, N'Chỉ có Binary Search', 0),
(105, N'Chỉ có Linear Search', 0),
(105, N'Không có thuật toán nào', 0);

-- =============================================
-- TOPIC 7: Hướng nghiệp (Thêm 7 câu: 106-112)
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(7, N'Frontend Developer làm việc với phần nào của website?', 'easy', '10', 1, 'active'),
(7, N'Backend Developer sử dụng ngôn ngữ nào?', 'easy', '10', 1, 'active'),
(7, N'QA/Tester làm công việc gì?', 'easy', '10', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(7, N'Full-stack Developer cần biết những gì?', 'medium', '11', 1, 'active'),
(7, N'Product Manager có vai trò gì?', 'medium', '11', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, GradeLevel, CreatedBy, Status)
VALUES 
(7, N'Site Reliability Engineer (SRE) khác DevOps như thế nào?', 'hard', '12', 1, 'active'),
(7, N'Blockchain Developer cần nắm vững công nghệ nào?', 'hard', '12', 1, 'active');

-- Answers cho Topic 7
-- Câu 106
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(106, N'Giao diện người dùng (HTML, CSS, JavaScript)', 1),
(106, N'Cơ sở dữ liệu', 0),
(106, N'Server', 0),
(106, N'Mạng máy tính', 0);

-- Câu 107
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(107, N'Python, Java, C#, Node.js, PHP', 1),
(107, N'Chỉ HTML, CSS', 0),
(107, N'Chỉ JavaScript', 0),
(107, N'Không cần ngôn ngữ lập trình', 0);

-- Câu 108
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(108, N'Kiểm thử phần mềm, tìm lỗi', 1),
(108, N'Viết code', 0),
(108, N'Thiết kế giao diện', 0),
(108, N'Quản lý dự án', 0);

-- Câu 109
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(109, N'Cả Frontend và Backend', 1),
(109, N'Chỉ Frontend', 0),
(109, N'Chỉ Backend', 0),
(109, N'Chỉ Database', 0);

-- Câu 110
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(110, N'Quản lý sản phẩm, định hướng phát triển, kết nối team', 1),
(110, N'Chỉ viết code', 0),
(110, N'Chỉ test phần mềm', 0),
(110, N'Chỉ thiết kế UI', 0);

-- Câu 111
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(111, N'SRE tập trung reliability, metrics, incident response', 1),
(111, N'Giống nhau hoàn toàn', 0),
(111, N'SRE chỉ làm support', 0),
(111, N'DevOps không quan trọng bằng SRE', 0);

-- Câu 112
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES
(112, N'Solidity, Smart Contract, Cryptography, Consensus', 1),
(112, N'Chỉ cần HTML, CSS', 0),
(112, N'Chỉ cần Java', 0),
(112, N'Không cần kiến thức gì', 0);

-- =============================================
-- Tóm tắt:
-- File question.sql (gốc): 63 câu (9 câu/topic)
-- File question_2.sql (bổ sung): 49 câu (7 câu/topic)
-- TỔNG: 112 câu (16 câu/topic)
-- 
-- Phân bổ mỗi topic:
-- - Easy: 6 câu (3 gốc + 3 bổ sung)
-- - Medium: 5 câu (3 gốc + 2 bổ sung)
-- - Hard: 5 câu (3 gốc + 2 bổ sung)
--
-- Đủ để tạo đề thi 100 câu:
-- 100 ÷ 7 topics ≈ 14.3 câu/topic
-- Với 40% Easy, 30% Medium, 30% Hard:
-- - Easy: 14.3 × 40% ≈ 6 câu ✅
-- - Medium: 14.3 × 30% ≈ 4 câu ✅
-- - Hard: 14.3 × 30% ≈ 4 câu ✅
-- =============================================
