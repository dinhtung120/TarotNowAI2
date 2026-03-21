# Frontend Refactor Checklist - TarotNow FE (Clean Architecture + SOLID)

**Ngày kiểm tra:** 22/03/2026  
**Phạm vi:** `Frontend/src`, `Frontend/messages`  
**Mục tiêu chính:**
- Loại bỏ lỗi còn sót và chuẩn hóa chất lượng code.
- Refactor theo hướng Clean Architecture + SOLID, không làm đổi UI/layout hiện tại.
- Giảm duplicate code, giảm độ dài file, giảm số trách nhiệm trong mỗi file.
- Tăng khả năng bảo trì dài hạn và tốc độ phát triển feature mới.

## Cập nhật tiến độ (22/03/2026)

- Hoàn tất xử lý lỗi lint blocker và warning ưu tiên cao; `lint/build` đang pass.
- Tạo nền tảng shared (`serverAuth`, `serverHttpClient`, `parseApiError`, `logger`, `browserStorage`, `formatCurrency`, `formatDateTime`, `useAuthGuard`).
- Migrate toàn bộ `src/actions/*` sang lớp HTTP dùng chung (không còn `await fetch()` trong action code).
- Chuẩn hóa nhiều lỗi SSR/client safety: bỏ `alert/confirm`, thay `<img>` bằng `next/image`, sửa dependency hooks, bỏ `key={index|i}`.
- Hoàn tất tách action layer theo domain với facade tương thích ngược: `actions/{admin,auth,reader,chat,escrow}/*`.
- Làm mỏng nhiều route lớn: chuyển logic UI sang `src/features/**/presentation/*`, route `app/**/page.tsx` còn wrapper.
- Tách thêm hook ứng dụng cho các cụm nặng: `useProfilePage`, `useWithdrawPage`, `useAdminPromotions`.
- Hoàn tất auth feature split theo clean layers: `domain/schemas`, `application/hooks`, `presentation/components` + route wrappers cho `register|verify|forgot|reset`.
- Hoàn tất wallet split: `wallet overview` + `deposit` chuyển sang `features/wallet` (hook + presentation), bổ sung domain constants và status mapper cho withdrawal.
- Hoàn tất chat split theo concern: thêm `useChatConnection`, `usePaymentOfferActions`, `mergeMessages` utility; `ChatRoomPage` chỉ còn orchestration/render.
- Chuẩn hóa admin data layer bằng `useAdminUsers`, `useAdminDeposits`, `useAdminReaderRequests`, `useAdminReadings` để tách logic khỏi presentation.
- Chuẩn hóa contract `ActionResult<T>` cho cụm auth actions (`session/registration/recovery`) và giữ tương thích call-site hiện tại.
- Bổ sung shared admin UI primitives (`FilterTabs`, `ActionConfirmModal`, `StepPagination`, `TableStates`) và áp dụng vào các màn `users/deposits/reader-requests/promotions`.

## 1) Snapshot hiện trạng (đã quét thực tế)

| Hạng mục | Số liệu hiện tại | Ghi chú |
|---|---:|---|
| Tổng file TS/TSX trong `src` | 144 | Tập trung ở `app/`, `features/`, `components/`, `actions/`, `shared/` |
| Số route `page.tsx` | 34 | App Router (locale + auth/user/admin) |
| File > 100 dòng | 70 | Bao gồm nhiều file feature mới tách từ route |
| File > 200 dòng | 33 | Đã giảm so với mốc trước |
| Action files | 36 | ~2812 dòng, đã tách module theo domain |
| Components files | 29 | Có nhiều component >150 dòng |
| `messages/*.json` | 3 file / 4509 dòng | Monolithic i18n (en/vi/zh) |
| `console.log` | 0 | Tốt |
| `console.error` | 14 | Đã giảm mạnh, còn lại chủ yếu ngoài action layer |
| `any` | 0 | Đã xử lý explicit `any` còn sót |
| `key={index|i}` | 0 | Đã thay bằng key ổn định |
| Truy cập trực tiếp `window/document/storage` | 103 | Cần chuẩn hóa SSR-safe utility |
| `getAccessToken()` alias | 14 alias | Đã dùng chung `getServerAccessToken` trong action layer (còn alias cần gom tiếp) |
| `await fetch` trong `actions` | 0 calls | Đã chuyển qua `serverHttpClient` (còn 2 chỗ trong comment) |

