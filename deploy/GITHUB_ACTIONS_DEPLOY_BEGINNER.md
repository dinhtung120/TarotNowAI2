# Hướng Dẫn Deploy Bằng GitHub Actions Cho Người Mới (3 EC2: DB/BE/FE)

## 1) Tài liệu này giúp bạn làm được gì

Bạn sẽ làm từ trạng thái gần như trống (chưa có pipeline production) đến trạng thái:
- Push code lên `main` thì GitHub Actions tự chạy build/test/scan/phân tích.
- Chỉ khi đạt chuẩn mới được deploy (Flow A có duyệt tay).
- Deploy theo đúng thứ tự `DB -> BE -> FE`.
- Có smoke test sau deploy.
- Nếu BE/FE lỗi sau deploy thì tự rollback về tag trước.
- Có thông báo Slack khi fail hoặc thành công.

Tài liệu này bám theo contract đang có trong repo:
- Workflow chính: `.github/workflows/cd-main-3ec2.yml`
- Script tổng hợp scan: `scripts/ci/summarize-findings.sh`
- Script deploy remote:
  - `deploy/scripts/remote/deploy_db.sh`
  - `deploy/scripts/remote/deploy_be.sh`
  - `deploy/scripts/remote/deploy_fe.sh`
  - `deploy/scripts/remote/rollback_be_fe.sh`

## 2) Deploy bằng GitHub Actions là gì (bản dễ hiểu)

Hiểu ngắn gọn:
- Bạn chỉ cần `git push`.
- GitHub sẽ tự chạy chuỗi kiểm tra.
- Đạt chuẩn thì pipeline mới đi tiếp đến deploy production.

Luồng thực tế của dự án này:

```text
push main
  -> build-test
  -> code-analysis (CodeQL + SonarCloud gate)
  -> security-scan (Trivy)
  -> findings-summary
  -> publish-images (GHCR)
  -> approval (Flow A) hoặc bỏ approval (Flow B)
  -> deploy-db
  -> deploy-be
  -> deploy-fe
  -> post-deploy-smoke
  -> auto-rollback (nếu deploy/smoke fail)
  -> notify-slack
```

Kiến trúc hạ tầng 3 EC2:

```text
User
  -> Cloudflare
    -> EC2-FE (frontend + reverse-proxy)
      -> EC2-BE (backend API)
        -> EC2-DB (postgres + mongodb + redis)
```

## 2.1 Bạn sẽ cần sửa những file nào

- Bắt buộc trên server (không commit): `.env` trên DB/BE/FE host.
- Bắt buộc trong GitHub UI: secrets/variables của repo.
- Tùy chọn khi muốn auto deploy 100%: sửa `.github/workflows/cd-main-3ec2.yml` để bỏ job `approval`.
- Không cần sửa API code để dùng pipeline này.

## 3) Bạn cần chuẩn bị gì trước khi bắt đầu

## 3.1 Tài khoản và quyền bắt buộc

- GitHub: quyền `Admin` trên repo.
- SonarCloud: có `Organization` + `Project`.
- Slack: tạo được Incoming Webhook.
- AWS: đã có 3 EC2 chạy ổn định (DB, BE, FE).

## 3.2 Ứng dụng cần có

Trên máy cá nhân (macOS/Linux):
- Terminal
- `git`
- `ssh`
- Khuyến nghị thêm `gh` (GitHub CLI) để chạy lệnh nhanh

Kiểm tra:

```bash
git --version
ssh -V
gh --version || echo "Chưa cài gh CLI (vẫn có thể làm bằng giao diện web)"
```

Trên **self-hosted runner** và cả 3 EC2:
- `docker`
- `docker compose` plugin
- `git`
- `curl`
- `jq`

## 3.3 Cài gói hệ thống trên Ubuntu (runner + 3 EC2)

Chạy trên từng máy Ubuntu:

```bash
sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg lsb-release git jq

sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo $VERSION_CODENAME) stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

sudo usermod -aG docker "$USER"
newgrp docker

docker --version
docker compose version
jq --version
```

## 3.4 Checklist nhanh trước khi cấu hình pipeline

