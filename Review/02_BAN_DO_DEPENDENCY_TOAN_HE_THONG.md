# Bản đồ dependency toàn hệ thống

## Backend layer dependency

```text
Domain <- Application <- Infrastructure <- API
```

- `Backend/src/TarotNow.Domain`: entity, value object, domain event contract, invariant.
- `Backend/src/TarotNow.Application`: CQRS entry point, requested domain event handler, Application-owned interface.
- `Backend/src/TarotNow.Infrastructure`: implementation cho persistence, cache, provider, outbox, Redis Pub/Sub, background worker.
- `Backend/src/TarotNow.Api`: controller, composition, middleware, SignalR bridge wiring.

## Backend command/event flow chuẩn

```text
Controller -> MediatR Command -> thin CommandHandler -> IInlineDomainEventDispatcher -> RequestedDomainEventHandler -> repository/provider/transaction -> DomainEvent/Outbox -> async handler/worker/bridge
```

## Frontend dependency

```text
app/[locale] route -> features/*/public.ts -> feature presentation/application/domain -> shared primitives/lib
```

Điểm cần review kỹ: `shared/server/prefetch` có thể điều phối nhiều feature để SSR prefetch; cần xác nhận coupling này là chủ đích và không biến shared thành nơi chứa business logic feature-specific.

## Data dependency

- PostgreSQL: transactional source-of-truth cho finance, escrow, subscription, entitlement và state cần ACID.
- MongoDB: document/read-model cho reading sessions, chat messages, card catalog, notification/profile tùy module.
- Redis: cache, rate limit, pub/sub, presence, coordination.

## Ops dependency

- Docker compose điều phối frontend, backend, postgres, mongodb, redis, reverse proxy.
- Deploy scripts chịu trách nhiệm bootstrap, smoke, rollback, backup, restore.
- GitHub workflows cần được review cùng với smoke path và rollback path.
