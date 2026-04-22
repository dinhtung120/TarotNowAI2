# DANH SÁCH TOÀN BỘ HARD-CODED VALUES TRONG BACKEND (BẢN ĐÃ LỌC)

## Tổng quan
- Số lượng hard-coded values tìm thấy (sau khi lọc): 260
- Số file bị ảnh hưởng: 83
- Đã loại bỏ: toàn bộ literal không có ý nghĩa tuning (tên cột, tên key/header/cookie, tên route, enum code, text message invariant).
- Khuyến nghị phân loại:
  - Nên đưa vào `AppSettings + Options`: khoảng 136 giá trị vận hành (TTL, timeout, retry, batch, rate-limit, window).
  - Nên đưa vào `Database`: khoảng 94 giá trị kinh doanh/economy (pricing, gacha odds, quest reward, package nạp).
  - Nên giữ trong `Constants class` (đã tách một phần): khoảng 32 giá trị mang tính policy ổn định (biên validation/security floor).

## Danh sách chi tiết

### 1. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 11**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: `Jwt.ExpiryMinutes` cho access token.
  Lý do cần có data này: Đây là chính sách bảo mật theo môi trường.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đọc từ `JwtOptions` theo từng môi trường (dev/staging/prod) và có policy profile.
- **Dòng 12**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: `Jwt.RefreshExpiryDays`.
  Lý do cần có data này: Quy định vòng đời phiên đăng nhập dài hạn.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa vào policy config riêng cho mobile/web.
- **Dòng 15**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: `AuthSecurity.RefreshLockSeconds`.
  Lý do cần có data này: Chống race-condition khi refresh token.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ ở options; thêm dashboard theo dõi contention để tinh chỉnh.
- **Dòng 16**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: `RefreshIdempotencyWindowSeconds`.
  Lý do cần có data này: Chống replay/duplicate refresh trong cửa sổ ngắn.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong config, expose metric hit-rate.
- **Dòng 17**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: TTL blacklist access token.
  Lý do cần có data này: Đồng bộ logout/revoke giữa node.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Ràng theo `access token expiry` thay vì số cố định.
- **Dòng 18**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: TTL revocation marker cho session.
  Lý do cần có data này: Chặn access token cũ sau revoke.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đồng bộ theo expiry thực tế của token.
- **Dòng 19**: `2592000`
  Ý nghĩa / ngữ cảnh sử dụng: `SessionCacheTtlSeconds`.
  Lý do cần có data này: Giảm truy vấn DB cho session active.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách policy cache TTL theo role/độ rủi ro.
- **Dòng 20**: `86400`
  Ý nghĩa / ngữ cảnh sử dụng: `ReplaySecurityRecordTtlSeconds`.
  Lý do cần có data này: Lưu dấu hiệu replay phục vụ điều tra bảo mật.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong options.
- **Dòng 21**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: `AuthSecurity.CleanupBatchSize`.
  Lý do cần có data này: Batch cleanup refresh/session.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Theo dõi DB load để auto-tune theo giờ thấp điểm.
- **Dòng 22**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: `AuthSecurity.CleanupIntervalMinutes`.
  Lý do cần có data này: Chu kỳ dọn dữ liệu auth.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ ở appsettings theo môi trường.
- **Dòng 23**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Giữ lại refresh token đã revoke/hết hạn.
  Lý do cần có data này: Cân bằng audit và dung lượng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách retention cho prod/non-prod.
- **Dòng 24**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Retention auth sessions revoked.
  Lý do cần có data này: Truy vết bảo mật và xử lý sự cố.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Điều khiển bằng policy data-retention trung tâm.
- **Dòng 42**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: `Deposit.LinkExpiryMinutes`.
  Lý do cần có data này: Hạn sống payment link PayOS.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ ở config, có override theo campaign.
- **Dòng 46**: `50000`
  Ý nghĩa / ngữ cảnh sử dụng: Mệnh giá gói `topup_50k`.
  Lý do cần có data này: Giá bán gói nạp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển catalog package sang DB + admin UI.
- **Dòng 47**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_50k`.
  Lý do cần có data này: Quy đổi tiền->diamond.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Quản lý từ bảng `DepositPackages`.
- **Dòng 52**: `100000`
  Ý nghĩa / ngữ cảnh sử dụng: Mệnh giá gói `topup_100k`.
  Lý do cần có data này: Cấu hình thương mại.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB configurable.
- **Dòng 53**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_100k`.
  Lý do cần có data này: Tỉ lệ quy đổi package.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB + version package.
- **Dòng 58**: `200000`
  Ý nghĩa / ngữ cảnh sử dụng: Mệnh giá gói `topup_200k`.
  Lý do cần có data này: Offer nạp tiền.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB campaign pricing.
- **Dòng 59**: `2000`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_200k`.
  Lý do cần có data này: Cân bằng economy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB + rollout theo cohort.
- **Dòng 64**: `500000`
  Ý nghĩa / ngữ cảnh sử dụng: Mệnh giá gói `topup_500k`.
  Lý do cần có data này: Đóng gói thương mại.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đặt trong DB.
- **Dòng 65**: `5000`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_500k`.
  Lý do cần có data này: Reward package.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB editable.
- **Dòng 70**: `1000000`
  Ý nghĩa / ngữ cảnh sử dụng: Mệnh giá gói `topup_1m`.
  Lý do cần có data này: Gói nạp cao.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB với effective date.
