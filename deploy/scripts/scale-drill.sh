#!/usr/bin/env bash
set -euo pipefail

BASE_URL="${1:-http://localhost}"
ENV_FILE="${2:-.env}"
COMPOSE_FILE="${3:-docker-compose.prod.yml}"
BACKEND_REPLICAS="${4:-2}"
FRONTEND_REPLICAS="${5:-2}"

compose_args=(-f "$COMPOSE_FILE")
if [[ -f "$ENV_FILE" ]]; then
  compose_args=(--env-file "$ENV_FILE" -f "$COMPOSE_FILE")
fi

dc() {
  docker compose "${compose_args[@]}" "$@"
}

wait_ready() {
  local max_tries="${1:-40}"
  local sleep_secs="${2:-3}"
  local i
  for i in $(seq 1 "$max_tries"); do
    if curl -fsS "$BASE_URL/api/v1/health/ready" >/dev/null; then
      return 0
    fi
    sleep "$sleep_secs"
  done
  return 1
}

count_service_containers() {
  local service="$1"
  dc ps "$service" --status running | awk 'NR>1 && NF>0 {count++} END {print count+0}'
}

echo "[scale] scaling backend=$BACKEND_REPLICAS frontend=$FRONTEND_REPLICAS"
dc up -d --build --scale backend="$BACKEND_REPLICAS" --scale frontend="$FRONTEND_REPLICAS" backend frontend reverse-proxy postgres mongodb redis

if ! wait_ready 50 2; then
  echo "[scale] readiness did not become healthy after scaling" >&2
  exit 1
fi

backend_count="$(count_service_containers backend)"
frontend_count="$(count_service_containers frontend)"

if [[ "$backend_count" -lt "$BACKEND_REPLICAS" ]]; then
  echo "[scale] expected $BACKEND_REPLICAS backend containers, got $backend_count" >&2
  exit 1
fi

if [[ "$frontend_count" -lt "$FRONTEND_REPLICAS" ]]; then
  echo "[scale] expected $FRONTEND_REPLICAS frontend containers, got $frontend_count" >&2
  exit 1
fi

echo "[scale] validating shared upload storage across backend replicas"
first_backend_id="$(dc ps -q backend | head -n1)"
second_backend_id="$(dc ps -q backend | sed -n '2p')"

if [[ -n "$first_backend_id" && -n "$second_backend_id" ]]; then
  marker="/app/wwwroot/uploads/scale-drill-marker.txt"
  docker exec "$first_backend_id" sh -lc "echo scale-ok > $marker"
  docker exec "$second_backend_id" test -f "$marker"
fi

echo "[scale] checking Redis cache backend logs"
cache_log_found=0
for i in $(seq 1 10); do
  backend_logs="$(dc logs --no-color backend || true)"
  if [[ "$backend_logs" == *"Cache backend initialized with Redis."* ]]; then
    cache_log_found=1
    break
  fi
  sleep 2
done
if [[ "$cache_log_found" -ne 1 ]]; then
  echo "[scale] redis cache backend log marker not found" >&2
  exit 1
fi

curl -fsS "$BASE_URL/api/v1/health/live" >/dev/null
curl -fsS "$BASE_URL/api/v1/health/ready" >/dev/null
curl -fsS "$BASE_URL/" >/dev/null

echo "[scale] scale drill passed"
