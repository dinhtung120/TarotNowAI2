# DANH SÁCH TOÀN BỘ HARD-CODED VALUES TRONG FRONTEND

## Tổng quan
- Số lượng hard-coded values tunable tìm thấy: 186
- Số file bị ảnh hưởng: 83
- Phạm vi lọc: chỉ giữ các hard-code có ý nghĩa vận hành/business (timeout, retry, TTL, page size, pricing/economy, limit, policy, upload constraints, realtime settings); đã loại bỏ phần lớn literal UI thuần style/className.
- Khuyến nghị:
  - Đưa 140+ giá trị có thể đổi runtime vào `system_configs` (source-of-truth chung với BE).
  - Giữ ~30 giá trị technical guardrail ở `constants` (ví dụ min timeout floor, hard security cap).
  - Giữ ~10 giá trị domain invariant ở code (ví dụ 78 lá bài tarot), chỉ tài liệu hóa rõ lý do.

## Danh sách chi tiết

### 1. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/next.config.ts`
- **Dòng 90**: `'443'` và `'80'`
  Ý nghĩa / ngữ cảnh sử dụng: Port mặc định khi build `allowedOrigins` cho server actions.
  Lý do cần có data này: Quy tắc kết nối origin theo protocol.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa sang env (`NEXT_DEFAULT_HTTPS_PORT`, `NEXT_DEFAULT_HTTP_PORT`) hoặc helper dùng chuẩn platform.

- **Dòng 183**: `10485760`
  Ý nghĩa / ngữ cảnh sử dụng: `serverActions.bodySizeLimit` = 10MB.
  Lý do cần có data này: Giới hạn payload cho server actions.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đọc từ env/system config để tune theo môi trường deploy.

- **Dòng 187**: `300`
  Ý nghĩa / ngữ cảnh sử dụng: `experimental.staleTimes.dynamic`.
  Lý do cần có data này: Ảnh hưởng mức cache của dynamic RSC.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa thành env hoặc `system_configs` nhóm `operational.cache`.

- **Dòng 188**: `600`
  Ý nghĩa / ngữ cảnh sử dụng: `experimental.staleTimes.static`.
  Lý do cần có data này: Ảnh hưởng cache page static/hybrid.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách cấu hình theo môi trường.

### 2. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/lib/appQueryClient.ts`
- **Dòng 7**: `15_000`
  Ý nghĩa / ngữ cảnh sử dụng: `staleTime` mặc định cho query client.
  Lý do cần có data này: Điều khiển tần suất refetch toàn app.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa về `QueryPolicyOptions` đọc từ env/system_configs.

- **Dòng 8**: `10 * 60 * 1000`
  Ý nghĩa / ngữ cảnh sử dụng: `gcTime` mặc định.
  Lý do cần có data này: Tuổi thọ cache trong memory.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config hóa theo profile web/mobile/low-memory.

- **Dòng 12**: `1`
  Ý nghĩa / ngữ cảnh sử dụng: `retry` mặc định.
  Lý do cần có data này: Số lần retry mặc định cho query.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Điều khiển qua `operational.retry.query_default`.

### 3. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/http/clientFetch.ts`
- **Dòng 5**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: `DEFAULT_CLIENT_TIMEOUT_MS`.
  Lý do cần có data này: Timeout mặc định client-side fetch.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Lấy từ `system_configs` (`operational.http.client_timeout_ms`).

- **Dòng 6**: `1_000`
  Ý nghĩa / ngữ cảnh sử dụng: `MIN_CLIENT_TIMEOUT_MS`.
  Lý do cần có data này: Floor chống cấu hình timeout quá thấp.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ trong constants guardrail, thêm docs.

### 4. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/http/serverHttpClient.ts`
- **Dòng 29**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: `DEFAULT_SERVER_TIMEOUT_MS`.
  Lý do cần có data này: Timeout request server action -> BE.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa vào `operational.http.server_timeout_ms`.

- **Dòng 30**: `1_000`
  Ý nghĩa / ngữ cảnh sử dụng: `MIN_SERVER_TIMEOUT_MS`.
  Lý do cần có data này: Floor timeout server-side.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ dạng guardrail.

### 5. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/http/clientJsonRequest.ts`
- **Dòng 47**: `1_000`
  Ý nghĩa / ngữ cảnh sử dụng: floor timeout cho request JSON.
  Lý do cần có data này: Tránh timeout <= 0.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ constants.

- **Dòng 47**: `15_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout default nếu caller không truyền.
  Lý do cần có data này: Điều phối độ trễ client request.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đồng bộ với policy timeout toàn hệ thống qua config.

### 6. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/auth/authConstants.ts`
- **Dòng 14**: `60`
  Ý nghĩa / ngữ cảnh sử dụng: ngưỡng refresh access token sớm.
  Lý do cần có data này: Tránh token hết hạn giữa request.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đặt key `auth.access_refresh_threshold_seconds` trong `system_configs`.

- **Dòng 15**: `600`
  Ý nghĩa / ngữ cảnh sử dụng: TTL access token fallback.
  Lý do cần có data này: Dùng khi backend không trả TTL.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đồng bộ từ BE qua `/auth/session` metadata hoặc `system_configs`.

- **Dòng 16**: `30 * 24 * 60 * 60`
  Ý nghĩa / ngữ cảnh sử dụng: TTL refresh token fallback 30 ngày.
  Lý do cần có data này: Đồng bộ vòng đời session.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa sang config trung tâm auth policy.

### 7. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/auth/_shared.ts`
- **Dòng 154**: `128`
  Ý nghĩa / ngữ cảnh sử dụng: cắt `deviceId` tối đa 128 ký tự.
  Lý do cần có data này: Guardrail chống header/cookie bất thường.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ constants + comment liên kết giới hạn BE.

- **Dòng 174**: `365 * 24 * 60 * 60`
  Ý nghĩa / ngữ cảnh sử dụng: Max-Age cookie `deviceId` = 1 năm.
  Lý do cần có data này: Ảnh hưởng persist device fingerprint.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config hóa theo policy bảo mật/compliance.

### 8. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/auth/clientSessionSnapshot.ts`
- **Dòng 32**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: TTL cache snapshot session client.
  Lý do cần có data này: Cân bằng freshness và giảm request thừa.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đặt trong `auth.client_snapshot_ttl_ms`.

