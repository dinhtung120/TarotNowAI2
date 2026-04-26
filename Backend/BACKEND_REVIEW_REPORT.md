# BACKEND_REVIEW_REPORT

## 1. Executive summary

- Phiên bản được review: commit `b8b423cda3ab24073f5e791d5f9d291fae83630d`.
- Kết quả kiểm chứng: toàn bộ test hiện có đều pass (`Architecture 27/27`, `Domain 7/7`, `Application 139/139`, `Infrastructure.Integration 11/11`, `Api.Integration 36/36`).
- Tất cả issue đã fix tận gốc đã được xóa khỏi report này.
- Issue còn mở sau re-review:
  - `DEBT-001`: migration event-only toàn bộ command handlers chưa hoàn tất (55 handlers còn dependency trực tiếp repository/service/provider).

## 2. Review theo từng tính năng / module

### 2.1 Reading / AI / Conversation / Escrow money flow

- Chức năng:
  - Freeze tiền khi accept add-money, ghi message accept vào chat, đồng bộ realtime.
- Luồng xử lý chính:
  - `FreezeOfferAsync` (PG) -> publish `ConversationAddMoneyAcceptedSyncRequestedDomainEvent` (outbox) -> domain event handler materialize `payment_accept` message idempotent trên Mongo.
- Vấn đề còn lại:
  - Không còn issue mở thuộc flow compensation add-money trong phạm vi re-review này.
- Mức độ ưu tiên:
  - `Low`.
- Đề xuất xử lý:
  - Tiếp tục theo dõi dead-letter outbox cho nhánh sync message để phát hiện sự cố hạ tầng sớm.

### 2.2 Application / CQRS architecture governance

- Chức năng:
  - Đảm bảo command handlers tuân thủ guardrail event-only.
- Luồng xử lý chính:
  - Architecture tests khóa baseline không tăng nợ.
- Vấn đề còn lại:
  - Nợ kiến trúc gốc chưa được triệt tiêu, hiện chỉ khóa không tăng thêm.
- Bug tiềm ẩn:
  - Phạm vi side-effect trực tiếp còn rộng ở các handler legacy, tăng rủi ro drift kiến trúc theo thời gian.
- Nợ kỹ thuật:
  - 55 command handlers còn dependency trực tiếp `I*Repository|I*Service|I*Provider`.
- Mức độ ưu tiên:
  - `Medium`.
- Đề xuất xử lý:
  - Migrate theo batch bounded-context, giữ architecture test ở chế độ strict baseline set theo danh sách file cụ thể.

## 3. Danh sách issue chi tiết

### 3.1 Tech debt

- ID: `DEBT-001`
- Module: `Application/CQRS architecture`
- Loại: `Tech debt`
- Mô tả: Event-only migration chưa hoàn tất toàn cục; guardrail hiện đã khóa theo strict baseline set.
- Tác động: Rủi ro không đồng nhất dependency direction và side-effect boundary giữa các module legacy.
- Bằng chứng trong code:
  - `tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs` khóa drift theo added/removed entries so với baseline file.
  - `tests/TarotNow.ArchitectureTests/Baselines/command-handler-dependency-debt-baseline.txt` đang lock 55 file vi phạm hiện hữu.
  - Static scan hiện tại: còn 55 command handler có dependency `I*Repository|I*Service|I*Provider`.
- Nguyên nhân gốc rễ: Legacy command handlers chưa migrate sang event-driven choreography.
- Cách sửa: Migrate theo module ưu tiên (`Wallet/Escrow/Auth/Reading`) + siết rule test theo allowlist/subset sau mỗi batch.
- Priority: `Medium`

## 4. Danh sách refactor đề xuất

### Việc nên làm ngay

1. Lập migration plan đóng dần `DEBT-001` theo batch nhỏ, mỗi batch kèm giảm thực tế baseline file.

### Việc có thể trì hoãn

1. Chuẩn hóa template migration playbook để giảm ma sát cho các batch event-only kế tiếp.
2. Bổ sung dashboard theo dõi tiến độ giảm baseline `DEBT-001` theo sprint.

## 5. Kết luận

- Các lỗi đã fix tận gốc đã được loại khỏi report.
- Hệ thống hiện còn **1 nợ kiến trúc dài hạn** (`DEBT-001`).
- Khuyến nghị release:
  - Có thể release thận trọng; rủi ro còn lại thuộc nhóm kiến trúc dài hạn, không phải blocker vận hành tức thời.
