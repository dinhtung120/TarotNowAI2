# Sơ đồ phân rã hệ thống

## Nhóm capability chính

| Capability | Backend module | Frontend module | Data/ops liên quan |
|---|---|---|---|
| Identity & Access | Auth, Mfa, UserContext, Admin | Auth, Admin | JWT, refresh token, rate limit, audit |
| Reader & Reading | Reader, Reading, Home, History | Reader, Reading, Home | PostgreSQL transactional state, MongoDB reading documents, AI provider |
| Conversation & Realtime | Chat, Presence, Notification | Chat, Notifications | Redis Pub/Sub, SignalR bridge, MongoDB messages |
| Finance & Escrow | Wallet, Deposit, Withdrawal, Escrow | Wallet, Chat | PostgreSQL ACID, idempotency, MoneyChangedDomainEvent |
| Engagement | Gamification, Gacha, Inventory, CheckIn, Promotions | Gamification, Gacha, Inventory, CheckIn | reward state, anti-duplication, event fan-out |
| Community & Profile | Community, Profile, Legal | Community, Profile, Legal | consent, moderation, user documents |
| Data & Ops | Infrastructure, database, deploy | Frontend build/deploy | Docker, workflows, smoke, rollback, backup |

## Nguyên tắc review

- Review theo capability trước, sau đó đi sâu từng feature file.
- Mọi flow ghi backend phải đối chiếu với command/event-driven architecture.
- Mọi UI route phải đối chiếu với thin app route, public feature export, i18n và guard scripts.
- Các flow finance, quota, AI, realtime phải được đánh dấu rủi ro cao hơn do yêu cầu transaction, idempotency và outbox.
