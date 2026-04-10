# TarotNow

> 🔮 Nền tảng xem bài Tarot trực tuyến tích hợp AI, hỗ trợ đa ngôn ngữ (VI/EN/ZH).

## Kiến trúc

Dự án áp dụng **Clean Architecture** với 3 phần chính:

| Phần | Công nghệ | Mô tả |
|------|-----------|-------|
| **Backend** | ASP.NET Core 9 | Web API, MediatR CQRS, JWT Auth |
| **Frontend** | Next.js 16 | App Router, SSR, Tailwind CSS |
| **Mobile** | React Native | (Planned) |

### Backend (Clean Architecture 4 layers)
```
Backend/src/
├── TarotNow.Domain          # Entities, ValueObjects, Enums
├── TarotNow.Application     # CQRS Handlers, DTOs, Validators
├── TarotNow.Infrastructure  # EF Core, MongoDB, Redis, Auth
└── TarotNow.Api             # Controllers, Middlewares, Swagger
```

### Database
- **PostgreSQL**: Finance, Auth, Subscriptions (ACID)
- **MongoDB**: Content, Chat, Logs, Gamification
- **Redis**: Caching, Rate Limiting

## Bắt đầu nhanh

### Prerequisites
- .NET 9 SDK
- Node.js 20+
- PostgreSQL 16+
- MongoDB 7+
- Redis 7+

### Setup
```bash
# 1. Clone & config
cp .env.example .env
cp Backend/.env.example Backend/.env
cp Frontend/.env.example Frontend/.env.local
# Chỉnh sửa Backend/.env và Frontend/.env.local với giá trị thật
# Thiếu biến bắt buộc -> ứng dụng sẽ fail-fast khi khởi động/build

# 2. Database
psql -h localhost -U postgres -d tarotweb -f database/postgresql/schema.sql
mongosh mongodb://localhost:27017/tarotweb < database/mongodb/init.js

# 3. Backend
cd Backend
dotnet restore
dotnet run --project src/TarotNow.Api

# 4. Frontend
cd Frontend
npm install
npm run dev
```

### Quy ước file cấu hình

| File | Subsystem | Mục đích |
|------|-----------|----------|
| `.env` | Docker Compose (root) | Chỉ cấp biến cho `docker-compose.yml` |
| `Backend/.env` | Backend API | Secrets và runtime config của ASP.NET Core |
| `Frontend/.env.local` | Frontend Next.js | URL API và biến môi trường frontend |

### Verify
```bash
# Health check
curl http://localhost:5037/api/v1/health

# Swagger UI
open http://localhost:5037/swagger

# Frontend
open http://localhost:3000
```

## Production (EC2 + Docker Compose)
```bash
# 1) Chuẩn bị biến môi trường production
cp deploy/.env.prod.example deploy/.env.prod

# 2) Bootstrap DB lần đầu (start data services + bootstrap scripts)
./deploy/scripts/bootstrap-db.sh deploy/.env.prod docker-compose.prod.yml

# 3) Build + khởi chạy app stack production
docker compose --env-file deploy/.env.prod -f docker-compose.prod.yml up -d --build backend frontend reverse-proxy

# 4) Smoke test
./deploy/scripts/smoke.sh http://localhost
```

Chi tiết vận hành backup/restore/rollback xem `deploy/README.md`.

## Tài liệu
Xem thêm trong thư mục `Doccument/` và `docs/`.

## License
Private – All rights reserved.
