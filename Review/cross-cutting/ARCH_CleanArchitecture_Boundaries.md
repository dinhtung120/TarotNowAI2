# Clean Architecture Boundaries

## Evidence đã rà

- `Backend/src/TarotNow.Domain/TarotNow.Domain.csproj`
- `Backend/src/TarotNow.Application/TarotNow.Application.csproj`
- `Backend/src/TarotNow.Infrastructure/TarotNow.Infrastructure.csproj`
- `Backend/src/TarotNow.Api/TarotNow.Api.csproj`
- `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs`
- `Backend/tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs`
- `Backend/tests/TarotNow.ArchitectureTests/CodeQualityRulesTests.cs`

## Kết luận source-backed

Backend dùng mô hình Clean Architecture với project ownership rõ: Domain chứa model/event core; Application chứa CQRS, interfaces, validators/behaviors; Infrastructure implement persistence/cache/provider/outbox; API composition/controllers/middleware. Source-of-truth của rule là architecture tests, không phải tài liệu này.

## Boundary cần giữ

- Domain không reference Application/Infrastructure/API hoặc framework persistence/web.
- Application chỉ phụ thuộc Domain và Application-owned contracts.
- Infrastructure phụ thuộc Application/Domain để implement contracts.
- API phụ thuộc Application và Infrastructure composition, nhưng controller không nên gọi repository/db context/provider concrete trực tiếp.

## Guard hiện có

- `ArchitectureBoundariesTests.cs`: kiểm compile-time dependency, forbidden namespace/framework, API direct dependency risk.
- `ApiAndConfigurationStandardsTests.cs`: API version metadata, route standards, rate-limit/auth ordering, options pattern.
- `CodeQualityRulesTests.cs`: file/method/parameter/cyclomatic/cognitive budgets.

## Rủi ro

- P0: Application import Infrastructure concrete hoặc framework packages; Domain dính persistence/web framework; API bypass Application cho data mutation.
- P1: allowlist trong architecture tests tăng mà không có rationale.
- P2: docs hoặc file naming không phản ánh bounded context hiện tại.