```text
[ ] Repo có file .github/workflows/cd-main-3ec2.yml
[ ] Repo có script scripts/ci/summarize-findings.sh
[ ] Repo có script deploy/scripts/remote/*.sh
[ ] 3 EC2 đã SSH được
[ ] Runner host đi được mạng đến DB/BE/FE host
[ ] Trên cả runner + 3 host đã có docker compose + jq
[ ] Domain/Cloudflare đã trỏ đến FE host
```

## 4) Chuẩn bị self-hosted runner (label bắt buộc: aws-prod-runner)

Mục tiêu:
- Job build/scan chạy trên `ubuntu-latest` (GitHub-hosted).
- Job deploy chạy trên self-hosted runner với label `aws-prod-runner`.

## 4.1 Tạo runner từ GitHub UI

1. Vào `GitHub Repo -> Settings -> Actions -> Runners -> New self-hosted runner`.
2. Chọn Linux x64.
3. GitHub sẽ hiện chuỗi lệnh cài runner cho bạn.

## 4.2 Chạy lệnh cài runner trên máy runner

Ví dụ thư mục:

```bash
mkdir -p /opt/actions-runner
cd /opt/actions-runner
```

Sau đó copy đúng lệnh GitHub vừa hiển thị (vì token/phiên bản thay đổi theo thời điểm). Mẫu:

```bash
curl -o actions-runner-linux-x64-<version>.tar.gz -L https://github.com/actions/runner/releases/download/v<version>/actions-runner-linux-x64-<version>.tar.gz
tar xzf ./actions-runner-linux-x64-<version>.tar.gz
./config.sh --url https://github.com/<owner>/<repo> --token <token-tu-github>
```

Cài như service:

```bash
sudo ./svc.sh install
sudo ./svc.sh start
sudo ./svc.sh status
```

## 4.3 Gán label `aws-prod-runner`

Trong GitHub UI (runner detail), thêm label:

```text
aws-prod-runner
```

Xác nhận runner online:

```bash
# Chạy trên runner host
cd /opt/actions-runner
sudo ./svc.sh status
```

## 5) Chuẩn bị code và file môi trường trên 3 EC2

Trên từng host (DB, BE, FE), clone repo vào cùng một path để đỡ nhầm:

```bash
sudo mkdir -p /opt/tarotnow
sudo chown -R "$USER":"$USER" /opt/tarotnow
cd /opt/tarotnow
git clone git@github.com:dinhtung120/TarotNowAI2.git TarotNowAI2
cd TarotNowAI2
cp .env.example .env
```

Lưu ý rất quan trọng:
- Workflow deploy sẽ chạy `git fetch`, `git checkout <sha>`, `git reset --hard <sha>` trên host.
- Không sửa code thủ công trên server production.
- Chỉ sửa code trong repo Git, rồi push.

## 6) Bảng GitHub Secrets/Variables đầy đủ (copy-paste setup)

Vào:
- `Repo -> Settings -> Secrets and variables -> Actions`

## 6.1 Secrets bắt buộc

| Tên | Dùng để làm gì | Ví dụ | Lấy ở đâu | Sai thì lỗi gì |
|---|---|---|---|---|
| `SONAR_TOKEN` | Token gọi SonarCloud scan/gate | `sqp_xxx...` | SonarCloud account token | Job `code-analysis` fail auth |
| `SONAR_ORG` | Tên Sonar organization | `my-org` | SonarCloud org key | Sonar scan fail `organization not found` |
| `SONAR_PROJECT_KEY` | Project key Sonar | `my-org_tarotnow` | SonarCloud project settings | Gate fail hoặc không map project |
| `SLACK_WEBHOOK_URL` | Gửi thông báo pipeline | `https://hooks.slack.com/services/...` | Slack Incoming Webhook | Không có alert success/fail |
| `PROD_SSH_USER` | User SSH vào 3 host | `ubuntu` | User Linux trên EC2 | SSH fail `Permission denied` |
| `PROD_DB_HOST` | Địa chỉ SSH host DB | `54.x.x.x` hoặc hostname | Public IP/hostname DB | `deploy-db` không SSH được |
| `PROD_BE_HOST` | Địa chỉ SSH host BE | `18.x.x.x` | Public IP/hostname BE | `deploy-be` fail |
| `PROD_FE_HOST` | Địa chỉ SSH host FE | `3.x.x.x` | Public IP/hostname FE | `deploy-fe` fail |
| `PROD_SSH_PRIVATE_KEY` | Private key SSH để CI vào host | nội dung key PEM | Key pair dùng cho EC2 | SSH fail ngay bước đầu |
| `PROD_KNOWN_HOSTS` | Host key chống MITM khi SSH | output `ssh-keyscan -H ...` | Tự sinh từ lệnh ssh-keyscan | `Host key verification failed` |
| `GHCR_USERNAME` | User pull image từ GHCR trên host | `github_username` | Tài khoản/pat owner | `docker login ghcr.io` fail |
| `GHCR_TOKEN_READ` | Token read package GHCR trên host | `ghp_xxx...` | GitHub PAT có `read:packages` | Host không pull được image |
| `PROD_DB_PRIVATE_IP` | Private IP host DB cho BE kết nối | `10.0.3.21` | EC2 DB private IP | BE `health/ready` fail DB |
| `PROD_BE_PRIVATE_IP` | Private IP host BE cho FE/nginx upstream | `10.0.2.18` | EC2 BE private IP | FE deploy pass nhưng smoke fail |

