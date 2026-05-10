# Kết quả review backend tests

## Phạm vi

Đã review các test project backend trong `Backend/tests` bằng kiểm tra tĩnh và chạy chọn lọc `dotnet test`. Có đưa thêm `TarotNow.Infrastructure.UnitTests` vào báo cáo vì đây cũng là backend test project, dù plan ban đầu chỉ nêu năm project. `docs/backend-architecture-fix-plan.md` không tồn tại trong worktree này; phần đối chiếu dùng `docs/backend-architecture-review.md`.

## Tóm tắt chạy test

| Project | Lệnh | Kết quả | Ghi chú |
| --- | --- | --- | --- |
| `TarotNow.ArchitectureTests` | `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj --no-restore` | PASS | Exit code 0. Tool không in log test bình thường. |
| `TarotNow.Domain.UnitTests` | `dotnet test Backend/tests/TarotNow.Domain.UnitTests/TarotNow.Domain.UnitTests.csproj --no-restore` | PASS | Exit code 0. Tool không in log test bình thường. |
| `TarotNow.Application.UnitTests` | `dotnet test Backend/tests/TarotNow.Application.UnitTests/TarotNow.Application.UnitTests.csproj --no-restore` | PASS | Exit code 0. Tool không in log test bình thường. |
| `TarotNow.Api.IntegrationTests` | `dotnet test Backend/tests/TarotNow.Api.IntegrationTests/TarotNow.Api.IntegrationTests.csproj --no-restore` | PASS | Exit code 0. Tool không in log test bình thường. |
| `TarotNow.Infrastructure.IntegrationTests` | `dotnet test Backend/tests/TarotNow.Infrastructure.IntegrationTests/TarotNow.Infrastructure.IntegrationTests.csproj --no-restore` | PASS | Exit code 0. Tool không in log test bình thường. |
| `TarotNow.Infrastructure.UnitTests` | `dotnet test Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj --no-restore` | PASS | Exit code 0. Tool không in log test bình thường. |

## Findings

### High

- Rủi ro realtime dedup đã biết, nhưng test mới chỉ che routing, chưa che idempotency phân tán.
  - Bằng chứng: `docs/backend-architecture-review.md:32` đánh dấu local-memory bridge dedup là High; `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.cs:31` lưu `_bridgeDedupByEventId` bằng `ConcurrentDictionary` trong process; `Backend/src/TarotNow.Api/Realtime/RedisRealtimeSignalRBridgeService.cs:143` dùng `TryAdd` để chặn duplicate; test realtime hiện tại kiểm tra routing matrix ở `Backend/tests/TarotNow.Api.IntegrationTests/RedisRealtimeBridgeRoutingMatrixIntegrationTests.cs:22`.
  - Vì sao chưa hợp lý: test suite chứng minh message route tới SignalR groups, nhưng không chứng minh duplicate `eventId` vẫn đúng qua nhiều API instance hoặc sau restart, đúng rủi ro production đã nêu trong architecture review.
  - Gợi ý sửa: thêm integration/contract test tập trung vào distributed dedup abstraction hoặc Redis `SET NX EX` trước khi đổi implementation.

- Độ giòn của outbox event contract đang được test lặp lại thay vì được guard.
  - Bằng chứng: `docs/backend-architecture-review.md:41` đánh dấu việc lưu outbox bằng `GetType().FullName` là High; production publisher dùng ở `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs:38` và persist ở `Backend/src/TarotNow.Infrastructure/Services/MediatRDomainEventPublisher.cs:48`; test seed cùng kiểu CLR name giòn ở `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/ChatModerationOutboxIntegrationTests.cs:165`, `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/OutboxBatchProcessorIntegrationTests.cs:205`, và `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Reconciliation/ProjectionReconcileWorkerIntegrationTests.cs:31`.
  - Vì sao chưa hợp lý: test đang củng cố storage contract không ổn định thay vì fail khi namespace/class bị rename hoặc khi thiếu stable event name/version, nên refactor vẫn có thể làm hỏng outbox rows cũ trong khi test vẫn pass.
  - Gợi ý sửa: thêm test cho stable event contract names và backward compatibility resolver từ CLR names cũ trước khi migrate outbox serialization.

### Medium