- **Dòng 33**: `6_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout fetch session endpoint.
  Lý do cần có data này: Ổn định bootstrap auth state.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đồng bộ timeout profile toàn app.

### 9. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/auth/refreshClient.ts`
- **Dòng 8**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout gọi `/api/auth/refresh`.
  Lý do cần có data này: Ảnh hưởng resilience luồng refresh.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đặt key `auth.refresh_timeout_ms`.

### 10. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/components/auth/AuthSessionManager.ts`
- **Dòng 18**: `40 * 60 * 1000`
  Ý nghĩa / ngữ cảnh sử dụng: chu kỳ refresh định kỳ.
  Lý do cần có data này: Duy trì phiên đăng nhập.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config từ policy auth.

- **Dòng 19**: `20 * 60 * 1000`
  Ý nghĩa / ngữ cảnh sử dụng: throttle refresh tối thiểu.
  Lý do cần có data này: Ngăn spam refresh.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config `auth.refresh_throttle_ms`.

- **Dòng 113**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: maxAge session snapshot khi bootstrap.
  Lý do cần có data này: Độ tươi auth state ban đầu.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Dùng chung với config snapshot TTL.

- **Dòng 131**: `120`
  Ý nghĩa / ngữ cảnh sử dụng: delay bootstrap session sau mount.
  Lý do cần có data này: Tránh tranh chấp first paint/request.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa vào config UI-runtime.

### 11. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/realtime/realtimeSessionGuard.ts`
- **Dòng 10**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: TTL snapshot trước khi mở SignalR.
  Lý do cần có data này: Giảm reconnect khi session cũ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đồng bộ với auth snapshot policy.

### 12. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/application/hooks/usePresenceConnection.ts`
- **Dòng 12**: `[0, 2000, 5000, 10000, 30000]`
  Ý nghĩa / ngữ cảnh sử dụng: lịch reconnect SignalR.
  Lý do cần có data này: Ảnh hưởng recovery mạng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Cho phép cấu hình `operational.realtime.reconnect_schedule_ms`.

- **Dòng 13**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout negotiation.
  Lý do cần có data này: Cắt sớm phiên lỗi.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config hóa.

- **Dòng 14**: `45_000`
  Ý nghĩa / ngữ cảnh sử dụng: cooldown sau lỗi negotiation.
  Lý do cần có data này: Chống reconnect storm.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config hóa theo môi trường.

- **Dòng 110**: `120000`
  Ý nghĩa / ngữ cảnh sử dụng: `serverTimeoutInMilliseconds`.
  Lý do cần có data này: Ngưỡng timeout kết nối real-time.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa vào policy realtime.

### 13. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/application/hooks/usePresenceConnection.registration.ts`
- **Dòng 16**: `5 * 60 * 1000`
  Ý nghĩa / ngữ cảnh sử dụng: heartbeat interval.
  Lý do cần có data này: Giữ kết nối realtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config `operational.realtime.heartbeat_ms`.

- **Dòng 17**: `320`
  Ý nghĩa / ngữ cảnh sử dụng: delay batch invalidation.
  Lý do cần có data này: Gom nhiều event để giảm invalidate spam.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đưa về policy cache invalidation.

- **Dòng 18**: `2_000`
  Ý nghĩa / ngữ cảnh sử dụng: khoảng cách tối thiểu refresh ví.
  Lý do cần có data này: Tránh gọi balance API quá dày.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config `wallet.refresh_min_interval_ms`.

- **Dòng 19**: `80`
  Ý nghĩa / ngữ cảnh sử dụng: delay retry invalidation tối thiểu.
  Lý do cần có data này: Tránh retry loop quá sát.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: Giữ constants guardrail.

- **Dòng 23-33**: `wallet:1000`, `inventory:1800`, `collection:1800`, `readingSetup:1200`, `readingHistory:1000`, `profile:1000`, `readerRequest:1000`, `notifications:800`, `chat:1000`, `gacha:1000`, `gamification:1000`
  Ý nghĩa / ngữ cảnh sử dụng: cooldown theo domain invalidation.
  Lý do cần có data này: Cân bằng realtime consistency và API load.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Chuyển thành `operational.invalidation.cooldown_by_domain` (JSON trong `system_configs`).

- **Dòng 216 và 226**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: delay invalidate inbox/unread.
  Lý do cần có data này: debounce burst event.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Tách debounce config.

### 14. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/application/hooks/useChatRealtimeSync.ts`
- **Dòng 21**: `60_000`
  Ý nghĩa / ngữ cảnh sử dụng: cooldown retry sau unauthorized.
  Lý do cần có data này: hạn chế reconnect vô ích.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config trong realtime auth policy.

- **Dòng 22**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: negotiation timeout.
  Lý do cần có data này: kiểm soát thời gian chờ kết nối.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Đồng bộ với presence timeout.

- **Dòng 107, 118**: `3000`
  Ý nghĩa / ngữ cảnh sử dụng: bỏ qua invalidate trong 3s đầu app-start.
  Lý do cần có data này: tránh burst refetch lúc bootstrap.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: Config hóa startup grace period.

- **Dòng 114, 125**: `1000`
  Ý nghĩa / ngữ cảnh sử dụng: debounce invalidate inbox/unread.
  Lý do cần có data này: giảm spam invalidate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng chung debounce config.

- **Dòng 140**: `[0, 2000, 5000, 10000, 30000]`
  Ý nghĩa / ngữ cảnh sử dụng: reconnect schedule.
  Lý do cần có data này: chính sách recovery realtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: central config.

- **Dòng 144**: `120000`
  Ý nghĩa / ngữ cảnh sử dụng: server timeout SignalR.
  Lý do cần có data này: tránh disconnect giả.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: central config.

- **Dòng 174**: `UNAUTHORIZED_COOLDOWN_MS / 2` (= 30_000)
  Ý nghĩa / ngữ cảnh sử dụng: cooldown tạm với lỗi không phải 401.
  Lý do cần có data này: fallback retry policy.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: tách rõ `generic_error_cooldown_ms`.

### 15. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/navigation/useOptimizedNavigation.ts`
- **Dòng 14**: `90_000`
  Ý nghĩa / ngữ cảnh sử dụng: cooldown prefetch route.
  Lý do cần có data này: tránh prefetch trùng lặp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình theo thiết bị/network profile.

- **Dòng 15**: `90_000`
  Ý nghĩa / ngữ cảnh sử dụng: cooldown prefetch query.
  Lý do cần có data này: giảm load query.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ chính sách prefetch.

