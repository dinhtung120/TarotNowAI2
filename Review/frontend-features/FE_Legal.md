# FE Legal

## Source đã đọc thủ công

- Feature: `Frontend/src/features/legal`
- Public export: `Frontend/src/features/legal/public.ts`
- Routes: `Frontend/src/app/[locale]/(site)/legal/privacy/page.tsx`, `legal/tos/page.tsx`, `legal/ai-disclaimer/page.tsx`
- Messages: `Frontend/messages/{vi,en,zh}/legal/legal.json`
- API routes: không thấy legal API proxy rõ ràng trong map hiện tại
- Prefetch: không thấy runner legal riêng rõ ràng

## Entry points & luồng chính

Legal routes là site/static content routes. `privacy/page.tsx` đã đọc re-export trực tiếp từ `@/features/legal/public`:

- `PrivacyPolicyPage` + metadata.
- `TermsOfServicePage` + metadata.
- `AiDisclaimerPage` + metadata.

`features/legal/public.ts` export page component và metadata generator cho từng document.

## Dependency và dữ liệu

Legal frontend phụ thuộc chủ yếu vào:

- localized content/message namespace `legal/legal.json`.
- SEO metadata generators trong feature legal presentation.

Không thấy SSR prefetch runner riêng hoặc app API proxy legal trong evidence hiện tại. Runtime legal policy/consent API nếu dùng ở auth/register flow thuộc Auth/Legal backend, không chứng minh route static legal đang gọi API.

## Boundary / guard

- Routes import qua `@/features/legal/public`, đúng public API boundary.
- Legal copy phải đồng bộ `vi/en/zh` và không hardcode ngoài namespace nếu thay đổi nội dung user-facing.
- Metadata phải đi cùng page export để SEO/static routes ổn định.
- Không biến static legal route thành client-heavy route nếu không có lý do.

## Rủi ro

- P1: nội dung legal ở các locale lệch version/meaning; metadata route sai document; hardcoded copy ngoài messages.
- P2: docs claim legal API proxy/prefetch tồn tại khi không thấy evidence.

## Kết luận

FE Legal là static/site content feature với public exports rõ ràng và không thấy prefetch/API proxy riêng. Review đúng tập trung vào route re-export, metadata và i18n consistency ba ngôn ngữ.