## 2) Kết quả kiểm tra nhanh chất lượng hiện tại

### 2.1 Lint/Build baseline
- `npm -C Frontend run build`: **PASS**.
- `npm -C Frontend run lint`: **PASS** (0 errors, 0 warnings).

### 2.2 Lỗi blocking cần xử lý ở phase đầu
- [x] `Frontend/src/app/[locale]/(user)/notifications/page.tsx:115`: React rule `set-state-in-effect`.
- [x] `Frontend/src/components/ui/Input.tsx:127`: `Unexpected any` (`ref={ref as any}`).

### 2.3 Vấn đề chất lượng ưu tiên cao
- [x] `key` không ổn định (`key={index}`, `key={i}`) ở nhiều file: 
  `app/[locale]/page.tsx`, `app/[locale]/reading/session/[id]/page.tsx`,
  `app/[locale]/(user)/reading/history/page.tsx`,
  `app/[locale]/(user)/reading/history/[id]/page.tsx`,
  `app/[locale]/(user)/reader/apply/page.tsx`, `components/layout/AstralBackground.tsx`, `components/ui/SkeletonLoader.tsx`.
- [x] Dùng `alert/confirm` trực tiếp:
  `components/chat/EscrowPanel.tsx`, `app/[locale]/(user)/chat/[id]/page.tsx`.
- [x] `useEffect` dependency warnings:
  `app/[locale]/(user)/chat/[id]/page.tsx`, `components/AiInterpretationStream.tsx`.
- [x] Dùng `<img>` thay vì `next/image` trong chat pages.
- [x] `sessionStorage` đọc trực tiếp trong render ở
  `app/[locale]/reading/session/[id]/page.tsx` (nên chuyển vào hook SSR-safe).
- [x] Một điểm dùng trực tiếp `process.env.NEXT_PUBLIC_API_URL` trong chat room thay vì dùng `lib/api.ts` chuẩn chung.

## 3) Đánh giá SOLID theo code hiện tại

| Nguyên tắc | Trạng thái | Ví dụ vi phạm thực tế | Hướng sửa |
|---|---|---|---|
| S - Single Responsibility | Vi phạm nhiều | `reading/session/[id]/page.tsx` vừa animation, vừa game logic, vừa API reveal, vừa profile sync, vừa render; `chat/[id]/page.tsx` vừa SignalR, vừa message state, vừa payment offer, vừa layout | Tách thành hook logic + component presentation + service adapter |
| O - Open/Closed | Chưa tốt | Logic status badge/icon/style lặp ở nhiều page admin/wallet/chat; thêm status mới phải sửa nhiều file | Tạo `status-mappers.ts` theo feature, expose map/config |
| L - Liskov Substitution | Tạm ổn | Ít vấn đề trực tiếp | Duy trì contract rõ cho reusable components |
| I - Interface Segregation | Chưa tốt ở vài nơi | Một số page/state object lớn + modal state trộn nhiều concern (`admin/users`, `admin/deposits`, `admin/reader-requests`) | Tách type nhỏ theo use-case (`FormState`, `ListState`, `ActionState`) |
| D - Dependency Inversion | Vi phạm rõ | UI layer (`app/**`, `components/**`) import trực tiếp `@/actions/*` ở rất nhiều nơi | Thêm Application hooks/use-cases làm abstraction, UI chỉ gọi hook |

## 4) Duplicate code backlog (ưu tiên tách dùng chung)

