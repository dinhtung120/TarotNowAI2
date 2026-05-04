# Event-driven, Outbox, Realtime Bridge

## Evidence đã rà

- `Backend/src/TarotNow.Application/Interfaces/IInlineDomainEventDispatcher.cs`
- `Backend/src/TarotNow.Application/Interfaces/IDomainEventPublisher.cs`
- `Backend/src/TarotNow.Application/Interfaces/IRedisPublisher.cs`
- `Backend/src/TarotNow.Infrastructure/Services/InlineMediatRDomainEventDispatcher.cs`
- `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs`
- `Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox`
- `Backend/src/TarotNow.Infrastructure/DomainEvents/Handlers`
- `Backend/tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs`

## Mô hình thực tế

Write command entry handler trong Application phải là thin handler: nhận command, dispatch requested domain event qua `IInlineDomainEventDispatcher`, trả result. Orchestration nghiệp vụ nằm trong `*RequestedDomainEventHandler`, nơi được phép dùng Application interfaces như repositories, provider abstractions, cache, transaction coordinator và domain event publisher.

## Side effect path

- Side effects phụ như notification, realtime, gamification, task, email phải đi qua domain event/outbox/handler hoặc Infrastructure worker.
- Realtime path dùng Redis publisher/bridge và SignalR ở edge, không broadcast trực tiếp từ controller nếu không có allowlist.
- Money state mutation phải review cùng canonical money event và idempotency contract.

## Guard hiện có

- `EventDrivenArchitectureRulesTests.cs` enforce command handler dependency, controller realtime restrictions, hub event restrictions, money/idempotency-related contracts.

## Rủi ro

- P0: command handler inject repository/provider/side-effect service trực tiếp; realtime direct broadcast; finance mutation thiếu money event/idempotency.
- P1: event handler làm quá nhiều orchestration phụ thay vì publish event tiếp theo.
- P2: thiếu evidence test cho một requested event handler cụ thể.
