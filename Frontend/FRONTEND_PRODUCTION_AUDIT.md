# Frontend Production Audit — TarotNowAI2

Ngày audit: 2026-04-27  
Phạm vi: `src/app`, `src/features`, `src/shared`, `src/store`, `src/components`, `next.config.ts`, `eslint.config.mjs`, `tsconfig.json`, `vitest.config.ts`, `tests/*`, `.github/workflows/*`

## 1. Executive Summary

### Chất lượng frontend tổng thể
- Mức độ sẵn sàng production hiện tại: **No-Go**.
- Khu vực nguy hiểm nhất: **Auth/session refresh flow**, **CI/E2E reliability**, **Chat realtime lifecycle + media recorder cleanup**, **kiến trúc state/query phân tán**.
- Độ ổn định production: có thể chạy trong điều kiện bình thường, nhưng có rủi ro cao về logout bất thường, test pipeline không đáng tin cậy, và debt lớn trong các hook/feature phức tạp.

### Baseline định lượng
| Hạng mục | Kết quả |
| --- | --- |
| Tổng file TS/TSX | `1020` |
| Tổng route handler BFF (`src/app/api/**/*.ts`) | `24` |
| Lint | Pass (`npm run lint`) |
| Unit tests | `103 passed / 103` (`npm test`) |
| Coverage (Vitest) | Statements `68.29%`, Branch `56.2%`, Functions `71.6%`, Lines `70.25%` |
| Dead code scan (`knip`) | `26` unused files, `1` unused dependency (`uuid`), `31` unused exports, `36` unused exported types |
| E2E smoke run thực tế | Fail do `ECONNREFUSED 127.0.0.1:3100` khi chạy `npx playwright test tests/example.spec.ts --project=chromium` |

### Severity distribution
| Priority | Số lượng issue |
| --- | ---: |
| Critical | 4 |
| High | 8 |
| Medium | 9 |
| Low | 3 |
| **Tổng** | **24** |

### Heatmap rủi ro theo module
| Module | Risk | Vấn đề chính |
| --- | --- | --- |
| Auth / Session | Critical | Refresh rotation, refresh cadence, global throttle state |
| Testing / CI | Critical | Workflow không boot app, benchmark đánh vào production, test không assert |
| Chat / Realtime | High | Loading state treo tiềm ẩn, recorder leak, global reconnect block |
| Wallet | High | Balance không sync sau withdraw, custom store debt |
| API/BFF Layer | High | Status mapping sai (`400` hóa mọi lỗi), ProblemDetails bị copy-paste |
| Community / Notifications / Reader | Medium | Race condition, cache/query consistency, UX pagination |
| UI/A11y/Styling | Medium | Icon button thiếu aria-label, class debt, i18n hardcoded text |

---

## 2. Architecture Review

### Cấu trúc folder / layer separation / dependency direction
- Dự án đang tổ chức theo feature + shared tương đối rõ, nhưng dependency direction đang lệch chuẩn Clean Architecture đã định nghĩa.
- Nhiều `application/actions` gọi thẳng `shared/infrastructure/http/serverHttpClient`, khiến layer application phụ thuộc infrastructure.
- Server state đang đi qua cả React Query lẫn custom store global (`authStore`, `walletStore`) gây split source-of-truth.

### Architecture issues

**ISSUE-03 — Server State Split + Store Re-render Debt**
- File: `src/store/authStore.ts:121-124`, `src/store/walletStore.ts:136-140`, `src/store/walletStore.ts:138`
- Component / Hook / Function: `useAuthStore`, `useWalletStore`
- Loại issue: Architecture problem / Performance / State management
- Mô tả: Store custom dùng `useSyncExternalStore` trả snapshot object mới mỗi lần update; selector không có equality check. `walletStore` còn dùng `getWalletSnapshot` cho cả server snapshot.
- Root cause: Tự triển khai store global mutable thay vì dùng React Query/Zustand selector semantics cho server state.
- Impact: Re-render thừa trên toàn bộ subscriber, tăng CPU khi realtime cập nhật; nguy cơ lệch SSR snapshot ở wallet store.
- Cách fix: Chuyển server state (`auth`, `wallet`) về React Query + hydration; nếu giữ store thì dùng selector + equality function và tách `getServerSnapshot` hằng số bất biến.
- Priority: High

