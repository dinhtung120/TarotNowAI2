# DANH SÁCH TOÀN BỘ HARD-CODED VALUES TRONG BACKEND

## Tổng quan
- Số lượng hard-coded values tìm thấy: 711
- Số file bị ảnh hưởng: 259
- Khuyến nghị: Nên chuyển khoảng 311 giá trị vào Config/AppSettings, 286 giá trị vào Database (bảng cấu hình nghiệp vụ), và 114 giá trị gom về Constants class/policy dùng chung.

## Danh sách chi tiết

### 1. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Constants/AuthCookieNames.cs`
- **Dòng 11**: `"accessToken"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string AccessToken = "accessToken";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 2. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Constants/AuthCookieNames.cs`
- **Dòng 16**: `"refreshToken"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string RefreshToken = "refreshToken";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 3. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Constants/AuthHeaders.cs`
- **Dòng 11**: `"x-idempotency-key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string IdempotencyKey = "x-idempotency-key";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 4. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Constants/FeatureFlags.cs`
- **Dòng 16**: `"ChatKeywordModerationEnabled"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string ChatKeywordModerationEnabled = "ChatKeywordModerationEnabled";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 5. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AdminDepositsController.cs`
- **Dòng 12**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 6. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AdminGamificationController.cs`
- **Dòng 16**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 7. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AdminOutboxController.cs`
- **Dòng 13**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 8. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AdminReaderRequestsController.cs`
- **Dòng 14**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 9. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AdminUsersController.cs`
- **Dòng 12**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 10. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AdminUsersController.cs`
- **Dòng 85**: `"X-Idempotency-Key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var headerKey = Request.Headers["X-Idempotency-Key"].ToString();`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 11. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AdminUsersController.cs`
- **Dòng 112**: `"X-Idempotency-Key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `command.IdempotencyKey = Request.Headers["X-Idempotency-Key"].ToString();`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 12. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AdminWithdrawalsController.cs`
- **Dòng 16**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 13. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/AuthSessionController.cs`
- **Dòng 58**: `"auth-refresh-token-family"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-refresh-token-family")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 14. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/CheckInController.cs`
- **Dòng 17**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 15. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Acceptance.cs`
- **Dòng 21**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 16. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Acceptance.cs`
- **Dòng 49**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 17. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Completion.cs`
- **Dòng 19**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 18. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Completion.cs`
- **Dòng 45**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 19. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Finance.cs`
- **Dòng 23**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 20. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Finance.cs`
- **Dòng 52**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 21. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Finance.cs`
- **Dòng 81**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 22. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Inbox.cs`
- **Dòng 35**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `SlaHours = body.SlaHours ?? 12`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 23. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.MediaUpload.cs`
- **Dòng 15**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 24. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Messages.cs`
- **Dòng 25**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[FromQuery] int limit = 50)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 25. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.Messages.cs`
- **Dòng 52**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 26. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ConversationController.cs`
- **Dòng 13**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 27. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/DepositController.Orders.cs`
- **Dòng 15**: `"Idempotency-Key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string IdempotencyHeaderName = "Idempotency-Key";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 28. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/DepositController.cs`
- **Dòng 10**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 29. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/GachaController.cs`
- **Dòng 23**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 30. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/GamificationController.cs`
- **Dòng 16**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 31. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/GamificationController.cs`
- **Dòng 44**: `"daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public async Task<IActionResult> GetActiveQuests([FromQuery] string type = "daily")`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 32. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/GamificationController.cs`
- **Dòng 110**: `"daily_rank_score"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public async Task<IActionResult> GetLeaderboard([FromQuery] string track = "daily_rank_score", [FromQuery] string? periodKey = null)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 33. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/InventoryController.cs`
- **Dòng 21**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 34. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/InventoryController.cs`
- **Dòng 126**: `"idempotencyKey"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[System.Text.Json.Serialization.JsonPropertyName("idempotencyKey")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 35. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/MfaController.cs`
- **Dòng 23**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 36. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ProfileController.cs`
- **Dòng 20**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 37. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ReaderController.Directory.cs`
- **Dòng 18**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 38. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ReaderController.Directory.cs`
- **Dòng 42**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 39. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ReaderController.ReaderFlow.cs`
- **Dòng 24**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 40. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ReaderController.ReaderFlow.cs`
- **Dòng 61**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 41. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ReaderController.ReaderFlow.cs`
- **Dòng 82**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 42. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ReaderController.ReaderFlow.cs`
- **Dòng 122**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 43. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/ReaderController.cs`
- **Dòng 14**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 44. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/WalletController.cs`
- **Dòng 19**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 45. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/WalletController.cs`
- **Dòng 62**: `1, 20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public async Task<IActionResult> GetLedger([FromQuery] int page = 1, [FromQuery] int limit = 20)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 46. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Controllers/WithdrawalController.cs`
- **Dòng 20**: `"auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[EnableRateLimiting("auth-session")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 47. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler.Database.cs`
- **Dòng 53**: `"ix_chat_question_items_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ix_chat_question_items_idempotency_key",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 48. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Realtime/RedisUserPresenceTracker.cs`
- **Dòng 13**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private static readonly TimeSpan OnlineWindow = TimeSpan.FromMinutes(15);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 49. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Services/PresenceTimeoutBackgroundService.cs`
- **Dòng 18**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private readonly TimeSpan _timeoutPeriod = TimeSpan.FromMinutes(15);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 50. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.Platform.cs`
- **Dòng 95**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `options.ForwardLimit = 2;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 51. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.Platform.cs`
- **Dòng 147**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (parts.Length != 2)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 52. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.Platform.cs`
- **Dòng 174**: `10, 1024`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `options.MaximumReceiveMessageSize = 10 * 1024 * 1024;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 53. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 36**: `5, 60, "auth-login"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AddFixedWindowPolicy(options, "auth-login", ResolveClientIp, permitLimit: 5, TimeSpan.FromSeconds(60));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 54. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 39**: `100, 1, "auth-session"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AddFixedWindowPolicy(options, "auth-session", ResolveAuthenticatedPartitionKey, permitLimit: 100, TimeSpan.FromMinutes(1));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 55. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 40**: `30, 1, "auth-refresh"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AddFixedWindowPolicy(options, "auth-refresh", ResolveRefreshPartitionKey, permitLimit: 30, TimeSpan.FromMinutes(1));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 56. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 41**: `10, 1, "auth-refresh-token-family"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AddFixedWindowPolicy(options, "auth-refresh-token-family", ResolveRefreshFamilyKey, permitLimit: 10, TimeSpan.FromMinutes(1));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 57. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 42**: `30, 1, "auth-logout"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AddFixedWindowPolicy(options, "auth-logout", ResolveRefreshPartitionKey, permitLimit: 30, TimeSpan.FromMinutes(1));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 58. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 45**: `60, 1, "community-write"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AddFixedWindowPolicy(options, "community-write", ResolveAuthenticatedPartitionKey, permitLimit: 60, TimeSpan.FromMinutes(1));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 59. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 47**: `200, 1, "chat-standard"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AddFixedWindowPolicy(options, "chat-standard", ResolveAuthenticatedPartitionKey, permitLimit: 200, TimeSpan.FromMinutes(1));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 60. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 70**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `QueueLimit = 0,`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 61. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 84**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 62. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 167**: `"refresh-family:{refreshTokenPartition}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return $"refresh-family:{refreshTokenPartition}";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 63. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs`
- **Dòng 220**: `8`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var max = Math.Clamp(length, 8, hash.Length);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 64. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/DatabaseSchemaGuardExtensions.cs`
- **Dòng 12**: `"refresh_tokens", "auth_sessions"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private static readonly string[] RequiredTables = ["users", "refresh_tokens", "auth_sessions"];`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 65. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/EnvLoader.cs`
- **Dòng 48**: `"JWT_EXPIRYMINUTES", "JWT__EXPIRYMINUTES"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CopyEnvIfTargetMissing("JWT_EXPIRYMINUTES", "JWT__EXPIRYMINUTES");`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 66. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/EnvLoader.cs`
- **Dòng 49**: `"JWT_REFRESHEXPIRYDAYS", "JWT__REFRESHEXPIRYDAYS"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CopyEnvIfTargetMissing("JWT_REFRESHEXPIRYDAYS", "JWT__REFRESHEXPIRYDAYS");`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 67. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/EnvLoader.cs`
- **Dòng 56**: `"DEPOSIT__RETURNURL"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CopyEnvIfTargetMissing("PAYOS_RETURN_URL", "DEPOSIT__RETURNURL");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 68. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/EnvLoader.cs`
- **Dòng 57**: `"DEPOSIT__CANCELURL"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CopyEnvIfTargetMissing("PAYOS_CANCEL_URL", "DEPOSIT__CANCELURL");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 69. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/EnvLoader.cs`
- **Dòng 58**: `"DEPOSIT_LINK_EXPIRY_MINUTES", "DEPOSIT__LINKEXPIRYMINUTES"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CopyEnvIfTargetMissing("DEPOSIT_LINK_EXPIRY_MINUTES", "DEPOSIT__LINKEXPIRYMINUTES");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 70. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/EnvLoader.cs`
- **Dòng 72**: `"AI_TIMEOUT_SECONDS", "AIPROVIDER__TIMEOUTSECONDS"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CopyEnvIfTargetMissing("AI_TIMEOUT_SECONDS", "AIPROVIDER__TIMEOUTSECONDS");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 71. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 11**: `10, "ExpiryMinutes"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ExpiryMinutes": 10,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 72. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 12**: `30, "RefreshExpiryDays"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"RefreshExpiryDays": 30`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 73. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 15**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"RefreshLockSeconds": 15,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 74. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 16**: `60, "RefreshIdempotencyWindowSeconds"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"RefreshIdempotencyWindowSeconds": 60,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 75. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 17**: `1200, "AccessTokenBlacklistTtlSeconds"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"AccessTokenBlacklistTtlSeconds": 1200,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 76. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 18**: `1800, "SessionRevocationTtlSeconds"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"SessionRevocationTtlSeconds": 1800,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 77. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 19**: `2592000, "SessionCacheTtlSeconds"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"SessionCacheTtlSeconds": 2592000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 78. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 20**: `86400, "ReplaySecurityRecordTtlSeconds"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ReplaySecurityRecordTtlSeconds": 86400,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 79. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 21**: `200, "CleanupBatchSize"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"CleanupBatchSize": 200,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 80. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 22**: `30, "CleanupIntervalMinutes"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"CleanupIntervalMinutes": 30,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 81. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 23**: `30, "RefreshTokenRetentionDays"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"RefreshTokenRetentionDays": 30,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 82. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 24**: `30, "RevokedSessionRetentionDays"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"RevokedSessionRetentionDays": 30,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 83. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 39**: `"Deposit"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Deposit": {`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 84. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 42**: `15, "LinkExpiryMinutes"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"LinkExpiryMinutes": 15,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 85. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 43**: `"Packages"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Packages": [`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 86. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 45**: `"topup_50k"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Code": "topup_50k",`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 87. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 46**: `50000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"AmountVnd": 50000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 88. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 47**: `500, "BaseDiamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"BaseDiamond": 500,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 89. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 51**: `"topup_100k"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Code": "topup_100k",`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 90. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 52**: `100000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"AmountVnd": 100000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 91. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 53**: `1000, "BaseDiamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"BaseDiamond": 1000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 92. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 57**: `"topup_200k"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Code": "topup_200k",`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 93. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 58**: `200000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"AmountVnd": 200000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 94. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 59**: `2000, "BaseDiamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"BaseDiamond": 2000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 95. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 63**: `"topup_500k"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Code": "topup_500k",`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 96. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 64**: `500000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"AmountVnd": 500000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 97. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 65**: `5000, "BaseDiamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"BaseDiamond": 5000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 98. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 69**: `"topup_1m"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Code": "topup_1m",`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 99. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 70**: `1000000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"AmountVnd": 1000000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 100. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 71**: `10000, "BaseDiamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"BaseDiamond": 10000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 101. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 84**: `19456`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"MemoryKB": 19456,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 102. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 85**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Iterations": 2,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 103. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 90**: `587`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Port": 587,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 104. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 100**: `30, "TimeoutSeconds"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"TimeoutSeconds": 30,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 105. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 101**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"MaxRetries": 2`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 106. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 105**: `"Default"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"Default": "Information",`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 107. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 130**: `"ChatKeywordModerationEnabled"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ChatKeywordModerationEnabled": true`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 108. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 134**: `2000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"MaxQueueSize": 2000,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 109. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 153**: `100, "DailyAiQuota"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"DailyAiQuota": 100,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 110. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 154**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"InFlightAiCap": 10,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 111. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 155**: `2, "ReadingRateLimitSeconds"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ReadingRateLimitSeconds": 2`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 112. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 167**: `10485760`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"MaxUploadBytes": 10485760,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 113. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 168**: `10, "PresignExpiryMinutes"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"PresignExpiryMinutes": 10,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 114. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 169**: `24, "CommunityOrphanTtlHours"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"CommunityOrphanTtlHours": 24,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 115. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 170**: `200, "CleanupBatchSize"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"CleanupBatchSize": 200,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 116. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/appsettings.json`
- **Dòng 171**: `10, "CleanupIntervalMinutes"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"CleanupIntervalMinutes": 10,`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 117. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/ChatDtos.cs`
- **Dòng 49**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int SlaHours { get; set; } = 12;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 118. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthCacheKeys.cs`
- **Dòng 8**: `"auth:cleanup:lock"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string AuthCleanupLockKey = "auth:cleanup:lock";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 119. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthCacheKeys.cs`
- **Dòng 10**: `"auth:session:{sessionId}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static string BuildSessionSnapshotKey(Guid sessionId) => $"auth:session:{sessionId}";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 120. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthCacheKeys.cs`
- **Dòng 12**: `"auth:session-revoked:{sessionId}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static string BuildSessionRevokedKey(Guid sessionId) => $"auth:session-revoked:{sessionId}";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 121. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthCacheKeys.cs`
- **Dòng 16**: `"auth:user-sessions:{userId}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static string BuildUserSessionsIndexKey(Guid userId) => $"auth:user-sessions:{userId}";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 122. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthCacheKeys.cs`
- **Dòng 18**: `"auth:security:replay:{sessionId}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static string BuildReplaySecurityKey(Guid sessionId) => $"auth:security:replay:{sessionId}";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 123. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthCacheKeys.cs`
- **Dòng 20**: `"auth:refresh-lock:{tokenHash}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static string BuildRefreshLockKey(string tokenHash) => $"auth:refresh-lock:{tokenHash}";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 124. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthCacheKeys.cs`
- **Dòng 25**: `"auth:refresh-idem:{sessionPart}:{idempotencyKey}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return $"auth:refresh-idem:{sessionPart}:{idempotencyKey}";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 125. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthCacheKeys.cs`
- **Dòng 29**: `"auth:refresh-idem-token:{tokenHash}:{idempotencyKey}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> $"auth:refresh-idem-token:{tokenHash}:{idempotencyKey}";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 126. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthErrorCodes.cs`
- **Dòng 16**: `"AUTH_TOKEN_EXPIRED"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string TokenExpired = "AUTH_TOKEN_EXPIRED";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 127. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthErrorCodes.cs`
- **Dòng 21**: `"AUTH_TOKEN_REPLAY"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string TokenReplay = "AUTH_TOKEN_REPLAY";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 128. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthErrorCodes.cs`
- **Dòng 31**: `"AUTH_RATE_LIMITED"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string RateLimited = "AUTH_RATE_LIMITED";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 129. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthSecurityPolicyConstants.cs`
- **Dòng 11**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static readonly TimeSpan LoginFailureWindow = TimeSpan.FromMinutes(15);`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 130. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthSecurityPolicyConstants.cs`
- **Dòng 16**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const int LoginIdentityFailureLimit = 10;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 131. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/AuthSecurityPolicyConstants.cs`
- **Dòng 21**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const int LoginIpFailureLimit = 20;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 132. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/EconomyConstants.cs`
- **Dòng 7**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const long VndPerDiamond = 100;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 133. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/GachaErrorCodes.cs`
- **Dòng 11**: `"GACHA_POOL_NOT_FOUND"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string PoolNotFound = "GACHA_POOL_NOT_FOUND";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 134. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/GachaErrorCodes.cs`
- **Dòng 16**: `"GACHA_INVALID_POOL_CONFIGURATION"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string InvalidPoolConfiguration = "GACHA_INVALID_POOL_CONFIGURATION";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 135. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/GachaErrorCodes.cs`
- **Dòng 21**: `"GACHA_INVALID_IDEMPOTENCY_KEY"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string InvalidIdempotencyKey = "GACHA_INVALID_IDEMPOTENCY_KEY";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 136. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/GachaErrorCodes.cs`
- **Dòng 26**: `"GACHA_INSUFFICIENT_BALANCE"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string InsufficientBalance = "GACHA_INSUFFICIENT_BALANCE";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 137. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/GachaErrorCodes.cs`
- **Dòng 31**: `"GACHA_REWARD_RESOLUTION_FAILED"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string RewardResolutionFailed = "GACHA_REWARD_RESOLUTION_FAILED";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 138. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/GachaNotificationTypes.cs`
- **Dòng 11**: `"gacha_item_granted"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string ItemGranted = "gacha_item_granted";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 139. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryBusinessConstants.cs`
- **Dòng 13**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const int SpreadCardCount3 = 3;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 140. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryBusinessConstants.cs`
- **Dòng 18**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const int SpreadCardCount5 = 5;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 141. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryBusinessConstants.cs`
- **Dòng 23**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const int SpreadCardCount10 = 10;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 142. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryBusinessConstants.cs`
- **Dòng 28**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const long LuckyStarOwnedTitleGoldReward = 500;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 143. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryItemCodes.cs`
- **Dòng 11**: `"exp_booster"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string ExpBooster = "exp_booster";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 144. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryItemCodes.cs`
- **Dòng 21**: `"defense_booster"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string DefenseBooster = "defense_booster";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 145. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryItemCodes.cs`
- **Dòng 26**: `"free_draw_ticket_3"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FreeDrawTicket3 = "free_draw_ticket_3";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 146. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryItemCodes.cs`
- **Dòng 31**: `"free_draw_ticket_5"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FreeDrawTicket5 = "free_draw_ticket_5";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 147. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryItemCodes.cs`
- **Dòng 36**: `"free_draw_ticket_10"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FreeDrawTicket10 = "free_draw_ticket_10";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 148. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/InventoryNotificationConstants.cs`
- **Dòng 11**: `"inventory.free_draw_granted"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FreeDrawGranted = "inventory.free_draw_granted";`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 149. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/WithdrawalPolicyConstants.cs`
- **Dòng 7**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const long MinimumWithdrawDiamond = 500;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 150. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/WithdrawalPolicyConstants.cs`
- **Dòng 10**: `0.10m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const decimal FeeRate = 0.10m;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 151. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/WithdrawalPolicyConstants.cs`
- **Dòng 13**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const int IdempotencyKeyMaxLength = 128;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 152. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Constants/WithdrawalPolicyConstants.cs`
- **Dòng 16**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const int NoteMaxLength = 1000;`
  Lý do cần có data này: Giá trị đã được gom vào Constants/Enum (đã tách một phần) để tránh magic values rải rác.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Duy trì Constants cho invariant; với business rule thay đổi theo thời gian thì chuyển sang Config/DB.

### 153. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/MediaUpload/MediaUploadConstants.cs`
- **Dòng 9**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static readonly TimeSpan PresignExpiry = TimeSpan.FromMinutes(10);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 154. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/MediaUpload/MediaUploadConstants.cs`
- **Dòng 12**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static readonly TimeSpan CommunityOrphanTtl = TimeSpan.FromHours(24);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 155. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/MediaUpload/MediaUploadConstants.cs`
- **Dòng 15**: `10, 1024`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const long MaxImageUploadBytes = 10 * 1024 * 1024;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 156. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/MediaUpload/MediaUploadConstants.cs`
- **Dòng 18**: `5, 1024`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const long MaxVoiceUploadBytes = 5 * 1024 * 1024;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 157. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Realtime/RealtimeChannelNames.cs`
- **Dòng 26**: `"realtime:gacha"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Gacha = "realtime:gacha";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 158. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Realtime/RealtimeEventNames.cs`
- **Dòng 51**: `"gacha.result"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string GachaResult = "gacha.result";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 159. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs`
- **Dòng 9**: `1, 2, 4, 8, 16`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private static readonly int[] PriceTiers = [1, 2, 4, 8, 16];`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 160. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs`
- **Dòng 12**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const int MaxFollowupsAllowed = 5;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 161. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Common/Services/FollowupPricingService.cs`
- **Dòng 109**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `paidTierIndex = PriceTiers.Length - 1;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 162. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/RefreshTokenReplayDetectedDomainEventHandler.cs`
- **Dòng 35**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `_accessBlacklistTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.AccessTokenBlacklistTtlSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 163. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/RefreshTokenReplayDetectedDomainEventHandler.cs`
- **Dòng 36**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `_sessionRevokedTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.SessionRevocationTtlSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 164. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/RefreshTokenReplayDetectedDomainEventHandler.cs`
- **Dòng 37**: `300`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `_replayRecordTtl = TimeSpan.FromSeconds(Math.Max(300, authSecuritySettings.ReplaySecurityRecordTtlSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 165. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/RefreshTokenReplayDetectedDomainEventHandler.cs`
- **Dòng 112**: `"auth.session_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `activity?.SetTag("auth.session_id", domainEvent.SessionId);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 166. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/TokenRefreshedDomainEventHandler.cs`
- **Dòng 32**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `_sessionCacheTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.SessionCacheTtlSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 167. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/TokenRefreshedDomainEventHandler.cs`
- **Dòng 44**: `"auth.session_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `activity?.SetTag("auth.session_id", domainEvent.SessionId);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 168. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/TokenRefreshedDomainEventHandler.cs`
- **Dòng 45**: `"auth.new_token_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `activity?.SetTag("auth.new_token_id", domainEvent.NewTokenId);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 169. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/UserLoggedInDomainEventHandler.cs`
- **Dòng 32**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `_sessionCacheTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.SessionCacheTtlSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 170. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/UserLoggedInDomainEventHandler.cs`
- **Dòng 44**: `"auth.session_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `activity?.SetTag("auth.session_id", domainEvent.SessionId);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 171. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/UserLoggedOutDomainEventHandler.cs`
- **Dòng 33**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `_accessBlacklistTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.AccessTokenBlacklistTtlSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 172. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/UserLoggedOutDomainEventHandler.cs`
- **Dòng 34**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `_sessionRevokedTtl = TimeSpan.FromSeconds(Math.Max(60, authSecuritySettings.SessionRevocationTtlSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 173. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/UserLoggedOutDomainEventHandler.cs`
- **Dòng 47**: `"auth.session_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `activity?.SetTag("auth.session_id", domainEvent.SessionId);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 174. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositOrderCreateRequestedDomainEventHandler.cs`
- **Dòng 167**: `"deposit_{userId:N}_{hashPrefix}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return $"deposit_{userId:N}_{hashPrefix}";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 175. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentSucceededDomainEventHandler.cs`
- **Dòng 16**: `"DepositOrder"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string DepositReferenceSource = "DepositOrder";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 176. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentSucceededDomainEventHandler.cs`
- **Dòng 60**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (domainEvent.DiamondAmount <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 177. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentSucceededDomainEventHandler.cs`
- **Dòng 97**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (domainEvent.BonusGoldAmount <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 178. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentSucceededDomainEventHandler.cs`
- **Dòng 138**: `"deposit:{orderId}:diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> $"deposit:{orderId}:diamond";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 179. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/DepositPaymentSucceededDomainEventHandler.cs`
- **Dòng 141**: `"deposit:{orderId}:bonus_gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> $"deposit:{orderId}:bonus_gold";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 180. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.ReplayAndFinalize.cs`
- **Dòng 42**: `"gacha_pull_debit_{context.DomainEvent.IdempotencyKey}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"gacha_pull_debit_{context.DomainEvent.IdempotencyKey}",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 181. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.Rewards.cs`
- **Dòng 21**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var pityCountAtRoll = checked(context.UserPity.PullCount + 1);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 182. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.Rewards.cs`
- **Dòng 76**: `"gacha_pull_reward_{currency}_{context.Operation.Id}_{roll.SelectedRate.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"gacha_pull_reward_{currency}_{context.Operation.Id}_{roll.SelectedRate.Id}",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 183. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.Selection.cs`
- **Dòng 17**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `&& pool.HardPityCount > 0`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 184. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.Selection.cs`
- **Dòng 18**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `&& currentPityCount >= pool.HardPityCount - 1;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 185. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.Selection.cs`
- **Dòng 27**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (candidateRates.Count == 0 && isPityForced)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 186. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.Selection.cs`
- **Dòng 36**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return new PullRollSelection(candidateRates[0], isPityForced, null);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 187. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/GachaPulledDomainEventHandler.cs`
- **Dòng 17**: `10000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int MinimumProbabilityBasisPoints = 10000;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 188. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryMysteryPackOpenedDomainEventHandler.cs`
- **Dòng 18**: `1, 3000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65011"), InventoryItemCodes.ExpBooster, 1, 3000),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 189. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryMysteryPackOpenedDomainEventHandler.cs`
- **Dòng 19**: `1, 2200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65022"), InventoryItemCodes.PowerBooster, 1, 2200),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 190. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryMysteryPackOpenedDomainEventHandler.cs`
- **Dòng 20**: `1, 2200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65033"), InventoryItemCodes.DefenseBooster, 1, 2200),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 191. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryMysteryPackOpenedDomainEventHandler.cs`
- **Dòng 21**: `1, 1200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65055"), InventoryItemCodes.FreeDrawTicket3, 1, 1200),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 192. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryMysteryPackOpenedDomainEventHandler.cs`
- **Dòng 22**: `1, 900`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65066"), InventoryItemCodes.FreeDrawTicket5, 1, 900),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 193. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/InventoryMysteryPackOpenedDomainEventHandler.cs`
- **Dòng 23**: `1, 500`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65077"), InventoryItemCodes.FreeDrawTicket10, 1, 500),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 194. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/MoneyChangedSpendingLeaderboardDomainEventHandler.cs`
- **Dòng 47**: `"daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var dailyKey = PeriodKeyHelper.GetPeriodKey("daily");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 195. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/MoneyChangedSpendingLeaderboardDomainEventHandler.cs`
- **Dòng 75**: `"spent_gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CurrencyType.Gold => "spent_gold",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 196. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/MoneyChangedSpendingLeaderboardDomainEventHandler.cs`
- **Dòng 76**: `"spent_diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CurrencyType.Diamond => "spent_diamond",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 197. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderProfileUpdateDomainRules.cs`
- **Dòng 12**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const long MinDiamondPerQuestion = 50;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 198. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReaderRequestReviewRequestedDomainEventHandler.Validation.cs`
- **Dòng 11**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const long MinDiamondPerQuestion = 50;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 199. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionInitRequestedDomainEventHandler.cs`
- **Dòng 47**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var amountCharged = costGold > 0 ? costGold : costDiamond;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 200. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionInitRequestedDomainEventHandler.cs`
- **Dòng 119**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `? (0, diamondPrice)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 201. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionInitRequestedDomainEventHandler.cs`
- **Dòng 120**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `: (goldPrice, 0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 202. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.BillingAndExp.cs`
- **Dòng 16**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var amount = Math.Max(session.AmountCharged, 0);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 203. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.BillingAndExp.cs`
- **Dòng 67**: `"reading_reveal_charge_{session.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"reading_reveal_charge_{session.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 204. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.BillingAndExp.cs`
- **Dòng 140**: `2m, 1m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return session.SpreadType != SpreadType.Daily1Card && usesDiamond ? 2m : 1m;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 205. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.cs`
- **Dòng 124**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `SpreadType.Daily1Card => 1,`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 206. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.cs`
- **Dòng 125**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `SpreadType.Spread3Cards => 3,`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 207. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.cs`
- **Dòng 126**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `SpreadType.Spread5Cards => 5,`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 208. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/ReadingSessionRevealRequestedDomainEventHandler.cs`
- **Dòng 127**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `SpreadType.Spread10Cards => 10,`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 209. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/WithdrawalCreateRequestedDomainEventHandler.Helpers.cs`
- **Dòng 112**: `"{userId:N}:{normalizedIdempotencyKey}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var source = $"{userId:N}:{normalizedIdempotencyKey}";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 210. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ProcessDeposit/ProcessDepositCommandValidator.cs`
- **Dòng 31**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 211. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Payouts.cs`
- **Dòng 19**: `0.10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 212. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Payouts.cs`
- **Dòng 30**: `"settle_release_{item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_release_{item.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 213. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Payouts.cs`
- **Dòng 45**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (fee > 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 214. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Payouts.cs`
- **Dòng 51**: `"platform_fee"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `referenceSource: "platform_fee",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 215. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Payouts.cs`
- **Dòng 55**: `"settle_fee_{item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_fee_{item.Id}",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 216. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Payouts.cs`
- **Dòng 81**: `"settle_refund_{item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_refund_{item.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 217. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.ReaderPolicy.cs`
- **Dòng 22**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 218. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.ReaderPolicy.cs`
- **Dòng 34**: `-7`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var fromUtc = DateTime.UtcNow.AddDays(-7);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 219. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.ReaderPolicy.cs`
- **Dòng 36**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (recentDisputes <= 3)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 220. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.Transfers.cs`
- **Dòng 30**: `"settle_release_{context.Item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_release_{context.Item.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 221. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.Transfers.cs`
- **Dòng 54**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (context.Split.Fee <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 222. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.Transfers.cs`
- **Dòng 63**: `"platform_fee"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `referenceSource: "platform_fee",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 223. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.Transfers.cs`
- **Dòng 67**: `"settle_fee_{context.Item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_fee_{context.Item.Id}",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 224. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.Transfers.cs`
- **Dòng 92**: `"settle_refund_{context.Item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_refund_{context.Item.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 225. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.cs`
- **Dòng 50**: `100.0m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var readerGross = (long)Math.Floor(amountDiamond * splitPercentToReader / 100.0m);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 226. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.cs`
- **Dòng 53**: `0, 0.10m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var fee = readerGross <= 0 ? 0 : (long)Math.Ceiling(readerGross * 0.10m);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 227. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Commands/ResolveDispute/ResolveDisputeCommandHandler.Settlement.Split.cs`
- **Dòng 55**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var readerNet = Math.Max(0, readerGross - fee);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 228. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Queries/ListDeposits/ListDepositsQuery.cs`
- **Dòng 13**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int PageSize { get; set; } = 20;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 229. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Queries/ListDeposits/ListDepositsQuery.cs`
- **Dòng 23**: `"deposits"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[JsonPropertyName("deposits")]`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 230. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Queries/ListDeposits/ListDepositsQuery.cs`
- **Dòng 51**: `"diamondAmount"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[JsonPropertyName("diamondAmount")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 231. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Queries/ListReaderRequests/ListReaderRequestsQuery.cs`
- **Dòng 13**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int PageSize { get; set; } = 20;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 232. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Queries/ListUsers/ListUsersQuery.cs`
- **Dòng 52**: `"exp"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[JsonPropertyName("exp")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 233. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Queries/ListUsers/ListUsersQuery.cs`
- **Dòng 56**: `"goldBalance"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[JsonPropertyName("goldBalance")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 234. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Admin/Queries/ListUsers/ListUsersQuery.cs`
- **Dòng 60**: `"diamondBalance"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[JsonPropertyName("diamondBalance")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 235. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/ForgotPassword/ForgotPasswordCommandHandler.cs`
- **Dòng 52**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `expiryMinutes: 15);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 236. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/Login/LoginCommandHandler.Helpers.cs`
- **Dòng 177**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `ExpiresInSeconds = _jwtTokenSettings.AccessTokenExpiryMinutes * 60,`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 237. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandHandler.Helpers.cs`
- **Dòng 60**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `ExpiresInSeconds = _jwtTokenSettings.AccessTokenExpiryMinutes * 60,`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 238. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandHandler.cs`
- **Dòng 16**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int MaxRotateLockRetries = 3;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 239. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandHandler.cs`
- **Dòng 92**: `40`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await Task.Delay(TimeSpan.FromMilliseconds(40 * attempt), cancellationToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 240. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandValidator.cs`
- **Dòng 21**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 241. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandValidator.cs`
- **Dòng 25**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 242. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandValidator.cs`
- **Dòng 29**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 243. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandValidator.cs`
- **Dòng 33**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 244. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RevokeToken/RevokeTokenCommandValidator.cs`
- **Dòng 30**: `1024`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(1024)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 245. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/SendEmailVerificationOtp/SendEmailVerificationOtpCommandHandler.cs`
- **Dòng 60**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `expiryMinutes: 15);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 246. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/VerifyEmail/VerifyEmailCommandHandler.cs`
- **Dòng 58**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `user.Wallet.Credit(CurrencyType.Gold, 5, TransactionType.RegisterBonus);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 247. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/AcceptConversation/AcceptConversationCommandHandler.Helpers.cs`
- **Dòng 38**: `6, 12, 24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return configuredSlaHours is 6 or 12 or 24 ? configuredSlaHours : 12;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 248. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/CreateConversation/CreateConversationCommand.cs`
- **Dòng 18**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int SlaHours { get; set; } = 12;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 249. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/CreateConversation/CreateConversationCommandHandler.Validation.cs`
- **Dòng 21**: `6, 12, 24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (request.SlaHours is not (6 or 12 or 24))`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 250. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RejectConversation/RejectConversationCommandHandler.Refunds.cs`
- **Dòng 40**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `session.TotalFrozen = Math.Max(0, session.TotalFrozen - refundedAmount);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 251. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RejectConversation/RejectConversationCommandHandler.Refunds.cs`
- **Dòng 41**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (session.TotalFrozen == 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 252. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RejectConversation/RejectConversationCommandHandler.Refunds.cs`
- **Dòng 74**: `"settle_refund_{item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_refund_{item.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 253. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RejectConversation/RejectConversationCommandHandler.Refunds.cs`
- **Dòng 91**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `item.DisputeWindowEnd = now.AddHours(24);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 254. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandHandler.Validation.cs`
- **Dòng 13**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (request.AmountDiamond <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 255. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandHandler.Validation.cs`
- **Dòng 25**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (request.Description.Trim().Length > 100)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 256. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandHandler.Workflow.cs`
- **Dòng 101**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `ExpiresAt = DateTime.UtcNow.AddHours(24)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 257. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandValidator.cs`
- **Dòng 28**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(1000)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 258. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationAddMoney/RequestConversationAddMoneyCommandValidator.cs`
- **Dòng 34**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 259. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationComplete/RequestConversationCompleteCommandHandler.Flow.cs`
- **Dòng 30**: `12, 48`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `context.Conversation.Confirm.AutoResolveAt = context.Now.AddHours(context.IsUserRequester ? 12 : 48);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 260. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RequestConversationComplete/RequestConversationCompleteCommandHandler.Settlement.cs`
- **Dòng 113**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (lockedSession.TotalFrozen <= 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 261. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RespondConversationAddMoney/RespondConversationAddMoneyCommandHandler.Workflow.Messages.cs`
- **Dòng 59**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (offer.PaymentPayload == null || offer.PaymentPayload.AmountDiamond <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 262. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RespondConversationAddMoney/RespondConversationAddMoneyCommandHandler.Workflow.cs`
- **Dòng 101**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var amountDiamond = offer.PaymentPayload?.AmountDiamond ?? 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 263. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RespondConversationAddMoney/RespondConversationAddMoneyCommandHandler.Workflow.cs`
- **Dòng 102**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (amountDiamond <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 264. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/RespondConversationComplete/RespondConversationCompleteCommandHandler.Settlement.cs`
- **Dòng 102**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (lockedSession.TotalFrozen <= 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 265. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.FirstMessageFreeze.Workflow.cs`
- **Dòng 86**: `"freeze_{idempotencyKey}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"freeze_{idempotencyKey}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 266. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.FirstMessageFreeze.cs`
- **Dòng 29**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (readerProfile.DiamondPerQuestion <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 267. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Commands/SendMessage/SendMessageCommandHandler.FirstMessageFreeze.cs`
- **Dòng 79**: `6, 12`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return slaHours <= 6 ? 6 : 12;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 268. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Chat/Queries/ListMessages/ListMessagesQuery.cs`
- **Dòng 21**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int Limit { get; set; } = 50;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 269. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Commands/DailyCheckIn/DailyCheckInCommandHandler.cs`
- **Dòng 76**: `"{CheckinIdempotencyPrefix}{user.Id}_{todayString}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `ReferenceId = $"{CheckinIdempotencyPrefix}{user.Id}_{todayString}"`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 270. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Commands/DailyCheckIn/DailyCheckInCommandHandler.cs`
- **Dòng 109**: `"{CheckinIdempotencyPrefix}{userId}_{businessDate}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"{CheckinIdempotencyPrefix}{userId}_{businessDate}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 271. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Commands/DailyCheckIn/DailyCheckInCommandHandler.cs`
- **Dòng 121**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `GoldRewarded = 0,`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 272. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Commands/PurchaseFreeze/PurchaseStreakFreezeCommandHandler.cs`
- **Dòng 123**: `-2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var dateBeforeBreak = lastStreakDate ?? todayDate.AddDays(-2);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 273. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Commands/PurchaseFreeze/PurchaseStreakFreezeCommandHandler.cs`
- **Dòng 124**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var breakDiscoveryDate = dateBeforeBreak.AddDays(2);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 274. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Commands/PurchaseFreeze/PurchaseStreakFreezeCommandValidator.cs`
- **Dòng 21**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 275. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Queries/GetStreakStatus/GetStreakStatusQueryHandler.cs`
- **Dòng 85**: `-2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var dateBeforeBreak = user.LastStreakDate ?? todayDate.AddDays(-2);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 276. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/CheckIn/Queries/GetStreakStatus/GetStreakStatusQueryHandler.cs`
- **Dòng 86**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var breakDiscoveryDate = dateBeforeBreak.AddDays(2);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 277. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Community/Commands/AddComment/AddCommentCommand.cs`
- **Dòng 96**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await _postRepo.IncrementCommentsCountAsync(request.PostId, 1, cancellationToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 278. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Community/Commands/ToggleReaction/ToggleReactionCommand.cs`
- **Dòng 104**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, 1, cancellationToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 279. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Community/Commands/ToggleReaction/ToggleReactionCommand.cs`
- **Dòng 120**: `-1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, -1, cancellationToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 280. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Community/Commands/ToggleReaction/ToggleReactionCommand.cs`
- **Dòng 136**: `-1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await _postRepo.IncrementReactionCountAsync(request.PostId, oldType, -1, cancellationToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 281. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Community/Commands/ToggleReaction/ToggleReactionCommand.cs`
- **Dòng 137**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await _postRepo.IncrementReactionCountAsync(request.PostId, request.ReactionType, 1, cancellationToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 282. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Deposit/Commands/CreateDepositOrder/CreateDepositOrderCommandValidator.cs`
- **Dòng 18**: `64`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(64);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 283. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Deposit/Commands/CreateDepositOrder/CreateDepositOrderCommandValidator.cs`
- **Dòng 22**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 284. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Deposit/Commands/ProcessDepositWebhook/ProcessDepositWebhookCommandValidator.cs`
- **Dòng 15**: `64_000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(64_000);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 285. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Deposit/Queries/ListMyDepositOrders/ListMyDepositOrdersQuery.cs`
- **Dòng 23**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int PageSize { get; set; } = 10;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 286. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Deposit/Queries/ListMyDepositOrders/ListMyDepositOrdersQueryHandler.cs`
- **Dòng 27**: `0, 10, 50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedPageSize = request.PageSize <= 0 ? 10 : Math.Min(request.PageSize, 50);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 287. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Deposit/Queries/ListMyDepositOrders/ListMyDepositOrdersQueryValidator.cs`
- **Dòng 30**: `1, 50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.InclusiveBetween(1, 50);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 288. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs`
- **Dòng 22**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (normalized.Length > 128)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 289. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs`
- **Dòng 110**: `"freeze_{idempotencyKey}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"freeze_{idempotencyKey}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 290. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs`
- **Dòng 136**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `ReaderResponseDueAt = now.AddHours(24),`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 291. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandHandler.Workflow.cs`
- **Dòng 137**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AutoRefundAt = now.AddHours(24),`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 292. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandValidator.cs`
- **Dòng 28**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 293. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommandValidator.cs`
- **Dòng 34**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(128);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 294. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs`
- **Dòng 28**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int MinReasonLength = 10;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 295. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs`
- **Dòng 31**: `48`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private static readonly TimeSpan DisputeWindowDuration = TimeSpan.FromHours(48);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 296. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommandValidator.cs`
- **Dòng 25**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MinimumLength(10)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 297. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommandValidator.cs`
- **Dòng 26**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(1000);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 298. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaHistory/GetGachaHistoryQuery.cs`
- **Dòng 24**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int PageSize { get; init; } = 20;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 299. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaHistory/GetGachaHistoryQuery.cs`
- **Dòng 29**: `1, 20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public GetGachaHistoryQuery(Guid userId, int page = 1, int pageSize = 20)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 300. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaHistory/GetGachaHistoryQueryHandler.cs`
- **Dòng 28**: `1, 100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedPageSize = Math.Clamp(request.PageSize, 1, 100);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 301. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaPoolOdds/GetGachaPoolOddsQueryHandler.cs`
- **Dòng 67**: `100d`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `ProbabilityPercent = r.ProbabilityBasisPoints / 100d,`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 302. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gacha/Queries/GetGachaPools/GetGachaPoolsQueryHandler.cs`
- **Dòng 36**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var pity = 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 303. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Commands/ClaimQuestRewardCommandHandler.Rewards.cs`
- **Dòng 87**: `"quest_reward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Source = "quest_reward"`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 304. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Commands/ClaimQuestRewardCommandHandler.Rewards.cs`
- **Dòng 135**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static RewardSummary Empty => new(string.Empty, 0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 305. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Commands/ClaimQuestRewardCommandValidator.cs`
- **Dòng 21**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(100);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 306. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Commands/ClaimQuestRewardCommandValidator.cs`
- **Dòng 26**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(100);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 307. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQuery.cs`
- **Dòng 10**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public record GetLeaderboardQuery(Guid? UserId, string ScoreTrack, string? PeriodKey, int Limit = 100) : IRequest<LeaderboardResultDto>;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 308. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQueryHandler.Helpers.cs`
- **Dòng 85**: `"spent_gold_daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var track = (rawTrack?.Trim() ?? "spent_gold_daily").ToLowerInvariant();`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 309. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQueryHandler.Helpers.cs`
- **Dòng 87**: `"_daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (track.EndsWith("_daily", StringComparison.Ordinal))`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 310. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQueryHandler.Helpers.cs`
- **Dòng 89**: `6, "daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return new LeaderboardScope(track[..^6], periodKey ?? PeriodKeyHelper.GetPeriodKey("daily"));`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 311. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQueryHandler.Helpers.cs`
- **Dòng 94**: `8`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return new LeaderboardScope(track[..^8], periodKey ?? PeriodKeyHelper.GetPeriodKey("monthly"));`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 312. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQueryHandler.Helpers.cs`
- **Dòng 99**: `4`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return new LeaderboardScope(track[..^4], periodKey ?? "all");`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 313. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Gamification/Queries/GetLeaderboardQueryHandler.Helpers.cs`
- **Dòng 104**: `"daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"daily" => new LeaderboardScope(track, PeriodKeyHelper.GetPeriodKey("daily")),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 314. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/History/Queries/GetReadingDetail/GetReadingDetailQuery.cs`
- **Dòng 160**: `"ai_stream"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RequestType = a.IdempotencyKey != null && a.IdempotencyKey.Contains("ai_stream")`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 315. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/History/Queries/GetReadingDetail/GetReadingDetailQuery.cs`
- **Dòng 161**: `5, "Followup"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `? (a.ChargeDiamond > 5 ? "Followup" : "InitialReading")`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 316. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reader/Commands/SubmitReaderRequest/SubmitReaderRequestValidator.cs`
- **Dòng 12**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int MinBioLength = 20;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 317. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reader/Commands/SubmitReaderRequest/SubmitReaderRequestValidator.cs`
- **Dòng 13**: `4_000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int MaxBioLength = 4_000;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 318. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reader/Commands/SubmitReaderRequest/SubmitReaderRequestValidator.cs`
- **Dòng 15**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const long MinDiamondPerQuestion = 50;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 319. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reader/Commands/UpdateReaderProfile/UpdateReaderProfileCommandValidator.cs`
- **Dòng 14**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const long MinDiamondPerQuestion = 50;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 320. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.Processing.cs`
- **Dòng 77**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (record.ChargeDiamond <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 321. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.WalletAndTelemetry.cs`
- **Dòng 88**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (record.ChargeDiamond <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 322. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reading/Commands/InitSession/InitReadingSessionCommandValidator.cs`
- **Dòng 29**: `2000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.MaximumLength(2000)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 323. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.RequestCreation.cs`
- **Dòng 31**: `"ai_stream_{session.Id}_{Guid.CreateVersion7():N}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `IdempotencyKey = $"ai_stream_{session.Id}_{Guid.CreateVersion7():N}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 324. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommandHandler.Validation.cs`
- **Dòng 68**: `"ratelimit:{userId}:ai_interpret"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var rateLimitKey = $"ratelimit:{userId}:ai_interpret";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 325. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Wallet/Queries/GetLedgerList/GetLedgerListQuery.cs`
- **Dòng 17**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int Limit { get; set; } = 20;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 326. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Wallet/Queries/GetLedgerList/GetLedgerListQuery.cs`
- **Dòng 23**: `1, 20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public GetLedgerListQuery(Guid userId, int page = 1, int limit = 20)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 327. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Wallet/Queries/GetLedgerList/GetLedgerListQuery.cs`
- **Dòng 30**: `1, 20, 100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Limit = limit < 1 ? 20 : limit > 100 ? 100 : limit;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 328. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Withdrawal/Queries/GetWithdrawalDetail/GetWithdrawalDetailQuery.cs`
- **Dòng 74**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int TransferSuffixLength = 12;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 329. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Withdrawal/Queries/ListWithdrawals/ListWithdrawalsQuery.cs`
- **Dòng 25**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int PageSize { get; set; } = 20;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 330. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Helpers/PeriodKeyHelper.cs`
- **Dòng 22**: `"daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"daily" => now.ToString("yyyy-MM-dd"),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 331. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Interfaces/IChatMessageRepository.cs`
- **Dòng 65**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `int limit = 200,`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 332. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Interfaces/IConversationRepository.cs`
- **Dòng 67**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `int limit = 200,`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 333. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Interfaces/IRefreshTokenRepository.cs`
- **Dòng 220**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `InvalidToken = 3,`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 334. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Services/EscrowSettlementService.State.cs`
- **Dòng 39**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `item.DisputeWindowEnd = now.AddHours(24);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 335. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Services/EscrowSettlementService.State.cs`
- **Dòng 63**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 336. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Services/EscrowSettlementService.cs`
- **Dòng 36**: `0.10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 337. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Services/EscrowSettlementService.cs`
- **Dòng 48**: `"settle_release_{item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_release_{item.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 338. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Services/EscrowSettlementService.cs`
- **Dòng 52**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (fee > 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 339. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Services/EscrowSettlementService.cs`
- **Dòng 57**: `"platform_fee"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `referenceSource: "platform_fee",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 340. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Services/EscrowSettlementService.cs`
- **Dòng 60**: `"settle_fee_{item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_fee_{item.Id}",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 341. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/DepositOrder.cs`
- **Dòng 180**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (baseDiamondAmount <= 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 342. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/DepositOrder.cs`
- **Dòng 185**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (bonusGoldAmount < 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 343. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/EmailOtp.cs`
- **Dòng 49**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public EmailOtp(Guid userId, string otpCode, string type, int expiryMinutes = 15)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 344. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/FreeDrawCredit.cs`
- **Dòng 55**: `3, 5, 10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (spreadCardCount is not (3 or 5 or 10))`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 345. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/GachaHistoryEntry.cs`
- **Dòng 103**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (pityBefore < 0 || pityAfter < 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 346. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/GachaPool.cs`
- **Dòng 144**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `HardPityCount = pityEnabled ? EnsurePositiveInt(hardPityCount, nameof(hardPityCount)) : 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 347. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/GachaPullOperation.cs`
- **Dòng 100**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CurrentPityCount = 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 348. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/GachaPullOperation.cs`
- **Dòng 101**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `HardPityThreshold = 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 349. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/GachaPullOperation.cs`
- **Dòng 112**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (currentPityCount < 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 350. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/GachaPullOperation.cs`
- **Dòng 117**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (hardPityThreshold < 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 351. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/ReadingSession.cs`
- **Dòng 57**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public ReadingSession(string userId, string spreadType, string? question = null, string? currencyUsed = null, long amountCharged = 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 352. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/RefreshToken.cs`
- **Dòng 171**: `64`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RevocationReason = Normalize(reason, 64, RefreshRevocationReasons.ManualRevoke);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 353. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/RefreshToken.cs`
- **Dòng 194**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `LastRotateIdempotencyKey = Normalize(idempotencyKey, 128, string.Empty);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 354. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/RefreshToken.cs`
- **Dòng 228**: `64`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (Token.Length < 64)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 355. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/User.Account.cs`
- **Dòng 21**: `1, 100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var newLevel = 1 + (int)(Exp / 100);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 356. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/User.Streak.cs`
- **Dòng 49**: `1.0, 100.0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return 1.0 + (CurrentStreak / 100.0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 357. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/User.Streak.cs`
- **Dòng 87**: `10.0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return (long)Math.Ceiling(PreBreakStreak / 10.0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 358. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserCollection.cs`
- **Dòng 18**: `100m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const decimal BaseExpToNextLevel = 100m;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 359. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserCollection.cs`
- **Dòng 23**: `50m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const decimal ExpIncreasePerLevel = 50m;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 360. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserCollection.cs`
- **Dòng 28**: `10m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const decimal DefaultBaseAtk = 10m;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 361. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserCollection.cs`
- **Dòng 33**: `10m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const decimal DefaultBaseDef = 10m;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 362. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserCollection.cs`
- **Dòng 188**: `100m`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var total = clampedBase + (clampedBase * clampedBonus / 100m);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 363. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserWallet.cs`
- **Dòng 11**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long GoldBalance { get; private set; } = 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 364. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserWallet.cs`
- **Dòng 14**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long DiamondBalance { get; private set; } = 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 365. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Entities/UserWallet.cs`
- **Dòng 17**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long FrozenDiamondBalance { get; private set; } = 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 366. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/AiRequestStatus.cs`
- **Dòng 14**: `"failed_before_first_token"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FailedBeforeFirstToken = "failed_before_first_token";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 367. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/AiRequestStatus.cs`
- **Dòng 17**: `"failed_after_first_token"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FailedAfterFirstToken = "failed_after_first_token";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 368. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/CurrencyType.cs`
- **Dòng 8**: `"gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Gold = "gold";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 369. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/CurrencyType.cs`
- **Dòng 11**: `"diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Diamond = "diamond";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 370. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/QuestRewardType.cs`
- **Dòng 8**: `"gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Gold = "gold";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 371. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/QuestRewardType.cs`
- **Dòng 11**: `"diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Diamond = "diamond";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 372. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/QuestRewardType.cs`
- **Dòng 14**: `"exp"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Exp = "exp";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 373. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/QuestType.cs`
- **Dòng 8**: `"daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Daily = "daily";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 374. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/SpreadType.cs`
- **Dòng 8**: `"daily_1"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Daily1Card = "daily_1";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 375. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/SpreadType.cs`
- **Dòng 11**: `"spread_3"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Spread3Cards = "spread_3";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 376. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/SpreadType.cs`
- **Dòng 14**: `"spread_5"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Spread5Cards = "spread_5";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 377. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/SpreadType.cs`
- **Dòng 17**: `"spread_10"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Spread10Cards = "spread_10";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 378. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 8**: `"daily_checkin"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string DailyCheckin = "daily_checkin";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 379. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 11**: `"register_bonus"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string RegisterBonus = "register_bonus";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 380. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 20**: `"achievement_reward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string AchievementReward = "achievement_reward";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 381. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 23**: `"quest_reward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string QuestReward = "quest_reward";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 382. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 26**: `"share_reward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string ShareReward = "share_reward";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 383. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 29**: `"friend_chain_reward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FriendChainReward = "friend_chain_reward";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 384. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 35**: `"reading_cost_gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string ReadingCostGold = "reading_cost_gold";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 385. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 38**: `"deposit"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Deposit = "deposit";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 386. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 41**: `"deposit_bonus"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string DepositBonus = "deposit_bonus";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 387. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 44**: `"referral_reward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string ReferralReward = "referral_reward";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 388. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 53**: `"reading_cost_diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string ReadingCostDiamond = "reading_cost_diamond";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 389. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 65**: `"followup_cost"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FollowupCost = "followup_cost";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 390. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 68**: `"gacha_cost"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string GachaCost = "gacha_cost";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 391. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 71**: `"gacha_reward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string GachaReward = "gacha_reward";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 392. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 74**: `"inventory_reward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string InventoryReward = "inventory_reward";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 393. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/Enums/TransactionType.cs`
- **Dòng 83**: `"streak_freeze_cost"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string StreakFreezeCost = "streak_freeze_cost";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 394. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/ValueObjects/EnhancementType.cs`
- **Dòng 11**: `"exp"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string Exp = "exp";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 395. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Domain/ValueObjects/EnhancementType.cs`
- **Dòng 31**: `"free_draw"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public const string FreeDraw = "free_draw";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 396. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 16**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int MaxBatchLoopsPerCycle = 10;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 397. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 94**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `for (var loop = 0; loop < MaxBatchLoopsPerCycle; loop++)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 398. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 128**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var refreshRetentionDays = Math.Max(1, _options.RefreshTokenRetentionDays);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 399. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 129**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var revokedSessionRetentionDays = Math.Max(1, _options.RevokedSessionRetentionDays);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 400. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 166**: `30, 0.8`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var leaseSeconds = Math.Max(30, (int)Math.Ceiling(interval.TotalSeconds * 0.8));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 401. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 167**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return TimeSpan.FromSeconds(Math.Min(1800, leaseSeconds));`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 402. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 172**: `50, 5000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return Math.Clamp(_options.CleanupBatchSize, 50, 5000);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 403. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/AuthSessionCleanupJob.cs`
- **Dòng 177**: `0, 30`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var minutes = _options.CleanupIntervalMinutes <= 0 ? 30 : _options.CleanupIntervalMinutes;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 404. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/ChatModerationQueue.cs`
- **Dòng 25**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var capacity = Math.Max(100, options.Value.MaxQueueSize);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 405. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoRefunds.Workflow.cs`
- **Dòng 37**: `"settle_refund_{item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_refund_{item.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 406. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoRefunds.Workflow.cs`
- **Dòng 76**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `item.DisputeWindowEnd = now.AddHours(24);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 407. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoRefunds.Workflow.cs`
- **Dòng 93**: `"reader_sla_timeout"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RefundSource = "reader_sla_timeout"`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 408. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.AutoReleases.cs`
- **Dòng 70**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (session != null && session.TotalFrozen <= 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 409. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.CompletionTimeouts.Helpers.cs`
- **Dòng 105**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (lockedSession.TotalFrozen <= 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 410. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.CompletionTimeouts.cs`
- **Dòng 21**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.GetConversationsAwaitingCompletionResolutionAsync(DateTime.UtcNow, 200, cancellationToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 411. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.Conversations.cs`
- **Dòng 19**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (session == null || session.TotalFrozen > 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 412. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.PaymentOffers.cs`
- **Dòng 19**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `200,`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 413. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs`
- **Dòng 19**: `-48`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var dueAtUtc = DateTime.UtcNow.AddHours(-48);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 414. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs`
- **Dòng 57**: `-48`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `|| (item.UpdatedAt ?? item.CreatedAt) > DateTime.UtcNow.AddHours(-48))`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 415. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.DisputesAndOffers.cs`
- **Dòng 70**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (session.TotalFrozen <= 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 416. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.ExpiredOffers.Workflow.cs`
- **Dòng 67**: `"settle_refund_{item.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"settle_refund_{item.Id}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 417. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.ExpiredOffers.Workflow.cs`
- **Dòng 87**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `item.DisputeWindowEnd = now.AddHours(24);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 418. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.ExpiredOffers.Workflow.cs`
- **Dòng 105**: `"offer_expired"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RefundSource = "offer_expired"`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 419. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.SessionBalances.cs`
- **Dòng 24**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 420. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs`
- **Dòng 36**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 421. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs`
- **Dòng 45**: `0, 5, 15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (now.Hour == 0 && now.Minute >= 5 && now.Minute <= 15)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 422. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs`
- **Dòng 49**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await Task.Delay(TimeSpan.FromHours(1), stoppingToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 423. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs`
- **Dòng 62**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 424. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs`
- **Dòng 87**: `"daily_rank_score"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await CreateSnapshotAsync(lbRepo, userRepo, "daily_rank_score", dailyKey, ct);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 425. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/LeaderboardSnapshotJob.cs`
- **Dòng 112**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var entries = await lbRepo.GetTopEntriesAsync(track, periodKey, 100, ct);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 426. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/MediaUploadCleanupJob.cs`
- **Dòng 131**: `0, 10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var minutes = _options.CleanupIntervalMinutes <= 0 ? 10 : _options.CleanupIntervalMinutes;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 427. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs`
- **Dòng 19**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int BatchSize = 50;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 428. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs`
- **Dòng 20**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int MaxRetryAttempts = 12;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 429. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs`
- **Dòng 21**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private static readonly TimeSpan LockTimeout = TimeSpan.FromMinutes(2);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 430. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs`
- **Dòng 154**: `3900`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `message.LastError = Truncate(exception.ToString(), 3900);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 431. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs`
- **Dòng 178**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var seconds = Math.Pow(2, attemptCount);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 432. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxBatchProcessor.cs`
- **Dòng 179**: `300`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return (int)Math.Min(300, seconds);`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 433. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxProcessorWorker.cs`
- **Dòng 13**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(5);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 434. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/StreakBreakBackgroundJob.cs`
- **Dòng 55**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await Task.Delay(TimeSpan.FromHours(1), stoppingToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 435. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/StreakBreakBackgroundJob.cs`
- **Dòng 94**: `100, 0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (processed++ % 100 == 0)`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 436. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/StreakBreakBackgroundJob.cs`
- **Dòng 96**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await Task.Delay(100, stoppingToken);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 437. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/DependencyInjection.Auth.cs`
- **Dòng 80**: `"access_token"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var queryToken = context.Request.Query["access_token"].ToString();`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 438. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/DependencyInjection.Auth.cs`
- **Dòng 81**: `"accessToken"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var cookieToken = context.Request.Cookies["accessToken"];`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 439. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
- **Dòng 87**: `2000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `options.ConnectTimeout = 2000;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 440. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
- **Dòng 88**: `2000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `options.SyncTimeout = 2000;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 441. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
- **Dòng 89**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `options.ConnectRetry = 1;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 442. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/DependencyInjection.cs`
- **Dòng 38**: `"Deposit"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `services.Configure<DepositOptions>(configuration.GetSection("Deposit"));`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 443. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AiProviderOptions.cs`
- **Dòng 16**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int TimeoutSeconds { get; set; } = 30;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 444. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AiProviderOptions.cs`
- **Dòng 19**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int MaxRetries { get; set; } = 2;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 445. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/Argon2Options.cs`
- **Dòng 7**: `19456`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int MemoryKB { get; set; } = 19456;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 446. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/Argon2Options.cs`
- **Dòng 10**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int Iterations { get; set; } = 2;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 447. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 11**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int RefreshLockSeconds { get; set; } = 15;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 448. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 16**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int RefreshIdempotencyWindowSeconds { get; set; } = 60;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 449. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 21**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int AccessTokenBlacklistTtlSeconds { get; set; } = 1200;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 450. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 26**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int SessionRevocationTtlSeconds { get; set; } = 1800;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 451. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 31**: `30, 24, 60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int SessionCacheTtlSeconds { get; set; } = 30 * 24 * 60 * 60;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 452. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 36**: `24, 60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int ReplaySecurityRecordTtlSeconds { get; set; } = 24 * 60 * 60;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 453. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 41**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int CleanupBatchSize { get; set; } = 200;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 454. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 46**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int CleanupIntervalMinutes { get; set; } = 30;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 455. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 51**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int RefreshTokenRetentionDays { get; set; } = 30;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 456. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/AuthSecurityOptions.cs`
- **Dòng 56**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int RevokedSessionRetentionDays { get; set; } = 30;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 457. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/ChatModerationOptions.cs`
- **Dòng 10**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int MaxQueueSize { get; set; } = 1000;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 458. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/DepositOptions.cs`
- **Dòng 13**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int LinkExpiryMinutes { get; set; } = 15;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 459. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/DepositOptions.cs`
- **Dòng 18**: `50_000, 500, "topup_50k"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new DepositPackageOption { Code = "topup_50k", AmountVnd = 50_000, BaseDiamond = 500, IsActive = true },`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 460. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/DepositOptions.cs`
- **Dòng 19**: `100_000, 1_000, "topup_100k"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new DepositPackageOption { Code = "topup_100k", AmountVnd = 100_000, BaseDiamond = 1_000, IsActive = true },`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 461. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/DepositOptions.cs`
- **Dòng 20**: `200_000, 2_000, "topup_200k"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new DepositPackageOption { Code = "topup_200k", AmountVnd = 200_000, BaseDiamond = 2_000, IsActive = true },`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 462. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/DepositOptions.cs`
- **Dòng 21**: `500_000, 5_000, "topup_500k"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new DepositPackageOption { Code = "topup_500k", AmountVnd = 500_000, BaseDiamond = 5_000, IsActive = true },`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 463. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/DepositOptions.cs`
- **Dòng 22**: `1_000_000, 10_000, "topup_1m"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new DepositPackageOption { Code = "topup_1m", AmountVnd = 1_000_000, BaseDiamond = 10_000, IsActive = true }`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 464. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/JwtOptions.cs`
- **Dòng 16**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int ExpiryMinutes { get; set; } = 15;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 465. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/JwtOptions.cs`
- **Dòng 19**: `7`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int RefreshExpiryDays { get; set; } = 7;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 466. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/ObjectStorageOptions.cs`
- **Dòng 12**: `10, 1024`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long MaxUploadBytes { get; set; } = 10 * 1024 * 1024;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 467. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/ObjectStorageOptions.cs`
- **Dòng 15**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int PresignExpiryMinutes { get; set; } = 10;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 468. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/ObjectStorageOptions.cs`
- **Dòng 18**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int CommunityOrphanTtlHours { get; set; } = 24;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 469. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/ObjectStorageOptions.cs`
- **Dòng 21**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int CleanupBatchSize { get; set; } = 200;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 470. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/ObjectStorageOptions.cs`
- **Dòng 24**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int CleanupIntervalMinutes { get; set; } = 10;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 471. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SmtpOptions.cs`
- **Dòng 10**: `587`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int Port { get; set; } = 587;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 472. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 10**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int DailyAiQuota { get; set; } = 3;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 473. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 13**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int InFlightAiCap { get; set; } = 3;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 474. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 16**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int ReadingRateLimitSeconds { get; set; } = 30;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 475. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 22**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long Spread3Gold { get; set; } = 50;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 476. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 25**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long Spread3Diamond { get; set; } = 5;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 477. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 28**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long Spread5Gold { get; set; } = 100;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 478. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 31**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long Spread5Diamond { get; set; } = 10;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 479. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 34**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long Spread10Gold { get; set; } = 500;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 480. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Options/SystemConfigOptions.cs`
- **Dòng 37**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long Spread10Diamond { get; set; } = 50;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 481. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AiRequestConfiguration.cs`
- **Dòng 19**: `"idx_ai_requests_idempotency"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string IdempotencyIndexName = "idx_ai_requests_idempotency";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 482. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AiRequestConfiguration.cs`
- **Dòng 52**: `"reading_session_ref"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasColumnName("reading_session_ref")`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 483. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AiRequestConfiguration.cs`
- **Dòng 62**: `"followup_sequence"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.FollowupSequence).HasColumnName("followup_sequence").IsRequired(false);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 484. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AiRequestConfiguration.cs`
- **Dòng 68**: `"first_token_at"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.FirstTokenAt).HasColumnName("first_token_at");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 485. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AiRequestConfiguration.cs`
- **Dòng 70**: `0, "charge_gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.ChargeGold).HasColumnName("charge_gold").HasDefaultValue(0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 486. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AiRequestConfiguration.cs`
- **Dòng 71**: `0, "charge_diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.ChargeDiamond).HasColumnName("charge_diamond").HasDefaultValue(0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 487. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AiRequestConfiguration.cs`
- **Dòng 74**: `100, "idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.IdempotencyKey).HasColumnName("idempotency_key").HasMaxLength(100);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 488. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/ChatQuestionItemConfiguration.cs`
- **Dòng 33**: `"finance_session_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.FinanceSessionId).HasColumnName("finance_session_id");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 489. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/ChatQuestionItemConfiguration.cs`
- **Dòng 38**: `"amount_diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.AmountDiamond).HasColumnName("amount_diamond");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 490. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/ChatQuestionItemConfiguration.cs`
- **Dòng 41**: `"idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.IdempotencyKey).HasColumnName("idempotency_key");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 491. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/ChatQuestionItemConfiguration.cs`
- **Dòng 76**: `"fk_chat_question_items_chat_finance_sessions_finance_session_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasConstraintName("fk_chat_question_items_chat_finance_sessions_finance_session_id");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 492. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/ChatQuestionItemConfiguration.cs`
- **Dòng 86**: `"ix_chat_question_items_finance_session_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_chat_question_items_finance_session_id");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 493. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/ChatQuestionItemConfiguration.cs`
- **Dòng 89**: `"ix_chat_question_items_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_chat_question_items_idempotency_key")`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 494. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs`
- **Dòng 15**: `"deposit_orders"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.ToTable("deposit_orders");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 495. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs`
- **Dòng 25**: `64, "package_code"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(o => o.PackageCode).HasColumnName("package_code").HasMaxLength(64).IsRequired();`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 496. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs`
- **Dòng 27**: `"base_diamond_amount"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(o => o.BaseDiamondAmount).HasColumnName("base_diamond_amount").IsRequired();`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 497. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs`
- **Dòng 28**: `"bonus_gold_amount"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(o => o.BonusGoldAmount).HasColumnName("bonus_gold_amount").IsRequired();`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 498. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs`
- **Dòng 29**: `"diamond_amount"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(o => o.DiamondAmount).HasColumnName("diamond_amount").IsRequired();`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 499. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs`
- **Dòng 57**: `"ix_deposit_orders_client_request_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_deposit_orders_client_request_key");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 500. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs`
- **Dòng 61**: `"ix_deposit_orders_payos_order_code"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_deposit_orders_payos_order_code");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 501. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositOrderConfiguration.cs`
- **Dòng 64**: `"ix_deposit_orders_status"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_deposit_orders_status");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 502. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositPromotionConfiguration.cs`
- **Dòng 15**: `"deposit_promotions"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.ToTable("deposit_promotions");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 503. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositPromotionConfiguration.cs`
- **Dòng 24**: `"bonus_gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasColumnName("bonus_gold")`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 504. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/DepositPromotionConfiguration.cs`
- **Dòng 36**: `"ix_deposit_promotions_min_amount_vnd"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_deposit_promotions_min_amount_vnd");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 505. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/FreeDrawCreditConfiguration.cs`
- **Dòng 18**: `"free_draw_credits"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"free_draw_credits",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 506. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/FreeDrawCreditConfiguration.cs`
- **Dòng 21**: `"ck_free_draw_credits_available_count"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `tableBuilder.HasCheckConstraint("ck_free_draw_credits_available_count", "\"available_count\" >= 0");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 507. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/FreeDrawCreditConfiguration.cs`
- **Dòng 23**: `"ck_free_draw_credits_spread_card_count_valid"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_free_draw_credits_spread_card_count_valid",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 508. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaHistoryEntryConfiguration.cs`
- **Dòng 18**: `"gacha_history_entries"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"gacha_history_entries",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 509. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaHistoryEntryConfiguration.cs`
- **Dòng 21**: `"ck_gacha_history_entries_pull_count_positive"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `tableBuilder.HasCheckConstraint("ck_gacha_history_entries_pull_count_positive", "\"pull_count\" > 0");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 510. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaHistoryEntryConfiguration.cs`
- **Dòng 22**: `"ck_gacha_history_entries_pity_before_non_negative"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `tableBuilder.HasCheckConstraint("ck_gacha_history_entries_pity_before_non_negative", "\"pity_before\" >= 0");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 511. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaHistoryEntryConfiguration.cs`
- **Dòng 23**: `"ck_gacha_history_entries_pity_after_non_negative"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `tableBuilder.HasCheckConstraint("ck_gacha_history_entries_pity_after_non_negative", "\"pity_after\" >= 0");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 512. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolConfiguration.cs`
- **Dòng 18**: `"gacha_pools"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"gacha_pools",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 513. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolConfiguration.cs`
- **Dòng 22**: `"ck_gacha_pools_cost_amount_positive"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pools_cost_amount_positive",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 514. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolConfiguration.cs`
- **Dòng 25**: `"ck_gacha_pools_hard_pity_non_negative"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pools_hard_pity_non_negative",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 515. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolConfiguration.cs`
- **Dòng 28**: `"ck_gacha_pools_hard_pity_when_enabled"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pools_hard_pity_when_enabled",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 516. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolRewardRateConfiguration.cs`
- **Dòng 18**: `"gacha_pool_reward_rates"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"gacha_pool_reward_rates",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 517. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolRewardRateConfiguration.cs`
- **Dòng 22**: `"ck_gacha_pool_reward_rates_probability_positive"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pool_reward_rates_probability_positive",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 518. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolRewardRateConfiguration.cs`
- **Dòng 25**: `"ck_gacha_pool_reward_rates_quantity_positive"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pool_reward_rates_quantity_positive",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 519. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolRewardRateConfiguration.cs`
- **Dòng 28**: `"ck_gacha_pool_reward_rates_kind_item"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pool_reward_rates_kind_item",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 520. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolRewardRateConfiguration.cs`
- **Dòng 31**: `"ck_gacha_pool_reward_rates_kind_currency"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pool_reward_rates_kind_currency",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 521. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPoolRewardRateConfiguration.cs`
- **Dòng 38**: `32`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.RewardKind).IsRequired().HasMaxLength(32);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 522. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullOperationConfiguration.cs`
- **Dòng 18**: `"gacha_pull_operations"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"gacha_pull_operations",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 523. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullOperationConfiguration.cs`
- **Dòng 22**: `"ck_gacha_pull_operations_pull_count_positive"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pull_operations_pull_count_positive",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 524. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullOperationConfiguration.cs`
- **Dòng 25**: `"ck_gacha_pull_operations_current_pity_non_negative"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pull_operations_current_pity_non_negative",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 525. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullOperationConfiguration.cs`
- **Dòng 28**: `"ck_gacha_pull_operations_hard_pity_non_negative"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pull_operations_hard_pity_non_negative",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 526. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullOperationConfiguration.cs`
- **Dòng 36**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.IdempotencyKey).IsRequired().HasMaxLength(128);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 527. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullRewardLogConfiguration.cs`
- **Dòng 18**: `"gacha_pull_reward_logs"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"gacha_pull_reward_logs",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 528. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullRewardLogConfiguration.cs`
- **Dòng 22**: `"ck_gacha_pull_reward_logs_quantity_positive"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pull_reward_logs_quantity_positive",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 529. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullRewardLogConfiguration.cs`
- **Dòng 25**: `"ck_gacha_pull_reward_logs_pity_non_negative"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pull_reward_logs_pity_non_negative",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 530. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullRewardLogConfiguration.cs`
- **Dòng 28**: `"ck_gacha_pull_reward_logs_kind_item"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pull_reward_logs_kind_item",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 531. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullRewardLogConfiguration.cs`
- **Dòng 31**: `"ck_gacha_pull_reward_logs_kind_currency"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ck_gacha_pull_reward_logs_kind_currency",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 532. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/GachaPullRewardLogConfiguration.cs`
- **Dòng 43**: `32`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.RewardKind).IsRequired().HasMaxLength(32);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 533. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/InventoryItemUseOperationConfiguration.cs`
- **Dòng 21**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.IdempotencyKey).HasMaxLength(128).IsRequired();`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 534. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/InventoryItemUseOperationConfiguration.cs`
- **Dòng 27**: `"ix_inventory_item_use_operations_user_id_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_inventory_item_use_operations_user_id_idempotency_key")`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 535. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/RefreshTokenConfiguration.cs`
- **Dòng 18**: `"refresh_tokens"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.ToTable("refresh_tokens");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 536. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserConfiguration.cs`
- **Dòng 69**: `0, "user_exp"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(u => u.Exp).HasColumnName("user_exp").HasDefaultValue(0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 537. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserConfiguration.cs`
- **Dòng 71**: `0, "current_streak"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(u => u.CurrentStreak).HasColumnName("current_streak").HasDefaultValue(0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 538. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserConfiguration.cs`
- **Dòng 72**: `"last_streak_date"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(u => u.LastStreakDate).HasColumnName("last_streak_date");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 539. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserConfiguration.cs`
- **Dòng 73**: `0, "pre_break_streak"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(u => u.PreBreakStreak).HasColumnName("pre_break_streak").HasDefaultValue(0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 540. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserConfiguration.cs`
- **Dòng 106**: `0L, "gold_balance"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `wallet.Property(w => w.GoldBalance).HasColumnName("gold_balance").HasDefaultValue(0L);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 541. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserConfiguration.cs`
- **Dòng 107**: `0L, "diamond_balance"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `wallet.Property(w => w.DiamondBalance).HasColumnName("diamond_balance").HasDefaultValue(0L);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 542. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserConfiguration.cs`
- **Dòng 108**: `0L, "frozen_diamond_balance"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `wallet.Property(w => w.FrozenDiamondBalance).HasColumnName("frozen_diamond_balance").HasDefaultValue(0L);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 543. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserGachaPityConfiguration.cs`
- **Dòng 18**: `"user_gacha_pities"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"user_gacha_pities",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 544. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserGachaPityConfiguration.cs`
- **Dòng 19**: `"ck_user_gacha_pities_pull_count_non_negative"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `tableBuilder => tableBuilder.HasCheckConstraint("ck_user_gacha_pities_pull_count_non_negative", "\"pull_count\" >= 0"));`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 545. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WalletTransactionConfiguration.cs`
- **Dòng 44**: `"idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 546. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WalletTransactionConfiguration.cs`
- **Dòng 48**: `"ix_wallet_transactions_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_wallet_transactions_idempotency_key")`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 547. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WithdrawalRequestConfiguration.cs`
- **Dòng 34**: `"amount_diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.AmountDiamond).HasColumnName("amount_diamond");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 548. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WithdrawalRequestConfiguration.cs`
- **Dòng 36**: `"fee_vnd"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `builder.Property(x => x.FeeVnd).HasColumnName("fee_vnd");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 549. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WithdrawalRequestConfiguration.cs`
- **Dòng 44**: `"request_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasColumnName("request_idempotency_key")`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 550. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WithdrawalRequestConfiguration.cs`
- **Dòng 48**: `"process_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasColumnName("process_idempotency_key")`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 551. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WithdrawalRequestConfiguration.cs`
- **Dòng 73**: `"ix_withdrawal_process_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.HasDatabaseName("ix_withdrawal_process_idempotency_key")`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 552. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Checkin.cs`
- **Dòng 23**: `90, "idx_ttl_90d"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 553. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Core.cs`
- **Dòng 84**: `90, "idx_ttl_90d"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new CreateIndexOptions { Name = "idx_ttl_90d", ExpireAfter = TimeSpan.FromDays(90) }));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 554. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Core.cs`
- **Dòng 101**: `30, "idx_ttl_30d"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new CreateIndexOptions { Name = "idx_ttl_30d", ExpireAfter = TimeSpan.FromDays(30) }));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 555. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.Gamification.Helpers.cs`
- **Dòng 30**: `90`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(90) })`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 556. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.Indexes.MediaUpload.cs`
- **Dòng 25**: `"idx_expiresat_consumed_cleanup"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new CreateIndexOptions { Name = "idx_expiresat_consumed_cleanup" }));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 557. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`
- **Dòng 45**: `"reading_sessions"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> _database.GetCollection<ReadingSessionDocument>("reading_sessions");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 558. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`
- **Dòng 49**: `"daily_checkins"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> _database.GetCollection<DailyCheckinDocument>("daily_checkins");`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 559. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`
- **Dòng 93**: `"upload_sessions"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> _database.GetCollection<UploadSessionDocument>("upload_sessions");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 560. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`
- **Dòng 125**: `"leaderboard_entries"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> _database.GetCollection<LeaderboardEntryDocument>("leaderboard_entries");`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 561. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDbContext.cs`
- **Dòng 129**: `"leaderboard_snapshots"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> _database.GetCollection<LeaderboardSnapshotDocument>("leaderboard_snapshots");`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 562. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ChatMessageDocument.cs`
- **Dòng 70**: `"amount_diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("amount_diamond")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 563. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ConversationDocument.cs`
- **Dòng 43**: `12`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public int SlaHours { get; set; } = 12;`
  Lý do cần có data này: Giá trị cố định đang nằm trong code và có nguy cơ thành magic value khi logic mở rộng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách thành Constants class/policy dùng chung; nếu có khả năng tuning thì chuyển Config/DB.

### 564. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/DailyCheckinDocument.cs`
- **Dòng 24**: `"goldReward"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("goldReward")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 565. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReaderProfileDocument.cs`
- **Dòng 130**: `"diamond_per_question"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("diamond_per_question")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 566. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReaderProfileDocument.cs`
- **Dòng 131**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public long DiamondPerQuestion { get; set; } = 50;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 567. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReaderRequestDocument.cs`
- **Dòng 72**: `"diamond_per_question"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("diamond_per_question")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 568. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReadingSessionDocument.cs`
- **Dòng 20**: `"session_type"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("session_type")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 569. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReadingSessionDocument.cs`
- **Dòng 24**: `"spread_type"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("spread_type")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 570. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReadingSessionDocument.cs`
- **Dòng 51**: `"followups"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("followups")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 571. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReadingSessionDocument.cs`
- **Dòng 103**: `"suggested_followup"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("suggested_followup")][BsonIgnoreIfNull] public string? SuggestedFollowup { get; set; }`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 572. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReadingSessionDocument.cs`
- **Dòng 127**: `"cost_diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("cost_diamond")] public long CostDiamond { get; set; }`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 573. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/ReadingSessionDocument.cs`
- **Dòng 129**: `0, "cost_gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("cost_gold")] public long CostGold { get; set; } = 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 574. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UploadSessionDocument.cs`
- **Dòng 10**: `"upload_token"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("upload_token")]`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 575. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 40**: `"exp"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("exp")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 576. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 47**: `"exp_to_next_level"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("exp_to_next_level")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 577. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 73**: `"base_atk"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("base_atk")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 578. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 80**: `"base_def"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("base_def")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 579. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 87**: `"bonus_atk_percent"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("bonus_atk_percent")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 580. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 94**: `"bonus_def_percent"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("bonus_def_percent")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 581. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 101**: `"atk"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("atk")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 582. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 108**: `"def"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("def")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 583. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 203**: `"atk_bonus"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("atk_bonus")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 584. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/MongoDocuments/UserCollectionDocument.cs`
- **Dòng 210**: `"def_bonus"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `[BsonElement("def_bonus")]`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 585. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/AdminRepository.cs`
- **Dòng 35**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `WHERE u.gold_balance IS DISTINCT FROM COALESCE(v.ledger_gold, 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 586. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/AdminRepository.cs`
- **Dòng 36**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `OR u.diamond_balance IS DISTINCT FROM COALESCE(v.ledger_diamond, 0);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 587. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/AuthSessionRepository.cs`
- **Dòng 156**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (batchSize <= 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 588. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/CommunityMediaAssetRepository.cs`
- **Dòng 125**: `0, 100, 1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedLimit = limit <= 0 ? 100 : Math.Min(limit, 1000);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 589. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/DepositOrderRepository.Idempotency.cs`
- **Dòng 10**: `"ix_deposit_orders_client_request_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string ClientRequestKeyUniqueConstraintName = "ix_deposit_orders_client_request_key";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 590. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/FreeDrawCreditRepository.cs`
- **Dòng 56**: `3, 5, 10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (spreadCardCount is not (3 or 5 or 10))`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 591. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/FreeDrawCreditRepository.cs`
- **Dòng 88**: `3, 0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var spread3 = items.FirstOrDefault(x => x.SpreadCardCount == 3)?.AvailableCount ?? 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 592. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/FreeDrawCreditRepository.cs`
- **Dòng 89**: `5, 0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var spread5 = items.FirstOrDefault(x => x.SpreadCardCount == 5)?.AvailableCount ?? 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 593. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/FreeDrawCreditRepository.cs`
- **Dòng 90**: `10, 0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var spread10 = items.FirstOrDefault(x => x.SpreadCardCount == 10)?.AvailableCount ?? 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 594. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/FreeDrawCreditRepository.cs`
- **Dòng 106**: `3, 5, 10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (spreadCardCount is not (3 or 5 or 10))`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 595. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/GachaPoolRepository.PityHistory.cs`
- **Dòng 10**: `"ix_user_gacha_pities_user_id_pool_id"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string UserPityUniqueConstraint = "ix_user_gacha_pities_user_id_pool_id";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 596. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/GachaPoolRepository.cs`
- **Dòng 13**: `"ix_gacha_pull_operations_user_id_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string PullOperationUniqueConstraint = "ix_gacha_pull_operations_user_id_idempotency_key";`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 597. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/GachaPoolRepository.cs`
- **Dòng 74**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return currentPity?.PullCount ?? 0;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 598. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/GachaPoolRepository.cs`
- **Dòng 134**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (rewardLogs.Count == 0)`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 599. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/GachaPoolRepository.cs`
- **Dòng 161**: `1, 200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedLimit = Math.Clamp(limit, 1, 200);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 600. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/LedgerRepository.cs`
- **Dòng 42**: `0, 20, 200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedLimit = limit <= 0 ? 20 : Math.Min(limit, 200);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 601. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/LedgerRepository.cs`
- **Dòng 48**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.Skip((normalizedPage - 1) * normalizedLimit)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 602. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.Pagination.cs`
- **Dòng 50**: `0, 50, 200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedLimit = limit <= 0 ? 50 : Math.Min(limit, 200);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 603. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.Pagination.cs`
- **Dòng 73**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.Limit(normalizedLimit + 1)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 604. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.PaymentOffers.Fetch.cs`
- **Dòng 28**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.Limit(20)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 605. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.PaymentOffers.Fetch.cs`
- **Dòng 57**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `int limit = 200,`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 606. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.PaymentOffers.Fetch.cs`
- **Dòng 60**: `0, 200, 1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedLimit = limit <= 0 ? 200 : Math.Min(limit, 1000);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 607. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoChatMessageRepository.PaymentOffers.cs`
- **Dòng 38**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.Limit(100)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 608. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoConversationRepository.cs`
- **Dòng 105**: `200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `int limit = 200,`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 609. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoConversationRepository.cs`
- **Dòng 108**: `0, 200, 1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedLimit = limit <= 0 ? 200 : Math.Min(limit, 1000);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 610. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoReadingSessionRepository.Commands.cs`
- **Dòng 36**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (session.CurrencyUsed != null && session.AmountCharged > 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 611. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoTitleRepository.cs`
- **Dòng 104**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return await _context.UserTitles.CountDocumentsAsync(filter, cancellationToken: ct) > 0;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 612. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoUserCollectionRepository.Enhancements.cs`
- **Dòng 17**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int ExpBranchOneUpper = 60;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 613. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoUserCollectionRepository.Enhancements.cs`
- **Dòng 18**: `85`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int ExpBranchTwoUpper = 85;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 614. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/MongoUserCollectionRepository.Enhancements.cs`
- **Dòng 19**: `95`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const int ExpBranchThreeUpper = 95;`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 615. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Revoke.cs`
- **Dòng 51**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (batchSize <= 0)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 616. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Rotate.Context.cs`
- **Dòng 17**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var contentionWindow = TimeSpan.FromSeconds(Math.Max(3, _authSecurityOptions.RefreshLockSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 617. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Rotate.Guards.cs`
- **Dòng 16**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var lockWindow = TimeSpan.FromSeconds(Math.Max(3, _authSecurityOptions.RefreshLockSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 618. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Rotate.Helpers.cs`
- **Dòng 70**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var window = TimeSpan.FromSeconds(Math.Max(10, _authSecurityOptions.RefreshIdempotencyWindowSeconds));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 619. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Rotate.Helpers.cs`
- **Dòng 96**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `LIMIT 1`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 620. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/UploadSessionRepository.cs`
- **Dòng 56**: `0, 100, 1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var normalizedLimit = limit <= 0 ? 100 : Math.Min(limit, 1000);`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 621. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/UserItemRepository.cs`
- **Dòng 13**: `"ix_inventory_item_use_operations_user_id_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string UseOperationUniqueConstraint = "ix_inventory_item_use_operations_user_id_idempotency_key";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 622. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/WalletRepository.ReleaseOperations.cs`
- **Dòng 101**: `"{idempotencyKey}_receiver"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `=> idempotencyKey == null ? null : $"{idempotencyKey}_receiver";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 623. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/WalletRepository.cs`
- **Dòng 92**: `"ix_wallet_transactions_idempotency_key"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `"ix_wallet_transactions_idempotency_key",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 624. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/SeedGamificationData.cs`
- **Dòng 36**: `"daily_1_reading"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Code = "daily_1_reading",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 625. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/SeedGamificationData.cs`
- **Dòng 42**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Rewards = new List<QuestRewardItem> { new() { Type = QuestRewardType.Gold, Amount = 50 } }`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 626. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/SeedGamificationData.cs`
- **Dòng 46**: `"daily_checkin"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Code = "daily_checkin",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 627. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/SeedGamificationData.cs`
- **Dòng 50**: `"daily_checkin"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `TriggerEvent = "daily_checkin",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 628. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/SeedGamificationData.cs`
- **Dòng 52**: `20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Rewards = new List<QuestRewardItem> { new() { Type = QuestRewardType.Gold, Amount = 20 } }`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 629. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.Models.cs`
- **Dòng 38**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `public static RewardTemplate Item(string rarity, int probabilityBasisPoints, string itemCode, int quantityGranted = 1)`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 630. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.Models.cs`
- **Dòng 63**: `"Diamond", "Gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var nameEn = normalizedCurrency == DiamondCurrency ? "Diamond" : "Gold";`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 631. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 8**: `"gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string GoldCurrency = "gold";`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 632. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 9**: `"diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private const string DiamondCurrency = "diamond";`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 633. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 34**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CostAmount: 500,`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 634. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 35**: `"gacha-pool-v1"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `OddsVersion: "gacha-pool-v1",`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 635. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 37**: `80`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `HardPityCount: 80,`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 636. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 55**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CostAmount: 50,`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 637. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 56**: `"gacha-pool-v1"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `OddsVersion: "gacha-pool-v1",`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 638. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 58**: `70`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `HardPityCount: 70,`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 639. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 76**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `CostAmount: 100,`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 640. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 77**: `"gacha-pool-v1"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `OddsVersion: "gacha-pool-v1",`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 641. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 79**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `HardPityCount: 50,`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 642. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 89**: `3500, 100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.CurrencyReward(GachaRarity.Common, 3500, GoldCurrency, 100),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 643. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 90**: `2300, 250`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.CurrencyReward(GachaRarity.Common, 2300, GoldCurrency, 250),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 644. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 91**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Rare, 1800, InventoryItemCodes.ExpBooster),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 645. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 92**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Rare, 1200, InventoryItemCodes.DefenseBooster),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 646. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 93**: `700`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 700, InventoryItemCodes.FreeDrawTicket3),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 647. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 94**: `300`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 300, InventoryItemCodes.FreeDrawTicket5),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 648. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 95**: `150`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Legendary, 150, InventoryItemCodes.FreeDrawTicket10),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 649. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 96**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Legendary, 50, InventoryItemCodes.RareTitleLuckyStar),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 650. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 104**: `1800, 1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.CurrencyReward(GachaRarity.Common, 1800, GoldCurrency, 1000),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 651. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 105**: `1700, 20`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.CurrencyReward(GachaRarity.Rare, 1700, DiamondCurrency, 20),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 652. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 106**: `1400`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Rare, 1400, InventoryItemCodes.PowerBooster),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 653. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 107**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 1200, InventoryItemCodes.DefenseBooster),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 654. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 108**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 1200, InventoryItemCodes.ExpBooster),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 655. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 109**: `900`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 900, InventoryItemCodes.FreeDrawTicket5),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 656. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 110**: `800`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Legendary, 800, InventoryItemCodes.FreeDrawTicket10),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 657. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 111**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Legendary, 1000, InventoryItemCodes.RareTitleLuckyStar),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 658. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 119**: `1200, 50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.CurrencyReward(GachaRarity.Rare, 1200, DiamondCurrency, 50),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 659. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 120**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Legendary, 1800, InventoryItemCodes.FreeDrawTicket10),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 660. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 121**: `2500`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Legendary, 2500, InventoryItemCodes.RareTitleLuckyStar),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 661. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 122**: `1100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 1100, InventoryItemCodes.PowerBooster),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 662. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 123**: `1100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 1100, InventoryItemCodes.DefenseBooster),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 663. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 124**: `1100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 1100, InventoryItemCodes.ExpBooster),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 664. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.PoolTemplates.cs`
- **Dòng 125**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `RewardTemplate.Item(GachaRarity.Epic, 1200, InventoryItemCodes.FreeDrawTicket5),`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 665. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GachaSeed.cs`
- **Dòng 116**: `10000`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (totalProbability != 10000)`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 666. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Achievements.cs`
- **Dòng 48**: `"streak_7"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Code = "streak_7",`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 667. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 15**: `"daily_checkin"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await context.Quests.DeleteManyAsync(x => x.Code == "daily_checkin");`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 668. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 46**: `"daily_reading_1"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Code = "daily_reading_1",`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 669. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 51**: `"daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `QuestType = "daily",`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 670. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 54**: `100, "gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 100 } }`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 671. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 65**: `7`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Target = 7,`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 672. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 66**: `1000, 5, "gold", "diamond"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 1000 }, new() { Type = "diamond", Amount = 5 } }`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 673. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 70**: `"daily_post_1"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Code = "daily_post_1",`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 674. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 75**: `"daily"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `QuestType = "daily",`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 675. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Seeds/GamificationSeed.Quests.cs`
- **Dòng 78**: `150, "gold"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Rewards = new List<QuestRewardItem> { new() { Type = "gold", Amount = 150 } }`
  Lý do cần có data này: Giá trị nghiệp vụ đang seed cứng trong source code nên mỗi lần chỉnh phải redeploy/migrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển sang bảng cấu hình nghiệp vụ (Pricing/Gacha/Quest/Promotion/Withdrawal) + cache invalidation.

### 676. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.cs`
- **Dòng 63**: `0, 30`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var timeoutSeconds = providerOptions.TimeoutSeconds > 0 ? providerOptions.TimeoutSeconds : 30;`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 677. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 18**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `AccessTokenBlacklistTtlSeconds = value.AccessTokenBlacklistTtlSeconds > 0`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 678. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 20**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `: 1200;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 679. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 21**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `SessionRevocationTtlSeconds = value.SessionRevocationTtlSeconds > 0`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 680. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 23**: `1800`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `: 1800;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 681. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 24**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `SessionCacheTtlSeconds = value.SessionCacheTtlSeconds > 0`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 682. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 26**: `30, 24, 60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `: 30 * 24 * 60 * 60;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 683. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 27**: `0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `ReplaySecurityRecordTtlSeconds = value.ReplaySecurityRecordTtlSeconds > 0`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 684. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/AuthSecuritySettings.cs`
- **Dòng 29**: `24, 60`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `: 24 * 60 * 60;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 685. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/DepositPayOsSettings.cs`
- **Dòng 19**: `0, 15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `LinkExpiryMinutes = value.LinkExpiryMinutes > 0 ? value.LinkExpiryMinutes : 15;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 686. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/JwtTokenSettings.cs`
- **Dòng 20**: `15`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `fallback: 15);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 687. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/JwtTokenSettings.cs`
- **Dòng 25**: `7`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `fallback: 7);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 688. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 18**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Spread3GoldCost = ResolveNonNegativeLong(config.Pricing.Spread3Gold, fallback: 50);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 689. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 19**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Spread3DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread3Diamond, fallback: 5);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 690. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 21**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Spread5GoldCost = ResolveNonNegativeLong(config.Pricing.Spread5Gold, fallback: 100);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 691. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 22**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Spread5DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread5Diamond, fallback: 10);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 692. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 24**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Spread10GoldCost = ResolveNonNegativeLong(config.Pricing.Spread10Gold, fallback: 500);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 693. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 25**: `50`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `Spread10DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread10Diamond, fallback: 50);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 694. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 28**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `DailyAiQuota = ResolvePositiveInt(config.DailyAiQuota, fallback: 3);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 695. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 29**: `3`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `InFlightAiCap = ResolvePositiveInt(config.InFlightAiCap, fallback: 3);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 696. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 30**: `30`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `ReadingRateLimitSeconds = ResolvePositiveInt(config.ReadingRateLimitSeconds, fallback: 30);`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 697. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 33**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `DailyCheckinGold = 5;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 698. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/Configuration/SystemConfigSettings.cs`
- **Dòng 34**: `24`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `StreakFreezeWindowHours = 24;`
  Lý do cần có data này: Giá trị mặc định đang nằm trong tầng cấu hình hệ thống (Options/AppSettings), có thể điều chỉnh theo môi trường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong AppSettings + Options; nếu cần thay đổi runtime theo campaign/khách hàng thì bổ sung nguồn DB/Feature Flag.

### 699. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/DiagnosticsService.cs`
- **Dòng 64**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `.Limit(5)`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 700. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/GamificationService.Activity.cs`
- **Dòng 17**: `1, "daily_checkin"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `new QuestProgressApplyRequest(activeQuests, "daily_checkin", 1, false),`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 701. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/GamificationService.Activity.cs`
- **Dòng 20**: `5`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await IncrementRankScoresAsync(userId, dailyPoints: 5, monthlyPoints: 5, lifetimePoints: 5, ct);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 702. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/GamificationService.Activity.cs`
- **Dòng 36**: `2, 0`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await IncrementRankScoresAsync(userId, dailyPoints: 2, monthlyPoints: 0, lifetimePoints: 0, ct);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 703. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/GamificationService.Activity.cs`
- **Dòng 49**: `"streak_reached"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `foreach (var quest in quests.Where(q => q.IsActive && q.TriggerEvent == "streak_reached" && currentStreak >= q.Target))`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 704. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/GamificationService.Activity.cs`
- **Dòng 73**: `0, "daily_rank_score"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `if (dailyPoints > 0) await _leaderboardRepo.IncrementScoreAsync(userId, "daily_rank_score", dailyPeriod, dailyPoints, ct);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 705. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/GamificationService.Reading.cs`
- **Dòng 21**: `10`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `await IncrementRankScoresAsync(userId, dailyPoints: 10, monthlyPoints: 10, lifetimePoints: 10, ct);`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 706. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/ReadinessService.cs`
- **Dòng 15**: `"gacha_pools", "auth_sessions", "refresh_tokens"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `private static readonly string[] RequiredPostgreSqlTables = ["users", "gacha_pools", "auth_sessions", "refresh_tokens"];`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 707. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/ReadingSessionOrchestrator.Rollback.cs`
- **Dòng 77**: `"refund_rollback_{sessionId}_{currency}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `idempotencyKey: $"refund_rollback_{sessionId}_{currency}",`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 708. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/ReadingSessionOrchestrator.cs`
- **Dòng 44**: `"read_{request.Session.Id}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `var idempotencyKey = $"read_{request.Session.Id}";`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 709. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/ReadingSessionOrchestrator.cs`
- **Dòng 78**: `"start_paid_session_failed"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return (false, "start_paid_session_failed");`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.

### 710. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/ReadingSessionOrchestrator.cs`
- **Dòng 105**: `"Tarot_{instruction.SpreadType}"`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `$"Tarot_{instruction.SpreadType}",`
  Lý do cần có data này: Giá trị nghiệp vụ ảnh hưởng trực tiếp tới cân bằng kinh tế/tính năng đang hard-coded trong logic.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: Ưu tiên đưa vào DB cấu hình nghiệp vụ + cache để có thể tuning realtime mà không redeploy.

### 711. File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/TotpMfaService.cs`
- **Dòng 84**: `2`
  Ý nghĩa / ngữ cảnh sử dụng: Giá trị được dùng trực tiếp trong biểu thức/thiết lập: `return totp.VerifyTotp(code, out _, new VerificationWindow(2, 2));`
  Lý do cần có data này: Giá trị vận hành (timeout/retry/rate/TTL/interval) đang hard-coded trong luồng xử lý runtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển vào AppSettings + Options (hoặc Feature Flag) để tinh chỉnh theo môi trường vận hành.
