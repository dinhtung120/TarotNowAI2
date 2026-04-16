# Navigation Prefetch Matrix (TarotNow)

Tài liệu này ghi **chi tiết hiện trạng prefetch** để bạn tự trim các trang ít dùng.

## 1) Chỗ cần sửa khi muốn chỉnh prefetch

- Route + query prefetch logic (đích nào prefetch gì):
  - `src/shared/infrastructure/navigation/routeQueryPrefetch.ts`
- Link wrapper mặc định:
  - `src/shared/infrastructure/navigation/useOptimizedLink.tsx`
- Push/replace wrapper (button navigation):
  - `src/shared/infrastructure/navigation/useOptimizedNavigation.ts`
- Link i18n mặc định prefetch route tree:
  - `src/i18n/routing.tsx`

## 2) Hiện trạng: vào trang nào thì prefetch route nào

Lưu ý: prefetch route tree của Next chỉ xảy ra khi link được render/visible (hoặc khi gọi `navigation.push/replace` sẽ prefetch trước).

## A. Nhóm Public/Site (`/[locale]/(site)/*`, ví dụ `/`, `/legal/*`)

- Từ navbar luôn có:
  - `/`
  - `/reading`
  - `/readers`
- Nếu chưa đăng nhập (navbar right):
  - `/login`
  - `/register`
- Nếu mở avatar menu (đã đăng nhập):
  - `/profile`
  - `/wallet`
  - `/reading/history`
  - `/admin` (nếu role admin)

## B. Nhóm User (`/[locale]/(user)/*`)

- Ngoài navbar như trên, khi mở user sidebar sẽ có link đến:
  - `/reading`
  - `/reading/history`
  - `/collection`
  - `/readers`
  - `/chat`
  - `/community`
  - `/gamification`
  - `/inventory`
  - `/leaderboard`
  - `/gacha`
  - `/gacha/history`
  - `/profile`
  - `/wallet`
  - `/wallet/deposit`
  - `/notifications`
- Mobile bottom tab/menu có thể prefetch thêm các route tương ứng nhóm tab.

## C. Nhóm Admin (`/[locale]/admin/*`)

- Khi mở admin menu sẽ có:
  - `/admin`
  - `/admin/users`
  - `/admin/deposits`
  - `/admin/promotions`
  - `/admin/readings`
  - `/admin/reader-requests`
  - `/admin/withdrawals`
  - `/admin/disputes`
  - `/` (exit portal)

## 3) Hiện trạng: route đích nào thì query nào được prefetch

Đây là mapping đang nằm trong `routeQueryPrefetch.ts`.

| Route đích | Query prefetch hiện tại |
|---|---|
| `/wallet` và mọi route con `/wallet/*` | `['wallet','ledger',1]` |
| `/wallet/deposit` | `['wallet','deposit-promotions']` |
| `/wallet/withdraw` | `['wallet','withdrawals','mine']` |
| `/notifications` | `['notifications','dropdown']`, `['notifications','unread-count']` |
| `/chat` và `/chat/*` | `['chat','inbox','active']`, `['chat','unread-badge']` |
| `/readers` | `['readers',1,12,'','','']` |
| `/readers/:id` | `['reader-profile', readerId]` |
| `/profile` | `['profile','me']`, `['reader','my-request']` |
| `/profile/mfa` | `['profile','mfa-status']` |
| `/reading` | `['me','reading-setup-snapshot']` |
| `/reading/history` | `historySessionsListQueryKey(1,'all','')` |
| `/reading/history/:id` | `historyDetailQueryKey(sessionId)` |
| `/collection` | `['collection','user']`, `['reading','cards-catalog']` |
| `/inventory` | `inventoryQueryKeys.mine()` |
| `/gacha` | `gachaQueryKeys.pools()` |
| `/gacha/history` | `gachaQueryKeys.history(1,20)` |

## 4) Đề xuất trim: trang ít vào nên tắt prefetch trước

## Nhóm nên tắt **query prefetch** trước (ít ảnh hưởng UX):

- `/wallet/withdraw`
- `/gacha/history`
- `/profile/mfa`
- `/readers/:id`
- `/reading/history/:id`

Cách tắt: xoá block tương ứng trong `buildRouteQuerySpecs()` của `routeQueryPrefetch.ts`.

## Nhóm có thể tắt **route prefetch** (nếu cực ít dùng):

- `/admin/disputes`
- `/admin/readings`
- `/admin/reader-requests`
- `/wallet/deposit` (nếu tỉ lệ vào thấp)

Cách tắt: tại từng link, set:

```tsx
<Link href="/admin/disputes" prefetch={false} prefetchQueries={false} />
```

## 5) Ma trận gợi ý (lean) theo trang nguồn

Đây là ma trận khuyến nghị để giữ cảm giác nhanh mà không prefetch quá rộng.

| Trang đang ở | Nên prefetch | Nên bỏ prefetch |
|---|---|---|
| `/` | `/reading`, `/readers`, `/login` hoặc `/profile` | `/gacha/history`, `/wallet/withdraw`, admin routes |
| `/reading` | `/reading/history`, `/collection` | `/profile/mfa`, `/admin/*` |
| `/readers` | `/chat`, `/profile` | prefetch detail `/readers/:id` hàng loạt |
| `/chat` | `/readers`, `/notifications` | `/gacha/history`, `/profile/mfa` |
| `/wallet` | `/wallet/deposit`, `/profile` | `/wallet/withdraw` (nếu ít dùng) |
| `/profile` | `/wallet`, `/notifications` | `/profile/mfa` (nếu ít dùng) |
| `/admin` | `/admin/users`, `/admin/deposits` | `/admin/disputes`, `/admin/readings` (nếu ít dùng) |

## 6) Rule chỉnh nhanh

- Link thường dùng: giữ `prefetch={true}` + `prefetchQueries={true}`.
- Link ít dùng: đặt `prefetch={false}` và `prefetchQueries={false}`.
- Query block ít dùng: xoá trong `routeQueryPrefetch.ts` trước, vì đây là phần tiết kiệm request rõ nhất.

## 7) Gợi ý rollout an toàn

1. Tắt query prefetch cho 5 route ít dùng ở mục 4.
2. Deploy và đo lại navigation (DevTools + thực tế người dùng).
3. Nếu vẫn thừa request, mới tắt route prefetch cho admin/secondary links.