**ISSUE-06 — Vi phạm hướng phụ thuộc Clean Architecture ở FE Application Layer**
- File: `src/features/chat/application/actions/conversations.core.ts:1-5`, `src/features/profile/application/actions/get-profile.ts:4-7`, `src/features/wallet/application/actions/withdrawal/reader.ts:5-8`
- Component / Hook / Function: nhiều `*Action` trong `features/*/application/actions`
- Loại issue: Architecture problem / Technical debt
- Mô tả: Layer `application` import trực tiếp `shared/infrastructure/*` để gọi HTTP.
- Root cause: Chưa có abstraction boundary (gateway/port) giữa application và infrastructure ở frontend BFF layer.
- Impact: Coupling cao, khó thay transport, khó test độc lập application logic.
- Cách fix: Tạo interface gateway ở `application` và implementation ở `infrastructure`; inject qua factory/composition root.
- Priority: High

**ISSUE-07 — Mandatory Event-Driven Architecture: không đủ bằng chứng tuân thủ từ FE contracts**
- File: `src/features/chat/application/actions/conversations.core.ts:10-95`, `src/features/wallet/application/actions/withdrawal/reader.ts:24-39`, `src/shared/infrastructure/auth/serverAuth.ts:168-178`
- Component / Hook / Function: BFF `serverHttpRequest` calls cho command endpoints
- Loại issue: Architecture compliance gap (Needs backend evidence)
- Mô tả: FE/BFF gọi trực tiếp command endpoints nhưng không có contract nào xác nhận side-effects được xử lý qua Domain Events theo chuẩn bắt buộc.
- Root cause: Thiếu event-audit contract test/telemetry trong boundary FE↔BE.
- Impact: Có khả năng vi phạm kiến trúc event-driven mà FE không phát hiện được; rủi ro kiến trúc cấp hệ thống.
- Cách fix: Bổ sung contract test bắt buộc event emission (`MoneyChangedEvent`, AI quota/refund events...) và expose trace/audit endpoint cho QA.
- Priority: Critical

**ISSUE-18 — `buildProblemResponse` bị copy-paste diện rộng**
- File: `src/app/api/**/route.ts` (19 bản sao), ví dụ `src/app/api/notifications/route.ts:15-25`, `src/app/api/wallet/ledger/route.ts:15-25`
- Component / Hook / Function: `buildProblemResponse`
- Loại issue: Code smell / Maintainability
- Mô tả: Cùng một helper ProblemDetails bị nhân bản thủ công ở nhiều route.
- Root cause: Chưa chuẩn hóa error response helper ở shared layer.
- Impact: Dễ lệch format error, tăng chi phí bảo trì, tăng khả năng bug khi đổi chuẩn response.
- Cách fix: Trích helper dùng chung (`shared/infrastructure/error/problemDetails.ts`) và enforce qua lint rule/codemod.
- Priority: Medium

---

## 3. Feature-by-feature Review

### Wave P0 — Auth / Wallet / Chat / Reading

