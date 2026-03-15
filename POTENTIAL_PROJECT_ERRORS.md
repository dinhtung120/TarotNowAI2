# DANH SÁCH CÁC LỖI TIỀM TÀNG TRONG DỰ ÁN TAROTNOW

Tài liệu này liệt kê các rủi ro, lỗi logic và lỗi kỹ thuật có thể phát sinh trong quá trình phát triển dự án TarotNow, dựa trên cấu trúc Clean Architecture và các yêu cầu nghiệp vụ phức tạp về AI và tài chính.

---

## 1. Kiến trúc & Clean Architecture (Phòng tránh vi phạm Layering)

*   **Lỗi phụ thuộc ngược (Circular Dependency):** Các lớp bên trong (Domain/Application) vô tình tham chiếu trực tiếp đến các lớp bên ngoài (Infrastructure/Presentation).
    *   *Nguyên nhân:* Không sử dụng Interface/Dependency Inversion đúng cách.
    *   *Hậu quả:* Khó viết Unit Test, hệ thống bị thắt nút cổ chai (tight coupling).
*   **Lỗi rò rỉ Logic nghiệp vụ (Logic Leakage):** Đưa các đoạn code kiểm tra điều kiện (Invariants) vào Controller hoặc Database thay vì giữ trong Domain Entity hoặc Application Service.
*   **Sử dụng trực tiếp Entity trong API Response:** Không thông qua DTO/Mapper.
    *   *Hậu quả:* Rò rỉ thông tin nhạy cảm của database hoặc gây lỗi vòng lặp JSON (Circular Reference).

---

## 2. Hệ thống AI & Streaming (SSE & OpenAI/Gemini integration)

*   **Lỗi treo kết nối SSE (Zombie Connections):** Client đã ngắt kết nối nhưng Server vẫn tiếp tục gọi API AI và tiêu tốn Token.
    *   *Giải pháp:* Phải lắng nghe sự kiện `CancellationToken` từ trình duyệt hủy kết nối để stop stream ngay lập tức.
*   **Lỗi vượt định mức AI Quota (Over-spending):** Người dùng thực hiện gọi AI mà không bị trừ tiền hoặc trừ tiền sau khi đã gọi xong.
    *   *Quy tắc:* Phải tạm giữ (Reserve) số dư/quota trước khi gọi AI, sau đó mới xác nhận (Confirm) hoặc hoàn trả (Refund).
*   **Lỗi Parsing Stream:** Định dạng Markdown từ AI trả về bị cắt ngang hoặc làm hỏng cấu trúc UI của Frontend.
*   **Chat History quá tải:** Gửi toàn bộ lịch sử chat quá dài cho AI dẫn đến lỗi vượt giới hạn Context Window của Model.

---

## 3. Quản lý Tài chính & Giao dịch (Wallet, Quota, Escrow)

*   **Lỗi Race Condition khi nạp/rút tiền:** Hai yêu cầu thanh toán diễn ra đồng thời dẫn đến số dư bị sai lệch.
    *   *Giải pháp:* Sử dụng Optimistic Concurrency (Row Version) hoặc Pessimistic Locking trong database.
*   **Lỗi Double Spending:** Người dùng lợi dụng độ trễ của hệ thống để sử dụng một số dư cho nhiều lượt trải bài khác nhau.
*   **Lỗi Transaction không nguyên tử (Atomic):** Trừ tiền thành công nhưng hệ thống gặp lỗi khi tạo bản ghi Reading, dẫn đến người dùng mất tiền mà không có kết quả.
    *   *Yêu cầu:* Sử dụng ACID Transaction bao trùm cả logic trừ tiền và tạo reading.

---

## 4. Cơ sở dữ liệu & Đồng bộ hóa (PostgreSQL & MongoDB)

*   **Lỗi không nhất quán dữ liệu (Data Inconsistency):** Dữ liệu write-model (PostgreSQL) đã cập nhật nhưng read-model (MongoDB/Redis) chưa được cập nhật kịp thời hoặc gặp lỗi khi đồng bộ.
*   **Lỗi Format ID:** PostgreSQL sử dụng Integer/UUID trong khi MongoDB sử dụng ObjectId. Việc mapping sai lệch dẫn đến không tìm thấy dữ liệu.
*   **Lỗi N+1 Query:** Xảy ra khi lấy danh sách Reading và cố gắng truy xuất thông tin User/Card cho từng dòng một cách riêng lẻ thay vì dùng `Include` hoặc `Join`.

---

## 5. Frontend & Trải nghiệm người dùng (Next.js App Router)

*   **Lỗi Hydration Mismatch:** Server render một kiểu dữ liệu (ví dụ: ngày tháng), Client lại hiển thị kiểu khác do múi giờ địa phương.
*   **Lỗi Memory Leak trong SSE:** Component bị unmount nhưng EventSource vẫn chưa được close, dẫn đến rò rỉ bộ nhớ trình duyệt.
*   **Lỗi Client/Server Component:** Cố gắng sử dụng các React Hook (`useState`, `useEffect`) trong Server Component mà không khai báo `"use client"`.
*   **Lỗi Routing:** Token JWT hết hạn nhưng người dùng vẫn truy cập được vào các trang cần bảo mật do Client-side cache chưa cập nhật.

---

## 6. Bảo mật & Xác thực

*   **Lỗi Refresh Token Rotation:** Hacker chiếm được refresh token cũ và cố gắng tái sử dụng.
*   **Lỗi Phân quyền (IDOR - Insecure Direct Object Reference):** Người dùng A có thể xem kết quả trải bài của người dùng B bằng cách thay đổi ID trên URL.
    *   *Giải pháp:* Luôn kiểm tra quyền sở hữu (`OwnerCheck`) cho mọi request truy xuất dữ liệu cá nhân.
*   **Lỗi lộ thông tin lỗi (Information Exposure):** Backend trả về Stack Trace chi tiết cho Client thay vì dùng `ProblemDetails` chuẩn hóa.

---

> [!TIP]
> Luôn sử dụng bộ lọc (Guard Clauses) ở đầu các method và viết Unit Test cho các trường hợp biên (Edge cases) để giảm thiểu 80% các lỗi trên.
