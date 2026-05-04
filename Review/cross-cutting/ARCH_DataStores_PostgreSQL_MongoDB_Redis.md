# Data Stores PostgreSQL, MongoDB, Redis

## 1. Source đã đọc thủ công

- `Backend/src/TarotNow.Infrastructure/Persistence/ApplicationDbContext.cs`
- `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`
- `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
- `database/postgresql/schema.sql`
- `database/mongodb/schema.md`
- `database/mongodb/init.js`

## 2. PostgreSQL — transactional write model

`ApplicationDbContext.cs` dùng EF Core/Npgsql (`UseNpgsql`) và khai báo các DbSet runtime sau:

- Identity/auth/legal/admin: `Users`, `RefreshTokens`, `AuthSessions`, `EmailOtps`, `UserConsents`, `SystemConfigs`.
- Finance/chat finance: `WalletTransactions`, `DepositOrders`, `DepositPromotions`, `ChatFinanceSessions`, `ChatQuestionItems`, `WithdrawalRequests`.
- AI/reading: `AiRequests`, `ReadingRevealSagaStates`.
- Gacha/inventory: `GachaPools`, `GachaPoolRewardRates`, `GachaPullOperations`, `GachaPullRewardLogs`, `UserGachaPities`, `GachaHistoryEntries`, `ItemDefinitions`, `UserItems`, `InventoryItemUseOperations`, `FreeDrawCredits`, `InventoryLuckEffects`.
- Outbox: `OutboxMessages`, `OutboxHandlerStates`, `OutboxInlineHandlerStates`.

`OnConfiguring` cố định migration history table là `__ef_migrations_history`. `OnModelCreating` áp entity configurations, sensitive field encryption và snake_case convention. `WithdrawalRequest.BankAccountName` và `BankAccountNumber` được protect/unprotect qua `ISensitiveDataProtector`.

## 3. MongoDB — document/read-model/high-volume store

`MongoDbContext.cs` khai báo collection runtime cụ thể:

- Reading/card/collection: `cards_catalog`, `user_collections`, `reading_sessions`.
- Check-in/AI/auth/notification: `daily_checkins`, `ai_provider_logs`, `notifications`, `refresh_tokens`.
- Reader: `reader_requests`, `reader_profiles`.
- Chat/review/report: `conversations`, `chat_messages`, `conversation_reviews`, `reports`.
- Community/upload/media: `community_posts`, `community_reactions`, `community_comments`, `upload_sessions`, `community_media_assets`.
- Gamification: `quests`, `quest_progress`, `achievements`, `user_achievements`, `titles`, `user_titles`, `leaderboard_entries`, `leaderboard_snapshots`.

Important naming: runtime collection cho review hội thoại là `conversation_reviews` trong `MongoDbContext.cs`; không gọi chung là `reviews` trong review docs trừ khi đang nói về mismatch với tài liệu schema cũ.

Constructor `MongoDbContext` chạy `EnsureIndexes()` lúc startup. Nếu index bootstrap lỗi ở non-development environment, code throw `InvalidOperationException`; development được fallback để app còn chạy.

## 4. Redis — cache/realtime/coordination

`DependencyInjection.Cache.cs` luôn thêm memory cache, sau đó thử tạo Redis multiplexer từ `ConnectionStrings:Redis`.

Rule runtime đáng chú ý:

- Nếu có Redis connection string mà thiếu `Redis:InstanceName`, app throw `InvalidOperationException`.
- Nếu `ASPNETCORE_ENVIRONMENT=Production` mà Redis multiplexer không tạo được, app throw `InvalidOperationException` với message Redis required for realtime consistency.
- Nếu không phải Production và Redis không khả dụng, code fallback `AddDistributedMemoryCache()`.
- `IRedisPublisher` đăng ký `RedisPublisher` khi có multiplexer, fallback `NoOpRedisPublisher` khi không có.
- `IChatRealtimeFastLanePublisher` tương tự: Redis implementation hoặc NoOp fallback.

## 5. Cross-store consistency risk

- PostgreSQL là nơi phải giữ ACID cho money, escrow, deposit, withdrawal, entitlement/quota-like state và operation logs.
- MongoDB phù hợp document/read-model/high-volume; không nên dùng làm source duy nhất cho mutation tiền hoặc invariant cần ACID.
- Outbox DbSet trong PostgreSQL là cầu nối consistency cho side effects async.
- Redis pub/sub/cache là hạ tầng runtime; Production fail-fast nếu Redis thiếu để tránh realtime/cache inconsistency.

## 6. Rủi ro

- P0: mutation tiền hoặc settlement chỉ ghi MongoDB/Redis mà không có PostgreSQL transaction; Production chạy không Redis dù flow realtime yêu cầu; docs/feature gọi sai collection `conversation_reviews`.
- P1: state được split PostgreSQL/MongoDB nhưng thiếu outbox/reconcile/saga evidence; schema docs lệch runtime DbContext.
- P2: Review feature docs không phân biệt rõ table/collection/cache key hoặc chỉ nói “data store liên quan” chung chung.

## 7. Verify khi review PR

- Đối chiếu schema/code bằng `ApplicationDbContext.cs`, `MongoDbContext.cs`, `database/postgresql/schema.sql`, `database/mongodb/schema.md`.
- Với finance/reading/gacha/inventory, luôn đọc EF entity/config/repository trước khi kết luận transaction boundary.
- Với realtime/cache, đọc `DependencyInjection.Cache.cs` và nơi publish `IRedisPublisher`.
