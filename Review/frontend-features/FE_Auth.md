# FE Auth

## Source đã đọc thủ công

- Feature: `Frontend/src/features/auth`
- Public export: `Frontend/src/features/auth/public.ts`
- Routes: `Frontend/src/app/[locale]/(auth)/login/page.tsx`, `register/page.tsx`, `forgot-password/page.tsx`, `reset-password/page.tsx`, `verify-email/page.tsx`
- API routes: `Frontend/src/app/api/auth/login/route.ts`, `logout/route.ts`, `refresh/route.ts`, `session/route.ts`, `session/handshake/route.ts`
- Messages: `Frontend/messages/{vi,en,zh}/auth/auth.json`
- Guard liên quan: `Frontend/scripts/check-auth-fail-closed.mjs`, `check-clean-architecture.mjs`

## Entry points & luồng chính

Auth pages import qua feature public API:

- `LoginPage`
- `RegisterPage`
- `ForgotPasswordPage`
- `ResetPasswordPage`
- `VerifyEmailPage`

`login/page.tsx` đã đọc là thin route: resolve `locale`, gọi `redirectAuthenticatedAuthEntry({ locale })`, rồi render `<LoginPage />` từ `@/features/auth/public`.

`public.ts` cũng export `AppNavbar` và `AppAuthSessionManager`, cho thấy auth feature còn tham gia session/navbar shell ngoài nhóm `(auth)` pages.

## Dependency và dữ liệu

Auth frontend phụ thuộc:

- App API routes under `app/api/auth/*` cho cookie/session boundary.
- Shared server auth redirect `redirectAuthenticatedAuthEntry` để ngăn user đã đăng nhập vào auth entry page.
- i18n namespace `auth/auth.json` ở cả `vi/en/zh`.

Không thấy prefetch runner auth riêng trong evidence map; auth pages chủ yếu là form/session flow chứ không có SSR query prefetch riêng rõ ràng.

## Boundary / guard

- Auth pages phải fail-closed: protected/session logic không được bỏ qua `app/api/auth/session` và guards.
- App routes phải import `@/features/auth/public`, không deep import presentation internals.
- API routes auth là cookie/token boundary; không expose access/refresh token ra client logs/local storage.
- User-facing copy mới phải vào `messages/{vi,en,zh}/auth/auth.json`.

## Rủi ro

- P0: auth fail-open; token/cookie leak; refresh/session API proxy trả token nhạy cảm ra client; redirect authenticated bị bypass.
- P1: auth route deep-import internals; missing i18n key cho validation/error copy; session handshake mismatch frontend/backend.
- P2: docs claim SSR prefetch auth runner trong khi không thấy evidence trực tiếp.

## Kết luận

FE Auth có route boundary mỏng và public export rõ. Review đúng phải đọc page, feature form/action, app API auth proxy và fail-closed guard trước khi kết luận thay đổi an toàn.
