# FE Inventory

## Source đã đọc thủ công

- Feature: `Frontend/src/features/inventory`
- Public export: `Frontend/src/features/inventory/public.ts`
- Route: `Frontend/src/app/[locale]/(user)/inventory/page.tsx`
- API proxy đã đọc: `Frontend/src/app/api/inventory/route.ts`
- Prefetch: `prefetchInventoryPage` trong `Frontend/src/shared/server/prefetch/runners/user/collection.ts`
- Shared infra: `Frontend/src/shared/infrastructure/inventory/*`

## Entry points & luồng chính

`inventory/page.tsx` là route mỏng:

- gọi `dehydrateAppQueries(prefetchInventoryPage)`.
- render `InventoryPage` từ `@/features/inventory/public` trong `AppQueryHydrationBoundary`.
- export shared SEO metadata.

`features/inventory/public.ts` export `InventoryPage` từ presentation layer.

## Dependency và dữ liệu

`prefetchInventoryPage` nằm chung file với collection/reading setup runner:

- dùng `swallowPrefetch`.
- prefetch query key `inventoryQueryKeys.mine()`.
- queryFn là `fetchInventoryServer`.

`Frontend/src/app/api/inventory/route.ts` là protected proxy:

- GET lấy token bằng `getServerAccessToken`; thiếu token trả `401`.
- GET gọi backend `/inventory`.
- POST parse `UseInventoryItemPayload`; invalid JSON trả `400`.
- POST yêu cầu `itemCode` không rỗng.
- POST normalize `quantity` về `1..10`.
- POST yêu cầu idempotency key từ `INVENTORY_IDEMPOTENCY_HEADER` hoặc payload; thiếu key trả `400`.
- POST gọi backend `/inventory/use` và forward idempotency header.

## Boundary / guard

- Inventory là protected user route; app API proxy không được fail-open khi thiếu token.
- Use item là reward/entitlement-facing mutation; idempotency key và quantity clamp là boundary quan trọng.
- Query key `inventoryQueryKeys.mine()` phải khớp hook trong `InventoryPage`.
- Vì runner nằm trong `user/collection.ts`, khi sửa không được làm hỏng collection hoặc reading setup prefetch cùng file.
- Copy mới cần đồng bộ namespace inventory trong `vi/en/zh`.

## Rủi ro

- P0: duplicate item use do mất idempotency key; dùng item của user khác nếu backend/proxy ownership sai; quantity clamp bị bypass.
- P1: inventory query key mismatch làm SSR hydration không được dùng; invalidation thiếu sau POST; route deep import internals.
- P2: docs bỏ qua việc prefetch inventory nằm trong `collection.ts`, dễ tìm nhầm runner.

## Kết luận

FE Inventory là protected entitlement/reward surface với SSR prefetch và app API proxy dùng idempotency. Review đúng phải kiểm tra `inventoryQueryKeys.mine()`, `fetchInventoryServer`, POST `/inventory/use` và invalidation sau khi dùng item.