- API integration tests tắt Redis/backplane, nên không bắt được regression consistency cache/realtime ở môi trường non-local.
  - Bằng chứng: `docs/backend-architecture-review.md:50` đánh dấu Redis fallback là High; `Backend/tests/TarotNow.Api.IntegrationTests/CustomWebApplicationFactory.cs:97` set `ConnectionStrings:Redis` rỗng, `Backend/tests/TarotNow.Api.IntegrationTests/CustomWebApplicationFactory.cs:164` đăng ký `AddDistributedMemoryCache()`, và `Backend/tests/TarotNow.Api.IntegrationTests/CustomWebApplicationFactory.cs:165` remove `IConnectionMultiplexer`.
  - Vì sao chưa hợp lý: in-memory fallback giúp CI ổn định, nhưng không có test bổ sung để assert production-like tiers fail closed hoặc require Redis, nên lỗi deployment-tier vẫn có thể pass integration suite.
  - Gợi ý sửa: thêm configuration tests cho non-local Redis-required mode; giữ in-memory factory hiện tại chỉ cho test không claim coverage consistency đa instance.

- Một số API/realtime integration tests thực chất mock trực tiếp hub boundary.
  - Bằng chứng: `Backend/tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs:19`, `Backend/tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs:23`, và `Backend/tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs:25` tạo `Mock<IMediator>`, `Mock<IUserPresenceTracker>`, và `Mock<IGroupManager>`; các test khác lặp lại pattern tại `Backend/tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs:47`, `Backend/tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs:51`, và `Backend/tests/TarotNow.Api.IntegrationTests/PresenceHubTests.cs:53`.
  - Vì sao chưa hợp lý: các test này kiểm tra branch logic trong hub và mock interactions, không kiểm tra SignalR connection thật, authentication, group membership, hoặc DI wiring mà API integration test nên bảo vệ.
  - Gợi ý sửa: chuyển pure hub unit tests sang unit-test project hoặc thêm ít nhất một test tích hợp bằng `WebApplicationFactory`/SignalR client cho connect, subscribe, disconnect, và group fanout.

- Gaps middleware/security trong architecture review chưa có behavior tests trực tiếp.
  - Bằng chứng: `docs/backend-architecture-review.md:135` nói thiếu test trực tiếp cho `GlobalExceptionHandler`, `CorrelationIdMiddleware`, `ChatFeatureGateMiddleware`, và forwarded-header trust cases; `Backend/src/TarotNow.Api/Middlewares/ChatFeatureGateMiddleware.cs:48` trả `ProblemDetails`, trong khi backend tests chỉ có architecture/config checks rộng thay vì behavior matrix.
  - Vì sao chưa hợp lý: architecture/order checks không chứng minh response shape, correlation propagation, 404 hiding behavior, hoặc trusted/untrusted proxy handling, đều là contract security/observability.
  - Gợi ý sửa: thêm API integration tests cho exception response shape, correlation ID sanitization/propagation, chat feature gate 404, và trusted versus untrusted forwarded headers.

- Findings về API identity/error drift chưa có regression tests targeted.
  - Bằng chứng: `docs/backend-architecture-review.md:72` đánh dấu error response contract không nhất quán; `docs/backend-architecture-review.md:81` đánh dấu direct user identity extraction; architecture tests hiện tại có API version/config-style checks như `Backend/tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs:51`, nhưng chưa có guard tương ứng cho việc dùng `User.TryGetUserId` hoặc uniform `ProblemDetails` contract.
  - Vì sao chưa hợp lý: đây là cross-cutting API contracts; nếu thiếu architecture hoặc integration tests, controller mới vẫn có thể thêm direct `ClaimTypes.NameIdentifier` parsing hoặc ad hoc error payload mà backend tests vẫn pass.
  - Gợi ý sửa: thêm architecture tests cấm direct claim parsing ngoài canonical extension và API tests/snapshots enforce normalized `ProblemDetails` error shape.

