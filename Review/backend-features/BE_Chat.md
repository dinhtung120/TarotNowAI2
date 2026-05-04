# BE Chat

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Chat`
- Controllers: `Backend/src/TarotNow.Api/Controllers/ConversationController*.cs`
- Tests: `Backend/tests/TarotNow.Application.UnitTests/Features/Chat/*`, `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/ChatModerationOutboxIntegrationTests.cs`
- Datastore/runtime: `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`, `Backend/src/TarotNow.Infrastructure/Persistence/ApplicationDbContext.cs`, `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
- Guards: `Backend/tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs`

## Entry points & luồng chính

Chat là bounded context lớn, entry qua `ConversationController.cs` và các partial controller:

- `ConversationController.Messages.cs`: message flow.
- `ConversationController.Finance.cs`: add money / finance operations.
- `ConversationController.Acceptance.cs`: accept/reject conversation.
- `ConversationController.Completion.cs`: complete conversation.
- `ConversationController.Inbox.cs`: inbox/unread/listing.
- `ConversationController.Review.cs`: conversation review.
- `ConversationController.MediaUpload.cs`: presign/upload media.

Application commands đã thấy trong `Features/Chat/Commands`:

- Conversation lifecycle: `CreateConversation`, `AcceptConversation`, `RejectConversation`, `CancelPendingConversation`.
- Messaging: `SendMessage`, `MarkMessagesRead`, `PublishTypingState`, `PresignConversationMedia`.
- Finance in chat: `RequestConversationAddMoney`, `RespondConversationAddMoney`, `RequestConversationComplete`, `RespondConversationComplete`.
- Moderation/report/review: `CreateReport`, `OpenConversationDispute`, `SubmitConversationReview`.

Queries đã thấy:

- `ListConversations`, `ListMessages`, `GetUnreadTotal`, `GetParticipantConversationIds`, `GetConversationParticipants`.

Nhiều command folder có `*CommandHandler.EventOnly.cs`, phù hợp với architecture rule command entry handler mỏng. Một số folder vẫn có các partial workflow/helper như `SendMessageCommandHandler.FirstMessageFreeze.*`, `RequestConversationCompleteCommandHandler.Settlement.cs`, `RespondConversationCompleteCommandHandler.Settlement.cs`; đây là nơi phải audit orchestration thực tế trong requested-event handler/partial flow.

## Dependency và dữ liệu

MongoDB runtime collections trong `MongoDbContext.cs` liên quan trực tiếp:

- `conversations`
- `chat_messages`
- `conversation_reviews`
- `reports`
- `upload_sessions` nếu media upload dùng one-time upload session.

PostgreSQL runtime DbSet liên quan finance chat trong `ApplicationDbContext.cs`:

- `ChatFinanceSessions`
- `ChatQuestionItems`
- `WalletTransactions`
- Outbox tables: `OutboxMessages`, `OutboxHandlerStates`, `OutboxInlineHandlerStates`.

Redis/realtime liên quan qua `IRedisPublisher` và `IChatRealtimeFastLanePublisher` đăng ký trong `DependencyInjection.Cache.cs`. Production yêu cầu Redis; không thể coi realtime Redis là optional trong môi trường production.

## Boundary / guard

- `EventDrivenArchitectureRulesTests.CommandHandlers_ShouldOnlyDependOnInlineDomainEventDispatcher` áp dụng cho toàn bộ command handlers trong `Features/Chat/Commands`.
- `Controllers_ShouldNotBroadcastRealtimeDirectly` cấm `ConversationController*.cs` dùng `IHubContext` hoặc `.Clients.`.
- `Hubs_ShouldNotBroadcastMigratedRealtimeEventsDirectly` cấm hub gửi trực tiếp `message.created`, `conversation.updated`, `chat.unread_changed`.
- Finance paths trong Chat phải được review cùng wallet mutation/money event/idempotency vì có add money, freeze/settlement và completion.

## Test coverage hiện có

Tests đã thấy rõ:

- `CreateConversationCommandHandlerTests.cs`
- `SendMessageCommandHandlerTests.cs`
- `MarkMessagesReadCommandHandlerTests.cs`
- `RespondConversationAddMoneyCommandHandlerTests.cs`
- `ConversationAddMoneyAcceptedSyncRequestedDomainEventHandlerTests.cs`
- `ConversationCompleteSessionSettlementTests.cs`
- `OpenConversationDisputeCommandHandlerTests.cs`
- `CreateReportCommandHandlerTests.cs`
- `PresignConversationMediaCommandHandlerTests.cs`
- `ChatModerationRequestedDomainEventHandlerTests.cs`
- `ChatModerationOutboxIntegrationTests.cs`

## Rủi ro

- P0: realtime broadcast trực tiếp từ controller/hub; settlement/add-money thiếu transaction/idempotency; message first-freeze hoặc completion settlement double-spend; command handler không còn mỏng.
- P1: unread/inbox/list enrichment coupling quá mạnh; media upload session thiếu ownership/expiry evidence; moderation outbox thiếu retry/idempotency evidence.
- P2: docs hoặc tests không phân biệt `conversation_reviews` với collection generic `reviews`.

## Kết luận

Chat có evidence code/test khá dày và là module rủi ro cao vì kết hợp MongoDB document state, PostgreSQL finance state, Redis realtime và outbox. Review PR liên quan Chat phải đọc controller partial + command folder cụ thể + tests tương ứng, không chỉ dựa vào feature-level summary.
