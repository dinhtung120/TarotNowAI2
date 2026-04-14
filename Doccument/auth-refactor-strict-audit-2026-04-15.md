# Auth Refactor Strict Audit - 2026-04-15

## 1. Overall Risk Assessment

- Điểm tổng thể: `N/A` theo tiêu chí của bạn (chưa đủ điều kiện chấm điểm vì vẫn còn lỗi critical). Nếu quy đổi tham chiếu nội bộ hiện trạng: `6.1/10`.
- Còn khoảng `24%` rủi ro so với kế hoạch refactor ban đầu.
- High risk còn lại khoảng `13%`.
- Medium risk còn lại khoảng `8%`.
- Low risk còn lại khoảng `3%`.
- Code này có thể deploy production ngay không: `Không`.
- Lý do 1: Luồng `serverAuth` đang refresh trực tiếp từ Server Component/Route helper nhưng không đồng bộ `Set-Cookie` về browser, có thể gây replay/logout ngẫu nhiên. Bằng chứng: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/auth/serverAuth.ts:36-66`.
- Lý do 2: Login brute-force theo identity đang phụ thuộc event handler outbox async, có độ trễ thực thi nên có cửa sổ bypass nhanh. Bằng chứng: publish event ở `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/Login/LoginCommandHandler.cs:55-57`, xử lý tăng counter ở `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/LoginFailedDomainEventHandler.cs:38-44`, worker poll mỗi 5s tại `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox/OutboxProcessorWorker.cs:13`.
- Lý do 3: Middleware protect route chỉ kiểm tra presence/exp của JWT, không verify signature nên fake token vẫn vượt middleware gate (dù backend vẫn chặn data). Bằng chứng: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/proxy.ts:79-105`, `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/proxy.ts:359-395`.
- Lý do 4: Migration snapshot EF không đồng bộ với schema auth mới, rủi ro cao cho vòng migration kế tiếp. Bằng chứng thiếu hoàn toàn `auth_sessions`/cột refresh mới trong `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Migrations/ApplicationDbContextModelSnapshot.cs` (đoạn refresh token cũ ở `:782-829`).

### Technical Debt còn sót lại

- Critical:
- `serverAuth` refresh không ghi cookie mới về client, gây rotation lệch trạng thái cookie.
- High:
- Brute-force lock theo identity có độ trễ outbox, không realtime.
- Middleware guard chưa verify JWT signature.
- EF `ApplicationDbContextModelSnapshot` stale.
- Medium:
- Revoke-all phụ thuộc Redis session index để blacklist JTI, có thể bỏ sót session khi index mất/evict.
- Refresh route/network error mapping ở client vẫn có nhánh trả `AUTH_UNAUTHORIZED` cho lỗi transient.
- Low:
- Regex parse `Max-Age` trong BFF cookie parser dùng `\\d` trong regex literal, logic parse không đúng như mong muốn.
- Chính sách rate-limit còn policy `login` legacy trùng `auth-login`.

---

## 2. Security Deep Audit

### Cookie policy

- Kết quả: `Chưa đạt 100% end-to-end`.
- Backend cookie auth đang đúng yêu cầu tại `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Services/AuthCookieService.cs:42-47`:
- `HttpOnly=true` (access/refresh).
- `Secure=true`.
- `SameSite=Strict`.
- `Path=/`.
- `MaxAge` theo giây.
- Frontend BFF cookie set cũng đúng strict cho access/refresh tại `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/auth/_shared.ts:116-143`.
- Vấn đề còn lại:
- `serverAuth` refresh từ server helper không set cookie về browser nên state cookie mất đồng bộ dù policy đúng về thuộc tính.
- Parser `Max-Age` có bug regex tại `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/auth/_shared.ts:86`.

### JWT claims

- `sid`: Có, ở `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Security/JwtTokenService.cs:52`.
- `jti`: Có, ở `:39` và `:51`.
- `exp`: Có qua `expires` trong `JwtSecurityToken` ở `:61`.
- `nbf`: Có qua `notBefore` ở `:60`.
- Leak thông tin nhạy cảm: Không thấy claim nhạy cảm trực tiếp.
- Lưu ý: middleware FE không verify signature khi gate protected route.

### Refresh token rotation (atomic/idempotent/replay)

