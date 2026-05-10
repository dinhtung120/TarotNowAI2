# Backend Architecture Fix Plan

## Executive Summary

Kế hoạch này xử lý toàn bộ finding đã verify trong `docs/backend-architecture-review.md`. Không nên fix bằng một PR lớn. Thứ tự an toàn là: guardrails/tests trước, security/correctness nhỏ tiếp theo, rồi realtime/outbox/AI consistency, sau cùng mới cleanup maintainability.

Nguyên tắc thực thi:

- Không đổi behavior production khi chưa có regression tests.
- Money/quota/AI/realtime changes phải có test idempotency/concurrency hoặc integration coverage tương ứng.
- API contract cleanup cần kiểm tra frontend consumers trước khi đổi response shape public.
- Low-priority maintainability cleanup chỉ làm khi có test guard, tránh refactor lớn không cần thiết.

## Verified Issue Matrix

| Issue | Verification status | Final severity | Confidence | Remediation task |
|---|---|---:|---|---|
| H-01 | Confirmed | High | High | T3.1 Distributed realtime dedup |
| H-02 | Confirmed | High | High | T3.2 Stable outbox event contract |
| H-03 | Confirmed | High | Medium | T2.1 Explicit Redis requirement policy |
| H-04 | Confirmed | High | High | T2.2 AI disconnect settlement threshold |
| M-01 | Confirmed | Medium | High | T4.1 ProblemDetails contract normalization |
| M-02 | Confirmed | Medium | High | T1.1 Identity extraction guard and cleanup |
| M-03 | Confirmed | Medium | High | T2.3 Forwarded header trust in auth cookies |
| M-04 | Confirmed | Medium | High | T3.3 Domain event notification factory validation |
| M-05 | Corrected | Low | High | T2.4 AI provider readiness/preflight signal |
| M-06 | Confirmed | Medium | High | T2.5 AI telemetry failure signal |
| M-07 | Confirmed | Medium | High | T4.2 API route style guard |
| M-08 | Corrected | Medium | Medium | T1.2 Middleware/security behavior tests |
| L-01 | Confirmed | Low | High | T5.1 Upload directory bootstrap cleanup |
| L-02 | Confirmed | Low | High | T5.2 Structured bootstrap logging |
| L-03 | Corrected | Low | High | T5.3 Controller partial drift guardrails |
| L-04 | Corrected | Low | High | T5.4 Common/Helpers cleanup policy |
| Dead Code Report | Corrected | Low/Medium | Medium | T5.5 Debt candidate triage |
| Architecture Violations | Corrected | Medium/High | High | Covered by T1-T5 |

## Phase 1 — Guardrails And Baseline Tests

### T1.1 Add architecture guard for canonical user identity extraction

- **Covers:** M-02, Architecture Violations #3
- **Files to touch:**
  - `Backend/tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs`
  - Later cleanup files: `Backend/src/TarotNow.Api/Controllers/GamificationController.cs`, `Backend/src/TarotNow.Api/Controllers/InventoryController.cs`, `Backend/src/TarotNow.Api/Controllers/HistoryController.cs`
- **Implementation direction:**
  - Add an architecture test that scans `Backend/src/TarotNow.Api/Controllers` and `Backend/src/TarotNow.Api/Hubs` for `FindFirstValue(ClaimTypes.NameIdentifier)` and direct `ClaimTypes.NameIdentifier` parsing.
  - Allow `Backend/src/TarotNow.Api/Extensions/ClaimsPrincipalExtensions.cs` as the canonical parser.
  - If rate-limit partitioning must keep direct claim parsing, allowlist only `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.RateLimit.Partitioning.cs` with explicit reason.
  - Replace controller direct parsing with `User.TryGetUserId(out var userId)`.
- **Tests to add/run:**
  - `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj`
  - Targeted API integration tests for affected endpoints if existing coverage exists.
- **Risk/rollback:**
  - Guard may fail before controller cleanup; land guard and cleanup in same PR or mark current violations in a temporary allowlist with deletion date.