### 16. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/navigation/useOptimizedLink.tsx`
- **Dòng 17-18**: `90_000`, `90_000`
  Ý nghĩa / ngữ cảnh sử dụng: cooldown prefetch trên intent hover/focus.
  Lý do cần có data này: cân bằng UX và băng thông.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng chung config với `useOptimizedNavigation`.

### 17. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/navigation/routeQueryPrefetch.ts`
- **Dòng 16**: `6_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout prefetch API.
  Lý do cần có data này: đảm bảo prefetch không treo.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào `operational.prefetch.timeout_ms`.

- **Dòng 74, 84, 108**: `20_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime prefetch inventory/gacha/wallet.
  Lý do cần có data này: cache độ tươi ngắn hạn.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình theo route.

- **Dòng 98**: `45_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime prefetch reading setup.
  Lý do cần có data này: snapshot reading có độ ổn định cao hơn.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: route-level prefetch policy trong config.

### 18. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/navigation/prefetchPolicy.ts`
- **Dòng 16**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: gate delay sau route change trước khi prefetch.
  Lý do cần có data này: tránh prefetch sai route vừa thoát.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đặt thành `prefetch.route_change_delay_ms`.

### 19. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/gacha/useGacha.ts`
- **Dòng 19, 28, 38**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout fetch pool/odds/history.
  Lý do cần có data này: điều khiển độ ổn định UI gacha.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: timeout profile riêng cho gacha API.

- **Dòng 42**: `historyPreviewSize = 6`
  Ý nghĩa / ngữ cảnh sử dụng: số item preview history mặc định.
  Lý do cần có data này: ảnh hưởng UX màn gacha.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config qua `gacha.history_preview_size`.

- **Dòng 45**: clamp `1..20`
  Ý nghĩa / ngữ cảnh sử dụng: giới hạn pageSize preview.
  Lý do cần có data này: bảo vệ API/render.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ guardrail, chỉ config max nếu cần.

- **Dòng 43, 56, 62**: `20_000`, `20_000`, `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime pools/odds/history.
  Lý do cần có data này: quyết định độ tươi dữ liệu gacha.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: gói vào `gacha.cache_policy`.

### 20. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/gacha/useGachaHistory.ts`
- **Dòng 23**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout tải history.
  Lý do cần có data này: responsiveness page lịch sử.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng timeout policy chung.

- **Dòng 29**: clamp pageSize `1..50`
  Ý nghĩa / ngữ cảnh sử dụng: giới hạn số bản ghi mỗi trang.
  Lý do cần có data này: bảo vệ API và table render.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ guardrail, thêm doc.

- **Dòng 34**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime history page.
  Lý do cần có data này: mức đồng bộ dữ liệu quay gần nhất.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config hóa.

### 21. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/gacha/gachaServerActions.ts`
- **Dòng 54**: `page = 1`, `pageSize = 20`
  Ý nghĩa / ngữ cảnh sử dụng: default pagination server action.
  Lý do cần có data này: fallback khi không truyền param.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ page-size policy với route/client.

- **Dòng 57**: clamp pageSize `<=100`
  Ý nghĩa / ngữ cảnh sử dụng: cap server-side.
  Lý do cần có data này: chặn request quá lớn.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants guardrail.

### 22. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/gacha/history/route.ts`
- **Dòng 14-17**: default `page=1`, `pageSize=20`, cap `100`
  Ý nghĩa / ngữ cảnh sử dụng: normalize query params gacha history API.
  Lý do cần có data này: tránh input bẩn.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: tạo constants chung dùng lại ở client/server.

### 23. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/gacha/pull/route.ts`
- **Dòng 33-34**: default count `1`, clamp `1..10`
  Ý nghĩa / ngữ cảnh sử dụng: giới hạn số lượt quay mỗi request.
  Lý do cần có data này: bảo vệ tài nguyên backend và UX.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy từ `gacha.pool.<code>.meta` hoặc `system_configs`.

### 24. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/gacha/usePullGacha.ts`
- **Dòng 24**: `12_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout pull gacha.
  Lý do cần có data này: kéo dài hơn fetch thường do mutation nặng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào `gacha.pull_timeout_ms`.

### 25. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/gacha/gachaRealtimeDedup.ts`
- **Dòng 1**: `45_000`
  Ý nghĩa / ngữ cảnh sử dụng: suppression window tránh invalidation trùng local mutation.
  Lý do cần có data này: giảm double refresh cache.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `gacha.realtime_sync_suppression_ms`.

### 26. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/gacha/useRareDropLottie.ts`
- **Dòng 17-23**: priority map `mythic=4`, `legendary=3`, `epic=2`, `rare=1`, map alias `'5'/'4'/'3'`
  Ý nghĩa / ngữ cảnh sử dụng: phân cấp rarity để chọn animation.
  Lý do cần có data này: ánh xạ giao diện theo rarity.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đọc từ metadata pool hoặc enum chung do BE trả.

- **Dòng 86**: `1000 * 60 * 30`
  Ý nghĩa / ngữ cảnh sử dụng: cache lottie 30 phút.
  Lý do cần có data này: giảm fetch animation lặp.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình `gacha.lottie_cache_ttl_ms`.

### 27. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/components/ui/gacha/GachaHistoryPageClient.tsx`
- **Dòng 32**: `DEFAULT_PAGE_SIZE = 20`
  Ý nghĩa / ngữ cảnh sử dụng: số bản ghi lịch sử mỗi trang.
  Lý do cần có data này: ảnh hưởng UX/history load.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng constants pagination chung gacha.

### 28. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/components/ui/gacha/GachaResultModal.tsx`
- **Dòng 18**: `650`
  Ý nghĩa / ngữ cảnh sử dụng: delay reveal animation mặc định.
  Lý do cần có data này: pacing UX khi mở thưởng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào UI animation config.

- **Dòng 19**: `150`
  Ý nghĩa / ngữ cảnh sử dụng: delay reveal cho reduced-motion.
  Lý do cần có data này: accessibility tuning.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants nhưng tài liệu hóa.

### 29. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/inventory/inventoryRealtimeDedup.ts`
- **Dòng 1**: `30_000`
  Ý nghĩa / ngữ cảnh sử dụng: suppression inventory realtime invalidation.
  Lý do cần có data này: tránh invalidate lặp sau local use-item.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `inventory.realtime_sync_suppression_ms`.

