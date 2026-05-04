# FE CheckIn

## Source đã đọc thủ công

- Feature: `Frontend/src/features/checkin`
- Public export: không thấy `Frontend/src/features/checkin/public.ts` ở path đã đọc.
- Dedicated route: không thấy route check-in riêng trong evidence hiện tại; check-in được hydrate qua user shell metadata.
- Prefetch/integration: `Frontend/src/shared/server/prefetch/runners/user/shell.ts`
- Messages: cần đối chiếu namespace check-in trong `Frontend/messages/{vi,en,zh}` khi sửa copy; evidence hiện tại không chứng minh route riêng.

## Entry points & luồng chính

CheckIn không xuất hiện như một page route độc lập trong evidence đã đọc. Luồng entry đáng tin cậy hiện là user shell prefetch:

- `prefetchUserSegmentShell` gọi `getInitialMetadata()`.
- Nếu có data, runner set `checkinQueryKeys.streakStatus` bằng `data.streak`.
- Cùng một metadata response cũng hydrate wallet balance, unread notification count, recent notifications, unread chat count và active conversations.

Vì không có `features/checkin/public.ts`, review không được yêu cầu app route import qua public export cho CheckIn; thay vào đó cần đọc component/navbar/shell đang consume `checkinQueryKeys.streakStatus` khi sửa UI check-in.

## Dependency và dữ liệu

CheckIn frontend phụ thuộc:

- `checkinQueryKeys.streakStatus` trong `features/checkin/domain/checkinQueryKeys`.
- `getInitialMetadata` từ `shared/application/actions/metadata`.
- Backend user-context metadata trả `streak`.
- Shell/navbar client state nơi người dùng thấy streak/check-in status.

Không thấy dedicated app API proxy check-in hoặc dedicated SSR page runner trong evidence đã đọc; mutation check-in nếu có phải nằm trong feature action/component và cần đọc riêng khi sửa.

## Boundary / guard

- Shell prefetch là protected user context; không được hydrate streak cho anonymous route.
- Query key `checkinQueryKeys.streakStatus` phải khớp hook/component check-in.
- Vì shell metadata hydrate nhiều domain cùng lúc, sửa CheckIn không được làm hỏng wallet/notification/chat keys trong `prefetchUserSegmentShell`.
- Copy mới về streak/check-in phải đồng bộ `vi/en/zh` theo namespace thực tế.

## Rủi ro

- P0: check-in mutation duplicate reward/streak nếu UI retry không có idempotency/backend guard; shell metadata leak user state ở anonymous route.
- P1: query key mismatch làm streak stale; sửa shell prefetch làm mất wallet/chat/notification hydration; docs claim có dedicated route/public export khi không thấy evidence.
- P2: thiếu mapping message namespace cụ thể nếu chỉ review feature folder mà bỏ qua navbar/shell consumer.

## Kết luận

FE CheckIn không phải page route riêng trong evidence hiện tại; nó là user-shell state được hydrate từ metadata qua `checkinQueryKeys.streakStatus`. Review đúng phải bắt đầu từ shell/navbar consumer và feature action mutation, không giả định tồn tại `public.ts` hoặc prefetch runner riêng.
