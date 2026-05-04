# FE Admin

## Source đã đọc thủ công

- Feature: `Frontend/src/features/admin`
- Public export: `Frontend/src/features/admin/public.ts`
- Routes: `Frontend/src/app/[locale]/admin/page.tsx` và các nhánh `admin/users`, `admin/deposits`, `admin/withdrawals`, `admin/reader-requests`, `admin/disputes`, `admin/readings`, `admin/gamification`
- API routes: `Frontend/src/app/api/admin/gamification/**/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/admin.ts`
- Messages: `Frontend/messages/{vi,en,zh}/admin/admin.json`

## Entry points & luồng chính

`admin/page.tsx` đã đọc là thin route:

- dynamic import `AdminDashboardPage` từ `@/features/admin/public`.
- hydrate TanStack Query bằng `dehydrateAppQueries(prefetchAdminDashboardPage)`.
- render trong `AppQueryHydrationBoundary`.

`features/admin/public.ts` export admin page shells:

- `AdminDashboardPage`, `AdminUsersPage`, `AdminDepositsPage`, `AdminWithdrawalsPage`, `AdminReaderRequestsPage`, `AdminDisputesPage`, `AdminReadingsPage`, `AdminPromotionsPage`, `AdminSystemConfigsPage`.

Admin prefetch runner đã đọc có các runner cụ thể:

- `prefetchAdminDashboardPage`
- `prefetchAdminUsersPage`
- `prefetchAdminDepositsPage`
- `prefetchAdminReaderRequestsPage`
- `prefetchAdminReadingsPage`
- `prefetchAdminWithdrawalsQueuePage`
- `prefetchAdminDisputesPage`
- `prefetchAdminGamificationPage`

## Dependency và dữ liệu

Admin frontend aggregate nhiều backend domains:

- Users/deposits/reader requests từ `features/admin/application/actions`.
- Disputes từ `features/chat/application/actions`.
- Admin gamification từ `features/gamification/admin/infrastructure`.
- Admin readings từ `features/reading/public`.
- Withdrawal queue từ `features/wallet/public`.

Prefetch dùng `queryFnOrThrow` và `swallowPrefetch` cho một số pages, nên review hydration phải đọc query key/action tương ứng.

## Boundary / guard

- Admin routes phải được protected/fail-closed bởi auth/admin layout/middleware; route page mỏng chưa đủ chứng minh RBAC.
- App routes phải import qua public API; `admin/page.tsx` đang dùng `@/features/admin/public`.
- Admin finance actions như withdrawals/deposits/users add balance phải giữ idempotency và không duplicate mutation từ UI retry.
- Messages mới thuộc namespace `admin/admin.json` ở `vi/en/zh`.

## Rủi ro

- P0: admin route hoặc API proxy fail-open cho non-admin; finance/admin mutation retry gây duplicate; frontend exposes sensitive admin data in public shell.
- P1: prefetch query key/action mismatch làm stale/hydration bug; admin page deep import internals; missing i18n for admin copy.
- P2: docs review Admin như một page duy nhất trong khi source có nhiều subpages/prefetch runners.

## Kết luận

FE Admin là route group lớn có SSR prefetch rõ ràng và aggregate nhiều backend modules. Review đúng phải đọc route subpage, public export, prefetch runner và backend action tương ứng cho từng admin screen.
