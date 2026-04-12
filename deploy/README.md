# Production Deployment (EC2 + Docker Compose)

## 1) Prepare environment file

```bash
cp .env.example .env
```

Update all required secrets in `.env`.

**Lưu ý:** `.env` không nằm trong git. Trên server production, tạo file một lần (`cp .env.example .env` rồi sửa), hoặc trong GitHub Actions đặt secret **`PROD_DOTENV_B64`** = chuỗi base64 của toàn bộ file `.env` (UTF-8), ví dụ: `base64 -w0 < .env` (Linux) / `base64 -i .env` (macOS). Workflow deploy sẽ ghi `REPO_ROOT/.env` trước khi chạy `deploy_db` / `deploy_be` / `deploy_fe`.

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