- **Dòng 71**: `10000`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_1m`.
  Lý do cần có data này: Quy tắc thưởng gói cao.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB + AB test.
- **Dòng 84**: `19456`
  Ý nghĩa / ngữ cảnh sử dụng: `Argon2.MemoryKB`.
  Lý do cần có data này: Cost tham số hash mật khẩu.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ ở options theo profile máy chủ.
- **Dòng 85**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: `Argon2.Iterations`.
  Lý do cần có data này: Độ mạnh hash.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ ở options, audit định kỳ.
- **Dòng 86**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: `Argon2.Parallelism`.
  Lý do cần có data này: Cân bằng CPU.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Có profile theo instance size.
- **Dòng 90**: `587`
  Ý nghĩa / ngữ cảnh sử dụng: SMTP port.
  Lý do cần có data này: Kết nối mail provider.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ config môi trường.
- **Dòng 100**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: `AiProvider.TimeoutSeconds`.
  Lý do cần có data này: SLA gọi model.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Per-model timeout profile.
- **Dòng 101**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: `AiProvider.MaxRetries`.
  Lý do cần có data này: Retry request AI khi lỗi tạm thời.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Tách retry theo lỗi 429/5xx.
- **Dòng 134**: `2000`
  Ý nghĩa / ngữ cảnh sử dụng: `ChatModeration.MaxQueueSize`.
  Lý do cần có data này: Giới hạn hàng đợi moderation.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Alert theo queue depth để tune.
- **Dòng 153**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: `SystemConfig.DailyAiQuota`.
  Lý do cần có data này: Quota AI/ngày.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Quota theo tier người dùng trong DB.
- **Dòng 154**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: `InFlightAiCap`.
  Lý do cần có data này: Chống bùng nổ request đồng thời.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Có throttle động theo tải.
- **Dòng 155**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: `ReadingRateLimitSeconds`.
  Lý do cần có data này: Chặn spam đọc bài.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa về rule engine/rate policy.
- **Dòng 167**: `10485760`
  Ý nghĩa / ngữ cảnh sử dụng: `ObjectStorage.MaxUploadBytes`.
  Lý do cần có data này: Trần kích thước upload.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Tách giới hạn theo loại media.
- **Dòng 168**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: `PresignExpiryMinutes`.
  Lý do cần có data này: Hạn URL upload.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Per-scope expiry (avatar/chat/community).
- **Dòng 169**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: `CommunityOrphanTtlHours`.
  Lý do cần có data này: TTL asset mồ côi.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Điều chỉnh theo storage cost.
- **Dòng 170**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: `ObjectStorage.CleanupBatchSize`.
  Lý do cần có data này: Tốc độ cleanup.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Auto-tune theo error rate của R2.
- **Dòng 171**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: `ObjectStorage.CleanupIntervalMinutes`.
  Lý do cần có data này: Chu kỳ job dọn media.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Cron theo khung giờ tải thấp.

### 2. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 10**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Default `DailyAiQuota` fallback code-level.
  Lý do cần có data này: Giá trị mặc định khi thiếu config.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong options, đồng bộ với appsettings.
- **Dòng 13**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Default `InFlightAiCap`.
  Lý do cần có data này: Safety net runtime.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong options.
- **Dòng 16**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Default `ReadingRateLimitSeconds`.
  Lý do cần có data này: Chặn spam nếu config thiếu.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách default theo environment.
- **Dòng 22**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá Gold cho `spread_3`.
  Lý do cần có data này: Rule kinh tế lõi.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng `Pricing` trong DB.
- **Dòng 25**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá Diamond cho `spread_3`.
  Lý do cần có data này: Rule pricing linh hoạt.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB + effective date.
- **Dòng 28**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá Gold cho `spread_5`.
  Lý do cần có data này: Cân bằng economy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB pricing.
- **Dòng 31**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá Diamond cho `spread_5`.
  Lý do cần có data này: Rule thương mại thay đổi theo campaign.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB + admin panel.
- **Dòng 34**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Giá Gold cho `spread_10`.
  Lý do cần có data này: Hard business rule.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách sang data layer.
- **Dòng 37**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá Diamond cho `spread_10`.
  Lý do cần có data này: Pricing cần thay đổi nhanh.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB configurable.

### 3. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 18**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback `Spread3GoldCost`.
  Lý do cần có data này: Chặn null/misconfig.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Đồng bộ fallback với central pricing defaults.
- **Dòng 19**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback `Spread3DiamondCost`.
  Lý do cần có data này: Bảo vệ runtime.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Dùng 1 nguồn fallback duy nhất.
- **Dòng 21**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback `Spread5GoldCost`.
  Lý do cần có data này: Đảm bảo không crash khi config lỗi.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ fallback, nhưng log cảnh báo bắt buộc.
- **Dòng 22**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback `Spread5DiamondCost`.
  Lý do cần có data này: Runtime safety.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Theo dõi số lần dùng fallback.
- **Dòng 24**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback `Spread10GoldCost`.
  Lý do cần có data này: Business continuity.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Dùng centralized default provider.
- **Dòng 25**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback `Spread10DiamondCost`.
  Lý do cần có data này: Tương thích backward config.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Đồng bộ cùng pricing service.
- **Dòng 28**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback `DailyAiQuota`.
  Lý do cần có data này: Safety default.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Move vào default profile config.
- **Dòng 29**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback `InFlightAiCap`.
  Lý do cần có data này: Chống quá tải.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Dynamic throttle từ telemetry.
- **Dòng 30**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Fallback rate-limit seconds.
  Lý do cần có data này: Kiểm soát spam khi mất config.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: sync với API policy.
- **Dòng 33**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: `DailyCheckinGold` cố định.
  Lý do cần có data này: Reward kinh tế hàng ngày.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa vào `Gamification/RewardConfig` trong DB.
- **Dòng 34**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: `StreakFreezeWindowHours`.
  Lý do cần có data này: Cửa sổ mua phục hồi streak.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Cấu hình động theo event/campaign.

### 4. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/DepositOptions.cs`
- **Dòng 13**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Default link expiry của nạp tiền.
  Lý do cần có data này: Fallback khi chưa bind config.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong options.
- **Dòng 18**: `50_000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá gói nạp `topup_50k`.
  Lý do cần có data này: Cấu hình kinh doanh.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB package catalog.
- **Dòng 18**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond của gói `topup_50k`.
  Lý do cần có data này: Rule economy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB configurable.
- **Dòng 19**: `100_000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá gói `topup_100k`.
  Lý do cần có data này: Bảng giá nạp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Quản trị từ DB.
- **Dòng 19**: `1_000`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_100k`.
  Lý do cần có data này: Tỷ lệ quy đổi.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 20**: `200_000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá gói `topup_200k`.
  Lý do cần có data này: Gói thương mại.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 20**: `2_000`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_200k`.
  Lý do cần có data này: Cân bằng nạp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 21**: `500_000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá gói `topup_500k`.
  Lý do cần có data này: Cấu hình gói nạp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 21**: `5_000`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_500k`.
  Lý do cần có data này: Reward package.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 22**: `1_000_000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá gói `topup_1m`.
  Lý do cần có data này: Gói premium nạp tiền.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 22**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: Diamond gói `topup_1m`.
  Lý do cần có data này: Quy tắc thưởng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.

### 5. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/DepositPayOsSettings.cs`
- **Dòng 19**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: fallback `LinkExpiryMinutes` nếu giá trị <= 0.
  Lý do cần có data này: Tránh tạo link không hợp lệ.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ fallback; thêm warning log khi fallback xảy ra.

### 6. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 11**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: refresh lock seconds.
  Lý do cần có data này: Chống refresh race.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Keep in options.
- **Dòng 16**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: idempotency window refresh.
  Lý do cần có data này: Replay safety.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Keep in options.
- **Dòng 21**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: blacklist TTL.
  Lý do cần có data này: token revoke propagation.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Sync theo access token expiry thực tế.
- **Dòng 26**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: session revoke marker TTL.
  Lý do cần có data này: Chặn session cũ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Policy theo expiry.
- **Dòng 31**: `30 * 24 * 60 * 60`
  Ý nghĩa / ngữ cảnh sử dụng: session cache TTL 30 ngày.
  Lý do cần có data này: Cache session read path.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Cho phép override theo role/device.
- **Dòng 36**: `24 * 60 * 60`
  Ý nghĩa / ngữ cảnh sử dụng: replay security record TTL 1 ngày.
  Lý do cần có data này: lưu dấu hiệu tấn công.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Keep.
- **Dòng 41**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: cleanup batch.
  Lý do cần có data này: giới hạn I/O mỗi vòng.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: auto-tune theo DB latency.
- **Dòng 46**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: cleanup interval minutes.
  Lý do cần có data này: scheduler auth cleanup.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Keep.
- **Dòng 51**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: retention refresh token ngày.
  Lý do cần có data này: compliance + storage.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: move vào retention policy config.
- **Dòng 56**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: retention revoked session ngày.
  Lý do cần có data này: audit.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: retention policy.

### 7. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 20**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: fallback blacklist TTL.
  Lý do cần có data này: runtime safety.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: centralized fallback constants.
- **Dòng 23**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: fallback session revocation TTL.
  Lý do cần có data này: tránh TTL = 0.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: centralized fallback.
