# Phase 0 – Tài liệu thiết kế liên quan

**Mục đích:** Trích các phần thiết kế cần đọc khi làm Phase 0 (Setup/Spike).  
**Nguồn gốc:** Trích từ `tai-lieu-thiet-ke/01-business-rules.md`, `03-tech-architecture.md`, `04-ops-security-compliance.md`

---

## 1. Kiến trúc tổng quan (từ ARCH)

### 1.1 Stack công nghệ

| Layer | Công nghệ | Ghi chú |
|---|---|---|
| Frontend web | Next.js 16 + TypeScript | App Router, SSR |
| UI system | Tailwind CSS + Radix UI + shadcn/ui | Component tái sử dụng |
| Animation & FX | Framer Motion | Cấu hình theo năng lực thiết bị |
| API backend | ASP.NET Core 10 Web API | MediatR, FluentValidation, ProblemDetails |
| Authentication | JWT + refresh token rotation | Argon2id hasher |
| Realtime | SignalR | Chat 1-1 |
| AI streaming | SSE (Server-Sent Events) | Token streaming |
| Primary DB | PostgreSQL | ACID, finance, escrow |
| Document DB | MongoDB | Content, chat, logs |
| Cache/Queue | Redis | Rate limiting, cache, pub/sub |
| Background jobs | TickerQ | Settlement, refund, reminder |
| Logging | Serilog structured logging | Masking PII bắt buộc |
| Observability | OpenTelemetry | Traces, metrics, logs |
| Rate limiting | ASP.NET rate limiter + Redis | Fixed/sliding window |

### 1.2 Ngôn ngữ hỗ trợ
- `vi` (Tiếng Việt), `en` (English), `zh-Hans` (简体中文)
- Fallback: `en`

---

## 2. API Contract cơ bản (từ ARCH)

### ProblemDetails format
- Mọi API error trả về format chuẩn RFC 7807 ProblemDetails
- Có `type`, `title`, `status`, `detail`, `instance`
- Content-Type: `application/problem+json`
- Error code contract để FE map UX message thống nhất

### API versioning
- Base path: `/api/v1`
- Swagger/OpenAPI tự động generate

---

## 3. Authentication cơ bản (từ ARCH 4.1.5)

### JWT + Refresh Token Rotation
- Access token: short-lived
- Refresh token: rotation + reuse detection → revoke chain
- Web transport: httpOnly secure cookie
- Device binding cho mobile
- Argon2id password hashing (không log plain text)
- CSRF protection bắt buộc với cookie-authenticated browser endpoints

---

## 4. Database Setup (từ DB)

### PostgreSQL
- Schema: `database/postgresql/schema.sql`
- 25+ bảng, 5 stored procedures, 2 views, triggers
- System accounts seed: platform + escrow

### MongoDB
- Init script: `database/mongodb/init.js`
- 29 collections với indexes, validators, TTL
- Schema docs: `database/mongodb/schema.md`

### Seed data cần thiết
- `cards_catalog`: 78 lá bài (unique code, name vi/en/zh)
- `system_configs`: runtime configs (quota, pricing, timeouts)
- `user_exp_levels` + `card_exp_levels`: EXP → level lookup

---

## 5. Bảo mật cơ bản (từ OPS)

### Secrets strategy
- Không commit secret vào repo
- Dùng `.env.example` làm template
- Inject secrets qua environment variables hoặc secret store
- Separate configs per env: dev/staging/prod

### Logging & PII
- Không log raw PII (password, card data, payout info)
- Structured logging với trace IDs
- Masking PII bắt buộc

---

## 6. CI/CD Requirements (từ ARCH + OPS)

### Build pipeline
- Backend: `dotnet build` + `dotnet test`
- Frontend: `npm install` + `npm run build` + typecheck
- Cache: NuGet packages, npm deps
- Fail-fast: compile error → pipeline fail

### Test framework
- BE: xUnit + Testcontainers (Postgres + MongoDB)
- FE E2E: Playwright smoke tests
- CI phải chạy được unit tests tối thiểu

---

## Tham chiếu đầy đủ

| Tài liệu | Sections liên quan |
|---|---|
| [03-tech-architecture.md](../All_Phase/tai-lieu-thiet-ke/03-tech-architecture.md) | Bảng stack (1.1), Auth (4.1.5) |
| [04-ops-security-compliance.md](../All_Phase/tai-lieu-thiet-ke/04-ops-security-compliance.md) | Security (5), Observability (5) |
| [database/README.md](../../database/README.md) | Tổng quan DB |
| [database/DESIGN_DECISIONS.md](../../database/DESIGN_DECISIONS.md) | Quyết định thiết kế DB |
