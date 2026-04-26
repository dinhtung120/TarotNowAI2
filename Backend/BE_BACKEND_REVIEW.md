# 1. Executive summary

Sau đợt fix thủ công hiện tại, các nhóm lỗi về dependency security, mapping auth, idempotency refresh DB-first, telemetry token usage thật từ provider, mở rộng inline idempotency coverage, và test moderation failure modes đã được xử lý.

Trạng thái còn mở tập trung vào **kiến trúc executor/command-dispatch**:

- `BE-002` (High)
- `BE-003` (High)
- `BE-004` (Medium)

Đánh giá release: hệ thống đã an toàn hơn đáng kể so với bản trước, nhưng còn nợ kiến trúc lớn cần hoàn tất để đạt chuẩn clean boundary theo guardrail.

---

# 2. Review theo từng tính năng / module

## 2.1. CQRS event-dispatch boundary

- **Chức năng:** Command -> inline domain event -> xử lý nghiệp vụ.
- **Trạng thái:** command vẫn đi qua `CommandExecutor` + `ICommandExecutionExecutor<,>` + `CommandDispatch` handlers.
- **Rủi ro:** boundary side-effects chưa dồn hoàn toàn vào event handlers theo tiêu chuẩn đã chốt.
- **Ưu tiên:** High.

## 2.2. Architecture tests enforcement

- **Chức năng:** kiểm soát vi phạm kiến trúc ở CI.
- **Trạng thái:** vẫn còn allowlist legacy và rule chưa chặn triệt để executor pattern.
- **Rủi ro:** vi phạm kiến trúc có thể lọt khi mở rộng module mới.
- **Ưu tiên:** High/Medium.

---

# 3. Danh sách issue chi tiết (còn mở)

- **ID:** BE-002  
  **Module:** Architecture / CQRS  
  **Loại:** Tech debt  
  **Mô tả:** Side-effects vẫn nằm trong `CommandExecutor`, chưa migrate full vào event handlers như target architecture.  
  **Tác động:** Boundary mờ, khó kiểm soát dependency direction và side-effects khi scale.  
  **Bằng chứng trong code:** còn nhiều class `*CommandExecutor` implement `ICommandExecutionExecutor<,>` trong `src/TarotNow.Application/Features/**/Commands/**`.  
  **Cách sửa:** refactor toàn bộ 57 cặp executor/dispatch sang event handler thực thụ, xóa executor pattern.  
  **Priority:** High

- **ID:** BE-003  
  **Module:** Architecture Tests  
  **Loại:** Test gaps  
  **Mô tả:** Rule kiến trúc chưa chặn triệt để blind spot của executor pattern.  
  **Tác động:** CI chưa enforce đúng target “no executor pattern”.  
  **Bằng chứng trong code:** `tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs` chưa có rule hard-fail theo symbol cho `ICommandExecutionExecutor<,>`.  
  **Cách sửa:** thêm rule cứng cấm `ICommandExecutionExecutor<,>` và `*CommandExecutor`.  
  **Priority:** High

- **ID:** BE-004  
  **Module:** Architecture Tests  
  **Loại:** Tech debt  
  **Mô tả:** Còn allowlist legacy trong architecture tests.  
  **Tác động:** policy bị nới lỏng, khó đóng debt kiến trúc dứt điểm.  
  **Bằng chứng trong code:** `allowedLegacyPaths` tại `tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs`.  
  **Cách sửa:** xóa allowlist sau khi migrate phase kiến trúc BE-002.  
  **Priority:** Medium

---

# 4. Danh sách refactor đề xuất

## 4.1. Việc nên làm ngay

- Hoàn tất migration `CommandExecutor` -> event handlers cho toàn bộ command flow.
- Xóa `ICommandExecutionExecutor<,>`, xóa `RegisterCommandExecutors()`.
- Xóa toàn bộ allowlist legacy trong architecture tests.

## 4.2. Việc có thể trì hoãn

- Không còn mục trì hoãn lớn ngoài cụm kiến trúc trên (các issue khác đã đóng).

---

# 5. Kết luận

Backend đã đóng phần lớn lỗi vận hành và security quan trọng của vòng review trước.  
Cụm rủi ro còn lại là **kiến trúc command executor** và enforcement test tương ứng (`BE-002/003/004`).  
Khi hoàn tất migration cụm này, file review có thể đóng toàn bộ.
