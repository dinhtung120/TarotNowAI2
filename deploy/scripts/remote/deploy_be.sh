#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "usage: $0 <backend_image_ref>"
  exit 1
fi

if [[ -z "${DB_PRIVATE_IP:-}" ]]; then
  echo "[deploy-be] missing required env DB_PRIVATE_IP" >&2
  exit 1
fi

BACKEND_IMAGE_REF="$1"
REPO_DIR="${REPO_DIR:-/opt/tarotnow/TarotNowAI2}"
ENV_FILE="${ENV_FILE:-.env}"
COMPOSE_FILE="${COMPOSE_FILE:-docker-compose.prod.yml}"
OVERRIDE_FILE="${OVERRIDE_FILE:-deploy/.generated.backend.override.yml}"
STATE_DIR="${STATE_DIR:-/opt/tarotnow/release-state}"
CURRENT_FILE="$STATE_DIR/backend_current_image"
PREVIOUS_FILE="$STATE_DIR/backend_previous_image"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=lib-env.sh
source "$SCRIPT_DIR/lib-env.sh"

cd "$REPO_DIR"

materialize_root_env_from_ci "$ENV_FILE" || exit 1

if [[ ! -f "$ENV_FILE" ]]; then
  echo "[deploy-be] env file not found: $REPO_DIR/$ENV_FILE" >&2
  echo "[deploy-be] Tạo thủ công: cp .env.example .env && chỉnh secrets, hoặc đặt GitHub secret PROD_DOTENV_B64 (base64 của .env)." >&2
  exit 1
fi

if [[ ! -f "$COMPOSE_FILE" ]]; then
  echo "[deploy-be] compose file not found: $COMPOSE_FILE" >&2
  exit 1
fi

dc() {
  docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" -f "$OVERRIDE_FILE" "$@"
}

dump_backend_diagnostics() {
  echo "[deploy-be] backend diagnostics begin" >&2
  dc ps backend >&2 || true
  echo "[deploy-be] backend logs (last 200 lines):" >&2
  dc logs --no-color --tail=200 backend >&2 || true

  local container_id
  container_id="$(dc ps -q backend 2>/dev/null | head -n 1 || true)"
  if [[ -n "$container_id" ]]; then
    echo "[deploy-be] backend container inspect:" >&2
    docker inspect --format 'name={{.Name}} status={{.State.Status}} health={{if .State.Health}}{{.State.Health.Status}}{{else}}none{{end}} exit={{.State.ExitCode}} started={{.State.StartedAt}} finished={{.State.FinishedAt}} error={{.State.Error}}' "$container_id" >&2 || true
  fi

  echo "[deploy-be] probe /api/v1/health/live:" >&2
  curl -sS -i http://localhost:5037/api/v1/health/live >&2 || true
  echo "[deploy-be] probe /api/v1/health/ready:" >&2
  curl -sS -i http://localhost:5037/api/v1/health/ready >&2 || true
  echo "[deploy-be] backend diagnostics end" >&2
}

echo "[deploy-be] pulling backend image: $BACKEND_IMAGE_REF"
docker pull "$BACKEND_IMAGE_REF"

echo "[deploy-be] rendering backend override with DB_PRIVATE_IP=$DB_PRIVATE_IP"
cat > "$OVERRIDE_FILE" <<YAML
services:
  backend:
    ports:
      - "5037:5037"
    environment:
      CONNECTIONSTRINGS__POSTGRESQL: Host=${DB_PRIVATE_IP};Port=5432;Database=\${POSTGRES_DB};Username=\${POSTGRES_USER};Password=\${POSTGRES_PASSWORD}
      CONNECTIONSTRINGS__MONGODB: mongodb://${DB_PRIVATE_IP}:27017/\${MONGO_DB}
      CONNECTIONSTRINGS__REDIS: ${DB_PRIVATE_IP}:6379
YAML

export BACKEND_IMAGE="$BACKEND_IMAGE_REF"

echo "[deploy-be] running schema migration guard (--migrate) with backend image"
dc run --rm --no-deps backend --migrate

echo "[deploy-be] deploying backend container"
dc up -d --no-deps backend

if [[ -z "$(dc ps --status running -q backend)" ]]; then
  echo "[deploy-be] backend container is not running right after deploy" >&2
  dump_backend_diagnostics
  exit 1
fi

echo "[deploy-be] waiting for backend readiness"
for i in $(seq 1 60); do
  code="$(curl -s -o /dev/null -w "%{http_code}" http://localhost:5037/api/v1/health/ready || true)"
  if [[ "$code" == "200" ]]; then
    break
  fi
  sleep 2
  if [[ "$i" -eq 60 ]]; then
    echo "[deploy-be] backend readiness check failed" >&2
    dump_backend_diagnostics
    exit 1
  fi
done

mkdir -p "$STATE_DIR"
if [[ -f "$CURRENT_FILE" ]]; then
  previous_image="$(cat "$CURRENT_FILE")"
  if [[ -n "$previous_image" && "$previous_image" != "$BACKEND_IMAGE_REF" ]]; then
    echo "$previous_image" > "$PREVIOUS_FILE"
  fi
fi
echo "$BACKEND_IMAGE_REF" > "$CURRENT_FILE"


echo "[deploy-be] done"