| Mẫu lặp | Dấu hiệu | Ước lượng | Đề xuất tách |
|---|---|---:|---|
| Lấy access token từ cookie | `getAccessToken()` copy ở nhiều actions | 8 nơi | `src/shared/infrastructure/auth/serverAuth.ts` |
| Fetch + `response.ok` + parse error | Boilerplate trong action files | 40+ nơi | `src/shared/infrastructure/http/serverHttpClient.ts` + `parseApiError.ts` |
| Format tiền VND | `new Intl.NumberFormat(locale...)` lặp | 4+ nơi | `src/shared/utils/format/formatCurrency.ts` |
| Format thời gian | `toLocaleDateString/toLocaleTimeString` lặp | 25+ nơi | `src/shared/utils/format/formatDateTime.ts` |
| Auth guard push login | `router.push('/login')` lặp | 7 nơi | `src/shared/application/hooks/useAuthGuard.ts` |
| Initial fetch timeout | `initialFetchTimer` lặp | 6 nơi | `src/shared/application/hooks/useAsyncInit.ts` (hoặc bỏ hẳn timeout 0ms) |
| Status badge/icon/style | Mỗi page tự định nghĩa | 6-8 nơi | `features/*/presentation/status/*` hoặc `shared/constants/statusMap.ts` |
| Toast success/error pattern | Action handler giống nhau | Nhiều nơi | `useActionFeedback` hook |
| Loading/Empty table blocks | Trùng ở admin/user pages | Nhiều nơi | `shared/ui/TableStates.tsx` |

## 5) File dài cần tách trước (top priority)

| File | Số dòng | Lý do dài | Đề xuất tách |
|---|---:|---|---|
| `src/app/[locale]/reading/session/[id]/page.tsx` | 663 | Animation + chọn bài + reveal + stream + profile sync | `useReadingSessionLogic.ts`, `DeckBoard.tsx`, `PickedStack.tsx`, `ReadingSessionHeader.tsx`, `useSessionDraftStorage.ts` |
| `src/app/[locale]/(user)/chat/[id]/page.tsx` | 536 | SignalR + inbox + payment offer + escrow + rendering | `useChatConnection.ts`, `useChatMessages.ts`, `ChatMessageList.tsx`, `ChatComposer.tsx`, `ChatOfferActions.tsx` |
| `src/actions/adminActions.ts` | 485 | Quá nhiều endpoint admin trong 1 file | Chia theo domain: `users`, `deposits`, `readerRequests`, `reconciliation` |
| `src/app/[locale]/(user)/profile/page.tsx` | 478 | Auth guard + fetch + form + reader request + render lớn | `useProfilePage.ts`, `ProfileSummaryCard.tsx`, `ProfileEditForm.tsx`, `ReaderUpgradeCard.tsx` |
| `src/app/[locale]/admin/promotions/promotions-client.tsx` | 427 | CRUD + form + list + toast + state | `usePromotionsAdmin.ts`, `PromotionForm.tsx`, `PromotionsTable.tsx` |
| `src/app/[locale]/(user)/wallet/withdraw/page.tsx` | 403 | Form + MFA + lịch sử + status map | `useWithdrawForm.ts`, `WithdrawSummary.tsx`, `WithdrawHistoryTable.tsx` |
| `src/app/[locale]/admin/users/page.tsx` | 402 | List + search + modal edit + mutation | `useAdminUsers.ts`, `UsersTable.tsx`, `EditUserModal.tsx` |
| `src/components/common/Navbar.tsx` | 363 | Nav desktop/mobile + avatar dropdown + auth action | `DesktopNav.tsx`, `MobileNav.tsx`, `UserMenu.tsx`, `useNavbarState.ts` |
| `src/components/AiInterpretationStream.tsx` | 351 | SSE + follow-up pricing + chat UI | `useAiInterpretationStream.ts`, `InterpretationMessages.tsx`, `FollowupComposer.tsx` |
| `src/app/[locale]/admin/deposits/page.tsx` | 351 | Filter + list + modal + actions | `useAdminDeposits.ts`, `DepositsFilterBar.tsx`, `DepositsTable.tsx`, `DepositActionModal.tsx` |
| `src/app/[locale]/(user)/wallet/deposit/page.tsx` | 347 | Amount chooser + promo + order + error | `useDepositOrder.ts`, `DepositAmountPicker.tsx`, `DepositSummaryCard.tsx` |
| `src/app/[locale]/admin/readings/page.tsx` | 338 | Filter/list/pagination/format | `useAdminReadings.ts`, `ReadingsTable.tsx` |
| `src/app/[locale]/admin/page.tsx` | 337 | Dashboard stats + cards + fetch nhiều nguồn | `useAdminDashboard.ts`, `DashboardStats.tsx`, `DashboardInsights.tsx` |
| `src/app/[locale]/(user)/collection/page.tsx` | 314 | Fetch + filter + card grid + pagination | `useCollectionPage.ts`, `CollectionFilters.tsx`, `CollectionGrid.tsx` |
| `src/app/[locale]/admin/reader-requests/page.tsx` | 312 | List + filter + detail + approve/reject | `useAdminReaderRequests.ts`, `ReaderRequestCard.tsx`, `ReaderRequestActions.tsx` |

