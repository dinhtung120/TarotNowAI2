# FRONTEND Production Audit - TarotNowAI2 (Re-review)

Ngày review lại: 2026-04-28  
Phạm vi: `Frontend/` (App Router pages, API routes, feature modules, shared layers, store, config build/test/lint, proxy guard)

## 1. Executive Summary
- Chất lượng hiện tại đã cải thiện rõ rệt so với bản audit 2026-04-27.
- Quality gates hiện tại:
  - `npm run lint`: pass.
  - `npm run test`: pass (`33` files, `130` tests).
  - `npm run build`: pass.
  - `npm run test:e2e`: pass (`21` passed, `6` skipped).
  - `npm run verify:event-evidence`: pass.
- Các issue đã fix tận gốc và đã xóa khỏi báo cáo:
  - `AR-02, AR-04, AR-05, AR-07`
  - `RE-01, RE-02, RE-03, RE-04, RE-05, RE-06, RE-07, RE-08, RE-09`
  - `UX-01, UX-02`
  - `CS-01`
  - `TS-01, TS-03`
- Vẫn còn vùng rủi ro chính:
  - Auth protected-route contract chưa strict thực sự (chấp nhận access token “signed-like” nhưng không verify chữ ký).
  - Flow update user + balance vẫn còn cửa bypass event-contract theo nhánh.
  - Guardrails kiến trúc/kích thước component/testing coverage chưa siết đủ để ngăn tái phát.
- Release verdict hiện tại: **NO-GO**.

## 2. Architecture Review

### Issue AR-01
- File: `src/shared/server/auth/protectedRouteAuthDecision.ts:82`, `src/shared/server/auth/protectedRouteAuthDecision.ts:89`, `src/shared/server/auth/protectedRouteAuthDecision.ts:106`, `src/shared/server/auth/protectedRouteAuthDecision.test.ts:13`, `src/shared/server/auth/protectedRouteAuthDecision.test.ts:20`
- Component / Hook / Function: `resolveProtectedRouteAuthDecision`, `createSignedLikeJwt` test scenario
- Loại issue: Auth architecture gap (strict contract chưa đạt)
- Mô tả: Logic hiện tại coi access token là “valid” nếu có `alg != none` và `exp` hợp lệ. Token giả mạo kiểu `header.payload.fake-signature` vẫn được `ALLOW` ở edge.
- Root cause: Auth decision layer dùng structural token check thay vì session validity proof/verified token contract.
- Impact: Protected route có thể không redirect deterministic cho token giả mạo kiểu “signed-like”; drift với mục tiêu strict protected-route semantics.
- Cách fix: Chuyển contract về `ALLOW` chỉ khi đã có session validity proof đáng tin cậy (ví dụ verify chữ ký JWT tại edge bằng key phù hợp, hoặc bắt buộc handshake cho mọi request protected có cookie và để `/api/auth/session` quyết định).
- Priority: **Critical**

### Issue AR-03
- File: `src/features/admin/application/actions/users.ts:15`, `src/features/admin/application/actions/users.ts:55`, `src/features/admin/application/actions/users.ts:62`, `src/features/admin/users/application/useAdminUsers.ts:177`, `src/features/admin/users/application/useAdminUsers.ts:184`
- Component / Hook / Function: `updateUser`, `handleSaveUser`
- Loại issue: Event-contract ownership chưa tận gốc (financial mutation)
- Mô tả: Enforcement `admin.user.adjust-balance` đang phụ thuộc `enforceMoneyEvent` từ UI diff (`hasBalanceChanged`) thay vì boundary bắt buộc ở action/command contract.
- Root cause: Endpoint `/admin/users/{id}` vẫn gộp profile + balance mutation, khiến event ownership bị quyết định theo nhánh UI.
- Impact: Có thể còn đường bypass `MoneyChangedEvent` nếu callsite khác dùng `updateUser(..., { enforceMoneyEvent: false })` hoặc diff phía UI không phản ánh đúng biến động thực.
- Cách fix: Tách command riêng cho balance adjustment (không dùng endpoint cập nhật user chung), hoặc buộc action layer tự xác định command contract theo payload/intent thay vì option từ UI.
- Priority: **Critical**

