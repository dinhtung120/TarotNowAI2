# Frontend Boundary, Prefetch, i18n

## Evidence đã rà

- Feature public exports: `Frontend/src/features/*/public.ts`.
- App Router: `Frontend/src/app/[locale]`.
- Shared prefetch/hydration: `Frontend/src/shared/server/prefetch`.
- i18n runtime: `Frontend/src/i18n/messages.ts`, `Frontend/src/i18n/request.ts`, `Frontend/src/i18n/clientMessages.ts`, `Frontend/messages/{vi,en,zh}`.
- Guards: `Frontend/scripts/check-clean-architecture.mjs`, `check-component-size.mjs`, `check-hook-action-size.mjs`, `check-auth-fail-closed.mjs`, `check-next-image-policy.mjs`, `check-risk-coverage.mjs`.

## Boundary thực tế

`page.tsx` và `layout.tsx` nên là composition wrapper. Feature UI/data orchestration nằm trong `Frontend/src/features/*`, public surface qua `public.ts`. Shared layer chứa primitives, query client, auth/prefetch utilities; cần tránh nhúng business logic feature-specific quá sâu trong shared.

## Prefetch/hydration

SSR prefetch đi qua shared server prefetch runner và TanStack Query dehydration/hydration. Review từng route phải đối chiếu: route -> prefetch runner -> query key -> component consume.

## i18n

New user-facing copy phải đi qua existing localization approach và có VI/EN/ZH với fallback locale -> vi. Hardcoded copy hiện hữu không nên migrate ngoài scope, nhưng review docs phải ghi gap nếu touched flow có copy mới không có messages.

## Rủi ro

- P0: auth fail-open, route bypass backend command/security boundary, secret/token exposure.
- P1: route quá dày, import sâu vào internals thay vì public export, hydration mismatch, thiếu i18n.
- P2: component/hook gần vượt size guard.
