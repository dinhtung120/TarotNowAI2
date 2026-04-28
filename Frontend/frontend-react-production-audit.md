# Frontend React Production Audit

## 1. Executive Summary

- Báo cáo này chỉ giữ lại các issue còn mở sau đợt refactor vừa rồi.
- Các issue đã được fix tận gốc và đã xóa khỏi report:
  - `AR-01`, `AR-03`
  - `FE-ADMIN-01`, `FE-ADMIN-02`, `FE-ADMIN-03`
  - `FE-AUTH-01`, `FE-AUTH-02`
  - `FE-CHAT-01`, `FE-CHAT-02`
  - `FE-PROFILE-01`, `FE-READER-01`, `FE-WALLET-01`
  - `FE-NOTIF-01`, `FE-NOTIF-02`, `FE-NOTIF-03`
  - `FE-GACHA-01`, `FE-INVENTORY-01`
- Open findings hiện tại:
  - `3` issue `High`
  - `1` issue `Medium`
- Vùng rủi ro còn lại:
  - legacy privacy leak path của AI follow-up stream
  - clean-architecture guard vẫn chưa enforce trọn vẹn folder/layer ownership
  - test/coverage gate vẫn chưa chạm tới vài flow runtime rủi ro cao
  - component-size debt vẫn còn tồn đọng ở baseline
- Production readiness hiện tại: **chưa nên release như trạng thái hoàn tất remediation**. Functional regressions chính đã được dọn, nhưng vẫn còn 1 privacy hole tương thích ngược và 2 guardrail chưa khép kín.

## 2. Architecture Review

### AR-02 - `High` - Clean architecture guard vẫn bỏ lọt file ngoài 4 layer chuẩn

- File: `scripts/check-clean-architecture.mjs:30-44,128-133`
- Component / Hook / Function:
  - `resolveLayer`
  - vòng lặp kiểm tra `sourceFiles`
- Loại issue: Architecture problem / tooling gap / incomplete remediation
- Mô tả:
  - Guard mới đã chặn được một số bypass nguy hiểm như `NEXT_PUBLIC_API_URL`, `getPublicApiBaseUrl`, absolute-origin `fetch`, và `followupQuestion` trên `EventSource`.
  - Tuy nhiên, file nào không nằm trong path chứa `/domain/`, `/application/`, `/infrastructure/`, `/presentation/` vẫn bị `continue` ở `line 42-44`.
  - Điều này nghĩa là dependency-direction violations trong các file `features/**` hoặc `shared/**` sống ngoài 4 folder chuẩn vẫn không được kiểm tra.
- Root cause:
  - Rule engine vẫn coi layer detection theo path naming là điều kiện tiên quyết để chạy dependency graph check.
  - Việc thêm boundary detectors mới chưa giải quyết tận gốc blind spot ban đầu của `resolveLayer()`.
- Impact:
  - CI hiện bắt được vài pattern nguy hiểm cụ thể, nhưng vẫn chưa enforce được “mọi runtime file phải thuộc một layer hợp lệ”.
  - Một import sai chiều mới hoàn toàn vẫn có thể lọt nếu nó không dùng trúng các detector string-based hiện tại.
- Cách fix:
  - Fail cứng với file thuộc `src/features/**` và `src/shared/**` nhưng không map được vào 1 trong 4 layer chuẩn.
  - Hoặc định nghĩa layer map rõ ràng cho từng subtree rồi buộc mọi file runtime phải match.
  - Sau đó mới chạy dependency-direction check trên toàn bộ set đã được classify.
- Priority: `High`

## 3. Feature-by-feature Review

### Reading / Streaming

#### FE-READ-01 - `High` - Legacy query path vẫn giữ lại privacy leak của follow-up stream

- File: `src/app/[locale]/api/reading/sessions/[sessionId]/stream/route.ts:80-90`
- Component / Hook / Function:
  - `GET /[locale]/api/reading/sessions/[sessionId]/stream`
