# Mục lục review kiến trúc TarotNowAI2

## Trạng thái tài liệu

Bộ `Review/` này là review kiến trúc source-backed cho repo hiện tại. Mỗi file phải dẫn evidence bằng đường dẫn cụ thể; khi chưa thấy evidence trực tiếp, file phải nói rõ thay vì suy đoán.

## Nguồn sự thật dùng để review

- Backend features: `Backend/src/TarotNow.Application/Features` — 23 thư mục feature.
- Frontend features: `Frontend/src/features` — 16 thư mục feature, đa số có `public.ts`.
- Backend architecture guards: `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs`, `EventDrivenArchitectureRulesTests.cs`, `ApiAndConfigurationStandardsTests.cs`, `CodeQualityRulesTests.cs`.
- Frontend guards: `Frontend/scripts/check-clean-architecture.mjs`, `check-component-size.mjs`, `check-hook-action-size.mjs`, `check-auth-fail-closed.mjs`, `check-next-image-policy.mjs`, `check-risk-coverage.mjs`.
- Data/Ops: `database/*`, `deploy/*`, `.github/workflows/*`, `docker-compose.yml`, `docker-compose.prod.yml`. Lưu ý: `database/*` và `deploy/*` là hai nhánh riêng biệt, không phải một thư mục lồng nhau.

## File tổng quan

- [Sơ đồ phân rã hệ thống](01_SO_DO_PHAN_RA_HE_THONG.md)
- [Bản đồ dependency toàn hệ thống](02_BAN_DO_DEPENDENCY_TOAN_HE_THONG.md)
- [Batch execution plan](03_BATCH_EXECUTION_PLAN.md)
- [Template review tính năng chuẩn](04_TEMPLATE_REVIEW_TINH_NANG_CHUAN.md)
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

- [BE Admin](backend-features/BE_Admin.md) — `Identity/Admin`; evidence chính: `Backend/src/TarotNow.Application/Features/Admin`.
- [BE Auth](backend-features/BE_Auth.md) — `Identity/Auth`; evidence chính: `Backend/src/TarotNow.Application/Features/Auth`.
- [BE Mfa](backend-features/BE_Mfa.md) — `Identity/MFA`; evidence chính: `Backend/src/TarotNow.Application/Features/Mfa`.
- [BE UserContext](backend-features/BE_UserContext.md) — `Identity/UserContext`; evidence chính: `Backend/src/TarotNow.Application/Features/UserContext`.
- [BE Legal](backend-features/BE_Legal.md) — `Compliance/Legal`; evidence chính: `Backend/src/TarotNow.Application/Features/Legal`.
- [BE Reader](backend-features/BE_Reader.md) — `Reader domain`; evidence chính: `Backend/src/TarotNow.Application/Features/Reader`.
- [BE Reading](backend-features/BE_Reading.md) — `Reading/AI`; evidence chính: `Backend/src/TarotNow.Application/Features/Reading`.
- [BE Home](backend-features/BE_Home.md) — `Home queries`; evidence chính: `Backend/src/TarotNow.Application/Features/Home`.
- [BE History](backend-features/BE_History.md) — `Reading history`; evidence chính: `Backend/src/TarotNow.Application/Features/History`.
- [BE Chat](backend-features/BE_Chat.md) — `Chat/realtime`; evidence chính: `Backend/src/TarotNow.Application/Features/Chat`.
- [BE Escrow](backend-features/BE_Escrow.md) — `Chat finance escrow`; evidence chính: `Backend/src/TarotNow.Application/Features/Escrow`.
- [BE Presence](backend-features/BE_Presence.md) — `Realtime presence`; evidence chính: `Backend/src/TarotNow.Application/Features/Presence`.
- [BE Notification](backend-features/BE_Notification.md) — `Notifications`; evidence chính: `Backend/src/TarotNow.Application/Features/Notification`.
- [BE Deposit](backend-features/BE_Deposit.md) — `Finance deposit`; evidence chính: `Backend/src/TarotNow.Application/Features/Deposit`.
- [BE Withdrawal](backend-features/BE_Withdrawal.md) — `Finance withdrawal`; evidence chính: `Backend/src/TarotNow.Application/Features/Withdrawal`.
- [BE Wallet](backend-features/BE_Wallet.md) — `Wallet ledger`; evidence chính: `Backend/src/TarotNow.Application/Features/Wallet`.
- [BE Promotions](backend-features/BE_Promotions.md) — `Finance/growth promotions`; evidence chính: `Backend/src/TarotNow.Application/Features/Promotions`.
- [BE CheckIn](backend-features/BE_CheckIn.md) — `Engagement check-in`; evidence chính: `Backend/src/TarotNow.Application/Features/CheckIn`.
- [BE Community](backend-features/BE_Community.md) — `Community/social`; evidence chính: `Backend/src/TarotNow.Application/Features/Community`.
- [BE Gacha](backend-features/BE_Gacha.md) — `Gacha/reward`; evidence chính: `Backend/src/TarotNow.Application/Features/Gacha`.
- [BE Gamification](backend-features/BE_Gamification.md) — `Quests/leaderboard`; evidence chính: `Backend/src/TarotNow.Application/Features/Gamification`.
- [BE Inventory](backend-features/BE_Inventory.md) — `Inventory/items`; evidence chính: `Backend/src/TarotNow.Application/Features/Inventory`.
- [BE Profile](backend-features/BE_Profile.md) — `User profile`; evidence chính: `Backend/src/TarotNow.Application/Features/Profile`.

