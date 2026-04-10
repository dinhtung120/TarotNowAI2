#!/usr/bin/env bash
set -euo pipefail

ENV_FILE="${1:-deploy/.env.prod}"
COMPOSE_FILE="${2:-docker-compose.prod.yml}"
USE_PREBUILT_IMAGES="${USE_PREBUILT_IMAGES:-false}"

if [ ! -f "$ENV_FILE" ]; then
  echo "[bootstrap] env file not found: $ENV_FILE" >&2
  exit 1
fi

dc() {
  docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" "$@"
}

echo "[bootstrap] starting data services (postgres/mongodb/redis)"
dc up -d --remove-orphans postgres mongodb redis

echo "[bootstrap] running one-time bootstrap services"
# Xóa container one-shot cũ để tránh stale network references giữa các lần chạy.
dc rm -fsv backend-migrate postgres-bootstrap mongo-bootstrap >/dev/null 2>&1 || true

if [[ "$USE_PREBUILT_IMAGES" == "true" ]]; then
  echo "[bootstrap] using prebuilt images (skip compose build)"
  dc --profile bootstrap up --force-recreate --exit-code-from backend-migrate backend-migrate
else
  dc --profile bootstrap up --build --force-recreate --exit-code-from backend-migrate backend-migrate
fi
dc --profile bootstrap up --force-recreate --exit-code-from postgres-bootstrap postgres-bootstrap
dc --profile bootstrap up --force-recreate --exit-code-from mongo-bootstrap mongo-bootstrap

echo "[bootstrap] completed"
