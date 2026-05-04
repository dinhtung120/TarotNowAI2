# Data Stores PostgreSQL, MongoDB, Redis

## PostgreSQL

Evidence:
- `Backend/src/TarotNow.Infrastructure/Persistence/ApplicationDbContext.cs`
- `database/postgresql/schema.sql`
- `database/DATABASE_OVERVIEW.md`

Vai trò: ACID write model cho identity, finance, subscription/entitlement, legal/admin, AI transactional trace, gacha/inventory transactional state.

Bảng/module tiêu biểu:
- Identity/legal: `users`, `auth_sessions`, `email_otps`, `password_reset_tokens`, `user_consents`, `data_rights_requests`, `admin_actions`.
- Finance: `wallet_transactions`, `deposit_orders`, `deposit_promotions`, `chat_finance_sessions`, `chat_question_items`, `withdrawal_requests`, `reader_payout_profiles`.
- Reading/AI: `ai_requests`, `reading_rng_audits`, `reading_reveal_saga_states`.
- Engagement: `gacha_*`, `item_definitions`, `user_items`, `inventory_*`, `free_draw_credits`.

## MongoDB

Evidence:
- `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`
- `database/mongodb/schema.md`
- `database/mongodb/init.js`

Vai trò: document/read-model/high-volume store.

Collections/module tiêu biểu:
- Reading/card/collection: `cards_catalog`, `reading_sessions`, `user_collections`.
- Chat/community: `conversations`, `chat_messages`, `conversation_reviews`, `reports`, `community_posts`, `community_reactions`, `community_comments`, `community_media_assets`.
- Reader/profile: `reader_requests`, `reader_profiles`.
- Notification/auth/log/upload: `notifications`, `refresh_tokens`, `ai_provider_logs`, `upload_sessions`.
- Gamification: `quests`, `quest_progress`, `achievements`, `user_achievements`, `titles`, `user_titles`, `leaderboard_entries`, `leaderboard_snapshots`.

## Redis

Evidence:
- `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
- `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.Bootstrap.cs`
- `Backend/src/TarotNow.Application/Interfaces/ICacheService.cs`
- `Backend/src/TarotNow.Application/Interfaces/IRedisPublisher.cs`

Vai trò: cache, pub/sub realtime, coordination/rate-limit support. Production yêu cầu Redis theo cấu hình DI; thiếu Redis ở production là lỗi fail-fast.

## Rủi ro

- P0: finance mutation ngoài PostgreSQL transaction; double spend; thiếu idempotency; direct Mongo write cho state cần ACID.
- P1: cross-store consistency không có saga/outbox/reconcile path rõ.
- P2: docs/schema không đồng bộ với `ApplicationDbContext` hoặc `MongoDbContext`.