## 6.2 Variables

| Tên | Dùng để làm gì | Ví dụ | Bắt buộc |
|---|---|---|---|
| `PROD_REPO_DIR` | Path repo trên host deploy | `/opt/tarotnow/TarotNowAI2` | Không bắt buộc (mặc định giá trị này) |
| `PROD_PUBLIC_BASE_URL` | URL public để smoke ngoài internet | `https://app.example.com` | Không bắt buộc (nếu để trống sẽ bỏ smoke public) |

## 6.3 Lệnh hỗ trợ lấy giá trị Secrets nhanh

Lấy private IP trên DB/BE host:

```bash
hostname -I | awk '{print $1}'
```

Tạo known_hosts (chạy từ máy có thể SSH đến 3 host):

```bash
ssh-keyscan -H <PROD_DB_HOST> > /tmp/prod_known_hosts
ssh-keyscan -H <PROD_BE_HOST> >> /tmp/prod_known_hosts
ssh-keyscan -H <PROD_FE_HOST> >> /tmp/prod_known_hosts
cat /tmp/prod_known_hosts
```

Copy toàn bộ nội dung `cat` vào secret `PROD_KNOWN_HOSTS`.

## 7) Flow A (khuyến nghị): Push -> Scan/Gate -> Manual Approval -> Deploy

Flow này an toàn hơn cho production.

## 7.1 Bật package permissions cho GHCR

1. Vào repo `Settings -> Actions -> General`.
2. Mục `Workflow permissions` chọn:
   - `Read and write permissions`.
3. Tick:
   - `Allow GitHub Actions to create and approve pull requests` (nếu cần cho workflow khác).

## 7.2 Tạo environment `production` và required reviewers

1. Vào `Settings -> Environments -> New environment`.
2. Tạo tên: `production`.
3. Add `Required reviewers` (ít nhất 1 người duyệt).

Đây là cổng chặn của job `approval` trong `cd-main-3ec2.yml`.

## 7.3 Nạp secrets/variables

Nhập đúng bảng ở Mục 6.

## 7.4 Test pipeline chế độ dry-run (không deploy)

### Cách 1 (không cần CLI): GitHub UI

- Vào `Actions -> CD Main 3EC2 -> Run workflow`.
- Chọn branch `main`.
- Tick `dry_run = true`.
- Run.

### Cách 2 (dùng gh CLI)

```bash
gh workflow run cd-main-3ec2.yml -f dry_run=true
gh run list --workflow "CD Main 3EC2" --limit 5
gh run watch
```

Kết quả mong đợi:
- Build/test/scan/summary chạy.
- Không có deploy DB/BE/FE.

## 7.5 Push commit thật lên main để chạy full flow

```bash
git checkout main
git pull --rebase origin main
git add .
git commit -m "ci: trigger production pipeline" || echo "Không có thay đổi để commit"
git push origin main
```

Nếu chỉ muốn test pipeline mà không đổi code:

```bash
git commit --allow-empty -m "ci: empty commit for pipeline test"
git push origin main
```

## 7.6 Theo dõi từng job và cách đọc kết quả