**ISSUE-01 — Refresh token rotation risk do server-side refresh không propagate cookie**
- File: `src/shared/infrastructure/auth/serverAuth.ts:162-189`, `src/shared/infrastructure/auth/serverAuth.ts:195-219`
- Component / Hook / Function: `refreshServerAccessToken`, `getServerAccessTokenOrRefresh`
- Loại issue: Security risk / Authentication flow
- Mô tả: Server-side refresh chỉ lấy `accessToken` từ upstream response, không copy `Set-Cookie` refresh token rotation về client cookie.
- Root cause: Refresh flow ở `serverAuth` dùng `serverHttpRequest` rồi bỏ qua refresh cookie metadata.
- Impact: Nếu backend bật refresh rotation đúng chuẩn, cookie refresh ở browser có thể stale → logout bất thường, replay/token invalid chain.
- Cách fix: Không refresh trực tiếp trong `serverAuth`; thay bằng gọi `/api/auth/refresh` nội bộ và append `set-cookie`, hoặc parse/copy upstream `set-cookie` bắt buộc.
- Priority: Critical

**ISSUE-02 — Refresh cadence lệch với TTL + global throttle quá rộng**
- File: `src/shared/components/auth/AuthSessionManager.ts:20-22`, `src/shared/components/auth/AuthSessionManager.ts:73-80`, `src/shared/components/auth/AuthSessionManager.ts:138-145`, `src/shared/infrastructure/auth/authConstants.ts:14-16`
- Component / Hook / Function: `AuthSessionManager` (`tryRefresh`)
- Loại issue: Async flow / Session stability
- Mô tả: Refresh interval cố định `40m`, throttle `20m`, trong khi default access TTL là `600s` (10m).
- Root cause: Refresh schedule hard-code, không dựa vào `expiresInSeconds` thực tế.
- Impact: Có cửa sổ token hết hạn dài; request 401 tăng; trạng thái đăng nhập thiếu ổn định.
- Cách fix: Tính lịch refresh theo `expiresInSeconds` (ví dụ refresh trước hạn 1-2 phút), throttle theo exponential backoff ngắn thay vì khóa cứng 20 phút.
- Priority: High

**ISSUE-11 — Withdraw thành công nhưng không đồng bộ balance ngay**
- File: `src/features/wallet/application/useWithdrawPage.ts:127-131`
- Component / Hook / Function: `useWithdrawPage` (`handleSubmit`)
- Loại issue: UX issue / Data consistency
- Mô tả: Sau khi tạo withdrawal, hook chỉ invalidate lịch sử rút tiền, không invalidate/fetch balance.
- Root cause: Thiếu bước đồng bộ state wallet sau mutation side-effect lên số dư.
- Impact: User thấy số dư cũ sau khi gửi lệnh rút, gây sai kỳ vọng và dễ thao tác lặp.
- Cách fix: Thêm `invalidateQueries(userStateQueryKeys.wallet.balance())` và `useWalletStore.getState().fetchBalance()` sau thành công.
- Priority: High

**ISSUE-09 — `useChatHistoryState.loadInitial` thiếu `try/finally` bảo toàn loading state**
- File: `src/features/chat/application/chat-connection/useChatHistoryState.ts:35-63`
- Component / Hook / Function: `useChatHistoryState` (`loadInitial`)
- Loại issue: React async bug / Loading state
- Mô tả: `setLoading(true)` được bật trước request, nhưng không có `finally` để đảm bảo `setLoading(false)` khi throw bất thường.
- Root cause: Async flow chỉ xử lý nhánh `result.success`/`!success`, thiếu guard cho exception path.
- Impact: UI có thể treo loading/initializing vĩnh viễn trong lỗi runtime/network edge case.
- Cách fix: Wrap `listMessages` trong `try/catch/finally`, luôn reset `loading` và `initializing` trong `finally`.
- Priority: High

**ISSUE-10 — Voice recorder thiếu cleanup khi unmount**
- File: `src/features/chat/application/useVoiceRecorder.ts:29-37`, `src/features/chat/application/useVoiceRecorder.ts:14-98`
- Component / Hook / Function: `useVoiceRecorder`
- Loại issue: Memory leak / Media resource leak
- Mô tả: Hook có `cleanup()` nhưng không gắn vào unmount lifecycle (`useEffect` return).
- Root cause: Cleanup chỉ gọi khi user cancel/stop; không đảm bảo khi component bị unmount đột ngột.
- Impact: Microphone stream/timer có thể còn chạy sau unmount; hao tài nguyên và rủi ro privacy.
- Cách fix: Thêm `useEffect(() => () => cleanup(), [cleanup])` và đảm bảo idempotent cleanup.
- Priority: High

