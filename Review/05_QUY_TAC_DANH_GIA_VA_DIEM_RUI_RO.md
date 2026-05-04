# Quy tắc đánh giá và điểm rủi ro

## P0 — chặn merge/remediation ngay

- Vi phạm guard trong `ArchitectureBoundariesTests.cs` hoặc `EventDrivenArchitectureRulesTests.cs`.
- Backend command handler ghi dữ liệu nhưng inject repository/provider/realtime/notification/wallet service trực tiếp thay vì `IInlineDomainEventDispatcher`.
- Controller/API hoặc hub broadcast realtime trực tiếp ngoài allowlist kiến trúc.
- Flow finance/quota/AI thiếu transaction boundary, idempotency key, settlement/refund path hoặc canonical money event.
- Frontend auth/session fail-open, route/API route bypass ownership/auth guard, hoặc lộ token/secret.

## P1 — cần kế hoạch sửa

- Dependency chéo module không phá guard nhưng làm tăng coupling.
- Shared frontend chứa logic feature-specific quá nhiều hoặc route app quá dày.
- Prefetch/hydration/query key không nhất quán.
- i18n thiếu VI/EN/ZH cho copy mới.
- Test coverage không có cho flow nghiệp vụ chính hoặc flow tiền/realtime.
- Deploy rollback/smoke/backup path không được cập nhật khi thay đổi ops.

## P2 — theo dõi

- Evidence chưa đủ chi tiết nhưng không phát hiện vi phạm.
- File gần vượt line budget hoặc component/hook gần vượt guard.
- Naming/docs chưa đồng nhất.

## Cách ghi finding

Mỗi finding phải có: severity, evidence path, tác động, đề xuất verify. Nếu chưa đọc đủ source, ghi `Không tìm thấy evidence trực tiếp trong phạm vi đã rà soát`.

## Guard source tham chiếu

- Backend: `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs`, `EventDrivenArchitectureRulesTests.cs`, `ApiAndConfigurationStandardsTests.cs`, `CodeQualityRulesTests.cs`.
- Frontend: `Frontend/scripts/check-clean-architecture.mjs`, `check-component-size.mjs`, `check-hook-action-size.mjs`, `check-auth-fail-closed.mjs`, `check-risk-coverage.mjs`.
- Data/Ops: `database/postgresql/schema.sql`, `database/mongodb/schema.md`, `deploy/scripts`, `.github/workflows`.