## 6) God component / God hook cần xử lý

| File | Trách nhiệm chính hiện tại | Tách đề xuất |
|---|---|---|
| `reading/session/[id]/page.tsx` | shuffle state, viewport, selected cards, timers, reveal API, profile sync, layout render | 1 hook orchestration + 4 presentation components |
| `chat/[id]/page.tsx` | token parse, SignalR lifecycle, join/leave, message merge, payment offer accept/reject, modal state | `useSignalRConnection`, `useChatRoom`, `useOfferActions` |
| `profile/page.tsx` | auth guard, fetch profile, form validation, save, fetch reader-request | `useProfileData`, `useProfileForm`, `ReaderUpgradeCard` |
| `admin/users/page.tsx` | list/search/pagination, modal edit, submit mutation, toast | `useAdminUserList`, `EditUserModal`, `UserRow` |
| `admin/deposits/page.tsx` | fetch/filter, confirm modal, process action, status style helpers | `useAdminDepositList`, `DepositStatusBadge`, `DepositActionModal` |
| `AiInterpretationStream.tsx` | SSE lifecycle, buffering, follow-up quota, pricing tier, markdown render | `useSseStream`, `useFollowupPricing`, `AiMessageList` |
| `AuthSessionManager.tsx` | refresh token, logout policy, multi-tab sync, visibility/focus hooks | `useAuthTokenRefresh`, `useAuthSync`, `useSessionExpiry` |

## 7) Kiến trúc mục tiêu sau refactor (phù hợp Next.js App Router hiện tại)

> Không chuyển route ra khỏi `app/`. `app/**/page.tsx` sẽ trở thành **thin route** chỉ compose UI + hook từ `features`.

```text
src/
├── app/                               # giữ route App Router
│   └── [locale]/...
├── features/
│   ├── auth/
│   │   ├── domain/                    # types, schemas, pure rules
│   │   ├── application/               # use-cases, hooks logic
│   │   ├── infrastructure/            # action adapters, API mappers
│   │   ├── presentation/              # feature components
│   │   └── i18n/
│   ├── profile/
│   ├── wallet/
│   ├── reading/
│   ├── readers/
│   ├── chat/
│   ├── notifications/
│   ├── admin/
│   └── legal/
├── shared/
│   ├── domain/                        # shared types/constants thuần
│   ├── application/                   # shared hooks
│   ├── infrastructure/                # http/auth/storage/logger
│   ├── presentation/                  # shared ui wrappers/layout fragments
│   ├── utils/
│   ├── constants/
│   ├── types/
│   └── i18n/
├── i18n/
└── proxy.ts
```

### Quy tắc dependency (bắt buộc)
- `domain` không import React, không side-effects.
- `application` không import trực tiếp `next/*` UI API nếu không cần.
- `presentation` không gọi trực tiếp `fetch`; chỉ gọi hooks/use-cases.
- `infrastructure` là nơi duy nhất biết details HTTP/cookies/storage.

## 8) Kế hoạch refactor theo phase nhỏ

## Phase 0 - Baseline & Guardrails (0.5 ngày)
**Mục tiêu:** đóng băng hành vi trước khi refactor.
- [ ] Tạo branch `codex/fe-refactor-phase-0`.
- [x] Chụp baseline `lint`, `build`, route list.
- [x] Ghi danh sách file ưu tiên cao (bảng ở mục 5).
- [x] Thống nhất rule PR: không đổi UI/layout và không đổi behavior.

**Exit criteria:** có tài liệu baseline + checklist, build pass.

## Phase 1 - Scaffold Clean Layers (1 ngày)
**Mục tiêu:** tạo skeleton `features/` + `shared/` không đổi behavior.
- [x] Tạo cấu trúc thư mục chuẩn (mục 7).
- [ ] Tạo `README.md` ngắn cho quy tắc import/dependency.
- [ ] Tạo barrel exports theo feature (`index.ts`) để migration dễ.
- [ ] Giữ `src/actions/*` hiện tại, chưa đổi logic.