**ISSUE-17 — Cooldown reconnect dùng biến module-scope**
- File: `src/shared/application/hooks/usePresenceConnection.ts:14`, `src/shared/application/hooks/useChatRealtimeSync.ts:24`
- Component / Hook / Function: `usePresenceConnection`, `useChatRealtimeSync`
- Loại issue: Race condition / Reconnect behavior
- Mô tả: `reconnectBlockedUntil` và `unauthorizedRetryBlockedUntil` là biến global của module.
- Root cause: Cooldown state không được scope theo connection instance/user session.
- Impact: Một failure có thể chặn reconnect không mong muốn cho flow tiếp theo trong cùng runtime tab.
- Cách fix: Đưa cooldown vào `useRef` instance-level hoặc connection manager keyed theo user/session.
- Priority: Medium

**ISSUE-24 — Stream route log quá chi tiết ở production path**
- File: `src/app/[locale]/api/reading/sessions/[sessionId]/stream/route.ts:72-76`, `:81-83`, `:110-114`, `:123`
- Component / Hook / Function: `GET` stream route
- Loại issue: Observability hygiene / Security hardening
- Mô tả: Route log nhiều message `console.log/error/warn` theo request path/session stream.
- Root cause: Debug log để lại trong runtime handler.
- Impact: Noise log lớn, khó truy vết sự cố thật; tăng nguy cơ lộ thông tin vận hành.
- Cách fix: Thay bằng logger có level + redact + sampling; tắt debug log ở production.
- Priority: Low

---

### Wave P1 — Admin / Reader / Profile / Community / Notifications / Gamification

**ISSUE-08 — Reader API mapping sai HTTP status (mọi lỗi trả 400)**
- File: `src/app/api/readers/route.ts:29-32`, `src/app/api/readers/[id]/route.ts:31-34`
- Component / Hook / Function: `GET /api/readers`, `GET /api/readers/[id]`
- Loại issue: API layer / Error handling
- Mô tả: Khi action fail (kể cả lỗi backend/internal), route luôn trả `400`.
- Root cause: `ActionResult` không mang HTTP status và route hard-code `400`.
- Impact: Monitoring sai severity, client retry/backoff sai, khó phân biệt lỗi người dùng vs lỗi hệ thống.
- Cách fix: Mở rộng `ActionResult` có status/errorCode hoặc map lỗi chuẩn sang 4xx/5xx rõ ràng.
- Priority: High

**ISSUE-13 — `useUseItem` quá lớn + mutation trực tiếp input payload**
- File: `src/shared/infrastructure/inventory/useUseItem.ts:348-418`, `src/shared/infrastructure/inventory/useUseItem.ts:387`
- Component / Hook / Function: `useUseItem` (`mutationFn`)
- Loại issue: Code smell / Side-effect risk
- Mô tả: Hook 418 dòng, trộn normalization + optimistic update + invalidation + domain patching; `mutationFn` sửa trực tiếp `payload.idempotencyKey`.
- Root cause: Thiếu tách use-case/use-helper và immutability discipline.
- Impact: Khó test, khó maintain, dễ tạo bug side-effect khi caller tái sử dụng object.
- Cách fix: Tách thành các module nhỏ (serializer, optimistic handler, invalidation planner), không mutate input object.
- Priority: Medium

