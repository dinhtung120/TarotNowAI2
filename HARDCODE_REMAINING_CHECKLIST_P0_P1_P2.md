# CHECKLIST HARDCODE CÒN LẠI (FE/BE) ĐỂ MIGRATE SANG `system_configs`

Ngày quét: 2026-04-25  
Phạm vi quét: `Backend/src`, `Frontend/src`  
Nguyên tắc lọc: chỉ giữ hard-code có ý nghĩa business/operational/runtime tuning; bỏ qua tên cột DB, hằng schema, literal kỹ thuật không cần tuning.

## Tổng quan
- Tổng mục còn lại: **25**
- P0 (migrate ngay): **8**
- P1 (migrate sprint kế): **9**
- P2 (cleanup sau ổn định): **8**

---

## P0 - Ảnh hưởng trực tiếp business balance hoặc gây sai lệch policy

| ID | Vị trí | Hard-code còn lại | Rủi ro | Đề xuất migrate |
|---|---|---|---|---|
| P0-01 | `Backend/src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs:28,31` | `MinReasonLength = 10`, `DisputeWindowDuration = TimeSpan.FromHours(48)` | Rule dispute không điều chỉnh runtime dù đã có hạ tầng `system_configs` | `escrow.dispute.min_reason_length`, dùng lại `escrow.dispute_window_hours` |
| P0-02 | `Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.cs:54` + `Frontend/src/features/admin/disputes/application/useAdminDisputes.ts:36` + `Frontend/src/features/admin/disputes/presentation/AdminDisputesPage.tsx:28` | Default split `%` = `50` | FE/BE cùng hard-code, khó đổi policy tranh chấp theo campaign | `admin.dispute.default_split_percent_to_reader` |
| P0-03 | `Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.ReaderPolicy.cs:34-36` | `lookback = 7 ngày`, `threshold = 3` | Rule freeze reader sau dispute bị cứng, không tinh chỉnh được theo fraud trend | `admin.dispute.reader_freeze.lookback_days`, `admin.dispute.reader_freeze.threshold` |
| P0-04 | `Backend/src/TarotNow.Application/Features/Chat/Commands/RejectConversation/RejectConversationCommandHandler.Refunds.cs:67` + `Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.ReaderReply.cs:73` | fallback `: 24` khi đọc `EscrowDisputeWindowHours` | Inconsistent fallback: các luồng khác dùng `48` -> lệch cửa sổ release/dispute | Bỏ fallback cứng tại feature; đọc duy nhất từ `escrow.dispute_window_hours` |
| P0-05 | `Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.cs:18` + `...BillingAndExp.cs:149` | `ExpPerCard = 1m`, diamond multiplier `2m` (non-daily) | Economy progression không A/B test được | `progression.reading.exp_per_card`, `progression.reading.diamond_multiplier_non_daily` |
| P0-06 | `Backend/src/TarotNow.Application/Common/Constants/InventoryBusinessConstants.cs:28` | `LuckyStarOwnedTitleGoldReward = 500` | Reward value cứng, khó cân bằng inflation | `inventory.lucky_star.owned_title_gold_reward` |
| P0-07 | `Backend/src/TarotNow.Infrastructure/DependencyInjection.cs:94` + `Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs` | Hosted seed + toàn bộ cost/pity/probability hard-code | Có nguy cơ drift/ghi đè cấu hình gacha đã DB hóa | Tắt `GachaSeedService` ở runtime production; source-of-truth chỉ `gacha.pools` |
| P0-08 | `Frontend/src/features/auth/domain/schemas.ts:86` + `Backend/src/TarotNow.Application/Features/Auth/Commands/Register/RegisterCommandValidator.cs:53` | FE `age >= 16`, BE `minAge = 18` | Policy tuổi đăng ký bị lệch FE/BE | Chuẩn hóa 1 nguồn: `legal.minimum_age` (hoặc giữ cố định 18 nhưng FE phải đọc từ runtime policy) |