### 30. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/inventory/useInventory.ts`
- **Dòng 17**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout gọi inventory API.
  Lý do cần có data này: responsiveness trang inventory.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng timeout profile chung.

- **Dòng 29**: `20_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime cache inventory.
  Lý do cần có data này: cân bằng realtime và số lần fetch.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config hóa.

### 31. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/inventory/useOwnedInventoryCards.ts`
- **Dòng 47**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout fetch collection để chọn card mục tiêu.
  Lý do cần có data này: ảnh hưởng luồng dùng item.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: timeout policy chung.

- **Dòng 49**: `20_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime collection query trong modal use item.
  Lý do cần có data này: giữ dữ liệu ổn định ngắn hạn.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config hóa.

### 32. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/infrastructure/inventory/useUseItem.ts`
- **Dòng 106-117**: mapping spread ticket -> `3`, `5`, `10`
  Ý nghĩa / ngữ cảnh sử dụng: suy ra loại spread từ item code.
  Lý do cần có data này: đồng bộ quota free draw sau khi dùng item.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: map code -> spread lấy từ metadata item từ BE.

### 33. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/components/ui/inventory/UseItemModal.tsx`
- **Dòng 25, 41-42**: default quantity `1`, clamp `1..10`
  Ý nghĩa / ngữ cảnh sử dụng: số lượng item dùng mỗi lần trong modal.
  Lý do cần có data này: giới hạn thao tác người dùng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy max theo item metadata từ API.

### 34. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/components/ui/inventory/UseItemQuantitySelector.tsx`
- **Dòng 27, 34**: max `10`, min `1`
  Ý nghĩa / ngữ cảnh sử dụng: control tăng/giảm số lượng.
  Lý do cần có data này: guardrail front-end.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng `item.maxUsePerRequest` từ backend.

### 35. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/inventory/route.ts`
- **Dòng 68-70**: quantity default `1`, clamp `1..10`
  Ý nghĩa / ngữ cảnh sử dụng: normalize payload use item ở API route.
  Lý do cần có data này: chặn payload bất hợp lệ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants chung với UI + BE.

### 36. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/domain/constants.ts`
- **Dòng 1**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: tỷ lệ quy đổi VND/diamond.
  Lý do cần có data này: tính gross/net khi rút.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: không để FE tự giữ source-of-truth; đọc từ `system_configs` (`economy.vnd_per_diamond`).

- **Dòng 2**: `500`
  Ý nghĩa / ngữ cảnh sử dụng: min rút diamond.
  Lý do cần có data này: rule nghiệp vụ rút tiền.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: đọc từ `withdrawal.minimum_diamond` (DB).

- **Dòng 3**: `0.1`
  Ý nghĩa / ngữ cảnh sử dụng: phí rút 10%.
  Lý do cần có data này: rule tính phí.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: đọc từ `withdrawal.fee_rate` (DB).

- **Dòng 4**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: mức nạp tối thiểu VND.
  Lý do cần có data này: rule nạp tiền.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng `deposit.policy` từ `system_configs`.

- **Dòng 5**: `[50_000,100_000,200_000,500_000,1_000_000,2_000_000]`
  Ý nghĩa / ngữ cảnh sử dụng: preset amount ở FE.
  Lý do cần có data này: ảnh hưởng funnel nạp tiền.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: lấy trực tiếp `deposit.packages` từ backend.

### 37. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/application/useWithdrawPage.ts`
- **Dòng 46-48**: tính toán gross/fee/net dựa trên constants FE
  Ý nghĩa / ngữ cảnh sử dụng: hiển thị ước tính tiền rút.
  Lý do cần có data này: phản hồi tức thì cho người dùng.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: dùng policy từ BE snapshot để tránh lệch FE/BE.

### 38. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/presentation/components/withdraw/WithdrawSubmitSection.tsx`
- **Dòng 45**: `amountNum < 50`
  Ý nghĩa / ngữ cảnh sử dụng: disable submit dưới 50 diamond.
  Lý do cần có data này: validation UI.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: bỏ hard-code này, chỉ dùng `MIN_WITHDRAW_DIAMOND` từ policy (hiện là 500) để tránh bug.

### 39. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/presentation/components/withdraw/useWithdrawFormCard.ts`
- **Dòng 20**: `1_000`
  Ý nghĩa / ngữ cảnh sử dụng: max length user note khi rút tiền.
  Lý do cần có data này: ràng buộc input.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ với backend `NoteMaxLength` qua schema metadata.

### 40. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/wallet/ledger/route.ts`
- **Dòng 33-34**: default `page=1`, `limit=10`, cap `50`
  Ý nghĩa / ngữ cảnh sử dụng: normalize ledger query.
  Lý do cần có data này: giới hạn phân trang API.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants pagination chung wallet.

### 41. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/application/useWalletOverviewPage.ts`
- **Dòng 36**: `limit=10`
  Ý nghĩa / ngữ cảnh sử dụng: số dòng ledger/trang.
  Lý do cần có data này: UI pagination.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng constant chung với route/action.

- **Dòng 43**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout fetch ledger.
  Lý do cần có data này: network budget.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: timeout policy chung.

### 42. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/application/actions/ledger.ts`
- **Dòng 12**: `limit = 20`
  Ý nghĩa / ngữ cảnh sử dụng: default page size server action.
  Lý do cần có data này: fallback phân trang.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ lại với UI/route để tránh lệch 10 vs 20.

### 43. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/application/actions/withdrawal/admin.ts`
- **Dòng 15**: `pageSize = 20`
  Ý nghĩa / ngữ cảnh sử dụng: queue withdrawals admin mặc định.
  Lý do cần có data này: phân trang danh sách xử lý.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants admin pagination.

### 44. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/application/actions/withdrawal/reader.ts`
- **Dòng 56**: `pageSize = 20`
  Ý nghĩa / ngữ cảnh sử dụng: lịch sử rút của reader.
  Lý do cần có data này: pagination mặc định.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants chung wallet history.

### 45. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/application/useDepositHistoryPage.ts`
- **Dòng 13**: `DEFAULT_PAGE_SIZE = 10`
  Ý nghĩa / ngữ cảnh sử dụng: số giao dịch nạp mỗi trang.
  Lý do cần có data này: pagination UX.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: unify với server action.

