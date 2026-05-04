# FE Community

## Source đã đọc thủ công

- Feature: `Frontend/src/features/community`
- Public export: `Frontend/src/features/community/public.ts`
- Route: `Frontend/src/app/[locale]/(user)/community/page.tsx`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/community.ts`
- Messages: `Frontend/messages/{vi,en,zh}/community/community.json`
- API proxy: không thấy community app API proxy riêng trong evidence đã đọc; route dùng feature action qua prefetch runner.

## Entry points & luồng chính

`community/page.tsx` là route có composition mỏng:

- gọi `dehydrateAppQueries(prefetchCommunityFeedsPage)`.
- bọc `FeedPage` trong `AppQueryHydrationBoundary`.
- import `FeedPage` từ `@/features/community/public`.
- thêm wrapper `<main>` với background/text classes.

`features/community/public.ts` chỉ export `FeedPage` từ feature component.

## Dependency và dữ liệu

`prefetchCommunityFeedsPage` gọi `prefetchCommunityFeedInfinite(qc, 'public')`.

Luồng prefetch:

- query key `['community', 'feed', visibility]`.
- queryFn gọi `getFeedAction(pageParam, 10, visibility)`.
- `getNextPageParam` dựa trên `metadata.page`, `metadata.pageSize`, `metadata.totalCount`.
- Chỉ SSR feed `public`; comment trong source ghi tab `private` tải khi user chuyển tab trên client.

Community frontend phụ thuộc backend community feed/action, TanStack infinite query, i18n community messages và media/upload/report UI trong feature internals nếu sửa sâu.

## Boundary / guard

- Community là protected user route nhưng SSR chỉ hydrate public feed; private feed phải giữ ownership/token boundary ở action/API backend.
- App route import qua `@/features/community/public`, đúng public API boundary.
- Query key `['community', 'feed', 'public']` phải khớp hook infinite feed trong `FeedPage`.
- Không claim có app API proxy community riêng nếu chưa đọc thấy route dưới `Frontend/src/app/api/community`.
- Copy mới thuộc `community/community.json` ở cả `vi/en/zh`.

## Rủi ro

- P0: private feed/post/media/report action leak dữ liệu user khác hoặc bypass auth khi chuyển tab client-side.
- P1: infinite query key mismatch giữa SSR public feed và client hook; hardcoded copy ngoài messages; route page phình thành business orchestration.
- P2: docs mô tả SSR cả private feed trong khi source chỉ prefetch public feed.

## Kết luận

FE Community là protected feed route với SSR hydration cho public feed và private tab tải client-side. Review đúng tập trung vào `FeedPage`, `getFeedAction`, query key infinite feed và boundary private/public khi sửa social UI.
