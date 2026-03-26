# HỆ THỐNG NHẮN TIN (CHAT) CHO ỨNG DỤNG XEM BÀI TAROT
**Phiên bản thiết kế Production – Đã tối ưu cho Mobile & Desktop**

**Mục tiêu:**  
Xây dựng hệ thống chat công bằng, tự động hóa cao, giảm tranh chấp, trải nghiệm giống Instagram nhưng phù hợp cả mobile và desktop.

---

## 1. Danh sách Tarot Reader & Bắt đầu chat

- Ứng dụng hiển thị danh sách Tarot Reader với các chức năng:
  - Filter, search, sắp xếp theo: giá/câu hỏi, rating trung bình, số lượt chat hoàn thành, số lượt đánh giá, tình trạng Online/Offline.
- Mỗi Reader **chỉ được thiết lập 1 giá cố định mỗi câu hỏi** (ví dụ: 500 kim cương). Giá này Reader tự thiết lập khi tạo profile và chỉ áp dụng cho các chat **mới**.
- Khi User click vào Reader:
  - Mở phòng chat mới (nếu chưa tồn tại) hoặc tiếp tục phòng chat cũ nếu đang ở trạng thái “Ongoing”.
- **Chỉ khi User gửi tin nhắn đầu tiên** mới kích hoạt thanh toán:
  - Hệ thống kiểm tra ví User có đủ kim cương không. Không đủ → chặn gửi tin nhắn và thông báo nạp thêm.
  - Tự động **trừ và đóng băng** đúng số tiền giá của Reader.
  - Hiển thị thông báo rõ ràng cho User: “Đã đóng băng X kim cương cho cuộc chat này”.
  - Cả User và Reader đều thấy dòng “Đang đóng băng: X kim cương” ở header phòng chat.

**Edge cases:**
- User click Reader nhưng chưa gửi tin nhắn → phòng chat ở trạng thái “Chưa kích hoạt”, không trừ tiền.
- User đang có > 5 chat Ongoing → cảnh báo “Bạn đang chat quá nhiều, vui lòng hoàn thành một số chat trước”.
- Reader bị ban hoặc xóa tài khoản → tự động hoàn tiền toàn bộ và chuyển chat sang trạng thái “Reader không khả dụng”.

---

## 2. Quy tắc thanh toán & giải ngân

Tất cả khoản tiền (tiền ban đầu + tiền cộng thêm) đều tuân theo cùng một quy tắc.  
Hệ thống sử dụng **cron job chạy lúc 00:00 UTC hàng ngày** để xử lý tự động.

**Trường hợp Reader chưa reply sau 48 giờ** (tính từ tin nhắn đầu tiên của User):
- Hệ thống tự động **hoàn 100%** kim cương về ví User lúc **00:00 UTC ngày thứ 3**.

**Trường hợp Reader đã reply** (có tương tác):
- Cả hai bên đều có nút **“Hoàn thành cuộc trò chuyện”**.
- Khi một bên bấm nút → hệ thống gửi push notification + request xác nhận cho bên kia.

**Kết quả cuối cùng:**
- Cả hai bên đồng ý → **giải ngân ngay lập tức** cho Reader.
- User bấm hoàn thành, Reader chưa phản hồi → tiền chuyển cho Reader lúc **00:00 UTC ngày hôm sau**.
- Reader bấm hoàn thành:
  - User **đồng ý** → giải ngân ngay.
  - User **từ chối** → tiền giữ nguyên đóng băng. User có nút “Tố cáo / Không đồng ý kết thúc”. Admin review chat trong 7 ngày. Nếu User không tố cáo → tự động giải ngân cho Reader lúc **00:00 UTC ngày thứ 7**.
  - User **không bấm gì** → tự động giải ngân cho Reader lúc **00:00 UTC ngày thứ 3**.

**Edge cases:**
- Reader reply sau đúng 48 giờ 1 phút → vẫn tính là “có tương tác”.
- Chat có nhiều lần “Cộng thêm tiền” → tất cả gộp chung vào 1 khoản đóng băng duy nhất.
- User tố cáo trong thời gian chờ → chat bị khóa tạm thời, admin xử lý thủ công.

---

## 3. Giao diện Chat (Responsive – Desktop & Mobile)

**Trên Desktop / Tablet (lớn hơn 768px):**
- Màn hình chính chia **2 cột** giống Instagram DM.
  - Bên trái: Danh sách các cuộc trò chuyện.
    - Tab 1: **Đang chat (Ongoing)**
    - Tab 2: **Đã hoàn thành (Completed)**
  - Bên phải: Nội dung chat chi tiết của cuộc trò chuyện đang chọn.

