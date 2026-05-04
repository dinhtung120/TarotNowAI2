# FE Gamification

## Source đã đọc thủ công

- Feature: `Frontend/src/features/gamification`
- Public export: `Frontend/src/features/gamification/public.ts`
- User routes: `Frontend/src/app/[locale]/(user)/gamification/page.tsx`, `Frontend/src/app/[locale]/(user)/leaderboard/page.tsx`
- Admin route: `Frontend/src/app/[locale]/admin/gamification/page.tsx`
- Admin API proxy đã đọc: `Frontend/src/app/api/admin/gamification/quests/route.ts`, `Frontend/src/app/api/admin/gamification/achievements/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/gamification.ts`, admin runner `prefetchAdminGamificationPage` từ shared runners index
- Messages: `Frontend/messages/{vi,en,zh}` namespace `Gamification`/admin gamification copy cần đối chiếu khi sửa UI.

## Entry points & luồng chính

`gamification/page.tsx`:

- gọi `getTranslations('Gamification')`.
- hydrate `prefetchGamificationHubPage`.
- render `GamificationStatsBar`, `QuestsPanel`, `TitleSelector`, `AchievementsGrid` từ local route sections.
- dùng metadata generator `generateGamificationMetadata`.

`leaderboard/page.tsx`:

- hydrate `prefetchLeaderboardPage`.
- render `LeaderboardTable` từ `@/features/gamification/public`.
- dùng metadata generator `generateLeaderboardMetadata`.

`admin/gamification/page.tsx`:

- dynamic import `AdminGamificationClient` từ `@/features/gamification/public`.
- hydrate `prefetchAdminGamificationPage`.

`features/gamification/public.ts` export `AdminGamificationClient` và `LeaderboardTable`.

## Dependency và dữ liệu

`prefetchGamificationHubPage`:

- gọi `getRuntimePoliciesAction()` để lấy `gamification.defaultQuestType`.
- nếu có default quest type, prefetch `gamificationKeys.quests(defaultQuestType)`.
- prefetch achievements và titles qua `fetchGamificationAchievements`, `fetchGamificationTitles`.

`prefetchLeaderboardPage`:

- lấy `gamification.defaultLeaderboardTrack` từ runtime policies.
- prefetch `gamificationKeys.leaderboard(defaultTrack)` bằng `fetchGamificationLeaderboard`.

Admin API proxy:

- `quests/route.ts` và `achievements/route.ts` đều gọi `requireAdminSession()`.
- GET gọi backend `/admin/gamification/quests` hoặc `/admin/gamification/achievements`.
- POST parse JSON payload và forward lên backend; invalid JSON trả `400`.

## Boundary / guard

- User gamification route là protected user route; leaderboard/hub query phải dùng runtime policy default track/type.
- Admin gamification route/proxy phải fail-closed qua `requireAdminSession`, không dùng user token thường.
- Public export chỉ có `AdminGamificationClient` và `LeaderboardTable`; hub page sections đang nằm cùng route folder, nên khi refactor cần kiểm tra boundary guard thực tế trước khi di chuyển.
- Query keys `gamificationKeys.*` phải khớp hooks trong components.
- Copy route dùng namespace `Gamification`; admin copy cần đồng bộ `vi/en/zh` nếu thêm text user-facing.

## Rủi ro

- P0: admin API proxy fail-open cho non-admin; quest/achievement POST ghi sai payload không được backend validate; reward/task state stale gây claim trùng nếu UI mutation sai.
- P1: runtime policy defaultQuestType/defaultLeaderboardTrack mismatch làm SSR prefetch không khớp client; admin route deep import internals; missing i18n.
- P2: docs bỏ qua `leaderboard` hoặc admin gamification route khi chỉ review user hub.

## Kết luận

FE Gamification gồm user hub, leaderboard và admin management surface. Review đúng phải đọc cả runtime policy-based prefetch, public exports và admin API proxy vì cùng feature nhưng boundary/risk khác nhau rõ rệt.
