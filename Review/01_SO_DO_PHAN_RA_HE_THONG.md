# Sơ đồ phân rã hệ thống

## Capability map source-backed

| Capability | Backend evidence | Frontend evidence | Data/Ops evidence |
|---|---|---|---|
| Identity & Access | `Features/Auth`, `Features/Mfa`, `Features/UserContext`, `Features/Admin`, `Features/Legal` | `features/auth`, `features/admin`, `features/legal`, `features/profile` | `users`, `auth_sessions`, `email_otps`, `password_reset_tokens`, `user_consents`, `data_rights_requests` trong `database/postgresql/schema.sql`; `refresh_tokens` trong `MongoDbContext.cs` |
| Reading/AI/Reader | `Features/Reading`, `Features/Reader`, `Features/Home`, `Features/History` | `features/reading`, `features/reader`, `features/home`, `features/collection` | `ai_requests`, `reading_rng_audits`, `reading_reveal_saga_states` trong PostgreSQL; `reading_sessions`, `cards_catalog`, `reader_profiles`, `reader_requests`, `user_collections` trong MongoDB |
| Conversation/Realtime | `Features/Chat`, `Features/Escrow`, `Features/Presence`, `Features/Notification` | `features/chat`, `features/notifications`, `features/community` | MongoDB `conversations`, `chat_messages`, `notifications`; Redis Pub/Sub qua `IRedisPublisher`; SignalR bridge trong API/Infrastructure realtime paths |
| Finance | `Features/Deposit`, `Features/Withdrawal`, `Features/Wallet`, `Features/Promotions`, finance phần `Escrow` | `features/wallet`, finance-facing flows trong `features/chat` | PostgreSQL `wallet_transactions`, `deposit_orders`, `chat_finance_sessions`, `chat_question_items`, `withdrawal_requests`, promotions tables |
| Engagement/Social | `Features/CheckIn`, `Features/Gacha`, `Features/Gamification`, `Features/Inventory`, `Features/Community`, `Features/Profile` | `features/checkin`, `features/gacha`, `features/gamification`, `features/inventory`, `features/community`, `features/profile` | PostgreSQL gacha/inventory item tables; MongoDB quests, achievements, leaderboard, community documents |
| Ops/Delivery | Infrastructure + API composition | Next.js app router + scripts guards | `docker-compose*.yml`, `deploy/scripts/*`, `.github/workflows/cd-*.yml` |

## Review boundary quan trọng

- Backend write command entry handlers phải mỏng và dispatch requested domain event qua `IInlineDomainEventDispatcher`; rule được enforce bởi `EventDrivenArchitectureRulesTests.cs`.
- Backend layer dependency được review qua `ArchitectureBoundariesTests.cs`: Domain không phụ thuộc outer layer; Application không dùng Infrastructure concrete/framework; API không bypass Application cho repository/db context ngoài allowlist.
- Frontend `page.tsx`/`layout.tsx` phải là composition wrapper; import từ app sang feature nên qua `features/*/public.ts`, kiểm tra bằng `check-clean-architecture.mjs`.
- Tài chính/AI/realtime là vùng rủi ro cao: phải review transaction, idempotency, event/outbox và không side-effect trực tiếp từ controller/handler mỏng.