- **Dòng 40**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime trang lịch sử nạp.
  Lý do cần có data này: refresh tần suất vừa phải.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config cache policy.

### 46. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/wallet/application/useDepositPage.ts`
- **Dòng 79**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: polling interval khi order trạng thái `pending`.
  Lý do cần có data này: đồng bộ trạng thái thanh toán.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình `deposit.pending_poll_interval_ms`.

### 47. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/notifications/route.ts`
- **Dòng 49-50**: default `page=1`, `pageSize=20`, cap `100`
  Ý nghĩa / ngữ cảnh sử dụng: normalize query notifications.
  Lý do cần có data này: phân trang + giới hạn tải.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants pagination thống nhất.

### 48. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/notifications/application/actions/list.ts`
- **Dòng 11-12**: `page=1`, `pageSize=20`
  Ý nghĩa / ngữ cảnh sử dụng: defaults server action notifications.
  Lý do cần có data này: fallback request.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng constants chung.

### 49. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/notifications/application/useNotificationsPage.ts`
- **Dòng 15**: `pageSize = 20`
  Ý nghĩa / ngữ cảnh sử dụng: kích thước trang notifications chính.
  Lý do cần có data này: pagination UX.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng shared constant.

### 50. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/notifications/application/useNotificationDropdown.ts`
- **Dòng 20**: `pageSize = 10`
  Ý nghĩa / ngữ cảnh sử dụng: số thông báo preview dropdown.
  Lý do cần có data này: giảm tải navbar.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `notifications.dropdown_page_size`.

- **Dòng 30, 47, 63**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout list/count/patch.
  Lý do cần có data này: network budget notifications.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: timeout policy chung.

- **Dòng 83**: `60_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime danh sách dropdown.
  Lý do cần có data này: cache tối ưu navbar.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình theo channel realtime.

### 51. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/application/hooks/useChatUnreadNotifications.ts`
- **Dòng 77**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout unread chat count.
  Lý do cần có data này: phản hồi badge.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: timeout policy chung.

- **Dòng 81**: `15_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime unread badge.
  Lý do cần có data này: giảm fetch nhưng vẫn đủ tươi.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `chat.unread_badge_stale_ms`.

### 52. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/api/readers/route.ts`
- **Dòng 23-24**: default `page=1`, `pageSize=12`, cap `50`
  Ý nghĩa / ngữ cảnh sử dụng: normalize API readers list.
  Lý do cần có data này: phân trang directory.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants chung readers pagination.

### 53. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reader/application/actions/directory.ts`
- **Dòng 115-116**: `page=1`, `pageSize=12`
  Ý nghĩa / ngữ cảnh sử dụng: defaults listReaders.
  Lý do cần có data này: fallback server action.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng constants.

- **Dòng 153**: `limit=4`
  Ý nghĩa / ngữ cảnh sử dụng: số reader nổi bật.
  Lý do cần có data này: layout homepage.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đưa vào `home.featured_readers_limit`.

- **Dòng 157**: `revalidate: 120`
  Ý nghĩa / ngữ cảnh sử dụng: cache TTL featured readers.
  Lý do cần có data này: cân bằng độ tươi/home SEO.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `reader.featured_revalidate_seconds`.

### 54. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reader/application/useReadersDirectoryPage.ts`
- **Dòng 14**: `pageSize = 12`
  Ý nghĩa / ngữ cảnh sử dụng: page size client directory.
  Lý do cần có data này: đồng bộ UI phân trang.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng constants chung với API/action.

- **Dòng 19**: `300`
  Ý nghĩa / ngữ cảnh sử dụng: debounce search.
  Lý do cần có data này: giảm request khi user gõ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `reader.search_debounce_ms`.

- **Dòng 35**: `8_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout API readers.
  Lý do cần có data này: network budget.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: timeout policy chung.

- **Dòng 37**: `30_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime directory.
  Lý do cần có data này: hiệu năng khi phân trang/filter.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config cache policy theo module.

### 55. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reading/history/domain/historyQueryKeys.ts`
- **Dòng 1**: `HISTORY_PAGE_SIZE = 10`
  Ý nghĩa / ngữ cảnh sử dụng: page size cố định reading history.
  Lý do cần có data này: query key và action đồng bộ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants pagination shared.

### 56. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reading/application/useReadingSetupPage.ts`
- **Dòng 35-38**: `daily_1=1`, `spread_3=3`, `spread_5=5`, `spread_10=10`
  Ý nghĩa / ngữ cảnh sử dụng: map spread -> số lá cần rút.
  Lý do cần có data này: điều hướng sang session.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: trả từ BE metadata spreads thay vì hard-code FE.

- **Dòng 41-42**: `8_000`, `12_000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout snapshot/init reading.
  Lý do cần có data này: ổn định UX luồng setup.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: tách timeout policy.

- **Dòng 61**: `10_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime reading setup snapshot.
  Lý do cần có data này: giữ độ tươi quota/cost.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config cache policy.

- **Dòng 64-66**: `spread3=0`, `spread5=0`, `spread10=0`
  Ý nghĩa / ngữ cảnh sử dụng: fallback free draw quota.
  Lý do cần có data này: tránh crash khi API thiếu dữ liệu.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: fallback từ default payload BE, không hard-code FE.

- **Dòng 70-75**: `50`, `5`, `100`, `10`, `500`, `50`
  Ý nghĩa / ngữ cảnh sử dụng: fallback pricing spread gold/diamond.
  Lý do cần có data này: hiển thị giá nếu snapshot thiếu.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: bắt buộc đọc từ `system_configs`; thiếu key thì cảnh báo/fail-fast, không fallback cứng.

- **Dòng 86**: `max(300)`
  Ý nghĩa / ngữ cảnh sử dụng: giới hạn câu hỏi đầu vào.
  Lý do cần có data này: bảo vệ payload stream/init.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ với BE validation metadata.

- **Dòng 106, 120, 134, 148**: `exp = 1` hoặc `2`
  Ý nghĩa / ngữ cảnh sử dụng: EXP gain hiển thị theo currency.
  Lý do cần có data này: gameplay feedback.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: lấy từ policy progression của BE.

### 57. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reading/presentation/components/AiInterpretationStream.tsx`
- **Dòng 18**: `[1, 2, 4, 8, 16]`
  Ý nghĩa / ngữ cảnh sử dụng: price tiers follow-up theo lần hỏi.
  Lý do cần có data này: rule tính phí follow-up.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: lấy từ `followup.policy.priceTiers` trong `system_configs`.

