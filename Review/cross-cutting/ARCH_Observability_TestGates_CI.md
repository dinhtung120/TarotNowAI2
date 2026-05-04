# Observability, Test Gates, CI

## 1. Source đã đọc thủ công

- `.github/workflows/cd-main-3ec2.yml`
- `deploy/scripts/smoke.sh`
- `docker-compose.prod.yml`
- `Backend/tests/TarotNow.ArchitectureTests/*.cs`
- `Frontend/scripts/check-*.mjs`

Các workflow khác cần đọc tiếp ở batch ops chi tiết:

- `.github/workflows/cd-fast-deploy.yml`
- `.github/workflows/cd-fe-only-deploy.yml`
- `deploy/scripts/bootstrap-db.sh`
- `deploy/scripts/rollback-drill.sh`
- `deploy/scripts/backup-db.sh`
- `deploy/scripts/restore-db.sh`
- `deploy/scripts/remote/rollback_be_fe.sh`

## 2. CI gate chính trong `cd-main-3ec2.yml`

Workflow `CD Main 3EC2` chạy khi push `main` hoặc `workflow_dispatch`. Job `build-test` đang làm các bước đáng chú ý:

- Validate .NET SDK/Dockerfile alignment: `scripts/ci/validate-dotnet-container-version.sh`.
- Backend restore/build/test:
  - `dotnet restore src/TarotNow.Api/TarotNow.Api.csproj`
  - `dotnet build src/TarotNow.Api/TarotNow.Api.csproj --configuration Release --no-restore`
  - `dotnet test TarotNow.slnx --configuration Release --nologo`
- Frontend install/build:
  - `npm ci`
  - `npm run build`
- Frontend risk coverage gate:
  - `npm run test:coverage:risk`

Job `code-analysis` chạy CodeQL cho C# và JavaScript/TypeScript, đồng thời chạy Sonar scan/quality gate cho C# matrix entry.

Job `security-scan` build backend/frontend scan images rồi chạy Trivy JSON scan cho HIGH/CRITICAL. Đọc tiếp phần sau line 199 của workflow khi cần map đầy đủ publish/deploy/rollback/slack.

## 3. Smoke test runtime

`deploy/scripts/smoke.sh` là smoke tối thiểu sau deploy. Script nhận `BASE_URL` mặc định `http://localhost` và kiểm tra:

- `$BASE_URL/nginx-health`
- `$BASE_URL/api/v1/health/live`
- `$BASE_URL/api/v1/health/ready`
- `$BASE_URL/`

Nếu bất kỳ `curl -fsS` fail, script fail do `set -euo pipefail`.

## 4. Architecture/test gates liên quan

Backend architecture tests là gate bắt buộc khi thay đổi boundary/event/API/code quality:

- `ArchitectureBoundariesTests.cs`
- `EventDrivenArchitectureRulesTests.cs`
- `ApiAndConfigurationStandardsTests.cs`
- `CodeQualityRulesTests.cs`

Frontend guard scripts là gate bắt buộc khi thay đổi UI/routes/hooks/actions:

- `check-clean-architecture.mjs`: layer direction, domain purity, client/runtime boundary, sensitive EventSource URL, app page/layout import qua feature public API.
- `check-component-size.mjs`: TSX component size policy.
- `check-hook-action-size.mjs`: hook/action size policy.
- `check-auth-fail-closed.mjs`: auth fail-closed checks.
- `check-risk-coverage.mjs`: risk coverage gate dùng trong CI qua `npm run test:coverage:risk`.

## 5. Observability hiện có từ source đã đọc

Evidence trực tiếp đã đọc cho health/smoke là `deploy/scripts/smoke.sh`. Compose healthchecks cần đọc chi tiết trong `docker-compose.prod.yml` trước khi kết luận từng service có healthcheck gì. Không ghi giả định về metrics/logging dashboard nếu chưa thấy file config tương ứng.

## 6. Rủi ro

- P0: deploy thay đổi API/health path nhưng không cập nhật `deploy/scripts/smoke.sh`; DB migration thay đổi nhưng không có backup/restore/rollback evidence; bỏ qua `dotnet test` hoặc `npm run test:coverage:risk` trong main workflow.
- P1: `cd-fast-deploy.yml` được dùng cho thay đổi rủi ro cao dù bỏ bớt analysis/security gate; Trivy/Sonar/CodeQL findings không được triage.
- P2: docs nói có observability nhưng chỉ có smoke/health evidence, không có metrics/tracing source.

## 7. Verify khi review PR

- Nếu chỉ sửa docs: kiểm `git diff -- Review/...` và không cần chạy production tests.
- Nếu sửa backend boundary: chạy ArchitectureTests.
- Nếu sửa frontend route/feature: chạy guard scripts trong `Frontend/scripts` hoặc npm script tương ứng.
- Nếu sửa deploy/workflow: đọc `cd-main-3ec2.yml`, smoke script và rollback/backup scripts cùng lúc.