- **Dòng 26**: `30 * 24 * 60 * 60`
  Ý nghĩa / ngữ cảnh sử dụng: fallback session cache TTL.
  Lý do cần có data này: default dài hạn.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 29**: `24 * 60 * 60`
  Ý nghĩa / ngữ cảnh sử dụng: fallback replay record TTL.
  Lý do cần có data này: bảo vệ fail-safe.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 8. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/JwtOptions.cs`
- **Dòng 16**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: default access token expiry minutes.
  Lý do cần có data này: fallback bảo mật.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ trong options.
- **Dòng 19**: `7`
  Ý nghĩa / ngữ cảnh sử dụng: default refresh token expiry days.
  Lý do cần có data này: fallback session lifetime.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ trong options.

### 9. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/JwtTokenSettings.cs`
- **Dòng 20**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: fallback access expiry.
  Lý do cần có data này: đảm bảo >0.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 25**: `7`
  Ý nghĩa / ngữ cảnh sử dụng: fallback refresh expiry.
  Lý do cần có data này: đảm bảo >0.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 10. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AiProviderOptions.cs`
- **Dòng 16**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: timeout mặc định gọi AI.
  Lý do cần có data này: chống treo request.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: config per model/provider.
- **Dòng 19**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: max retry mặc định.
  Lý do cần có data này: retry lỗi tạm thời.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: tách retry theo mã lỗi.

### 11. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.cs`
- **Dòng 60**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: fallback `_maxRetries` khi config âm.
  Lý do cần có data này: tránh retry bất định.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: dùng fallback constant dùng chung.
- **Dòng 63**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: fallback timeout giây.
  Lý do cần có data này: ngăn timeout <=0.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 12. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.Streaming.cs`
- **Dòng 77**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: hệ số backoff ms cho retry tuyến tính.
  Lý do cần có data này: giãn retry tránh bão request.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển sang options (`RetryBaseDelayMs`).
- **Dòng 121**: `0.7`
  Ý nghĩa / ngữ cảnh sử dụng: `temperature` mặc định request chat completion.
  Lý do cần có data này: ảnh hưởng chất lượng/cost output AI.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình theo use-case (`creative`, `strict`).

### 13. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/ObjectStorageOptions.cs`
- **Dòng 12**: `10 * 1024 * 1024`
  Ý nghĩa / ngữ cảnh sử dụng: max upload bytes mặc định.
  Lý do cần có data này: giới hạn payload upload.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: tách theo media kind.
- **Dòng 15**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: presign expiry phút.
  Lý do cần có data này: bảo mật URL upload.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 18**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: orphan TTL giờ.
  Lý do cần có data này: dọn rác storage.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 21**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: cleanup batch size.
  Lý do cần có data này: giới hạn work mỗi vòng.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 24**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: cleanup interval phút.
  Lý do cần có data này: tần suất dọn.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 14. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/MediaUpload/MediaUploadConstants.cs`
- **Dòng 9**: `10` (phút)
  Ý nghĩa / ngữ cảnh sử dụng: TTL presign mặc định.
  Lý do cần có data này: upload session security.
  Đánh giá: Hợp lý (đã tách một phần)
  Đề xuất cải thiện cụ thể: Đồng bộ với `ObjectStorageOptions` để tránh lệch.
- **Dòng 12**: `24` (giờ)
  Ý nghĩa / ngữ cảnh sử dụng: TTL orphan community asset.
  Lý do cần có data này: chi phí lưu trữ.
  Đánh giá: Hợp lý (đã tách một phần)
  Đề xuất cải thiện cụ thể: giữ nhưng nên đọc từ options trước.
- **Dòng 15**: `10 * 1024 * 1024`
  Ý nghĩa / ngữ cảnh sử dụng: max image upload bytes.
  Lý do cần có data này: validate upload.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy từ config theo scope.
- **Dòng 18**: `5 * 1024 * 1024`
  Ý nghĩa / ngữ cảnh sử dụng: max voice upload bytes.
  Lý do cần có data này: kiểm soát chi phí băng thông.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào config.

### 15. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/Argon2Options.cs`
- **Dòng 7**: `19456`
  Ý nghĩa / ngữ cảnh sử dụng: memory KB mặc định Argon2.
  Lý do cần có data này: bảo mật mật khẩu.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ ở options.
- **Dòng 10**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: iterations mặc định.
  Lý do cần có data này: security-work factor.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: security review định kỳ.
- **Dòng 13**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: parallelism mặc định.
  Lý do cần có data này: cân bằng CPU.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 16. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Security/Argon2idPasswordHasher.cs`
- **Dòng 12**: `19456`
  Ý nghĩa / ngữ cảnh sử dụng: `DefaultMemoryKb` fallback.
  Lý do cần có data này: fail-safe bảo mật.
  Đánh giá: Hợp lý (đã tách một phần)
  Đề xuất cải thiện cụ thể: dùng chung 1 source với options.
- **Dòng 13**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: `DefaultIterations` fallback.
  Lý do cần có data này: fallback hợp lệ.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 14**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: `DefaultParallelism` fallback.
  Lý do cần có data này: fallback hợp lệ.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 30**: `1..4`
  Ý nghĩa / ngữ cảnh sử dụng: clamp parallelism.
  Lý do cần có data này: chống config quá mức.
  Đánh giá: Hợp lý (policy bảo mật)
  Đề xuất cải thiện cụ thể: keep constants, document policy.
- **Dòng 34**: `8 * 1024 .. 1_048_576`
  Ý nghĩa / ngữ cảnh sử dụng: clamp memory KB.
  Lý do cần có data này: guardrail tài nguyên/bảo mật.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ trong security policy constants.
- **Dòng 38**: `1..10`
  Ý nghĩa / ngữ cảnh sử dụng: clamp iterations.
  Lý do cần có data này: giới hạn cost hash.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 54**: `32`
  Ý nghĩa / ngữ cảnh sử dụng: hash length cố định.
  Lý do cần có data này: chuẩn hóa token hash output.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ ở constants class security.

### 17. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Security/JwtTokenService.cs`
- **Dòng 75**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: fallback access token expiry minutes.
  Lý do cần có data này: chặn expiry <=0.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 85**: `64`
  Ý nghĩa / ngữ cảnh sử dụng: số byte ngẫu nhiên refresh token.
  Lý do cần có data này: entropy bảo mật.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ ở security constants.

### 18. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/DependencyInjection.Auth.cs`
- **Dòng 195**: `32`
  Ý nghĩa / ngữ cảnh sử dụng: độ dài tối thiểu `Jwt:SecretKey`.
  Lý do cần có data này: policy bảo mật chữ ký JWT.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants và document trong security guideline.

### 19. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/HmacPaymentGatewayService.cs`
- **Dòng 27**: `16`
  Ý nghĩa / ngữ cảnh sử dụng: chiều dài tối thiểu webhook secret.
  Lý do cần có data này: tránh secret yếu.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants; thêm check entropy.

### 20. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 36**: `5` requests / `60` giây
  Ý nghĩa / ngữ cảnh sử dụng: policy `auth-login`.
  Lý do cần có data này: chống brute-force login.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: chuyển sang config `RateLimitPolicies`.
- **Dòng 39**: `100` requests / `1` phút
  Ý nghĩa / ngữ cảnh sử dụng: policy `auth-session`.
  Lý do cần có data này: giới hạn thao tác auth đã đăng nhập.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config theo tier client.
