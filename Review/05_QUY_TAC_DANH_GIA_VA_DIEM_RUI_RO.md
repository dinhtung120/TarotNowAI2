# Quy tắc đánh giá và điểm rủi ro

## P0 — chặn merge/remediation ngay

- Vi phạm guard trong `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs` hoặc `EventDrivenArchitectureRulesTests.cs`.
- Backend command handler ghi dữ liệu nhưng inject repository/provider/wallet/realtime/notification side-effect service trực tiếp thay vì dispatch qua `IInlineDomainEventDispatcher`.
- Controller/API/hub broadcast realtime trực tiếp ngoài allowlist kiến trúc hoặc bypass outbox/Redis bridge cho side effect đã migrated.
- Finance/quota/AI/reward flow thiếu transaction boundary, deterministic idempotency key, settlement/refund path hoặc canonical `MoneyChangedDomainEvent` khi wallet state đổi.
- Auth/session/API proxy fail-open, route protected render dữ liệu private khi anonymous, hoặc lộ token/secret/prompt/payment/bank data qua URL/log/client state.
- Admin route/API proxy cho phép non-admin đọc/ghi dữ liệu admin hoặc finance queue.

## P1 — cần kế hoạch sửa

- Dependency chéo module không phá guard nhưng làm tăng coupling hoặc dùng public surface sai cách.
- Frontend app route phình thành business orchestration thay vì composition wrapper.
- App route deep import feature internals dù feature có `public.ts`.
- Prefetch/hydration/query key không nhất quán, làm SSR cache không được dùng hoặc stale data trong wallet/chat/notification/reading.
- i18n thiếu `vi/en/zh` cho copy mới.
- Test coverage thiếu cho flow nghiệp vụ chính, đặc biệt money, AI, admin, realtime, ownership.
- Deploy/smoke/rollback/backup docs không khớp workflow hoặc script hiện tại khi thay đổi ops.

## P2 — theo dõi

- Evidence chưa đủ chi tiết cho phần nội bộ component/action nhưng route/public/prefetch boundary đã rõ.
- Component/hook gần hoặc vượt line budget baseline nhưng không phải touched scope.
- Naming/docs chưa đồng nhất nhưng chưa làm sai behavior.
- Review docs thiếu link tới một test phụ trợ nhưng đã có source runtime chính.

## Cách ghi finding

Mỗi finding phải có:

- Severity: P0/P1/P2.
- Evidence path: file code/test/config cụ thể.
- Tác động: dữ liệu, security, money, realtime, UX hoặc deploy.
- Verify đề xuất: test/guard/manual flow cần chạy.
- Source gap nếu có: ghi rõ phần không thấy evidence trực tiếp trong phạm vi đã đọc.

## Guard source tham chiếu

- Backend architecture: `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs`, `EventDrivenArchitectureRulesTests.cs`, `ApiAndConfigurationStandardsTests.cs`, `CodeQualityRulesTests.cs`.
- Backend business tests tiêu biểu: unit/integration tests dưới `Backend/tests/TarotNow.Application.UnitTests` và `Backend/tests/TarotNow.Api.IntegrationTests`.
- Frontend boundary/size/security: `Frontend/scripts/check-clean-architecture.mjs`, `check-auth-fail-closed.mjs`, `check-component-size.mjs`, `check-hook-action-size.mjs`, `check-risk-coverage.mjs`, `check-next-image-policy.mjs`.
- Data/Ops: `database/postgresql/schema.sql`, `database/mongodb/schema.md`, `docker-compose*.yml`, `deploy/scripts/*`, `.github/workflows/*`.
