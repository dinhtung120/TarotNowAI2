# Clean Architecture Boundaries

## 1. Source đã đọc thủ công

- `Backend/tests/TarotNow.ArchitectureTests/ArchitectureBoundariesTests.cs`
- `Backend/tests/TarotNow.ArchitectureTests/ApiAndConfigurationStandardsTests.cs`
- `Backend/tests/TarotNow.ArchitectureTests/CodeQualityRulesTests.cs`
- Project references được test đọc trực tiếp từ:
  - `Backend/src/TarotNow.Domain/TarotNow.Domain.csproj`
  - `Backend/src/TarotNow.Application/TarotNow.Application.csproj`
  - `Backend/src/TarotNow.Infrastructure/TarotNow.Infrastructure.csproj`
  - `Backend/src/TarotNow.Api/TarotNow.Api.csproj`

## 2. Kết luận kiến trúc hiện hành

`ArchitectureBoundariesTests.ProjectReferences_ShouldFollowCleanArchitectureDirection` là nguồn sự thật cho dependency graph backend. Rule đang enforce:

```text
Domain: không reference project nào
Application -> Domain
Infrastructure -> Domain + Application
Api -> Application + Infrastructure
```

Đây là Clean Architecture theo hướng dependency inward. API được phép reference Infrastructure để composition/runtime wiring, nhưng không được bypass Application bằng repository/db context trực tiếp trong controllers/hubs.

## 3. Boundary rules được enforce bằng test

### Domain

`ArchitectureBoundariesTests.DomainLayer_ShouldNotIntroduceForbiddenDependencies` cấm Domain import:

- `TarotNow.Application`
- `TarotNow.Infrastructure`
- `Microsoft.EntityFrameworkCore`
- `MongoDB.*`
- `Npgsql`
- `StackExchange.Redis`
- `Microsoft.AspNetCore`

`Domain_ShouldNotIntroduceNewPersistenceAttributesOutsideCurrentAllowlist` cấm annotation persistence `[Table]`, `[Column]`, `[Key]` trong Domain. Allowlist hiện tại rỗng, nghĩa là mapping phải nằm ở Infrastructure.

### Application

`ApplicationLayer_ShouldNotIntroduceForbiddenDependencies` cấm Application import:

- `TarotNow.Infrastructure`
- `Microsoft.EntityFrameworkCore`
- `MongoDB.*`
- `Npgsql`
- `StackExchange.Redis`
- `Microsoft.AspNetCore`

`ApplicationLayer_ShouldNotUseDomainExceptionType` và `DomainLayer_ShouldNotContainLegacyDomainExceptionType` enforce bỏ mô hình `DomainException` cũ, chuẩn hóa lỗi business về Application-side model.

### API

`Api_ShouldNotIntroduceNewDirectDbOrRepositoryDependenciesOutsideAllowlist` quét controllers/hubs để cấm field repository trực tiếp, `ApplicationDbContext`, `MongoDbContext`. Allowlist hiện tại rỗng.

`Api_ShouldNotIntroduceNewConcreteOpenAiProviderUsageOutsideAllowlist` cấm API dùng concrete `OpenAiProvider` trực tiếp.

`ApiLayer_ShouldNotIntroduceDomainNamespaceDependencies` cấm API import namespace `TarotNow.Domain`.

## 4. API/config/code-quality guard liên quan

`ApiAndConfigurationStandardsTests.cs` bổ sung các rule presentation/config:

- `ApiControllers_ShouldDeclareApiVersionMetadata`: mọi `[ApiController]` cần `ApiVersion` hoặc `ApiVersionNeutral`.
- `ApiLayer_ShouldNotHardcodeV1RouteLiteralsInAttributes`: không hardcode `/api/v1` trong route attributes.
- `InfrastructureServices_ShouldUseOptionsPatternInsteadOfIConfiguration`: Infrastructure services không dùng `IConfiguration` trực tiếp ngoài allowlist DI/composition files.
- `ApiHttpActions_ShouldHaveXmlSummaryComments`: mọi action có `Http*` attribute cần XML summary.
- `ApiPipeline_ShouldAuthenticateBeforeRateLimiting`: `UseAuthentication()` phải đứng trước `UseRateLimiter()` trong `Backend/src/TarotNow.Api/Startup/ApiApplicationBuilderExtensions.cs`.
- `AuthorizedHttpActions_ShouldDeclareRateLimitingMetadata`: authorized HTTP actions cần rate limiting metadata ở method hoặc class.

`CodeQualityRulesTests.cs` enforce budget hiện tại:

- file source <= 180 logical lines,
- method/local function <= 50 logical lines,
- required parameters <= 5,
- cyclomatic complexity <= 15,
- cognitive complexity <= 15.

## 5. Rủi ro kiến trúc cần ghi P0/P1/P2

- P0: Domain/Application/API vi phạm forbidden dependency; API controller/hub inject repository/db context trực tiếp; API dùng `OpenAiProvider` concrete; Application dùng EF/Mongo/Redis package.
- P1: thêm allowlist trong architecture tests mà không có rationale cụ thể; Infrastructure service mới đọc `IConfiguration` trực tiếp thay vì options pattern.
- P2: file/method gần vượt code-quality budget hoặc tài liệu feature không trỏ đúng guard đang enforce.

## 6. Cách verify khi review PR

- Chạy `dotnet test Backend/tests/TarotNow.ArchitectureTests/TarotNow.ArchitectureTests.csproj` nếu thay đổi backend boundary hoặc API/config.
- Đối chiếu diff với các test method nêu trên, không dựa vào tài liệu này như nguồn sự thật cuối cùng.