**Exit criteria:** code chạy bình thường, import path mới sẵn sàng.

## Phase 2 - Shared Infrastructure Foundation (1-2 ngày)
**Mục tiêu:** gom cross-cutting logic dùng chung.
- [x] Tạo `shared/infrastructure/http/serverHttpClient.ts`.
- [x] Tạo `shared/infrastructure/auth/serverAuth.ts` (thay 8 bản `getAccessToken`).
- [x] Tạo `shared/infrastructure/error/parseApiError.ts`.
- [x] Tạo `shared/infrastructure/logging/logger.ts` (thay `console.error` trực tiếp).
- [x] Tạo `shared/utils/format/*` (currency, date-time, relative-time).
- [x] Tạo `shared/infrastructure/storage/browserStorage.ts` (SSR-safe).

**Exit criteria:** action mới có thể tái sử dụng http/auth helper; không thay đổi UI.

## Phase 3 - Fix Blocking Quality Issues (1 ngày)
**Mục tiêu:** xử lý lỗi lint blocker + warning rủi ro cao.
- [x] Fix `notifications/page.tsx` rule `set-state-in-effect`.
- [x] Fix `Input.tsx` bỏ `any` (ref typing cho textarea/input).
- [x] Sửa `useEffect` dependency warnings ở chat + AI stream.
- [x] Thay `<img>` bằng `next/image` tại chat pages.
- [x] Thay `key={index|i}` bằng key ổn định.
- [x] Đổi `alert/confirm` sang shared modal/toast pattern.

**Exit criteria:** `npm -C Frontend run lint` không còn error.

## Phase 4 - Action Layer Modularization (2-3 ngày)
**Mục tiêu:** tách `actions` theo feature + chuẩn hóa response contract.
- [x] Tách `adminActions.ts` thành nhiều file theo nghiệp vụ.
- [x] Tách `authActions.ts`, `readerActions.ts`, `chatActions.ts`, `escrowActions.ts` thành module nhỏ.
- [x] Chuẩn hóa kiểu trả về `Result<T, E>` (success/error rõ ràng).
- [x] Dùng chung `serverHttpClient` + `serverAuth`.
- [x] Để tương thích ngược: giữ file cũ làm facade re-export trong giai đoạn chuyển tiếp.

**Exit criteria:** 100% action gọi qua helper chung, giảm duplicate fetch/token.

## Phase 5 - Auth + Profile Feature Refactor (2 ngày)
**Mục tiêu:** làm mỏng các page auth/profile.
- [x] Tách logic form login/register/verify/reset vào `features/auth/application/hooks`.
- [x] Tách schema validation vào `features/auth/domain/schemas`.
- [x] Tách UI sections vào `features/auth/presentation/components`.
- [x] Refactor `profile/page.tsx` thành container mỏng + card/form nhỏ.

**Exit criteria:** mỗi page auth/profile <= ~180 dòng, không đổi UI.

## Phase 6 - Wallet + Promotions + Withdrawal Refactor (2 ngày)
**Mục tiêu:** chuẩn hóa flow tài chính phía user/admin.
- [x] Tách `wallet/page.tsx`, `wallet/deposit/page.tsx`, `wallet/withdraw/page.tsx` thành hook + component.
- [x] Dùng chung `Pagination`, `TableStates`, `formatCurrency`, `formatDateTime`.
- [x] Tách promotion logic vào `features/wallet` hoặc `features/admin/promotions` rõ ràng.
- [x] Chuẩn hóa status badge mapping.

**Exit criteria:** wallet pages mỏng hơn, không còn lặp format/status.

## Phase 7 - Reading Flow + History Refactor (3 ngày)
**Mục tiêu:** xử lý cụm file lớn nhất/rủi ro nhất.
- [x] Tách `reading/page.tsx` thành setup hook + spread cards component.
- [x] Tách `reading/session/[id]/page.tsx` (deck, animation, reveal, storage, stream trigger).
- [x] Chuyển `sessionStorage` access vào hook SSR-safe.
- [x] Tách 2 trang history thành hook dữ liệu + UI render cards/list.

