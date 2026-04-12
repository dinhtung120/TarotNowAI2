#!/usr/bin/env bash
set -euo pipefail

# Hỗ trợ cả hai cách truyền tham số:
# 1) rollback-drill.sh BASE_URL ENV_FILE COMPOSE_FILE [BASE_TAG] [NEXT_TAG]
# 2) rollback-drill.sh ENV_FILE COMPOSE_FILE BASE_URL [BASE_TAG] [NEXT_TAG]
if [[ "${1:-}" =~ ^https?:// ]]; then
  BASE_URL="${1:-http://localhost}"
  ENV_FILE="${2:-.env}"
  COMPOSE_FILE="${3:-docker-compose.prod.yml}"
  BASE_TAG="${4:-drill-n}"
  NEXT_TAG="${5:-drill-n1}"
else
  ENV_FILE="${1:-.env}"
  COMPOSE_FILE="${2:-docker-compose.prod.yml}"
  BASE_URL="${3:-http://localhost}"
  BASE_TAG="${4:-drill-n}"
  NEXT_TAG="${5:-drill-n1}"
fi

if [[ ! -f "$ENV_FILE" ]]; then
  echo "[rollback] env file not found: $ENV_FILE" >&2
  exit 1
fi

wait_ready() {
  local max_tries="${1:-40}"
  local sleep_secs="${2:-3}"
  local i
  for i in $(seq 1 "$max_tries"); do
    local code
    code="$(curl -s -o /dev/null -w "%{http_code}" "$BASE_URL/api/v1/health/ready" || true)"
    if [[ "$code" == "200" ]]; then
      return 0
    fi
    sleep "$sleep_secs"
  done
  return 1
}

deploy_tag() {
  local tag="$1"
  echo "[rollback] deploying tag: $tag"
  BACKEND_IMAGE="tarotnow/backend:$tag" FRONTEND_IMAGE="tarotnow/frontend:$tag" \
    docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up -d backend frontend reverse-proxy postgres mongodb redis

  if ! wait_ready 50 2; then
    echo "[rollback] readiness failed for tag $tag" >&2
    exit 1
  fi

  ./deploy/scripts/smoke.sh "$BASE_URL"
}

echo "[rollback] building baseline images"
docker build -f Backend/Dockerfile -t "tarotnow/backend:$BASE_TAG" .
docker build \
  --build-arg NEXT_PUBLIC_API_URL="$BASE_URL/api/v1" \
  --build-arg NEXT_PUBLIC_BASE_URL="$BASE_URL" \
  --build-arg NEXTAUTH_URL="$BASE_URL" \
  -f Frontend/Dockerfile -t "tarotnow/frontend:$BASE_TAG" .

echo "[rollback] creating simulated N+1 images"
docker tag "tarotnow/backend:$BASE_TAG" "tarotnow/backend:$NEXT_TAG"
docker tag "tarotnow/frontend:$BASE_TAG" "tarotnow/frontend:$NEXT_TAG"

echo "[rollback] bootstrapping database with baseline image"
BACKEND_IMAGE="tarotnow/backend:$BASE_TAG" FRONTEND_IMAGE="tarotnow/frontend:$BASE_TAG" \
  ./deploy/scripts/bootstrap-db.sh "$ENV_FILE" "$COMPOSE_FILE"

deploy_tag "$BASE_TAG"
deploy_tag "$NEXT_TAG"
deploy_tag "$BASE_TAG"

echo "[rollback] rollback drill passed"
