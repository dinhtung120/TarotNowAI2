# Frontend Deep Review Findings

Ngày review: 2026-05-06
Phạm vi: `Frontend`
Reviewer: Claude Code + agents đọc-only theo miền

## Tóm tắt điều hành

- Tổng số vấn đề ghi nhận: 22
- P0: 0
- P1: 8
- P2: 11
- P3: 3
- Luồng rủi ro cao nhất: auth/session redirect, reading init/reveal, chat realtime unread, notifications mark-read, gacha optimistic inventory, wallet withdraw validation.
- Khuyến nghị thứ tự fix:
  1. Chặn open redirect trong auth handshake URL builder.
  2. Bịt double-submit ở reading init/reveal để tránh tạo/trừ quota nhiều lần.
  3. Sửa drift unread badge chat do realtime event chồng chéo.
  4. Chuẩn hóa contract mark-as-read notification.
  5. Sửa test middleware/proxy đang target sai implementation.
  6. Dọn cookie lifecycle/logout và stale auth UI state.
  7. Bổ sung test race/edge cho các luồng P1.

## Phạm vi đã kiểm tra

- [x] Root/config/tooling
- [x] Routing/proxy/locale
- [x] Auth/session/protected routes
- [x] API client/query/mutation/cache
- [x] Shared components/hooks/utils
- [x] Messages i18n VI/EN/ZH
- [x] Reading/tarot/AI flows
- [x] Wallet/quota/payment-related UI
- [x] Chat/realtime
- [x] Notifications
- [x] Profile/settings
- [x] Readers
- [x] Collection/inventory
- [x] Gacha/gamification/community
- [x] Admin/legal/home/layout
- [x] Tests/e2e/coverage

## Findings cần fix

### FE-001 — Open redirect qua `x-forwarded-host`/`host` trong auth handshake redirect

- Priority: P1
- Loại: Security / Auth / Routing
- File/luồng: `Frontend/src/app/api/auth/_shared/requestUrl.ts:79-106`, dùng bởi `Frontend/src/app/api/auth/session/handshake/route.ts`
- Bằng chứng:
  - `resolvePublicOrigin` đọc `x-forwarded-host` và `host` rồi dựng origin public mà không thấy allowlist host.
  - `buildPublicRequestUrl` dùng origin này để tạo URL redirect tuyệt đối.
- Tác động:
  - Nếu proxy/ingress không sanitize header, attacker có thể ép redirect auth flow sang domain lạ, tạo rủi ro phishing/open redirect.
- Hướng fix đề xuất:
  - Chỉ dùng canonical origin/`request.nextUrl.origin` đã được normalize, hoặc allowlist host rõ ràng trước khi tin `x-forwarded-host`.
  - Thêm test với `x-forwarded-host: evil.tld`.
- Verification sau fix:
  - Unit/integration test handshake redirect không chứa host ngoài allowlist.
  - Test host hợp lệ vẫn redirect đúng.

### FE-002 — `clearAuthCookies` không xóa cookie `deviceId`

- Priority: P1
- Loại: Auth / Security hygiene / Session lifecycle
- File/luồng: `Frontend/src/app/api/auth/_shared.ts:315-344`
- Bằng chứng:
  - `setDeviceCookie` set `AUTH_COOKIE.DEVICE` tại `Frontend/src/app/api/auth/_shared.ts:301-312`.
  - `clearAuthCookies` chỉ append clear cookie cho `AUTH_COOKIE.ACCESS` và `AUTH_COOKIE.REFRESH`, không clear `AUTH_COOKIE.DEVICE`.
- Tác động:
  - Logout/auth failure không reset sạch device fingerprint/session binding state.
  - Có thể gây stale behavior khó debug và giảm hygiene của auth lifecycle.
- Hướng fix đề xuất:
  - Clear thêm `AUTH_COOKIE.DEVICE` cho cả default path và cleanup domain variants.
- Verification sau fix:
  - Test logout/refresh fail response có `Set-Cookie` xóa access, refresh, device.

### FE-003 — Forward raw `user-agent` sang backend qua `x-forwarded-user-agent`