**ISSUE-14 — Notification dropdown optimistic unread count có thể lệch**
- File: `src/features/notifications/application/useNotificationDropdown.ts:99-112`
- Component / Hook / Function: `markAsRead`
- Loại issue: Cache invalidation / Optimistic update bug
- Mô tả: `unreadCount` luôn giảm `-1` không kiểm tra item trước đó đã `isRead` chưa.
- Root cause: Thiếu guard dựa trên state trước mutation.
- Impact: Badge unread có thể lệch âm/thiếu chính xác nếu click lặp hoặc data stale.
- Cách fix: Giảm count có điều kiện khi trạng thái cũ là unread; fallback refetch sau mutate.
- Priority: Medium

**ISSUE-15 — Reader directory không reset page khi đổi filter/search**
- File: `src/features/reader/application/useReadersDirectoryPage.ts:20-24`, `:35-52`
- Component / Hook / Function: `useReadersDirectoryPage`
- Loại issue: UX issue / Pagination consistency
- Mô tả: `page` giữ nguyên khi đổi `selectedSpecialty`, `selectedStatus`, `searchTerm`.
- Root cause: Không có effect `setPage(1)` theo filter state.
- Impact: User dễ rơi vào page rỗng dù có dữ liệu ở page đầu.
- Cách fix: Reset `page=1` khi search/filter thay đổi.
- Priority: Medium

**ISSUE-16 — Community image upload có race + timeout cleanup thiếu**
- File: `src/features/community/hooks/useCommunityImageUpload.ts:35-38`, `:91`
- Component / Hook / Function: `useCommunityImageUpload` (`uploadImage`)
- Loại issue: Async bug / Memory leak
- Mô tả: Guard `isUploading` dựa state có thể race khi trigger nhanh; `setTimeout` reset progress không được clear khi unmount.
- Root cause: Dùng state cho lock thay vì ref atomic lock; thiếu lifecycle cleanup timeout.
- Impact: Có thể chạy nhiều upload song song ngoài ý muốn; warning state update after unmount.
- Cách fix: Dùng `inFlightRef` lock đồng bộ, lưu timer ref và clear trong cleanup.
- Priority: Medium

**ISSUE-22 — Icon-only buttons thiếu `aria-label`**
- File: `src/components/ui/inventory/UseItemQuantitySelector.tsx:51-77`, `src/features/chat/presentation/components/voice-recorder/VoiceRecorderStartButton.tsx:12-20`, `src/features/chat/presentation/components/voice-recorder/VoiceRecorderActiveInline.tsx:44-67`, `src/features/chat/presentation/components/voice-recorder/VoiceRecorderErrorInline.tsx:15-22`
- Component / Hook / Function: nhiều icon action button
- Loại issue: Accessibility issue
- Mô tả: Nhiều nút chỉ có icon, dùng `title` nhưng không có `aria-label` rõ ràng.
- Root cause: A11y semantics chưa được enforce đồng nhất trong reusable button patterns.
- Impact: Trợ năng screen reader đọc không đủ ngữ nghĩa thao tác.
- Cách fix: Thêm `aria-label`/`aria-describedby`, chuẩn hóa icon-button component có contract a11y bắt buộc.
- Priority: Low

**ISSUE-23 — Styling debt (arbitrary values dày đặc) + i18n hardcoded text**
- File: `src/components/ui/inventory/InventoryItemCard.tsx:19-22`, `src/components/ui/inventory/UseItemQuantitySelector.tsx:85`, `src/features/community/hooks/useCommunityImageUpload.ts:37-45`
- Component / Hook / Function: nhiều UI components/hook messages
- Loại issue: Styling consistency / Localization debt
- Mô tả: Tailwind arbitrary value lặp nhiều (`shadow-[...]`, `rounded-[...]`, `h-[...]`) và có text hardcoded tiếng Việt ngoài i18n.
- Root cause: Thiếu quy tắc trích variant/token và thiếu gate i18n lint.
- Impact: UI khó maintain/theme, đa ngôn ngữ thiếu nhất quán.
- Cách fix: Trích variant component (`src/ui`), giảm arbitrary values, đưa toàn bộ user-facing strings vào `next-intl`.
- Priority: Low

