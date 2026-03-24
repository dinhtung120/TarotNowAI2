# FE Audit Report – TarotNowAI2

- **Project audited**: `Frontend/`
- **Date**: 2026-03-24
- **Scope**: Frontend architecture, FE design system, folder/file organization, naming, server-client separation, duplication, technical debt, security, maintainability, testability.
- **Audit mode**: Static code audit + build/lint/audit commands.

## 1) Executive Summary

### Tổng quan nhanh

Frontend đang ở trạng thái **refactor lớn theo hướng feature-first** và đã có nhiều nền tảng tốt:

- App Router tách route entrypoint mỏng (`app/[locale]/* -> features/*`) rõ ràng.
- Có layering tương đối rõ (`features`, `shared`, `store`, `i18n`).
- `eslint` và `next build` hiện **pass**.

Tuy nhiên, có các vấn đề quan trọng cần ưu tiên xử lý, đặc biệt về **security** và **consistency**:

- **[Critical] Access token đang lộ ở client** (cookie `httpOnly: false`, persist vào localStorage, và đưa vào URL query khi SSE).
- **[High] Wallet state có nguy cơ hiển thị sai giữa các phiên đăng nhập** (không reset wallet store khi logout + cơ chế fetch một lần).
- **[High] Dùng Next.js 16.1.6 đang có advisory bảo mật** theo `npm audit`.
- **[Medium] i18n thiếu key giữa các locale**, nhiều hardcoded string.
- **[Medium] Action layer chưa chuẩn hóa response contract**, lặp logic nhiều.

### Điểm số đánh giá (tham khảo)

- Kiến trúc tổng thể FE: **7.5/10**
- Tổ chức folder/file: **8/10**
- Naming consistency: **7/10**
- Server-client separation: **7/10**
- Code duplication/control: **6.5/10**
- Security frontend/BFF boundary: **5.5/10**
- Maintainability/technical debt: **6.5/10**
- Test readiness: **5.5/10**

---

## 2) Method & Commands Executed

Đã chạy các lệnh chính:

- `npm run lint` (Frontend): **PASS**
- `npm run build` (Frontend): **PASS**
- `npm audit --omit=dev --json`:
  - 1 vulnerability mức **moderate** trên `next`.
- `npm audit --json`:
  - thêm 1 vulnerability mức **high** ở `flatted` (dev dependency/transitive).

Các kết quả kỹ thuật đáng chú ý:

- FE source quy mô ~`21,369` dòng (TS/TSX/CSS)
- `Frontend/src/app/globals.css` dài `2,525` dòng (rất lớn, central critical file).
- Nhiều page component >250-600 dòng (độ phức tạp UI cao).

---

## 3) Findings by Severity

## 3.1 Critical

### C1. Access token bị lộ ở client-side (XSS blast radius cao)

**Evidence**

- Set cookie access token với `httpOnly: false`:
  - `Frontend/src/features/auth/application/actions/session.ts:69`
  - `Frontend/src/features/auth/application/actions/session.ts:142`
- Lưu access token vào localStorage qua zustand persist:
  - `Frontend/src/store/authStore.ts:65`
  - `Frontend/src/store/authStore.ts:68`
- Client có hàm đọc trực tiếp token từ `document.cookie`:
  - `Frontend/src/lib/auth-client.ts:15`
- Token được gắn vào query string SSE URL:
  - `Frontend/src/features/reading/presentation/components/AiInterpretationStream.tsx:138`

**Risk**

- Nếu có XSS, attacker lấy được access token dễ dàng.
- Query token có thể bị lộ qua logs/proxy/history/referrer hạ tầng.
- Kết hợp localStorage persist + cookie readable làm bề mặt tấn công lớn.

**Recommendation**

- Chuyển hoàn toàn access token sang `httpOnly` cookie.
- Không persist access token ở localStorage.
- Tránh truyền token qua query string (SSE nên đi qua BFF route/proxy nội bộ nếu backend không hỗ trợ header).
- Ưu tiên defense-in-depth: CSP strict, encode output, sanitize inputs.

---

## 3.2 High

### H1. Phiên bản Next đang có advisory bảo mật

**Evidence**

- `Frontend/package.json:15` đang dùng `next: 16.1.6`.
- `npm audit` báo các advisory (request smuggling, CSRF bypass trong Server Actions check null origin, unbounded buffering/cache growth) ảnh hưởng dải `<16.1.7`.

**Risk**

- Ảnh hưởng trực tiếp nền tảng web runtime, đặc biệt với app đang dùng nhiều server actions.

**Recommendation**

