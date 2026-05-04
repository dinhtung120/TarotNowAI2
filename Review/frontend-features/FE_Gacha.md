# FE Gacha

## 1. Phạm vi

- Mục tiêu nghiệp vụ: review frontend feature `Gacha`.
- Module liên quan: `Frontend/src/features/gacha` nếu tồn tại, route trong `Frontend/src/app/[locale]`, shared/i18n liên quan.
- In scope: route composition, public API boundary, server state, i18n, accessibility cơ bản, guard scripts.
- Out of scope: refactor UI ngoài phạm vi review.

## 2. Entry points & luồng chính

- API/Command/Query/Route chính: xác định page/layout/API route gọi feature `Gacha`.
- Requested Domain Event (nếu có): áp dụng gián tiếp qua backend command/API contract.
- Realtime event (nếu có): rà subscription, SignalR/client bridge hoặc notification channel.
- External integration (nếu có): rà browser API, payment redirect, upload, analytics hoặc provider SDK nếu feature dùng.

## 3. Dependency map

### 3.1 Upstream phụ thuộc vào module này

- App Router page/layout hoặc shared prefetch runner import feature `Gacha`.
- Feature khác import qua `public.ts` nếu có cross-feature flow.

### 3.2 Module này phụ thuộc downstream

- Application interfaces: frontend actions, query hooks, route handlers hoặc gateways.
- Infrastructure repositories/services: không dùng concrete backend; chỉ qua API/action/gateway pattern hiện có.
- Shared utilities: UI primitives, query client, auth/session, i18n, navigation, prefetch.
- Data stores: TanStack Query cho server state, Zustand chỉ cho local UI state nếu cần.

### 3.3 Ràng buộc kiến trúc

- Clean Architecture boundary: app route import qua `@/features/*/public` khi có public export.
- Event-driven rules: UI không bypass backend command/event contract cho side effect nghiệp vụ.
- Thin handler / thin route rules: `page.tsx` và `layout.tsx` chỉ composition, orchestration nằm trong feature/hook.

## 4. Dữ liệu & trạng thái

- Entity/Document chính: backend DTO/domain contract mà UI consume.
- Transaction boundary: thuộc backend; UI cần không duplicate settlement hoặc optimistic mutation nguy hiểm.
- Idempotency key path (nếu có): rà nếu UI khởi tạo command finance/AI/payment.
- Outbox/realtime bridge path (nếu có): rà nếu UI hiển thị realtime state hoặc notification.

## 5. Frontend contract

- public.ts exports: xác định `Frontend/src/features/*/public.ts` liên quan `Gacha`.
- App route wrapper: route phải mỏng và có boundary import rõ.
- i18n keys: đối chiếu `Frontend/messages/vi`, `Frontend/messages/en`, `Frontend/messages/zh`.
- Prefetch/hydration/guard liên quan: đối chiếu `Frontend/src/shared/server/prefetch` và query key tiêu thụ trong component.

## 6. Test coverage hiện tại

- Architecture tests liên quan: frontend guard scripts trong `Frontend/scripts`.
- Unit/Integration tests liên quan: tìm trong `Frontend/tests`, colocated tests hoặc Playwright nếu có.
- Gaps: route thiếu test, hydration mismatch risk, hardcoded copy, accessibility hoặc size guard gần vượt budget.

## 7. Rủi ro kiến trúc

- P0: auth fail-open, route bypass boundary gây side effect nhạy cảm, hardcoded secret/token, unsafe browser handling.
- P1: app route quá dày, import sâu vào feature internals, i18n thiếu, duplicate fetch/hydration mismatch.
- P2: component/hook gần vượt budget, evidence review chưa đủ.

## 8. Output review chuẩn

- Kết luận: Pass / Pass có điều kiện / Cần remediation.
- Evidence: liệt kê route, public export, hook/component, i18n và guard đã đọc.
- Việc cần làm ưu tiên cao: item P0/P1 có owner rõ.
- Việc theo dõi sau: cleanup hoặc tài liệu hóa gap P2.