- Infrastructure integration test trộn Postgres outbox thật với in-memory repositories cho handler side effects.
  - Bằng chứng: `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/ChatModerationOutboxIntegrationTests.cs:19` đánh dấu test thuộc `InfrastructurePostgres`; `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/ChatModerationOutboxIntegrationTests.cs:42` và `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/ChatModerationOutboxIntegrationTests.cs:43` inject `InMemoryReportRepository` và `InMemoryChatMessageRepository`; implementations nằm trong cùng file tại `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/ChatModerationOutboxIntegrationTests.cs:182` và `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Outbox/ChatModerationOutboxIntegrationTests.cs:241`.
  - Vì sao chưa hợp lý: test chạy outbox status transitions với Postgres, nhưng không verify repository persistence, transaction behavior, hoặc EF/Mongo mapping cho side effects do handler kích hoạt.
  - Gợi ý sửa: đổi tên test để nói rõ đây là partial integration hoặc thêm full-boundary test dùng implementation persistence thật của report/chat.

### Low

- Nhiều query/unit tests dùng non-null assertion rộng làm kiểm tra giá trị chính.
  - Bằng chứng: `Backend/tests/TarotNow.Application.UnitTests/Admin/GetLedgerMismatchQueryHandlerTests.cs:48`, `Backend/tests/TarotNow.Application.UnitTests/Features/Community/Commands/CreatePostCommandHandlerTests.cs:58`, `Backend/tests/TarotNow.Application.UnitTests/Reading/InitReadingSessionCommandHandlerTests.cs:57`, và `Backend/tests/TarotNow.Infrastructure.UnitTests/Rng/RngServiceTests.cs:33` dùng `Should().NotBeNull()`.
  - Vì sao chưa hợp lý: non-null checks có thể ổn như setup guard, nhưng yếu nếu là assertion chính cho business DTO hoặc service output; regression về fields, ordering, hoặc invariants vẫn có thể pass.
  - Gợi ý sửa: tăng độ cụ thể bằng field/value assertions phản ánh behavior đang test.

- Inventory trong plan ban đầu đếm thiếu backend test project.
  - Bằng chứng: inventory thấy `Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj`; package scan cũng thấy dependencies của project này trong `Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj`.
  - Vì sao chưa hợp lý: nếu bỏ project này khỏi scope, audit sẽ thiếu infrastructure unit tests như RNG và repositories/helpers không được integration tests che hết.
  - Gợi ý sửa: cập nhật các backend test audit plans và verification matrices sau này để gồm `TarotNow.Infrastructure.UnitTests`.

## Coverage gaps theo project

- `TarotNow.ArchitectureTests`: Có guardrails tốt cho boundary/event/config, gồm cả rule chặn direct realtime bypass. Gaps còn lại: stable outbox event names, distributed realtime dedup, canonical API identity extraction, và consistent error contract enforcement.
- `TarotNow.Application.UnitTests`: Coverage rộng cho commands, handlers, và domain event handlers. Một số test mock-heavy và vài query tests dùng non-null assertions rộng; finance/AI settlement edge matrices từ `docs/backend-architecture-review.md` vẫn cần regression coverage mạnh hơn.
- `TarotNow.Domain.UnitTests`: Suite nhỏ và đang pass; domain invariants có vẻ ít hơn nhiều so với volume application tests.
- `TarotNow.Api.IntegrationTests`: Suite pass và có coverage hữu ích cho realtime routing/API, nhưng test factory remove Redis/backplane và một số hub tests là unit-style mocks trong integration project.
- `TarotNow.Infrastructure.IntegrationTests`: Suite pass, có chỗ dùng Postgres/Testcontainers thật, nhưng một số side effects được verify bằng in-memory repositories, và outbox tests vẫn phụ thuộc CLR `FullName` event types.
- `TarotNow.Infrastructure.UnitTests`: Có tồn tại và pass nhưng vắng khỏi plan ban đầu; nên đưa vào backend verification matrices.

## Non-findings

- Architecture tests có giá trị thật, không hời hợt: chúng enforce dependency direction Application/Domain, direct realtime bypass rules, wallet mutation event publication, và command-requested idempotency shape.
- Việc dùng in-memory fakes trong `CustomWebApplicationFactory` là hợp lý để giữ API tests deterministic; vấn đề nằm ở việc thiếu test bổ sung cho Redis-required production-like mode, không phải bản thân fake factory.
- Các `Assert.Empty(...)` đã kiểm tra trong outbox/MFA tests là negative assertions hợp lệ, không phải placeholder asserts yếu.
