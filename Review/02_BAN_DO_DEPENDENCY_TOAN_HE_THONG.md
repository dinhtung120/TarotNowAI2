# Bản đồ dependency toàn hệ thống

## Backend compile-time graph

```text
TarotNow.Domain <- TarotNow.Application <- TarotNow.Infrastructure
                         ^                       ^
                         |                       |
                     TarotNow.Api ---------------+
```

Evidence:

- `Backend/src/TarotNow.Domain/TarotNow.Domain.csproj`
- `Backend/src/TarotNow.Application/TarotNow.Application.csproj`
- `Backend/src/TarotNow.Infrastructure/TarotNow.Infrastructure.csproj`
- `Backend/src/TarotNow.Api/TarotNow.Api.csproj`
- `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs`

## Backend write dependency graph

```text
API Controller
  -> MediatR Command
  -> thin CommandHandler
  -> IInlineDomainEventDispatcher
  -> *RequestedDomainEventHandler
  -> Application-owned interfaces/repositories/services
  -> Infrastructure implementations
  -> PostgreSQL/MongoDB/Redis/provider adapters
  -> Domain events/outbox/realtime bridge
```

Evidence:

- `Backend/src/TarotNow.Application/Interfaces/IInlineDomainEventDispatcher.cs`
- `Backend/tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs`
- `Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox`
- `Backend/src/TarotNow.Application/Interfaces/IRedisPublisher.cs`
- `Backend/src/TarotNow.Infrastructure/DomainEvents/Handlers`

## Backend read/query dependency graph

```text
API Controller -> MediatR Query/Application service -> repository/query abstraction -> EF Core/Mongo/Redis implementation -> DTO/result
```

Examples reviewed:

- `WalletController.cs` -> `GetWalletBalanceQueryHandler.cs`, `GetLedgerListQueryHandler.cs`.
- `HomeController.cs` -> public home snapshot query path.
- `HistoryController.cs` -> reading history query path with ownership/admin access.
- `NotificationController*.cs` -> Mongo notification list/read state.

## Frontend route dependency graph

```text
Frontend/src/app/[locale]/page.tsx
  -> @/features/<feature>/public
  -> feature presentation component
  -> feature application action/hook
  -> app API proxy or shared server action
  -> Backend API
```

Evidence:

- Public exports: `Frontend/src/features/*/public.ts`.
- App routes: `Frontend/src/app/[locale]`.
- App API proxy: `Frontend/src/app/api`.
- SSR hydration: `Frontend/src/shared/server/prefetch`.
- Query key sources: `Frontend/src/shared/infrastructure/query`, feature query-key files.
- Guard: `Frontend/scripts/check-clean-architecture.mjs`.

## Data dependency graph

- PostgreSQL is the ACID/write-model store for identity, finance, entitlement/reward, legal/admin, AI request/audit and selected operational state. Evidence: `Backend/src/TarotNow.Infrastructure/Persistence/ApplicationDbContext.cs`, `database/postgresql/schema.sql`.
- MongoDB is the document/read-model/high-volume store for reading sessions, chat messages/conversations, reader profiles/requests, notifications, community, gamification, uploads and `conversation_reviews`. Evidence: `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`, `database/mongodb/schema.md`.
- Redis supports cache, rate limiting/coordination and realtime Pub/Sub bridge. Evidence: backend cache DI and `IRedisPublisher`/realtime bridge contracts.

## Ops dependency graph

- Local/dev service graph: `docker-compose.yml`.
- Production service graph/healthchecks: `docker-compose.prod.yml`.
- Deployment/smoke/rollback/backup/restore: `deploy/scripts/*`.
- CI/CD workflows: `.github/workflows/cd-main-3ec2.yml`, `cd-fast-deploy.yml`, `cd-fe-only-deploy.yml`.