Job bắt buộc pass trước deploy:
- `build-test`
- `code-analysis`
- `security-scan`
- `findings-summary`

Kiểm tra artifact report:

```bash
# Tìm run id gần nhất
gh run list --workflow "CD Main 3EC2" --limit 1

# Tải artifact findings
gh run download <RUN_ID> -n security-quality-report -D /tmp/security-report
cat /tmp/security-report/security-quality-report.md
```

Nếu không dùng CLI: mở run trong UI -> tab `Artifacts` -> tải `security-quality-report`.

## 7.7 Duyệt manual approval

Khi pipeline dừng ở `approval`:
- Mở run -> `Review deployments` -> chọn `production`.
- Bấm `Approve and deploy`.

## 7.8 Theo dõi deploy DB -> BE -> FE -> smoke

Sau khi duyệt, kiểm tra trạng thái container trên từng host.

### Trên DB host

```bash
ssh -i <KEY_PATH> <PROD_SSH_USER>@<PROD_DB_HOST>
cd /opt/tarotnow/TarotNowAI2
docker compose -f docker-compose.prod.yml ps postgres mongodb redis
docker compose -f docker-compose.prod.yml logs --tail=100 postgres mongodb redis
```

### Trên BE host

```bash
ssh -i <KEY_PATH> <PROD_SSH_USER>@<PROD_BE_HOST>
cd /opt/tarotnow/TarotNowAI2
docker compose -f docker-compose.prod.yml ps backend
curl -fsS http://localhost:5037/api/v1/health/live
curl -fsS http://localhost:5037/api/v1/health/ready
ls -lah /opt/tarotnow/release-state
```

### Trên FE host

```bash
ssh -i <KEY_PATH> <PROD_SSH_USER>@<PROD_FE_HOST>
cd /opt/tarotnow/TarotNowAI2
docker compose -f docker-compose.prod.yml ps frontend reverse-proxy
curl -fsS http://localhost/nginx-health
ls -lah /opt/tarotnow/release-state
```

### Smoke từ public domain (nếu có `PROD_PUBLIC_BASE_URL`)

```bash
curl -fsS https://<DOMAIN>/api/v1/health/live
curl -fsS https://<DOMAIN>/api/v1/health/ready
curl -I https://<DOMAIN>/
```

## 7.9 Xác nhận rollback tự động hoạt động

Rollback tự động chạy ở job `auto-rollback` khi:
- `deploy-be` fail hoặc
- `deploy-fe` fail hoặc
- `post-deploy-smoke` fail

Rollback chỉ áp dụng cho `BE + FE`:
- Dùng state file tại `/opt/tarotnow/release-state`.
- Không rollback DB migration tự động.

## 8) Flow B: Auto deploy 100% khi push main (không approval)

Chỉ dùng khi team đã vận hành ổn định, monitor/alert tốt, rollback đã test thật.

## 8.1 Điều kiện bắt buộc trước khi bật auto 100%

```text
[ ] Đã test auto-rollback BE/FE thành công ít nhất 1 lần
[ ] Slack fail/success đã nhận ổn định
[ ] Người trực vận hành biết rollback thủ công
[ ] Đã có backup DB định kỳ
```

## 8.2 Cách sửa workflow để bỏ cổng approval

File cần sửa: `.github/workflows/cd-main-3ec2.yml`

Sửa 3 điểm:
1. Xóa job `approval`.
2. Trong job `deploy-db`, đổi:
   - `needs: [approval, publish-images]`
   - thành `needs: [publish-images]`
3. Trong job `notify-slack`, xóa `- approval` khỏi danh sách `needs`.

Sau đó commit/push:

```bash
git checkout main
git pull --rebase origin main
git add .github/workflows/cd-main-3ec2.yml
git commit -m "ci: remove manual approval gate for auto deploy"
git push origin main
```

## 8.3 Cách bật lại approval khi cần

Làm ngược lại Mục 8.2:
- Thêm lại job `approval` có `environment: production`.
- Thêm lại `approval` vào `needs` của `deploy-db` và `notify-slack`.

## 9) Cách đọc kết quả scan và quality (rất quan trọng)

