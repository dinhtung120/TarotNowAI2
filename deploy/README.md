# Production Deployment (EC2 + Docker Compose)

## 1) Prepare environment file

```bash
cp .env.example .env
```

Update all required secrets in `.env`.

**Lưu ý:** File env production không nằm trong git. Mỗi máy deploy (DB / BE / FE) cần cùng một file trong thư mục repo, mặc định **`${PROD_REPO_DIR:-/opt/tarotnow/TarotNowAI2}/.env`**. Nếu trên EC2 bạn đặt tên **`.env.prod`**, thêm **GitHub Variable** `PROD_ENV_FILE` = `.env.prod` (repository → Settings → Variables and secrets → Actions → Variables): workflow sẽ `scp`/kiểm tra và gọi `docker compose --env-file` với đúng tên đó.

### GitHub Actions — một lần cấu hình, mọi máy EC2 nhận file `.env` (không cần SSH từng máy)

Dùng **Repository secrets** (mục **Secrets**, không dùng **Variables** công khai — Variables ai có quyền xem repo cũng đọc được).

**Cách A — Dán nguyên file local (đơn giản nhất)**  
1. **Settings → Secrets and variables → Actions → New repository secret**  
2. Tên: **`PROD_DOTENV_PLAIN`**  
3. Mở file `.env` trên máy bạn, **Select all → Copy**, dán vào ô **Value** (nhiều dòng được).  
4. Lưu. Mỗi lần deploy, runner ghi nội dung này ra file tạm rồi **`scp`** lên DB / BE / FE vào đúng tên file (`PROD_ENV_FILE`, mặc định `.env` hoặc `.env.prod`).

Giới hạn GitHub secret khoảng **48KB**; file env lớn hơn → dùng cách B.

**Cách B — Base64 (file lớn / tránh lỗi ký tự đặc biệt)**  
1. Secret tên **`PROD_DOTENV_B64`**, value = một dòng base64 của cùng file `.env`:  
   - **Linux:** `base64 -w0 < .env`  
   - **macOS:** `base64 -i .env | tr -d '[:space:]'`  

Nếu **cả hai** secret đều có giá trị, workflow **ưu tiên `PROD_DOTENV_B64`**. Chỉ cần **một** trong hai (hoặc đã có file env sẵn trên server).

**Lỗi base64** thường do dán nhầm nội dung thô vào `PROD_DOTENV_B64` — khi đó dùng **`PROD_DOTENV_PLAIN`** hoặc tạo lại chuỗi base64 đúng.

Workflow gọi `deploy/scripts/ci/ensure-remote-root-env.sh`: tạo file trên runner rồi **`scp`** lên host trước bước deploy.

Nếu **không** dùng secret: tạo file env thủ công trên **từng** host. Bước ensure chỉ pass khi file tồn tại hoặc có một trong hai secret ở trên.

## 2) Start data services and bootstrap DB (first deployment only)

```bash
./deploy/scripts/bootstrap-db.sh .env docker-compose.prod.yml
```

Script này sẽ:
- chạy Postgres/Mongo/Redis nền,
- apply EF Core migrations bằng service `backend-migrate` (`dotnet TarotNow.Api.dll --migrate`),
- seed PostgreSQL config và seed Mongo cards.

## 3) Build and start application stack

```bash
docker compose -f docker-compose.prod.yml up -d --build backend frontend reverse-proxy
```

## 4) Smoke test

```bash
./deploy/scripts/smoke.sh http://localhost
```

## 5) Failure/scale/rollback drills (pre go-live)

```bash
# Gate 5: failure scenarios
./deploy/scripts/failure-drills.sh http://localhost .env docker-compose.prod.yml

# Gate 6: scale scenarios (example 2 backend + 2 frontend replicas)
./deploy/scripts/scale-drill.sh http://localhost .env docker-compose.prod.yml 2 2

# Gate 7: rollback scenario (supports both arg orders, this is the recommended one)
./deploy/scripts/rollback-drill.sh .env docker-compose.prod.yml http://localhost
```

## 6) Backup and restore

```bash
ENV_FILE=.env ./deploy/scripts/backup-db.sh
ENV_FILE=.env ./deploy/scripts/restore-db.sh backups/<timestamp>
```

## 7) Rollback runbook

1. Keep the previous image tags available locally or in registry.
2. Update image tag in `docker-compose.prod.yml` (or your env-driven tag file) back to previous version.
3. Run:

```bash
docker compose -f docker-compose.prod.yml pull
docker compose -f docker-compose.prod.yml up -d
./deploy/scripts/smoke.sh http://localhost
```

If schema changes are backward-incompatible, restore from latest backup before switching traffic.

## Notes

- `FORWARDED_NETWORK_0` nên trùng với `PRIVATE_NETWORK_SUBNET` để backend trust đúng reverse-proxy trong Docker network.
- Nếu deploy sau ALB TLS termination, Nginx đã preserve `X-Forwarded-Proto` để backend không bị redirect-loop.
- Nếu local FE từng dùng `NODE_TLS_REJECT_UNAUTHORIZED=0`, cần gỡ trước khi build production.
