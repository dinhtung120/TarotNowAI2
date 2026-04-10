#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "usage: $0 <frontend_image_ref>"
  exit 1
fi

if [[ -z "${BE_PRIVATE_IP:-}" ]]; then
  echo "[deploy-fe] missing required env BE_PRIVATE_IP" >&2
  exit 1
fi

FRONTEND_IMAGE_REF="$1"
REPO_DIR="${REPO_DIR:-/opt/tarotnow/TarotNowAI2}"
ENV_FILE="${ENV_FILE:-deploy/.env.prod}"
COMPOSE_FILE="${COMPOSE_FILE:-docker-compose.prod.yml}"
NGINX_DEFAULT_TEMPLATE="${NGINX_DEFAULT_TEMPLATE:-deploy/nginx/conf.d/default.conf.template}"
STATE_DIR="${STATE_DIR:-/opt/tarotnow/release-state}"
CURRENT_FILE="$STATE_DIR/frontend_current_image"
PREVIOUS_FILE="$STATE_DIR/frontend_previous_image"

cd "$REPO_DIR"

if [[ ! -f "$ENV_FILE" ]]; then
  echo "[deploy-fe] env file not found: $ENV_FILE" >&2
  exit 1
fi

if [[ ! -f "$COMPOSE_FILE" ]]; then
  echo "[deploy-fe] compose file not found: $COMPOSE_FILE" >&2
  exit 1
fi

if [[ ! -f "$NGINX_DEFAULT_TEMPLATE" ]]; then
  echo "[deploy-fe] nginx template not found: $NGINX_DEFAULT_TEMPLATE" >&2
  exit 1
fi

echo "[deploy-fe] pulling frontend image: $FRONTEND_IMAGE_REF"
docker pull "$FRONTEND_IMAGE_REF"

export BE_PRIVATE_IP

export FRONTEND_IMAGE="$FRONTEND_IMAGE_REF"

echo "[deploy-fe] deploying frontend + reverse-proxy"
docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up -d --no-deps frontend reverse-proxy

echo "[deploy-fe] waiting for nginx health"
for i in $(seq 1 50); do
  if curl -fsS http://localhost/nginx-health >/dev/null; then
    break
  fi
  sleep 2
  if [[ "$i" -eq 50 ]]; then
    echo "[deploy-fe] nginx health check failed" >&2
    exit 1
  fi
done

mkdir -p "$STATE_DIR"
if [[ -f "$CURRENT_FILE" ]]; then
  previous_image="$(cat "$CURRENT_FILE")"
  if [[ -n "$previous_image" && "$previous_image" != "$FRONTEND_IMAGE_REF" ]]; then
    echo "$previous_image" > "$PREVIOUS_FILE"
  fi
fi
echo "$FRONTEND_IMAGE_REF" > "$CURRENT_FILE"

echo "[deploy-fe] cleaning up old images"
docker system prune -a -f --volumes

echo "[deploy-fe] done"
