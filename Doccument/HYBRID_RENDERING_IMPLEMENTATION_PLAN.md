# Kế Hoạch Triển Khai Hybrid Rendering (Next.js)

## 1. Mục tiêu
- Tối ưu tốc độ hiển thị lần đầu (First Paint / LCP) cho các trang quan trọng.
- Giảm cảm giác chậm do pattern `mount rồi mới fetch toàn trang`.
- Giữ trải nghiệm tương tác mượt cho các khu vực realtime và UI state phức tạp.

## 2. Khuyến nghị thực tế (giữ nguyên)

### Chuyển sang server render cho:
- Trang/list dữ liệu đọc nhiều, ít tương tác tức thời
- Nội dung SEO/public page
- Data cần hiện ngay khi vào trang (admin tables, dashboards, profile summary)

### Giữ client fetch cho:
- Tương tác realtime (chat, streaming, polling)
- UI phụ thuộc trạng thái local phức tạp
- Thành phần chỉ xuất hiện sau hành động người dùng (modal/filter nâng cao)

### Mô hình tốt nhất với Next:
- Server render dữ liệu ban đầu (fast first paint)
- Sau đó hydrate + cập nhật cục bộ ở client (optimistic update/revalidate nhẹ)
- Tránh “mount rồi mới fetch toàn trang” ở các page CRUD list

### Kết luận ngắn:
- Không nên “all-in server” cho mọi thứ. Nên dùng hybrid: server cho initial load quan trọng, client cho tương tác động. Đây là cách nhanh và ổn định nhất.

## 3. Phạm vi áp dụng cho TarotNowAI2

### Nhóm SSR-first (ưu tiên chuyển ngay)
- Public/SEO:
  - `/[locale]`
  - `/[locale]/readers`
  - `/[locale]/readers/[id]`
  - `/[locale]/legal/*`
- Admin list/dashboard:
  - `/[locale]/admin`
  - `/[locale]/admin/users`
  - `/[locale]/admin/deposits`
  - `/[locale]/admin/promotions`
  - `/[locale]/admin/readings`
  - `/[locale]/admin/reader-requests`
  - `/[locale]/admin/withdrawals`
  - `/[locale]/admin/disputes`
- Profile summary:
  - `/[locale]/profile`
  - `/[locale]/profile/reader`

### Nhóm Hybrid (SSR initial + client update cục bộ)
- Các trang CRUD có thao tác liên tục:
  - Wallet pages
  - Reading history/list pages
  - Admin tables có actions approve/reject/toggle/delete

### Nhóm Client-first (giữ client fetch/realtime)
- Chat/inbox/room:
  - `/[locale]/chat`
  - `/[locale]/chat/[id]`
- Streaming/session:
  - `/[locale]/reading/session/[id]`

## 4. Lộ trình triển khai

## Giai đoạn 1: Audit & Baseline (1-2 ngày)
- Lập bảng cho từng route:
  - Kiểu render hiện tại
  - Nguồn dữ liệu
  - Có realtime hay không
  - Có SEO hay không
- Đo baseline:
  - TTFB, FCP, LCP, INP, CLS
  - JS bundle size theo route
  - Số request khi load lần đầu

## Giai đoạn 2: Refactor theo cụm (3-5 ngày)
- Chuyển các page list/dashboard sang:
  - Server Component load initial data
  - Client component chỉ giữ phần tương tác
- Thay pattern “action xong -> fetch full list” bằng:
  - Optimistic/local update
  - Hoặc refresh nhẹ (không bật loading toàn trang)
- Chuẩn hóa:
  - `loading.tsx`/`skeleton`
  - Error boundary
  - Empty state

## Giai đoạn 3: Caching & Revalidation (2-3 ngày)
- Xác định TTL/revalidate cho từng loại dữ liệu:
  - Public read-heavy: cache dài hơn
  - Admin data: cache ngắn hoặc no-store tùy nghiệp vụ
- Bổ sung invalidation sau create/update/delete để dữ liệu không stale.

## Giai đoạn 4: QA & Rollout (2 ngày)
- QA viewport + regression:
  - Mobile/tablet/desktop
  - Auth/admin role redirect
  - Error/empty/retry path
- Theo dõi sau release:
  - So sánh KPI trước/sau
  - Log lỗi console/network

## 5. Quy tắc kỹ thuật khi implement
- Không đặt toàn bộ trang thành client component chỉ vì có 1 vài tương tác nhỏ.
- Tách rõ:
  - `page.tsx` (server, lấy initial data)
  - `*-client.tsx` (useState/useEffect cho interaction)
- Ưu tiên cập nhật cục bộ thay vì refetch full table.
- Tránh `useEffect(fetch...)` cho initial load nếu có thể lấy từ server.

## 6. KPI chấp nhận
- Trang admin/public chính:
  - Giảm thời gian “thấy dữ liệu lần đầu” rõ rệt so với baseline.
- Giảm số request load ban đầu ở các trang list/dashboard.
- Không tăng lỗi hydration/client runtime.
- Không regression về UX trên mobile.

## 7. Deliverables
- Tài liệu phân loại route theo chiến lược render.
- PR refactor theo từng cụm route (small batch, dễ review).
- Báo cáo before/after KPI.
- Checklist QA hoàn tất cho mobile/tablet/desktop.
