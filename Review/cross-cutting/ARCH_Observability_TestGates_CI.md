# Observability, Test Gates, CI

## Evidence đã rà

- Backend architecture tests: `Backend/tests/TarotNow.ArchitectureTests/*.cs`.
- Frontend guards: `Frontend/scripts/check-*.mjs`, `Frontend/scripts/lib/cleanArchitectureGuard.test.ts`.
- Compose healthchecks: `docker-compose.prod.yml`.
- Smoke: `deploy/scripts/smoke.sh`.
- Failure/scale/rollback drills: `deploy/scripts/failure-drills.sh`, `deploy/scripts/scale-drill.sh`, `deploy/scripts/rollback-drill.sh`.
- Backup/restore: `deploy/scripts/backup-db.sh`, `deploy/scripts/restore-db.sh`.
- CI/CD: `.github/workflows/cd-main-3ec2.yml`, `cd-fast-deploy.yml`, `cd-fe-only-deploy.yml`.

## CI/CD map

- `cd-main-3ec2.yml`: full gate gồm build/test, analysis/security scan, publish, approval, deploy DB/BE/FE, smoke, rollback, Slack/artifact reporting.
- `cd-fast-deploy.yml`: fast deploy, có trade-off vì bỏ bớt analysis/security gate.
- `cd-fe-only-deploy.yml`: deploy frontend riêng, smoke và rollback frontend.

## Ops boundary

- `deploy/scripts/bootstrap-db.sh`: start DB + migration/bootstrap seed.
- `deploy/scripts/remote/rollback_be_fe.sh`: rollback service image BE/FE; không rollback DB migration tự động.
- `deploy/scripts/smoke.sh`: kiểm health/live/ready/root route.

## Rủi ro

- P0: thay đổi DB/schema không có migration/backup/restore/rollback plan.
- P1: thay đổi API/FE không cập nhật smoke hoặc health gate liên quan.
- P2: findings artifact chưa đủ context để triage.
