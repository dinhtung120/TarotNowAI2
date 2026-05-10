# Backend Architecture Review

## Executive Summary

- **Overall architecture score:** 7/10 — backend có hướng Clean Architecture rõ, tách `Api`, `Application`, `Domain`, `Infrastructure`, có architecture tests và outbox; nhưng boundary thực tế còn nhiều điểm lệch ở API/realtime/config.
- **Maintainability score:** 6.5/10 — codebase đã chia feature khá tốt, nhưng diện tích rất lớn: `Backend/src/TarotNow.Application` có 649 file C#, `Backend/src/TarotNow.Api` có 55 controller files, nhiều partial/controller/service/helper làm tăng chi phí hiểu hệ thống.
- **Scalability score:** 6.5/10 — transaction/outbox/index khá mạnh, nhưng realtime dedup còn local-memory, Redis fallback theo env string có thể làm vỡ multi-instance consistency.
- **Technical debt score:** 6/10 — debt chính nằm ở route/error/auth extraction inconsistency, helper/common sprawl, reflection/event type string contracts, và test gaps cho middleware/security.
- **Security score:** 7/10 — auth/cookie/JWT/rate-limit/CORS có nền tốt, nhưng forwarded-header trust không đồng bộ, Redis fallback ngoài `Production` gây risk khi deploy env đặt sai tên.

**Inventory chính:**

- Không thấy `.sln` dưới `Backend`; có `Directory.Build.props`, `global.json`, 4 source projects và 6 test projects.
- Source C# count: `TarotNow.Api` 148, `TarotNow.Application` 649, `TarotNow.Domain` 135, `TarotNow.Infrastructure` 379.
- Test C# count: 103, gồm API integration, Application unit, Architecture tests, Domain unit, Infrastructure integration/unit.
- `Backend/src/TarotNow.Application/Features` có 429 file; `Common` 55, `DomainEvents` 72, `Interfaces` 75.
- `Backend/src/TarotNow.Infrastructure/Persistence` có 181 file; `Migrations` 69; `Services` 56.
- Top large files chủ yếu là EF migration designer/snapshot; đây không phải god-service runtime, nhưng làm repo nặng và review diff khó.

**Kết luận nhanh:** nền kiến trúc tốt hơn mức trung bình, nhưng production risk còn ở consistency khi scale, contract stability của outbox/realtime, và chuẩn hóa API/auth/error. Refactor nên bắt đầu bằng guardrails/tests, không rewrite lớn ngay.

---

# Critical Issues

Không ghi nhận issue Critical chắc chắn trong phạm vi audit rộng này. Các luồng tiền/AI/auth có guard đáng kể: transaction, idempotency, lock, outbox, cookie/JWT, CORS/rate limit. Tuy nhiên các High issues bên dưới có thể trở thành Critical nếu hệ thống chạy multi-instance hoặc deploy production với cấu hình lệch.

---

# High Priority Issues

## H-01 Realtime bridge dedup dùng memory local, không bền khi scale/restart

- **Severity:** High
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.cs:31` có `_bridgeDedupByEventId = new ConcurrentDictionary<string, DateTime>`, `ShouldSkipDuplicatedFastLaneEvent` ở cùng file dùng `TryAdd` để bỏ duplicate; `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.Forwarding.FastLane.cs:12` gọi check này.
- **Root cause:** dedup state nằm trong process memory thay vì distributed idempotency store.
- **Impact:** khi chạy nhiều instance, restart, rolling deploy, hoặc Redis pub/sub resend, cùng event có thể fanout nhiều lần tới SignalR. UI có thể nhận duplicate chat/unread/conversation update; finance/status event duplicate dễ gây trạng thái client sai.
- **Fix direction:** chuyển dedup sang Redis `SET NX EX` theo `eventId` + TTL; thêm event sequence/version trong envelope; client apply idempotent theo event id.

## H-02 Outbox lưu event type bằng `GetType().FullName`, dễ vỡ khi refactor namespace/class

- **Severity:** High
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs:38` lấy `domainEvent.GetType().FullName`; `:48` gán vào `OutboxMessage.EventType`; `Backend/src/TarotNow.Infrastructure/Persistence/Outbox/OutboxMessage.cs:16` lưu `EventType` dạng string.
- **Root cause:** internal CLR type name đang bị dùng như wire/storage contract.
- **Impact:** rename class, move namespace, split assembly, hoặc version event sẽ làm outbox rows cũ không deserialize/dispatch đúng. Side effects như notification/realtime/projection có thể stuck sau deploy.
- **Fix direction:** định nghĩa stable event contract name + version (`money.changed.v1`, `conversation.updated.v1`); registry map stable name → CLR type; migration/backward compatibility binder cho rows cũ.

