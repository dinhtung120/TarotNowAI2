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
# Chỉnh sửa .env với giá trị thật

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

### Verify
```bash
# Health check
curl http://localhost:5000/api/v1/health

# Swagger UI
open http://localhost:5000/swagger

# Frontend
open http://localhost:3000
```

## Tài liệu
Xem thêm trong thư mục `Doccument/` và `docs/`.

## License
Private – All rights reserved.