**Exit criteria:** reading session page giảm mạnh độ dài, behavior giữ nguyên.

## Phase 8 - Chat + Escrow Refactor (3 ngày)
**Mục tiêu:** ổn định realtime + giảm coupling.
- [x] Tách SignalR connection lifecycle thành hook riêng.
- [x] Tách message merge/reconcile logic thành utility thuần.
- [x] Tách payment-offer/accept flow thành module application.
- [x] Refactor `EscrowPanel` và modal/report/dispute theo pattern nhất quán.

**Exit criteria:** chat room page thành thin container, dễ test logic.

## Phase 9 - Admin Module Refactor (3 ngày)
**Mục tiêu:** giảm god pages admin.
- [x] Refactor `admin/users`, `admin/deposits`, `admin/reader-requests`, `admin/readings`, `admin/withdrawals`, `admin/promotions`.
- [x] Dùng lại shared table/pagination/filter/action-modal.
- [x] Chuẩn hóa `useAdminXxx` hooks và DTO mappers.

**Exit criteria:** admin pages tách theo list/filter/modal rõ ràng.

## Phase 10 - i18n Modularization (1-2 ngày)
**Mục tiêu:** bỏ monolithic messages và cho phép feature ownership.
- [ ] Tách `messages/{locale}.json` thành file theo feature/domain.
- [ ] Tạo cơ chế merge messages trong `src/i18n/request.ts`.
- [ ] Đưa key dùng chung vào `shared/i18n/common|validation|http-errors`.
- [ ] Đảm bảo không thay key public trong phase đầu (để tránh regression).

**Exit criteria:** i18n chia theo feature nhưng output runtime không đổi.

## Phase 11 - Stabilization, Tests, Cleanup (2 ngày)
**Mục tiêu:** khóa chất lượng sau refactor.
- [x] Lint/build xanh hoàn toàn.
- [ ] Bổ sung smoke Playwright cho flow quan trọng: auth, reading, chat, wallet, admin.
- [ ] Bổ sung unit tests cho utils/hooks thuần logic.
- [ ] Xóa facade cũ khi migration hoàn tất.
- [ ] Cập nhật tài liệu cấu trúc FE mới.

**Exit criteria:** baseline mới ổn định, không regression chức năng chính.

## 9) Tiêu chí kiểm thử cho từng phase

- [x] `npm -C Frontend run lint`.
- [x] `npm -C Frontend run build`.
- [ ] Chạy smoke test Playwright cho route bị ảnh hưởng.
- [ ] Kiểm tra thủ công 5 luồng chính:
  1. Auth (login/register/verify)
  2. Reading (setup/session/history)
  3. Chat + escrow
  4. Wallet (deposit/withdraw/history)
  5. Admin (users/deposits/requests/readings/withdrawals/promotions)

## 10) KPI refactor mục tiêu (sau Phase 11)

| KPI | Hiện tại | Mục tiêu |
|---|---:|---:|
| Lint errors | 0 | 0 |
| Lint warnings | 0 | 0-2 (không critical) |
| File >200 dòng | 38 | <= 10 |
| `key={index|i}` | 0 | 0 |
| `any` explicit | 0 | 0 |
| `console.error` trực tiếp | 15 | <= 10 (qua logger) |
| `getAccessToken` alias | 8 | 1 |
| `await fetch` trong `actions` | 0 | <= 5 |
| Monolithic i18n file | 3 file lớn | chia theo feature + merge runtime |

## 11) Thứ tự ưu tiên đề xuất (thực thi)

1. Phase 0 -> 3 (chặn lỗi trước, giảm rủi ro merge).
2. Phase 4 (xương sống action/infrastructure).
3. Phase 5 -> 8 theo luồng người dùng chính.
4. Phase 9 (admin).
5. Phase 10 -> 11 để ổn định, test, cleanup.

---

## Ghi chú triển khai
- Chỉ refactor, **không đổi UI/layout**.
- Không big-bang rename/move toàn bộ trong một PR.
- Mỗi PR nên giới hạn theo 1 phase nhỏ hoặc 1 sub-feature để dễ review/rollback.
- Nếu phát hiện behavior thay đổi ngoài ý muốn, rollback về phase trước và tách nhỏ task hơn.