Nguồn dữ liệu:
- `CodeQL`: job `code-analysis`
- `SonarCloud Quality Gate`: output `sonar_quality_gate`
- `Trivy`: `trivy-backend.json`, `trivy-frontend.json`, `trivy-fs.json`
- Tổng hợp: `security-quality-report.md` từ `scripts/ci/summarize-findings.sh`

## 9.1 Rule chặn deploy hiện tại

Trong `security-scan`, pipeline fail nếu tổng số finding `HIGH/CRITICAL` > 0 ở:
- backend image
- frontend image
- filesystem scan

Hệ quả:
- `publish-images` và deploy không chạy.
- `notify-slack` gửi cảnh báo fail.

## 9.2 Đọc report `security-quality-report.md`

Các phần chính:
- `Gate Status`
  - `CodeQL result`
  - `Sonar quality gate`
  - `Security scan job`
- `Trivy Summary`:
  - tổng số `CRITICAL`, `HIGH` cho backend/frontend/fs
- `Top ... Vulnerabilities`:
  - package lỗi, version hiện tại, version có bản vá, ID, title

## 9.3 Khi bị block vì scan

Thứ tự xử lý khuyến nghị:
1. Ưu tiên fix `CRITICAL`.
2. Nâng version package có `FixedVersion`.
3. Build/test lại local.
4. Push lại `main` để pipeline chạy lại.

## 10) Cấu hình Slack và kiểm tra thông báo

## 10.1 Tạo Slack webhook

1. Vào Slack App (Incoming Webhooks).
2. Chọn channel nhận cảnh báo.
3. Copy webhook URL và lưu vào secret `SLACK_WEBHOOK_URL`.

## 10.2 Nội dung thông báo gửi đi

Pipeline gửi thông tin:
- repo
- commit SHA
- actor
- trạng thái stage
- Sonar status
- CodeQL result
- Trivy high/critical counts
- link run GitHub Actions

## 10.3 Kiểm tra nhanh webhook bằng lệnh

```bash
curl -X POST -H 'Content-type: application/json' \
  --data '{"text":"[test] Slack webhook from deployment setup"}' \
  '<SLACK_WEBHOOK_URL>'
```

## 11) Troubleshooting chi tiết cho người mới

Mẫu đọc nhanh: `Dấu hiệu -> Nguyên nhân -> Cách xử lý -> Cách xác nhận`.

## 11.1 Sonar token/org/project sai

**Dấu hiệu**
- Job `code-analysis` fail ở bước Sonar scan/quality gate.
- Log có chữ `Unauthorized`, `organization not found`, `project not found`.

**Nguyên nhân**
- `SONAR_TOKEN`, `SONAR_ORG`, `SONAR_PROJECT_KEY` sai hoặc trống.

**Cách xử lý**
1. Vào SonarCloud, copy lại chính xác token/org/project key.
2. Cập nhật lại 3 secrets trong GitHub.
3. Push lại commit (hoặc empty commit) để chạy lại pipeline.

**Cách xác nhận**
- `code-analysis` pass.
- `findings-summary` hiển thị `Sonar quality gate: OK`.

## 11.2 Trivy fail vì HIGH/CRITICAL

**Dấu hiệu**
- Job `security-scan` fail ở `Enforce security gate`.
- Log báo `Security gate failed: HIGH/CRITICAL findings detected`.

**Nguyên nhân**
- Có lỗ hổng mức cao trong image hoặc filesystem scan.

**Cách xử lý**
1. Tải artifact `trivy-reports` hoặc `security-quality-report`.
2. Xác định package có bản vá (`FixedVersion`).
3. Nâng version package/dependency.
4. Build test lại.
5. Push lại để scan lại.

**Cách xác nhận**
- `security-scan` pass.
- Report có `HIGH=0`, `CRITICAL=0`.

## 11.3 SSH/known_hosts lỗi

**Dấu hiệu**
- Deploy job fail ngay khi SSH.
- Log có `Host key verification failed` hoặc `Permission denied (publickey)`.

**Nguyên nhân**
- `PROD_SSH_PRIVATE_KEY` sai.
- `PROD_KNOWN_HOSTS` thiếu key hoặc sai host.

**Cách xử lý**
1. Kiểm tra private key có đúng keypair dùng để SSH thủ công không.
2. Chạy lại `ssh-keyscan -H` cho DB/BE/FE host.
3. Cập nhật secret `PROD_KNOWN_HOSTS`.
4. Chạy lại pipeline.