- **Dòng 29**: `freeSlotsTotal = 0`
  Ý nghĩa / ngữ cảnh sử dụng: số lượt follow-up miễn phí.
  Lý do cần có data này: business policy.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: lấy từ `followup.policy.freeSlotThresholds`.

- **Dòng 34**: `userFollowupCount >= 5`
  Ý nghĩa / ngữ cảnh sử dụng: hard cap follow-up.
  Lý do cần có data này: giới hạn chi phí/tải model.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: lấy từ `followup.policy.maxFollowupsAllowed`.

### 58. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reading/presentation/components/ai-stream/useAiStreamSession.ts`
- **Dòng 96**: `48`
  Ý nghĩa / ngữ cảnh sử dụng: flush chunk streaming mỗi 48ms.
  Lý do cần có data này: cân bằng mượt mà và render cost.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config UI streaming cadence.

- **Dòng 120**: `100`
  Ý nghĩa / ngữ cảnh sử dụng: delay khởi tạo EventSource.
  Lý do cần có data này: tránh race condition mount.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config startup delay.

### 59. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/app/[locale]/api/reading/sessions/[sessionId]/stream/route.ts`
- **Dòng 6**: `64`
  Ý nghĩa / ngữ cảnh sử dụng: max length sessionId.
  Lý do cần có data này: security/input validation.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ guardrail + đồng bộ BE.

- **Dòng 7**: `['vi','en','zh']`
  Ý nghĩa / ngữ cảnh sử dụng: whitelist language.
  Lý do cần có data này: kiểm soát language param stream.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy từ danh sách locale app hoặc config chung.

- **Dòng 8**: `2000`
  Ý nghĩa / ngữ cảnh sử dụng: cắt độ dài follow-up question.
  Lý do cần có data này: bảo vệ stream endpoint.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ với BE follow-up max length từ config.

### 60. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reader/presentation/readerApplyFormSchema.ts`
- **Dòng 19**: `DEFAULT_DIAMOND_PER_QUESTION = 100`
  Ý nghĩa / ngữ cảnh sử dụng: default giá reader khi chưa có dữ liệu.
  Lý do cần có data này: prefill form.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: lấy từ `reader.min_diamond_per_question` hoặc snapshot policy.

- **Dòng 20-21**: `MAX_BIO_LENGTH=4000`, `MAX_SOCIAL_URL_LENGTH=500`
  Ý nghĩa / ngữ cảnh sử dụng: validator max length.
  Lý do cần có data này: ràng buộc form.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng schema metadata từ backend.

- **Dòng 25, 27, 28**: `bio min 20`, `years min 1`, `diamond min 50`
  Ý nghĩa / ngữ cảnh sử dụng: business floor reader apply.
  Lý do cần có data này: rule kiểm duyệt profile.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: đọc từ `reader.*` policy trong `system_configs`.

### 61. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reader/presentation/components/ReaderApplyExperiencePriceRow.tsx`
- **Dòng 33, 49**: `min=1`, `min=50`
  Ý nghĩa / ngữ cảnh sử dụng: min input years và diamondPerQuestion trên UI.
  Lý do cần có data này: guardrail frontend.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: bind từ policy provider FE.

### 62. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/reader/presentation/ReaderApplyPage.tsx`
- **Dòng 45-46**: fallback watch `years=1`, `diamond=50`
  Ý nghĩa / ngữ cảnh sử dụng: default khi form chưa có value.
  Lý do cần có data này: tránh NaN khi render.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng constants central policy.

- **Dòng 84**: fallback setValue `1` và `50`
  Ý nghĩa / ngữ cảnh sử dụng: sanitize input khi giá trị không hợp lệ.
  Lý do cần có data này: an toàn form update.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy floor từ config thay vì literal.

### 63. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/profile/reader/application/useProfileReaderSettingsPage.ts`
- **Dòng 44-45**: `DEFAULT_DIAMOND_PER_QUESTION=100`, `DEFAULT_YEARS_OF_EXPERIENCE=1`
  Ý nghĩa / ngữ cảnh sử dụng: fallback khi profile reader thiếu dữ liệu.
  Lý do cần có data này: giữ form hoạt động.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: policy from BE, không hard-code.

- **Dòng 50-52**: clamp `min 50`, `min 1`
  Ý nghĩa / ngữ cảnh sử dụng: giới hạn giá và năm kinh nghiệm.
  Lý do cần có data này: business validation.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: đọc từ reader policy.

### 64. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/profile/reader/presentation/components/useReaderSettingsFormCard.ts`
- **Dòng 50-51**: `MAX_BIO_LENGTH=4000`, `MAX_SOCIAL_URL_LENGTH=500`
  Ý nghĩa / ngữ cảnh sử dụng: max field trong form settings.
  Lý do cần có data này: đồng bộ validation.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chia sẻ schema metadata với BE.

- **Dòng 77-78**: `years min 1`, `price min 50`
  Ý nghĩa / ngữ cảnh sử dụng: min validator form reader settings.
  Lý do cần có data này: tránh submit dưới chuẩn.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: dùng policy động từ BE.

### 65. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/auth/domain/schemas.ts`
- **Dòng 55**: `email max 100`
  Ý nghĩa / ngữ cảnh sử dụng: giới hạn email đăng ký.
  Lý do cần có data này: validation input.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ với giới hạn DB/BE metadata.

- **Dòng 58-60**: `username min 3`, `max 32`
  Ý nghĩa / ngữ cảnh sử dụng: rule username.
  Lý do cần có data này: chuẩn identity.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config + expose auth policy endpoint.

- **Dòng 63-64**: `password min 8`, `max 100`
  Ý nghĩa / ngữ cảnh sử dụng: rule mật khẩu.
  Lý do cần có data này: policy bảo mật.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ từ backend/auth provider.

- **Dòng 67**: `displayName max 50`
  Ý nghĩa / ngữ cảnh sử dụng: ràng buộc display name.
  Lý do cần có data này: storage/UX.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: metadata từ BE.

- **Dòng 86**: `age >= 16`
  Ý nghĩa / ngữ cảnh sử dụng: ngưỡng tuổi tối thiểu.
  Lý do cần có data này: compliance/business rule.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy config theo pháp lý thị trường.