- Atomic DB transaction: Có `Serializable` + `FOR UPDATE`.
- Bằng chứng transaction: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Rotate.cs:84-89`.
- Bằng chứng lock row: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Rotate.Helpers.cs:75-87`.
- Distributed lock: Có, dùng Redis `SET NX` owner lock.
- Bằng chứng: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Services/RedisCacheService.cs:229-295`.
- Idempotency cache: Có `session-key` và `token-key`.
- Bằng chứng: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Rotate.Helpers.cs:106-115`.
- Replay protection: Có, revoke family/session + publish event.
- Bằng chứng: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/RefreshToken/RefreshTokenCommandHandler.Helpers.cs:79-119`.
- Điểm yếu:
- Nếu Redis unavailable, lock/idempotency fallback memory cache không đảm bảo multi-instance consistency.

### Brute-force / Rate limiting

- IP limiter login: Có ở `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.cs:36-37`.
- Refresh limiter: Có ở `:41-42`.
- Identity limiter login: Có logic qua cache key hash identity trong `LoginCommandThrottleBehavior`.
- Bằng chứng: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/Login/LoginCommandThrottleBehavior.cs:41-50`.
- Lỗ hổng:
- Identity counter được tăng async qua outbox nên có cửa sổ bypass.
- Hash identity không normalize lowercase, có thể bypass theo case-variation.

### Replay attack scenarios (3 kịch bản)

- Kịch bản 1: Reuse cùng refresh token sau khi rotate thành công.
- Trạng thái: Chặn được.
- Luồng: token không active -> `ReplayDetected` -> revoke family/session.
- Kịch bản 2: 2 request đồng thời cùng idempotency key.
- Trạng thái: Chặn tốt phần lớn, trả idempotent hoặc locked retry.
- Kịch bản 3: Multi-instance khi Redis down (fallback memory).
- Trạng thái: Chưa chặn đủ trong scale-out; lock/idempotency có thể lệch giữa instance.

### CSRF & SameSite Strict

- `SameSite=Strict` + cookie HttpOnly giúp giảm CSRF cross-site mạnh.
- Edge case còn lại:
- Same-site subdomain attack vẫn cần cân nhắc nếu có subdomain không tin cậy.
- Chưa có anti-CSRF token cho state-changing endpoint (logout/refresh/login qua BFF).

### Multi-device session isolation