## Frontend features

- [FE Admin](frontend-features/FE_Admin.md) — evidence chính: `Frontend/src/features/admin` và `Frontend/src/app/[locale]`.
- [FE Auth](frontend-features/FE_Auth.md) — evidence chính: `Frontend/src/features/auth` và `Frontend/src/app/[locale]`.
- [FE Legal](frontend-features/FE_Legal.md) — evidence chính: `Frontend/src/features/legal` và `Frontend/src/app/[locale]`.
- [FE Profile](frontend-features/FE_Profile.md) — evidence chính: `Frontend/src/features/profile` và `Frontend/src/app/[locale]`.
- [FE Reading](frontend-features/FE_Reading.md) — evidence chính: `Frontend/src/features/reading` và `Frontend/src/app/[locale]`.
- [FE Reader](frontend-features/FE_Reader.md) — evidence chính: `Frontend/src/features/reader` và `Frontend/src/app/[locale]`.
- [FE Home](frontend-features/FE_Home.md) — evidence chính: `Frontend/src/features/home` và `Frontend/src/app/[locale]`.
- [FE Collection](frontend-features/FE_Collection.md) — evidence chính: `Frontend/src/features/collection` và `Frontend/src/app/[locale]`.
- [FE Chat](frontend-features/FE_Chat.md) — evidence chính: `Frontend/src/features/chat` và `Frontend/src/app/[locale]`.
- [FE Community](frontend-features/FE_Community.md) — evidence chính: `Frontend/src/features/community` và `Frontend/src/app/[locale]`.
- [FE Notifications](frontend-features/FE_Notifications.md) — evidence chính: `Frontend/src/features/notifications` và `Frontend/src/app/[locale]`.
- [FE CheckIn](frontend-features/FE_CheckIn.md) — evidence chính: `Frontend/src/features/checkin` và `Frontend/src/app/[locale]`.
- [FE Gacha](frontend-features/FE_Gacha.md) — evidence chính: `Frontend/src/features/gacha` và `Frontend/src/app/[locale]`.
- [FE Gamification](frontend-features/FE_Gamification.md) — evidence chính: `Frontend/src/features/gamification` và `Frontend/src/app/[locale]`.
- [FE Inventory](frontend-features/FE_Inventory.md) — evidence chính: `Frontend/src/features/inventory` và `Frontend/src/app/[locale]`.
- [FE Wallet](frontend-features/FE_Wallet.md) — evidence chính: `Frontend/src/features/wallet` và `Frontend/src/app/[locale]`.