- **Dòng 40**: `30` requests / `1` phút
  Ý nghĩa / ngữ cảnh sử dụng: policy `auth-refresh`.
  Lý do cần có data này: chống lạm dụng refresh.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: config hóa.
- **Dòng 41**: `10` requests / `1` phút
  Ý nghĩa / ngữ cảnh sử dụng: policy `auth-refresh-token-family`.
  Lý do cần có data này: chặn replay theo token family.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep, nhưng config hóa.
- **Dòng 42**: `30` requests / `1` phút
  Ý nghĩa / ngữ cảnh sử dụng: policy `auth-logout`.
  Lý do cần có data này: chống spam logout endpoint.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: config hóa.
- **Dòng 45**: `60` requests / `1` phút
  Ý nghĩa / ngữ cảnh sử dụng: policy `community-write`.
  Lý do cần có data này: anti-spam community.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy theo trust-score user.
- **Dòng 47**: `200` requests / `1` phút
  Ý nghĩa / ngữ cảnh sử dụng: policy `chat-standard`.
  Lý do cần có data này: điều khiển lưu lượng chat.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: tách read/write policy.
- **Dòng 70**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: `QueueLimit` cho limiter = từ chối ngay.
  Lý do cần có data này: tránh hàng chờ đè latency.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Keep, có thể config per endpoint.
- **Dòng 84**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: `Retry-After` tối thiểu 1 giây.
  Lý do cần có data này: tránh trả 0 gây retry nóng.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 154**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: hash prefix length cho partition key device refresh.
  Lý do cần có data này: cân bằng entropy/key size.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào security settings.
- **Dòng 200**: `16`
  Ý nghĩa / ngữ cảnh sử dụng: hash prefix length refresh-token partition.
  Lý do cần có data này: key cardinality cho limiter.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào config.
- **Dòng 220**: `8`
  Ý nghĩa / ngữ cảnh sử dụng: min hash prefix clamp.
  Lý do cần có data này: tránh prefix quá ngắn.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants.

### 21. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.Platform.cs`
- **Dòng 95**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: `ForwardLimit` cho forwarded headers.
  Lý do cần có data này: giới hạn số proxy hop tin cậy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển vào config theo topology.
- **Dòng 174**: `10 * 1024 * 1024`
  Ý nghĩa / ngữ cảnh sử dụng: `SignalR MaximumReceiveMessageSize`.
  Lý do cần có data này: giới hạn payload realtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình theo kênh chat/media.

### 22. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Services/PresenceTimeoutBackgroundService.cs`
- **Dòng 18**: `15` phút
  Ý nghĩa / ngữ cảnh sử dụng: timeout presence.
  Lý do cần có data này: xác định online/offline.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào options `PresenceOptions`.
- **Dòng 21**: `60` giây
  Ý nghĩa / ngữ cảnh sử dụng: chu kỳ quét timeout.
  Lý do cần có data này: cân bằng độ trễ/cost.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: `PresenceOptions.ScanIntervalSeconds`.

### 23. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Realtime/RedisUserPresenceTracker.cs`
- **Dòng 13**: `15` phút
  Ý nghĩa / ngữ cảnh sử dụng: `OnlineWindow` fallback khi mất connection set.
  Lý do cần có data này: chống nhấp nháy trạng thái online.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ 1 nguồn `PresenceOptions` với worker timeout.

### 24. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Realtime/InMemoryUserPresenceTracker.cs`
- **Dòng 86**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: threshold phút để coi user còn online.
  Lý do cần có data này: fallback online window in-memory.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: tái sử dụng `PresenceOptions` dùng chung.

### 25. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs`
- **Dòng 19**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: `BatchSize` mỗi lần claim outbox.
  Lý do cần có data này: throughput xử lý domain events.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào `OutboxOptions`.
- **Dòng 20**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: `MaxRetryAttempts` trước khi dead-letter.
  Lý do cần có data này: retry budget.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình theo loại event.
- **Dòng 21**: `2` phút
  Ý nghĩa / ngữ cảnh sử dụng: lock timeout batch.
  Lý do cần có data này: reclaim stale processing lock.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config hóa.
- **Dòng 154**: `3900`
  Ý nghĩa / ngữ cảnh sử dụng: truncate length của `LastError`.
  Lý do cần có data này: giữ lỗi trong giới hạn cột DB.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants theo schema.
- **Dòng 179**: `300`
  Ý nghĩa / ngữ cảnh sử dụng: cap backoff seconds.
  Lý do cần có data này: chặn backoff quá dài.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình `OutboxOptions.MaxBackoffSeconds`.

### 26. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxProcessorWorker.cs`
- **Dòng 13**: `5` giây
  Ý nghĩa / ngữ cảnh sử dụng: poll interval outbox worker.
  Lý do cần có data này: độ trễ dispatch event.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình `OutboxOptions.PollIntervalSeconds`.

### 27. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 16**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: `MaxBatchLoopsPerCycle`.
  Lý do cần có data này: giới hạn vòng dọn mỗi chu kỳ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config hóa.
- **Dòng 166**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: lease lock tối thiểu giây.
  Lý do cần có data này: tránh lease quá ngắn.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants.
- **Dòng 167**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: lease lock tối đa giây.
  Lý do cần có data này: tránh lock treo quá lâu.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 172**: `50..5000`
  Ý nghĩa / ngữ cảnh sử dụng: clamp batch size cleanup.
  Lý do cần có data này: guardrail DB load.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 177**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: fallback cleanup interval phút.
  Lý do cần có data này: default scheduler.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 28. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs`
- **Dòng 36**: `1` phút
  Ý nghĩa / ngữ cảnh sử dụng: startup delay job snapshot.
  Lý do cần có data này: tránh tranh chấp tài nguyên lúc boot.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config hóa.
- **Dòng 45**: `00:05..00:15` UTC
  Ý nghĩa / ngữ cảnh sử dụng: cửa sổ chạy snapshot mỗi ngày.
  Lý do cần có data này: bảo đảm dữ liệu đã ổn định đầu ngày.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng cron config.
- **Dòng 49**: `1` giờ
  Ý nghĩa / ngữ cảnh sử dụng: sleep sau khi snapshot thành công.
  Lý do cần có data này: tránh chạy trùng.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep hoặc cron.
- **Dòng 62**: `1` phút
  Ý nghĩa / ngữ cảnh sử dụng: vòng loop interval.
  Lý do cần có data này: tần suất kiểm tra.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cron.
- **Dòng 90**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: chỉ snapshot monthly ngày mùng 1.
  Lý do cần có data này: boundary period.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 112**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: số lượng top entry lưu snapshot.
  Lý do cần có data này: payload leaderboard.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào `LeaderboardOptions.TopN`.

### 29. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/StreakBreakBackgroundJob.cs`
- **Dòng 55**: `1` giờ
  Ý nghĩa / ngữ cảnh sử dụng: chu kỳ quét break streak.
  Lý do cần có data này: scheduler domain job.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cron config.
- **Dòng 94**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: throttle mỗi 100 user xử lý.
  Lý do cần có data này: giảm áp lực DB.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: expose config.
- **Dòng 96**: `100` ms
  Ý nghĩa / ngữ cảnh sử dụng: delay throttle.
  Lý do cần có data này: giảm spike write.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: config hóa.