- Priority: P1
- Loại: Privacy / Logging risk / Security
- File/luồng: `Frontend/src/app/api/auth/login/route.ts:37`, `Frontend/src/app/api/auth/logout/route.ts:37`, `Frontend/src/app/api/auth/refresh/refreshRouteHandler.ts:57`
- Bằng chứng:
  - Auth routes forward raw `user-agent` client thành custom header.
- Tác động:
  - User agent là fingerprinting data; nếu backend/log pipeline ghi raw header, tăng rủi ro leak dữ liệu định danh thiết bị.
- Hướng fix đề xuất:
  - Chỉ forward nếu backend bắt buộc cần.
  - Nếu cần, sanitize/truncate/control-char-strip hoặc hash/normalize thay vì raw full string.
- Verification sau fix:
  - Contract test header bị truncate/sanitize.
  - Kiểm tra log không chứa raw UA đầy đủ.

### FE-004 — Double-submit có thể tạo nhiều reading session

- Priority: P1
- Loại: Bug logic / Finance-quota risk / Race condition
- File/luồng: `Frontend/src/features/reading/setup/useReadingSetupPage.ts:148-191`, `Frontend/src/features/reading/setup/components/ReadingSetupSubmitAction.tsx:17-26`
- Bằng chứng:
  - `submitSetup` set `isInitializing(true)` nhưng không có guard đồng bộ nếu handler bị gọi liên tiếp trước khi React re-render.
  - Button disabled phụ thuộc state async.
- Tác động:
  - Có thể tạo nhiều reading session cho một thao tác người dùng, dẫn tới trừ quota/tiền nhiều lần hoặc điều hướng session không đoán trước.
- Hướng fix đề xuất:
  - Thêm `isInitializingRef`/promise lock trong handler, hoặc chuyển sang mutation có pending guard đồng bộ.
- Verification sau fix:
  - Interaction/e2e spam click/Enter chỉ gửi 1 request `/api/reading/init`.

### FE-005 — Double reveal trong reading session modal

- Priority: P1
- Loại: Bug logic / Race condition
- File/luồng: `Frontend/src/features/reading/session/components/useRevealReading.ts:30-56`, `Frontend/src/features/reading/session/components/RevealConfirmActions.tsx:22-29`
- Bằng chứng:
  - `revealCards` không có early guard khi đang reveal; disabled UI phụ thuộc state cập nhật async.
- Tác động:
  - Có thể gửi nhiều request reveal cho cùng session, gây flip timer/state chồng chéo hoặc trạng thái backend/UI lệch.
- Hướng fix đề xuất:
  - Thêm lock/ref trong hook và disable sync trước await.
- Verification sau fix:
  - Test click liên tiếp chỉ gọi `revealReadingSession` một lần.

### FE-006 — Notification mark-read contract không đồng nhất

- Priority: P1
- Loại: Bug logic / API contract / Consistency
- File/luồng: `Frontend/src/features/notifications/dropdown/hooks/useNotificationDropdown.ts:98-109`, `Frontend/src/features/notifications/list/hooks/useNotificationMarkRead.ts:18-26`
- Bằng chứng:
  - Dropdown `markAsRead` trả `void`, trong khi hook list/caller kỳ vọng object có `result.success`.
- Tác động:
  - Dễ toast/UX báo sai trạng thái hoặc silent failure khi tái sử dụng hook theo contract không thống nhất.
- Hướng fix đề xuất:
  - Chuẩn hóa return type `{ success: boolean }` hoặc đổi caller không đọc result.
  - Đồng bộ type ở dropdown/list.
- Verification sau fix:
  - Typecheck + unit/integration test mark-read success/failure.

### FE-007 — Chat unread badge có thể cộng trùng khi nhận `message.created` và `chat.unread.delta`

- Priority: P1
- Loại: Realtime / Cache drift / Bug logic
- File/luồng: `Frontend/src/features/chat/realtime/useChatRealtimeSync.ts:313-338`
- Bằng chứng:
  - `message.created.fast`/`message.created` gọi `patchUnreadBadge(1)` nếu sender khác current user.
  - `chat.unread.delta` cũng gọi `patchUnreadBadge(payload.unreadDelta)` cho recipient hiện tại.
  - Không thấy dedup/correlation giữa hai event cho cùng message.