### Issue AR-06
- File: `scripts/check-clean-architecture.mjs:99`, `scripts/check-clean-architecture.mjs:101`, `scripts/check-clean-architecture.mjs:102`
- Component / Hook / Function: `extractImports` của clean architecture guard
- Loại issue: Architecture guard weakness
- Mô tả: Guard đã cải thiện nhưng vẫn parse import bằng regex text, chưa phải AST/boundary analyzer thực thụ.
- Root cause: Tooling chưa có semantic import graph/layer contract engine.
- Impact: Một số pattern import phức tạp hoặc edge case vẫn có khả năng lọt CI.
- Cách fix: Nâng guard lên AST-based boundary lint (eslint boundary rules hoặc analyzer graph) và fail CI bắt buộc.
- Priority: **High**

### Issue AR-08
- File: `scripts/check-component-size.mjs:5`, `scripts/check-component-size.mjs:6`, `scripts/check-component-size.mjs:7`
- Component / Hook / Function: component-size policy
- Loại issue: Rule enforcement gap
- Mô tả: Đã có ratchet nhưng hard limit vẫn `120`, strict ratchet mặc định `off`; policy chuẩn `70` chưa được enforce.
- Root cause: Governance đang ở mode chuyển tiếp, chưa có gate cứng theo chuẩn cuối.
- Impact: Debt component size còn tăng; scan hiện tại vẫn còn nhiều file vượt `70` lines.
- Cách fix: Bật ratchet strict ở CI theo phase đã định, khóa file mới vượt ngưỡng và giảm trần theo sprint.
- Priority: **High**

## 3. Feature-by-feature Review
| Module | Trạng thái sau re-review | Finding còn lại | Risk chính |
|---|---|---|---|
| `auth` | Đã harden nhiều nhưng chưa strict tuyệt đối | AR-01 / SE-02 | Token giả mạo kiểu signed-like có thể không bị loại tại edge decision |
| `admin` | Đã nâng RHF+Zod + event wrapper ở nhiều flow | AR-03 / SE-03 | Balance mutation ownership vẫn lệ thuộc nhánh UI |
| `wallet` | Deposit/withdraw critical flows đã tốt hơn | Không còn issue cũ mức P0/P1 trong audit | Rủi ro còn lại chủ yếu phụ thuộc AR-03 cross-flow |
| `reading` | Init/reveal contract đã thống nhất | Không còn issue cũ trong audit | Theo dõi thêm khi rollout canary |
| `chat` | Dispute contract đã tách đúng | Không còn issue cũ trong audit | Theo dõi thêm monitoring runtime |
| `community` + `reader` | Chưa xử lý debt component lớn | CS-03 | Maintainability/regression risk ở modal nặng |
| Các module business khác | Chưa bổ sung test đủ depth | TS-02 | Regression khó phát hiện sớm |

## 4. ReactJS Specific Problems
- Không còn issue `RE-*` từ audit cũ sau re-review.
- Các fix React/query/form chính đã được áp dụng và pass quality gates.
- Residual risk hiện tại thuộc governance/architecture (`AR-*`, `TS-*`, `CS-*`) hơn là hook lifecycle trực tiếp.

## 5. Performance Audit
- Không còn finding performance P0/P1 cũ (`RE-01`, `RE-02`, `RE-05` đã xử lý).
- Debt còn lại liên quan cấu trúc:
  - Component-size policy chưa enforce cứng theo target (`AR-08`, `CS-02`, `CS-03`).
  - Khả năng review/tối ưu granular render về dài hạn vẫn bị ảnh hưởng bởi file lớn.

## 6. Security Audit

### Issue SE-01
- File: `src/proxy.ts:195`, `src/proxy.ts:201`
- Component / Hook / Function: CSP builder
- Loại issue: Security hardening gap
- Mô tả: CSP production vẫn chứa `style-src 'unsafe-inline'` và `style-src-attr 'unsafe-inline'`.
- Root cause: Runtime styling hiện phụ thuộc inline style.
- Impact: Giảm độ chặt CSP policy, tăng surface cho style injection class lỗi.
- Cách fix: Lộ trình nonce/hash cho style + loại bỏ inline style runtime dần theo module.
- Priority: **Low**

### Issue SE-02
- File: `src/shared/server/auth/protectedRouteAuthDecision.ts:88`, `src/shared/server/auth/protectedRouteAuthDecision.ts:106`, `src/shared/server/auth/protectedRouteAuthDecision.test.ts:20`
- Component / Hook / Function: protected-route decision
- Loại issue: Auth security semantics drift
- Mô tả: Auth decision ở edge vẫn cho phép token không được cryptographic verify đi nhánh `ALLOW`.
- Root cause: Same as AR-01.
- Impact: Chưa đạt strict security contract cho protected route.
- Cách fix: Same as AR-01.
- Priority: **Critical**

