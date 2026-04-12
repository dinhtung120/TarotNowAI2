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

echo "[deploy-be] deploying backend container"
docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" -f "$OVERRIDE_FILE" up -d --no-deps backend

echo "[deploy-be] waiting for backend readiness"
for i in $(seq 1 60); do
  code="$(curl -s -o /dev/null -w "%{http_code}" http://localhost:5037/api/v1/health/ready || true)"
  if [[ "$code" == "200" ]]; then
    break
  fi
  sleep 2
  if [[ "$i" -eq 60 ]]; then
    echo "[deploy-be] backend readiness check failed" >&2
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

echo "[deploy-be] cleaning up old images"
docker system prune -a -f --volumes

echo "[deploy-be] done"
