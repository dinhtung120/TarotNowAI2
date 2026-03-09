# Phase 0 – Setup/Spike (để Phase 1 chạy được end-to-end)

**Nguồn:** `CODING_PLAN.md` Section 3  
**Mục tiêu:** Chuẩn bị hạ tầng, CI/CD, DB, API scaffold, i18n, và test framework để Phase 1 bắt đầu code nghiệp vụ không bị chặn.

---

## Quy ước

- **PD** = person-days (1 người × 1 ngày ≈ 8h focus)
- **Spec mapping:** `BR(x.y.z)` = 01-business-rules, `UX(x.y.z)` = 02-product-ux-specs, `ARCH(x.y.z)` = 03-tech-architecture, `OPS(x.y.z)` = 04-ops-security-compliance, `DB(...)` = database/

---

## Task Checklist

### DevOps – Repo & CI/CD

- [ ] **P0-REPO-0.1** (0.5 PD) – Chuẩn hóa cấu trúc thư mục + naming conventions
  - Chuẩn hoá layout để team thống nhất đường dẫn, tránh phát sinh rename về sau
  - Spec: ARCH(1.1)

- [ ] **P0-REPO-0.2** (0.5 PD) – Tạo mẫu config `.env.example` + appsettings skeleton
  - Chuẩn hoá key config (DB/Redis/AI/Payment) theo env dev/staging/prod để dễ deploy
  - Spec: ARCH(1.1) | Phụ thuộc: P0-REPO-0.1

- [ ] **P0-REPO-0.3** (0.5 PD) – Secrets strategy (không commit secret)
  - Thiết kế quy ước đặt tên secret + cách inject vào runtime (env/secret store)
  - Spec: OPS(5.Security) | Phụ thuộc: P0-REPO-0.2

- [ ] **P0-CICD-0.1** (0.5 PD) – CI build API (.NET)
  - Pipeline build backend, fail-fast khi compile error
  - Spec: ARCH(1.1) | Phụ thuộc: P0-REPO-0.1

- [ ] **P0-CICD-0.2** (0.5 PD) – CI build Web (Next.js)
  - Pipeline build frontend, verify typecheck/build pass
  - Spec: ARCH(1.1) | Phụ thuộc: P0-REPO-0.1

- [ ] **P0-CICD-0.3** (0.5 PD) – CI run unit tests (skeleton)
  - Chạy unit tests tối thiểu để tạo thói quen CI đỏ/xanh sớm
  - Spec: ARCH(Testing-strategy) | Phụ thuộc: P0-CICD-0.1

- [ ] **P0-CICD-0.4** (0.5 PD) – CI cache deps + upload artifacts
  - Cache để giảm thời gian build; artifacts để debug khi fail
  - Spec: ARCH(1.5) | Phụ thuộc: P0-CICD-0.1

### DB – Database Setup

- [ ] **P0-DB-0.1** (0.5 PD) – Apply Postgres schema
  - Tạo đủ bảng/enum/proc/view theo schema; xác nhận chạy sạch trên dev
  - Spec: DB(schema.sql)

- [ ] **P0-DB-0.2** (0.5 PD) – Apply Mongo init
  - Tạo collections/index/validator; xác nhận không lỗi khi chạy script init
  - Spec: DB(init.js)

- [ ] **P0-DB-0.3** (0.5 PD) – Seed `cards_catalog` (78 lá bài)
  - Nạp 78 lá bài, kiểm tra unique indexes, đảm bảo tra cứu được theo id/code
  - Spec: DB(mongodb-schema) | Phụ thuộc: P0-DB-0.2

- [ ] **P0-DB-0.4** (0.5 PD) – Seed `system_configs` tối thiểu
  - Tạo config nền: quota tier AI + RNG secret version để reading/AI hoạt động
  - Spec: BR(Phase-1.5), ARCH(4.4.2), ARCH(4.4.3) | Phụ thuộc: P0-DB-0.1

- [ ] **P0-DB-0.5** (0.5 PD) – Seed `user_exp_levels` + `card_exp_levels` lookup data
  - Nạp dữ liệu quy đổi EXP → level cho user và card. Cần thiết để P1-CARD-BE-1.2 tính level đúng
  - Spec: DB(schema.sql) | Phụ thuộc: P0-DB-0.1

### BE – API Scaffold

- [ ] **P0-API-0.1** (0.75 PD) – Base `/api/v1` + Swagger
  - Tạo nền route versioning + Swagger để FE/QA có contract sớm
  - Spec: ARCH(4.14.4)

- [ ] **P0-API-0.2** (0.75 PD) – ProblemDetails + error code contract
  - Chuẩn hoá format lỗi và code để FE map UX message thống nhất
  - Spec: UX(4.15.3) | Phụ thuộc: P0-API-0.1

- [ ] **P0-AUTH-COOKIE-0.1** (1.0 PD) – Refresh token transport cookie + CSRF note
  - Chốt cách lưu refresh token an toàn cho web (httpOnly cookie) + chuẩn bị CSRF nếu cần
  - Spec: ARCH(4.1.5) | Phụ thuộc: P0-API-0.1

### FE – Web Scaffold

- [ ] **P0-WEB-0.1** (0.75 PD) – Next.js scaffold
  - Tạo app shell, routing, layout, base UI framework
  - Spec: ARCH(1.1)

- [ ] **P0-WEB-0.2** (1.0 PD) – i18n scaffold vi/en/zh-Hans
  - Thiết lập locale + fallback en, chuẩn bị key/messages cho UI
  - Spec: ARCH(1.1) | Phụ thuộc: P0-WEB-0.1

### QA – Test Framework

- [ ] **P0-QA-0.1** (1.0 PD) – xUnit + Testcontainers skeleton
  - Khung integration test để kiểm thử finance/escrow/quota sau này
  - Spec: ARCH(Testing-strategy)

- [ ] **P0-QA-0.2** (1.0 PD) – Playwright smoke skeleton + CI
  - Khung E2E smoke để test auth/reading/wallet nhanh trong CI
  - Spec: ARCH(Testing-strategy) | Phụ thuộc: P0-CICD-0.2

---

## Tổng kết Phase 0

| Workstream | Số task | Tổng PD |
|---|---|---:|
| DevOps | 7 | 3.5 |
| DB | 5 | 2.5 |
| BE | 3 | 2.5 |
| FE | 2 | 1.75 |
| QA | 2 | 2.0 |
| **Tổng** | **19** | **12.25** |
