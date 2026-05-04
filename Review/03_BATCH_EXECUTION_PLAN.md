# Batch execution plan review kiến trúc

## Batch 0: Evidence baseline

- Đếm feature backend bằng `find Backend/src/TarotNow.Application/Features -maxdepth 1 -mindepth 1 -type d`.
- Đếm feature frontend bằng `find Frontend/src/features -maxdepth 1 -mindepth 1 -type d`.
- Đọc guard backend/frontend trước khi review module.
- Output: xác nhận 23 BE feature, 16 FE feature, 52 file Review.

## Batch 1: Cross-cutting architecture

- Review `ARCH_CleanArchitecture_Boundaries.md` và `ARCH_EventDriven_Outbox_RealtimeBridge.md` trước.
- Evidence bắt buộc: architecture tests, `.csproj`, dispatcher/outbox/realtime paths.
- Output: rủi ro P0/P1/P2 cho boundary, side effect, idempotency, realtime.

## Batch 2: Data/Ops/Security/Frontend boundary

- Review datastore mapping từ `ApplicationDbContext.cs`, `MongoDbContext.cs`, schemas.
- Review deploy/workflow từ `docker-compose*.yml`, `deploy/scripts/*`, `.github/workflows/*`.
- Review frontend guard/prefetch/i18n từ `Frontend/scripts`, `Frontend/src/shared/server/prefetch`, `Frontend/src/i18n`, `Frontend/messages`.

## Batch 3: Backend feature review

- Identity/Admin/Legal: `Admin`, `Auth`, `Mfa`, `UserContext`, `Legal`.
- Reading/Reader: `Reader`, `Reading`, `Home`, `History`.
- Chat/Realtime: `Chat`, `Escrow`, `Presence`, `Notification`.
- Finance: `Deposit`, `Withdrawal`, `Wallet`, `Promotions`.
- Engagement/Social: `CheckIn`, `Community`, `Gacha`, `Gamification`, `Inventory`, `Profile`.

## Batch 4: Frontend feature review

- Shell/Auth/Admin/Legal/Profile.
- Reading/Reader/Home/Collection.
- Chat/Community/Notifications.
- Finance/Engagement: CheckIn/Gacha/Gamification/Inventory/Wallet.

## Output chuẩn mỗi batch

- Files reviewed.
- Evidence paths.
- Dependency map.
- Findings P0/P1/P2.
- Kết luận: `Pass`, `Pass có điều kiện`, hoặc `Cần remediation`.
- Gaps cần follow-up, ghi rõ “không tìm thấy evidence trực tiếp” nếu chưa chứng minh được.