- Tác động:
  - Badge unread có thể tăng quá thực tế, gây lệch trạng thái unread và giảm tin cậy realtime UI.
- Hướng fix đề xuất:
  - Chọn một canonical source để mutate unread badge, ví dụ chỉ `chat.unread.delta` được cộng badge.
  - Hoặc dedup theo messageId/operationId TTL.
- Verification sau fix:
  - Realtime integration/unit test bắn cả `message.created` và `chat.unread.delta` cho cùng message, badge chỉ tăng đúng 1.

### FE-008 — Waveform voice recorder có thể sinh `NaN` với data rỗng/segment rỗng

- Priority: P1
- Loại: Bug logic / UI runtime stability
- File/luồng: `Frontend/src/features/chat/room/components/voiceRecorderHelpers.ts:28-38`
- Bằng chứng:
  - Tính `sum / ((end - start) * 255)`; khi `end === start`, mẫu số bằng 0.
- Tác động:
  - Output chứa `NaN`, có thể gây glitch render waveform hoặc class/style tính sai.
- Hướng fix đề xuất:
  - Guard denominator bằng `Math.max(1, end - start) * 255` hoặc skip segment rỗng.
- Verification sau fix:
  - Unit test `buildWaveformBars(new Uint8Array(0), n)` trả toàn số hữu hạn trong [0,1].

### FE-009 — Test middleware đang import nhầm proxy implementation

- Priority: P2
- Loại: Test gap / Architecture / Nợ kỹ thuật
- File/luồng: `Frontend/src/middleware.test.ts:2`, `Frontend/proxy.ts`, `Frontend/src/proxy.ts`
- Bằng chứng:
  - Test import `proxy` từ `../proxy`, trỏ tới `Frontend/proxy.ts`.
  - Runtime source hiện có `Frontend/src/proxy.ts` với logic khác.
- Tác động:
  - Test có thể pass nhưng không bảo vệ middleware/proxy runtime thật.
  - Regression route protection/CSP/canonical host có thể lọt.
- Hướng fix đề xuất:
  - Chỉ giữ một implementation proxy runtime.
  - Sửa test import đúng file runtime và cập nhật expected behavior.
- Verification sau fix:
  - Thay đổi có chủ đích ở `Frontend/src/proxy.ts` làm test fail/pass tương ứng.

### FE-010 — Logout redirect client hardcode `/login`, không giữ locale

- Priority: P2
- Loại: i18n / Routing / UX
- File/luồng: `Frontend/src/features/auth/session/components/AuthSessionManager.ts:109-113`
- Bằng chứng:
  - Khi terminal auth error, code push thẳng `/login` thay vì `/${locale}/login`.
  - App dùng locale prefix `always` trong routing.
- Tác động:
  - Có thể tạo redirect chain dư hoặc sai locale khi user đang ở `/zh/...`/`/en/...`.
- Hướng fix đề xuất:
  - Resolve locale hiện tại từ pathname/router và push locale-aware login path.
- Verification sau fix:
  - E2E auth expiry tại `/zh/...` redirect trực tiếp về `/zh/login`.

### FE-011 — `(site)` layout render shell auth-adjacent nhưng không hard-gate auth

- Priority: P2
- Loại: Auth UI boundary / Data leakage risk
- File/luồng: `Frontend/src/app/[locale]/(site)/layout.tsx:19-34`
- Bằng chứng:
  - Layout render `AuthBootstrap`, `AppNavbar`, `UserSidebar`, `BottomTabBar` với `sessionSnapshot` có thể unauthenticated.
  - Không hard gate như `(user)` layout.
- Tác động:
  - Nếu child/shell component quên fail-closed, UI private metadata hoặc request auth-only có thể xuất hiện cho guest.
- Hướng fix đề xuất:
  - Tách public shell/authenticated shell rõ hơn hoặc audit các shell component để không render/call private data khi unauthenticated.
