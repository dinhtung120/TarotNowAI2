# Frontend Deep Review Prompt

Tài liệu này là prompt/checklist để dùng cho một phiên review toàn bộ `Frontend` của TarotNowAI2. Mục tiêu là phát hiện mọi lỗi hợp lý có thể hành động được: bug logic, luồng sai, code vòng vo, code thừa, thiếu đồng nhất, vi phạm kiến trúc, nợ kỹ thuật, rủi ro security/performance/accessibility/i18n/test.

## Cách dùng

1. Mở một phiên review riêng, ưu tiên môi trường có quyền đọc toàn bộ repo.
2. Dán phần **Prompt master** bên dưới cho reviewer/agent.
3. Yêu cầu reviewer ghi kết quả vào một file markdown mới, ví dụ `docs/frontend-deep-review-findings.md`.
4. Không tự động sửa code trong phiên review này; chỉ ghi nhận vấn đề, bằng chứng, mức độ ưu tiên và hướng fix đề xuất.
5. Nếu phát hiện lỗi nghiêm trọng có thể gây leak token, bypass auth, mất tiền/quota, hoặc crash luồng chính, đánh dấu `P0`/`P1` và nêu rõ file/luồng ảnh hưởng.

## Prompt master

Bạn là reviewer senior frontend/architecture. Hãy review cực kỳ chi tiết toàn bộ thư mục `Frontend` của dự án TarotNowAI2.

Yêu cầu bắt buộc:

- Giao tiếp và viết báo cáo bằng tiếng Việt.
- Không sửa code. Chỉ đọc, phân tích và tạo báo cáo markdown.
- Kiểm tra từng file quan trọng và từng luồng tính năng, không chỉ grep bề mặt.
- Với mỗi nhận định, phải có bằng chứng cụ thể: đường dẫn file, dòng liên quan nếu có thể, mô tả hành vi sai/rủi ro, tác động, và hướng fix ngắn gọn.
- Không ghi nhận vấn đề chung chung kiểu “có thể refactor” nếu không chỉ ra được vì sao nó gây lỗi, rủi ro, nợ kỹ thuật hoặc làm khó bảo trì.
- Phân biệt rõ lỗi thật, rủi ro tiềm ẩn, nợ kỹ thuật, và đề xuất cải thiện.
- Không invent bug nếu chưa đủ bằng chứng. Nếu nghi ngờ nhưng chưa xác minh được, đưa vào mục “Cần xác minh thêm” kèm cách xác minh.
- Tôn trọng quy tắc dự án trong `CLAUDE.md`: feature-first frontend, route import qua public exports khi có, TanStack Query cho server state, Zustand cho local UI state khi cần, i18n VI/EN/ZH với fallback `vi`, không thêm `any`, interactive UI cần accessibility cơ bản.

Bối cảnh kỹ thuật chính:

- Frontend dùng Next.js 16, React 19, TypeScript, next-intl, TanStack Query, Zustand, Tailwind CSS 4, Vitest, Playwright.
- `npm run lint` bao gồm ESLint và các guard: component size, clean architecture, auth fail-closed, image policy, hook/action size.
- Các script liên quan: `npm run lint`, `npm run test`, `npm run test:coverage:risk`, `npm run verify:event-evidence`, `npm run test:e2e`, `npm run build`.

Nhiệm vụ review:

1. Khảo sát cấu trúc `Frontend`: `app`, `features`, shared/app-shared/lib/config/hooks/components, messages, public assets/themes, scripts, tests, config.
2. Lập inventory luồng tính năng chính và review từng luồng end-to-end:
   - Auth/login/register/session/refresh/logout/route protection.
   - Navigation, locale routing, middleware/proxy behavior.
   - Reading/tarot/AI interpretation/session/history.
   - Wallet/quota/payment/entitlement-related UI.
   - Chat/realtime/SignalR flows nếu có.
   - Notifications.
   - Profile/user settings.
   - Reader listing/apply flows.
   - Collection/inventory/gacha/gamification/community/admin/legal/home/shared layout.
3. Kiểm tra từng nhóm lỗi trong checklist bên dưới.
4. Tổng hợp kết quả vào markdown theo template ở cuối file.

## Checklist review chi tiết

### 1. Kiến trúc và layering