## H-03 Redis fallback theo tên môi trường `Production` có thể phá consistency ngoài local

- **Severity:** High
- **Confidence:** Medium
- **Evidence:** `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs:61` dùng `services.AddDistributedMemoryCache()` khi không require Redis; subagent scan xác định Redis chỉ strict theo env name `Production` ở cùng file. `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/RefreshTokenRepository.Rotate.cs` có fail-closed riêng cho refresh consistency, nhưng cache/realtime rộng hơn vẫn có thể degrade.
- **Root cause:** policy hạ tầng dựa vào string environment thay vì config explicit theo deployment tier.
- **Impact:** staging/preprod/prod-like env đặt tên khác `Production` có thể chạy memory cache, làm vỡ multi-instance locking, revocation, presence/realtime consistency, cache invalidation.
- **Fix direction:** thêm config `Cache:RequireRedis=true`; chỉ allow memory cache trong `Development`/test explicit; startup fail-fast nếu non-local thiếu Redis.

## H-04 AI streaming có policy consume escrow khi client disconnect sau first token

- **Severity:** High
- **Confidence:** Medium
- **Evidence:** `Backend/src/TarotNow.Api/Services/AiStreamSseOrchestrator.Completion.cs` có `FailedAfterFirstToken`, `clientDisconnected`, và final status/finish reason logic; grep thấy nhánh `FailedAfterFirstToken` gắn với disconnect. Evidence cần đọc sâu thêm settlement handler để xác nhận exact consume path.
- **Root cause:** hệ thống dùng “đã có token đầu tiên” như proxy cho delivered value khi client disconnect.
- **Impact:** mạng chập chờn/browser close có thể bị tính phí dù user nhận quá ít nội dung hữu ích; tăng dispute/refund thủ công.
- **Fix direction:** chuyển sang threshold settlement: min output tokens/chunks/time + terminal evidence; hoặc partial refund matrix. Log proof fields: first-token time, emitted chunks, output tokens, disconnect source.

---

# Medium Priority Issues

## M-01 Error response contract không nhất quán

- **Severity:** Medium
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler.cs` trả ProblemDetails; `Backend/src/TarotNow.Api/Controllers/ReaderController.ReaderFlow.cs` dùng `Problem()`; `Backend/src/TarotNow.Api/Controllers/TarotController.cs` trả anonymous error object; `Backend/src/TarotNow.Api/Controllers/GamificationController.cs` dùng plain detail code như `QUEST_ALREADY_CLAIMED`.
- **Root cause:** controller tự quyết định error shape thay vì đi qua contract/helper thống nhất.
- **Impact:** frontend phải special-case; telemetry/error tracking khó group; API client khó dự đoán response.
- **Fix direction:** chuẩn hóa mọi non-2xx về ProblemDetails có `errorCode`, `correlationId`, `details`; thêm architecture/API tests cấm anonymous error object trong controller public endpoints.

## M-02 User identity extraction không thống nhất

- **Severity:** Medium
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Api/Extensions/ClaimsPrincipalExtensions.cs` có `TryGetUserId` fallback `NameIdentifier`/`sub`; nhưng `Backend/src/TarotNow.Api/Controllers/GamificationController.cs:44`, `Backend/src/TarotNow.Api/Controllers/InventoryController.cs:83`, `Backend/src/TarotNow.Api/Controllers/HistoryController.cs:54-56` parse `ClaimTypes.NameIdentifier` trực tiếp.
- **Root cause:** không có guard test/code convention bắt buộc dùng extension canonical.
- **Impact:** nếu claim mapping thay đổi hoặc token chỉ có `sub`, một số endpoint fail 401/400 trong khi endpoint khác hoạt động.
- **Fix direction:** thay direct claim parsing bằng `User.TryGetUserId`; thêm architecture test cấm `FindFirstValue(ClaimTypes.NameIdentifier)` trong controllers/hubs trừ extension file.