- Có `AuthSession` per device và unique active index user+device.
- Bằng chứng: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AuthSessionConfiguration.cs:41-43`.
- Vấn đề:
- Cùng device login lại có thể tạo family refresh mới mà chưa revoke family cũ của cùng session/device ngay lúc login.

---

## 3. Audit chi tiết TỪNG SECTION (BE-01 đến BE-16 + FE-01 đến FE-15)

### Section: BE-01 AuthResponse.cs

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Dùng `ExpiresInSeconds` rõ đơn vị giây.
- DTO gọn, không magic string cho token type (mặc định Bearer).
- Vấn đề phát hiện:
- Không phát hiện lỗi spec trong phạm vi section.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: BE-02 LoginCommand.cs

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Có metadata `ClientIpAddress`, `DeviceId`, `UserAgentHash`.
- Có `[JsonIgnore]` cho metadata internal.
- Vấn đề phát hiện:
- Không có lỗi trực tiếp trong section.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: BE-03 RefreshTokenCommand.cs

- Trạng thái: ⚠️ Có vấn đề nhỏ
- Điểm đã đúng:
- Có `IdempotencyKey`, `DeviceId`, `UserAgentHash`.
- Vấn đề phát hiện:
- Khác BE-02, metadata không gắn `[JsonIgnore]` (dù controller đang build command thủ công).
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp ở section này.
- Edge case chưa được xử lý:
- Nếu sau này bind trực tiếp từ body mà không kiểm soát, metadata có thể bị client ghi đè.
- Gợi ý fix chi tiết:
- Bổ sung `[JsonIgnore]` cho các field metadata internal để khóa contract.

### Section: BE-04 RefreshToken.cs

- Trạng thái: ⚠️ Có vấn đề nhỏ
- Điểm đã đúng:
- Có family/parent/replaced/used/revoked/reason.
- Có `MarkUsed`, `MarkCompromised`, `MatchesToken`, hash token SHA-256.
- Vấn đề phát hiện:
- `IsExpired` dùng `DateTime.UtcNow` trực tiếp (`/Backend/src/TarotNow.Domain/Entities/RefreshToken.cs:100`) làm giảm testability và khó inject clock policy.
- Code thừa / code cũ / legacy còn sót:
- Dual-compat `Token` có thể chứa raw legacy hoặc hash là cần cho migration, không phải rác.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp, race đã xử lý ở repository lock/transaction.
- Edge case chưa được xử lý:
- Không có `clock skew` abstraction ở domain logic.
- Gợi ý fix chi tiết:
- Đổi `IsExpired` sang hàm nhận `nowUtc` để deterministic trong test và policy.

### Section: BE-05 AuthSession.cs

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Session theo thiết bị với `DeviceId`, `UserAgentHash`, `LastIpHash`, revoke/touch.
- Vấn đề phát hiện:
- Không có lỗi spec trực tiếp.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: BE-06 IRefreshTokenRepository.cs

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Có `RotateAsync`, `RevokeFamilyAsync`, `RevokeSessionAsync`.
- Có status enum phân biệt success/idempotent/invalid/expired/replay/locked.
- Vấn đề phát hiện:
- Interface mở rộng thêm `AddAsync`, `UpdateAsync`, `RevokeAllByUserIdAsync` là hợp lý nhưng khác bản kế hoạch tối giản.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: BE-07 RefreshTokenRepository.cs

- Trạng thái: ⚠️ Có vấn đề nhỏ
- Điểm đã đúng:
- Có distributed lock.
- Có transaction `Serializable`.
- Có `SELECT ... FOR UPDATE`.
- Có idempotency cache hai lớp.
- Vấn đề phát hiện:
- Fallback `DistributedMemoryCache` không đảm bảo lock/idempotency cross-instance (`/Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs:45-49`).
- Với production multi-node, nếu Redis lỗi sẽ giảm mức bảo vệ replay/race.
- Code thừa / code cũ / legacy còn sót:
- Logic dual-read hashed/raw token vẫn còn để migration compatibility, không phải code rác.
- Race condition / concurrency bug tiềm ẩn:
- Khi lock hết hạn sớm hơn giao dịch lâu bất thường, request khác có thể chen vào.
- Edge case chưa được xử lý:
- Legacy token `sessionId=Guid.Empty` collision key theo `legacy` vẫn tồn tại về lý thuyết nếu bỏ qua bước upgrade trước rotate.
- Gợi ý fix chi tiết:
- Production mode: fail-closed cho refresh khi Redis unavailable thay vì fallback memory.
- Tăng hardening lock lease + auto-renew (nếu cần).

### Section: BE-08 IAuthSessionRepository + AuthSessionRepository.cs

- Trạng thái: ⚠️ Có vấn đề nhỏ
- Điểm đã đúng:
- Create/get/revoke/revoke-all đầy đủ.
- Có xử lý `DbUpdateException` khi race create cùng device.
- Vấn đề phát hiện:
- Chưa có transactional method revoke old token family theo cùng session/device tại login.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Đã giảm đáng kể nhờ unique index + catch/reload.
- Edge case chưa được xử lý:
- Cùng device login nhiều lần có thể tồn tại nhiều refresh family active nếu business policy muốn 1 family/device.
- Gợi ý fix chi tiết:
- Khi login thành công cùng device: revoke toàn bộ family active cũ của session trước khi cấp family mới.

### Section: BE-09 JwtTokenService.cs

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Có `sid`, `jti`, `exp`, `nbf`.
- TTL access ngắn theo config.
- Vấn đề phát hiện:
- Không có lỗi critical trong service phát token.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Optional: bổ sung `iat` explicit claim nếu muốn trace sâu hơn.

### Section: BE-10 AuthSessionController.cs

- Trạng thái: ⚠️ Có vấn đề nhỏ
- Điểm đã đúng:
- Nhận `x-idempotency-key`, `x-device-id`.
- Hash user-agent.
- Set cookie tập trung qua cookie service.
- Có rate-limit policy cho login/refresh/logout.
- Vấn đề phát hiện:
- `UnauthorizedProblem` không gắn `errorCode` chuẩn trong controller extension path, khiến FE parsing đôi lúc phải fallback message.
- Code thừa / code cũ / legacy còn sót:
- Magic message `"Logged out successfully."` tại `/Backend/src/TarotNow.Api/Controllers/AuthSessionController.cs:124`.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp.
- Edge case chưa được xử lý:
- Không có explicit anti-CSRF token cho các endpoint state-changing.
- Gợi ý fix chi tiết:
- Dùng `ProblemDetails.Extensions["errorCode"]` nhất quán cho nhánh 401/400 trong controller helper.

### Section: BE-11 IAuthCookieService + AuthCookieService.cs

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Access/Refresh cookie strict security (`HttpOnly`, `Secure`, `SameSite=Strict`, `Path=/`, `MaxAge`).
- Clear cookie tập trung một chỗ.
- Vấn đề phát hiện:
- `request` parameter chưa dùng ở implementation.
- Code thừa / code cũ / legacy còn sót:
- `request` arg có thể bỏ nếu không cần domain/path dynamic.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Local non-HTTPS dev sẽ không nhận cookie `Secure=true` (đúng security, nhưng cần env note).
- Gợi ý fix chi tiết:
- Có thể rút gọn signature `SetAccessToken(HttpResponse...)` nếu không dùng `request`.

### Section: BE-12 ApiServiceCollectionExtensions.RateLimit.cs

- Trạng thái: ❌ Sai spec
- Điểm đã đúng:
- Có policy riêng `auth-login`, `auth-refresh`, `auth-logout`.
- Không tin trực tiếp `X-Forwarded-For`.
- Vấn đề phát hiện:
- Chưa có limiter refresh theo token-family như bản kế hoạch.
- Login identity limiter thực thi async qua outbox (không realtime).
- Hash identity ở throttle không normalize lowercase, có thể bypass theo case.
- Code thừa / code cũ / legacy còn sót:
- Policy `login` và `auth-login` đang trùng mục đích.
- Race condition / concurrency bug tiềm ẩn:
- Cửa sổ bypass brute-force trước khi outbox handler cập nhật counter.
- Edge case chưa được xử lý:
- Nếu ForwardedHeaders chưa enable đúng môi trường proxy, IP partition có thể lệch.
- Gợi ý fix chi tiết:
- Đưa identity lockout path sang sync redis increment ở pipeline login (không chờ outbox).
- Thêm normalize lowercase cho identity hash.

### Section: BE-13 Domain Events Auth mới

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Đủ 5 event chính: login, refresh, logout, replay, login-failed.
- Payload có session/device/iphash/jti theo yêu cầu.
- Vấn đề phát hiện:
- Namespace không tách `.Auth` nhưng không ảnh hưởng chức năng.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp ở event contracts.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Optional: chuẩn hóa namespace folder và namespace cho dễ maintain.

### Section: BE-14 Auth Security Event Handlers

- Trạng thái: ❌ Sai spec
- Điểm đã đúng:
- Side-effects đặt ở event handlers.
- Handlers idempotent qua outbox handler state.
- Có Redis session index, blacklist, session revoked key, OTel spans.
- Vấn đề phát hiện:
- Login failed counter bị trễ vì outbox async, không đáp ứng brute-force protection tức thời.
- Revoke-all logout phụ thuộc `auth:user-sessions:{userId}` cache index; nếu index thiếu thì không blacklist đủ JTI tất cả session.
- Code thừa / code cũ / legacy còn sót:
- Không đáng kể.
- Race condition / concurrency bug tiềm ẩn:
- Outbox ordering có thể làm `clear counter` chạy trước `increment failure`, dẫn tới ghost lockout.
- Edge case chưa được xử lý:
- Cache eviction làm revoke-all không đủ coverage access-token blacklist.
- Gợi ý fix chi tiết:
- Tách login-fail counter path thành synchronous security write tại command pipeline.
- Với revoke-all: đọc session list từ DB và mark revoked key/blacklist cho từng session ngay trong transaction-scope command hoặc synchronous post-commit worker có guaranteed replay.

### Section: BE-15 RefreshTokenConfiguration + AuthSessionConfiguration

- Trạng thái: ⚠️ Có vấn đề nhỏ
- Điểm đã đúng:
- Index/unique cần thiết đã có.
- `RevocationReason` max length và family/session indexes phù hợp.
- Vấn đề phát hiện:
- `ApplicationDbContextModelSnapshot` không phản ánh config auth mới, gây lệch migration state.
- Code thừa / code cũ / legacy còn sót:
- Không trong config file, nhưng snapshot cũ là technical debt lớn.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Regenerate migration snapshot chuẩn sau khi xác nhận schema hiện tại.

### Section: BE-16 Migrations (Refactor + Backfill)

- Trạng thái: ❌ Sai spec
- Điểm đã đúng:
- Migration additive + backfill legacy session.
- Có unique active session index migration riêng.
- Vấn đề phát hiện:
- Snapshot EF không cập nhật theo migration auth mới.
- Một phần migration được viết thủ công nhưng thiếu đồng bộ designer/snapshot cho chuỗi migration tương lai.
- Code thừa / code cũ / legacy còn sót:
- Không drop dữ liệu cũ là đúng zero-downtime, không xem là thừa.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp runtime, nhưng migration drift sẽ gây rủi ro ở release sau.
- Edge case chưa được xử lý:
- Không có automated validation step kiểm tra schema drift trong CI.
- Gợi ý fix chi tiết:
- Tạo migration no-op để sync snapshot hoặc regenerate migration chain ở nhánh migration maintenance.

### Section: FE-01 authConstants.ts

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Hằng số cookie/header/session tập trung, bỏ magic string chính.
- Vấn đề phát hiện:
- Không.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: FE-02 authRoutes.ts

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Bao phủ đầy đủ prefix `(user)` + `/admin`.
- Vấn đề phát hiện:
- Không.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: FE-03 proxy.ts

- Trạng thái: ❌ Sai spec
- Điểm đã đúng:
- Middleware protect toàn bộ protected prefixes.
- Có auto refresh trước render khi thiếu/sắp hết hạn access.
- Có copy `Set-Cookie` từ refresh route.
- Vấn đề phát hiện:
- Chỉ parse `exp` từ JWT để quyết định auth, không verify chữ ký -> fake JWT vẫn qua gate middleware.
- Khi refresh fail transient, middleware vẫn cho đi tiếp protected route (`/Frontend/src/proxy.ts:380-385`) có thể gây behavior không nhất quán (đang auth-route nhưng backend fail 401 sau đó).
- Code thừa / code cũ / legacy còn sót:
- Không rõ code thừa lớn.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp.
- Edge case chưa được xử lý:
- Token giả có `exp` xa tương lai.
- Gợi ý fix chi tiết:
- Đổi guard strategy: nếu token không verify được hoặc không có session snapshot xác thực thì redirect login sớm cho protected SSR.

### Section: FE-04 app/api/auth/refresh/route.ts

- Trạng thái: ⚠️ Có vấn đề nhỏ
- Điểm đã đúng:
- Có idempotency key + device id propagation.
- Có phân nhánh terminal auth failure vs transient failure.
- Vấn đề phát hiện:
- Phụ thuộc parser cookie metadata ở `_shared.ts` đang có regex bug `max-age`.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không trực tiếp.
- Edge case chưa được xử lý:
- Trường hợp backend không gửi refresh `Set-Cookie` (upstream anomaly) chưa fail-fast rõ ràng.
- Gợi ý fix chi tiết:
- Validate bắt buộc refresh cookie rotation header ở nhánh thành công.

### Section: FE-05 app/api/auth/login/route.ts

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- BFF login set access/refresh/device cookies chuẩn.
- Không expose access token về client body.
- Vấn đề phát hiện:
- Không có lỗi spec chính.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: FE-06 app/api/auth/logout/route.ts

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Revoke BE + clear cookie local luôn.
- Propagate device/user-agent headers.
- Vấn đề phát hiện:
- Không critical.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: FE-07 shared/infrastructure/auth/serverAuth.ts

- Trạng thái: 🔴 Lỗi nghiêm trọng
- Điểm đã đúng:
- Có logic đọc access token từ cookie và refresh fallback.
- Vấn đề phát hiện:
- Gọi thẳng backend `/auth/refresh` từ helper server nhưng không set cookie mới về browser (`/Frontend/src/shared/infrastructure/auth/serverAuth.ts:36-66`).
- Điều này tạo split-state: backend đã rotate refresh token, browser vẫn giữ refresh cũ -> request sau dễ đi vào replay detected/logout.
- Code thừa / code cũ / legacy còn sót:
- Đây là legacy pattern pre-BFF, không còn phù hợp với cookie rotation flow mới.
- Race condition / concurrency bug tiềm ẩn:
- Server render song song có thể gọi refresh nhiều lần theo cùng cookie cũ.
- Edge case chưa được xử lý:
- Public route có auth cookie vẫn có thể trigger refresh âm thầm.
- Gợi ý fix chi tiết:
- Tuyệt đối không refresh trực tiếp trong `serverAuth`.
- Chỉ refresh tại middleware/BFF route nơi có thể set cookie outbound.
- `getServerAccessToken()` chỉ nên đọc access cookie hiện tại; hết hạn thì trả `undefined`.

### Section: FE-08 features/auth/application/actions/session.ts

- Trạng thái: ❌ Sai spec
- Điểm đã đúng:
- Dùng internal BFF auth routes.
- Dùng `credentials: 'include'`.
- Có single-flight `refreshPromise`.
- Vấn đề phát hiện:
- Catch network error đang trả `AUTH_UNAUTHORIZED` cho refresh (`/Frontend/src/features/auth/application/actions/session.ts:141-143`) và login (`:90-92`), dễ kích hoạt logout sai trong lỗi transient.
- Code thừa / code cũ / legacy còn sót:
- Không token store legacy.
- Race condition / concurrency bug tiềm ẩn:
- Không lớn nhờ `refreshPromise`.
- Edge case chưa được xử lý:
- Network drop/timeouts nên map `AUTH_TEMPORARY_FAILURE`.
- Gợi ý fix chi tiết:
- Đổi catch path của refresh/login thành `AUTH_TEMPORARY_FAILURE` cho lỗi hạ tầng.

### Section: FE-09 store/authStore.ts

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Không còn token/localStorage auth split-brain.
- Chỉ giữ `user` + `isAuthenticated`.
- Vấn đề phát hiện:
- Không có lỗi spec chính.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: FE-10 app/[locale]/layout.tsx

- Trạng thái: ❌ Sai spec
- Điểm đã đúng:
- Hydrate auth từ server snapshot.
- Có check `hasAuthCookies` trước khi gọi session snapshot.
- Vấn đề phát hiện:
- Vẫn gọi `getServerSessionSnapshot()` trên mọi trang có auth cookie (`/Frontend/src/app/[locale]/layout.tsx:53-58`), kéo theo rủi ro critical ở FE-07 (refresh âm thầm không set cookie).
- Code thừa / code cũ / legacy còn sót:
- Không thừa, nhưng coupling sai với `serverAuth` hiện tại.
- Race condition / concurrency bug tiềm ẩn:
- Nếu nhiều request layout parallel, nguy cơ refresh chồng.
- Edge case chưa được xử lý:
- Public route + access sắp hết hạn.
- Gợi ý fix chi tiết:
- Tạm dừng refresh từ layout path; chỉ hydrate từ `/api/auth/session` sau khi FE-07 sửa an toàn cookie.

### Section: FE-11 shared/infrastructure/http/clientJsonRequest.ts

- Trạng thái: ⚠️ Có vấn đề nhỏ
- Điểm đã đúng:
- `credentials: 'include'`.
- Retry 1 lần sau refresh khi 401.
- Vấn đề phát hiện:
- Chỉ retry cho 401, chưa xử lý 403 session-revoked path.
- Không phân loại rõ transient refresh failure.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không đáng kể vì refreshClient single-flight.
- Edge case chưa được xử lý:
- Backend trả 401 do replay/session revoked cần nhánh UX riêng.
- Gợi ý fix chi tiết:
- Mở rộng handling 403 + errorCode terminal để clear auth chuẩn.

### Section: FE-12 Realtime hooks (useChatRealtimeSync, usePresenceConnection, useChatSignalRLifecycle)

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Bỏ token từ store.
- Dùng `.withUrl(..., { withCredentials: true })` cho SignalR.
- Vấn đề phát hiện:
- Không lỗi spec auth chính.
- Code thừa / code cũ / legacy còn sót:
- Không thấy `accessTokenFactory`.
- Race condition / concurrency bug tiềm ẩn:
- Không liên quan auth security chính.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: FE-13 features/gamification/useAdminGamification.ts

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Đã chuyển sang cookie auth `credentials: 'include'`.
- Không dùng bearer từ Zustand.
- Vấn đề phát hiện:
- Không có lỗi auth spec chính.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: FE-14 shared/domain/authErrors.ts

- Trạng thái: ✅ Hoàn hảo
- Điểm đã đúng:
- Chuẩn hóa AUTH error constants.
- Có helper phân loại terminal vs non-terminal.
- Vấn đề phát hiện:
- Không.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không.
- Edge case chưa được xử lý:
- Không.
- Gợi ý fix chi tiết:
- Không cần.

### Section: FE-15 Test files

- Trạng thái: ❌ Sai spec
- Điểm đã đúng:
- Có test store mới không token.
- Có e2e smoke routes unauth redirect.
- Có test middleware không redirect login khi có cookie.
- Vấn đề phát hiện:
- Chưa có test concurrent refresh thực sự với backend thật.
- Chưa có test replay attack thực sự.
- `auth-session-refresh.spec.ts` dùng JWT unsigned giả, chỉ kiểm tra redirect, không validate rotation/idempotency.
- Chưa có test legacy-token-upgrade.
- Code thừa / code cũ / legacy còn sót:
- Không.
- Race condition / concurrency bug tiềm ẩn:
- Không được test coverage.
- Edge case chưa được xử lý:
- Network timeout/drop khi refresh.
- Gợi ý fix chi tiết:
- Bổ sung integration test backend cho rotate/idempotency/replay + e2e multi-tab refresh race.

---

## 4. Cross-File & Cross-Layer Consistency Check

- Protected routes trong `proxy.ts` + `authRoutes.ts` có bao quát 100% `(user)` segment + admin không: `Có`, đã bao quát theo prefix.
- Danh sách `(user)` thực tế đã kiểm tra: `/reader`, `/reading`, `/collection`, `/chat`, `/gamification`, `/leaderboard`, `/gacha`, `/profile`, `/wallet`, `/readers`, `/community`, `/premium`, `/notifications`.
- `expiresInSeconds` đã thống nhất giây ở BE + FE + cookie + client chưa: `Gần như có`, phần chính đã đúng.
- Điểm chưa ổn: parser `max-age` BFF regex bug (`/Frontend/src/app/api/auth/_shared.ts:86`) có thể bỏ qua max-age parse.
- AuthStore/localStorage/Zustand token đã bị xóa sạch chưa: `Có` cho auth token.
- Còn localStorage cho `deviceId` là chủ đích, không phải auth token.
- Tất cả client fetch/SignalR/admin hooks đã dùng cookie include và bỏ bearer hoàn toàn chưa:
- Client-side realtime/admin đã chuyển tốt.
- Server-side actions vẫn dùng bearer nội bộ (`getServerAccessToken -> Authorization`) là chấp nhận được, nhưng helper refresh hiện tại gây lỗi critical cookie rotation.
- Domain Events + Outbox handlers có tuân thủ Rule 0 không:
- Side-effects auth đặt ở handlers/outbox là đúng hướng.
- Nhưng security counter login-fail phụ thuộc async outbox làm giảm hiệu lực bảo vệ brute-force realtime.
- Idempotency-Key, Device-Id, User-Agent-Hash propagate đúng luồng chưa:
- `Middleware -> BFF -> BE`: Có.
- `Client refresh -> BFF -> BE`: Có.
- `ServerAuth`: Có header nhưng dùng sai điểm refresh (không set cookie response).
- Migration & zero-downtime dual-read legacy token có đúng chưa:
- Có dual-read hashed/raw + backfill migration + legacy binding helper.
- Chưa ổn phần migration snapshot sync.

---

## 5. Code Quality & Maintainability Deep Check

- Magic string/hard-coded còn sót:
- Redis key strings vẫn hard-coded ở nhiều file auth handlers/repositories thay vì constants tập trung.
- Message string hard-coded ở một số nhánh controller/auth actions.
- Naming/comment/error handling:
- Tổng thể rõ ràng, nhưng comment quá dài ở một số file gây nhiễu review.
- Error mapping chưa đồng nhất 100% cho transient network vs unauthorized.
- Performance issue tiềm ẩn:
- `layout.tsx` gọi server session snapshot trên mọi route có auth cookies.
- `serverAuth` refresh gọi backend trực tiếp có thể tạo refresh churn không cần thiết.
- Middleware refresh có thể tăng latency khi token cận hạn (đây là tradeoff hợp lý nếu flow đúng).
- Code thừa/duplicate logic:
- Legacy query-token fallback ở JWT bearer resolve (`/Backend/src/TarotNow.Infrastructure/DependencyInjection.Auth.cs:79-95`) vẫn tồn tại dù mục tiêu cookie-auth cho realtime web.
- `login` rate-limit policy trùng `auth-login`.
- Phụ thuộc code cũ chưa refactor:
- `serverAuth` là điểm phụ thuộc pattern cũ (server-side refresh trực tiếp).
- EF snapshot cũ là technical debt migration pipeline.

---

## 6. Test & Verification Plan (nâng cao)

### 8 test case nguy hiểm nhất cần chạy ngay

1. Protected route SSR + access gần hết hạn + refresh trong middleware.
- Kỳ vọng: Không redirect login, cookie access+refresh rotate đúng.

2. Public route có auth cookie + access gần hết hạn.
- Kỳ vọng: Không được rotate refresh từ server helper nếu không set-cookie được.

3. Concurrent refresh cùng `idempotency-key` (2 tab cùng lúc).
- Kỳ vọng: 1 rotation, 1 idempotent response, không replay.

4. Concurrent refresh khác `idempotency-key` cùng refresh token.
- Kỳ vọng: 1 success, request còn lại replay/locked theo policy.

5. Reuse refresh token cũ sau rotate.
- Kỳ vọng: Revoke family/session + event replay_detected + reject tiếp theo.

6. Login brute-force burst 20 req trong <5s theo cùng identity.
- Kỳ vọng: Bị chặn đúng hạn mức identity ngay, không phụ thuộc delay outbox.

7. Revoke all devices khi session index Redis bị xóa.
- Kỳ vọng: Tất cả access token cũ bị invalidate theo sid/jti ngay.

8. Network drop khi client refresh.
- Kỳ vọng: Không auto logout, trả `AUTH_TEMPORARY_FAILURE`, retry hợp lý.

### Test cần bổ sung

- Backend integration test cho `RefreshTokenRepository.RotateAsync` với Postgres + Redis thật.
- Backend integration test login brute-force timing (outbox delay).
- Frontend e2e test middleware refresh thực với backend thật (không dùng JWT giả unsigned).
- Frontend e2e test `serverAuth` không gây replay khi vào public pages.

---

## 7. Final Recommendation & Priority Fix List

### Top 5 vấn đề nguy hiểm nhất cần fix NGAY

1. Sửa `serverAuth` để không refresh trực tiếp từ server helper khi không thể set cookie outbound.
- File chính: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/auth/serverAuth.ts`.