- Route/page/layout có import trực tiếp module nội bộ thay vì public entry point `@/features/*/public` không?
- Route/page có chứa orchestration phức tạp, data fetching, mapping API, hoặc business logic đáng lẽ nằm trong feature hook/component không?
- Feature có leak implementation detail sang feature khác không?
- Có import vòng, dependency ngược, hoặc shared module phụ thuộc feature không?
- Có logic domain/business nằm trong component presentational không?
- Có duplicate model/type/schema giữa feature và shared khiến dễ lệch không?
- Có file quá lớn làm nhiều trách nhiệm: component vừa fetch, validate, transform, render, side effect không?
- Có “barrel export” public quá rộng làm lộ private internals không?
- Có code mới/hiện tại vi phạm guard clean architecture nhưng chưa bị test bắt không?

### 2. Routing, locale và navigation

- Locale route có fallback đúng về `vi` không?
- Link/navigation có giữ locale hiện tại không?
- Redirect login/logout/protected route có fail-closed không?
- Middleware/proxy có bypass route bảo vệ do path matcher thiếu, trailing slash, query string, dynamic segment hoặc locale prefix không?
- Có route ẩn/legacy/public đáng lẽ protected không?
- Có duplicate route behavior giữa proxy, layout, hook và component không?
- Error/not-found/loading boundary có nhất quán theo locale và auth state không?

### 3. Auth, session và bảo mật frontend

- Có đọc/ghi token ở nơi không an toàn không?
- Có log token, cookie, PII, payment/quota data, provider response nhạy cảm không?
- Có tin vào client role/claim để cho phép hành động nhạy cảm mà không dựa backend không?
- UI protected có flash dữ liệu riêng tư trước khi auth check hoàn tất không?
- Refresh/session expiration có xử lý race condition nhiều request 401 cùng lúc không?
- Logout có clear cache/state liên quan user không?
- Form auth có validation, error association, và thông báo fail-closed không?
- Có open redirect qua `returnUrl`, `callbackUrl`, `next`, hoặc query tương tự không?
- Có XSS risk khi render markdown/html/user-generated content không?
- Có unsafe URL/image src/link target không?

### 4. API client, data fetching và cache

- Server state có dùng TanStack Query hợp lý không?
- Query key có đủ tham số ảnh hưởng dữ liệu: userId, locale, filters, pagination, sort, role không?
- Mutation có invalidate/update cache đúng scope không?
- Có stale cache sau login/logout/switch user/switch locale không?
- Có duplicate fetch trong component tree hoặc waterfall không cần thiết không?
- Error handling có phân biệt auth error, validation error, network error, domain error không?
- Có swallow error khiến UI báo thành công giả không?
- Có retry không phù hợp với mutation nhạy cảm, payment/quota, hoặc non-idempotent action không?
- Có request race gây overwrite state cũ lên state mới không?
- Có thiếu abort/cancel/debounce trong search/infinite scroll/form async không?

### 5. State management và side effects

- Zustand chỉ dùng cho local UI state hay đang giữ server state dài hạn không hợp lý?
- Store có clear/reset khi logout hoặc đổi user không?
- Có state duplicated giữa URL, query cache, Zustand và local component không?
- Effect có dependency sai gây stale closure, loop, hoặc không chạy khi input đổi không?
- Có side effect trong render path không?
- Subscription/timer/event listener có cleanup đầy đủ không?
- Có optimistic update thiếu rollback không?
- Có local state làm lệch source of truth backend không?

### 6. Logic luồng tính năng

Với từng feature, kiểm tra:

- Happy path có đủ bước và điều kiện không?
- Empty/loading/error/success states có đúng không?
- Boundary cases: danh sách rỗng, page cuối, filter không kết quả, mạng chậm, request trùng, double click, back/forward, refresh page.
- Permission/role mismatch: user thường/admin/reader/guest.
- Locale switch giữa chừng có giữ dữ liệu và copy đúng không?
- Form submit nhiều lần có double action không?
- Luồng có thể hiển thị dữ liệu cũ hoặc trạng thái thành công sau khi mutation fail không?
- Có logic copy-paste từ feature khác nhưng field/status khác nhau không?
- Có magic string/status không thống nhất với API contract không?

### 7. Forms và validation

