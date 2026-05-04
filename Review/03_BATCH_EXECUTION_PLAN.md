# Batch execution plan review kiến trúc

## Mục tiêu

Dùng plan này để review lại bộ `Review/` theo source code, không sinh nội dung bằng script và không dùng template chung chung. Mỗi batch phải đọc source trước, sau đó mới sửa markdown tương ứng.

## Batch 0 — Evidence baseline

- Đếm backend feature folders dưới `Backend/src/TarotNow.Application/Features`: kết quả mục tiêu 23.
- Đếm frontend feature folders dưới `Frontend/src/features`: kết quả mục tiêu 16.
- Đếm markdown docs dưới `Review`: kết quả mục tiêu 52.
- Đọc guard backend/frontend trước khi kết luận boundary.

Output: baseline count và danh sách source-of-truth đã đọc.

## Batch 1 — Cross-cutting architecture

Files:

- `Review/cross-cutting/ARCH_CleanArchitecture_Boundaries.md`
- `Review/cross-cutting/ARCH_EventDriven_Outbox_RealtimeBridge.md`
- `Review/cross-cutting/ARCH_DataStores_PostgreSQL_MongoDB_Redis.md`
- `Review/cross-cutting/ARCH_Auth_Security_Mfa.md`
- `Review/cross-cutting/ARCH_Observability_TestGates_CI.md`
- `Review/cross-cutting/ARCH_Frontend_Boundary_Prefetch_i18n.md`

Evidence bắt buộc: architecture tests, `.csproj`, dispatcher/outbox/realtime paths, datastore contexts/schemas, frontend guard scripts, deploy/workflow files.

## Batch 2 — Backend feature review

Groups:

- Identity/Admin/Legal: `BE_Admin.md`, `BE_Auth.md`, `BE_Mfa.md`, `BE_UserContext.md`, `BE_Legal.md`.
- Reading/Reader: `BE_Reader.md`, `BE_Reading.md`, `BE_Home.md`, `BE_History.md`.
- Chat/Realtime: `BE_Chat.md`, `BE_Escrow.md`, `BE_Presence.md`, `BE_Notification.md`.
- Finance: `BE_Deposit.md`, `BE_Withdrawal.md`, `BE_Wallet.md`, `BE_Promotions.md`.
- Engagement/Social/Profile: `BE_CheckIn.md`, `BE_Community.md`, `BE_Gacha.md`, `BE_Gamification.md`, `BE_Inventory.md`, `BE_Profile.md`.

Evidence bắt buộc: Application feature folder, API controller, related Infrastructure persistence/realtime/outbox path, unit/integration/architecture tests nếu có.

## Batch 3 — Frontend feature review

Groups:

- Shell/Auth/Admin/Legal/Profile: `FE_Auth.md`, `FE_Admin.md`, `FE_Profile.md`, `FE_Legal.md`.
- Reading/Reader/Home/Collection: `FE_Reading.md`, `FE_Reader.md`, `FE_Home.md`, `FE_Collection.md`.
- Realtime/Social: `FE_Chat.md`, `FE_Community.md`, `FE_Notifications.md`.
- Engagement/Finance: `FE_CheckIn.md`, `FE_Gacha.md`, `FE_Gamification.md`, `FE_Inventory.md`, `FE_Wallet.md`.

Evidence bắt buộc: app route, feature public export, prefetch runner, app API proxy nếu có, i18n namespace.

## Batch 4 — Root docs and consistency pass

Files:

- `00_MUC_LUC_REVIEW_KIEN_TRUC.md`
- `01_SO_DO_PHAN_RA_HE_THONG.md`
- `02_BAN_DO_DEPENDENCY_TOAN_HE_THONG.md`
- `03_BATCH_EXECUTION_PLAN.md`
- `04_TEMPLATE_REVIEW_TINH_NANG_CHUAN.md`
- `05_QUY_TAC_DANH_GIA_VA_DIEM_RUI_RO.md`
- `06_CHECKLIST_VERIFY_VA_DAU_RA.md`

Output: root docs match feature docs, no obsolete generic phrasing, no invalid combined database-and-deploy path, and Mongo collection name `conversation_reviews` is not mislabeled as generic `reviews`.

## Verification after each batch

- Count files in the batch.
- Grep for generic/template phrases.
- Inspect `git diff -- Review/...`.
- Record gaps as source gaps, not assumptions.
