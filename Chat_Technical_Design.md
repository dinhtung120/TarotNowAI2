# THIẾT KẾ KỸ THUẬT: HỆ THỐNG NHẮN TIN (CHAT) MỞ RỘNG
Dự án: TarotNowAI

Tài liệu này trình bày chi tiết về các quyết định kiến trúc, công nghệ và thư viện được sử dụng để xây dựng tính năng Chat v2.0 cho nền tảng TarotNow, đảm bảo hiệu năng cao, tối ưu chi phí lưu trữ và tuân thủ chặt chẽ Clean Architecture như bạn yêu cầu.

## 1. Tổng Quan Kiến Trúc (Architecture Overview)

Hệ thống Chat được thiết kế phân tán kết hợp giữa xử lý đồng bộ tuyến tính (API) và phi đồng bộ thời gian thực (WebSockets), chia làm các lớp như sau:

- **Frontend (FE):** Next.js (App Router), React 19, Tailwind CSS v4.
  - **Quản lý State:** `Zustand` (UI state nội bộ), TanStack Query (Server state quản lý danh sách chat, load more).
  - **Kết nối Realtime:** Gói `@microsoft/signalr` cho WebSocket Client gửi/nhận dòng sự kiện từ BE.
  - **Xử lý Form:** `react-hook-form` + `zod` để validate input như yêu cầu cộng tiền, lý do từ chối.
- **Backend (BE):** .NET 9.0, Clean Architecture.
  - **Mẫu thiết kế:** CQRS (Commands cho sự kiện nhắn tin, thanh toán; Queries để đọc lịch sử) kết nối qua `MediatR`, input được kiểm soát bởi `FluentValidation`.
  - **Kết nối Realtime:** SignalR Core Hub (`ChatHub`).
- **Database (Cơ sở dữ liệu):**
  - **PostgreSQL (với EF Core layer):** Lưu trữ các thực thể mang tính giao dịch nghiêm ngặt: Phòng Chat (Trạng thái Pending, Ongoing...), Ví Kim Cương 💎, Escrow. Đảm bảo tính ACID, chống double-spend mạnh mẽ.
  - **MongoDB (MongoDB.Driver):** Lưu trữ lượng lớn dữ liệu phi cấu trúc như Payload Tin nhắn (Text messages, URL ảnh/voice), System messages, log xử lý nội dung. Schema-less phù hợp cho luồng chat tốc độ cao.
  - **Redis (StackExchange.Redis):** Chịu trách nhiệm Caching, Rate Limiting (chặn user spam chat), Distributed Lock (khoá tài khoản khi giải quyết tiền/đóng băng), lưu trữ heartbeat cập nhật Online/Offline của Users.

---

## 2. Công Nghệ Xử Lý Đa Phương Tiện (Media Processing)

Chiến lược nén đa tầng (Client-side trước, Server-side đánh giá sau) nhằm tối ưu băng thông đường truyền và dung lượng Cloud Storage nhưng vẫn duy trì UX xuất sắc.

### 2.1. Nén Tin Nhắn Thoại (Voice Messages)
- **Frontend (Client-side):**
  - **Thư viện:** `opus-media-recorder` (hoặc AudioContext API).
  - **Logic:** Ngay khi User giữ nút thu âm, tín hiệu được chuyển đổi và nén trực tiếp sang định dạng **Opus** siêu tối ưu cho giọng thoại thay vì Wav hay MP3. Định dạng này mang lại tiếng nói trong trẻo ở bitrate rất thấp. File tải lên (Upload stream) siêu nhỏ, giúp UX gửi tin nhanh chóng dù mạng chập chờn.
