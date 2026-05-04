# Auth, Security, MFA

## Evidence đã rà

- Backend features: `Backend/src/TarotNow.Application/Features/Auth`, `Mfa`, `UserContext`, `Legal`, `Admin`.
- PostgreSQL auth/legal tables: `database/postgresql/schema.sql`.
- Mongo refresh token collection: `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`.
- API/security guards: `Backend/tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs`.
- Frontend auth guard: `Frontend/scripts/check-auth-fail-closed.mjs`.
- Env/security config: `.env.example`, `docker-compose.prod.yml`.

## Kết luận

Auth/security trải qua cả backend và frontend: backend quản token/session/MFA/legal consent; frontend route/API wrapper phải fail-closed. Security review phải gắn với rate-limit, policy ownership, cookie/JWT handling, refresh rotation và data-rights/legal audit.

## Data và state

- PostgreSQL: users/session/otp/password reset/consent/data rights/admin action.
- MongoDB: refresh token document nếu runtime map vẫn dùng collection này.
- Redis: rate-limit/cache/session coordination tùy cấu hình.

## Rủi ro

- P0: fail-open auth route; thiếu ownership check; token/cookie unsafe; secret logging; sensitive endpoint thiếu rate-limit metadata theo guard.
- P1: consent/data-right flow thiếu audit trail hoặc tests.
- P2: i18n/auth copy thiếu đầy đủ locale.
