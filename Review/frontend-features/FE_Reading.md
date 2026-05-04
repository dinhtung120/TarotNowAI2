# FE Reading

## Source đã đọc thủ công

- Feature: `Frontend/src/features/reading`
- Public export: `Frontend/src/features/reading/public.ts`
- Routes: `Frontend/src/app/[locale]/(user)/reading/page.tsx`, `reading/history/page.tsx`, `reading/history/[id]/page.tsx`, `reading/session/[id]/page.tsx`
- API/SSE routes: `Frontend/src/app/[locale]/api/reading/sessions/[sessionId]/stream/route.ts`, `stream-ticket/route.ts`; app API reading routes under `Frontend/src/app/api/reading/**/route.ts`
- Prefetch: `prefetchReadingSetupPage` in `runners/user/collection.ts`, `prefetchReadingHistoryListPage` and `prefetchReadingHistoryDetailPage` in `runners/user/readingHistory.ts`
- Messages: `messages/{vi,en,zh}/reading/{tarot,setup,session,history,ai-interpretation}.json`

## Entry points & luồng chính

`reading/page.tsx` đã đọc là thin route:

- imports `ReadingSetupPage` from `@/features/reading/public`.
- hydrates `prefetchReadingSetupPage`.
- wraps with `AppQueryHydrationBoundary`.

`features/reading/public.ts` exports:

- `ReadingSetupPage`
- `ReadingHistoryPage`
- `ReadingHistoryDetailPage`
- `ReadingSessionPage`
- `getAllHistorySessionsAdminAction`

## Dependency và dữ liệu

`prefetchReadingSetupPage` gọi `getReadingSetupSnapshotAction`, hydrate query key `userStateQueryKeys.reading.setupSnapshot()` và set cache `userStateQueryKeys.reading.cardsCatalog()` từ `cardsCatalog`.

Reading frontend phụ thuộc:

- Wallet balance + cards catalog setup snapshot.
- Reading history list/detail runners.
- SSE stream and stream-ticket localized API routes for AI interpretation.

## Boundary / guard

- EventSource URL không được chứa prompt/follow-up nhạy cảm; stream-ticket route là evidence cần review khi sửa AI stream.
- Reading setup prefetch phải giữ query keys khớp hooks trong session/setup components.
- App pages phải import qua `@/features/reading/public`.
- AI/session copy phải có keys trong reading message namespaces cả ba locale.

## Rủi ro

- P0: prompt/follow-up sensitive payload trên EventSource URL; stream token leak; reading command duplicate gây double charge; protected reading route fail-open.
- P1: setup snapshot/cache key mismatch làm thiếu catalog/wallet; history detail route leak session của user khác qua API proxy/action; missing i18n in AI statuses.
- P2: docs bỏ qua localized SSE API route under `[locale]/api`.

## Kết luận

FE Reading là high-risk frontend module vì kết hợp SSR setup snapshot, history, AI SSE stream và wallet/AI billing backend. Review đúng phải đọc route, public exports, prefetch runners và localized stream API routes.
