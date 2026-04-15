#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "usage: $0 <backend_image_ref>"
  exit 1
fi

BACKEND_IMAGE_REF="$1"
REPO_DIR="${REPO_DIR:-/opt/tarotnow/TarotNowAI2}"
ENV_FILE="${ENV_FILE:-.env}"
COMPOSE_FILE="${COMPOSE_FILE:-docker-compose.prod.yml}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=lib-env.sh
source "$SCRIPT_DIR/lib-env.sh"

cd "$REPO_DIR"

materialize_root_env_from_ci "$ENV_FILE" || exit 1

if [[ ! -f "$ENV_FILE" ]]; then
  echo "[deploy-db] env file not found: $REPO_DIR/$ENV_FILE" >&2
  echo "[deploy-db] Tạo thủ công: cp .env.example .env && chỉnh secrets, hoặc đặt GitHub secret PROD_DOTENV_B64 (base64 của .env)." >&2
  exit 1
fi

if [[ ! -f "$COMPOSE_FILE" ]]; then
  echo "[deploy-db] compose file not found: $COMPOSE_FILE" >&2
  exit 1
fi

echo "[deploy-db] pulling backend image: $BACKEND_IMAGE_REF"
docker pull "$BACKEND_IMAGE_REF"

echo "[deploy-db] running bootstrap with prebuilt image"
BACKEND_IMAGE="$BACKEND_IMAGE_REF" USE_PREBUILT_IMAGES=true \
  ./deploy/scripts/bootstrap-db.sh "$ENV_FILE" "$COMPOSE_FILE"

echo "[deploy-db] verifying DB containers"
docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" ps postgres mongodb redis

echo "[deploy-db] validating critical auth schema"
schema_ok="$(docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" exec -T postgres sh -lc "psql -U \"\$POSTGRES_USER\" -d \"\$POSTGRES_DB\" -tAc \"SELECT to_regclass('public.auth_sessions') IS NOT NULL\"")"
schema_ok="$(echo "$schema_ok" | tr -d '[:space:]')"
if [[ "$schema_ok" != "t" ]]; then
  echo "[deploy-db] schema validation failed: table public.auth_sessions is missing" >&2
  exit 1
fi

echo "[deploy-db] cleaning up old images"
docker system prune -a -f

echo "[deploy-db] done"