- Nâng ngay lên bản vá an toàn (`>=16.1.7`, tốt nhất latest stable đã test).
- Re-run full regression (auth, actions, middleware/proxy, i18n routing).

---

### H2. Wallet state có nguy cơ stale/cross-account hiển thị sai

**Evidence**

- Wallet chỉ fetch 1 lần theo flag `hasFetched`:
  - `Frontend/src/shared/components/common/WalletWidget.tsx:46`
  - `Frontend/src/shared/components/common/WalletWidget.tsx:49`
- Logout chỉ clear auth store, không reset wallet store:
  - `Frontend/src/shared/components/common/Navbar.tsx:116`
- Wallet store không có action reset state khi logout:
  - `Frontend/src/store/walletStore.ts:32` (state init), không có reset action.

**Risk**

- Trong cùng browser session: logout/login tài khoản khác có thể thấy balance cũ một thời gian.
- Dù là UI leak ngắn hạn, vẫn là thông tin nhạy cảm.

**Recommendation**

- Thêm `resetWalletState()` vào walletStore.
- Gọi reset ngay khi logout và khi auth user id thay đổi.
- Bỏ/điều chỉnh cơ chế `hasFetched` để refetch theo user id.

---

### H3. Middleware decode JWT nhưng không verify signature

**Evidence**

- Decode payload thủ công tại edge:
  - `Frontend/src/proxy.ts:9`
  - `Frontend/src/proxy.ts:18`
- Role admin check dựa trên payload đã decode:
  - `Frontend/src/proxy.ts:116`
  - `Frontend/src/proxy.ts:120`

**Risk**

- Có thể giả payload để qua UI gating phía frontend (dù backend vẫn chặn API).
- Tạo false sense of security ở tầng FE guard.

**Recommendation**

- Giữ middleware như UX guard, nhưng cần nhấn mạnh đây không phải security boundary.
- Security boundary thực tế phải ở backend authorization.
- Nếu cần verify token ở edge, phải dùng cơ chế verify chữ ký hợp lệ.

---

## 3.3 Medium

### M1. i18n thiếu key giữa locale

**Evidence**

So sánh key flatten của `Frontend/messages/*.json`:

- `en.json` thiếu 4 key (`ReadingSetup.*` liên quan currency/exp bonus)
- `zh.json` thiếu 25 key (Notifications + ReadingSetup)
- `vi.json` đầy đủ hơn hai locale còn lại.

**Risk**

- UI fallback lẫn ngôn ngữ, trải nghiệm không nhất quán.
- Tăng nguy cơ runtime missing translation ở các flow mới.

**Recommendation**

- Thiết lập script CI bắt missing keys trước merge.
- Đặt rule: merge blocked khi locale thiếu key tối thiểu.

---

### M2. Hardcoded text rải rác trong component/hook (không fully localized)

**Evidence**

- `Frontend/src/features/admin/users/application/useAdminUsers.ts:86`
- `Frontend/src/features/admin/users/application/useAdminUsers.ts:91`
- `Frontend/src/features/chat/presentation/ChatRoomPage.tsx:104`
- `Frontend/src/features/chat/presentation/ChatRoomPage.tsx:177`
- `Frontend/src/features/chat/presentation/components/PaymentOfferModal.tsx:67`
- `Frontend/src/shared/components/common/LanguageSwitcher.tsx:97`

**Risk**

- Trải nghiệm locale không đồng nhất, khó scale đa ngôn ngữ.

**Recommendation**

- Dọn toàn bộ hardcoded UI text vào `messages/*.json`.
- Hạn chế dùng fallback string khác ngôn ngữ trong runtime trừ khi có chiến lược rõ ràng.

---

### M3. Action contract chưa chuẩn hóa (null/false/object trộn lẫn)

**Evidence**

- Nhiều action trả `null`, `false`, `[]`, `{ success: false }` không đồng nhất.
- Ví dụ:
  - `Frontend/src/features/admin/application/actions/deposits.ts:27`
  - `Frontend/src/features/admin/application/actions/users.ts:87`
  - `Frontend/src/features/chat/application/actions/conversations.ts:56`
  - `Frontend/src/features/wallet/application/actions/withdrawal/admin.ts:13`

**Risk**

- UI layer phải đoán nhiều trạng thái lỗi khác nhau.
- Khó maintain, dễ bỏ sót nhánh error handling.

**Recommendation**

- Chuẩn hóa về một envelope chung, ví dụ:
  - `{ ok: true, data } | { ok: false, code, message }`
- Dùng shared typed helper để giảm boilerplate.