### 30. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/MediaUploadCleanupJob.cs`
- **Dòng 131**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: fallback cleanup interval phút.
  Lý do cần có data này: khi config <=0.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 31. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.cs`
- **Dòng 11**: `1` giờ
  Ý nghĩa / ngữ cảnh sử dụng: `ScanInterval` timer escrow.
  Lý do cần có data này: scheduler xử lý timeout/refund/release.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: `EscrowOptions.ScanIntervalMinutes`.

### 32. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/ChatModerationOptions.cs`
- **Dòng 10**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: max queue size mặc định moderation.
  Lý do cần có data này: bảo vệ tài nguyên.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 33. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/ChatModerationQueue.cs`
- **Dòng 25**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: floor capacity queue moderation.
  Lý do cần có data này: tránh cấu hình quá thấp gây drop quá sớm.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants policy.

### 34. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/EconomyConstants.cs`
- **Dòng 7**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: `VndPerDiamond`.
  Lý do cần có data này: tỷ giá nội bộ kinh tế hệ thống.
  Đánh giá: Cần cải thiện (đã tách một phần)
  Đề xuất cải thiện cụ thể: chuyển sang bảng `EconomySettings` có version.

### 35. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/WithdrawalPolicyConstants.cs`
- **Dòng 7**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: rút tối thiểu (diamond).
  Lý do cần có data này: policy chống rút vi mô.
  Đánh giá: Cần cải thiện (đã tách một phần)
  Đề xuất cải thiện cụ thể: đưa vào DB config tuần/tháng.
- **Dòng 10**: `0.10`
  Ý nghĩa / ngữ cảnh sử dụng: phí rút 10%.
  Lý do cần có data này: fee policy tài chính.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB + rule theo tier.
- **Dòng 13**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: max idempotency key length.
  Lý do cần có data này: guard input + index length.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants security/validation.
- **Dòng 16**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: max note length.
  Lý do cần có data này: chặn payload lớn bất thường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 36. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Repositories/WithdrawalRepository.cs`
- **Dòng 66**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: page size mặc định list withdrawal.
  Lý do cần có data này: bảo vệ query.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: expose pagination defaults config.
- **Dòng 66**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: page size tối đa.
  Lý do cần có data này: chống truy vấn quá nặng.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants API policy.

### 37. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderProfileUpdateDomainRules.cs`
- **Dòng 11**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: `MinYearsOfExperience`.
  Lý do cần có data này: quality floor reader profile.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển sang policy table cho onboarding.
- **Dòng 12**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: `MinDiamondPerQuestion`.
  Lý do cần có data này: sàn giá dịch vụ reader.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB pricing policy.

### 38. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reader/Commands/SubmitReaderRequest/SubmitReaderRequestValidator.cs`
- **Dòng 12**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: bio tối thiểu.
  Lý do cần có data này: chất lượng profile.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep hoặc config onboarding.
- **Dòng 13**: `4000`
  Ý nghĩa / ngữ cảnh sử dụng: bio tối đa.
  Lý do cần có data này: giới hạn payload.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants validation.
- **Dòng 14**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: min years.
  Lý do cần có data này: policy đầu vào.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy config.
- **Dòng 15**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: min diamond/question.
  Lý do cần có data này: sàn giá reader.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB policy.

### 39. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reader/Commands/UpdateReaderProfile/UpdateReaderProfileCommandValidator.cs`
- **Dòng 12**: `4000`
  Ý nghĩa / ngữ cảnh sử dụng: max bio update.
  Lý do cần có data này: kiểm soát kích thước dữ liệu.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 13**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: min years update.
  Lý do cần có data này: quality floor.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy config.
- **Dòng 14**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: min diamond/question update.
  Lý do cần có data này: pricing floor.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB policy.

### 40. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderRequestReviewRequestedDomainEventHandler.Validation.cs`
- **Dòng 10**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: min years cho review approve.
  Lý do cần có data này: ràng buộc domain.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy config.
- **Dòng 11**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: min diamond/question khi duyệt.
  Lý do cần có data này: đồng nhất floor pricing.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy DB.

### 41. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReaderProfileDocument.cs`
- **Dòng 131**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: default `DiamondPerQuestion` trong document reader.
  Lý do cần có data này: baseline khi chưa set giá.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đọc từ policy service thay vì literal trong document model.

### 42. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/ChatDtos.cs`
- **Dòng 49**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: default SLA giờ của conversation DTO.
  Lý do cần có data này: fallback hiển thị/logic.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy từ `ChatPolicyOptions.DefaultSlaHours`.

### 43. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ConversationDocument.cs`
- **Dòng 43**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: default SLA trong persistence doc.
  Lý do cần có data này: baseline cho conversation mới.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: inject từ policy khi create entity.

### 44. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/CreateConversation/CreateConversationCommand.cs`
- **Dòng 18**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: SLA mặc định khi client không gửi.
  Lý do cần có data này: DX/API fallback.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đọc từ config thay vì default trong command.

### 45. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/CreateConversation/CreateConversationCommandValidator.cs`
- **Dòng 29**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: min SLA giờ.
  Lý do cần có data này: biên validation input.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants validation.
- **Dòng 29**: `168`
  Ý nghĩa / ngữ cảnh sử dụng: max SLA giờ.
  Lý do cần có data này: tránh SLA bất thường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 46. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/CreateConversation/CreateConversationCommandHandler.Validation.cs`
- **Dòng 21**: `6, 12, 24`
  Ý nghĩa / ngữ cảnh sử dụng: tập SLA được phép.
  Lý do cần có data này: policy SLA cố định cho sản phẩm.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển thành `AllowedSlaHours` trong DB/config.
- **Dòng 55**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: active conversation cap/user.
  Lý do cần có data này: chống quá tải và abuse.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: quota theo tier user.

### 47. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/AcceptConversation/AcceptConversationCommandHandler.Helpers.cs`
- **Dòng 38**: `6 or 12 or 24`, fallback `12`
  Ý nghĩa / ngữ cảnh sử dụng: chuẩn hóa SLA khi accept conversation.
  Lý do cần có data này: giữ SLA trong tập hỗ trợ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng `ChatPolicyOptions` dùng chung toàn module.

### 48. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Inbox.cs`
- **Dòng 35**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: SLA mặc định khi body không có `SlaHours`.
  Lý do cần có data này: fallback API.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: map từ config, không hardcode trong controller.
- **Dòng 51**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: page mặc định list inbox.
  Lý do cần có data này: pagination default.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants API.
- **Dòng 52**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: pageSize mặc định list inbox.
  Lý do cần có data này: cân bằng payload.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep/constants.

### 49. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs`
- **Dòng 28**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: min độ dài reason mở dispute.
  Lý do cần có data này: tránh dispute rỗng.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 31**: `48` giờ
  Ý nghĩa / ngữ cảnh sử dụng: dispute window duration.
  Lý do cần có data này: SLA xử lý tranh chấp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: `EscrowPolicyOptions.DisputeWindowHours`.

### 50. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs`
- **Dòng 22**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: max length idempotency key add-question.
  Lý do cần có data này: input guard.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants validation.
- **Dòng 136**: `24` giờ
  Ý nghĩa / ngữ cảnh sử dụng: `ReaderResponseDueAt`.
  Lý do cần có data này: SLA phản hồi reader.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: `EscrowPolicyOptions.ReaderResponseDueHours`.
