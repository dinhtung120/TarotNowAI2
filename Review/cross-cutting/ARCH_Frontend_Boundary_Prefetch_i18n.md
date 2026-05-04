# Frontend Boundary, Prefetch, i18n

## 1. Source đã đọc thủ công

- `Frontend/scripts/check-clean-architecture.mjs`
- `Frontend/scripts/lib/cleanArchitectureGuard.mjs` cần đọc chi tiết khi audit rule layer mapping.
- `Frontend/scripts/check-component-size.mjs` cần đọc chi tiết khi audit size budgets.
- `Frontend/scripts/check-hook-action-size.mjs` cần đọc chi tiết khi audit hook/action budgets.
- `Frontend/scripts/check-auth-fail-closed.mjs` cần đọc chi tiết khi audit protected routes.
- `Frontend/src/features/*/public.ts`
- `Frontend/src/app/[locale]`
- `Frontend/src/shared/server/prefetch`
- `Frontend/src/i18n/*`
- `Frontend/messages/{vi,en,zh}`

## 2. Clean architecture guard thực tế

`Frontend/scripts/check-clean-architecture.mjs` quét `src/**/*.{ts,tsx,mts}` trừ `.d.ts` và test files. Guard đang kiểm:

- unclassified runtime files qua `findUnclassifiedRuntimeFiles`;
- layer direction violations bằng `resolveLayer`, `layerOrder`, `isAllowedLayerException`;
- domain purity: file layer `domain` không được import external package;
- client/runtime boundary violations qua `findClientBoundaryViolation`;
- sensitive stream payload không được xuất hiện trên EventSource URLs qua `findSensitiveStreamViolation`;
- `page.tsx`/`layout.tsx` trong app route nếu import `@/features/...` thì phải import đúng pattern `@/features/<feature>/public`.

Thông báo lỗi cụ thể của guard gồm:

- `Layer direction violations`
- `Domain purity violations`
- `Client/runtime boundary violations`
- `Sensitive stream payload must not appear on EventSource URLs`
- `App page/layout files must import features through feature public APIs only`

## 3. Boundary review rule

Với mỗi `FE_*.md`, reviewer phải đọc route thực tế trong `Frontend/src/app/[locale]` và public export trong `Frontend/src/features/<feature>/public.ts` nếu có. Nếu app page/layout import sâu như `@/features/<feature>/presentation/...`, đó là finding P1 hoặc P0 tùy impact và guard status.

Không được kết luận “thin route” chỉ vì file nằm trong `app`; phải mở `page.tsx`/`layout.tsx` và xem có orchestration/fetch/mutation/business logic không.

## 4. Prefetch/hydration

Prefetch source nằm ở `Frontend/src/shared/server/prefetch`. Review từng feature phải map route -> prefetch runner -> query key/action -> component consume. Nếu không thấy runner cho route, ghi rõ `Không tìm thấy evidence trực tiếp` thay vì nói chung “SSR prefetch đi qua shared”.

CheckIn là ví dụ không có dedicated page; evidence route/shell nằm ở navbar/shared shell và `Frontend/src/shared/server/prefetch/runners/user/shell.ts`, không được tạo giả route checkin.

## 5. i18n

Runtime/i18n source cần đọc:

- `Frontend/src/i18n/messages.ts`
- `Frontend/src/i18n/request.ts`
- `Frontend/src/i18n/clientMessages.ts`
- `Frontend/messages/vi`
- `Frontend/messages/en`
- `Frontend/messages/zh`

Rule review: user-facing copy mới phải có key trong message files tương ứng hoặc ghi gap. Không migrate hardcoded copy ngoài scope, nhưng feature doc phải chỉ ra message namespace thực tế như `messages/{vi,en,zh}/auth/auth.json`, `wallet/wallet.json`, `reading/...` khi đã đọc.

## 6. Size/auth/risk guards

Các guard chưa đọc chi tiết trong batch này nhưng là source bắt buộc khi audit frontend feature:

- `Frontend/scripts/check-component-size.mjs`
- `Frontend/scripts/check-hook-action-size.mjs`
- `Frontend/scripts/check-auth-fail-closed.mjs`
- `Frontend/scripts/check-next-image-policy.mjs`
- `Frontend/scripts/check-risk-coverage.mjs`

Không ghi budget cụ thể từ trí nhớ; phải đọc script trước khi ghi số ngưỡng trong feature docs.

## 7. Rủi ro

- P0: protected route/API wrapper fail-open; EventSource URL chứa payload nhạy cảm; client component import server-only secret/runtime path; app route bypass security/payment/backend command contract.
- P1: app page/layout import sâu vào internals thay vì `public.ts`; route chứa orchestration lớn; prefetch/hydration query key mismatch; i18n thiếu namespace cho copy mới.
- P2: component/hook/action gần vượt guard nhưng chưa fail; feature doc chưa dẫn route/action cụ thể.

## 8. Verify khi review PR

- Chạy `node Frontend/scripts/check-clean-architecture.mjs` từ `Frontend` hoặc npm script tương ứng nếu chạm route/import boundary.
- Chạy size/auth/risk guards tương ứng khi chạm component/hook/action/protected route.
- Với docs-only changes, verify bằng `git diff -- Review/frontend-features/...` và manual source path consistency.