---

### Wave P2 — Home / Legal / Collection / Inventory / Gacha / Checkin

**ISSUE-12 — Guard kích thước component không bao phủ toàn codebase**
- File: `scripts/check-component-size.mjs:5-14`, `src/features/community/components/PostReportModal.tsx` (171 lines), `src/features/reader/presentation/components/readers-directory/ReaderDetailModal.tsx` (123 lines)
- Component / Hook / Function: component-size lint guard
- Loại issue: Code style enforcement gap
- Mô tả: Rule `<=120 lines` chỉ kiểm tra 8 file hard-coded; nhiều file lớn ngoài danh sách không bị fail CI/lint.
- Root cause: Script kiểm tra whitelist thay vì scan toàn bộ `*.tsx`.
- Impact: Component phình to tiếp tục lọt qua gate, tăng coupling và regression risk.
- Cách fix: Đổi sang scan toàn project theo glob + allowlist explicit exception ngắn hạn.
- Priority: High

**ISSUE-21 — Dead code/dependency debt tích tụ**
- File: `package.json` (`uuid`), nhiều file theo kết quả `knip` (ví dụ `src/features/wallet/presentation/components/deposit/*`, `src/shared/components/auth/MfaChallengeModal.tsx`)
- Component / Hook / Function: nhiều export/file không dùng
- Loại issue: Dead code / Bundle hygiene
- Mô tả: `knip` báo `26` file không dùng, `1` dependency không dùng, `31` unused exports, `36` unused exported types.
- Root cause: Refactor chưa dọn dẹp + không có gate dead-code trong CI.
- Impact: Tăng cognitive load, tăng nguy cơ import nhầm code cũ, ảnh hưởng bundle/time-to-understand.
- Cách fix: Dọn dead code theo batch, bật `knip` ở CI với baseline allowlist.
- Priority: Medium

---

## 4. ReactJS Specific Problems

### useEffect / lifecycle / stale closure / async
- `ISSUE-02`: refresh cadence/throttle hard-coded (`AuthSessionManager`) gây stale auth behavior.
- `ISSUE-09`: async `loadInitial` thiếu `finally` có thể kẹt state loading.
- `ISSUE-10`: `useVoiceRecorder` không cleanup khi unmount.
- `ISSUE-16`: timeout cleanup thiếu trong image upload.
- `ISSUE-17`: reconnect cooldown module-scope tạo side-effect xuyên vòng đời.

### render behavior / hooks design
- `ISSUE-03`: custom store selector không có equality → re-render thừa.
- `ISSUE-13`: hook `useUseItem` quá lớn, trộn nhiều concern, khó kiểm soát regression.
- `ISSUE-12`: file component quá ngưỡng chuẩn, vi phạm tiêu chí tách nhỏ logic/UI.

---

## 5. Performance Audit

### Findings
- `ISSUE-03` (High): auth/wallet store snapshot pattern gây render fan-out.
- `ISSUE-13` (Medium): `useUseItem` monolithic làm tăng chi phí render/debug ở inventory flow.
- `ISSUE-21` (Medium): dead code/dependency tăng overhead bundle và parse time.
- `ISSUE-23` (Low): class strings phức tạp/arbitrary values dày đặc làm giảm maintainability hiệu năng styling.

### Bằng chứng định lượng
- Coverage core auth/http thấp: `deviceId.ts 8.33%`, `refreshClient.ts 20%`, `apiUrl.ts 13.04%`.
- `knip`: `26` unused files, `1` unused dependency.
- File lớn vượt chuẩn component: `171` và `123` lines.

---

## 6. Security Audit

### Findings

