# BE Reading

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Reading`
- Controllers: `Backend/src/TarotNow.Api/Controllers/TarotController.cs`, `Backend/src/TarotNow.Api/Controllers/AiController.cs`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Reading/InitReadingSessionCommandHandlerTests.cs`, `RevealReadingSessionCommandHandlerTests.cs`, `AiStreamFinalStatusesTests.cs`
- Datastore: `ApplicationDbContext.cs` DbSet `AiRequests`, `ReadingRevealSagaStates`; `MongoDbContext.cs` collections `reading_sessions`, `cards_catalog`, `user_collections`, `ai_provider_logs`
- Guards: `EventDrivenArchitectureRulesTests.cs`, `ArchitectureBoundariesTests.cs`, `ApiAndConfigurationStandardsTests.cs`

## Entry points & luồng chính

Reading có hai HTTP boundary chính:

- `TarotController.cs`: reading/session/catalog/collection REST endpoints.
- `AiController.cs`: AI stream SSE và stream-ticket endpoints.

`TarotController.cs` expose các path quan trọng:

- `POST init` → `InitReadingSessionCommand`.
- `POST reveal` → `RevealReadingSessionCommand`.
- `GET cards-catalog`, `GET cards-catalog/manifest`, `GET cards-catalog/chunks/{chunkId:int}`, `GET cards-catalog/details/{cardId:int}`.
- `GET collection` → `GetUserCollectionQuery`.

`TarotController` override `UserId` từ token cho init/reveal/collection, nên review không được coi `UserId` từ payload là trusted input.

`AiController.cs` expose:

- `GET {sessionId}/stream`: chuẩn bị request qua `IAiStreamEndpointService`, sau đó chạy SSE qua `IAiStreamSseOrchestrator`.
- `POST {sessionId}/stream-ticket`: tạo opaque stream token cho follow-up để không đưa prompt nhạy cảm trực tiếp lên EventSource URL.

## Application flow

Commands trong `Features/Reading/Commands`:

- `InitSession`: `InitReadingSessionCommandHandler` là thin handler, publish `ReadingSessionInitRequestedDomainEvent` qua `IInlineDomainEventDispatcher` và trả `SessionId`, `CostGold`, `CostDiamond` từ event.
- `RevealSession`: `RevealReadingSessionCommandHandler` publish `ReadingSessionRevealRequestedDomainEvent`, normalize language về `vi/en/zh`, trả `RevealedCards` từ event.
- `StreamReading`: `StreamReadingCommandHandler.EventOnly.cs` là thin handler; orchestration nằm trong `StreamReadingCommandHandlerRequestedDomainEventHandler`.
- `CompleteAiStream`: `CompleteAiStreamCommandHandler.EventOnly.cs` là thin handler; orchestration nằm trong `CompleteAiStreamCommandHandlerRequestedDomainEventHandler`.

AI stream orchestration trong `StreamReadingCommandHandler.cs` đã đọc gồm:

- validate session;
- enforce reading rate limit;
- tính cost bằng pricing service;
- resolve idempotency key;
- reserve/create AI request;
- freeze wallet escrow nếu có phí;
- build prompt bằng `IReadingPromptService`;
- gọi `IAiProvider.StreamChatAsync`;
- publish `MoneyChangedDomainEvent` khi freeze diamond.

Completion orchestration trong `CompleteAiStreamCommandHandler.cs` và `CompleteAiStreamCommandHandler.WalletAndTelemetry.cs` đã đọc gồm:

- chỉ chấp nhận final status từ `AiStreamFinalStatuses`;
- chạy billing/session update trong `ITransactionCoordinator.ExecuteAsync`;
- consume escrow với idempotency key `consume_{record.Id}`;
- refund escrow với idempotency key `refund_{record.Id}`;
- publish `MoneyChangedDomainEvent` với `EscrowRefund` hoặc `EscrowRelease`;
- publish `ReadingBillingCompletedDomainEvent` và telemetry event best-effort.