- **Dòng 137**: `24` giờ
  Ý nghĩa / ngữ cảnh sử dụng: `AutoRefundAt`.
  Lý do cần có data này: auto-refund deadline.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config hóa.

### 51. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoRefunds.Workflow.cs`
- **Dòng 76**: `24` giờ
  Ý nghĩa / ngữ cảnh sử dụng: `DisputeWindowEnd = now + 24h` sau refund.
  Lý do cần có data này: hậu kiểm khi auto-refund.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy configurable.

### 52. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.ExpiredOffers.Workflow.cs`
- **Dòng 87**: `24` giờ
  Ý nghĩa / ngữ cảnh sử dụng: dispute window hậu refund expired offer.
  Lý do cần có data này: chuẩn hóa hậu xử lý escrow.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng shared escrow policy.

### 53. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Services/EscrowSettlementService.State.cs`
- **Dòng 39**: `24` giờ
  Ý nghĩa / ngữ cảnh sử dụng: dispute window hậu release.
  Lý do cần có data này: khung khiếu nại sau giải ngân.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy option.
- **Dòng 63**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: floor `TotalFrozen` không âm.
  Lý do cần có data này: guard dữ liệu tài chính.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants domain invariant.

### 54. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs`
- **Dòng 19**: `-48` giờ
  Ý nghĩa / ngữ cảnh sử dụng: lấy due dispute auto-resolution sau 48h.
  Lý do cần có data này: SLA timeout tranh chấp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: `EscrowPolicyOptions.DisputeAutoResolveHours`.
- **Dòng 57**: `-48` giờ
  Ý nghĩa / ngữ cảnh sử dụng: kiểm tra item đã quá hạn 48h trước auto-resolve.
  Lý do cần có data này: policy nhất quán theo dispute timeout.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng chung 1 constant/config.

### 55. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs`
- **Dòng 9**: `1, 2, 4, 8, 16`
  Ý nghĩa / ngữ cảnh sử dụng: bậc giá follow-up trả phí.
  Lý do cần có data này: rule economy follow-up.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển sang bảng `FollowupPricingTiers` trong DB.
- **Dòng 12**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: max follow-up/phiên.
  Lý do cần có data này: giới hạn kinh tế và spam.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB policy.
- **Dòng 20**: `0..77`
  Ý nghĩa / ngữ cảnh sử dụng: biên cardId bộ tarot.
  Lý do cần có data này: validate deck index.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ trong constants deck.
- **Dòng 26**: `21`
  Ý nghĩa / ngữ cảnh sử dụng: ngưỡng major arcana.
  Lý do cần có data này: thuật toán mock level.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 33**: `14`
  Ý nghĩa / ngữ cảnh sử dụng: modulo cho minor arcana.
  Lý do cần có data này: phân tầng level minor.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 59**: `16`
  Ý nghĩa / ngữ cảnh sử dụng: threshold 3 free slots.
  Lý do cần có data này: chính sách ưu đãi level cao.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB/feature flag.
- **Dòng 65**: `11`
  Ý nghĩa / ngữ cảnh sử dụng: threshold 2 free slots.
  Lý do cần có data này: rule ưu đãi trung cấp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 71**: `6`
  Ý nghĩa / ngữ cảnh sử dụng: threshold 1 free slot.
  Lý do cần có data này: rule ưu đãi cơ bản.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.

### 56. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryBusinessConstants.cs`
- **Dòng 13**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: spread card count 3.
  Lý do cần có data này: mapping free-draw ticket.
  Đánh giá: Hợp lý (đã tách một phần)
  Đề xuất cải thiện cụ thể: giữ constants domain.
- **Dòng 18**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: spread card count 5.
  Lý do cần có data này: mapping spread.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 23**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: spread card count 10.
  Lý do cần có data này: mapping spread.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 28**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: gold reward khi dùng Lucky Star đã có title.
  Lý do cần có data này: reward fallback economy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển vào `InventoryRewardPolicy` trong DB.

### 57. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.cs`
- **Dòng 17**: `78`
  Ý nghĩa / ngữ cảnh sử dụng: kích thước bộ tarot.
  Lý do cần có data này: shuffle/deck draw logic.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants domain.
- **Dòng 18**: `1m`
  Ý nghĩa / ngữ cảnh sử dụng: EXP/card.
  Lý do cần có data này: progression economy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào progression config/DB.
- **Dòng 124**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: cards draw cho `daily_1`.
  Lý do cần có data này: spread shape.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants spread.
- **Dòng 125**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: cards draw cho `spread_3`.
  Lý do cần có data này: spread shape.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 126**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: cards draw cho `spread_5`.
  Lý do cần có data này: spread shape.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 127**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: cards draw cho `spread_10`.
  Lý do cần có data này: spread shape.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 58. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/FreeDrawCreditRepository.cs`
- **Dòng 56**: `3, 5, 10`
  Ý nghĩa / ngữ cảnh sử dụng: spread card count hợp lệ khi consume.
  Lý do cần có data này: ràng buộc domain free draw.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants domain dùng chung.
- **Dòng 88**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: fallback summary spread3.
  Lý do cần có data này: return default khi chưa có credit.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 89**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: fallback summary spread5.
  Lý do cần có data này: default no-credit.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 90**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: fallback summary spread10.
  Lý do cần có data này: default no-credit.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 59. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/FreeDrawCredit.cs`
- **Dòng 55**: `3 or 5 or 10`
  Ý nghĩa / ngữ cảnh sử dụng: spread card count hợp lệ ở domain entity.
  Lý do cần có data này: invariant domain.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ trong constants domain spread.

### 60. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserCollection.cs`
- **Dòng 13**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: level tối đa card.
  Lý do cần có data này: progression cap.
  Đánh giá: Cần cải thiện (đã tách một phần)
  Đề xuất cải thiện cụ thể: `ProgressionOptions.MaxLevel`.
- **Dòng 18**: `100m`
  Ý nghĩa / ngữ cảnh sử dụng: base EXP lên level đầu.
  Lý do cần có data này: đường cong leveling.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB bảng `CardProgressionCurve`.
- **Dòng 23**: `50m`
  Ý nghĩa / ngữ cảnh sử dụng: EXP tăng thêm mỗi level.
  Lý do cần có data này: progression slope.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB curve.
- **Dòng 28**: `10m`
  Ý nghĩa / ngữ cảnh sử dụng: base ATK mặc định.
  Lý do cần có data này: stat baseline.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: progression config.
- **Dòng 33**: `10m`
  Ý nghĩa / ngữ cảnh sử dụng: base DEF mặc định.
  Lý do cần có data này: stat baseline.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: progression config.
- **Dòng 164**: `(10, newLevel * 10)`
  Ý nghĩa / ngữ cảnh sử dụng: range roll bonus stat khi lên level.
  Lý do cần có data này: độ dao động chỉ số.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển thành bảng roll range theo level band.
- **Dòng 188**: `/100m`
  Ý nghĩa / ngữ cảnh sử dụng: chuyển bonus phần trăm vào công thức tổng stat.
  Lý do cần có data này: quy đổi % buff.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants domain math.

