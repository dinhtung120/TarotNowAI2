# Mục lục review kiến trúc TarotNowAI2

## Mục tiêu

Bộ tài liệu này dùng để điều phối review kiến trúc toàn hệ thống TarotNowAI2 theo từng module, từng dependency và từng batch thực thi. Tài liệu là khung review có cấu trúc chuẩn, không thay thế việc đọc code chi tiết khi audit từng issue cụ thể.

## File tổng quan

- [Sơ đồ phân rã hệ thống](01_SO_DO_PHAN_RA_HE_THONG.md)
- [Bản đồ dependency toàn hệ thống](02_BAN_DO_DEPENDENCY_TOAN_HE_THONG.md)
- [Batch execution plan](03_BATCH_EXECUTION_PLAN.md)
- [Template review tính năng chuẩn](04_TEMPLATE_REVIEW_TINH_NANG_CHUAN.md)
- [Quy tắc đánh giá và điểm rủi ro](05_QUY_TAC_DANH_GIA_VA_DIEM_RUI_RO.md)
- [Checklist verify và đầu ra](06_CHECKLIST_VERIFY_VA_DAU_RA.md)

## Cross-cutting architecture

- [Clean Architecture Boundaries](cross-cutting/ARCH_CleanArchitecture_Boundaries.md)
- [Event Driven, Outbox, Realtime Bridge](cross-cutting/ARCH_EventDriven_Outbox_RealtimeBridge.md)
- [Data Stores PostgreSQL, MongoDB, Redis](cross-cutting/ARCH_DataStores_PostgreSQL_MongoDB_Redis.md)
- [Auth, Security, MFA](cross-cutting/ARCH_Auth_Security_Mfa.md)
- [Observability, Test Gates, CI](cross-cutting/ARCH_Observability_TestGates_CI.md)
- [Frontend Boundary, Prefetch, i18n](cross-cutting/ARCH_Frontend_Boundary_Prefetch_i18n.md)

## Review theo tính năng

- [BE Admin](backend-features/BE_Admin.md)
- [BE Auth](backend-features/BE_Auth.md)
- [BE Mfa](backend-features/BE_Mfa.md)
- [BE UserContext](backend-features/BE_UserContext.md)
- [BE Reader](backend-features/BE_Reader.md)
- [BE Reading](backend-features/BE_Reading.md)
- [BE Home](backend-features/BE_Home.md)
- [BE History](backend-features/BE_History.md)
- [BE Chat](backend-features/BE_Chat.md)
- [BE Escrow](backend-features/BE_Escrow.md)
- [BE Presence](backend-features/BE_Presence.md)
- [BE Deposit](backend-features/BE_Deposit.md)
- [BE Withdrawal](backend-features/BE_Withdrawal.md)
- [BE Wallet](backend-features/BE_Wallet.md)
- [BE Community](backend-features/BE_Community.md)
- [BE Notification](backend-features/BE_Notification.md)
- [BE Profile](backend-features/BE_Profile.md)
- [BE Gamification](backend-features/BE_Gamification.md)
- [BE Gacha](backend-features/BE_Gacha.md)
- [BE Inventory](backend-features/BE_Inventory.md)
- [BE CheckIn](backend-features/BE_CheckIn.md)
- [BE Promotions](backend-features/BE_Promotions.md)
- [BE Legal](backend-features/BE_Legal.md)
- [FE Admin](frontend-features/FE_Admin.md)
- [FE Auth](frontend-features/FE_Auth.md)
- [FE Chat](frontend-features/FE_Chat.md)
- [FE CheckIn](frontend-features/FE_CheckIn.md)
- [FE Collection](frontend-features/FE_Collection.md)
- [FE Community](frontend-features/FE_Community.md)
- [FE Gacha](frontend-features/FE_Gacha.md)
- [FE Gamification](frontend-features/FE_Gamification.md)
- [FE Home](frontend-features/FE_Home.md)
- [FE Inventory](frontend-features/FE_Inventory.md)
- [FE Legal](frontend-features/FE_Legal.md)
- [FE Notifications](frontend-features/FE_Notifications.md)
- [FE Profile](frontend-features/FE_Profile.md)
- [FE Reader](frontend-features/FE_Reader.md)
- [FE Reading](frontend-features/FE_Reading.md)
- [FE Wallet](frontend-features/FE_Wallet.md)