- **Done criteria:**
  - No controller/hub uses direct `ClaimTypes.NameIdentifier` parsing outside the agreed allowlist.
  - Architecture test fails on a synthetic direct parsing sample and passes on current code.

### T1.2 Add middleware/security behavior tests

- **Covers:** M-08
- **Files to touch:**
  - `Backend/tests/TarotNow.Api.IntegrationTests/*` or existing API test project for middleware behavior.
  - If `Backend/tests/TarotNow.Api.Tests/Middleware/ChatFeatureGateMiddlewareTests.cs` exists in current branch, keep it and add missing cases nearby; otherwise create equivalent tests in the established API test project.
- **Implementation direction:**
  - Add tests for `GlobalExceptionHandler` mapping representative exceptions to ProblemDetails status/title/error code.
  - Add tests for `CorrelationIdMiddleware`: accepts valid correlation ID, rejects/sanitizes invalid or oversized value, propagates response header.
  - Add forwarded-header trust tests for auth cookie/header handling: trusted proxy applies forwarded headers; untrusted remote ignores them.
  - Keep `ChatFeatureGateMiddleware` tests, add only missing edge cases if coverage is partial.
- **Tests to add/run:**
  - `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj`
  - `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj`
- **Risk/rollback:**
  - Tests may need test-server config for proxy IPs and feature flags; isolate setup in test fixtures only.
- **Done criteria:**
  - Middleware/security behavior has direct tests for exception mapping, correlation ID, feature gate, and forwarded-header trust.

### T1.3 Add API error and route guardrails

- **Covers:** M-01, M-07
- **Files to touch:**
  - `Backend/tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs`
  - `Backend/tests/TarotNow.Api.IntegrationTests/*`
- **Implementation direction:**
  - Add static guard for anonymous error payload patterns in controllers, including object literals with `error` or `message` properties on non-2xx paths where ProblemDetails should be used.
  - Add route snapshot/style test for `ApiRoutes` public constants: new routes must follow the chosen singular/plural convention.
  - Do not rename v1 routes immediately; start by preventing new drift.
- **Tests to add/run:**
  - `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj`
  - `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj`
- **Risk/rollback:**
  - Public v1 route changes are breaking; tests should document current exceptions instead of forcing instant rename.
- **Done criteria:**
  - Existing drift is documented/allowlisted.
  - New route/error drift fails architecture tests.

## Phase 2 — Correctness And Security Fixes

### T2.1 Explicit Redis requirement policy