## M-03 Forwarded header trust không áp dụng đồng bộ trong auth cookie logic

- **Severity:** Medium
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Api/Services/AuthCookieService.cs:107` đọc `x-forwarded-proto`; `:118` đọc `x-forwarded-host` trực tiếp. Subagent scan xác định `ForwardedHeaderTrustEvaluator` được dùng ở `AuthService` cho forwarded user-agent, nhưng không dùng trong `AuthCookieService`.
- **Root cause:** trust boundary logic bị phân tán theo service.
- **Impact:** trong deployment không chuẩn proxy/header sanitize, spoofed forwarded headers có thể ảnh hưởng cookie secure/domain behavior.
- **Fix direction:** inject `IForwardedHeaderTrustEvaluator` vào `AuthCookieService`; bỏ qua forwarded headers nếu remote IP không trusted; ưu tiên server features sau `UseForwardedHeaders`.

## M-04 Inline domain event dispatcher dùng reflection runtime brittle

- **Severity:** Medium
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Infrastructure/Services/InlineMediatRDomainEventDispatcher.cs:31` lấy runtime event type; `:43` throw nếu không tìm constructor `DomainEventNotification`; `:52` throw nếu không tạo được notification.
- **Root cause:** constructor signature là runtime convention, không validate ở startup/compile time.
- **Impact:** đổi constructor/event notification làm request fail khi dispatch event, có thể chỉ lộ ở path runtime hiếm.
- **Fix direction:** build registry/factory tại startup, validate tất cả domain event notification constructors; hoặc dùng generic/static factory thay reflection mỗi dispatch.

## M-05 Provider/config validation fail-fast nhưng thiếu preflight report rõ ràng

- **Severity:** Medium
- **Confidence:** Medium
- **Evidence:** subagent scan xác định `Backend/src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.cs` throw khi thiếu key/model/baseUrl. Đây là fail-safe, nhưng operationally sharp.
- **Root cause:** DI constructor validation là nơi phát hiện config thay vì centralized options validation/readiness.
- **Impact:** secret rotation/config typo có thể brick full app startup với diagnostic khó gom nếu nhiều provider/options lỗi cùng lúc.
- **Fix direction:** thêm `IValidateOptions`/startup preflight tổng hợp lỗi config; health/readiness expose provider disabled/misconfigured rõ ràng; vẫn fail closed cho production.

## M-06 Telemetry write nuốt exception, chỉ warning

- **Severity:** Medium
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.Telemetry.cs:33` catch `Exception`; subagent scan thấy catch-all warning-only.
- **Root cause:** telemetry được thiết kế non-blocking nhưng thiếu backpressure/alert/counter.
- **Impact:** cost/audit trace thiếu mà request vẫn thành công; khó phát hiện provider/db telemetry outage.
- **Fix direction:** giữ non-blocking nhưng emit metric/counter, structured event, health signal; cân nhắc retry/buffer bounded cho telemetry critical fields.

## M-07 Route convention drift gây contract khó đoán

- **Severity:** Medium
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Api/Constants/ApiRoutes.cs` có `/api/v1/deposits` plural, `/api/v1/withdrawal` singular, `/api/v1/reading` singular; nhiều controller dùng route constants, một số dùng `[Route(ApiRoutes.Controller)]`.
- **Root cause:** route style guide chưa enforce bằng test.
- **Impact:** API client và docs khó nhất quán; versioning/deprecation khó quản lý.
- **Fix direction:** chuẩn hóa plural noun resources cho v2 hoặc alias/deprecation cho v1; add API route snapshot test.

## M-08 Middleware/security behavior thiếu test trực tiếp

- **Severity:** Medium
- **Confidence:** High
- **Evidence:** subagent scan không thấy tests trực tiếp cho `GlobalExceptionHandler`, `CorrelationIdMiddleware`, `ChatFeatureGateMiddleware`, forwarded-header trust cases trong `Backend/tests`; architecture/order checks có nhưng không thay thế behavior tests.
- **Root cause:** cross-cutting middleware dựa vào integration smoke thay vì matrix tests.
- **Impact:** regression security/error/observability dễ lọt qua unit test feature.
- **Fix direction:** thêm test matrix: exception → status/title/errorCode, correlation ID sanitization/propagation, chat feature gate 404, trusted/untrusted proxy header cases.

