# BE Hard-coded Tunable Values

Generated on: 2026-04-08
Scope: `Backend/src`

## Format
- `Value`: hard-coded value in code
- `Meaning`: what this value controls
- `Location`: `file:line`

## 1) Economy / Pricing / System Config

| Value | Meaning | Location |
|---|---|---|
| `1000` | VND per 1 diamond conversion | `Backend/src/TarotNow.Application/Common/Constants/EconomyConstants.cs:7` |
| `3` | Daily AI quota (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:10` |
| `3` | In-flight AI cap (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:13` |
| `30` | Reading rate limit seconds (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:16` |
| `50` | Spread 3 cards price in gold (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:22` |
| `5` | Spread 3 cards price in diamond (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:25` |
| `100` | Spread 5 cards price in gold (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:28` |
| `10` | Spread 5 cards price in diamond (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:31` |
| `500` | Spread 10 cards price in gold (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:34` |
| `50` | Spread 10 cards price in diamond (default) | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs:37` |
| `50` | Fallback spread 3 gold price | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:18` |
| `5` | Fallback spread 3 diamond price | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:19` |
| `100` | Fallback spread 5 gold price | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:22` |
| `10` | Fallback spread 5 diamond price | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:23` |
| `500` | Fallback spread 10 gold price | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:26` |
| `50` | Fallback spread 10 diamond price | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:27` |
| `3` | Fallback daily AI quota | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:30` |
| `5` | Daily checkin gold fixed policy | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:33` |
| `24` | Streak freeze window hours fixed policy | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs:34` |

## 2) Reading / Follow-up / Session Progression

| Value | Meaning | Location |
|---|---|---|
| `[1,2,4,8,16]` | Follow-up price tiers by question order | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:9` |
| `5` | Max follow-up questions per session | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:12` |
| `0..77` | Tarot card id valid range | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:20` |
| `<=21` | Major arcana threshold for card level mapping | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:26` |
| `10 + cardId` | Major arcana level mapping formula | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:29` |
| `(cardId % 14) + 1` | Minor arcana level mapping formula | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:33` |
| `>=16 => 3` | Free follow-up count at high card level | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:59` |
| `>=11 => 2` | Free follow-up count at medium card level | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:65` |
| `>=6 => 1` | Free follow-up count at low card level | `Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs:71` |
| `1` | Exp per revealed card | `Backend/src/TarotNow.Application/Features/Reading/Commands/RevealSession/RevealReadingSessionCommandHandler.cs:19` |
| `78` | Tarot deck size for shuffle | `Backend/src/TarotNow.Application/Features/Reading/Commands/RevealSession/RevealReadingSessionCommandHandler.cs:46` |
| `1,3,5,10` | Cards count by spread type | `Backend/src/TarotNow.Application/Features/Reading/Commands/RevealSession/RevealReadingSessionCommandHandler.Helpers.cs:47` |
| `2` | Exp multiplier for non-daily diamond reveal | `Backend/src/TarotNow.Application/Features/Reading/Commands/RevealSession/RevealReadingSessionCommandHandler.Helpers.cs:66` |
| `1` | Default exp multiplier | `Backend/src/TarotNow.Application/Features/Reading/Commands/RevealSession/RevealReadingSessionCommandHandler.Helpers.cs:66` |

## 3) Gacha

| Value | Meaning | Location |
|---|---|---|
| `5` | Gacha spin cost in diamond (entity default) | `Backend/src/TarotNow.Domain/Entities/GachaBanner.cs:31` |
| `90` | Gacha hard pity count (entity default) | `Backend/src/TarotNow.Domain/Entities/GachaBanner.cs:46` |
| `90` | ctor default hard pity count | `Backend/src/TarotNow.Domain/Entities/GachaBanner.cs:73` |
| `10000` | Odds sum basis points validation | `Backend/src/TarotNow.Domain/Entities/GachaBanner.cs:127` |
| `5` | Seeded banner spin cost diamond | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:68` |
| `90` | Seeded banner hard pity | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:73` |
| `50 @ 3000` | Common reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:91` |
| `100 @ 2500` | Common reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:92` |
| `200 @ 1000` | Common reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:93` |
| `500 @ 1500` | Rare reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:96` |
| `10 diamond @ 1000` | Rare reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:97` |
| `50 diamond @ 600` | Epic reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:100` |
| `title @ 200` | Epic reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:101` |
| `500 diamond @ 150` | Legendary reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:104` |
| `title @ 50` | Legendary reward weight | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs:105` |
| `90` | Replay hard pity threshold | `Backend/src/TarotNow.Application/Features/Gacha/Commands/SpinGacha/SpinGachaCommandHandler.Replay.cs:98` |
| `1` | Default spin count | `Backend/src/TarotNow.Application/Features/Gacha/Commands/SpinGacha/SpinGachaCommand.cs:19` |
| `128` | Idempotency key max length | `Backend/src/TarotNow.Application/Features/Gacha/Commands/SpinGacha/SpinGachaCommandValidator.cs:21` |
| `50` | Gacha history default limit | `Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaHistory/GetGachaHistoryQuery.cs:15` |
| `50` | Gacha history ctor default limit | `Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaHistory/GetGachaHistoryQuery.cs:21` |
| `/100.0` | Probability conversion basis | `Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaCatalog/GachaDtos.cs:69` |

## 4) Escrow / Deposit / Withdrawal / Dispute Finance

| Value | Meaning | Location |
|---|---|---|
| `10%` | Escrow platform fee | `Backend/src/TarotNow.Application/Services/EscrowSettlementService.cs:36` |
| `24h` | Dispute window after release | `Backend/src/TarotNow.Application/Services/EscrowSettlementService.State.cs:39` |
| `10%` | Dispute payout fee | `Backend/src/TarotNow.Application/Features/ChatFinance/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Payouts.cs:19` |
| `splitPercent/100` | Reader split gross formula | `Backend/src/TarotNow.Application/Features/ChatFinance/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.cs:50` |
| `10%` | Reader split fee | `Backend/src/TarotNow.Application/Features/ChatFinance/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.cs:53` |
| `Math.Max(0, ...)` | Clamp split net >= 0 | `Backend/src/TarotNow.Application/Features/ChatFinance/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.cs:55` |
| `1..99` | Allowed split percent range | `Backend/src/TarotNow.Application/Features/ChatFinance/Commands/ResolveDispute/ResolveDisputeCommandValidator.cs:36` |
| `1000` | Admin note max length | `Backend/src/TarotNow.Application/Features/ChatFinance/Commands/ResolveDispute/ResolveDisputeCommandValidator.cs:41` |
| `7 days` | Recent dispute count window | `Backend/src/TarotNow.Application/Features/ChatFinance/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.ReaderPolicy.cs:34` |
| `>3` | Freeze reader threshold by disputes | `Backend/src/TarotNow.Application/Features/ChatFinance/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.ReaderPolicy.cs:36` |
| `50` | Minimum withdrawal diamonds | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateWithdrawal/CreateWithdrawalCommandHandler.Validation.cs:63` |
| `10%` | Withdrawal fee in VND | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateWithdrawal/CreateWithdrawalCommandHandler.Workflow.cs:20` |
| `128` | Withdrawal idempotency max length | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateWithdrawal/CreateWithdrawalCommandHandler.Workflow.cs:97` |
| `>0` | Withdrawal amount must be positive | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateWithdrawal/CreateWithdrawalCommandValidator.cs:19` |
| `128` | Withdrawal idempotency key max | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateWithdrawal/CreateWithdrawalCommandValidator.cs:24` |
| `6..64` | MFA code length range | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateWithdrawal/CreateWithdrawalCommandValidator.cs:44` |
| `>0` | Deposit amount must be positive | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateDepositOrder/CreateDepositOrderCommandValidator.cs:22` |
| `1000` | Deposit amount must be multiple of 1000 VND | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateDepositOrder/CreateDepositOrderCommandValidator.cs:23` |
| `128` | Deposit idempotency key max | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateDepositOrder/CreateDepositOrderCommandValidator.cs:28` |
| `amountVnd / VndPerDiamond` | Deposit base diamond conversion formula | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateDepositOrder/CreateDepositOrderCommandHandler.cs:44` |
| `128` | Deposit idempotency max in handler | `Backend/src/TarotNow.Application/Features/Wallet/Commands/CreateDepositOrder/CreateDepositOrderCommandHandler.cs:78` |

## 5) Chat / Conversation SLA / Money Flow Windows

| Value | Meaning | Location |
|---|---|---|
| `12` | Default SLA hours when creating conversation | `Backend/src/TarotNow.Application/Features/Conversations/Commands/CreateConversation/CreateConversationCommand.cs:18` |
| `6/12/24` | Allowed SLA options | `Backend/src/TarotNow.Application/Features/Conversations/Commands/CreateConversation/CreateConversationCommandHandler.Validation.cs:21` |
| `5` | Max active conversations per user/reader gate | `Backend/src/TarotNow.Application/Features/Conversations/Commands/CreateConversation/CreateConversationCommandHandler.Validation.cs:55` |
| `1..168` | SLA hours validation range | `Backend/src/TarotNow.Application/Features/Conversations/Commands/CreateConversation/CreateConversationCommandValidator.cs:29` |
| `6/12/24 else 12` | SLA normalization/fallback | `Backend/src/TarotNow.Application/Features/Conversations/Commands/AcceptConversation/AcceptConversationCommandHandler.Helpers.cs:38` |
| `>0` | Reader question price must be positive | `Backend/src/TarotNow.Application/Features/Conversations/Commands/SendMessage/SendMessageCommandHandler.FirstMessageFreeze.cs:35` |
| `<=6 ? 6 : 12` | Acceptance due window in hours | `Backend/src/TarotNow.Application/Features/Conversations/Commands/SendMessage/SendMessageCommandHandler.FirstMessageFreeze.cs:85` |
| `+12h` | Auto resolve delay (user requested completion) | `Backend/src/TarotNow.Application/Features/Conversations/Commands/RequestConversationComplete/RequestConversationCompleteCommandHandler.Flow.cs:30` |
| `+48h` | Auto resolve delay (reader requested completion) | `Backend/src/TarotNow.Application/Features/Conversations/Commands/RequestConversationComplete/RequestConversationCompleteCommandHandler.Flow.cs:30` |
| `10` | Min dispute reason length | `Backend/src/TarotNow.Application/Features/Conversations/Commands/OpenDispute/OpenDisputeCommand.cs:27` |
| `48h` | Dispute opening window after completion | `Backend/src/TarotNow.Application/Features/Conversations/Commands/OpenDispute/OpenDisputeCommand.cs:30` |
| `>0` | Add-money amount must be positive | `Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandHandler.Validation.cs:13` |
| `100` | Add-money description max length (handler check) | `Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandHandler.Validation.cs:25` |
| `+24h` | Payment offer expiry for add-money | `Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandHandler.Workflow.cs:101` |
| `1000` | Add-money description max length (validator) | `Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandValidator.cs:28` |
| `128` | Add-money idempotency key max | `Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandValidator.cs:34` |
| `128` | Add-question idempotency key max | `Backend/src/TarotNow.Application/Features/Chat/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs:21` |
| `+24h` | Add-question response due | `Backend/src/TarotNow.Application/Features/Chat/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs:125` |
| `+24h` | Add-question auto-refund due | `Backend/src/TarotNow.Application/Features/Chat/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs:126` |
| `+24h` | Dispute window after reject+refund | `Backend/src/TarotNow.Application/Features/Conversations/Commands/RejectConversation/RejectConversationCommandHandler.Refunds.cs:80` |

## 6) Gamification / Streak / EXP / Rewards

| Value | Meaning | Location |
|---|---|---|
| `1 + Exp/100` | User level formula | `Backend/src/TarotNow.Domain/Entities/User.Account.cs:21` |
| `1.0 + CurrentStreak/100.0` | EXP multiplier by streak | `Backend/src/TarotNow.Domain/Entities/User.Streak.cs:49` |
| `today - 1 day` | Restore streak date baseline | `Backend/src/TarotNow.Domain/Entities/User.Streak.cs:69` |
| `ceil(PreBreakStreak/10.0)` | Freeze cost formula in diamonds | `Backend/src/TarotNow.Domain/Entities/User.Streak.cs:87` |
| `Level=1` | User collection default level | `Backend/src/TarotNow.Domain/Entities/UserCollection.cs:47` |
| `Copies=1` | User collection default copies | `Backend/src/TarotNow.Domain/Entities/UserCollection.cs:48` |
| `Atk=10` | User collection default attack | `Backend/src/TarotNow.Domain/Entities/UserCollection.cs:51` |
| `Def=10` | User collection default defense | `Backend/src/TarotNow.Domain/Entities/UserCollection.cs:52` |
| `5 copies` | Level-up threshold by copies | `Backend/src/TarotNow.Domain/Entities/UserCollection.cs:83` |
| `(10, newLevel*10)` | Random bonus range when leveling collection | `Backend/src/TarotNow.Domain/Entities/UserCollection.cs:107` |
| `target 1, reward 50 gold` | Quest seed (daily_1_reading) | `Backend/src/TarotNow.Infrastructure/Persistence/SeedGamificationData.cs:41` |
| `target 1, reward 20 gold` | Quest seed (daily_checkin) | `Backend/src/TarotNow.Infrastructure/Persistence/SeedGamificationData.cs:51` |
| `target 1, reward 100 gold` | Quest seed (daily_reading_1) | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs:53` |
| `target 7, reward 1000 gold + 5 diamond` | Quest seed (weekly_reading_7) | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs:65` |
| `target 1, reward 150 gold` | Quest seed (daily_post_1) | `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs:77` |
| `5/5/5` | Checkin leaderboard points tuple | `Backend/src/TarotNow.Infrastructure/Services/GamificationService.Activity.cs:22` |
| `2/0/0` | Post leaderboard points tuple | `Backend/src/TarotNow.Infrastructure/Services/GamificationService.Activity.cs:46` |
| `10/10/10` | Reading leaderboard points tuple | `Backend/src/TarotNow.Infrastructure/Services/GamificationService.Reading.cs:23` |
| `5 min` | Quest cache TTL | `Backend/src/TarotNow.Infrastructure/Services/GamificationService.Reading.cs:72` |
| `100` | Leaderboard query default limit | `Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQuery.cs:10` |
| `100` | Snapshot top N entries | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs:112` |

## 7) Rate Limit / Timeout / Background Interval

| Value | Meaning | Location |
|---|---|---|
| `5 per 60s` | Login endpoint rate limit | `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs:32` |
| `20 per 1m` | Auth-session endpoint rate limit | `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs:34` |
| `30 per 1m` | Community write rate limit | `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs:36` |
| `60 per 1m` | Call history rate limit | `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs:38` |
| `0` | Rate limiter queue limit | `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs:61` |
| `>=1s` | Retry-after minimum seconds | `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs:75` |
| `5s` | CallHub initiate signal rate gate | `Backend/src/TarotNow.Api/Hubs/CallHub.Signaling.InitiateRespond.cs:26` |
| `2s` | CallHub respond signal rate gate | `Backend/src/TarotNow.Api/Hubs/CallHub.Signaling.InitiateRespond.cs:55` |
| `2s` | CallHub end-call signal rate gate | `Backend/src/TarotNow.Api/Hubs/CallHub.Signaling.EndCall.cs:28` |
| `12s` | Disconnect grace period | `Backend/src/TarotNow.Api/Hubs/CallHub.ConnectionState.cs:10` |
| `2m` | Conversation access cache TTL | `Backend/src/TarotNow.Api/Hubs/CallHub.WebRTC.cs:10` |
| `15m` | Presence timeout threshold | `Backend/src/TarotNow.Infrastructure/Presence/PresenceTimeoutBackgroundService.cs:18` |
| `60s` | Presence timeout scan interval | `Backend/src/TarotNow.Infrastructure/Presence/PresenceTimeoutBackgroundService.cs:21` |
| `15m` | In-memory online fallback threshold | `Backend/src/TarotNow.Infrastructure/Presence/InMemoryUserPresenceTracker.cs:86` |
| `30s` | Call timeout check interval | `Backend/src/TarotNow.Infrastructure/Call/CallTimeoutBackgroundService.cs:14` |
| `60s` | Call timeout threshold | `Backend/src/TarotNow.Infrastructure/Call/CallTimeoutBackgroundService.cs:15` |
| `1h` | Escrow timer scan interval | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.cs:11` |
| `1m` | Leaderboard snapshot startup delay | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs:36` |
| `00:05..00:15 UTC` | Leaderboard snapshot run window | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs:45` |
| `1h` | Sleep after daily snapshot | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs:49` |
| `1m` | Leaderboard snapshot loop sleep | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs:62` |
| `15m` | Entitlement daily reset loop interval | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/EntitlementDailyResetJob.cs:55` |
| `1h` | Subscription expiry job loop interval | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/SubscriptionExpiryJob.cs:57` |
| `1h` | Streak break job loop interval | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/StreakBreakBackgroundJob.cs:55` |
| `100 users + 100ms` | Batch throttle in streak break job | `Backend/src/TarotNow.Infrastructure/BackgroundJobs/StreakBreakBackgroundJob.cs:96` |
| `Math.Max(100, configured)` | Chat moderation queue capacity floor | `Backend/src/TarotNow.Infrastructure/Services/ChatModerationQueue.cs:25` |
| `10m` | Entitlement balance cache TTL | `Backend/src/TarotNow.Infrastructure/Services/EntitlementService.cs:87` |
| `500ms` | Slow request warning threshold | `Backend/src/TarotNow.Application/Behaviors/PerformanceBehavior.cs:36` |

## 8) Mongo TTL / Data Retention

| Value | Meaning | Location |
|---|---|---|
| `30 days` | TTL for gacha logs | `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Gacha.cs:31` |
| `90 days` | TTL for call sessions | `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Call.cs:114` |
| `90 days` | TTL for checkin records | `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Checkin.cs:23` |
| `90 days` | TTL for AI provider logs | `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Core.cs:84` |
| `30 days` | TTL for notifications | `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Core.cs:101` |
| `90 days` | TTL for quest progress | `Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Gamification.Helpers.cs:30` |

## 9) Pagination / Caps / Batch Size

| Value | Meaning | Location |
|---|---|---|
| `page=1, limit=20, max=100` | Ledger list defaults and cap | `Backend/src/TarotNow.Application/Features/Ledger/Queries/GetLedgerList/GetLedgerListQuery.cs:30` |
| `page=1, pageSize=10, max=50` | Feed query defaults and cap | `Backend/src/TarotNow.Application/Features/Community/Queries/GetFeed/GetFeedQuery.cs:33` |
| `page=1, pageSize=10, max=50` | Comments query defaults and cap | `Backend/src/TarotNow.Application/Features/Community/Queries/GetComments/GetCommentsQuery.cs:56` |
| `page=1, pageSize=20, max=100` | Moderation queue defaults and cap | `Backend/src/TarotNow.Application/Features/Moderation/Queries/GetModerationQueue/GetModerationQueueQuery.cs:44` |
| `max=50, fallback=20, page>=1` | Call history pagination constraints | `Backend/src/TarotNow.Application/Features/Calls/Queries/GetCallHistory/GetCallHistoryQueryHandler.cs:33` |
| `default=50, max=200` | Participant conversation ids limit | `Backend/src/TarotNow.Application/Features/Conversations/Queries/GetParticipantConversationIds/GetParticipantConversationIdsQuery.cs:49` |
| `10` | Initial metadata notifications page size | `Backend/src/TarotNow.Application/Features/User/Queries/GetInitialMetadata/GetInitialMetadataQueryHandler.cs:20` |
| `100` | Initial metadata active conversations page size | `Backend/src/TarotNow.Application/Features/User/Queries/GetInitialMetadata/GetInitialMetadataQueryHandler.cs:26` |
| `pageSize=20, max=200` | Withdrawal repository pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/WithdrawalRepository.cs:47` |
| `pageSize=20, max=200` | Dispute repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/ChatFinanceRepository.Disputes.cs:20` |
| `pageSize=20, max=200` | Deposit order repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/DepositOrderRepository.cs:82` |
| `pageSize=20, max=200` | Ledger repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/LedgerRepository.cs:42` |
| `default=200, max=1000` | Conversation repo list limit | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoConversationRepository.cs:108` |
| `default=50, max=200` | Message repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.Pagination.cs:22` |
| `default=50, max=200` | Message repo cursor pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.Pagination.cs:50` |
| `default=200, max=1000` | Payment offers fetch limit | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.PaymentOffers.Fetch.cs:60` |
| `20` | Latest payment offer scan limit | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.PaymentOffers.Fetch.cs:28` |
| `pageSize=20, max=200` | Notification repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoNotificationRepository.cs:69` |
| `default=10, max=200` | Reading session repo list limits | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoReadingSessionRepository.Listing.cs:21` |
| `default=12, max=200` | Reader profile repo list limits | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoReaderProfileRepository.Pagination.cs:25` |
| `pageSize=20, max=200` | User repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/UserRepository.cs:96` |
| `pageSize=20, max=200` | Report repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoReportRepository.cs:59` |
| `pageSize=20, max=200` | Reader request repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoReaderRequestRepository.cs:80` |
| `pageSize=20, max=200` | AI provider log repo pagination | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoAiProviderLogRepository.cs:61` |
| `Take(50)` | Subscription expiring batch size | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/SubscriptionRepository.Subscriptions.cs:41` |
| `Take(1000)` | Entitlement reset batch size | `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/SubscriptionRepository.Buckets.cs:89` |

## 10) Upload / Message Size / Media Constraints

| Value | Meaning | Location |
|---|---|---|
| `10*1024*1024` | SignalR max receive size (10MB) | `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.Platform.cs:52` |
| `10*1024*1024` | Community post upload request size limit (10MB) | `Backend/src/TarotNow.Api/Controllers/CommunityController.Posts.cs:44` |
| `10*1024*1024` | Avatar upload request size limit (10MB) | `Backend/src/TarotNow.Api/Controllers/ProfileController.cs:104` |
| `52_428_800` | Media file max bytes (50MB) | `Backend/src/TarotNow.Application/Features/Conversations/Commands/SendMessage/SendMessageCommandHandler.MediaValidation.Guards.cs:35` |
| `1..600000 ms` | Voice duration limit | `Backend/src/TarotNow.Application/Features/Conversations/Commands/SendMessage/SendMessageCommandHandler.MediaValidation.Guards.cs:42` |
| `5_242_880` | Data URI max bytes (5MB) | `Backend/src/TarotNow.Application/Features/Conversations/Commands/SendMessage/SendMessageCommandHandler.MediaValidation.DataUri.cs:82` |
| `2048 x 2048` | Processed image max dimensions | `Backend/src/TarotNow.Infrastructure/Services/MediaProcessorService.cs:16` |
| `70` | AVIF output quality | `Backend/src/TarotNow.Infrastructure/Services/MediaProcessorService.cs:19` |
| `-b:a 16k -ac 1` | Audio bitrate/channels in FFmpeg conversion | `Backend/src/TarotNow.Infrastructure/Services/MediaProcessorService.cs:169` |
| `maxDimension=512, quality=80` | ImageSharp default processing params | `Backend/src/TarotNow.Infrastructure/Services/ImageSharpProcessingService.cs:32` |

## 11) AI / Auth / Security Defaults

| Value | Meaning | Location |
|---|---|---|
| `gpt-4o-mini` | Default AI model | `Backend/src/TarotNow.Infrastructure/Options/AiProviderOptions.cs:13` |
| `30s` | Default AI timeout | `Backend/src/TarotNow.Infrastructure/Options/AiProviderOptions.cs:16` |
| `2` | Default AI retries | `Backend/src/TarotNow.Infrastructure/Options/AiProviderOptions.cs:19` |
| `gpt-4o-mini` | OpenAI provider fallback model | `Backend/src/TarotNow.Infrastructure/Services/OpenAiProvider.cs:51` |
| `2` | OpenAI provider fallback retries | `Backend/src/TarotNow.Infrastructure/Services/OpenAiProvider.cs:54` |
| `30s` | OpenAI provider fallback timeout | `Backend/src/TarotNow.Infrastructure/Services/OpenAiProvider.cs:57` |
| `200ms * (attempt+1)` | OpenAI streaming retry backoff | `Backend/src/TarotNow.Infrastructure/Services/OpenAiProvider.Streaming.cs:77` |
| `0.7` | OpenAI streaming temperature | `Backend/src/TarotNow.Infrastructure/Services/OpenAiProvider.Streaming.cs:121` |
| `15 min` | Access token expiry default | `Backend/src/TarotNow.Infrastructure/Options/JwtOptions.cs:16` |
| `7 days` | Refresh token expiry default | `Backend/src/TarotNow.Infrastructure/Options/JwtOptions.cs:19` |
| `15` | Access token fallback minutes | `Backend/src/TarotNow.Infrastructure/Services/Security/JwtTokenSettings.cs:20` |
| `7` | Refresh token fallback days | `Backend/src/TarotNow.Infrastructure/Services/Security/JwtTokenSettings.cs:25` |
| `7 days` | Refresh cookie fallback expiry | `Backend/src/TarotNow.Infrastructure/Services/Security/RefreshTokenCookieService.cs:31` |
| `15` | Access token fallback minutes in service | `Backend/src/TarotNow.Infrastructure/Services/Security/JwtTokenService.cs:74` |
| `64` | Refresh token random bytes length | `Backend/src/TarotNow.Infrastructure/Services/Security/JwtTokenService.cs:84` |
| `19456` | Argon2 memory KB default | `Backend/src/TarotNow.Infrastructure/Options/Argon2Options.cs:7` |
| `2` | Argon2 iterations default | `Backend/src/TarotNow.Infrastructure/Options/Argon2Options.cs:10` |
| `1` | Argon2 parallelism default | `Backend/src/TarotNow.Infrastructure/Options/Argon2Options.cs:13` |
| `19456/2/1` | Argon2 hasher fallback params | `Backend/src/TarotNow.Infrastructure/Services/Security/Argon2idPasswordHasher.cs:12` |
| `8*1024 .. 1_048_576` | Argon2 memory clamp | `Backend/src/TarotNow.Infrastructure/Services/Security/Argon2idPasswordHasher.cs:34` |
| `1..10` | Argon2 iterations clamp | `Backend/src/TarotNow.Infrastructure/Services/Security/Argon2idPasswordHasher.cs:38` |
| `1..4` | Argon2 parallelism clamp | `Backend/src/TarotNow.Infrastructure/Services/Security/Argon2idPasswordHasher.cs:30` |
| `32` | Argon2 hash length | `Backend/src/TarotNow.Infrastructure/Services/Security/Argon2idPasswordHasher.cs:54` |
| `16` | Webhook secret minimum length | `Backend/src/TarotNow.Infrastructure/Payments/HmacPaymentGatewayService.cs:27` |
| `1000` | Chat moderation max queue default | `Backend/src/TarotNow.Infrastructure/Options/ChatModerationOptions.cs:10` |
| `15 min` | Email OTP expiry default | `Backend/src/TarotNow.Infrastructure/Services/Security/EmailOtp.cs:49` |
| `20` | TOTP secret bytes | `Backend/src/TarotNow.Infrastructure/Services/Security/TotpMfaService.cs:47` |
| `(2,2)` | TOTP verify clock drift window | `Backend/src/TarotNow.Infrastructure/Services/Security/TotpMfaService.cs:84` |
| `6` | Default backup code count | `Backend/src/TarotNow.Infrastructure/Services/Security/TotpMfaService.cs:91` |
| `% 100000000` | Backup code numeric space | `Backend/src/TarotNow.Infrastructure/Services/Security/TotpMfaService.cs:99` |
| `smtp.gmail.com` | SMTP default host | `Backend/src/TarotNow.Infrastructure/Options/SmtpOptions.cs:7` |
| `587` | SMTP default port | `Backend/src/TarotNow.Infrastructure/Options/SmtpOptions.cs:10` |
| sender defaults | SMTP sender fallback defaults | `Backend/src/TarotNow.Infrastructure/Options/SmtpOptions.cs:19` |
| `smtp.gmail.com` | SMTP fallback host in sender | `Backend/src/TarotNow.Infrastructure/Services/Email/SmtpEmailSender.cs:87` |
| `587` | SMTP fallback port in sender | `Backend/src/TarotNow.Infrastructure/Services/Email/SmtpEmailSender.cs:88` |
| sender fallbacks | SMTP sender fallback values | `Backend/src/TarotNow.Infrastructure/Services/Email/SmtpEmailSender.cs:89` |

## 12) Subscription Plans Seed (Economy-Critical)

| Value | Meaning | Location |
|---|---|---|
| `100` | Weekly Premium price in diamonds | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:23` |
| `7` | Weekly Premium duration days | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:24` |
| `3` | Weekly entitlement `free_spread_3_daily` | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:26` |
| `1` | Weekly entitlement `free_spread_5_daily` | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:27` |
| `5` | Weekly entitlement `free_ai_stream_daily` | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:28` |
| `1` | Weekly plan display order | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:31` |
| `300` | Monthly Premium price in diamonds | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:43` |
| `30` | Monthly Premium duration days | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:44` |
| `5` | Monthly entitlement `free_spread_3_daily` | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:46` |
| `3` | Monthly entitlement `free_spread_5_daily` | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:47` |
| `10` | Monthly entitlement `free_ai_stream_daily` | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:48` |
| `120` | Monthly entitlement `bonus_exp_multiplier` | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:49` |
| `2` | Monthly plan display order | `Backend/src/TarotNow.Infrastructure/Migrations/20260406074109_SeedInitialSubscriptionPlans.cs:52` |

## 13) Reader Default Pricing

| Value | Meaning | Location |
|---|---|---|
| `5` | Default diamond per question for reader profile | `Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReaderProfileDocument.cs:85` |

## Notes
- This report only lists hard-coded constants/thresholds/tunables that can impact economy, balance, performance, limits, and operations.
- Some values may also be overridden by runtime config/env; this file focuses on literals currently present in source code.