- Verification sau fix:
  - UI/network test guest vào site route không thấy user-private fields và không gọi endpoint auth-only.

### FE-012 — Middleware `clearAuthCookies` dùng `secure: true` cứng

- Priority: P2
- Loại: Auth reliability / Dev parity
- File/luồng: `Frontend/src/proxy.ts:49-70`
- Bằng chứng:
  - Cookie clear trong middleware luôn set Secure, trong khi auth route shared helper có logic protocol-aware `shouldUseSecureCookie`.
- Tác động:
  - Trên HTTP dev/local, browser có thể không clear cookie Secure, gây stale auth state hoặc redirect/logout bug khó tái hiện.
- Hướng fix đề xuất:
  - Dùng cùng logic secure flag với `_shared.ts`, hoặc gom cleanup cookie về một helper thống nhất.
- Verification sau fix:
  - Manual/unit test HTTP và HTTPS đều clear cookie đúng.

### FE-013 — `ReadingFreeDrawQuotaSummary` export nhưng không thấy usage trong setup page

- Priority: P2
- Loại: Dead code / Product correctness ambiguity
- File/luồng: `Frontend/src/features/reading/setup/components/ReadingFreeDrawQuotaSummary.tsx`, `Frontend/src/features/reading/setup/components/index.ts`
- Bằng chứng:
  - Component được export nhưng không thấy được render ở `ReadingSetupPage` theo review agent.
- Tác động:
  - Nếu intended, UI đang thiếu phần quota summary.
  - Nếu không intended, đây là code thừa gây hiểu nhầm.
- Hướng fix đề xuất:
  - Xác nhận nghiệp vụ: render component hoặc xóa export/component.
- Verification sau fix:
  - Search usage rõ ràng và snapshot UI nếu render.

### FE-014 — Hard-coded copy tiếng Việt trong chat inbox

- Priority: P2
- Loại: i18n / Consistency
- File/luồng: `Frontend/src/features/chat/inbox/ChatInboxPage.tsx:15-26`
- Bằng chứng:
  - Component chứa text tiếng Việt trực tiếp thay vì `next-intl` messages.
- Tác động:
  - Locale EN/ZH không thể hiển thị copy đúng.
- Hướng fix đề xuất:
  - Chuyển copy sang `messages/{vi,en,zh}/chat/chat.json` hoặc namespace phù hợp.
- Verification sau fix:
  - Chạy locale EN/ZH và test key đồng bộ.

### FE-015 — Chat inbox query key dùng literal thay vì helper chuẩn

- Priority: P2
- Loại: Cache consistency / Nợ kỹ thuật
- File/luồng: `Frontend/src/features/chat/inbox/useChatInboxPage.ts:17`
- Bằng chứng:
  - Dùng literal `['chat', 'inbox', tab]` thay vì query key factory/canonical helper.
- Tác động:
  - Dễ drift với realtime invalidation hoặc refactor query keys.
- Hướng fix đề xuất:
  - Dùng key helper hiện có cho user-state/chat queries.
- Verification sau fix:
  - Search không còn literal duplicate; realtime invalidation vẫn update inbox.

### FE-016 — Notification cache patch dedup query key bằng `JSON.stringify`

- Priority: P2
- Loại: Cache / Performance / Robustness
- File/luồng: `Frontend/src/features/notifications/shared/notificationCache.ts:45-48`
- Bằng chứng:
  - So sánh query key bằng `JSON.stringify(candidate) === JSON.stringify(queryKey)`.
- Tác động:
  - Dễ sai nếu query key chứa object không stable order; chi phí serialize tăng khi cache lớn.
- Hướng fix đề xuất:
  - Dùng key factory/hash ổn định hoặc so sánh tuple primitive có kiểm soát.
- Verification sau fix:
  - Unit test dedup key giống/khác, gồm key có object nếu có.

### FE-017 — Import `clientFetch` không đồng nhất giữa `shared/http` và `shared/gateways`

