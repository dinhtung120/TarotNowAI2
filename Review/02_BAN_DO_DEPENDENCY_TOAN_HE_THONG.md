# Bản đồ dependency toàn hệ thống

## Backend compile-time graph

```text
TarotNow.Domain <- TarotNow.Application <- TarotNow.Infrastructure
                         ^                       ^
                         |                       |
                     TarotNow.Api ---------------+
```

Evidence cần đối chiếu:
- `Backend/src/TarotNow.Domain/TarotNow.Domain.csproj`
- `Backend/src/TarotNow.Application/TarotNow.Application.csproj`
- `Backend/src/TarotNow.Infrastructure/TarotNow.Infrastructure.csproj`
- `Backend/src/TarotNow.Api/TarotNow.Api.csproj`
- `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs`

## Backend write flow chuẩn

```text
API Controller -> MediatR Command -> thin CommandHandler -> IInlineDomainEventDispatcher -> *RequestedDomainEventHandler -> Application interfaces -> Infrastructure implementations -> DomainEvent/Outbox/Redis bridge
```

Evidence:
- Interface inline dispatcher: `Backend/src/TarotNow.Application/Interfaces/IInlineDomainEventDispatcher.cs`.
- Architecture guard: `Backend/tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs`.
- Outbox/background infrastructure: `Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox`.
- Redis publisher contract: `Backend/src/TarotNow.Application/Interfaces/IRedisPublisher.cs`.

## Frontend graph

```text
Frontend/src/app/[locale] -> Frontend/src/features/*/public.ts -> feature application/domain/presentation -> Frontend/src/shared
```

Evidence:
- Feature public exports: `Frontend/src/features/*/public.ts`.
- App routes: `Frontend/src/app/[locale]`.
- SSR prefetch: `Frontend/src/shared/server/prefetch`.
- Guard: `Frontend/scripts/check-clean-architecture.mjs`.

## Data graph

- PostgreSQL: ACID write model cho identity, finance, entitlement, admin/legal, AI request/audit, gacha/inventory transactional state. Evidence: `ApplicationDbContext.cs`, `database/postgresql/schema.sql`.
- MongoDB: document/read-model/high-volume cho reading sessions, chat, reader profiles/requests, notifications, community, gamification documents, upload sessions. Evidence: `MongoDbContext.cs`, `database/mongodb/schema.md`.
- Redis: cache, rate-limit/coordination, realtime Pub/Sub, production-required cache. Evidence: `DependencyInjection.Cache.cs`, `IRedisPublisher.cs`.

## Ops graph

- Local/dev services: `docker-compose.yml`.
- Production service graph and healthchecks: `docker-compose.prod.yml`.
- Bootstrap/smoke/rollback/backup/restore: `deploy/scripts/*`.
- CI/CD: `.github/workflows/cd-main-3ec2.yml`, `cd-fast-deploy.yml`, `cd-fe-only-deploy.yml`.