---

# Low Priority Issues

## L-01 Startup pipeline tạo thư mục upload trực tiếp

- **Severity:** Low
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Api/Startup/ApiApplicationBuilderExtensions.cs:175` gọi `Directory.CreateDirectory(uploadsPath)`.
- **Root cause:** startup pipeline extension chứa filesystem side effect.
- **Impact:** container read-only filesystem hoặc permission thiếu có thể fail startup; trách nhiệm storage bootstrap lẫn vào middleware composition.
- **Fix direction:** chuyển sang hosted startup check/storage bootstrap service, log rõ path/permission, readiness báo lỗi.

## L-02 Console stderr trong infrastructure bootstrap không thống nhất observability

- **Severity:** Low
- **Confidence:** High
- **Evidence:** `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs:141` và `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.Bootstrap.cs:17` dùng `Console.Error.WriteLine`.
- **Root cause:** bootstrap path chưa dùng structured logger.
- **Impact:** log correlation/JSON pipeline có thể mất context; alerting khó bắt.
- **Fix direction:** dùng bootstrap logger factory hoặc defer logging sang hosted validator có `ILogger`.

## L-03 Controller surface lớn, partial split giúp nhưng vẫn tăng drift

- **Severity:** Low
- **Confidence:** Medium
- **Evidence:** `Backend/src/TarotNow.Api/Controllers` có 55 controller files; `ConversationController` split nhiều partial: `Acceptance`, `Completion`, `Finance`, `Inbox`, `MediaUpload`, `Messages`, `Review`.
- **Root cause:** API layer theo domain lớn nhưng consistency rules cho auth/error/idempotency chưa centralized đủ.
- **Impact:** cùng domain dễ lệch user extraction, error shape, authorization policy, response codes.
- **Fix direction:** giữ partial nếu size guard cần, nhưng thêm shared helper/action filter/test guard cho cross-cutting rules.

## L-04 `Common`/`Helpers` sprawl ở Application làm mờ boundary

- **Severity:** Low
- **Confidence:** Medium
- **Evidence:** `Backend/src/TarotNow.Application/Common` có 55 file; có `Common/Helpers`, `Application/Helpers`; helper-suffixed files rải ở Application/Infrastructure/Api.
- **Root cause:** code reuse gom theo kỹ thuật thay vì bounded-context responsibility.
- **Impact:** dễ biến thành god shared layer, coupling ngược giữa feature domains.
- **Fix direction:** khi chạm code, kéo helper về feature/context nếu chỉ dùng cục bộ; chỉ giữ shared abstractions có contract rõ và nhiều bounded contexts dùng.

---

# Dead Code Report

Audit này là scan rộng, không chạy IDE/compiler reference analysis. Các mục dưới đây là **candidate**, không phải proof tuyệt đối.

| Candidate | Path | Evidence | Confidence | Recommendation |
|---|---|---|---|---|
| Commented/temporary debt markers | `Backend/src`, `Backend/tests` | Scan marker nợ kỹ thuật, obsolete APIs, unsupported/not-implemented paths, và commented-out code; cần chạy sâu hơn nếu muốn xóa hàng loạt. | Low | Tạo task riêng dùng Roslyn/IDE references + compiler warnings trước khi delete. |
| Generic helpers/common files | `Backend/src/TarotNow.Application/Common`, `Backend/src/TarotNow.Application/Common/Helpers`, `Backend/src/TarotNow.Application/Helpers` | Inventory thấy 55 file trong `Common`, helper folder ở Application, helper suffix rải nhiều layer. | Medium | Không xóa ngay; phân loại helper theo bounded context và move dần khi có change liên quan. |
| Route/error custom payload patterns | `Backend/src/TarotNow.Api/Controllers/TarotController.cs`, `Backend/src/TarotNow.Api/Controllers/GamificationController.cs` | Anonymous error/plain detail code có thể là legacy API contract. | Medium | Verify frontend consumers trước khi thay bằng ProblemDetails chuẩn. |
| EF migration designer bulk | `Backend/src/TarotNow.Infrastructure/Migrations/*.Designer.cs` | Top 25 largest C# files chủ yếu là migration designer/snapshot. | High (not dead code) | Không xóa nếu migrations còn cần; cân nhắc migration squashing chỉ khi deployment policy cho phép. |

---

# Architecture Violations

## Confirmed / Strong candidates

1. **Realtime idempotency boundary chưa distributed**
   - Path: `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.cs`
   - Vấn đề: event dedup thuộc consistency concern nhưng đặt local memory trong API process.
   - Hướng sửa: Redis-backed dedup hoặc Infrastructure/Application-owned idempotent realtime publisher contract.

2. **Outbox event storage contract phụ thuộc CLR type name**
   - Path: `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs`, `Backend/src/TarotNow.Infrastructure/Persistence/Outbox/OutboxMessage.cs`
   - Vấn đề: persistence contract không stable qua refactor.
   - Hướng sửa: stable event names + versions + registry.

3. **Controller auth/error conventions không enforced**
   - Path: `Backend/src/TarotNow.Api/Controllers/*`, `Backend/src/TarotNow.Api/Extensions/ClaimsPrincipalExtensions.cs`
   - Vấn đề: API layer tự parse claims/return errors nhiều kiểu.
   - Hướng sửa: architecture tests + endpoint behavior tests.

4. **Config safety phụ thuộc environment string**
   - Path: `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
   - Vấn đề: deployment safety should be config/tier policy, not just `Production` name.
   - Hướng sửa: explicit `RequireRedis` config + fail-fast.

## Positive architecture findings

- Layering tổng thể đúng hướng: `Domain`, `Application`, `Infrastructure`, `Api` tách rõ.
- `Backend/tests/TarotNow.ArchitectureTests` tồn tại, cho thấy project đã có guardrails.
- Money/settlement areas có dấu hiệu dùng lock, transaction, idempotency, outbox — tốt cho production correctness.
- Database indexing posture nhìn chung tốt: nhiều EF/Mongo index configuration, không thấy N+1 hotspot chắc chắn trong sampled scan.

---

# Refactor Recommendations

## P0/P1 — Guardrails trước refactor

1. Thêm tests/architecture guards cho:
   - Cấm direct `FindFirstValue(ClaimTypes.NameIdentifier)` trong controllers/hubs trừ extension canonical.
   - Cấm anonymous error object trong API public controllers.
   - Cấm route drift mới bằng route snapshot/style test.
   - Bắt buộc Redis cho non-local tiers.

2. Harden realtime/outbox contract:
   - Redis-backed realtime dedup.
   - Stable outbox event type registry/versioning.
   - Backward compatibility cho existing outbox rows.

3. Add high-risk regression tests:
   - AI disconnect/partial output settlement.
   - Transaction scope exists before outbox publish for money/AI commands.
   - Refresh/session/cache behavior with Redis required/missing.

## P2 — API consistency cleanup

1. Chuẩn hóa ProblemDetails + `errorCode`.
2. Chuẩn hóa user id extraction.
3. Chuẩn hóa route naming cho v2 hoặc alias/deprecation trong v1.
4. Gom forwarded-header trust vào một service duy nhất.

## P3 — Maintainability cleanup

1. Giảm `Common/Helpers` bằng move-to-feature khi chạm code.
2. Tách controller partials chỉ theo bounded use case; dùng shared filters/helper cho cross-cutting concerns.
3. Đổi bootstrap console logs sang structured logger.
4. Tách startup filesystem side effects khỏi pipeline extension.

---

# Suggested New Structure

Không nên rewrite toàn backend ngay. Target structure nên tiến hóa dần:

```text
Backend/src/
  TarotNow.Domain/
    Wallet/
      Entities/
      ValueObjects/
      Events/
    Conversation/
      Entities/
      Events/
    Ai/
      ValueObjects/
      Events/
    Auth/
      Entities/
      Events/

  TarotNow.Application/
    Wallet/
      Commands/
      Queries/
      EventHandlers/
      Contracts/
    Conversation/
      Commands/
      Queries/
      EventHandlers/
      Contracts/
    Ai/
      Commands/
      Queries/
      Policies/
      Contracts/
    Auth/
      Commands/
      Queries/
      Contracts/
    SharedKernel/
      Behaviors/
      Errors/
      Idempotency/

  TarotNow.Infrastructure/
    Persistence/
      Postgres/
      Mongo/
      Outbox/
    Messaging/
      EventRegistry/
      Dispatching/
    Providers/
      Ai/
      Payment/
      Email/
    Realtime/
      RedisDedup/
      PubSub/
    Security/
      Tokens/
      Cookies/
      ProxyTrust/

  TarotNow.Api/
    Controllers/
      Wallet/
      Conversation/
      Ai/
      Auth/
    Hubs/
    Middleware/
    Filters/
    Startup/
```

Nguyên tắc:

- Domain không biết persistence/web/provider.
- Application giữ use-case orchestration và contracts.
- Infrastructure implement persistence/provider/outbox/realtime.
- API chỉ map HTTP/SignalR → Application entry points.
- `SharedKernel` chỉ chứa behavior/primitive thực sự cross-context, không chứa business helper tùy tiện.

---

# Immediate Fixes

1. **Realtime dedup distributed**
   - File: `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.cs`
   - Thêm Redis `SET NX` TTL theo event id hoặc chuyển dedup sang Infrastructure service.

2. **Stable outbox event contract**
   - File: `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs`
   - Thêm event name/version registry; giữ fallback đọc `FullName` cũ trong migration window.

3. **Redis required config**
   - File: `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
   - Thêm `Cache:RequireRedis`; fail-fast ngoài local/test.

4. **Canonical user id extraction**
   - Files: `Backend/src/TarotNow.Api/Controllers/GamificationController.cs`, `Backend/src/TarotNow.Api/Controllers/InventoryController.cs`, `Backend/src/TarotNow.Api/Controllers/HistoryController.cs`
   - Dùng `User.TryGetUserId`.

5. **ProblemDetails standardization**
   - Files: `Backend/src/TarotNow.Api/Controllers/TarotController.cs`, `Backend/src/TarotNow.Api/Controllers/GamificationController.cs`, `Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler.cs`
   - Thêm helper `Problem(errorCode, detail, status)` hoặc domain exception mapping.

6. **Forwarded header trust**
   - File: `Backend/src/TarotNow.Api/Services/AuthCookieService.cs`
   - Không đọc `x-forwarded-*` nếu remote IP không trusted.

7. **Middleware tests**
   - Add tests cho `GlobalExceptionHandler`, `CorrelationIdMiddleware`, `ChatFeatureGateMiddleware`, forwarded-header trust.

---

# Long-term Re-Architecture Plan

## Phase 1 — Guardrails and tests

- Mở rộng `Backend/tests/TarotNow.ArchitectureTests` cho API identity/error/route/config rules.
- Thêm integration tests cho middleware/security behavior.
- Thêm transaction/idempotency tests cho finance/AI paths.

## Phase 2 — Event/outbox/realtime consistency

- Introduce event contract registry + versioning.
- Migrate existing outbox rows or keep compatibility resolver.
- Move realtime dedup to Redis/distributed store.
- Add client idempotent apply contract for realtime events.

## Phase 3 — Finance/AI hardening

- Define AI settlement matrix: success, provider fail before token, provider fail after token, client disconnect before/after threshold, timeout.
- Add dispute evidence fields.
- Validate transaction scope around any money/quota/outbox publish path.
- Add runtime metrics for refund/consume decisions.

## Phase 4 — API contract normalization

- Standardize ProblemDetails + errorCode.
- Standardize route naming under next API version.
- Remove direct claim parsing.
- Consolidate forwarded-header trust.

## Phase 5 — Module boundary cleanup

- Reduce `Common/Helpers` by moving code into feature/domain context.
- Split or normalize large API partial controller families only when test coverage exists.
- Keep current Clean Architecture, avoid microservice split until module contracts and event contracts are stable.

---

# Final Verdict

Backend TarotNowAI2 có nền production khá tốt: Clean Architecture rõ, event/outbox có mặt, transaction/idempotency xuất hiện ở vùng tiền, indexing posture ổn, auth/cookie/rate-limit không sơ sài. Vấn đề lớn không phải “thiếu kiến trúc”, mà là **kiến trúc chưa được enforce đủ đồng đều khi codebase lớn lên**.

Ưu tiên cao nhất không nên là rewrite. Nên làm theo thứ tự: guardrails/tests → realtime/outbox consistency → AI/finance settlement hardening → API contract cleanup → module cleanup. Nếu làm đúng thứ tự này, hệ thống có thể tăng maintainability và production safety mà không tạo rủi ro rewrite lớn.
