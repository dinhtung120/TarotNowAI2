# Mục lục review kiến trúc TarotNowAI2

## Trạng thái tài liệu

Bộ `Review/` này là review kiến trúc source-backed cho repo hiện tại, gồm 52 file markdown: 7 file tổng quan, 6 file cross-cutting, 23 backend feature docs và 16 frontend feature docs. Mỗi file feature/cross-cutting đã được viết lại theo evidence source cụ thể; chỗ không thấy evidence trực tiếp được ghi rõ thay vì suy đoán.

## Nguồn sự thật dùng để review

- Backend features: `Backend/src/TarotNow.Application/Features` — 23 feature folders.
- Backend API: `Backend/src/TarotNow.Api/Controllers`, SignalR hubs và API composition.
- Backend architecture guards: `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs`, `EventDrivenArchitectureRulesTests.cs`, `ApiAndConfigurationStandardsTests.cs`, `CodeQualityRulesTests.cs`.
- Frontend features: `Frontend/src/features` — 16 feature folders.
- Frontend routes/API/prefetch/i18n: `Frontend/src/app/[locale]`, `Frontend/src/app/api`, `Frontend/src/shared/server/prefetch`, `Frontend/messages/{vi,en,zh}`.
- Frontend guards: `Frontend/scripts/check-clean-architecture.mjs`, `check-auth-fail-closed.mjs`, `check-component-size.mjs`, `check-hook-action-size.mjs`, `check-risk-coverage.mjs`, `check-next-image-policy.mjs`.
- Data/Ops: `database/postgresql/schema.sql`, `database/mongodb/schema.md`, `docker-compose.yml`, `docker-compose.prod.yml`, `deploy/scripts/*`, `.github/workflows/*`.

## File tổng quan

- [Sơ đồ phân rã hệ thống](01_SO_DO_PHAN_RA_HE_THONG.md)
- [Bản đồ dependency toàn hệ thống](02_BAN_DO_DEPENDENCY_TOAN_HE_THONG.md)
- [Batch execution plan](03_BATCH_EXECUTION_PLAN.md)
- [Format review tính năng chuẩn](04_TEMPLATE_REVIEW_TINH_NANG_CHUAN.md)
- [Quy tắc đánh giá và điểm rủi ro](05_QUY_TAC_DANH_GIA_VA_DIEM_RUI_RO.md)
- [Checklist verify và đầu ra](06_CHECKLIST_VERIFY_VA_DAU_RA.md)

## Cross-cutting

- [Clean Architecture Boundaries](cross-cutting/ARCH_CleanArchitecture_Boundaries.md)
- [Event-driven, Outbox, Realtime Bridge](cross-cutting/ARCH_EventDriven_Outbox_RealtimeBridge.md)
- [Data Stores PostgreSQL, MongoDB, Redis](cross-cutting/ARCH_DataStores_PostgreSQL_MongoDB_Redis.md)
- [Auth, Security, MFA](cross-cutting/ARCH_Auth_Security_Mfa.md)
- [Observability, Test Gates, CI](cross-cutting/ARCH_Observability_TestGates_CI.md)
- [Frontend Boundary, Prefetch, i18n](cross-cutting/ARCH_Frontend_Boundary_Prefetch_i18n.md)

## Backend features

- [BE Admin](backend-features/BE_Admin.md) — admin RBAC, finance/admin mutations, outbox/reconciliation surfaces.
- [BE Auth](backend-features/BE_Auth.md) — registration, login, refresh rotation, password reset, session cookies.
- [BE Mfa](backend-features/BE_Mfa.md) — setup, verify, challenge, recovery-code security.
- [BE UserContext](backend-features/BE_UserContext.md) — `/user-context/metadata`, shell snapshot, wallet/chat/notification/check-in aggregate.
- [BE Legal](backend-features/BE_Legal.md) — public runtime policies, user consent and compliance records.
- [BE Reader](backend-features/BE_Reader.md) — directory, application flow, reader profile, presence overlay.
- [BE Reading](backend-features/BE_Reading.md) — tarot session, AI stream, billing settlement/refund.
- [BE Home](backend-features/BE_Home.md) — public home snapshot.
- [BE History](backend-features/BE_History.md) — reading history list/detail ownership/admin access.
- [BE Chat](backend-features/BE_Chat.md) — conversations, realtime, media and message boundaries.
- [BE Escrow](backend-features/BE_Escrow.md) — chat finance escrow lifecycle.
- [BE Presence](backend-features/BE_Presence.md) — SignalR presence tracking and observer subscriptions.
- [BE Notification](backend-features/BE_Notification.md) — notification list/read state and Mongo storage.
- [BE Deposit](backend-features/BE_Deposit.md) — deposit orders, webhook, promotions and reconciliation.
- [BE Withdrawal](backend-features/BE_Withdrawal.md) — withdrawal request/admin processing and payout risk.
- [BE Wallet](backend-features/BE_Wallet.md) — balance/ledger read model and canonical money-event constraint.
- [BE Promotions](backend-features/BE_Promotions.md) — admin deposit promotions.
- [BE CheckIn](backend-features/BE_CheckIn.md) — daily streak and freeze/reward state.
- [BE Community](backend-features/BE_Community.md) — posts, comments, reactions, reports and media upload.
- [BE Gacha](backend-features/BE_Gacha.md) — pools, pull idempotency, pity/reward state.
- [BE Gamification](backend-features/BE_Gamification.md) — quests, achievements, titles, leaderboard.
- [BE Inventory](backend-features/BE_Inventory.md) — item definitions, user items, use-operation idempotency.
- [BE Profile](backend-features/BE_Profile.md) — profile, avatar upload, reader/payout fields.

## Frontend features

- [FE Admin](frontend-features/FE_Admin.md) — admin route group, SSR prefetch and cross-domain admin pages.
- [FE Auth](frontend-features/FE_Auth.md) — auth pages, redirect guard and auth API proxies.
- [FE Legal](frontend-features/FE_Legal.md) — static legal pages, metadata and localized legal content.
- [FE Profile](frontend-features/FE_Profile.md) — profile, MFA and reader-settings surfaces.
- [FE Reading](frontend-features/FE_Reading.md) — setup/history/session routes, reading setup prefetch and localized SSE stream routes.
- [FE Reader](frontend-features/FE_Reader.md) — directory/detail/apply/settings routes and role-gated reader settings.
- [FE Home](frontend-features/FE_Home.md) — public site home route.
- [FE Collection](frontend-features/FE_Collection.md) — protected collection route and card image proxy.
- [FE Chat](frontend-features/FE_Chat.md) — inbox/room routes, shell prefetch and unread API proxies.
- [FE Community](frontend-features/FE_Community.md) — protected feed route and public-feed SSR prefetch.
- [FE Notifications](frontend-features/FE_Notifications.md) — protected list route, notification API proxy and query hydration.
- [FE CheckIn](frontend-features/FE_CheckIn.md) — user shell streak hydration, no dedicated route/public export evidence.
- [FE Gacha](frontend-features/FE_Gacha.md) — gacha page/history, API proxies and idempotent pull.
- [FE Gamification](frontend-features/FE_Gamification.md) — user hub, leaderboard, admin gamification and runtime-policy prefetch.
- [FE Inventory](frontend-features/FE_Inventory.md) — inventory route, prefetch and idempotent use-item proxy.
- [FE Wallet](frontend-features/FE_Wallet.md) — wallet/deposit/history/withdraw routes, finance prefetch and role gate.