## Dependency và dữ liệu

Runtime state chính:

- PostgreSQL `AiRequests`: record AI request, charge, final status, settlement/refund reference.
- PostgreSQL `ReadingRevealSagaStates`: saga/reveal state theo DbSet đã thấy trong `ApplicationDbContext`.
- MongoDB `reading_sessions`: reading session document.
- MongoDB `cards_catalog`: tarot card catalog.
- MongoDB `user_collections`: collection của user.
- MongoDB `ai_provider_logs`: provider log/observability cho AI.
- Wallet state/ledger: qua `IWalletRepository` và `MoneyChangedDomainEvent` khi stream/finalize có charge.

Application handler phụ thuộc vào Application-owned abstractions như `IReadingSessionRepository`, `IAiRequestRepository`, `IWalletRepository`, `IAiProvider`, `ICacheService`, `ITransactionCoordinator`, `IReadingPromptService`, `ISystemConfigSettings`, `IDomainEventPublisher`. Không ghi docs rằng handler phụ thuộc concrete Infrastructure nếu chưa thấy evidence.

## Boundary / guard

- Reading write command entry handlers phải mỏng và chỉ dùng `IInlineDomainEventDispatcher`, khớp Rule 0 và `EventDrivenArchitectureRulesTests.cs`.
- AI provider call nằm trong requested-domain-event handler, không nằm trực tiếp trong controller.
- SSE endpoint phải tránh sensitive payload trên URL; `AiController.CreateStreamTicket` là evidence hiện có cho follow-up tokenization.
- Finance/AI billing phải review theo idempotency + transaction + wallet money event.
- Catalog endpoints `cards-catalog/*` là `[AllowAnonymous]`, còn init/reveal/collection/AI stream nằm sau `[Authorize]` ở controller level.

## Test coverage hiện có

- `InitReadingSessionCommandHandlerTests.cs`: xác nhận init command publish `ReadingSessionInitRequestedDomainEvent` và trả kết quả từ event handler.
- `RevealReadingSessionCommandHandlerTests.cs`: xác nhận reveal command publish `ReadingSessionRevealRequestedDomainEvent`, normalize language và map cards từ event.
- `AiStreamFinalStatusesTests.cs`: bảo vệ whitelist final statuses `completed`, `failed_before_first_token`, `failed_after_first_token`.

Evidence đã đọc chưa chứng minh đầy đủ integration test cho toàn bộ REST/SSE Reading API trong file này. Khi review sâu cần đối chiếu thêm `Backend/tests/TarotNow.Api.IntegrationTests/*Ai*/*Reading*` nếu có, đặc biệt cho stream-ticket, EventSource URL, settlement/refund và retry/idempotency.

## Rủi ro

- P0: gọi AI trước khi reserve/freeze quota/wallet; completion double consume/refund; final status không quyết định đúng settlement/refund; stream URL chứa prompt/follow-up nhạy cảm.
- P0: command entry handler Reading bị đổi sang inject repository/provider trực tiếp, vi phạm event-driven architecture.
- P1: thiếu integration coverage cho SSE stream-ticket và API ownership; catalog chunk/version/cache contract thay đổi nhưng không có test tương ứng.
- P1: telemetry/gamification/reward side effects chạy trong transaction chính hoặc làm fail nghiệp vụ billing.
- P2: docs nhầm `cards_catalog`/`reading_sessions` với bảng PostgreSQL thay vì Mongo collections.

## Kết luận

Reading là module rủi ro cao vì kết hợp MongoDB session/catalog, PostgreSQL AI billing/saga state, wallet escrow và external AI provider streaming. Review đúng phải đọc cả `TarotController`, `AiController`, command entry handlers, requested-domain-event handlers và tests final-status/idempotency/billing trước khi kết luận thay đổi an toàn.