- **Dòng 105 và 121**: OTP length `6`
  Ý nghĩa / ngữ cảnh sử dụng: verify/reset OTP.
  Lý do cần có data này: ràng buộc auth flow.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants auth, đồng bộ với backend.

### 66. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/profile/application/useProfilePage.ts`
- **Dòng 47-48**: `MIN_ACCOUNT_NUMBER_LENGTH=6`, `MAX_ACCOUNT_NUMBER_LENGTH=32`
  Ý nghĩa / ngữ cảnh sử dụng: validation số tài khoản payout.
  Lý do cần có data này: ngăn dữ liệu lỗi.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: theo metadata của bank validator từ BE.

- **Dòng 55**: `account holder max 120`
  Ý nghĩa / ngữ cảnh sử dụng: giới hạn tên chủ tài khoản.
  Lý do cần có data này: ràng buộc dữ liệu.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ với schema backend.

### 67. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/checkin/application/hooks.ts`
- **Dòng 28**: `failureCount < 2`
  Ý nghĩa / ngữ cảnh sử dụng: retry checkin streak tối đa 2 lần.
  Lý do cần có data này: resilience khi lỗi mạng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config retry policy query theo domain.

### 68. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/media-upload/constants.ts`
- **Dòng 2**: `10 * 1024 * 1024`
  Ý nghĩa / ngữ cảnh sử dụng: ảnh tối đa 10MB.
  Lý do cần có data này: giới hạn upload ảnh.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy từ upload policy BE để tránh lệch.

- **Dòng 3**: `5 * 1024 * 1024`
  Ý nghĩa / ngữ cảnh sử dụng: voice tối đa 5MB.
  Lý do cần có data này: kiểm soát băng thông/lưu trữ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: đồng bộ với backend upload rule.

- **Dòng 4**: `600_000`
  Ý nghĩa / ngữ cảnh sử dụng: voice max duration 10 phút.
  Lý do cần có data này: giới hạn media message.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config từ policy chat/media.

- **Dòng 6**: `80 * 1024`
  Ý nghĩa / ngữ cảnh sử dụng: target bytes sau nén ảnh.
  Lý do cần có data này: tối ưu tốc độ tải.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: biến thành quality profile.

- **Dòng 8-11**: compression steps `(quality/maxSizeMB/maxWidthOrHeight)` = `(0.68/0.15/1440)`, `(0.58/0.1/1200)`, `(0.48/0.06/960)`, `(0.38/0.03/640)`
  Ý nghĩa / ngữ cảnh sử dụng: chiến lược nén ảnh nhiều tầng.
  Lý do cần có data này: trade-off chất lượng/kích thước.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuyển sang config JSON để A/B và tune thiết bị.

- **Dòng 20-21**: retry attempts `3`, base delay `350ms`
  Ý nghĩa / ngữ cảnh sử dụng: retry upload mặc định.
  Lý do cần có data này: tăng tỷ lệ upload thành công.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: policy retry riêng theo loại media.

### 69. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/application/voiceRecorderHelpers.ts`
- **Dòng 1-3**: `MAX_DURATION_SECONDS=120`, `ANALYSER_INTERVAL_MS=80`, `WAVEFORM_BAR_COUNT=40`
  Ý nghĩa / ngữ cảnh sử dụng: cấu hình recorder và waveform.
  Lý do cần có data này: UX voice message.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: tách sang `chat.voice` config.

### 70. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/application/useVoiceRecorder.ts`
- **Dòng 53**: `analyser.fftSize = 256`
  Ý nghĩa / ngữ cảnh sử dụng: độ phân giải FFT.
  Lý do cần có data này: fidelity waveform và CPU cost.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình theo device capability.

- **Dòng 56**: `audioBitsPerSecond = 16_000`
  Ý nghĩa / ngữ cảnh sử dụng: bitrate ghi âm.
  Lý do cần có data này: chất lượng/size voice.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config theo mạng và chính sách media.

- **Dòng 59**: `recorder.start(250)`
  Ý nghĩa / ngữ cảnh sử dụng: timeslice chunk media.
  Lý do cần có data này: latency khi stream/upload.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình chunk size.

- **Dòng 66**: `setInterval(..., 200)`
  Ý nghĩa / ngữ cảnh sử dụng: tick cập nhật elapsed.
  Lý do cần có data này: smoothness UI.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: UI runtime config.

### 71. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/presentation/components/usePaymentOfferModalState.ts`
- **Dòng 10-11**: `amount min 1`, `note max 100`
  Ý nghĩa / ngữ cảnh sử dụng: validation đề nghị nạp thêm tiền trong chat.
  Lý do cần có data này: rule giao dịch nội dung chat.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy từ policy finance/dispute.

- **Dòng 27**: `amount default = 10`
  Ý nghĩa / ngữ cảnh sử dụng: giá trị prefill amount.
  Lý do cần có data này: UX khởi tạo form.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config default theo market/campaign.

### 72. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/presentation/components/useVoiceMessagePlayback.ts`
- **Dòng 70**: `10000`
  Ý nghĩa / ngữ cảnh sử dụng: timeout chờ `oncanplaythrough`.
  Lý do cần có data này: tránh treo UI khi preload audio lỗi.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `chat.voice_preload_timeout_ms`.

### 73. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/application/actions/conversations.core.ts`
- **Dòng 10**: `slaHours = 12`
  Ý nghĩa / ngữ cảnh sử dụng: SLA mặc định khi tạo conversation.
  Lý do cần có data này: rule vận hành chat session.
  Đánh giá: Nguy hiểm
  Đề xuất cải thiện cụ thể: đưa vào `chat.default_sla_hours` trong `system_configs`.

- **Dòng 26**: `pageSize = 20`
  Ý nghĩa / ngữ cảnh sử dụng: pagination inbox mặc định.
  Lý do cần có data này: số hội thoại mỗi lần tải.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants/chat pagination policy.

### 74. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/application/actions/conversations.finance.ts`
- **Dòng 80**: `pageSize = 20`
  Ý nghĩa / ngữ cảnh sử dụng: pagination danh sách dispute admin.
  Lý do cần có data này: tải dữ liệu moderation.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants admin pagination.

### 75. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/application/useChatInboxPage.ts`
- **Dòng 20**: `listConversations(..., pageSize=100)`
  Ý nghĩa / ngữ cảnh sử dụng: inbox fetch 100 conversation/lần.
  Lý do cần có data này: giảm số lần paging.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuẩn hóa với API page size policy.