- Priority: P2
- Loại: Architecture consistency / Nợ kỹ thuật
- File/luồng: `Frontend/src/features/gacha/shared/useGacha.ts:5`, `Frontend/src/features/reading/setup/useReadingSetupPage.ts:12`, `Frontend/src/features/notifications/dropdown/hooks/useNotificationDropdown.ts:5`
- Bằng chứng:
  - Cùng semantic fetch util nhưng feature import từ hai đường dẫn khác nhau.
- Tác động:
  - Khó enforce timeout/retry/auth policy nhất quán; tăng rủi ro dùng nhầm abstraction.
- Hướng fix đề xuất:
  - Chốt một canonical import path và migrate dần trong feature touched scope.
- Verification sau fix:
  - `rg "shared/(http|gateways)/clientFetch" Frontend/src` chỉ còn path chuẩn.

### FE-018 — Gacha optimistic inventory không append item mới chưa có trong cache

- Priority: P2
- Loại: Cache / Optimistic UI / Bug logic
- File/luồng: `Frontend/src/features/gacha/shared/usePullGacha.ts:30-71`
- Bằng chứng:
  - Patch inventory map qua items hiện có; nếu reward itemCode chưa tồn tại trong cache thì không thêm record mới.
- Tác động:
  - Sau pull nhận item mới, inventory UI có thể không thấy reward cho tới refetch/reload.
- Hướng fix đề xuất:
  - Append item tối thiểu khi reward mới hoặc invalidate inventory ngay sau success.
- Verification sau fix:
  - Test pull reward item mới chưa sở hữu, inventory hiển thị ngay hoặc refetch rõ ràng.

### FE-019 — Withdraw amount validation dùng `parseInt`, có thể nhận sai input decimal/scientific

- Priority: P2
- Loại: Form validation / Wallet UX
- File/luồng: `Frontend/src/features/wallet/withdraw/useWithdrawFormCard.ts:26-35`, `Frontend/src/features/wallet/withdraw/useWithdrawPage.ts:91`
- Bằng chứng:
  - `parseInt('1000.5')` thành `1000`, `parseInt('1e3')` thành `1`.
- Tác động:
  - User intent và amount gửi đi có thể lệch, gây UX/finance form bug.
- Hướng fix đề xuất:
  - Validate số nguyên dương bằng regex rõ ràng (`/^\d+$/`) rồi parse bằng `Number`.
- Verification sau fix:
  - Unit test các input `1000.5`, `1e3`, `0010`, whitespace.

### FE-020 — CSP `img-src` cho phép `http:`

- Priority: P3
- Loại: Security hardening
- File/luồng: `Frontend/src/proxy.ts:185`
- Bằng chứng:
  - CSP chứa `img-src 'self' data: blob: https: http:`.
- Tác động:
  - Cho phép ảnh qua HTTP trên production, tăng rủi ro mixed content/tracking không cần thiết.
- Hướng fix đề xuất:
  - Bỏ `http:` ở production hoặc dùng allowlist rõ ràng cho môi trường dev.
- Verification sau fix:
  - Test CSP header production không chứa `http:` trong `img-src`.

### FE-021 — Tồn tại hai proxy implementation khác nhau

- Priority: P3
- Loại: Dead code / Test drift / Maintainability
- File/luồng: `Frontend/proxy.ts`, `Frontend/src/proxy.ts`
- Bằng chứng:
  - Hai file cùng khái niệm proxy/middleware nhưng logic khác nhau; test hiện import root proxy.
- Tác động:
  - Dev/reviewer dễ sửa nhầm file; test drift khỏi runtime.
- Hướng fix đề xuất:
  - Xác định file runtime duy nhất, xóa hoặc đổi tên file còn lại sau khi migrate tests.
- Verification sau fix:
  - `rg "from '../proxy'|from '@/proxy'" Frontend` không còn target sai.

### FE-022 — `VoiceRecorderStartButton` có dấu hiệu không còn dùng

- Priority: P3
- Loại: Dead code / Cleanup
- File/luồng: `Frontend/src/features/chat/room/components/VoiceRecorderStartButton.tsx`
- Bằng chứng:
  - Review agent không thấy usage; luồng hiện dùng `VoiceRecorderButton` dạng menu.