- Loại issue: Security risk / privacy leak / partial fix
- Mô tả:
  - Client hook mới đã chuyển follow-up sang `POST /stream-ticket` rồi mở SSE với `streamToken`.
  - Nhưng BFF route vẫn còn nhánh tương thích ngược:
    - đọc `followupQuestion` từ query string
    - forward lại upstream nếu `streamToken` không có
  - Vì vậy privacy leak vẫn còn tồn tại ở boundary runtime, chỉ là FE hiện tại không còn chủ động dùng nhánh đó.
- Root cause:
  - Remediation giữ lại fallback legacy query flow để tương thích rollout, nhưng chưa có phase cleanup cuối cùng để xóa path cũ.
- Impact:
  - Bất kỳ caller cũ, replay request cũ, hoặc người gọi trực tiếp endpoint này vẫn có thể đưa prompt nhạy cảm lên URL.
  - Điều này giữ nguyên class bug ban đầu ở tầng BFF, nên issue chưa được xem là fix tận gốc.
- Cách fix:
  - Xóa hoàn toàn `followupQuestion` query handling khỏi FE BFF route.
  - Đồng bộ backend để chỉ chấp nhận `streamToken` cho follow-up stream.
  - Thêm test route khẳng định query param `followupQuestion` bị reject thay vì forward.
- Priority: `High`

## 4. ReactJS Specific Problems

- Không còn open finding nào từ nhóm React lifecycle/state ownership ban đầu sau đợt refactor này.
- Các lỗi `query-key collision`, `metadata bootstrap race`, `dirty-form overwrite`, `notification stale cache`, và `realtime wake-up` đã được fix và đã xóa khỏi report.

## 5. Performance Audit

- Không còn performance/regression blocker nào trong nhóm issue ban đầu ngoài debt cấu trúc ở component-size baseline.

## 6. Security Audit

### FE-READ-01 - `High` - Legacy SSE query path vẫn cho phép prompt riêng tư đi qua URL

- File: `src/app/[locale]/api/reading/sessions/[sessionId]/stream/route.ts:80-90`
- Component / Hook / Function:
  - `GET /[locale]/api/reading/sessions/[sessionId]/stream`
- Loại issue: Security risk / privacy leak
- Mô tả:
  - Security posture phía client đã tốt hơn, nhưng BFF vẫn giữ compatibility branch cho `followupQuestion`.
- Root cause:
  - Chưa hoàn tất cleanup phase sau migration sang `streamToken`.
- Impact:
  - Privacy leak chưa bị loại bỏ hoàn toàn ở tầng frontend boundary.
- Cách fix:
  - Loại bỏ hoàn toàn query-based follow-up transport khỏi route.
- Priority: `High`

## 7. UX / Accessibility Review

- Không còn open finding nào trong nhóm UX issue ban đầu của audit này.

## 8. Code Smell / Dead Code

### CS-01 - `Medium` - Component-size baseline vẫn còn giữ 12 entry debt

- File: `scripts/component-size-baseline.json:4-15`
- Component / Hook / Function:
  - baseline debt entries
- Loại issue: Code smell / maintainability / technical debt
- Mô tả:
  - Baseline đã được giảm từ `14` xuống `12` entry.
  - Hai file critical-flow vừa được sửa là:
    - `src/components/ui/gacha/GachaPageClient.tsx`
    - `src/components/ui/inventory/UseItemModal.tsx`
    đã được kéo xuống dưới ratchet và xóa khỏi baseline.
  - Tuy nhiên repo vẫn còn 12 exception sống trong baseline, ví dụ:
    - `src/app/[locale]/(user)/leaderboard/page.tsx`
    - `src/features/community/components/CommentSection.tsx`
    - `src/shared/infrastructure/navigation/useOptimizedLink.tsx`
- Root cause:
  - Debt cũ chưa được burn down hết sau khi guard component-size đã có mặt.