**ISSUE-04 — CI E2E không có app bootstrap nên pipeline reliability thấp**
- File: `.github/workflows/playwright.yml:16-21`, `playwright.config.ts:9-74`
- Component / Hook / Function: Playwright CI job
- Loại issue: Security process / Quality gate failure
- Mô tả: Workflow chạy Playwright trực tiếp, không có `webServer` hoặc bước start app.
- Root cause: CI config thiếu environment bootstrapping.
- Impact: Security regression có thể lọt do test gate không phản ánh thực tế.
- Cách fix: Build/start app trong job hoặc dùng `webServer` trong Playwright config; fail-fast khi service unavailable.
- Priority: Critical

**ISSUE-05 — Benchmark test dùng production URL + credential default hardcoded**
- File: `tests/tarotnow-navigation-benchmark.spec.ts:76-82`, `:460-466`, `:507`
- Component / Hook / Function: `loginAsBenchmarkUser`, benchmark scenario
- Loại issue: Security risk / Operational risk
- Mô tả: Base origin mặc định là `https://www.tarotnow.xyz` và có default username/password trong source.
- Root cause: Perf benchmark script thiết kế chạy production-by-default.
- Impact: Rò rỉ credential, vô tình tạo traffic/side-effect lên production từ CI/local.
- Cách fix: Bắt buộc env vars (không default), chặn host production trong test code trừ khi cờ explicit, dùng secret manager.
- Priority: Critical

**ISSUE-01 — Refresh rotation chain risk**
- File: `src/shared/infrastructure/auth/serverAuth.ts:162-219`
- Component / Hook / Function: `refreshServerAccessToken`, `getServerAccessTokenOrRefresh`
- Loại issue: Authentication security
- Mô tả: Server-side refresh không propagate refresh cookie mới.
- Root cause: Bỏ qua `Set-Cookie` của refresh response.
- Impact: Token lifecycle không ổn định, tăng nguy cơ unauthorized loop.
- Cách fix: Đồng bộ refresh qua route xử lý cookie chuẩn.
- Priority: Critical

**ISSUE-07 — Event-driven compliance chưa có bằng chứng**
- File: `src/features/*/application/actions/*.ts`, `src/shared/infrastructure/auth/serverAuth.ts`
- Component / Hook / Function: command-side BFF flows
- Loại issue: Architecture-security compliance
- Mô tả: Chưa có bằng chứng side-effects backend đi qua Domain Events như chuẩn bắt buộc.
- Root cause: Thiếu contract audit/event trace.
- Impact: Có thể tồn tại direct side-effect bypass mà FE không phát hiện.
- Cách fix: Contract tests + backend event telemetry.
- Priority: Critical

---

## 7. UX / Accessibility Review

### Findings
- `ISSUE-11` (High): số dư ví không cập nhật ngay sau withdraw thành công.
- `ISSUE-15` (Medium): đổi filter/search ở reader directory không reset trang.
- `ISSUE-22` (Low): icon-only button thiếu `aria-label`.
- `ISSUE-23` (Low): text hardcoded ngoài i18n gây UX đa ngôn ngữ không nhất quán.

---

## 8. Code Smell / Dead Code

### Findings
- `ISSUE-12` (High): component-size rule enforced không đầy đủ.
- `ISSUE-13` (Medium): hook `useUseItem` phình to + mutate input.
- `ISSUE-18` (Medium): ProblemDetails helper copy-paste 19 nơi.
- `ISSUE-21` (Medium): dead files/exports/dependency.

---

## 9. Testing Gap

**ISSUE-19 — Coverage yếu ở auth/http core**
- File: coverage report (`npm test -- --coverage`)
- Component / Hook / Function: `deviceId.ts`, `refreshClient.ts`, `apiUrl.ts`
- Loại issue: Testing gap
- Mô tả: Các module nhạy cảm auth/session/url có coverage rất thấp.
- Root cause: Unit tests chưa bao phủ nhánh lỗi/retry/edge-case auth.
- Impact: Bug auth/session dễ lọt production.
- Cách fix: Ưu tiên test matrix cho refresh race, cookie parsing, url resolution, token expiry paths.
- Priority: Medium

