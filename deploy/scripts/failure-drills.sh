#!/usr/bin/env bash
set -euo pipefail

BASE_URL="${1:-http://localhost}"
ENV_FILE="${2:-.env}"
COMPOSE_FILE="${3:-docker-compose.prod.yml}"

compose_args=(-f "$COMPOSE_FILE")
if [[ -f "$ENV_FILE" ]]; then
  compose_args=(--env-file "$ENV_FILE" -f "$COMPOSE_FILE")
fi

dc() {
  docker compose "${compose_args[@]}" "$@"
}

wait_ready() {
  local max_tries="${1:-30}"
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

wait_http_ok() {
  local url="$1"
  local max_tries="${2:-30}"
  local sleep_secs="${3:-2}"
  local i
  for i in $(seq 1 "$max_tries"); do
    if curl -fsS "$url" >/dev/null; then
      return 0
    fi
    sleep "$sleep_secs"
  done
  return 1
}

expect_ready_failure() {
  local max_tries="${1:-20}"
  local sleep_secs="${2:-2}"
  local i
  for i in $(seq 1 "$max_tries"); do
    local code
    code="$(curl -s -o /dev/null -w "%{http_code}" "$BASE_URL/api/v1/health/ready" || true)"
    if [[ "$code" != "200" ]]; then
      return 0
    fi
    sleep "$sleep_secs"
  done
  return 1
}

if [[ -f "$ENV_FILE" ]]; then
  echo "[drill] checking fail-fast when missing required env"
  tmp_env="$(mktemp)"
  grep -v '^JWT_SECRETKEY=' "$ENV_FILE" > "$tmp_env" || true
  set +e
  docker compose --env-file "$tmp_env" -f "$COMPOSE_FILE" config >/dev/null 2>&1
  rc=$?
  set -e
  rm -f "$tmp_env"
  if [[ "$rc" -eq 0 ]]; then
    echo "[drill] expected compose config to fail when JWT_SECRETKEY is missing" >&2
    exit 1
  fi
  echo "[drill] missing env fail-fast: OK"
else
  echo "[drill] skip missing-env check (env file not found: $ENV_FILE)"
fi

echo "[drill] redis down -> readiness should degrade"
dc stop redis
if ! expect_ready_failure 25 2; then
  echo "[drill] readiness did not degrade after redis stop" >&2
  exit 1
fi

echo "[drill] redis up -> readiness should recover"
dc start redis
if ! wait_ready 40 2; then
  echo "[drill] readiness did not recover after redis start" >&2
  exit 1
fi

echo "[drill] restart databases -> readiness should recover"
dc restart postgres mongodb
if ! wait_ready 120 2; then
  echo "[drill] readiness did not recover after DB restart" >&2
  exit 1
fi

echo "[drill] restart reverse-proxy"
dc restart reverse-proxy
if ! wait_http_ok "$BASE_URL/nginx-health" 40 2; then
  echo "[drill] reverse-proxy health endpoint did not recover" >&2
  exit 1
fi

echo "[drill] failure drills passed"