**Cách xác nhận**
- Bước `Configure SSH` pass.
- Job deploy SSH được vào host.

## 11.4 Self-hosted runner không nhận job

**Dấu hiệu**
- Job `deploy-*` bị treo ở trạng thái chờ runner.
- GitHub hiển thị không có runner phù hợp.

**Nguyên nhân**
- Runner offline.
- Thiếu label `aws-prod-runner`.
- Runner không cùng repo/org scope.

**Cách xử lý**
1. Trên runner host: `sudo ./svc.sh status`.
2. Nếu stop: `sudo ./svc.sh start`.
3. Trong GitHub UI, kiểm tra runner có label `aws-prod-runner`.
4. Kiểm tra runner đã đăng ký đúng repo.

**Cách xác nhận**
- Job `deploy-db` bắt đầu chạy thay vì chờ vô hạn.

## 11.5 GHCR permission denied

**Dấu hiệu**
- Log có `denied: permission denied` khi `docker pull` hoặc `docker login ghcr.io`.

**Nguyên nhân**
- `GHCR_USERNAME` sai.
- `GHCR_TOKEN_READ` thiếu quyền `read:packages`.
- Package visibility không cho phép pull.

**Cách xử lý**
1. Tạo PAT mới có `read:packages` (và `repo` nếu repo private cần).
2. Cập nhật `GHCR_TOKEN_READ`, `GHCR_USERNAME`.
3. Kiểm tra package trong GHCR có thể pull bằng account này.

**Cách xác nhận**
- Trên host chạy được:
  ```bash
  echo "<GHCR_TOKEN_READ>" | docker login ghcr.io -u "<GHCR_USERNAME>" --password-stdin
  docker pull ghcr.io/<owner>/<repo>-backend:<tag>
  ```

## 11.6 Deploy pass nhưng smoke fail

**Dấu hiệu**
- `deploy-fe` pass nhưng `post-deploy-smoke` fail.
- Hoặc public URL lỗi 5xx.

**Nguyên nhân**
- `PROD_BE_PRIVATE_IP` sai.
- Nginx FE chưa trỏ đúng backend upstream.
- BE `ready` chưa đạt (DB connection lỗi).

**Cách xử lý**
1. Kiểm tra `PROD_BE_PRIVATE_IP` secret.
2. Trên FE host:
   ```bash
   cd /opt/tarotnow/TarotNowAI2
   grep -n "set \$backend_upstream" deploy/nginx/conf.d/default.conf
   curl -fsS http://localhost/nginx-health
   ```
3. Trên BE host:
   ```bash
   curl -fsS http://localhost:5037/api/v1/health/ready
   docker compose -f docker-compose.prod.yml logs --tail=200 backend
   ```

**Cách xác nhận**
- `post-deploy-smoke` pass.
- Public domain trả `200` ở `/`, `/api/v1/health/live`, `/api/v1/health/ready`.

## 11.7 Rollback fail do thiếu state file

**Dấu hiệu**
- Job `auto-rollback` fail.
- Log có `previous state file not found`.

**Nguyên nhân**
- Chưa có lần deploy thành công trước đó để tạo state.
- Thư mục `/opt/tarotnow/release-state` chưa có file `*_previous_image`.

**Cách xử lý**
1. Vào BE/FE host kiểm tra:
   ```bash
   ls -lah /opt/tarotnow/release-state
   ```
2. Đảm bảo đã có ít nhất 1 lần deploy thành công để sinh `current_image`.
3. Deploy thêm 1 phiên bản khác để sinh `previous_image`.
4. Test rollback lại.

**Cách xác nhận**
- Có đủ file:
  - `backend_current_image`, `backend_previous_image`
  - `frontend_current_image`, `frontend_previous_image`
- `auto-rollback` pass khi mô phỏng lỗi.

## 12) Runbook vận hành sau go-live

## 12.1 Deploy bản mới an toàn

```bash
git checkout main
git pull --rebase origin main
# cập nhật code
git add .
git commit -m "release: <mo-ta-ngan>"
git push origin main
```

