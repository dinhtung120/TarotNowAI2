# FE Profile

## Source đã đọc thủ công

- Feature: `Frontend/src/features/profile`
- Public export: `Frontend/src/features/profile/public.ts`
- Route: `Frontend/src/app/[locale]/(user)/profile/page.tsx`; related mapped routes include profile MFA and reader settings pages
- API routes: `Frontend/src/app/api/profile/**/route.ts`
- Prefetch: `Frontend/src/shared/server/prefetch/runners/user/profile.ts` với `prefetchProfilePage`, `prefetchProfileMfaPage`
- Messages: `Frontend/messages/{vi,en,zh}/profile/profile.json`

## Entry points & luồng chính

`profile/page.tsx` đã đọc là thin route:

- calls `dehydrateAppQueries(prefetchProfilePage)`.
- wraps `<ProfilePage />` trong `AppQueryHydrationBoundary`.
- imports `ProfilePage` từ `@/features/profile/public`.

`features/profile/public.ts` export:

- `getProfileAction`
- `ProfilePage`
- `ProfileMfaPage`
- `ProfileReaderSettingsPage`

Profile feature bao gồm hồ sơ user, MFA settings surface và reader settings surface.

## Dependency và dữ liệu

Profile frontend phụ thuộc:

- Backend profile API/actions cho get/update profile.
- Avatar upload flow qua profile API routes/actions.
- MFA page/action nếu route profile MFA được sử dụng.
- Reader settings page cho reader profile/payout-facing fields.

Profile route có SSR query prefetch, nên query key trong prefetch runner phải khớp hooks trong `ProfilePage`.

## Boundary / guard

- Profile route là protected user route; không được render private profile data ở anonymous route.
- App route đang import qua public API đúng boundary.
- Profile/payout/avatar fields có sensitive data; form/action không được log account number/upload token.
- Copy mới phải vào `profile/profile.json` cả `vi/en/zh`.

## Rủi ro

- P0: profile update/avatar confirm gửi user id từ client thay vì token-bound API; upload token/object key leak; payout bank fields log/expose plaintext.
- P1: prefetch profile key mismatch; profile route deep import internals; MFA/reader settings đặt sai protected route boundary.
- P2: docs bỏ qua `ProfileMfaPage` và `ProfileReaderSettingsPage` dù public export có.

## Kết luận

FE Profile là protected user area có SSR prefetch và nhạy cảm vì avatar/payout/MFA surfaces. Review đúng phải đọc page route, public exports, prefetch runner và feature actions/forms.