---

### M4. Payload mapping backend chưa ổn định (camelCase + PascalCase cùng lúc)

**Evidence**

- `Frontend/src/features/admin/application/actions/users.ts:94-98`
- `Frontend/src/features/admin/application/actions/users.ts:126-133`
- `Frontend/src/features/admin/application/actions/deposits.ts:81-84`
- `Frontend/src/features/admin/application/actions/reader-requests.ts:86-92`

**Risk**

- Tăng duplication, khó đọc, khó validate API contract.
- Cho thấy integration contract chưa “single truth”.

**Recommendation**

- Chốt chuẩn request DTO 1 kiểu naming ở backend.
- Loại bỏ payload song song sau khi backend thống nhất.

---

### M5. Dùng `eslint-disable react-hooks/exhaustive-deps` ở logic quan trọng

**Evidence**

- `Frontend/src/features/profile/application/useProfilePage.ts:117`
- `Frontend/src/features/reading/history/presentation/HistoryPage.tsx:100`
- `Frontend/src/features/reading/presentation/components/AiInterpretationStream.tsx:202`

**Risk**

- Có thể ẩn bug stale closure / race condition.

**Recommendation**

- Refactor dependency với `useCallback`/`useMemo` đúng hướng.
- Chỉ disable khi có note kiến trúc rõ và test bảo vệ.

---

### M6. Pattern `setTimeout(..., 0)` lặp lại ở nhiều hook

**Evidence**

- `Frontend/src/features/admin/deposits/application/useAdminDeposits.ts:48`
- `Frontend/src/features/admin/reader-requests/application/useAdminReaderRequests.ts:38`
- `Frontend/src/features/admin/users/application/useAdminUsers.ts:56`
- `Frontend/src/features/notifications/application/useNotificationsPage.ts:29`
- `Frontend/src/features/wallet/application/useWithdrawPage.ts:30`

**Risk**

- Khó lý giải intent, tăng complexity không cần thiết, dễ tạo hành vi khó debug.

**Recommendation**

- Bỏ timeout 0 nếu không có lý do rõ (đa số fetch có thể gọi trực tiếp trong effect).

---

### M7. Parse JWT ở client chưa normalize base64url

**Evidence**

- `Frontend/src/features/chat/application/useChatConnection.ts:16`

**Risk**

- Một số token base64url có thể parse lỗi ngẫu nhiên ở browser.

**Recommendation**

- Dùng helper decode chung (`lib/jwt.ts`) đã normalize chuẩn.

---

## 3.4 Low

### L1. Test E2E mặc định chưa phản ánh app thực tế

**Evidence**

- `Frontend/tests/example.spec.ts:4` đang test `https://playwright.dev/`.

**Impact**

- Ít giá trị bảo vệ regression thật của sản phẩm.

**Recommendation**

- Thay bằng smoke test vào app route thật (`/vi`, `/vi/login`, `/vi/reading`...).

---

### L2. Dữ liệu i18n cũ trong `public/locales` khả năng đã legacy

**Evidence**

- Có file:
  - `Frontend/public/locales/en/common.json`
  - `Frontend/public/locales/vi/common.json`
- Không thấy import/reference trong `src/` qua grep.

**Impact**

- Dễ gây nhầm source-of-truth cho localization.

**Recommendation**

- Xóa/archived nếu không dùng, hoặc ghi rõ mục đích nếu vẫn cần.

---

## 4) Architecture Assessment

### 4.1 Điểm tốt

- **Feature-first organization**: `features/<domain>/{application,presentation,domain}` khá tốt, dễ mở rộng team.
- **Route layer clean**: nhiều `app/[locale]/**/page.tsx` chỉ re-export component từ feature, giảm coupling route-business.
- **Shared infra có hướng rõ**: `shared/infrastructure/http/serverHttpClient.ts`, `parseApiError`, logger.
- **BFF pattern qua server actions** đang được áp dụng tương đối rộng.

### 4.2 Điểm cần cải thiện

- **Boundary chưa thuần nhất**: một số flow vẫn gọi API trực tiếp từ client với token query (SSE).
- **Action layer chưa “single contract”**: return shape + payload mapping không đồng nhất.
- **State lifecycle chưa hoàn chỉnh**: auth clear chưa kéo theo reset state liên quan (wallet).

---

## 5) Folder & Naming Review

### 5.1 Folder structure

Nhìn chung **hợp lý** và có định hướng scale:

- `app/` làm routing shell
- `features/` làm business domain
- `shared/` chứa reusable layer
- `store/`, `types/`, `i18n/` tách riêng rõ

