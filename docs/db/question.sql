-- =============================================
-- Insert Questions và Answers
-- Mỗi topic có 9 câu hỏi: 3 easy, 3 medium, 3 hard
-- Mỗi câu hỏi có 4 đáp án (A, B, C, D)
-- created_by = 1 (giả sử user_id của giáo viên là 1)
-- =============================================

-- TOPIC 1: Máy tính và xã hội tri thức
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(1, N'Máy tính được sử dụng phổ biến nhất trong lĩnh vực nào sau đây?', 'easy', 1, 'active'),
(1, N'Thành phần nào sau đây là bộ phận cơ bản của máy tính?', 'easy', 1, 'active'),
(1, N'Phần mềm nào sau đây được dùng để soạn thảo văn bản?', 'easy', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(1, N'Trí tuệ nhân tạo (AI) đang được ứng dụng trong lĩnh vực nào?', 'medium', 1, 'active'),
(1, N'Công nghệ thông tin đã tác động đến giáo dục như thế nào?', 'medium', 1, 'active'),
(1, N'Big Data là gì?', 'medium', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(1, N'Cuộc cách mạng công nghiệp 4.0 có đặc điểm chính nào?', 'hard', 1, 'active'),
(1, N'Cloud Computing mang lại lợi ích gì cho doanh nghiệp?', 'hard', 1, 'active'),
(1, N'IoT (Internet of Things) hoạt động dựa trên nguyên tắc nào?', 'hard', 1, 'active');

-- Answers cho Topic 1 (9 câu x 4 đáp án = 36 đáp án)
-- Câu 1
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(1, N'Giáo dục và văn phòng', 1, 'A'),
(1, N'Nông nghiệp', 0, 'B'),
(1, N'Xây dựng', 0, 'C'),
(1, N'Du lịch', 0, 'D');

-- Câu 2
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(2, N'CPU, RAM, Ổ cứng', 1, 'A'),
(2, N'Bàn phím, chuột', 0, 'B'),
(2, N'Màn hình, loa', 0, 'C'),
(2, N'Máy in, scanner', 0, 'D');

-- Câu 3
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(3, N'Microsoft Word', 1, 'A'),
(3, N'Microsoft Excel', 0, 'B'),
(3, N'Adobe Photoshop', 0, 'C'),
(3, N'Google Chrome', 0, 'D');

-- Câu 4
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(4, N'Y tế, giáo dục, giao thông', 1, 'A'),
(4, N'Chỉ trong y tế', 0, 'B'),
(4, N'Chỉ trong giáo dục', 0, 'C'),
(4, N'Không ứng dụng được', 0, 'D');

-- Câu 5
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(5, N'Tạo ra học tập trực tuyến và tài nguyên số', 1, 'A'),
(5, N'Không có tác động gì', 0, 'B'),
(5, N'Làm giảm chất lượng giáo dục', 0, 'C'),
(5, N'Chỉ phục vụ giáo viên', 0, 'D');

-- Câu 6
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(6, N'Tập hợp dữ liệu cực lớn cần công nghệ đặc biệt để xử lý', 1, 'A'),
(6, N'Dữ liệu nhỏ', 0, 'B'),
(6, N'Dữ liệu bình thường', 0, 'C'),
(6, N'Dữ liệu văn bản', 0, 'D');

-- Câu 7
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(7, N'Kết hợp AI, IoT, tự động hóa', 1, 'A'),
(7, N'Chỉ sử dụng máy móc', 0, 'B'),
(7, N'Sản xuất thủ công', 0, 'C'),
(7, N'Không có đặc điểm gì', 0, 'D');

-- Câu 8
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(8, N'Tiết kiệm chi phí, mở rộng linh hoạt', 1, 'A'),
(8, N'Tốn kém hơn', 0, 'B'),
(8, N'Khó sử dụng', 0, 'C'),
(8, N'Không an toàn', 0, 'D');

-- Câu 9
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(9, N'Kết nối thiết bị qua Internet để thu thập và trao đổi dữ liệu', 1, 'A'),
(9, N'Chỉ kết nối máy tính', 0, 'B'),
(9, N'Không có kết nối', 0, 'C'),
(9, N'Chỉ dùng Bluetooth', 0, 'D');

-- =============================================
-- TOPIC 2: Mạng máy tính và Internet
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(2, N'WWW là viết tắt của cụm từ nào?', 'easy', 1, 'active'),
(2, N'Địa chỉ IP có tác dụng gì?', 'easy', 1, 'active'),
(2, N'Giao thức nào được sử dụng để gửi email?', 'easy', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(2, N'Phân biệt mạng LAN và WAN?', 'medium', 1, 'active'),
(2, N'Firewall có chức năng gì trong mạng?', 'medium', 1, 'active'),
(2, N'DNS Server có vai trò gì?', 'medium', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(2, N'VPN hoạt động theo nguyên tắc nào?', 'hard', 1, 'active'),
(2, N'Phân biệt IPv4 và IPv6?', 'hard', 1, 'active'),
(2, N'Mô hình OSI có bao nhiêu tầng và chức năng của từng tầng?', 'hard', 1, 'active');

-- Answers cho Topic 2
-- Câu 10
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(10, N'World Wide Web', 1, 'A'),
(10, N'World Wide Network', 0, 'B'),
(10, N'Web Wide World', 0, 'C'),
(10, N'Wide World Web', 0, 'D');

-- Câu 11
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(11, N'Xác định thiết bị trên mạng', 1, 'A'),
(11, N'Tăng tốc độ mạng', 0, 'B'),
(11, N'Lưu trữ dữ liệu', 0, 'C'),
(11, N'Bảo mật máy tính', 0, 'D');

-- Câu 12
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(12, N'SMTP', 1, 'A'),
(12, N'HTTP', 0, 'B'),
(12, N'FTP', 0, 'C'),
(12, N'DNS', 0, 'D');

-- Câu 13
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(13, N'LAN là mạng cục bộ, WAN là mạng diện rộng', 1, 'A'),
(13, N'LAN nhanh hơn WAN', 0, 'B'),
(13, N'WAN nhỏ hơn LAN', 0, 'C'),
(13, N'Không có sự khác biệt', 0, 'D');

-- Câu 14
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(14, N'Bảo vệ mạng khỏi truy cập trái phép', 1, 'A'),
(14, N'Tăng tốc độ mạng', 0, 'B'),
(14, N'Lưu trữ dữ liệu', 0, 'C'),
(14, N'Chia sẻ file', 0, 'D');

-- Câu 15
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(15, N'Chuyển đổi tên miền thành địa chỉ IP', 1, 'A'),
(15, N'Lưu trữ website', 0, 'B'),
(15, N'Gửi email', 0, 'C'),
(15, N'Bảo mật mạng', 0, 'D');

-- Câu 16
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(16, N'Tạo kết nối mạng riêng ảo qua Internet công cộng', 1, 'A'),
(16, N'Chỉ tăng tốc độ mạng', 0, 'B'),
(16, N'Lưu trữ dữ liệu', 0, 'C'),
(16, N'Chia sẻ file', 0, 'D');

-- Câu 17
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(17, N'IPv4 dùng 32 bit, IPv6 dùng 128 bit', 1, 'A'),
(17, N'IPv4 nhanh hơn IPv6', 0, 'B'),
(17, N'Không có sự khác biệt', 0, 'C'),
(17, N'IPv6 cũ hơn IPv4', 0, 'D');

-- Câu 18
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(18, N'7 tầng: Physical, Data Link, Network, Transport, Session, Presentation, Application', 1, 'A'),
(18, N'5 tầng', 0, 'B'),
(18, N'3 tầng', 0, 'C'),
(18, N'10 tầng', 0, 'D');

-- =============================================
-- TOPIC 3: Tổ chức lưu trữ, tìm kiếm và trao đổi thông tin
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(3, N'Đơn vị nhỏ nhất để lưu trữ thông tin trong máy tính là gì?', 'easy', 1, 'active'),
(3, N'1 GB bằng bao nhiêu MB?', 'easy', 1, 'active'),
(3, N'File có đuôi .docx là loại file gì?', 'easy', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(3, N'Cloud storage có ưu điểm gì so với lưu trữ cục bộ?', 'medium', 1, 'active'),
(3, N'Cơ sở dữ liệu quan hệ (RDBMS) là gì?', 'medium', 1, 'active'),
(3, N'Metadata là gì?', 'medium', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(3, N'Phân biệt SQL và NoSQL?', 'hard', 1, 'active'),
(3, N'Thuật toán tìm kiếm Binary Search hoạt động như thế nào?', 'hard', 1, 'active'),
(3, N'Blockchain lưu trữ dữ liệu theo cơ chế nào?', 'hard', 1, 'active');

-- Answers cho Topic 3
-- Câu 19
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(19, N'Bit', 1, 'A'),
(19, N'Byte', 0, 'B'),
(19, N'KB', 0, 'C'),
(19, N'MB', 0, 'D');

-- Câu 20
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(20, N'1024 MB', 1, 'A'),
(20, N'1000 MB', 0, 'B'),
(20, N'512 MB', 0, 'C'),
(20, N'2048 MB', 0, 'D');

-- Câu 21
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(21, N'File Word', 1, 'A'),
(21, N'File Excel', 0, 'B'),
(21, N'File PDF', 0, 'C'),
(21, N'File ảnh', 0, 'D');

-- Câu 22
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(22, N'Truy cập mọi lúc mọi nơi, dễ chia sẻ', 1, 'A'),
(22, N'Tốn kém hơn', 0, 'B'),
(22, N'Chậm hơn', 0, 'C'),
(22, N'Không an toàn', 0, 'D');

-- Câu 23
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(23, N'Hệ quản trị CSDL lưu trữ dữ liệu dạng bảng có mối quan hệ', 1, 'A'),
(23, N'Phần mềm soạn thảo', 0, 'B'),
(23, N'Hệ điều hành', 0, 'C'),
(23, N'Ngôn ngữ lập trình', 0, 'D');

-- Câu 24
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(24, N'Dữ liệu về dữ liệu, mô tả thông tin của dữ liệu', 1, 'A'),
(24, N'Dữ liệu thô', 0, 'B'),
(24, N'Dữ liệu lớn', 0, 'C'),
(24, N'Dữ liệu bảo mật', 0, 'D');

-- Câu 25
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(25, N'SQL dùng bảng quan hệ, NoSQL linh hoạt hơn (document, key-value)', 1, 'A'),
(25, N'SQL chậm hơn NoSQL', 0, 'B'),
(25, N'Không có sự khác biệt', 0, 'C'),
(25, N'NoSQL không lưu được dữ liệu', 0, 'D');

-- Câu 26
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(26, N'Chia đôi tập dữ liệu đã sắp xếp liên tục', 1, 'A'),
(26, N'Duyệt tuần tự', 0, 'B'),
(26, N'Tìm ngẫu nhiên', 0, 'C'),
(26, N'Sắp xếp rồi tìm', 0, 'D');

-- Câu 27
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(27, N'Chuỗi khối liên kết, mã hóa, phân tán', 1, 'A'),
(27, N'Lưu tập trung', 0, 'B'),
(27, N'Không mã hóa', 0, 'C'),
(27, N'Lưu trên 1 máy chủ', 0, 'D');

-- =============================================
-- TOPIC 4: Đạo đức, pháp luật và văn hóa trong môi trường số
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(4, N'Hành vi nào sau đây là vi phạm bản quyền phần mềm?', 'easy', 1, 'active'),
(4, N'Cyberbullying là gì?', 'easy', 1, 'active'),
(4, N'Nên làm gì khi nhận được email lạ yêu cầu cung cấp mật khẩu?', 'easy', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(4, N'GDPR (General Data Protection Regulation) là gì?', 'medium', 1, 'active'),
(4, N'Fake news gây hại như thế nào?', 'medium', 1, 'active'),
(4, N'Quyền riêng tư trực tuyến cần được bảo vệ như thế nào?', 'medium', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(4, N'Luật An ninh mạng Việt Nam quy định về trách nhiệm của doanh nghiệp công nghệ?', 'hard', 1, 'active'),
(4, N'Digital footprint là gì và tại sao cần quan tâm?', 'hard', 1, 'active'),
(4, N'Creative Commons license có các loại nào?', 'hard', 1, 'active');

-- Answers cho Topic 4
-- Câu 28
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(28, N'Sử dụng phần mềm crack không bản quyền', 1, 'A'),
(28, N'Mua phần mềm chính hãng', 0, 'B'),
(28, N'Dùng phần mềm miễn phí', 0, 'C'),
(28, N'Dùng phần mềm nguồn mở', 0, 'D');

-- Câu 29
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(29, N'Bắt nạt, quấy rối trên mạng', 1, 'A'),
(29, N'Chơi game online', 0, 'B'),
(29, N'Học trực tuyến', 0, 'C'),
(29, N'Mua sắm online', 0, 'D');

-- Câu 30
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(30, N'Xóa và không cung cấp thông tin', 1, 'A'),
(30, N'Cung cấp mật khẩu ngay', 0, 'B'),
(30, N'Click vào link trong email', 0, 'C'),
(30, N'Trả lời email', 0, 'D');

-- Câu 31
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(31, N'Quy định bảo vệ dữ liệu cá nhân của EU', 1, 'A'),
(31, N'Phần mềm diệt virus', 0, 'B'),
(31, N'Mạng xã hội', 0, 'C'),
(31, N'Ngôn ngữ lập trình', 0, 'D');

-- Câu 32
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(32, N'Gây hoang mang, ảnh hưởng dư luận, quyết định sai', 1, 'A'),
(32, N'Không gây hại gì', 0, 'B'),
(32, N'Chỉ giải trí', 0, 'C'),
(32, N'Tăng kiến thức', 0, 'D');

-- Câu 33
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(33, N'Không chia sẻ thông tin cá nhân, dùng mật khẩu mạnh, xác thực 2 yếu tố', 1, 'A'),
(33, N'Chia sẻ mọi thứ công khai', 0, 'B'),
(33, N'Dùng mật khẩu đơn giản', 0, 'C'),
(33, N'Không cần bảo vệ', 0, 'D');

-- Câu 34
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(34, N'Bảo vệ dữ liệu người dùng, ngăn chặn thông tin xấu', 1, 'A'),
(34, N'Không có trách nhiệm', 0, 'B'),
(34, N'Chỉ kiếm lợi nhuận', 0, 'C'),
(34, N'Chia sẻ dữ liệu tự do', 0, 'D');

-- Câu 35
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(35, N'Dấu vết số để lại khi hoạt động online, ảnh hưởng danh tiếng', 1, 'A'),
(35, N'Vết chân trên cát', 0, 'B'),
(35, N'Không quan trọng', 0, 'C'),
(35, N'Chỉ là dữ liệu tạm', 0, 'D');

-- Câu 36
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(36, N'BY, SA, NC, ND và kết hợp', 1, 'A'),
(36, N'Chỉ có 1 loại', 0, 'B'),
(36, N'Không có phân loại', 0, 'C'),
(36, N'Tất cả đều giống nhau', 0, 'D');

-- =============================================
-- TOPIC 5: Ứng dụng tin học
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(5, N'Phần mềm nào dùng để tạo bảng tính?', 'easy', 1, 'active'),
(5, N'PowerPoint dùng để làm gì?', 'easy', 1, 'active'),
(5, N'Photoshop là phần mềm gì?', 'easy', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(5, N'Google Workspace bao gồm những ứng dụng nào?', 'medium', 1, 'active'),
(5, N'Trello được sử dụng để làm gì?', 'medium', 1, 'active'),
(5, N'Zoom, Microsoft Teams thuộc nhóm ứng dụng nào?', 'medium', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(5, N'AutoCAD được ứng dụng trong lĩnh vực nào?', 'hard', 1, 'active'),
(5, N'Phần mềm ERP (Enterprise Resource Planning) có chức năng gì?', 'hard', 1, 'active'),
(5, N'Machine Learning được ứng dụng như thế nào trong y tế?', 'hard', 1, 'active');

-- Answers cho Topic 5
-- Câu 37
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(37, N'Microsoft Excel', 1, 'A'),
(37, N'Microsoft Word', 0, 'B'),
(37, N'Microsoft PowerPoint', 0, 'C'),
(37, N'Microsoft Access', 0, 'D');

-- Câu 38
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(38, N'Tạo bài thuyết trình', 1, 'A'),
(38, N'Soạn thảo văn bản', 0, 'B'),
(38, N'Tạo bảng tính', 0, 'C'),
(38, N'Chỉnh sửa ảnh', 0, 'D');

-- Câu 39
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(39, N'Phần mềm chỉnh sửa ảnh', 1, 'A'),
(39, N'Phần mềm soạn thảo', 0, 'B'),
(39, N'Phần mềm bảng tính', 0, 'C'),
(39, N'Phần mềm lập trình', 0, 'D');

-- Câu 40
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(40, N'Gmail, Drive, Docs, Sheets, Meet', 1, 'A'),
(40, N'Chỉ có Gmail', 0, 'B'),
(40, N'Chỉ có Drive', 0, 'C'),
(40, N'Không có ứng dụng nào', 0, 'D');

-- Câu 41
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(41, N'Quản lý dự án và công việc', 1, 'A'),
(41, N'Chỉnh sửa ảnh', 0, 'B'),
(41, N'Soạn thảo văn bản', 0, 'C'),
(41, N'Chơi game', 0, 'D');

-- Câu 42
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(42, N'Họp trực tuyến, video conference', 1, 'A'),
(42, N'Chỉnh sửa video', 0, 'B'),
(42, N'Thiết kế đồ họa', 0, 'C'),
(42, N'Lập trình', 0, 'D');

-- Câu 43
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(43, N'Thiết kế kiến trúc, xây dựng, cơ khí', 1, 'A'),
(43, N'Chỉnh sửa ảnh', 0, 'B'),
(43, N'Soạn thảo văn bản', 0, 'C'),
(43, N'Lập trình web', 0, 'D');

-- Câu 44
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(44, N'Quản lý tổng thể tài nguyên doanh nghiệp', 1, 'A'),
(44, N'Chỉ quản lý nhân sự', 0, 'B'),
(44, N'Chỉ quản lý tài chính', 0, 'C'),
(44, N'Không có chức năng gì', 0, 'D');

-- Câu 45
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(45, N'Chẩn đoán bệnh, dự đoán, phân tích hình ảnh y khoa', 1, 'A'),
(45, N'Chỉ lưu trữ dữ liệu', 0, 'B'),
(45, N'Không ứng dụng được', 0, 'C'),
(45, N'Chỉ in hồ sơ bệnh án', 0, 'D');

-- =============================================
-- TOPIC 6: Giải quyết vấn đề với sự trợ giúp của máy tính
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(6, N'Thuật toán là gì?', 'easy', 1, 'active'),
(6, N'Lưu đồ (flowchart) dùng để làm gì?', 'easy', 1, 'active'),
(6, N'Ngôn ngữ lập trình nào phổ biến cho người mới học?', 'easy', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(6, N'Tư duy thuật toán (Computational Thinking) bao gồm những yếu tố nào?', 'medium', 1, 'active'),
(6, N'Debugging là gì?', 'medium', 1, 'active'),
(6, N'Vòng lặp (loop) được sử dụng khi nào?', 'medium', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(6, N'Phân biệt thuật toán đệ quy và vòng lặp?', 'hard', 1, 'active'),
(6, N'Big O notation dùng để đánh giá điều gì?', 'hard', 1, 'active'),
(6, N'Thuật toán sắp xếp nào có độ phức tạp O(n log n)?', 'hard', 1, 'active');

-- Answers cho Topic 6
-- Câu 46
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(46, N'Dãy các bước để giải quyết vấn đề', 1, 'A'),
(46, N'Ngôn ngữ lập trình', 0, 'B'),
(46, N'Phần mềm máy tính', 0, 'C'),
(46, N'Hệ điều hành', 0, 'D');

-- Câu 47
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(47, N'Biểu diễn thuật toán dưới dạng hình ảnh', 1, 'A'),
(47, N'Vẽ tranh', 0, 'B'),
(47, N'Thiết kế website', 0, 'C'),
(47, N'Tạo video', 0, 'D');

-- Câu 48
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(48, N'Python', 1, 'A'),
(48, N'Assembly', 0, 'B'),
(48, N'Machine code', 0, 'C'),
(48, N'Binary', 0, 'D');

-- Câu 49
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(49, N'Phân tích, trừu tượng hóa, nhận dạng mẫu, thiết kế thuật toán', 1, 'A'),
(49, N'Chỉ có lập trình', 0, 'B'),
(49, N'Chỉ có sử dụng máy tính', 0, 'C'),
(49, N'Không có yếu tố nào', 0, 'D');

-- Câu 50
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(50, N'Tìm và sửa lỗi trong chương trình', 1, 'A'),
(50, N'Viết code mới', 0, 'B'),
(50, N'Thiết kế giao diện', 0, 'C'),
(50, N'Test phần mềm', 0, 'D');

-- Câu 51
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(51, N'Khi cần lặp lại một tác vụ nhiều lần', 1, 'A'),
(51, N'Chỉ khi viết hàm', 0, 'B'),
(51, N'Khi khai báo biến', 0, 'C'),
(51, N'Không bao giờ dùng', 0, 'D');

-- Câu 52
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(52, N'Đệ quy gọi chính nó, vòng lặp lặp lại khối lệnh', 1, 'A'),
(52, N'Không có sự khác biệt', 0, 'B'),
(52, N'Đệ quy nhanh hơn', 0, 'C'),
(52, N'Vòng lặp phức tạp hơn', 0, 'D');

-- Câu 53
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(53, N'Độ phức tạp thời gian của thuật toán', 1, 'A'),
(53, N'Kích thước file', 0, 'B'),
(53, N'Số dòng code', 0, 'C'),
(53, N'Ngôn ngữ lập trình', 0, 'D');

-- Câu 54
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(54, N'Quick Sort, Merge Sort', 1, 'A'),
(54, N'Bubble Sort', 0, 'B'),
(54, N'Selection Sort', 0, 'C'),
(54, N'Insertion Sort', 0, 'D');

-- =============================================
-- TOPIC 7: Hướng nghiệp với tin học
-- =============================================
-- Easy Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(7, N'Nghề lập trình viên cần kỹ năng nào?', 'easy', 1, 'active'),
(7, N'Web Developer làm công việc gì?', 'easy', 1, 'active'),
(7, N'IT Support là gì?', 'easy', 1, 'active');

-- Medium Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(7, N'Data Scientist cần có kiến thức gì?', 'medium', 1, 'active'),
(7, N'DevOps Engineer làm gì?', 'medium', 1, 'active'),
(7, N'UI/UX Designer khác nhau như thế nào?', 'medium', 1, 'active');

-- Hard Questions
INSERT INTO Questions (TopicId, QuestionText, DifficultyLevel, CreatedBy, Status)
VALUES 
(7, N'Cloud Architect cần có kỹ năng và chứng chỉ nào?', 'hard', 1, 'active'),
(7, N'Cybersecurity Specialist bảo vệ hệ thống như thế nào?', 'hard', 1, 'active'),
(7, N'AI/ML Engineer cần nắm vững kiến thức gì?', 'hard', 1, 'active');

-- Answers cho Topic 7
-- Câu 55
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(55, N'Ngôn ngữ lập trình, tư duy logic, giải quyết vấn đề', 1, 'A'),
(55, N'Chỉ cần biết Word, Excel', 0, 'B'),
(55, N'Không cần kỹ năng gì', 0, 'C'),
(55, N'Chỉ cần học cấp 3', 0, 'D');

-- Câu 56
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(56, N'Phát triển website, ứng dụng web', 1, 'A'),
(56, N'Sửa máy tính', 0, 'B'),
(56, N'Bán hàng online', 0, 'C'),
(56, N'Thiết kế đồ họa', 0, 'D');

-- Câu 57
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(57, N'Hỗ trợ kỹ thuật, khắc phục sự cố CNTT', 1, 'A'),
(57, N'Lập trình phần mềm', 0, 'B'),
(57, N'Thiết kế website', 0, 'C'),
(57, N'Quản lý dự án', 0, 'D');

-- Câu 58
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(58, N'Thống kê, toán học, lập trình, ML', 1, 'A'),
(58, N'Chỉ cần biết Excel', 0, 'B'),
(58, N'Không cần kiến thức gì', 0, 'C'),
(58, N'Chỉ cần biết vẽ đồ thị', 0, 'D');

-- Câu 59
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(59, N'Tự động hóa triển khai, vận hành hệ thống', 1, 'A'),
(59, N'Chỉ viết code', 0, 'B'),
(59, N'Chỉ test phần mềm', 0, 'C'),
(59, N'Thiết kế giao diện', 0, 'D');

-- Câu 60
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(60, N'UI thiết kế giao diện, UX thiết kế trải nghiệm người dùng', 1, 'A'),
(60, N'Giống nhau hoàn toàn', 0, 'B'),
(60, N'UI khó hơn UX', 0, 'C'),
(60, N'UX không quan trọng', 0, 'D');

-- Câu 61
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(61, N'AWS, Azure, GCP; chứng chỉ Solutions Architect', 1, 'A'),
(61, N'Chỉ cần biết Linux', 0, 'B'),
(61, N'Không cần chứng chỉ', 0, 'C'),
(61, N'Chỉ cần biết lập trình', 0, 'D');

-- Câu 62
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(62, N'Phát hiện lỗ hổng, thiết lập firewall, mã hóa, giám sát', 1, 'A'),
(62, N'Chỉ cài antivirus', 0, 'B'),
(62, N'Không làm gì', 0, 'C'),
(62, N'Chỉ backup dữ liệu', 0, 'D');

-- Câu 63
INSERT INTO Answers (QuestionId, AnswerText, IsCorrect, AnswerOrder) VALUES
(63, N'Toán học, thống kê, thuật toán ML, deep learning, Python/R', 1, 'A'),
(63, N'Chỉ cần biết Excel', 0, 'B'),
(63, N'Không cần kiến thức gì', 0, 'C'),
(63, N'Chỉ cần học cấp 3', 0, 'D');