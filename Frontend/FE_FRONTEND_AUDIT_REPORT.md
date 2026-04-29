# FE Frontend Production Audit Report (Re-Review)

## 1. Executive Summary
- Scope: toàn bộ `Frontend/` gồm `src`, `app/api`, query/state layer, config/build/test scripts.
- Automated evidence (re-run):
- `npm run lint` passed.
- `npm run lint:architecture` passed.
- `npm run lint:component-size` passed.
- `npm run lint:auth-policy` passed.
- `npm run verify:event-evidence` passed (6 required command keys).
- `npm run test:coverage:risk` passed.
- `npm run build` passed.
- `npm run knip` passed.
- `npm run test:e2e` passed.
- Result after re-review: các issue đã fix tận gốc đã được loại khỏi báo cáo. Còn lại 3 issue chưa đóng hoàn toàn (`HIGH-04`, `MED-06`, `MED-07`).
- Production stability: chưa đạt mức release sạch rủi ro vì còn 1 High liên quan error ownership ở admin-sensitive UI paths.

## 2. Architecture Review
- Không còn Critical về auth boundary, admin role boundary, domain-command/idempotency contract trong FE.
- Vấn đề kiến trúc còn lại:

### MED-06 - Structural decomposition chưa hoàn tất + guardrail chưa bao phủ toàn bộ hotspot
- File: `src/features/profile/application/useProfilePage.ts`, `src/shared/application/hooks/usePresenceConnection.registration.ts`, `scripts/check-hook-action-size.mjs`, `scripts/hook-action-size-baseline.json`
- Component/Hook/Function: `useProfilePage`, `registerPresenceConnectionHandlers`, hook-size guard
- Loại issue: Architecture/maintainability debt
- Mô tả: Một số hook trọng yếu vẫn quá lớn, đồng thời hook-size guard mới chỉ track 5 file nên chưa ngăn tái diễn ở các hotspot khác.
- Root cause: Guard áp dụng theo baseline cục bộ thay vì policy coverage cho toàn bộ hook/action layer.
- Impact: Refactor/risk review khó, tăng xác suất regression khi chỉnh sửa nhanh.
- Cách fix: Mở rộng guard cho toàn bộ `application/**/*.ts(x)` + `shared/**/*.ts(x)` theo ngưỡng complexity/line; tách tiếp `useProfilePage` và `usePresenceConnection.registration` theo responsibility slices.
- Priority: Medium
- Evidence: `src/features/profile/application/useProfilePage.ts` (317 lines), `src/shared/application/hooks/usePresenceConnection.registration.ts` (383 lines), `scripts/hook-action-size-baseline.json` chỉ track 5 files.

## 3. Feature-by-feature Review
- Auth/Session/Proxy: no findings còn mở.
- Admin - Gamification BFF: no findings còn mở.
- Admin - Deposits/Reader Requests/Readings/Dashboard/Notifications: core issues cũ đã fix.
- Admin - Promotions: còn issue `HIGH-04` ở initial error ownership.
- Admin - System Configs: còn issue `HIGH-04` (silent fallback).
- Profile/Realtime core hooks: còn debt cấu trúc `MED-06`.

## 4. ReactJS Specific Problems
- Không còn bug lifecycle/blocker thuộc nhóm đã audit trước (`HIGH-01`, `MED-02`, `MED-03` đã fix).
- Còn tồn tại debt React architecture thuộc `MED-06` (hook quá lớn, boundary chưa tách đủ).

## 5. Performance Audit
- Query-thrashing admin readings đã được xử lý.
- Re-render do `useAuthStore()` no-selector đã được xử lý.
- Remaining performance-structural risk: `MED-06` (file lớn gây khó tối ưu lâu dài).

## 6. Security Audit
- Không còn Critical/High về fail-open auth, admin role bypass, domain-command/idempotency bypass trong FE.
- Còn issue operational-risk (data critical UX integrity):

### HIGH-04 - Silent fallback vẫn còn ở một số admin-sensitive paths
- File: `src/features/admin/system-configs/application/useAdminSystemConfigs.ts`, `src/features/admin/promotions/presentation/AdminPromotionsPage.tsx`
- Component/Hook/Function: `useAdminSystemConfigs` queryFn, `AdminPromotionsPage` initial data mapping
- Loại issue: Error ownership weakness (data-critical UI)
- Mô tả: Khi backend lỗi, một số flow vẫn rơi về danh sách rỗng thay vì surfaced error state rõ ràng.
- Root cause: Query/data bootstrap vẫn dùng fallback `[]` trong nhánh thất bại thay vì throw + explicit error rendering.
- Impact: Admin có thể hiểu nhầm là “không có dữ liệu” thay vì “tải lỗi”, làm chậm incident detection.
- Cách fix: Bắt buộc `queryFnOrThrow`/typed error cho system-configs + promotions initial load; render error-state nhất quán thay vì fallback rỗng.
- Priority: High
- Evidence: `src/features/admin/system-configs/application/useAdminSystemConfigs.ts` (queryFn trả `[]` khi `!result.success`), `src/features/admin/promotions/presentation/AdminPromotionsPage.tsx` (fallback `initialPromotions = []` khi action fail).

## 7. UX / Accessibility Review
- A11y icon-only controls ở chat composer đã được xử lý.
- Không còn issue UX/A11y blocker trong danh sách cũ.
- Residual UX risk hiện tại phụ thuộc `HIGH-04` (empty vs failed state bị mờ ở vài admin paths).

## 8. Code Smell / Dead Code
- `LOW-01` và `LOW-02` đã được xử lý.
- Không còn dead code/dependency issue theo `knip` hiện tại.

## 9. Testing Gap

### MED-07 - Risk-based test matrix chưa bao phủ đủ một số hotspot
- File: `coverage/coverage-summary.json`, `src/features/profile/application/useProfilePage.ts`
- Component/Hook/Function: coverage gate and hotspot suites
- Loại issue: Testing gap
- Mô tả: Gate tổng thể pass nhưng branch coverage hotspot vẫn thấp, đặc biệt profile flow lớn.
- Root cause: Chưa có đủ scenario tests theo risk hotspots thay vì chỉ dựa vào tổng coverage.
- Impact: Regression vẫn có thể lọt ở các nhánh lỗi/edge-case quan trọng.
- Cách fix: Bổ sung scenario tests bắt buộc cho profile hotspot/error branches và lifecycle edge-cases còn mỏng; nâng branch threshold cục bộ cho hotspot files.
- Priority: Medium
- Evidence: `coverage/coverage-summary.json` branch ~67.53%; `src/features/profile/application/useProfilePage.ts` branch ~41.73%, lines ~52.75%.

## 10. Refactor Priority Roadmap
- Fix ngay trước release:
- `HIGH-04`: loại bỏ silent fallback ở `system-configs` và `admin/promotions` initial load.
- Nên xử lý trong sprint kế tiếp:
- `MED-06`: tách tiếp hooks lớn + mở rộng hook/action guardrail coverage.
- `MED-07`: tăng risk-based test matrix cho hotspot files.

## 11. Final Verdict
- Release recommendation: **NO-RELEASE** cho đến khi đóng `HIGH-04`.
- Biggest remaining risk: admin-sensitive UI có thể hiển thị empty-state giả khi backend lỗi.
- Confidence level: **0.89**.
