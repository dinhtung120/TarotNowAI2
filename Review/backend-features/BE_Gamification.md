# BE Gamification

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Gamification`
- Controllers: `Backend/src/TarotNow.Api/Controllers/GamificationController.cs`, `AdminGamificationController.cs`
- Tests: không thấy file test có `Gamification` trong tên theo search hiện tại; có coverage side effects liên quan trong `InventoryRewardEventHandlersTests.cs` và các inventory/gacha domain event tests
- Datastore: `MongoDbContext.cs` collections `quests`, `quest_progress`, `achievements`, `user_achievements`, `titles`, `user_titles`, `leaderboard_entries`, `leaderboard_snapshots`
- Runtime config: `ISystemConfigSettings.GamificationDefaultQuestType`, `GamificationDefaultLeaderboardTrack`

## Entry points & luồng chính

`GamificationController.cs` là authenticated API với `[Authorize]` và rate limit `auth-session`.

Endpoints chính:

- `GET quests`: `GetActiveQuestsQuery(userId, type)`; default type từ system config.
- `POST quests/{questCode}/claim`: `ClaimQuestRewardCommand`.
- `GET achievements`: `GetUserAchievementsQuery`.
- `GET titles`: `GetUserTitlesQuery`.
- `POST titles/active`: `SetActiveTitleCommand`.
- `GET leaderboard`: `GetLeaderboardQuery`; default track từ system config.

Controller lấy user id từ `ClaimTypes.NameIdentifier` và throw unauthorized nếu claim invalid.

## Dependency và dữ liệu

Gamification state chính ở MongoDB:

- Quest/progress: `quests`, `quest_progress`.
- Achievements: `achievements`, `user_achievements`.
- Titles: `titles`, `user_titles`.
- Leaderboard: `leaderboard_entries`, `leaderboard_snapshots`.

Feature có `Commands`, `Queries`, `EventHandlers`, `Validators`; event handlers có thể nhận events từ reading/check-in/gacha/inventory/chat để update progress/rewards.

## Boundary / guard

- Claim quest reward phải idempotent/anti-double-claim theo `questCode` + `periodKey` + user.
- Set active title phải verify ownership; controller trả `TITLE_NOT_OWNED` khi command false.
- Leaderboard/quest defaults đến từ system config, không hardcode ở controller.
- Side effects như reward grant/realtime phải qua event handlers/outbox, không controller.

## Test coverage hiện có

Không thấy test file tên `Gamification` trong evidence search hiện tại. Coverage gián tiếp có thể nằm ở Inventory/Gacha event handler tests, nhưng chưa đủ để chứng minh quest/achievement/title/leaderboard API.

Nếu audit sâu không tìm thêm, đây là gap P1 cho claim reward anti-duplication, title ownership và leaderboard query contract.

## Rủi ro

- P0: quest reward claim double grant; active title set khi chưa sở hữu; event handler reward chạy nhiều lần khi retry outbox.
- P1: leaderboard/quest defaults thay đổi nhưng FE/API tests không bắt; thiếu direct test cho GamificationController/handlers.
- P2: docs nói gamification state ở PostgreSQL trong khi evidence runtime collection nằm ở MongoDB.

## Kết luận

Gamification là MongoDB engagement module nhận nhiều domain events từ hệ thống. Review đúng phải đọc event handlers và reward idempotency, không chỉ user-facing controller.
