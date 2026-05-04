# Event-driven, Outbox, Realtime Bridge

## 1. Source đã đọc thủ công

- `Backend/tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs`
- `Backend/src/TarotNow.Application/Interfaces/IInlineDomainEventDispatcher.cs`
- `Backend/src/TarotNow.Application/Interfaces/IDomainEventPublisher.cs`
- `Backend/src/TarotNow.Application/Interfaces/IRedisPublisher.cs`
- `Backend/src/TarotNow.Infrastructure/Services/InlineMediatRDomainEventDispatcher.cs`
- `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs`
- `Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox`
- `Backend/src/TarotNow.Infrastructure/DomainEvents/Handlers`
- `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`

## 2. Command model đang được enforce

`EventDrivenArchitectureRulesTests.CommandHandlers_ShouldOnlyDependOnInlineDomainEventDispatcher` quét toàn bộ `Backend/src/TarotNow.Application/Features/**/Commands/**/*.cs`. Class implement `IRequestHandler<,>` trong thư mục `Commands` nhưng không phải `*RequestedDomainEventHandler` chỉ được có constructor với đúng một dependency: `IInlineDomainEventDispatcher`.

Ý nghĩa thực tế:

```text
Controller -> MediatR Command -> thin CommandHandler -> IInlineDomainEventDispatcher -> *RequestedDomainEventHandler
```

Command entry handler không được inject repository, provider, wallet service, notification service, realtime broadcaster hoặc infrastructure component. Use-case orchestration phải nằm trong requested domain event handler.

## 3. Legacy command-dispatch đã bị cấm

Các test sau khóa đường legacy:

- `Application_ShouldNotContainCommandExecutorClasses`: cấm class `*CommandExecutor` trong Application.
- `Application_ShouldNotReferenceICommandExecutionExecutorContract`: cấm interface/type `ICommandExecutionExecutor`.
- `LegacyCommandDispatchFolder_ShouldBeEmpty`: thư mục `Backend/src/TarotNow.Application/DomainEvents/Handlers/CommandDispatch` phải rỗng nếu tồn tại.

Khi review feature docs, nếu thấy flow còn mô tả executor/command-dispatch wrapper thì phải sửa vì không còn đúng với guard hiện tại.

## 4. Realtime boundary

`Controllers_ShouldNotBroadcastRealtimeDirectly` cấm controllers trong `Backend/src/TarotNow.Api/Controllers` dùng `IHubContext<...>` hoặc `.Clients.`. Kết luận: controller không được broadcast SignalR trực tiếp.

`Hubs_ShouldNotBroadcastMigratedRealtimeEventsDirectly` cấm hub `SendAsync` trực tiếp các event đã migrate:

- `notification.new`
- `wallet.balance_changed`
- `message.created`
- `conversation.updated`
- `chat.unread_changed`
- `gacha.result`
- `gamification.quest_completed`
- `gamification.achievement_unlocked`
- `gamification.card_level_up`

Realtime path đúng phải đi qua domain event/outbox/Redis publisher và bridge ở edge. Redis publisher được đăng ký trong `DependencyInjection.Cache.cs` qua `IRedisPublisher` và `IChatRealtimeFastLanePublisher`; khi không có multiplexer, code đăng ký NoOp publisher, nhưng Production yêu cầu Redis connect được.

## 5. Outbox và side effects

Outbox persistence nằm trong `ApplicationDbContext` với DbSet:

- `OutboxMessages`
- `OutboxHandlerStates`
- `OutboxInlineHandlerStates`

Worker/batch xử lý outbox nằm ở `Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox`. Domain event handlers nằm ở `Backend/src/TarotNow.Infrastructure/DomainEvents/Handlers`.

Review rule: notification, realtime, gamification, wallet balance fan-out và side effects phụ không được gọi trực tiếp từ controller hoặc command entry handler. Tài liệu feature phải chỉ ra event/outbox/handler path nếu feature phát side effect.

## 6. Money/idempotency boundary

`EventDrivenArchitectureRulesTests.WalletMutationCommands_ShouldPublishMoneyChangedDomainEvent` bắt command wallet mutation publish `MoneyChangedDomainEvent` trong cùng module command. Vì test quét command files, feature docs cho Wallet/Deposit/Withdrawal/Escrow/Chat finance phải ghi rõ canonical money event path hoặc ghi gap nếu chưa chứng minh được.

Các finance/AI/reward features cần review thêm deterministic idempotency key ở command và requested-event level nếu mutation liên quan tiền/quota/AI/reward.

## 7. Rủi ro

- P0: command handler inject dependency khác `IInlineDomainEventDispatcher`; controller/hub broadcast realtime trực tiếp; wallet mutation thiếu `MoneyChangedDomainEvent`; side effect phụ không qua event/outbox.
- P1: requested event handler orchestration đúng layer nhưng thiếu test cho transaction/idempotency/retry/refund path.
- P2: feature doc chỉ nói “có realtime/outbox” nhưng không dẫn path cụ thể tới handler, event hoặc Redis publisher.

## 8. Verify khi review PR

- Chạy architecture tests nếu chạm command handler, controller, hub, realtime, wallet mutation.
- Dùng `git grep` thủ công cho feature đang review: `RequestedDomainEvent`, `IInlineDomainEventDispatcher`, `MoneyChangedDomainEvent`, `IRedisPublisher`, `Outbox`.