- Impact:
  - Maintainability risk giảm nhưng chưa được đóng.
  - Những file còn lại vẫn là điểm dễ tạo coupling cao và regression khi chỉnh sửa tiếp.
- Cách fix:
  - Tiếp tục bào baseline về `0`, ưu tiên file có line count cao nhất hoặc nằm trên flow nhiều thay đổi.
  - Không thêm debt mới; mỗi file được chạm tới nên phải kéo ra khỏi baseline nếu khả thi.
- Priority: `Medium`

## 9. Testing Gap

### TG-01 - `High` - Coverage gate đã tốt hơn nhưng vẫn chưa gác được toàn bộ flow runtime rủi ro cao

- File: `scripts/check-risk-coverage.mjs:6-124`
- Component / Hook / Function:
  - `RISK_COVERAGE_RULES`
- Loại issue: Testing gap / regression risk / incomplete remediation
- Mô tả:
  - Đã có tiến triển thật:
    - `vitest.config.ts` đã nhận `.test.tsx`
    - risk coverage gate đã mở rộng từ `5` lên `13` file
    - đã có test mới cho `authStore`, `redirectAuthenticatedAuthEntry`, `notificationCache`, `useHydrateFormOnce`, `useReconnectWakeup`, `streamRouteGuards`, `problemDetails`, `adminGamificationFormSchema`
  - Nhưng gate hiện tại vẫn chưa cover nhiều flow runtime còn nhạy cảm:
    - `src/app/[locale]/api/reading/sessions/[sessionId]/stream/route.ts`
    - `src/features/chat/application/chat-connection/useChatSignalRLifecycle.ts`
    - `src/shared/application/hooks/usePresenceConnection.ts`
    - `src/features/notifications/application/useNotificationDropdown.ts`
    - `src/features/notifications/application/useNotificationsPage.ts`
    - `src/features/profile/application/useProfilePage.ts`
    - `src/features/gamification/admin/application/useAdminGamification.ts`
- Root cause:
  - Coverage gate đã được mở rộng theo những abstraction mới tạo, nhưng chưa vươn tới các hook/route orchestration có state machine và side effects phức tạp nhất.
- Impact:
  - Những flow có retry/reconnect/cache mutation/privacy boundary vẫn có nguy cơ regression mà CI chưa chặn trực tiếp.
- Cách fix:
  - Bổ sung test ưu tiên theo thứ tự:
    1. stream route reject legacy `followupQuestion`
    2. chat initial connect retry / cleanup khi conversation đổi
    3. presence cooldown expiry / reconnect schedule
    4. notification dropdown/page reject-path và refetch-path
    5. profile dirty-form retention khi refetch sau mutation
    6. admin gamification query/mutation integration qua local BFF
- Priority: `High`

## 10. Refactor Priority Roadmap

### Sửa ngay

1. Xóa legacy `followupQuestion` query path khỏi `stream/route.ts` và backend counterpart.
2. Hoàn tất architecture guard để fail với runtime file không thuộc 4 layer chuẩn.
3. Mở rộng test/risk coverage sang stream route, chat/presence, notifications, profile, admin gamification.

### Nên cải thiện ngay sau đó

1. Burn down thêm component-size baseline, bắt đầu từ các file > `110` lines còn lại.

### Có thể đưa backlog nhưng không nên bỏ quên

1. Tiếp tục đưa analyzer bundle về trạng thái parse được chunk thực tế để audit perf có ý nghĩa hơn.

## 11. Final Verdict

- Có nên release không: **Chưa**
- Rủi ro lớn nhất còn lại:
  1. prompt follow-up vẫn có đường đi qua query string ở BFF legacy path
  2. clean-architecture guard vẫn chưa khóa hết file ngoài layer folders
  3. CI coverage gate vẫn chưa chặn trực tiếp một số flow orchestration nhạy cảm
- Confidence level: **High**
  - Các finding còn lại đều có bằng chứng trực tiếp từ code hiện tại, không còn là giả thuyết từ audit cũ.
