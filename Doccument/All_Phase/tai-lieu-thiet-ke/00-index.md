# Chỉ mục tài liệu TarotWeb (Chỉ mục Blueprint) v1.5

Bộ tài liệu này được tách từ `FEATURE_REQUIREMENTS_BLUEPRINT_v1.5.md` để giảm độ dài và tăng khả năng tra cứu.

## 1) Danh sách tài liệu

1. `01-business-rules.md` - quy tắc kinh doanh, lộ trình (roadmap), định giá (pricing), KPI.
2. `02-product-ux-specs.md` - hành trình người dùng (user journeys), tính năng hướng người dùng (user-facing), trạng thái UX (UX states), a11y/SEO.
3. `03-tech-architecture.md` - ngăn xếp công nghệ (stack), kiến trúc (architecture), dữ liệu/máy trạng thái (data/state machines), tích hợp (integrations).
4. `04-ops-security-compliance.md` - pháp lý/tuân thủ (legal/compliance), NFR, sự cố/bảo trì (incident/maintenance).

## 2) Thứ tự ưu tiên khi có mâu thuẫn

1. `OPS-*`: ràng buộc pháp lý/tuân thủ/bảo mật (legal/compliance/security) bắt buộc, mức cao nhất.
2. `BR-*`: quyết định kinh doanh (business decisions), định giá (pricing), quyền lợi (entitlement), kiếm tiền (monetization).
3. `ARCH-*`: ràng buộc triển khai (implementation constraints) và bất biến hệ thống (system invariants).
4. `UX-*`: tương tác/nội dung/trạng thái (interaction/copy/state) hiển thị cho người dùng.

Quy tắc: tài liệu cấp dưới không được vi phạm ràng buộc cấp trên.

## 3) Không gian tên ID yêu cầu (Requirement ID Namespace)

- `BR-*`: Quy tắc kinh doanh (Business Rules)
- `UX-*`: Đặc tả UX và thiết kế sản phẩm (Product UX + Design Specs)
- `ARCH-*`: Kiến trúc kỹ thuật (Tech Architecture)
- `OPS-*`: Vận hành/Bảo mật/Tuân thủ (Ops/Security/Compliance)

## 4) Ma trận truy vết (Traceability Matrix) (mục cũ -> tài liệu mới)

- `1) Product scope và mục tiêu`
 - `1.1`, `1.2`, `1.5` -> `ARCH-*`
 - `1.4` + mục tiêu kinh doanh -> `BR-*`
 - `1.3` -> quy tắc quản trị (governance) tại file chỉ mục này
- `2) Phase rollout` (lộ trình theo giai đoạn)
 - `Phase 0..2.5` + cốt lõi/không cốt lõi (core/non-core) -> `BR-*`
 - `2.6`, `2.7`, `2.8` -> `UX-*`
- `3) business decisions` (quyết định kinh doanh) -> `BR-*`
- `4) Feature specs` (đặc tả tính năng)
 - `4.1`, `4.2`, `4.4.1`, `4.4.4-4.4.6`, `4.5`, `4.6.1-4.6.2`, `4.6.5`, `4.9`, `4.15.3` -> `UX-*`
 - `4.3`, `4.7`, `4.8`, `4.11`, `4.12`, `4.15.1`, `4.17` -> `BR-*`
 - `4.1.5`, `4.4.2-4.4.3`, `4.4.7`, `4.6.3-4.6.4`, `4.14`, `4.15.2`, `4.16`, `4.18` -> `ARCH-*`
 - `4.10`, `4.13`, `4.19`, `4.20` -> `OPS-*`
- `5) Non-functional requirements` (yêu cầu phi chức năng)
 - `Accessibility`, `SEO` -> `UX-*`
 - Phần còn lại -> `OPS-*`
 - `Rate-limit policy matrix` + `Testing strategy` -> `ARCH-*`
- `6) Risks và mitigation` (rủi ro và giảm thiểu)
 - `6.1`, `6.3`, `6.5`, `6.6` -> `ARCH-*`
 - `6.2`, `6.4` + rủi ro tổng quan (overall risk) -> `OPS-*`
 - `6.7` -> `BR-*`

Lưu ý tách tài liệu:
- Quy tắc kỹ thuật (technical rule) chỉ ghi một lần ở `ARCH-*`; `BR-*` chỉ giữ quyết định kinh doanh.
- Quy tắc tuân thủ/vận hành (compliance/operations rule) chỉ ghi một lần ở `OPS-*`.

## 5) Quy tắc cập nhật

- Mọi thay đổi quy tắc (rule change) phải cập nhật tài liệu có namespace ID tương ứng trước.
- Nếu một thay đổi ảnh hưởng nhiều namespace, cập nhật theo thứ tự ưu tiên ở mục 2.
- Luôn ghi rõ ngày cập nhật và người phụ trách (owner) cho mỗi thay đổi quan trọng.