- React Hook Form/Zod schema có khớp UI, API contract, và message i18n không?
- Client validation có thiếu case backend bắt buộc không?
- Error message có map đúng field không?
- Submit button có disabled/loading đúng và tránh double submit không?
- Input controlled/uncontrolled có warning/rủi ro không?
- Date/time/currency/number parsing có phụ thuộc locale sai không?
- File upload/image upload có validate type/size và xử lý cancel/failure không?

### 8. UI consistency và accessibility

- Button interactive có `type="button"` khi không submit form không?
- Icon-only button có accessible name không?
- Form error có liên kết `aria-describedby`/`aria-invalid` hợp lý không?
- Modal/dialog/dropdown có focus trap, keyboard close, escape, outside click phù hợp không?
- Loading skeleton/spinner có không che mất thông tin cần thiết không?
- Toast/error có đủ ngữ cảnh và không thay thế validation inline khi cần không?
- Focus visible có bị remove không?
- Color contrast/theme tokens có vấn đề trên các theme public không?
- Component có dùng `cn` khi class dài/conditional/variant-heavy không?
- Có arbitrary Tailwind value không cần thiết khi theme token đã có không?
- UI copy có hard-code thay vì i18n trong phần user-facing mới không?

### 9. i18n và nội dung

- Key VI/EN/ZH có đồng bộ không?
- Namespace có bị phình/đặt sai feature không?
- Có string user-facing hard-coded trong TSX/logic không?
- Fallback `vi` có được giữ không?
- Formatting number/date/currency có dùng locale đúng không?
- Error copy từ backend/API có cần map sang message localized không?
- Có duplicate copy giữa locale files hoặc copy lỗi ngữ cảnh không?

### 10. Performance và rendering

- Component có re-render không cần thiết vì prop/function/object tạo mới liên tục không?
- Có memoization thừa làm code phức tạp nhưng không có lợi không?
- Có computation lớn trong render mà nên memoize hoặc chuyển ra helper không?
- Có import client bundle quá nặng vào route phổ biến không?
- Có component đáng lẽ server component nhưng bị `use client` quá rộng không?
- Có image asset lớn/không tối ưu/Next Image policy risk không?
- Có dynamic import hợp lý cho chart/lottie/markdown/heavy UI không?
- Infinite list có virtualization/pagination hợp lý không?
- Theme CSS/public assets có duplicate lớn hoặc naming bất thường gây maintainability risk không?

### 11. Tests và coverage

- Feature quan trọng có test cho happy path và failure path không?
- Risk-tier logic có được coverage theo `npm run test:coverage:risk` không?
- Auth/protected route có test fail-closed không?
- Mutation/cache invalidation có test không?
- Hook phức tạp có test riêng không?
- Playwright e2e có cover luồng người dùng chính không?
- Test có mock quá mức khiến không bắt contract/API shape không?
- Test name và fixture có phản ánh business behavior không?
- Có snapshot/test brittle không đem lại giá trị không?

### 12. Code quality, consistency và nợ kỹ thuật

- Có code thừa, dead export, unused helper, duplicate helper không?
- Có logic vòng vo: biến trung gian vô nghĩa, abstraction một lần dùng, wrapper không thêm giá trị không?
- Có naming không nhất quán giữa API/type/component/hook không?
- Có file/hook/component vượt size budget hoặc cố split hình thức để né guard không?
- Có `any`, type assertion ép buộc, non-null assertion nguy hiểm không?
- Có magic string/status/error code lặp lại không?
- Có comment giải thích “what” thay vì “why” hoặc comment đã stale không?
- Có TODO/FIXME/HACK chưa có ngữ cảnh hoặc tồn đọng không?
- Có config/script/lint allowlist quá rộng che lỗi thật không?
- Có dependency không dùng hoặc dependency dùng sai mục đích không?

### 13. Build/config/tooling

- `next.config.ts`, `proxy.ts`, `eslint.config.mjs`, `vitest.config.ts`, Playwright config có logic phình, duplicate hoặc khó kiểm soát không?
- Security headers/CSP/cookie/proxy rewrite có mismatch với runtime không?
- Env variables trong `.env.example` có khớp code đang đọc không?
- Docker/build scripts có lệch với local dev/test không?
- Guard scripts có path cũ sau refactor feature-first không?
- `knip`/lint/test script có phát hiện nhưng đang bị ignore quá rộng không?

