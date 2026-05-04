# FE Reader

## Source đã đọc thủ công

- Feature: `Frontend/src/features/reader`
- Public export: `Frontend/src/features/reader/public.ts`
- Routes: `Frontend/src/app/[locale]/(user)/readers/page.tsx`, reader apply/profile/settings routes mapped under user app tree, admin `admin/reader-requests/page.tsx`
- API routes: reader-related routes under `Frontend/src/app/api/readers/**/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/readers.ts`, plus admin runner `prefetchAdminReaderRequestsPage`
- Messages: `messages/{vi,en,zh}/readers/readers.json`, `readers/reader-apply.json`

## Entry points & luồng chính

`readers/page.tsx` đã đọc là thin route:

- imports `ReadersDirectoryPage` from `@/features/reader/public`.
- hydrates `prefetchReadersDirectoryPage`.
- wraps with `AppQueryHydrationBoundary`.

`features/reader/public.ts` exports actions and pages:

- actions: `getMyReaderRequest`, `updateReaderProfile`, `updateReaderStatus`, `listFeaturedReaders`, `getReaderProfile`.
- pages: `ReaderApplyPage`, `ReaderPublicProfilePage`, `ReadersDirectoryPage`.

## Dependency và dữ liệu

Reader prefetch runner has concrete flows:

- `prefetchReadersDirectoryPage`: `listReaders(1, READERS_DIRECTORY_PAGE_SIZE, '', '', '')`.
- `prefetchReaderApplyPage`: `getMyReaderRequest()`.
- `prefetchReaderPublicProfilePage(qc, readerId)`: `getReaderProfile(readerId)`.
- `prefetchProfileReaderSettingsPage`: calls `getProfileAction()` first and only prefetches reader profile settings if role is `tarot_reader`.

## Boundary / guard

- Directory/public profile may be user-facing but apply/settings are protected; route/API proxy must preserve auth.
- Reader settings prefetch role gate (`role !== 'tarot_reader'`) is important evidence; do not remove without replacing fail-closed behavior.
- App pages should import through `@/features/reader/public`.
- Reader apply/profile copy must use readers message namespaces.

## Rủi ro

- P0: reader settings route/action accessible to non-reader; public profile exposes private application/proof fields; update status/profile without ownership.
- P1: prefetch key mismatch for directory/profile; missing admin reader request route coverage; presence/status display mismatch with backend.
- P2: docs call reader only admin feature and omit public directory/apply pages.

## Kết luận

FE Reader spans public directory/profile, protected apply/settings, and admin review. Review đúng phải read route-specific prefetch and role-gated profile settings flow.
