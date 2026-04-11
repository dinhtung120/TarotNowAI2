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

read_env_file_value() {
  local key="$1"
  awk -F= -v key="$key" '
    /^[[:space:]]*#/ { next }
    $1 == key {
      sub(/^[^=]*=/, "", $0)
      gsub(/^[[:space:]]+|[[:space:]]+$/, "", $0)
      print $0
      exit
    }
  ' "$ENV_FILE"
}

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

PUBLIC_BASE_URL_VALUE="${PUBLIC_BASE_URL:-$(read_env_file_value PUBLIC_BASE_URL)}"
NEXT_PUBLIC_API_URL_VALUE="${NEXT_PUBLIC_API_URL:-$(read_env_file_value NEXT_PUBLIC_API_URL)}"

if [[ -z "$PUBLIC_BASE_URL_VALUE" ]]; then
  echo "[deploy-fe] missing PUBLIC_BASE_URL (env or $ENV_FILE)" >&2
  exit 1
fi

if [[ -z "$NEXT_PUBLIC_API_URL_VALUE" ]]; then
  echo "[deploy-fe] missing NEXT_PUBLIC_API_URL (env or $ENV_FILE)" >&2
  exit 1
fi

PUBLIC_BASE_URL_NORMALIZED="${PUBLIC_BASE_URL_VALUE%/}"
NEXT_PUBLIC_API_URL_NORMALIZED="${NEXT_PUBLIC_API_URL_VALUE%/}"

if [[ "${ALLOW_CROSS_ORIGIN_API:-false}" != "true" ]]; then
  case "$NEXT_PUBLIC_API_URL_NORMALIZED" in
    "$PUBLIC_BASE_URL_NORMALIZED"/api|"$PUBLIC_BASE_URL_NORMALIZED"/api/v1)
      ;;
    *)
      echo "[deploy-fe] NEXT_PUBLIC_API_URL ($NEXT_PUBLIC_API_URL_NORMALIZED) does not match PUBLIC_BASE_URL ($PUBLIC_BASE_URL_NORMALIZED)." >&2
      echo "[deploy-fe] This mismatch can break App Router SPA behavior behind proxy/CDN. Set ALLOW_CROSS_ORIGIN_API=true if intentional." >&2
      exit 1
      ;;
  esac
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

echo "[deploy-fe] verifying App Router RSC passthrough"
rsc_headers_file="$(mktemp)"
if ! curl -fsSI -H 'RSC: 1' -H 'Next-Router-State-Tree: [\"\",{}]' "http://localhost/vi?_rsc=healthcheck" >"$rsc_headers_file"; then
  rm -f "$rsc_headers_file"
  echo "[deploy-fe] failed to probe RSC endpoint through nginx" >&2
  exit 1
fi
if ! grep -qi '^content-type:[[:space:]]*text/x-component' "$rsc_headers_file"; then
  echo "[deploy-fe] unexpected RSC response content-type via nginx:" >&2
  cat "$rsc_headers_file" >&2
  rm -f "$rsc_headers_file"
  exit 1
fi
rm -f "$rsc_headers_file"

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