---

## P1 - Ảnh hưởng vận hành/realtime, nên migrate sau P0

| ID | Vị trí | Hard-code còn lại | Rủi ro | Đề xuất migrate |
|---|---|---|---|---|
| P1-01 | `Frontend/src/features/chat/presentation/components/usePaymentOfferModalState.ts:11,27,32,34` | amount mặc định `10`, note max `100` | Offer UX/policy không đồng bộ với BE khi đổi rule | `chat.payment_offer.default_amount`, `chat.payment_offer.max_note_length` |
| P1-02 | `Frontend/src/features/reader/presentation/ReaderApplyPage.tsx:38-39` + `Frontend/src/features/profile/reader/application/useProfileReaderSettingsPage.ts:69-70` | fallback `1` năm, `50` diamond | Khi runtime policy API lỗi vẫn hiển thị giá trị cứng | Bỏ fallback cứng; dùng trạng thái `policy_unavailable` hoặc key `reader.*` từ `/me/runtime-policies` |
| P1-03 | `Frontend/src/shared/application/hooks/usePresenceConnection.ts:12-15,110` + `Frontend/src/shared/application/hooks/useChatRealtimeSync.ts:21-23,140,144` + `Frontend/src/features/chat/application/chat-connection/useChatSignalRLifecycle.ts:47,89` | reconnect schedule, cooldown `45_000/60_000`, negotiation timeout `8_000`, server timeout `120000` | Khó tune realtime khi traffic tăng/giảm | Nhóm key `realtime.*` trong `system_configs` + expose trong runtime policies |
| P1-04 | `Frontend/src/shared/infrastructure/http/clientFetch.ts:5-6` + `Frontend/src/shared/infrastructure/http/serverHttpClient.ts:29-30` | default timeout `8_000`, min `1_000` | Không tune được theo môi trường mạng | `operational.http.client_timeout_ms`, `operational.http.server_timeout_ms`, `operational.http.min_timeout_ms` |
| P1-05 | `Frontend/src/shared/application/hooks/useRuntimePolicies.ts:8-9` | timeout `8_000`, stale time `15_000` | Snapshot policy có thể stale hoặc gọi quá dày | `operational.runtime_policies.timeout_ms`, `operational.runtime_policies.stale_ms` |
| P1-06 | `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs:87-89` | Redis `ConnectTimeout=2000`, `SyncTimeout=2000`, `ConnectRetry=1` | Tuning kết nối Redis phải sửa code | `operational.redis.connect_timeout_ms`, `operational.redis.sync_timeout_ms`, `operational.redis.connect_retry` |
| P1-07 | `Backend/src/TarotNow.Infrastructure/Options/AiProviderOptions.cs:16,19,22,25` + `Backend/src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.cs:64-77` | timeout `30`, retries `2`, retry delay `200`, temperature `0.7` | Có 2 tầng fallback (options + provider) dễ drift | Gom về 1 nguồn `operational.ai.*` trong `system_configs` (hoặc appsettings duy nhất nếu không muốn runtime edit) |
| P1-08 | `Backend/src/TarotNow.Infrastructure/Options/OutboxOptions.cs:11,16,21,26,31` | batch `50`, retry `12`, lock `120`, backoff `300`, poll `5` | Không tune động khi queue tăng đột biến | `operational.outbox.*` (hoặc chuẩn hóa chỉ appsettings, bỏ hard default trong code) |
| P1-09 | `Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs:131,134` | fallback `24` cho response/refund due | Trùng fallback với settings, tăng nợ kỹ thuật | Bỏ fallback tại feature, chỉ dùng `_systemConfigSettings.EscrowReaderResponseDueHours/_EscrowAutoRefundHours` |

---

## P2 - Cleanup/chuẩn hóa để giảm nợ kỹ thuật

