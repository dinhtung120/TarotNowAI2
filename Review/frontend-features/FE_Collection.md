# FE Collection

## Source đã đọc thủ công

- Feature: `Frontend/src/features/collection`
- Public export: `Frontend/src/features/collection/public.ts`
- Route: `Frontend/src/app/[locale]/(user)/collection/page.tsx`
- API routes: `Frontend/src/app/api/collection/route.ts`, `collection/card-image/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/collection.ts` với `prefetchCollectionPage`
- Messages: `Frontend/messages/{vi,en,zh}/collection/collection.json`

## Entry points & luồng chính

`collection/page.tsx` đã đọc là thin route:

- imports `CollectionPage` from `@/features/collection/public`.
- hydrates `prefetchCollectionPage`.
- wraps with `AppQueryHydrationBoundary`.

`features/collection/public.ts` export `CollectionPage`.

## Dependency và dữ liệu

`prefetchCollectionPage` calls `getUserCollection()` and stores data under `userStateQueryKeys.collection.mine()`. It returns `[]` when action result is not successful or data is not an array.

Collection depends on:

- User collection backend/API.
- Card image API proxy `collection/card-image/route.ts`.
- Shared query key namespace `userStateQueryKeys.collection.mine()`.

## Boundary / guard

- Collection route is protected user route; card collection must be scoped to current user.
- App route imports through public API correctly.
- Query key in prefetch must match CollectionPage hook; otherwise hydration double fetch/stale UI.
- Card image proxy must not allow arbitrary SSRF/path traversal when fetching images.

## Rủi ro

- P0: user sees another user's collection; card-image proxy accepts unsafe remote URL/path; protected route fail-open.
- P1: prefetch silently falls back to empty array masking API/auth failure; hydration query key mismatch.
- P2: docs merge Collection and Reading catalog without distinguishing user collection vs cards catalog.

## Kết luận

FE Collection is a protected collection route with SSR prefetch and card-image proxy. Review đúng phải đọc route, action, query key and API proxy when changing collection/card rendering.