2. Sửa `layout/session` flow để không trigger silent refresh trên public routes theo pattern gây rotate lệch cookie.
- File chính: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/[locale]/layout.tsx`.

3. Chuyển login fail counter sang synchronous security path (không phụ thuộc outbox delay cho throttle realtime).
- Files: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/Features/Auth/Commands/Login/LoginCommandThrottleBehavior.cs`, `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Application/DomainEvents/Handlers/Auth/LoginFailedDomainEventHandler.cs`.

4. Hardening middleware protected-route gate để không tin JWT chưa verify signature.
- File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/proxy.ts`.

5. Sync lại EF migration snapshot để tránh drift release kế tiếp.
- File: `/Users/lucifer/Desktop/TarotNowAI2/Backend/src/TarotNow.Infrastructure/Migrations/ApplicationDbContextModelSnapshot.cs`.

### Những thay đổi nhỏ nhưng quan trọng còn sót

- Đổi network catch trong `refreshAccessTokenAction` sang `AUTH_TEMPORARY_FAILURE`.
- Fix regex `max-age` parser trong BFF `_shared.ts`.
- Bỏ policy `login` trùng hoặc document rõ purpose.
- Xem xét bỏ query-token fallback trong JwtBearer cho web flows nếu không còn dùng.

### Kết luận sau khi fix các vấn đề trên

- Sau khi xử lý 5 mục ưu tiên + bổ sung test integration/e2e nêu trên, hệ thống có thể đạt production-grade cho auth/session flow.
- Hiện tại: chưa đạt tiêu chí `zero-critical-bug`.

