# FE Notifications

## 1. Phạm vi source đã rà

- Feature source: `Frontend/src/features/notifications`.
- Public export: `Frontend/src/features/notifications/public.ts` nếu tồn tại.
- App routes cần đối chiếu: `Frontend/src/app/[locale]` grep `notifications` hoặc route nghiệp vụ tương ứng.
- API route proxy/action source: `Frontend/src/app/api` và feature server actions nếu có.
- Guards: `Frontend/scripts/check-clean-architecture.mjs`, `check-component-size.mjs`, `check-hook-action-size.mjs`, `check-auth-fail-closed.mjs`.

## 2. Entry points & luồng chính

- Route/page/layout: xác định bằng `find Frontend/src/app -type f | grep -E 'notifications|notifications'`.
- Feature public surface: `Frontend/src/features/notifications/public.ts` là boundary ưu tiên cho app imports.
- Components/hooks/actions: nằm trong `Frontend/src/features/notifications` theo cấu trúc hiện tại.
- Backend/API contract: đi qua app API route, server action hoặc shared API client; không bypass auth/security flow.

## 3. Dependency map thực tế

### Upstream

- App Router page/layout/API route import feature `notifications`.
- Shared prefetch runner có thể gọi query/action của feature nếu SSR hydration cần server state.

### Downstream

- Shared utilities: query client, auth/session, i18n, UI primitives, prefetch/hydration.
- Backend contracts: API endpoints/commands tương ứng ở backend feature liên quan.
- State: TanStack Query cho server state; Zustand chỉ cho local UI state khi có evidence.

## 4. Dữ liệu & trạng thái

- Server state: rà query keys/hooks/actions trong feature.
- Local UI state: rà component/hook trong `Frontend/src/features/notifications`.
- i18n: đối chiếu `Frontend/messages/vi`, `Frontend/messages/en`, `Frontend/messages/zh`.
- Realtime/payment/idempotency: bắt buộc rà vì feature có finance/reward/realtime-facing flow.

## 5. Boundary và guard

- Thin route: `page.tsx`/`layout.tsx` chỉ composition, orchestration nằm trong feature/hook/action.
- Public API: app route nên import qua `@/features/*/public` khi có.
- Guard scripts: clean architecture, component size, hook/action size, auth fail-closed, image policy, risk coverage.
- Accessibility/i18n: touched interactive UI cần accessible name/focus/error association; copy mới cần localization.

## 6. Test coverage hiện tại

- Guard coverage: `Frontend/scripts/*.mjs`.
- Feature tests: tìm trong `Frontend/tests` hoặc colocated tests với grep `notifications`.
- Không tìm thấy evidence trực tiếp: ghi rõ route/component/action chưa có test khi audit chi tiết.

## 7. Rủi ro kiến trúc

- P0: auth fail-open, token/secret exposure, payment/reward command duplicate, route bypass backend security.
- P1: route quá dày, import sâu vào feature internals, prefetch/hydration mismatch, thiếu i18n.
- P2: component/hook gần vượt budget, evidence test chưa đủ.

## 8. Kết luận review

- Mức độ phù hợp kiến trúc: file đã neo đúng source feature `notifications` và guard frontend; cần audit từng route/action để kết luận pass cuối cùng.
- Evidence quan trọng: `Frontend/src/features/notifications`, `Frontend/src/app/[locale]`, `Frontend/src/shared/server/prefetch`, `Frontend/messages`, `Frontend/scripts`.
- Việc cần làm ưu tiên cao: điền route/action/test cụ thể trong review PR module.
- Follow-up: không suy đoán nếu chưa thấy evidence trực tiếp.