Sau đó:
1. Theo dõi run `CD Main 3EC2`.
2. Đọc `security-quality-report`.
3. (Flow A) duyệt approval.
4. Kiểm tra health + smoke.

## 12.2 Rollback thủ công BE/FE

Trên BE host:

```bash
cd /opt/tarotnow/TarotNowAI2
REPO_DIR=/opt/tarotnow/TarotNowAI2 \
ENV_FILE=.env \
COMPOSE_FILE=docker-compose.prod.yml \
./deploy/scripts/remote/rollback_be_fe.sh backend
curl -fsS http://localhost:5037/api/v1/health/ready
```

Trên FE host:

```bash
cd /opt/tarotnow/TarotNowAI2
REPO_DIR=/opt/tarotnow/TarotNowAI2 \
ENV_FILE=.env \
COMPOSE_FILE=docker-compose.prod.yml \
./deploy/scripts/remote/rollback_be_fe.sh frontend
curl -fsS http://localhost/nginx-health
```

Lưu ý:
- DB migration không rollback tự động.
- Nếu migration lỗi, xử lý theo runbook DB riêng (backup/restore).

## 12.3 Tạm khóa auto deploy khi có incident

Cách nhanh nhất:
1. Vào GitHub `Actions -> CD Main 3EC2 -> ... -> Disable workflow`.
2. Hoặc revert commit đã bỏ approval (nếu đang Flow B).

Nếu cần dừng run đang chạy:

```bash
gh run list --workflow "CD Main 3EC2" --limit 5
gh run cancel <RUN_ID>
```

## 12.4 Checklist kiểm tra 24 giờ đầu sau go-live

Mỗi 2-4 giờ kiểm tra:

```bash
# FE
curl -fsS https://<DOMAIN>/api/v1/health/live
curl -fsS https://<DOMAIN>/api/v1/health/ready
curl -I https://<DOMAIN>/

# BE host
ssh -i <KEY_PATH> <USER>@<BE_HOST> "cd /opt/tarotnow/TarotNowAI2 && docker compose -f docker-compose.prod.yml ps backend"

# DB host
ssh -i <KEY_PATH> <USER>@<DB_HOST> "cd /opt/tarotnow/TarotNowAI2 && docker compose -f docker-compose.prod.yml ps postgres mongodb redis"
```

Quan sát:
- có restart loop container không
- độ trễ endpoint
- số lỗi trong log backend/nginx

## 13) Kiểm thử tài liệu này (đúng như kế hoạch chất lượng)

## 13.1 Doc walkthrough test

- Nhờ 1 người chưa setup trước đó làm theo từ đầu.
- Điều kiện pass: không cần hỏi thêm ngoài tài liệu.

## 13.2 Flow A test

1. Push commit sạch.
2. Xác nhận pipeline dừng đúng ở `approval`.
3. Duyệt approval.
4. Xác nhận deploy DB/BE/FE thành công.
5. Xác nhận Slack success gửi đúng.

## 13.3 Gate fail test

1. Tạo commit test có lỗi quality/security.
2. Xác nhận pipeline fail trước deploy.
3. Xác nhận Slack fail có link run + thông tin stage.

## 13.4 Rollback test

1. Mô phỏng fail ở FE hoặc smoke.
2. Xác nhận `auto-rollback` chạy.
3. Xác nhận service quay về image tag trước.

## 13.5 Doc quality test

Kiểm lại:
- 100% biến bắt buộc trong workflow đã được giải thích.
- Lệnh trong tài liệu chạy được theo thứ tự.
- Có cả 2 flow: `manual approval` và `auto deploy`.

## 14) Bảo mật tối thiểu bắt buộc

- Không commit `.env`.
- Chỉ dùng SSH key, không bật password login.
- Hạn chế port public (DB không mở internet).
- Rotate secrets định kỳ (`SONAR_TOKEN`, `GHCR_TOKEN_READ`, webhook, app secrets).
- Self-hosted runner nên đặt trong mạng riêng, chỉ mở cần thiết.
- Theo dõi log bất thường sau mỗi lần deploy.

---

Nếu bạn mới hoàn toàn, hãy chạy theo thứ tự: `Mục 3 -> 4 -> 5 -> 6 -> 7`.  
Flow B chỉ bật sau khi Flow A đã ổn định trong thực tế.
