#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "usage: $0 <backend|frontend>"
  exit 1
fi

SERVICE="$1"
REPO_DIR="${REPO_DIR:-/opt/tarotnow/TarotNowAI2}"
ENV_FILE="${ENV_FILE:-deploy/.env.prod}"
COMPOSE_FILE="${COMPOSE_FILE:-docker-compose.prod.yml}"
OVERRIDE_FILE="${OVERRIDE_FILE:-deploy/.generated.backend.override.yml}"
STATE_DIR="${STATE_DIR:-/opt/tarotnow/release-state}"

case "$SERVICE" in
  backend)
    CURRENT_FILE="$STATE_DIR/backend_current_image"
    PREVIOUS_FILE="$STATE_DIR/backend_previous_image"
    ;;
  frontend)
    CURRENT_FILE="$STATE_DIR/frontend_current_image"
    PREVIOUS_FILE="$STATE_DIR/frontend_previous_image"
    ;;
  *)
    echo "[rollback] invalid service: $SERVICE" >&2
    exit 1
    ;;
esac

cd "$REPO_DIR"

if [[ ! -f "$PREVIOUS_FILE" ]]; then
  echo "[rollback] previous state file not found: $PREVIOUS_FILE" >&2
  exit 1
fi

ROLLBACK_IMAGE="$(cat "$PREVIOUS_FILE")"
if [[ -z "$ROLLBACK_IMAGE" ]]; then
  echo "[rollback] previous image is empty for service $SERVICE" >&2
  exit 1
fi

if [[ "$SERVICE" == "backend" ]]; then
  export BACKEND_IMAGE="$ROLLBACK_IMAGE"
  docker pull "$BACKEND_IMAGE"
  if [[ ! -f "$OVERRIDE_FILE" ]]; then
    echo "[rollback] backend override file not found: $OVERRIDE_FILE" >&2
    exit 1
  fi
  docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" -f "$OVERRIDE_FILE" up -d --no-deps backend

  for i in $(seq 1 60); do
    code="$(curl -s -o /dev/null -w "%{http_code}" http://localhost:5037/api/v1/health/ready || true)"
    if [[ "$code" == "200" ]]; then
      break
    fi
    sleep 2
    if [[ "$i" -eq 60 ]]; then
      echo "[rollback] backend health check failed" >&2
      exit 1
    fi
  done
else
  export FRONTEND_IMAGE="$ROLLBACK_IMAGE"
  docker pull "$FRONTEND_IMAGE"
  docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up -d --no-deps frontend reverse-proxy

  for i in $(seq 1 50); do
    if curl -fsS http://localhost/nginx-health >/dev/null; then
      break
    fi
    sleep 2
    if [[ "$i" -eq 50 ]]; then
      echo "[rollback] frontend nginx check failed" >&2
      exit 1
    fi
  done
fi

current_image=""
if [[ -f "$CURRENT_FILE" ]]; then
  current_image="$(cat "$CURRENT_FILE")"
fi

echo "$ROLLBACK_IMAGE" > "$CURRENT_FILE"
if [[ -n "$current_image" && "$current_image" != "$ROLLBACK_IMAGE" ]]; then
  echo "$current_image" > "$PREVIOUS_FILE"
fi

echo "[rollback] service=$SERVICE rolled back to $ROLLBACK_IMAGE"
