# FE Gacha

## Source đã đọc thủ công

- Feature: `Frontend/src/features/gacha`
- Public export: `Frontend/src/features/gacha/public.ts`
- Routes: `Frontend/src/app/[locale]/(user)/gacha/page.tsx`, `Frontend/src/app/[locale]/(user)/gacha/history/page.tsx`
- API proxy đã đọc: `Frontend/src/app/api/gacha/pools/route.ts`, `Frontend/src/app/api/gacha/pull/route.ts`, `Frontend/src/app/api/gacha/history/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/gacha.ts`
- Shared infra: `Frontend/src/shared/infrastructure/gacha/*`

## Entry points & luồng chính

`gacha/page.tsx` là route mỏng:

- gọi `dehydrateAppQueries(prefetchGachaPage)`.
- render `GachaPage` từ `@/features/gacha/public` trong `AppQueryHydrationBoundary`.
- export shared SEO metadata.

`gacha/history/page.tsx` tương tự:

- gọi `prefetchGachaHistoryPage`.
- render `GachaHistoryPage` từ `@/features/gacha/public`.

`features/gacha/public.ts` export `GachaPage` và `GachaHistoryPage`.

## Dependency và dữ liệu

`prefetchGachaPage`:

- prefetch `gachaQueryKeys.pools()` bằng `fetchGachaPoolsServer()`.
- nếu có pool đầu tiên, prefetch odds `gachaQueryKeys.poolOdds(firstCode)` và history preview `gachaQueryKeys.history(1, 6)`.

`prefetchGachaHistoryPage` hydrate `gachaQueryKeys.history(1, 20)`.

App API proxy:

- `pools/route.ts` yêu cầu server access token và gọi backend `/gacha/pools`.
- `history/route.ts` yêu cầu token, normalize `page >= 1`, clamp `pageSize <= 100`, gọi `/gacha/history`.
- `pull/route.ts` yêu cầu token, parse payload, lấy idempotency key từ `GACHA_IDEMPOTENCY_HEADER` hoặc payload, trả `400` nếu thiếu, clamp count về `1..10`, forward `/gacha/pull` kèm idempotency header.

## Boundary / guard

- Gacha là protected user route; app API proxy phải fail-closed khi thiếu token.
- Pull gacha là reward/finance-facing mutation; idempotency key là bắt buộc và phải được giữ từ client tới backend.
- Query key pools/odds/history trong prefetch phải khớp hooks trong `GachaPage` và `GachaHistoryPage`.
- Không bỏ clamp count/pageSize ở proxy nếu sửa API route.
- Copy mới phải đồng bộ namespace gacha trong `vi/en/zh`.

## Rủi ro

- P0: duplicate pull do mất idempotency key; token fail-open ở app proxy; count clamp bị bypass gây reward/finance sai.
- P1: SSR pools/odds/history key mismatch; history preview/page full invalidation sai sau pull; client gửi idempotency khác với header.
- P2: docs chỉ nói page gacha mà bỏ qua history route và API proxy pull.

## Kết luận

FE Gacha là protected reward module có SSR prefetch và app API proxy rõ ràng. Review đúng phải kiểm tra idempotency key, count/pageSize normalization, query key hydration và invalidation sau pull.