| ID | Vị trí | Hard-code còn lại | Rủi ro | Đề xuất migrate |
|---|---|---|---|---|
| P2-01 | `Frontend/src/features/reader/application/actions/directory.ts:116,153` + `Frontend/src/features/reader/application/useReadersDirectoryPage.ts:14,19,35,37` + `Frontend/src/shared/server/prefetch/runners.ts:50,82,90,171,205,232,243,271,273,297,465,486,498,520,552` | page size / limit / stale-time nhiều giá trị cố định (`12`, `4`, `100`, `10`, `20`, `300`, `30_000`...) | Tuning UX/perf phân tán | Gom về config FE typed lấy từ runtime policies (`ui.pagination.*`, `ui.prefetch.*`) |
| P2-02 | `Frontend/src/features/chat/application/chat-connection/utils.ts:7` | `CHAT_PAGE_SIZE = 50` | Khó tune khi volume chat lớn | `chat.history.page_size` |
| P2-03 | `Frontend/src/features/chat/application/chat-connection/useChatSignalRLifecycle.ts:123,136` + `Frontend/src/shared/application/hooks/useChatRealtimeSync.ts:107,112,118,123` | typing clear `2500`, debounce `1000`, guards `2000/3000` | Hành vi realtime khó cân chỉnh | nhóm `realtime.chat.*` |
| P2-04 | `Backend/src/TarotNow.Application/Features/Chat/Queries/GetParticipantConversationIds/GetParticipantConversationIdsQuery.cs:49` | `request.MaxCount <= 0 ? 50 : Math.Min(..., 200)` | limit API cứng | `chat.participants.default_page_size`, `chat.participants.max_page_size` |
| P2-05 | `Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs` (nhiều dòng default: 10-114) | default pricing/followup/wallet/presence/escrow/chat/economy/reader/gamification | Trùng nguồn với `system_configs`, có thể drift | Giữ làm bootstrap fallback tối thiểu hoặc giảm còn fallback an toàn duy nhất |
| P2-06 | `Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.*.cs` (ví dụ: `PricingAndAi.cs:8,13,18,23,28,33`; `FollowupAndOps.cs:13,45,50,55,60,65,85,90,95,100,105`; `ChatAndEconomy.cs:14,31,37,44,49`) | nhiều literal fallback (`50/5/100/10/500/...`) | fallback phân tán khó kiểm soát | Chuẩn hóa fallback theo 1 helper/1 snapshot bootstrap |
| P2-07 | `Backend/src/TarotNow.Application/Common/SystemConfigs/SystemConfigRegistry.Defaults.Gacha.cs` + `...Defaults.Gamification.cs` | default JSON seed chứa toàn bộ giá trị kinh tế/quest | Có thể được xem là “hard-code bootstrap” | Giữ nếu mục tiêu bootstrap; nếu muốn sạch tuyệt đối thì import baseline từ SQL seed/JSON artifact |
| P2-08 | `Frontend/src/shared/media-upload/constants.ts:2-4,6-12,20-21` | kích thước file, duration, compression profile, retry attempts/delay | tuning upload phải sửa FE code | expose qua runtime policy `media.upload.*` (có clamp an toàn ở FE) |

---

## Thứ tự thực thi đề xuất
1. Xử lý toàn bộ P0 trước để dừng drift policy FE/BE và economy/dispute.
2. P1 xử lý theo nhóm: `realtime + http + ai + redis + outbox`.
3. P2 gom cleanup thành 1 đợt chuẩn hóa defaults/fallback.

## Gợi ý nguyên tắc implement
- Domain/business flow chỉ đọc typed provider, không đọc key string trực tiếp.
- Nếu key bắt buộc thiếu hoặc parse lỗi: fail fast ở flow liên quan (không silent fallback trong business quan trọng).
- Với tham số operational nhạy cảm (timeout/retry): giữ clamp min/max cứng trong code để chống config sai.
