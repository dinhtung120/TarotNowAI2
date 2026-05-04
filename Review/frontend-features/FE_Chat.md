# FE Chat

## Source đã đọc thủ công

- Feature: `Frontend/src/features/chat`
- Public export: `Frontend/src/features/chat/public.ts`
- Routes: `Frontend/src/app/[locale]/(user)/chat/page.tsx`, `Frontend/src/app/[locale]/(user)/chat/[id]/page.tsx`
- API proxy đã đọc: `Frontend/src/app/api/chat/inbox-preview/route.ts`, `Frontend/src/app/api/chat/unread-count/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/chat.ts`
- Messages: `Frontend/messages/{vi,en,zh}/chat/chat.json`

## Entry points & luồng chính

`chat/page.tsx` là route mỏng:

- re-export `ChatInboxPage` từ `@/features/chat/public`.
- re-export `generateLocaleMetadata` từ shared SEO metadata.

`chat/[id]/page.tsx` dynamic import `ChatRoomPage` từ `@/features/chat/public` và bọc bằng `Suspense` với `LoadingSpinner`. Route này không tự parse room id trong file đã đọc; room-level data/subscription nằm trong feature component.

`features/chat/public.ts` export:

- `createConversation`
- `ChatInboxPage`
- `ChatRoomPage`
- `ChatSegmentShell`

## Dependency và dữ liệu

Chat frontend phụ thuộc:

- Conversation actions trong `features/chat/application/actions`.
- App API proxy cho inbox preview và unread count.
- Backend `/conversations` contract qua `serverHttpRequest` trong app API routes.
- Shell/query prefetch `prefetchChatInboxShell`, hydrate `userStateQueryKeys.chat.inboxActive()` bằng `listConversations('active', 1, 100)`.

`inbox-preview/route.ts` lấy access token server-side bằng `getServerAccessToken`; nếu không có token trả `401` qua `buildProblemResponse`. Proxy gọi `/conversations?tab=active&page=1&pageSize=8`.

`unread-count/route.ts` cũng fail-closed khi thiếu token và gọi `/conversations/unread-total`.

## Boundary / guard

- Chat là protected user route; app API proxy không được trả dữ liệu khi thiếu server access token.
- App route import qua `@/features/chat/public`, đúng public API boundary.
- Realtime/message UI nên nằm trong feature component/hook, không đưa orchestration vào `page.tsx`.
- Query key prefetch `userStateQueryKeys.chat.inboxActive()` phải khớp hook đọc inbox active.
- Copy mới thuộc `chat/chat.json` ở cả `vi/en/zh`.

## Rủi ro

- P0: chat room leak conversation của user khác; app API proxy fail-open; gửi token/message content vào URL/log; duplicate finance/escrow mutation nếu chat action liên quan booking/payment.
- P1: route deep import internals thay vì public export; prefetch inbox key mismatch; unread-count cache stale làm navbar/shell hiển thị sai.
- P2: docs bỏ qua `ChatSegmentShell` hoặc shell prefetch làm review thiếu luồng sidebar/inbox.

## Kết luận

FE Chat là protected realtime/message surface với route mỏng, public exports rõ và shell prefetch cho inbox active. Review đúng phải đọc feature room/inbox component khi sửa chat UI, đồng thời kiểm tra app API proxy fail-closed và query key hydration.