- **Backend (Server-side):**
  - **Công cụ:** `FFmpegCore` (C# Wrapper của FFmpeg).
  - **Logic:** Server kiểm tra metadata của file nhận được. Do hệ thống web chặn khó lường từ client, nếu file bị lỗi format, dung lượng quá lớn hoặc không đúng audio/opus, BE sẽ kích hoạt process FFmpeg để convert lại định dạng chuẩn Opus (thường cấu hình 16kbps, Mono codec) đồng nhất hóa trước khi lưu trữ vĩnh viễn trên S3/Cloud Storage.

### 2.2. Nén Hình Ảnh (Images)
- **Frontend (Client-side):**
  - **Thư viện:** `browser-image-compression`.
  - **Logic:** Khi User chọn gửi ảnh, trước khi gọi API, FE chạy hàm nén convert sang định dạng **WebP** mức `quality = 0.85` (giảm độ phân giải quá cao, giữ nguyên chất lượng nhìn được). Thao tác này tiết kiệm ~60% thời gian upload của User.
- **Backend (Server-side):**
  - **Thư viện:** Sử dụng `SixLabors.ImageSharp` tích hợp thêm module encoder của `neoSlove.imageSharp.AVIF`.
  - **Logic:** BE nhận ảnh WebP, thực hiện gỡ bỏ siêu dữ liệu định vị (EXIF metadata - bảo mật cho User) và encode nén chuyên sâu thành định dạng **AVIF** với cờ chất lượng `70%`.
  - **Cơ sở kỹ thuật:** AVIF mang lại bước nhảy vọt về thuật toán rèn hình ảnh cao hơn đáng kể WebP, tiết kiệm ~30-50% không gian lưu trữ Backend. Băng thông truyền trả lại (cho lần load tin nhắn cũ) cũng rẻ hơn rõ rệt.

---

## 3. Kiến Trúc Của Cơ Chế Realtime & Quản Lý Trạng Thái

### 3.1. WebSockets với SignalR
- API Layer có một `ChatHub`. Mọi Client khi muốn chat gọi connect tới Endpoint này.
- **Quản lý phân khu (Grouping):** Thay vì gởi toàn mảng người dùng, Hub ghép 2 người chat vào một `SignalR Group` mang tên `ChatRoomId`. Mọi sự kiện như "Gửi tin mới", "Seen (Read Receipt)", "Typing Indicator" (Đang nhập văn bản) gọi tới Group này nên đảm bảo tính cô lập và bảo mật cao.

### 3.2. Lifecycle Online Status (Online/Away/Offline)
- User cập nhật status chủ động ("Away") thông qua cập nhật DB PostgreSQL nhưng realtime do Redis đảm nhận.
- Khi kết nối socket thành công, backend lưu key vào Redis: `user:status:{userId}` TTL set liên tục qua cơ chế Heartbeat check mỗi phút của SignalR Client.
- **Nhiệm vụ dọn vắng mặt (Cronjob/Worker):** Cấu hình Background task của Host .NET: Nếu sau 1 tiếng key trên Redis bị hết hạn (do user thoát app ko gửi lệnh disconnect tử tế), worker sẽ đổi status DB thành `Offline` để frontend hiển thị đúng cờ trạng thái "⚠️ Reader đang không hoạt động".

---

## 4. Giải Quyết Thanh Toán Escrow & Background Workers

- **Locking cho Giao dịch (Concurrency):**
  - Khi User chat câu đầu -> Trigger thanh toán AwaitingAcceptance -> BE dùng Distributed Lock bằng Redis để khoá cụm thao tác (Check ví -> Trừ -> Thêm Escrow) theo logic ACID Transaction từ PostgreSQL chạy ngầm EF Core. Đảm bảo nếu spam 2-3 chat cùng lúc, không bị vượt Quota 5 phòng hoặc double-spend kim cương cực đoan.
- **Cơ chế Đồng bộ Background Process (Cron Jobs định kỳ):**
  - Tuân thủ thiết kế xử lý tự động giải ngân mỗi 1 giờ, dùng `Quartz.NET` hoặc `Hangfire` hoặc thuần `IHostedService` cronjob kích hoạt theo chu kỳ hourly UTC.
  - Nhiệm vụ Worker này lướt qua bảng PostgreSQL để: Quét trạng thái SLA `Expired` (hoàn tiền), giải quyết nốt `Disputed` bị quá 48h (Auto-thắng cho Reader) và `Ongoing` User mất tích 48h.

---

## 5. Kỹ Thuật UI/UX Chat & Lấy Tin Nhắn

- **Infinite Scrolling mượt mà (Lazy Load):**
  - FE không query toàn bộ lịch sử nếu có 10,000 dòng.
  - Sử dụng API Backend cấu hình truy vấn theo kiểu **Cursor-based Pagination** (dựa vào `Id` hoặc `CreatedAt` của dòng cuối cùng). Không dùng Offset/Limit để tránh tình huống 1 loạt tin mới chèn vào gây Duplicate khi vuốt ngược.
  - Thư viện `TanStack Query` sử dụng `useInfiniteQuery` tích hợp với object HTML Intersection Observer (ví dụ qua `react-intersection-observer`) phát hiện User lướt lên trên đỉnh scroll, tự trigger fetch trang nhỏ tiếp theo, fill vào bộ nhớ đệm Frontend State mượt mà và không giật Layout chớp màn hình.

---

## 6. Worker Auto Flag & Kiểm Duyệt Nội Dung

- **Bất đồng bộ hóa (Asynchronous Analysis):** Không kìm luồng nhắn tin vào bộ xử lý kiểm duyệt. Khi BE gởi lại signalR "Sent", nó gởi đồng thời 1 tín hiệu Event-Driven (như MediatR Publish `MessageCreatedEvent`).
- **Cơ chế xử lý:** Một lớp Event Handler độc lập nằm ngầm bên dưới đọc nội dung tin nhắn. So sánh chuỗi (Regex patterns) với kho từ khóa blacklist của Platform (tình dục, đe doạ). 
- Nếu trùng lặp, nó gắn thẻ MongoDB Field `isFlagged: true` trên background. Việc này cung cấp tài nguyên bằng chứng dồi dào khi Dispute Report bị kích hoạt sau này cho Admin dashboard xem xét.