### 61. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 35**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: default level.
  Lý do cần có data này: khởi tạo card mới.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 42**: `0m`
  Ý nghĩa / ngữ cảnh sử dụng: default EXP.
  Lý do cần có data này: trạng thái ban đầu.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 55**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: default ascension tier.
  Lý do cần có data này: baseline progression.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 89**: `0m`
  Ý nghĩa / ngữ cảnh sử dụng: default bonus ATK %.
  Lý do cần có data này: baseline stat bonus.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 96**: `0m`
  Ý nghĩa / ngữ cảnh sử dụng: default bonus DEF %.
  Lý do cần có data này: baseline stat bonus.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 62. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoUserCollectionRepository.cs`
- **Dòng 14**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: max optimistic retries.
  Lý do cần có data này: xử lý concurrent update.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: cấu hình `CollectionOptions.MaxOptimisticRetries`.

### 63. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoUserCollectionRepository.Enhancements.cs`
- **Dòng 14**: `70`
  Ý nghĩa / ngữ cảnh sử dụng: upper bound nhánh 1 roll % stat.
  Lý do cần có data này: xác suất roll booster stat.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển sang bảng probability DB.
- **Dòng 15**: `95`
  Ý nghĩa / ngữ cảnh sử dụng: upper bound nhánh 2 roll % stat.
  Lý do cần có data này: xác suất nhánh hiếm.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB probability table.
- **Dòng 17**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: upper bound nhánh 1 roll EXP.
  Lý do cần có data này: phân phối reward EXP.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB probability.
- **Dòng 18**: `85`
  Ý nghĩa / ngữ cảnh sử dụng: upper bound nhánh 2 roll EXP.
  Lý do cần có data này: phân phối reward EXP.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB probability.
- **Dòng 19**: `95`
  Ý nghĩa / ngữ cảnh sử dụng: upper bound nhánh 3 roll EXP.
  Lý do cần có data này: phân phối reward EXP.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB probability.
- **Dòng 172-188**: `1..25`, `26..55`, `56..80`, `81..100`
  Ý nghĩa / ngữ cảnh sử dụng: các dải roll EXP delta.
  Lý do cần có data này: economy progression.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: bảng `EnhancementRollTable`.
- **Dòng 193-204**: `1..3`, `4..6`, `7..10`
  Ý nghĩa / ngữ cảnh sử dụng: dải roll % ATK/DEF booster.
  Lý do cần có data này: cân bằng booster.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB + version rollout.

### 64. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoUserCollectionRepository.LevelUpgradeEnhancement.cs`
- **Dòng 115**: `0..100`
  Ý nghĩa / ngữ cảnh sử dụng: clamp success rate percent.
  Lý do cần có data này: guard input hợp lệ.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep validation constants.
- **Dòng 116**: `0..10000` và `/100m`
  Ý nghĩa / ngữ cảnh sử dụng: scale roll xác suất 2 chữ số thập phân.
  Lý do cần có data này: precision khi roll success.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 65. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryItemUsedDomainEventHandler.cs`
- **Dòng 18**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: `MinimumEffectValue`.
  Lý do cần có data này: chặn effect <=0.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep constants domain.
- **Dòng 55**: `1..10`
  Ý nghĩa / ngữ cảnh sử dụng: clamp quantity item use/lần.
  Lý do cần có data này: chống abuse request lớn.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep hoặc config theo item type.

### 66. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Migrations/20260416112924_RefactorInventoryTicketsAndFreeDrawBySpread.cs`
- **Dòng 26**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: default `spread_card_count` khi thêm cột.
  Lý do cần có data này: migrate dữ liệu cũ.
  Đánh giá: Hợp lý (migration one-off)
  Đề xuất cải thiện cụ thể: giữ trong migration.
- **Dòng 31**: `(3, 5, 10)`
  Ý nghĩa / ngữ cảnh sử dụng: normalize spread count hợp lệ.
  Lý do cần có data này: data correction migration.
  Đánh giá: Hợp lý (migration one-off)
  Đề xuất cải thiện cụ thể: giữ migration.
- **Dòng 82/146/210**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: `effect_value` cho ticket/booster seed update.
  Lý do cần có data này: baseline effect item.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: sau migration, quản lý effect qua DB catalog.
- **Dòng 83/147/211**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: `success_rate_percent` seed item.
  Lý do cần có data này: tỷ lệ thành công booster.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB catalog + admin tools.

### 67. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.cs`
- **Dòng 17**: `10000`
  Ý nghĩa / ngữ cảnh sử dụng: tổng probability basis points bắt buộc của pool.
  Lý do cần có data này: kiểm tra integrity cấu hình gacha.
  Đánh giá: Hợp lý (đã tách một phần)
  Đề xuất cải thiện cụ thể: giữ constants basis-point chuẩn.

### 68. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gacha/Commands/PullGacha/PullGachaCommand.cs`
- **Dòng 28**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: `Count` mặc định pull.
  Lý do cần có data này: UX default 1 lượt.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 69. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gacha/Commands/PullGacha/PullGachaCommandValidator.cs`
- **Dòng 20**: `64`
  Ý nghĩa / ngữ cảnh sử dụng: max length `PoolCode`.
  Lý do cần có data này: input guard.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 24**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: max length `IdempotencyKey`.
  Lý do cần có data này: anti-abuse validation.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 27**: `1..10`
  Ý nghĩa / ngữ cảnh sử dụng: số lượt pull/lần cho phép.
  Lý do cần có data này: giới hạn chi phí và load.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config theo tier user/event.

### 70. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaHistory/GetGachaHistoryQuery.cs`
- **Dòng 19**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: page mặc định.
  Lý do cần có data này: pagination baseline.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 24**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: page size mặc định.
  Lý do cần có data này: payload size control.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 71. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 34**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: cost amount pool `normal` (gold).
  Lý do cần có data này: giá quay gacha.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: quản lý từ DB bảng `GachaPools` (admin editable).
- **Dòng 37**: `80`
  Ý nghĩa / ngữ cảnh sử dụng: hard pity pool `normal`.
  Lý do cần có data này: xác suất đảm bảo reward.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB/feature flag.
- **Dòng 55**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: cost amount pool `premium` (diamond).
  Lý do cần có data này: economy gacha premium.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 58**: `70`
  Ý nghĩa / ngữ cảnh sử dụng: hard pity pool premium.
  Lý do cần có data này: retention + fairness.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 76**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: cost amount pool special.
  Lý do cần có data này: event economy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 79**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: hard pity pool special.
  Lý do cần có data này: event odds policy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 38/59/80**: `nowUtc.AddDays(-1)`
  Ý nghĩa / ngữ cảnh sử dụng: effective-from mặc định của pool seed.
  Lý do cần có data này: hiệu lực seed ngay lập tức (lùi 1 ngày).
  Đánh giá: Hợp lý (seed)
  Đề xuất cải thiện cụ thể: giữ seed; runtime nên dùng DB values.
- **Dòng 89-96**: `3500, 2300, 1800, 1200, 700, 300, 150, 50`
  Ý nghĩa / ngữ cảnh sử dụng: basis points reward normal pool.
  Lý do cần có data này: xác suất quay gacha.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB odds table versioned.
- **Dòng 89/90**: `100`, `250`
  Ý nghĩa / ngữ cảnh sử dụng: gold amount reward normal pool.
  Lý do cần có data này: economy payout.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 104-111**: `1800,1700,1400,1200,1200,900,800,1000`
  Ý nghĩa / ngữ cảnh sử dụng: basis points reward premium pool.
  Lý do cần có data này: premium odds.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 104/105**: `1000`, `20`
  Ý nghĩa / ngữ cảnh sử dụng: amount currency reward premium pool.
  Lý do cần có data này: payout premium.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 119-125**: `1200,1800,2500,1100,1100,1100,1200`
  Ý nghĩa / ngữ cảnh sử dụng: basis points reward special pool.
  Lý do cần có data này: event odds.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB versioned odds.