## Mức độ ưu tiên

- `P0`: Rủi ro bảo mật nghiêm trọng, bypass auth, leak token/PII, mất tiền/quota, crash toàn app, hoặc luồng chính không thể dùng.
- `P1`: Bug logic rõ ràng trong luồng quan trọng, dữ liệu sai, cache sai, protected UI sai, mutation sai, hoặc lỗi có khả năng ảnh hưởng nhiều user.
- `P2`: Nợ kỹ thuật đáng kể, code khó bảo trì, duplication gây lệch logic, thiếu test cho logic rủi ro, accessibility/i18n/performance ảnh hưởng đáng kể.
- `P3`: Cleanup nhỏ, naming/consistency, docs, refactor nhẹ, test bổ sung ít rủi ro.

## Template báo cáo đầu ra

Tạo file markdown kết quả theo cấu trúc sau:

```md
# Frontend Deep Review Findings

Ngày review: YYYY-MM-DD
Phạm vi: `Frontend`
Reviewer: <tên agent/người review>

## Tóm tắt điều hành

- Tổng số vấn đề: <n>
- P0: <n>
- P1: <n>
- P2: <n>
- P3: <n>
- Luồng rủi ro cao nhất: <liệt kê>
- Khuyến nghị thứ tự fix: <3-7 bullet>

## Phạm vi đã kiểm tra

- [ ] Root/config/tooling
- [ ] Routing/proxy/locale
- [ ] Auth/session/protected routes
- [ ] API client/query/mutation/cache
- [ ] Shared components/hooks/utils
- [ ] Messages i18n VI/EN/ZH
- [ ] Reading/tarot/AI flows
- [ ] Wallet/quota/payment-related UI
- [ ] Chat/realtime
- [ ] Notifications
- [ ] Profile/settings
- [ ] Readers
- [ ] Collection/inventory
- [ ] Gacha/gamification/community
- [ ] Admin/legal/home/layout
- [ ] Tests/e2e/coverage

## Findings cần fix

### FE-001 — <tiêu đề ngắn>

- Priority: P0/P1/P2/P3
- Loại: Bug logic / Security / Architecture / i18n / A11y / Performance / Test gap / Tech debt / Consistency
- File/luồng: `path/to/file.tsx:line`
- Bằng chứng:
  - <mô tả cụ thể từ code>
- Tác động:
  - <hậu quả với user/dev/system>
- Cách tái hiện hoặc xác minh:
  1. <bước>
  2. <bước>
- Hướng fix đề xuất:
  - <ngắn gọn, không over-engineer>
- Verification sau fix:
  - <test/lint/build/e2e cần chạy>

## Findings theo luồng tính năng

### Auth/session

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |
| FE-xxx | P1 | ... | `...` | ... |

### Reading/tarot/AI

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |

### Wallet/quota/payment UI

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |

### Chat/realtime

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |

### Notifications

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |

### Other/shared/admin/community/profile

| ID | Priority | Vấn đề | File | Ghi chú |
| --- | --- | --- | --- | --- |

## Cần xác minh thêm

| Nghi vấn | Vì sao nghi ngờ | Cách xác minh | File/luồng |
| --- | --- | --- | --- |
| ... | ... | ... | `...` |

## Không ghi nhận là lỗi

Ghi lại các điểm đã kiểm tra nhưng không coi là lỗi để tránh review lại trùng lặp.

| Chủ đề | Lý do không phải lỗi |
| --- | --- |
| ... | ... |

## Backlog fix đề xuất

| Thứ tự | ID | Priority | Lý do ưu tiên | Nhóm fix nên gom chung |
| --- | --- | --- | --- | --- |
| 1 | FE-xxx | P0/P1 | ... | Auth/cache/... |
```

## Verification gợi ý cho reviewer

Reviewer có thể dùng các lệnh dưới đây để đối chiếu, nhưng không coi output lệnh là thay thế cho đọc code thủ công:

```bash
cd Frontend
npm run lint
npm run test
npm run test:coverage:risk
npm run verify:event-evidence
npm run build
npm run test:e2e
```

Chỉ chạy lệnh phù hợp với phạm vi review và môi trường hiện có. Nếu không chạy được, ghi rõ lý do trong báo cáo.
