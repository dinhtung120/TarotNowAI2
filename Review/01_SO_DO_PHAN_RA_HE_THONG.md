# Sơ đồ phân rã hệ thống

## Capability map source-backed

| Capability | Backend evidence | Frontend evidence | Data/Ops evidence |
|---|---|---|---|
| Identity & Access | `Backend/src/TarotNow.Application/Features/Auth`, `Mfa`, `UserContext`, `Admin`, `Legal`; controllers `AuthRegistrationController.cs`, `AuthSessionController.cs`, `AuthPasswordController.cs`, `MfaController.cs`, `MeController.cs`, `UserContextController.cs`, `LegalController.cs`, `Admin*Controller.cs` | `Frontend/src/features/auth`, `admin`, `legal`, `profile`; routes under `Frontend/src/app/[locale]/(auth)`, `admin`, `(site)/legal`, `(user)/profile` | PostgreSQL `users`, `auth_sessions`, `email_otps`, `password_reset_tokens`, `user_consents`, `data_rights_requests`; Mongo `refresh_tokens` |
| Reading, AI & Reader | `Features/Reading`, `Reader`, `Home`, `History`; controllers `TarotController.cs`, `AiController.cs`, `ReaderController*.cs`, `HomeController.cs`, `HistoryController.cs` | `features/reading`, `reader`, `home`, `collection`; routes `reading/**`, `readers/**`, home site route, collection route | PostgreSQL `ai_requests`, `reading_rng_audits`, `reading_reveal_saga_states`; Mongo `reading_sessions`, `cards_catalog`, `reader_profiles`, `reader_requests`, `user_collections` |
| Conversation & Realtime | `Features/Chat`, `Escrow`, `Presence`, `Notification`; `ConversationController*.cs`, `PresenceHub.cs`, `NotificationController*.cs` | `features/chat`, `notifications`, `community`; routes `chat`, `chat/[id]`, `notifications`, `community` | Mongo `conversations`, `chat_messages`, `notifications`, `conversation_reviews`; Redis Pub/Sub via `IRedisPublisher`; SignalR bridge paths |
| Finance | `Features/Deposit`, `Withdrawal`, `Wallet`, `Promotions`, finance paths in `Escrow`; controllers `DepositController*.cs`, `WithdrawalController.cs`, `WalletController.cs`, `AdminWithdrawalsController.cs`, `PromotionsController.cs` | `features/wallet`, finance-facing chat/reading/gacha/inventory flows; routes `wallet`, `deposit`, `withdraw` | PostgreSQL `wallet_transactions`, `deposit_orders`, `withdrawal_requests`, `chat_finance_sessions`, `chat_question_items`, deposit promotions tables |
| Engagement & Social | `Features/CheckIn`, `Community`, `Gacha`, `Gamification`, `Inventory`, `Profile`; controllers `CheckInController.cs`, `CommunityController*.cs`, `GachaController.cs`, `GamificationController.cs`, `InventoryController.cs`, `ProfileController.cs` | `features/checkin`, `community`, `gacha`, `gamification`, `inventory`, `profile`; routes `community`, `gacha`, `gamification`, `leaderboard`, `inventory`, `profile` | PostgreSQL gacha/inventory/check-in/profile transactional tables; Mongo community/gamification/upload documents |
| Ops & Delivery | API/Infrastructure composition, architecture tests | Next.js App Router, frontend guard scripts | `docker-compose.yml`, `docker-compose.prod.yml`, `deploy/scripts/*`, `.github/workflows/cd-*.yml` |

## Boundary trọng yếu

- Backend write command flow chuẩn: `Controller -> MediatR Command -> thin CommandHandler -> IInlineDomainEventDispatcher -> *RequestedDomainEventHandler -> Application interfaces -> Infrastructure implementations -> DomainEvent/Outbox/Redis bridge`.
- `Backend/tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs` enforce command handlers không inject repository/provider/wallet/realtime/notification side-effect services trực tiếp.
- `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs` enforce compile-time boundary Domain/Application/Infrastructure/API.
- Frontend route flow chuẩn: `Frontend/src/app/[locale] -> @/features/*/public -> feature presentation/application -> shared infrastructure`, được kiểm tra bởi `Frontend/scripts/check-clean-architecture.mjs`.
- Finance/AI/reward modules như Reading, Escrow, Deposit, Withdrawal, Gacha, Inventory, Wallet phải luôn review idempotency, transaction, settlement/refund và query invalidation cùng nhau.
