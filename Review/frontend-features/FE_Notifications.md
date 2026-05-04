# FE Notifications

## Source đã đọc thủ công

- Feature: `Frontend/src/features/notifications`
- Public export: `Frontend/src/features/notifications/public.ts`
- Route: `Frontend/src/app/[locale]/(user)/notifications/page.tsx`
- API proxy đã đọc: `Frontend/src/app/api/notifications/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/notifications.ts`
- Messages: `Frontend/messages/{vi,en,zh}/notifications/notifications.json`

## Entry points & luồng chính

`notifications/page.tsx` là route mỏng:

- gọi `dehydrateAppQueries(prefetchNotificationsPage)`.
- bọc `<NotificationsPage />` trong `AppQueryHydrationBoundary`.
- import `NotificationsPage` từ `@/features/notifications/public`.

`features/notifications/public.ts` export default `NotificationsPage` từ presentation layer.

## Dependency và dữ liệu

`prefetchNotificationsPage` hydrate query:

- query key `userStateQueryKeys.notifications.list(1, false)`.
- queryFn gọi `getNotifications(1, 20, undefined)`.
- nếu action fail thì trả `null` thay vì throw.

`Frontend/src/app/api/notifications/route.ts` là proxy protected:

- đọc token bằng `getServerAccessToken`.
- thiếu token trả `401` với `AUTH_ERROR.UNAUTHORIZED`.
- parse `page`, `pageSize` và optional `isRead` từ query string.
- clamp `page >= 1`, `pageSize` trong khoảng `1..100`.
- gọi backend `/Notification?...` qua `serverHttpRequest`.

Không thấy file `Frontend/src/app/api/notifications/mark-all-read/route.ts` ở path đã thử đọc; không ghi nhận API đó nếu chưa có evidence khác.

## Boundary / guard

- Notifications là protected user route; proxy phải fail-closed khi thiếu token.
- App route import qua `@/features/notifications/public`, đúng public API boundary.
- Query key list page đầu phải khớp hook trong `NotificationsPage`; đặc biệt prefetch key dùng tham số `(1, false)` nhưng action gọi `isRead` là `undefined`, cần kiểm tra khi sửa filter unread/all.
- Mark-read/delete actions nếu sửa phải kiểm tra ownership và invalidation unread count/shell.
- Copy mới thuộc `notifications/notifications.json` ở cả `vi/en/zh`.

## Rủi ro

- P0: notification list/mark-read leak dữ liệu user khác hoặc proxy fail-open khi thiếu token.
- P1: query key/filter mismatch làm hydration không được dùng hoặc hiển thị sai read/unread; unread badge stale sau mutation; hardcoded copy ngoài messages.
- P2: docs claim mark-all-read app route tồn tại khi chưa thấy evidence ở path đã đọc.

## Kết luận

FE Notifications là protected list route có SSR prefetch và app API proxy fail-closed. Review đúng phải đọc `NotificationsPage` và feature actions khi sửa filter/read state, vì rủi ro chính nằm ở query key, ownership và invalidation unread count.