### 5.2 Naming

- Component files PascalCase: tốt.
- Action files kebab-case: tốt.
- Hook files `useXxx`: tốt.
- Có một số naming semantic chưa rõ (ví dụ `public.ts` ở feature root không thống nhất cách dùng giữa các feature).

### 5.3 Observed inconsistency

- Một số file chứa cả comment/docs rất dài tiếng Việt + code tiếng Anh, làm nhiễu đọc nhanh.
- Có hardcoded text trong nhiều ngôn ngữ xen kẽ translation key.

---

## 6) Server-Client Layout Review

### Đang làm tốt

- Dùng server actions để gọi backend thay vì expose endpoint trực tiếp ở phần lớn nghiệp vụ.
- Shared server auth helper (`getServerAccessToken`) có tái sử dụng.

### Vấn đề

- Auth model hiện tại vẫn để access token lộ ra client.
- Middleware/proxy chỉ decode payload, không verify signature.
- Một số component client chứa logic network nặng (chat/stream) + state lớn -> cần kiểm soát complexity, tách thêm abstraction khi tiếp tục mở rộng.

---

## 7) Code Duplication & Technical Debt

### 7.1 Duplication patterns

- Nhiều admin hooks lặp pattern fetch + setLoading + pagination gần như giống nhau.
- Action files lặp boilerplate try/catch/logger/fallback.
- Payload dual-case duplication nhiều chỗ.

### 7.2 Technical debt hotspots

- `Frontend/src/app/globals.css` (~2525 lines): file monolithic, khó kiểm soát regression.
- Nhiều presentation file >250-600 lines (`ReadingSessionPage`, `AdminUsersPage`, `ChatRoomPage`, ...): độ phức tạp cao.
- Có suppress lint hooks ở các flow quan trọng.

### 7.3 Debt impact

- Onboarding dev mới chậm.
- Regression risk tăng khi chỉnh sửa UI/business flow.
- Harder testing ở mức unit/integration.

---

## 8) Security Review (Frontend/BFF perspective)

### Đánh giá hiện trạng

- Có guard route và role route ở proxy.
- Refresh token được `httpOnly: true` (điểm cộng).

### Rủi ro chính

- Token exposure client-side (critical).
- Next runtime version có advisory (high).
- Query token trong SSE (critical/high).
- Middleware role check không verify signature (high về UX guard mismatch).

### Khả năng bị “hack” (practical)

- **Không phải “dễ hack toàn hệ thống” ngay lập tức**, vì backend vẫn là security boundary chính.
- Nhưng **mức rủi ro token theft/XSS impact hiện ở mức cao hơn cần thiết** do mô hình token hiện tại.

---

## 9) Testing & QA Review

### Hiện có

- Playwright config chuẩn base.
- Có `viewport-qa.spec.ts` khá hữu ích để bắt lỗi responsive.

### Thiếu

- Smoke/regression test thực cho critical flows (login, payment offer, withdrawal with MFA, admin approvals).
- Unit tests cho hooks/action adapters phía FE gần như chưa thấy.

### Recommendation

- Ưu tiên test coverage cho flow tài chính + auth + role-based pages.

---

## 10) Prioritized Action Plan

## P0 (Làm ngay)

1. Chuyển access token sang mô hình an toàn hơn (không readable từ JS, không localStorage).  
2. Bỏ token query trên SSE (dùng proxy/BFF path).  
3. Upgrade Next.js khỏi dải bị advisory, rerun regression.

## P1 (Tuần 1-2)

1. Sửa wallet state lifecycle (reset theo logout/user-change).  
2. Chuẩn hóa action response contract toàn bộ feature.  
3. Gỡ dần `eslint-disable exhaustive-deps` và refactor hooks liên quan.

## P2 (Tuần 2-4)

1. Chuẩn hóa i18n keys + CI check missing translations.  
2. Loại bỏ payload dual-case sau khi align backend DTO.  
3. Tách nhỏ các file quá lớn (UI/page/css monolith).

## P3 (Liên tục)

1. Tăng test automation critical journeys.  
2. Dọn legacy artifacts (`public/locales`, test mẫu không liên quan).

---

## 11) Final Verdict

- **Kiến trúc FE đang đi đúng hướng** (feature-first, route mỏng, shared infra rõ).  
- **Chưa đạt mức production-hardening an toàn cao** vì mô hình token hiện tại và một số consistency debt.  
- Nếu xử lý P0/P1 đúng thứ tự, codebase này có thể lên mức ổn định tốt và mở rộng team dễ hơn rõ rệt.