- **Covers:** H-03
- **Files to touch:**
  - `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
  - `Backend/src/TarotNow.Infrastructure/Options/*` or existing options file for cache/Redis settings
  - `Backend/src/TarotNow.Api/appsettings.json`
  - `Backend/tests/TarotNow.Infrastructure.UnitTests/*`
  - `Backend/tests/TarotNow.Api.IntegrationTests/*`
- **Implementation direction:**
  - Introduce explicit config such as `Redis:RequireRedis` or `Cache:RequireRedis`.
  - Default behavior: `Development` and test may allow memory fallback; production-like deployments must set explicit value.
  - Fail fast if `RequireRedis=true` and connection string/instance name/bootstrap settings are missing.
  - Keep `RequireRedisForRefreshConsistency` semantics, but avoid relying on refresh-token code as the only fail-closed path.
- **Tests to add/run:**
  - Unit tests for config matrix: require Redis true/missing config throws; local false falls back to memory; production defaults safe.
  - `dotnet test Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj`
  - `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj`
- **Risk/rollback:**
  - Can break misconfigured staging deployments. Roll out with config in environment first, then enforce fail-fast.
- **Done criteria:**
  - Redis requirement is explicit by config, not only `ASPNETCORE_ENVIRONMENT == Production`.

### T2.2 AI disconnect settlement threshold

- **Covers:** H-04
- **Files to touch:**
  - `Backend/src/TarotNow.Api/Services/AiStreamSseOrchestrator.Completion.cs`
  - `Backend/src/TarotNow.Api/Services/AiStreamSseOrchestrator.Streaming.cs`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommand*.cs`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommandHandler.Processing.cs`
  - `Backend/tests/TarotNow.Application.UnitTests/Features/Reading/*`
- **Implementation direction:**
  - Define settlement matrix for stream final states:
    - completed: consume escrow;
    - failed before first token: refund;
    - provider failed after first token: refund unless policy says partial consume;
    - client disconnect after first token but below threshold: refund or partial refund;
    - client disconnect after threshold: consume or partial consume.
  - Add threshold fields to completion command if needed: emitted chunk count, output token count, elapsed stream duration, disconnect source.
  - Use config constants for minimum billable output threshold.
  - Emit telemetry fields for settlement reason.
- **Tests to add/run:**
  - Unit tests for every settlement matrix row.
  - Regression test proving current `FailedAfterFirstToken + IsClientDisconnect` no longer blindly consumes below threshold.
  - `dotnet test Backend/tests/TarotNow.Application.UnitTests/TarotNow.Application.UnitTests.csproj`
- **Risk/rollback:**
  - Billing behavior changes; coordinate product/legal. Rollback by restoring old policy behind config only if needed.
- **Done criteria:**
  - Escrow consume/refund decision is threshold-based and covered by tests.

### T2.3 Forwarded header trust in auth cookies

- **Covers:** M-03
- **Files to touch:**
  - `Backend/src/TarotNow.Api/Services/AuthCookieService.cs`
  - `Backend/src/TarotNow.Api/Services/ForwardedHeaderTrustEvaluator.cs`
  - `Backend/src/TarotNow.Api/Startup/ApiServiceCollectionExtensions.Platform.cs`
  - `Backend/tests/TarotNow.Api.IntegrationTests/*` or API unit tests for service behavior
- **Implementation direction:**
  - Inject `IForwardedHeaderTrustEvaluator` into `AuthCookieService`.
  - Ignore `x-forwarded-proto` and `x-forwarded-host` unless request remote IP is trusted.
  - Prefer normalized request scheme/host after `UseForwardedHeaders` where possible.
- **Tests to add/run:**
  - Trusted proxy applies forwarded scheme/host.
  - Untrusted remote ignores forwarded scheme/host.
  - `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj`
- **Risk/rollback:**
  - Cookie domain/secure behavior can change behind proxies; test with staging proxy config.
- **Done criteria:**
  - Auth cookie behavior no longer trusts raw forwarded headers from untrusted remotes.

### T2.4 AI provider readiness/preflight signal

- **Covers:** M-05
- **Files to touch:**
  - `Backend/src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.cs`
  - Existing readiness/health service files such as `Backend/src/TarotNow.Infrastructure/Services/ReadinessService.cs`
  - Option validation files if present
  - `Backend/tests/TarotNow.Infrastructure.UnitTests/*`
- **Implementation direction:**
  - Keep fail-closed config validation.
  - Add options validation or readiness detail that reports AI provider unavailable/misconfigured separately from generic startup failure.
  - Avoid logging secrets; report missing key names only.
- **Tests to add/run:**
  - Missing API key/model/base URL produces safe readiness/preflight message.
  - `dotnet test Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj`
- **Risk/rollback:**
  - Do not weaken fail-fast production behavior.
- **Done criteria:**
  - Operators can distinguish AI provider config failure from unrelated app health failure.

### T2.5 AI telemetry failure signal

- **Covers:** M-06
- **Files to touch:**
  - `Backend/src/TarotNow.Infrastructure/Services/Ai/OpenAiProvider.Telemetry.cs`
  - Metrics/observability infrastructure if present
  - `Backend/tests/TarotNow.Infrastructure.UnitTests/*`
- **Implementation direction:**
  - Keep telemetry non-blocking.
  - Add counter/metric/log event with stable event name when telemetry write fails.
  - Include provider, request id, status, and failure category; never include prompt/secret.
- **Tests to add/run:**
  - Telemetry write exception does not fail provider call.
  - Failure increments/emits observable signal.
  - `dotnet test Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj`
- **Risk/rollback:**
  - Avoid noisy logs/counters on transient failure; rate-limit or aggregate if needed.
- **Done criteria:**
  - Telemetry loss is observable without breaking AI responses.

## Phase 3 — Realtime, Outbox, And Consistency Fixes

### T3.1 Distributed realtime dedup

- **Covers:** H-01, Architecture Violations #1
- **Files to touch:**
  - `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.cs`
  - `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.Forwarding.FastLane.cs`
  - Redis/cache abstractions in `Backend/src/TarotNow.Application` or `Backend/src/TarotNow.Infrastructure`
  - `Backend/tests/TarotNow.Api.IntegrationTests/Realtime/*`
- **Implementation direction:**
  - Replace process-local dedup for cross-instance event id with Redis `SET NX` + TTL.
  - Keep small local memory cache only as optional optimization after distributed dedup succeeds.
  - Use event id as key: `realtime:dedup:{eventId}` with short TTL matching duplicate replay window.
  - If Redis unavailable and realtime requires Redis, fail closed or skip fast-lane dedup with explicit warning based on config policy.
- **Tests to add/run:**
  - Duplicate event id sent twice is forwarded once.
  - Simulated second bridge instance respects distributed key.
  - Redis unavailable behavior follows config.
  - `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj`
- **Risk/rollback:**
  - Redis latency on hot realtime path; use pipelining/low TTL and monitor latency.
- **Done criteria:**
  - Duplicate realtime fanout is prevented across process instances.

### T3.2 Stable outbox event contract

- **Covers:** H-02, Architecture Violations #2
- **Files to touch:**
  - `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs`
  - `Backend/src/TarotNow.Infrastructure/Persistence/Outbox/OutboxMessage.cs`
  - Outbox deserializer/processor files under `Backend/src/TarotNow.Infrastructure/BackgroundJobs/Outbox`
  - Domain event types under `Backend/src/TarotNow.Domain/Events`
  - `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/*`
- **Implementation direction:**
  - Introduce stable event name/version registry, e.g. `money.changed.v1`, `conversation.updated.v1`.
  - Persist stable name and version instead of raw CLR `FullName` for new rows.
  - Keep compatibility resolver for legacy rows whose `EventType` contains CLR FullName.
  - Add tests proving old rows still deserialize after registry introduction.
- **Tests to add/run:**
  - New outbox row stores stable event contract.
  - Legacy CLR FullName row still processes.
  - Unknown event name fails with observable dead-letter/error.
  - `dotnet test Backend/tests/TarotNow.Infrastructure.IntegrationTests/TarotNow.Infrastructure.IntegrationTests.csproj`
  - `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj`
- **Risk/rollback:**
  - Outbox compatibility is critical. Deploy reader compatibility before writer changes if split deploy is possible.
- **Done criteria:**
  - Event storage contract survives CLR rename/namespace move for newly written events.

### T3.3 Domain event notification factory validation

- **Covers:** M-04
- **Files to touch:**
  - `Backend/src/TarotNow.Infrastructure/Services/InlineMediatRDomainEventDispatcher.cs`
  - Domain event notification types in Application common/domain event folders
  - `Backend/tests/TarotNow.Infrastructure.UnitTests/*` or `Backend/tests/TarotNow.ArchitectureTests/*`
- **Implementation direction:**
  - Keep reflection if needed, but build/validate constructor factory at startup or with architecture test.
  - Validate every domain event notification has expected constructor signature.
  - Cache compiled factory per event type to avoid repeated reflection on hot path.
- **Tests to add/run:**
  - Dispatcher creates notification for representative domain event.
  - Missing constructor shape fails in architecture/unit test, not first production request.
  - `dotnet test Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj`
  - `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj`
- **Risk/rollback:**
  - Startup validation may reveal existing edge types; use test-first allowlist only if needed.
- **Done criteria:**
  - Constructor convention is validated before runtime dispatch path.

## Phase 4 — API Contract Normalization

### T4.1 ProblemDetails contract normalization

- **Covers:** M-01
- **Files to touch:**
  - `Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler*.cs`
  - `Backend/src/TarotNow.Api/Controllers/TarotController.cs`
  - `Backend/src/TarotNow.Api/Controllers/GamificationController.cs`
  - `Backend/src/TarotNow.Api/Extensions/ControllerProblemDetailsExtensions.cs`
  - API integration tests
- **Implementation direction:**
  - Define canonical ProblemDetails extension with `errorCode`, `correlationId`, optional `detail`.
  - Replace anonymous error payloads in `TarotController` with canonical ProblemDetails.
  - Replace business code in `Problem().detail` with `errorCode` extension field.
  - Check frontend consumers before changing public response body shape.
- **Tests to add/run:**
  - Endpoint tests assert content type/status/errorCode shape.
  - `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj`
- **Risk/rollback:**
  - Response shape change may break frontend. If needed, add versioned endpoint or compatibility field for one release.
- **Done criteria:**
  - Public non-2xx API responses follow one ProblemDetails shape.

### T4.2 API route style guard

- **Covers:** M-07
- **Files to touch:**
  - `Backend/src/TarotNow.Api/Constants/ApiRoutes.cs`
  - `Backend/tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs`
  - API docs if present
- **Implementation direction:**
  - Do not rename existing v1 routes immediately.
  - Add route style documentation in test names/comments: plural noun resources for new collection endpoints unless explicitly allowlisted.
  - Add allowlist for current v1 legacy routes: `/api/v1/reading`, `/api/v1/withdrawal`.
  - Plan v2 route migration separately if breaking changes are acceptable.
- **Tests to add/run:**
  - Architecture route style test.
  - `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj`
- **Risk/rollback:**
  - Route rename is breaking; guard only new routes first.
- **Done criteria:**
  - New route drift is blocked; existing drift documented.

## Phase 5 — Maintainability Cleanup

### T5.1 Upload directory bootstrap cleanup

- **Covers:** L-01
- **Files to touch:**
  - `Backend/src/TarotNow.Api/Startup/ApiApplicationBuilderExtensions.cs`
  - New or existing hosted startup/readiness service for storage bootstrap
  - API integration tests if startup behavior covered
- **Implementation direction:**
  - Move `Directory.CreateDirectory(uploadsPath)` out of pipeline composition into a bootstrap/readiness component.
  - Log path/permission failure with structured logger.
  - Keep static files middleware behavior unchanged after successful bootstrap.
- **Tests to add/run:**
  - Unit/integration test for missing upload directory bootstrap.
  - `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj`
- **Risk/rollback:**
  - Startup behavior can change in containers; test read-only FS behavior in staging.
- **Done criteria:**
  - Pipeline extension no longer performs direct filesystem mutation.

### T5.2 Structured bootstrap logging

- **Covers:** L-02
- **Files to touch:**
  - `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.cs`
  - `Backend/src/TarotNow.Infrastructure/DependencyInjection.Cache.Bootstrap.cs`
  - Bootstrap logging helper if needed
- **Implementation direction:**
  - Replace direct `Console.Error.WriteLine` with structured bootstrap logger or deferred `ILogger` path.
  - Preserve visibility before DI logger is fully ready by using a minimal logger factory if necessary.
- **Tests to add/run:**
  - Unit tests if logging helper introduced.
  - `dotnet test Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj`
- **Risk/rollback:**
  - Early startup logs can disappear if logger initialized too late; verify logs in local startup.
- **Done criteria:**
  - Cache bootstrap failures are structured and searchable.

### T5.3 Controller partial drift guardrails

- **Covers:** L-03
- **Files to touch:**
  - `Backend/tests/TarotNow.ArchitectureTests/CodeQualityRulesTests.cs`
  - `Backend/src/TarotNow.Api/Controllers/*` only when cleanup is tied to a feature change
- **Implementation direction:**
  - Avoid mass controller restructure.
  - Add guardrails for cross-cutting consistency: authorization attributes, canonical user extraction, ProblemDetails response shape, idempotency headers where required.
  - Track controller partial families and require a single base helper for repeated cross-cutting logic.
- **Tests to add/run:**
  - Architecture tests for controller standards.
  - `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj`
- **Risk/rollback:**
  - Size/partial cleanup without behavior tests is risky; keep this as gradual cleanup.
- **Done criteria:**
  - Controller drift is blocked by tests; no large rewrite required.

### T5.4 Common/Helpers cleanup policy

- **Covers:** L-04
- **Files to touch:**
  - `Backend/src/TarotNow.Application/Common/**`
  - `Backend/src/TarotNow.Application/Helpers/**`
  - `Backend/tests/TarotNow.ArchitectureTests/CodeQualityRulesTests.cs`
- **Implementation direction:**
  - Create policy: helper can stay shared only if used by multiple bounded contexts and has no domain-specific naming.
  - Move single-context helpers next to that feature during related work.
  - Add architecture test or documentation guard for new `Helpers` folders/files.
- **Tests to add/run:**
  - Architecture test for new helper sprawl if feasible.
  - Feature tests for any moved helper.
- **Risk/rollback:**
  - Moving helpers can cause churn. Do not do bulk move without strong tests.
- **Done criteria:**
  - New helper sprawl is blocked; existing helpers have triage owners/categories.

### T5.5 Debt candidate triage

- **Covers:** Dead Code Report
- **Files to touch:**
  - `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.cs`
  - `Backend/src/TarotNow.Api/Controllers/ReportController.cs`
  - `Backend/src/TarotNow.Infrastructure/Services/Finance/PaymentGatewayService.cs`
  - `Backend/tests/TarotNow.Api.IntegrationTests/Security/AuthSessionSecurityTests.cs`
  - `Backend/tests/TarotNow.ArchitectureTests/EventDrivenArchitectureRulesTests.cs`
- **Implementation direction:**
  - For each debt marker, classify as still-needed, convert to issue reference, or remove by fixing underlying work.
  - Use IDE/Roslyn reference analysis before deleting any symbol.
  - Migration designer bulk is not dead code; do not delete unless migration squashing policy is explicitly approved.
- **Tests to add/run:**
  - Run targeted tests for touched area.
  - Run architecture tests if comments/guards changed.
- **Risk/rollback:**
  - Removing tests/comments can lose context. Keep issue references if work remains.
- **Done criteria:**
  - Debt markers are either resolved or tracked with owner/context.

## Verification Matrix

Run smallest relevant checks per phase:

```bash
dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj
```

```bash
dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj
```

```bash
dotnet test Backend/tests/TarotNow.Application.UnitTests/TarotNow.Application.UnitTests.csproj
```

```bash
dotnet test Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj
```

```bash
dotnet test Backend/tests/TarotNow.Infrastructure.IntegrationTests/TarotNow.Infrastructure.IntegrationTests.csproj
```

Suggested phase gates:

- Phase 1: Architecture tests + API integration tests.
- Phase 2: API integration + Application unit + Infrastructure unit tests.
- Phase 3: Infrastructure integration + API integration + Architecture tests.
- Phase 4: API integration + Architecture tests + frontend contract smoke if response shapes change.
- Phase 5: Architecture tests + targeted tests for touched features.

## Rollback And Release Strategy

- **Guardrails/tests:** safe to rollback by removing new tests, but prefer fixing violations instead.
- **Redis requirement:** deploy config first, then enforcement. Rollback by setting explicit local/test fallback only; do not silently fallback in production-like tiers.
- **AI settlement:** ship behind config if product policy is not final. Keep telemetry to compare old vs new decisions before enforcing.
- **Realtime dedup:** deploy distributed dedup with monitoring on Redis latency and duplicate suppression count. Rollback to local dedup only if Redis path causes outage.
- **Outbox event contract:** deploy compatibility reader before writer if possible. Never remove legacy CLR FullName resolver until old rows are drained.
- **API response normalization:** coordinate with frontend. Use compatibility fields or versioned rollout if clients rely on old anonymous payloads.
- **Maintainability cleanup:** no bulk refactor without tests. Prefer opportunistic cleanup near touched code.