- Tác động:
  - Tăng chi phí đọc code và dễ sửa nhầm component không còn render.
- Hướng fix đề xuất:
  - Xác nhận bằng `rg`, nếu không dùng thì xóa cùng export liên quan.
- Verification sau fix:
  - `rg "VoiceRecorderStartButton" Frontend/src` chỉ còn kết quả hợp lệ hoặc không còn; build/lint pass.

## Findings theo luồng tính năng

### Auth/session

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |
| FE-001 | P1 | Open redirect qua forwarded host | `Frontend/src/app/api/auth/_shared/requestUrl.ts` | Cần allowlist/canonical origin |
| FE-002 | P1 | Không clear `deviceId` cookie | `Frontend/src/app/api/auth/_shared.ts` | Logout không sạch session fingerprint |
| FE-003 | P1 | Forward raw user-agent | Auth API routes | Privacy/logging risk |
| FE-009 | P2 | Test middleware target sai proxy | `Frontend/src/middleware.test.ts` | Test pass ảo |
| FE-010 | P2 | Logout redirect không locale-aware | `AuthSessionManager.ts` | Sai locale/redirect chain |
| FE-011 | P2 | `(site)` shell không hard-gate auth | `(site)/layout.tsx` | Cần audit private UI |
| FE-012 | P2 | Clear cookie Secure cứng | `Frontend/src/proxy.ts` | Bug dev/local HTTP |
| FE-020 | P3 | CSP img-src cho phép http | `Frontend/src/proxy.ts` | Hardening |
| FE-021 | P3 | Hai proxy implementation | `Frontend/proxy.ts`, `Frontend/src/proxy.ts` | Drift/test nhầm |

### Reading/tarot/AI

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |
| FE-004 | P1 | Double-submit tạo nhiều session | `useReadingSetupPage.ts` | Có rủi ro tiền/quota |
| FE-005 | P1 | Double reveal | `useRevealReading.ts` | Request chồng |
| FE-013 | P2 | Quota summary export nhưng không dùng | `ReadingFreeDrawQuotaSummary.tsx` | Cần xác nhận intended UI |

### Wallet/quota/payment UI

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |
| FE-019 | P2 | Withdraw parseInt sai edge input | `useWithdrawFormCard.ts` | Regex số nguyên dương |

### Chat/realtime

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |
| FE-007 | P1 | Unread badge cộng trùng realtime | `useChatRealtimeSync.ts` | Cần canonical event source |
| FE-008 | P1 | Waveform có thể NaN | `voiceRecorderHelpers.ts` | Unit test data rỗng |
| FE-014 | P2 | Hard-coded VI copy | `ChatInboxPage.tsx` | i18n gap |
| FE-015 | P2 | Query key literal | `useChatInboxPage.ts` | Cache consistency |
| FE-022 | P3 | VoiceRecorderStartButton có thể dead | `VoiceRecorderStartButton.tsx` | Cleanup sau xác nhận |

### Notifications

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |
| FE-006 | P1 | Mark-read contract lệch | Notification hooks | Chuẩn hóa return type |
| FE-016 | P2 | Dedup query key bằng JSON.stringify | `notificationCache.ts` | Robustness |

### Gacha/gamification/collection/inventory

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |
| FE-018 | P2 | Optimistic inventory không append item mới | `usePullGacha.ts` | UI reward mới có thể trễ |

### Shared/API/state/tooling

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |
| FE-017 | P2 | `clientFetch` import không đồng nhất | Nhiều feature | Cần canonical path |

## Cần xác minh thêm