### Issue SE-03
- File: `src/features/admin/application/actions/users.ts:55`, `src/features/admin/application/actions/users.ts:62`, `src/features/admin/users/application/useAdminUsers.ts:184`
- Component / Hook / Function: admin user update flow
- Loại issue: Event-contract enforcement inconsistency
- Mô tả: Contract money-event chỉ enforced theo cờ `enforceMoneyEvent`, chưa thành invariant của boundary write flow.
- Root cause: Same as AR-03.
- Impact: Compliance event-driven cho financial mutation chưa khóa tuyệt đối.
- Cách fix: Same as AR-03.
- Priority: **High**

## 7. UX / Accessibility Review
- `UX-01`, `UX-02` đã fix tận gốc và đã xóa khỏi danh sách issue.
- Không phát hiện regression UX/A11y mức nghiêm trọng mới từ các phần đã refactor.

## 8. Code Smell / Dead Code

### Issue CS-02
- File: `scripts/check-component-size.mjs:7`, `scripts/check-component-size.mjs:87`, `scripts/check-component-size.mjs:99`
- Component / Hook / Function: component size ratchet governance
- Loại issue: Technical debt policy drift
- Mô tả: Ratchet có tồn tại nhưng strict gate phụ thuộc env (`COMPONENT_SIZE_STRICT`), mặc định chưa block debt hiện hữu.
- Root cause: CI governance chưa bật chế độ siết theo tier/phase bắt buộc.
- Impact: Debt giảm chậm, tiêu chuẩn khó giữ ổn định theo thời gian.
- Cách fix: Bật strict mode trong CI cho changed files trước, sau đó siết toàn repo theo lộ trình.
- Priority: **Medium**

### Issue CS-03
- File: `scripts/check-component-size.mjs:9`, `scripts/check-component-size.mjs:10`, `scripts/check-component-size.mjs:14`
- Component / Hook / Function: temporary allowlist
- Loại issue: Code smell / maintainability hotspot
- Mô tả: Vẫn còn allowlist cho 2 component oversized (`PostReportModal`, `ReaderDetailModal`).
- Root cause: Chưa tách component theo SRP đến mức policy mong muốn.
- Impact: Khó review regression, tăng coupling UI logic.
- Cách fix: Tách subcomponents + bỏ allowlist trước hạn expire.
- Priority: **Medium**

## 9. Testing Gap

### Issue TS-02
- File: `src/features/admin/users/application/useAdminUsers.ts:67`, `src/features/checkin/`, `src/features/community/`, `src/features/notifications/`
- Component / Hook / Function: risk-heavy feature modules
- Loại issue: Coverage gap
- Mô tả: Dù tổng test pass, nhiều module business vẫn chưa có test file module-level (`admin`, `checkin`, `community`, `gacha`, `gamification`, `notifications`, `profile`, `reader`, ...).
- Root cause: Test distribution lệch về shared/core, chưa theo risk-based ownership map.
- Impact: Khả năng regression ở flow nghiệp vụ cao vẫn còn.
- Cách fix: Thiết lập threshold theo tier module + bổ sung integration/e2e cho admin wallet/chat/reading/profile trước.
- Priority: **High**

## 10. Refactor Priority Roadmap
### 10.1 Sửa ngay (P0)
1. Chốt strict auth contract để đóng AR-01/SE-02 (không chấp nhận signed-like forged token vào nhánh `ALLOW`).
2. Tách hẳn balance adjustment command khỏi `updateUser` general flow để đóng AR-03/SE-03.

### 10.2 Nên cải thiện sớm (P1)
1. Nâng clean architecture guard sang AST/boundary analyzer thật sự (AR-06).
2. Bật component-size ratchet strict theo phase trong CI (AR-08, CS-02).
3. Bắt đầu tháo allowlist 2 component lớn (CS-03).

### 10.3 Backlog có deadline (P2)
1. Hoàn thiện test matrix theo risk tiers cho module business chưa có test (TS-02).
2. Siết dần target component-size về chuẩn `70` lines với timeline cố định.
3. Hardening CSP khỏi `'unsafe-inline'` theo roadmap non-breaking (SE-01).

## 11. Final Verdict
- Có nên release không: **Chưa nên release (NO-GO)**.
- Rủi ro lớn nhất:
  1. Auth guard chưa strict đủ để loại bỏ triệt để token forged dạng signed-like ở edge decision.
  2. Financial event-contract ở user balance update chưa là invariant kiến trúc.
- Confidence level: **0.92** (cao cho các issue còn lại vì có evidence line-level + quality gate runtime đã chạy lại).