- **Dòng 119**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: amount diamond reward special.
  Lý do cần có data này: payout special event.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.

### 72. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs`
- **Dòng 116**: `10000`
  Ý nghĩa / ngữ cảnh sử dụng: validate tổng xác suất pool.
  Lý do cần có data này: integrity odds.
  Đánh giá: Hợp lý (đã tách một phần)
  Đề xuất cải thiện cụ thể: keep.

### 73. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryMysteryPackOpenedDomainEventHandler.cs`
- **Dòng 18-23**: `3000, 2200, 2200, 1200, 900, 500`
  Ý nghĩa / ngữ cảnh sử dụng: trọng số reward mystery pack.
  Lý do cần có data này: xác suất mở pack.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển sang bảng reward weight trong DB.
- **Dòng 18-23**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: quantity mỗi reward.
  Lý do cần có data này: lượng vật phẩm nhận được.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.

### 74. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/SeedGamificationData.cs`
- **Dòng 41**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: target quest `daily_1_reading`.
  Lý do cần có data này: goal quest hàng ngày.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: quản lý quest từ DB admin.
- **Dòng 42**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: reward gold quest `daily_1_reading`.
  Lý do cần có data này: economy reward.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB quest rewards.
- **Dòng 51**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: target quest `daily_checkin`.
  Lý do cần có data này: nhiệm vụ check-in.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 52**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: reward gold quest `daily_checkin`.
  Lý do cần có data này: economy reward.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.

### 75. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 53**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: target `daily_reading_1`.
  Lý do cần có data này: quest threshold.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 54**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: reward gold `daily_reading_1`.
  Lý do cần có data này: reward tuning.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 65**: `7`
  Ý nghĩa / ngữ cảnh sử dụng: target `weekly_reading_7`.
  Lý do cần có data này: weekly goal.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 66**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: reward gold weekly.
  Lý do cần có data này: economy pacing.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 66**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: reward diamond weekly.
  Lý do cần có data này: premium reward.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 77**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: target `daily_post_1`.
  Lý do cần có data này: community engagement quest.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.
- **Dòng 78**: `150`
  Ý nghĩa / ngữ cảnh sử dụng: reward gold `daily_post_1`.
  Lý do cần có data này: incentive community post.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: DB.

### 76. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQuery.cs`
- **Dòng 10**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: `Limit` mặc định top leaderboard.
  Lý do cần có data này: kích thước trả dữ liệu.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep hoặc expose config.

### 77. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/GamificationService.Reading.cs`
- **Dòng 19**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: progress increment cho event ReadingCompleted.
  Lý do cần có data này: quest progress unit.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 21**: `10, 10, 10`
  Ý nghĩa / ngữ cảnh sử dụng: điểm cộng daily/monthly/lifetime leaderboard.
  Lý do cần có data này: pacing leaderboard.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển vào DB score policy.
- **Dòng 63**: `5` phút
  Ý nghĩa / ngữ cảnh sử dụng: cache TTL quest definitions.
  Lý do cần có data này: giảm query lặp.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: đưa vào cache options.

### 78. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/User.Streak.cs`
- **Dòng 49**: `/100.0`
  Ý nghĩa / ngữ cảnh sử dụng: công thức exp multiplier theo streak.
  Lý do cần có data này: tốc độ tăng thưởng theo ngày.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa tham số multiplier slope vào config.
- **Dòng 87**: `/10.0`
  Ý nghĩa / ngữ cảnh sử dụng: công thức giá freeze theo `PreBreakStreak`.
  Lý do cần có data này: chi phí phục hồi streak tăng dần.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: pricing formula config hoặc DB table.

### 79. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Commands/PurchaseFreeze/PurchaseStreakFreezeCommandHandler.cs`
- **Dòng 123**: `today.AddDays(-2)`
  Ý nghĩa / ngữ cảnh sử dụng: fallback ngày trước khi gãy streak.
  Lý do cần có data này: tính break discovery khi thiếu `LastStreakDate`.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: tách thành domain policy constant.
- **Dòng 124**: `AddDays(2)`
  Ý nghĩa / ngữ cảnh sử dụng: ngày phát hiện break streak.
  Lý do cần có data này: rule business của freeze window.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy config.

### 80. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Queries/GetStreakStatus/GetStreakStatusQueryHandler.cs`
- **Dòng 85**: `today.AddDays(-2)`
  Ý nghĩa / ngữ cảnh sử dụng: fallback date trước break khi tính trạng thái.
  Lý do cần có data này: đồng bộ logic với command mua freeze.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng shared streak policy service.
- **Dòng 86**: `AddDays(2)`
  Ý nghĩa / ngữ cảnh sử dụng: xác định break discovery date.
  Lý do cần có data này: điều kiện mở quyền mua freeze.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: shared constants/policy config.

### 81. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/EnvLoader.cs`
- **Dòng 111**: `5432`
  Ý nghĩa / ngữ cảnh sử dụng: fallback port PostgreSQL local.
  Lý do cần có data này: local bootstrap khi thiếu env.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 122**: `tarotweb`
  Ý nghĩa / ngữ cảnh sử dụng: fallback Mongo DB name local.
  Lý do cần có data này: local bootstrap.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 126**: `27017`
  Ý nghĩa / ngữ cảnh sử dụng: local Mongo default port trong connection string.
  Lý do cần có data này: local bootstrap.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 132**: `6379`
  Ý nghĩa / ngữ cảnh sử dụng: fallback Redis port local.
  Lý do cần có data này: local bootstrap.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

### 82. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositOrderCreateRequestedDomainEventHandler.cs`
- **Dòng 173**: `0, 1000`
  Ý nghĩa / ngữ cảnh sử dụng: random suffix range cho `orderCode` (`GetInt32(0,1000)`).
  Lý do cần có data này: giảm va chạm mã đơn khi cùng millisecond.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa range vào config hoặc dùng sequence/ULID để tránh phụ thuộc random range.

### 83. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/RefreshToken.cs`
- **Dòng 146**: `64`
  Ý nghĩa / ngữ cảnh sử dụng: max length lưu `CreatedByIp` qua `Normalize`.
  Lý do cần có data này: giới hạn cột/audit field.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants validation.
- **Dòng 147**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: max length `CreatedDeviceId`.
  Lý do cần có data này: chống dữ liệu bất thường dài quá mức.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 148**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: max length `CreatedUserAgentHash`.
  Lý do cần có data này: chuẩn hóa dữ liệu audit.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.
- **Dòng 194**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: max length `LastRotateIdempotencyKey`.
  Lý do cần có data này: guard input idempotency.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: keep.

## Ghi chú lọc (đã loại bỏ)
- Đã loại khỏi danh sách toàn bộ literal kiểu: tên cột DB (`HasColumnName(...)`), tên cookie/header/key kỹ thuật (`AuthCookieNames`, `AuthHeaders`), route/path string, message text, enum code string.
- Các literal migration/designer auto-generated không mang ý nghĩa tuning runtime đã không liệt kê (trừ các giá trị seed/policy kinh doanh rõ ràng như reward/probability/effect).