- **Dòng 30**: `staleTime = 30000`
  Ý nghĩa / ngữ cảnh sử dụng: cache inbox 30s.
  Lý do cần có data này: tần suất refetch.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config chat cache policy.

- **Dòng 42**: `setInterval(..., 60000)`
  Ý nghĩa / ngữ cảnh sử dụng: tick thời gian tương đối mỗi phút.
  Lý do cần có data này: cập nhật timestamp hiển thị.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants UI runtime.

### 76. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/application/chat-connection/utils.ts`
- **Dòng 7**: `CHAT_PAGE_SIZE = 50`
  Ý nghĩa / ngữ cảnh sử dụng: limit load message mỗi trang chat history.
  Lý do cần có data này: cân bằng scroll hiệu năng.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `chat.history_page_size`.

### 77. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/application/chat-connection/useChatHistoryState.ts`
- **Dòng 48**: `setTimeout(..., 10)`
  Ý nghĩa / ngữ cảnh sử dụng: delay nhỏ để scroll bottom sau initial render.
  Lý do cần có data này: tránh scroll trước khi DOM ổn định.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants local.

### 78. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/application/chat-connection/useChatSignalRLifecycle.ts`
- **Dòng 47**: reconnect schedule `[0, 2000, 5000, 10000, 30000]`
  Ý nghĩa / ngữ cảnh sử dụng: tự reconnect SignalR room chat.
  Lý do cần có data này: resilience realtime.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: shared realtime reconnect config.

- **Dòng 89**: `120000`
  Ý nghĩa / ngữ cảnh sử dụng: server timeout chat hub.
  Lý do cần có data này: giới hạn im lặng kết nối.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: dùng config chung realtime.

- **Dòng 123**: `2500`
  Ý nghĩa / ngữ cảnh sử dụng: timeout hiển thị typing indicator.
  Lý do cần có data này: UX typing presence.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `chat.typing_timeout_ms`.

- **Dòng 136-137**: `2000`, `limit:1`
  Ý nghĩa / ngữ cảnh sử dụng: debounce refetch conversation status và lấy 1 message để sync.
  Lý do cần có data này: giảm request dư.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config `chat.status_refresh_debounce_ms`.

### 79. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/server/prefetch/runners.ts`
- **Dòng 49**: query key readers mặc định có `pageSize=12`
  Ý nghĩa / ngữ cảnh sử dụng: SSR prefetch readers directory.
  Lý do cần có data này: hiệu năng first load.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: constants/shared pagination.

- **Dòng 67**: `getFeedAction(..., pageSize=10)`
  Ý nghĩa / ngữ cảnh sử dụng: prefetch feed public/private.
  Lý do cần có data này: cân bằng payload SSR.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: config community feed page size.

- **Dòng 92**: `listConversations(..., pageSize=100)`
  Ý nghĩa / ngữ cảnh sử dụng: prefetch inbox active shell.
  Lý do cần có data này: dữ liệu ban đầu chat.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: thống nhất với chat page policy.

- **Dòng 157, 183, 363**: `60_000`, `30_000`, `60_000`
  Ý nghĩa / ngữ cảnh sử dụng: staleTime cho reading setup/chat shell/navbar snapshot.
  Lý do cần có data này: cache TTL SSR hydrate.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: `prefetch.cache_policy` theo route.

- **Dòng 173, 207, 234-235, 245-246, 273-275, 282, 295-299, 458-459, 479, 491, 513, 545**:
  Ý nghĩa / ngữ cảnh sử dụng: nhiều default page/pageSize (`10`, `20`, `6`, `1`, `100`) cho prefetch admin/user modules.
  Lý do cần có data này: bootstrap dữ liệu SSR.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: chuẩn hóa constants pagination theo từng module, tránh phân tán.

### 80. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/domain/theme.ts`
- **Dòng 6**: `60 * 60 * 24 * 365`
  Ý nghĩa / ngữ cảnh sử dụng: thời hạn cookie theme 1 năm.
  Lý do cần có data này: persist lựa chọn giao diện.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cho phép override từ config hoặc privacy policy.

### 81. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/shared/domain/tarotData.ts`
- **Dòng 8**: `78`
  Ý nghĩa / ngữ cảnh sử dụng: tổng số lá bài tarot.
  Lý do cần có data này: domain invariant deck chuẩn.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants, không cần DB/config.

- **Dòng 11-14**: ngưỡng suit `21`, `35`, `49`, `63`
  Ý nghĩa / ngữ cảnh sử dụng: chia card id theo nhóm major/wands/cups/swords/pentacles.
  Lý do cần có data này: domain mapping cố định.
  Đánh giá: Hợp lý
  Đề xuất cải thiện cụ thể: giữ constants + unit test bảo vệ.

### 82. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/profile/application/useProfilePage.ts`
- **Dòng 51**: `displayName min 2`
  Ý nghĩa / ngữ cảnh sử dụng: validate tối thiểu tên hiển thị.
  Lý do cần có data này: hạn chế dữ liệu rỗng/không hợp lệ.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: lấy schema từ backend metadata.

### 83. File: `/Users/lucifer/Desktop/TarotNowAI2/Frontend/src/features/chat/presentation/chat-room/useChatRoomInputMediaActions.ts`
- **Dòng 60**: `1200`
  Ý nghĩa / ngữ cảnh sử dụng: timeout tự gửi `typing=false`.
  Lý do cần có data này: giảm spam typing events.
  Đánh giá: Cần cải thiện
  Đề xuất cải thiện cụ thể: cấu hình `chat.typing_stop_delay_ms`.

---

## Ghi chú kỹ thuật quan trọng
- Có nhiều hard-code FE hiện đang trùng loại nhưng khác giá trị giữa các layer (ví dụ pageSize wallet/chat, min withdraw 500 vs disable button 50). Đây là nguồn tạo bug khó phát hiện.
- Các giá trị kinh doanh nhạy cảm (pricing reading, follow-up tiers/caps, economy rate, withdrawal policy, reader minimum) cần bỏ khỏi FE càng sớm càng tốt. FE chỉ nên hiển thị từ dữ liệu policy BE.
- Nên thêm một endpoint snapshot policy (hoặc mở rộng metadata endpoint) để FE nhận đầy đủ policy runtime từ `system_configs` và cache ngắn hạn.