**ISSUE-20 — Viewport QA test không có assertion hard-fail**
- File: `tests/viewport-qa.spec.ts:15-112`
- Component / Hook / Function: `viewport QA (mobile/tablet/desktop)`
- Loại issue: Testing gap / False confidence
- Mô tả: Test chỉ ghi report JSON, không có `expect` để fail khi overflow/error nghiêm trọng.
- Root cause: Thiết kế test thiên thu thập metrics, thiếu quality gate.
- Impact: Regression UX/responsive vẫn pass CI.
- Cách fix: Thêm ngưỡng fail cho `fatalError`, `hasHorizontalOverflow`, `pageErrors`, `failedRequests`.
- Priority: Medium

**ISSUE-04 (evidence run)**
- File: `tests/example.spec.ts:3-14`, `.github/workflows/playwright.yml:16-21`
- Component / Hook / Function: smoke tests
- Loại issue: CI pipeline reliability
- Mô tả: Chạy thật `npx playwright test tests/example.spec.ts --project=chromium` fail `ECONNREFUSED 127.0.0.1:3100`.
- Root cause: Workflow không start app/webServer.
- Impact: CI test signal không tin cậy.
- Cách fix: Bổ sung startup app trong workflow hoặc Playwright `webServer`.
- Priority: Critical

### Khoảng trống integration/E2E theo feature
- Chưa có E2E assert mạnh cho: withdraw/deposit success flow, realtime chat lifecycle, inventory use-item optimistic rollback, admin moderation flows, notification optimistic consistency.
- Chưa có contract test FE↔BE cho event emission requirements (`MoneyChangedEvent`, quota reserve/refund events, idempotency replay semantics).

---

## 10. Refactor Priority Roadmap

### Sửa ngay (P0 - blocking release)
1. Fix refresh rotation + cookie propagation trong server-side auth flow (`ISSUE-01`).
2. Sửa CI Playwright pipeline để boot app thật + fail khi service down (`ISSUE-04`).
3. Loại bỏ production default + hardcoded benchmark credentials (`ISSUE-05`).
4. Chốt cơ chế backend evidence cho event-driven compliance (`ISSUE-07`).

### Nên cải thiện sớm (P1)
1. Rework auth refresh scheduler/throttle theo token TTL thực (`ISSUE-02`).
2. Cleanup lifecycle cho voice recorder và chat loading safety (`ISSUE-09`, `ISSUE-10`).
3. Đồng bộ wallet balance ngay sau withdrawal (`ISSUE-11`).
4. Sửa status mapping readers API (`ISSUE-08`).
5. Chuẩn hóa server-state ownership (React Query vs custom store) (`ISSUE-03`).
6. Bật component-size enforcement toàn project (`ISSUE-12`).

### Backlog kỹ thuật (P2)
1. Tách nhỏ `useUseItem`, bỏ input mutation (`ISSUE-13`).
2. Chuẩn hóa query key governance (`ISSUE-14`, `ISSUE-15`, `ISSUE-17`).
3. Xóa dead code/dependency + centralize ProblemDetails (`ISSUE-18`, `ISSUE-21`).
4. Nâng a11y/i18n/styling consistency (`ISSUE-22`, `ISSUE-23`, `ISSUE-24`).
5. Tăng coverage + biến metric tests thành quality gates (`ISSUE-19`, `ISSUE-20`).

---

## 11. Final Verdict

- **Có nên release không:** **Không nên release ở trạng thái hiện tại (No-Go)**.
- **Rủi ro lớn nhất:** Auth/session refresh chain + CI/E2E gate không đáng tin cậy + benchmark script nguy cơ tác động production.
- **Confidence level:** **0.86** (cao cho code-level findings; trung bình cho event-driven backend compliance vì thiếu source backend trong phạm vi audit).