**Trên Mobile (≤ 768px):**
- Chỉ hiển thị **1 bảng danh sách các cuộc hội thoại** (full screen).
- Mỗi item trong danh sách hiển thị: tên Reader, tin nhắn cuối cùng, thời gian, số kim cương đang đóng băng (nếu có), trạng thái (Ongoing/Completed).
- Khi User **nhấn vào một cuộc hội thoại** → chuyển sang màn hình chat đầy đủ (full screen).
- Có nút **Back** (mũi tên trái) ở góc trên bên trái để quay về danh sách cuộc hội thoại.
- Tab **Ongoing** và **Completed** vẫn nằm ở đầu danh sách (segmented control hoặc 2 tab).

**Tính năng chung trong phòng chat (cả mobile & desktop):**
- Hỗ trợ gửi: Text, Hình ảnh (tối đa 10MB, tự động nén), Emoji, Sticker.
- Header phòng chat luôn hiển thị: “Đang đóng băng: X kim cương” + nút xem lịch sử giao dịch của chat.
- Hiển thị trạng thái tin nhắn: “Đã gửi”, “Đã nhận”, “Reader đang gõ…”.
- Hỗ trợ load thêm tin nhắn khi kéo lên (infinite scroll).

**Edge cases giao diện:**
- Chat quá dài (> 500 tin nhắn) → tự động load.
- Một bên xóa tin nhắn → chỉ xóa bên mình (server vẫn lưu để admin review).

---

## 4. Tính năng “Cộng thêm tiền” (Add Money)

- **Chỉ Tarot Reader** có nút “Yêu cầu cộng thêm tiền” trong phòng chat.
- Reader nhập số tiền + lý do ngắn gọn (bắt buộc, tối đa 100 ký tự).
- User nhận push notification và trong chat hiện 2 nút: **Đồng ý** / **Từ chối**.
- Đồng ý → trừ ví User và **cộng trực tiếp vào khoản đóng băng** của chat hiện tại.
- Từ chối → request bị hủy. Reader không thể gửi request mới trong **24 giờ** (chống spam).

**Edge cases:**
- Reader gửi request khi User đã bấm “Hoàn thành” → hệ thống tự động từ chối.
- User đồng ý nhưng ví không đủ → request vẫn pending cho đến khi User nạp.
- Chỉ cho phép **1 request add money đang pending** tại một thời điểm.

---

## 5. Kết thúc cuộc trò chuyện & Đánh giá + Báo cáo

- Khi chat chuyển sang trạng thái **Completed**:
  - Nếu **cả hai đã bấm hoàn thành** → hiện ngay popup đánh giá cho User.
  - Nếu **tự động hoàn thành** (timeout) → khi User mở lại chat Completed sẽ có nút “Đánh giá ngay”.
- User đánh giá **1–5 sao** + bình luận (không bắt buộc).
- Rating trung bình của Reader được cập nhật **ngay lập tức** và ảnh hưởng đến thứ hạng danh sách Reader.
- Đánh giá chỉ hiển thị công khai sau khi Reader cũng hoàn thành hoặc sau 48 giờ.

**Nút Báo cáo / Tố cáo:**
- Cả hai bên đều có nút “Báo cáo” trong mọi lúc.
- Khi tố cáo → chat bị khóa tạm thời, admin nhận thông báo + full lịch sử chat để xử lý trong tối đa 48 giờ.

**Edge cases:**
- Reader cố tình không bấm hoàn thành → User có thể tố cáo sau 7 ngày.
- User đánh giá 1 sao + bình luận tiêu cực → Reader nhận thông báo và có thể phản hồi 1 lần (không xóa được đánh giá).

---

## 6. Yêu cầu chung cho Production

- Toàn bộ lịch sử chat, transaction, request add money, nút hoàn thành… đều được lưu log vĩnh viễn (không cho phép xóa).
- Currency: Sử dụng **kim cương** (virtual currency).
- Push notification cho tất cả sự kiện quan trọng.
- Admin dashboard có bộ lọc tranh chấp, manual refund, freeze tài khoản Reader nếu > 3 tranh chấp trong 30 ngày.
- Tất cả thời gian đều tính theo **UTC** và ghi log rõ ràng.

**Trạng thái phòng chat:**
- `Pending` (chưa gửi tin nhắn đầu)
- `Ongoing`
- `Completed`
- `Disputed` (đang có tố cáo)

---

**File này đã bao gồm toàn bộ chi tiết cần thiết để đưa cho AI code (Backend + Frontend).**  
Bạn có thể copy trực tiếp vào Cursor, Claude, GPT… để bắt đầu generate code.

Nếu bạn muốn thêm phần Database Schema, API Endpoints, hoặc Flowchart, cứ nói mình sẽ bổ sung ngay vào file .md này!