| Nghi vấn | Vì sao nghi ngờ | Cách xác minh | File/luồng |
| --- | --- | --- | --- |
| `AUTH_VERIFIER_POLICY` có fail-open ngoài production không | Nếu staging/preprod không set `NODE_ENV=production`, missing verifier config có thể bị nới | Kiểm tra env deploy thực tế và test fail-closed | `Frontend/src/shared/server/auth/authVerifierPolicy.ts` |
| E2E auth coverage chưa phủ open redirect/locale logout/handshake edge | Test hiện có refresh flow nhưng chưa thấy case hostile forwarded host và locale redirect | Bổ sung Playwright/integration tests theo Given/When/Then | `Frontend/tests/auth-session-refresh.spec.ts` |
| Một số findings do agent báo cần confirm bằng grep trước khi xóa | Dead export/component có thể dùng qua dynamic import hoặc barrel | Chạy `rg` trước khi cleanup | `ReadingFreeDrawQuotaSummary`, `VoiceRecorderStartButton` |

## Không ghi nhận là lỗi

| Chủ đề | Lý do không phải lỗi |
| --- | --- |
| Không có P0 được ghi nhận | Chưa có bằng chứng trực tiếp về bypass auth backend, token leak chắc chắn, hoặc mất tiền/quota đã xảy ra; các P1 là rủi ro cao cần fix sớm |
| Không tự động sửa code trong phiên review | Đây là phiên audit/tổng hợp backlog theo yêu cầu, tránh trộn review với implementation |
| Findings không đủ bằng chứng cụ thể | Được đưa vào “Cần xác minh thêm” thay vì coi là lỗi chắc chắn |

## Backlog fix đề xuất

| Thứ tự | ID | Priority | Lý do ưu tiên | Nhóm fix nên gom chung |
| --- | --- | --- | --- | --- |
| 1 | FE-001 | P1 | Security redirect trong auth flow | Auth/session security |
| 2 | FE-004 | P1 | Có thể tạo/trừ quota nhiều lần | Reading race guards |
| 3 | FE-005 | P1 | Request reveal chồng chéo | Reading race guards |
| 4 | FE-007 | P1 | Realtime badge sai gây UX drift rộng | Chat realtime/cache |
| 5 | FE-006 | P1 | Contract hook lệch, dễ silent bug | Notifications cache/hooks |
| 6 | FE-002 | P1 | Logout/session cleanup không sạch | Auth cookie lifecycle |
| 7 | FE-003 | P1 | Privacy/logging hardening | Auth forwarding headers |
| 8 | FE-008 | P1 | Runtime UI stability edge | Chat voice recorder |
| 9 | FE-009 | P2 | Test bảo vệ sai file runtime | Proxy/middleware tests |
| 10 | FE-010 | P2 | Sai locale khi auth terminal | Auth i18n routing |
| 11 | FE-018 | P2 | Reward inventory UI trễ/sai | Gacha inventory cache |
| 12 | FE-019 | P2 | Wallet withdraw input edge | Wallet form validation |
| 13 | FE-011 | P2 | Giảm nguy cơ private UI render cho guest | Shell/auth boundary |
| 14 | FE-012 | P2 | Dev parity auth cookie cleanup | Proxy cookie cleanup |
| 15 | FE-014 | P2 | Locale EN/ZH không đúng | Chat i18n |
| 16 | FE-015 | P2 | Query key drift | Chat cache consistency |
| 17 | FE-016 | P2 | Query key robustness | Notification cache |
| 18 | FE-017 | P2 | Kiến trúc fetch không đồng nhất | Shared API boundary |
| 19 | FE-013 | P2 | Code thừa hoặc UI thiếu quota summary | Reading cleanup/product confirm |
| 20 | FE-020 | P3 | Security hardening CSP | Proxy/CSP |
| 21 | FE-021 | P3 | Dọn drift proxy/test | Proxy cleanup |
| 22 | FE-022 | P3 | Dọn component dead | Chat cleanup |

## Verification đề xuất khi bắt đầu fix

- Auth/proxy: `cd Frontend && npm run lint:auth-policy && npm run test -- src/middleware.test.ts`
- Reading/chat/notifications/gacha/wallet hooks: `cd Frontend && npm run test`
- Risk-tier/frontend logic: `cd Frontend && npm run test:coverage:risk`
- Full frontend gate sau khi gom fix: `cd Frontend && npm run lint && npm run build`
- UI/race behavior: chạy Playwright/manual browser flow cho reading init/reveal, auth logout locale, chat unread realtime, notification mark-read.
