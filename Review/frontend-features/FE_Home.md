# FE Home

## Source đã đọc thủ công

- Feature: `Frontend/src/features/home`
- Public export: `Frontend/src/features/home/public.ts`
- Route: `Frontend/src/app/[locale]/(site)/page.tsx`
- Messages: `Frontend/messages/{vi,en,zh}/home/index.json`
- API/prefetch: không thấy home API proxy hoặc prefetch runner riêng rõ ràng trong evidence hiện tại

## Entry points & luồng chính

`(site)/page.tsx` đã đọc re-export trực tiếp:

- `HomePage` as default from `@/features/home/public`.
- `generateLocaleMetadata` from shared SEO metadata.

`features/home/public.ts` export `HomePage` từ `presentation/HomePage`.

## Dependency và dữ liệu

Home route hiện là site route mỏng, không thấy `AppQueryHydrationBoundary` hoặc home prefetch runner riêng. Nếu `HomePage` fetches data client-side/server-side bên trong feature, cần đọc component/action khi audit sâu.

Backend Home có `/api/v1/home/snapshot`, nhưng frontend evidence hiện không thấy app API proxy home rõ ràng từ map; không tự khẳng định route đang prefetch snapshot nếu chưa đọc `HomePage`.

## Boundary / guard

- Site home route public/anonymous; không render dữ liệu protected/user-private.
- Route import qua `@/features/home/public`, đúng public API boundary.
- User-facing copy thuộc `home/index.json` cả ba locale.

## Rủi ro

- P1: HomePage fetch public snapshot nhưng không có prefetch/SSR strategy rõ; public route leak user-specific data nếu shared client action dùng session implicitly.
- P1: SEO/metadata mismatch nếu route/page đổi mà metadata không cập nhật.
- P2: docs claim dedicated home prefetch runner khi không có evidence.

## Kết luận

FE Home là public site route rất mỏng. Review đúng tập trung vào `HomePage` internals và message/SEO nếu thay đổi UI/content.
