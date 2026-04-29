#!/usr/bin/env bash
# ──────────────────────────────────────────────────────────────────────
# deploy_fe.sh – Triển khai Frontend container + Nginx reverse proxy
#
# Quy trình:
#   1. Kiểm tra biến môi trường bắt buộc
#   2. Pull image frontend mới nhất
#   3. Khởi động container frontend + reverse-proxy
#   4. Health check: nginx alive → HTML page → cập nhật release state
#
# Lý do thiết kế deploy script kiểm tra nhiều bước:
#   - Phát hiện lỗi sớm trước khi traffic thực tế đến
#   - Đảm bảo cả nginx proxy lẫn Next.js app đều hoạt động end-to-end
#   - Tránh deploy "thành công" nhưng thực tế app bị broken
# ──────────────────────────────────────────────────────────────────────
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "usage: $0 <frontend_image_ref>"
  exit 1
fi

# Biến BE_PRIVATE_IP là IP nội bộ của backend server trên private network
# Cần thiết để nginx proxy API requests đến backend container
if [[ -z "${BE_PRIVATE_IP:-}" ]]; then
  echo "[deploy-fe] missing required env BE_PRIVATE_IP" >&2
  exit 1
fi

FRONTEND_IMAGE_REF="$1"
REPO_DIR="${REPO_DIR:-/opt/tarotnow/TarotNowAI2}"
ENV_FILE="${ENV_FILE:-.env}"
COMPOSE_FILE="${COMPOSE_FILE:-docker-compose.prod.yml}"
NGINX_DEFAULT_TEMPLATE="${NGINX_DEFAULT_TEMPLATE:-deploy/nginx/conf.d/default.conf.template}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# shellcheck source=lib-env.sh
source "$SCRIPT_DIR/lib-env.sh"

# Thư mục lưu trữ trạng thái release (image hiện tại và trước đó)
# Dùng cho rollback nhanh khi cần quay về version cũ
STATE_DIR="${STATE_DIR:-/opt/tarotnow/release-state}"
CURRENT_FILE="$STATE_DIR/frontend_current_image"
PREVIOUS_FILE="$STATE_DIR/frontend_previous_image"

# ──────────────────────────────────────────────────────────────────────
# Hàm đọc giá trị từ .env file
# Dùng awk để tránh phụ thuộc vào source (có thể gây side-effect)
# Bỏ qua comment lines (#) và trailing whitespace
# ──────────────────────────────────────────────────────────────────────
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

materialize_root_env_from_ci "$ENV_FILE" || exit 1

# ── Kiểm tra file cần thiết tồn tại ──
if [[ ! -f "$ENV_FILE" ]]; then
  echo "[deploy-fe] env file not found: $REPO_DIR/$ENV_FILE" >&2
  echo "[deploy-fe] Tạo thủ công: cp .env.example .env && chỉnh secrets, hoặc đặt GitHub secret PROD_DOTENV_B64 (base64 của .env)." >&2
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

# ── Resolve URL values ──
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

# Normalize: bỏ trailing slash để so sánh chính xác
PUBLIC_BASE_URL_NORMALIZED="${PUBLIC_BASE_URL_VALUE%/}"
NEXT_PUBLIC_API_URL_NORMALIZED="${NEXT_PUBLIC_API_URL_VALUE%/}"
PUBLIC_BASE_AUTHORITY="${PUBLIC_BASE_URL_NORMALIZED#*://}"
PUBLIC_BASE_AUTHORITY="${PUBLIC_BASE_AUTHORITY%%/*}"

if [[ -z "$PUBLIC_BASE_AUTHORITY" ]]; then
  echo "[deploy-fe] cannot parse host from PUBLIC_BASE_URL ($PUBLIC_BASE_URL_VALUE)" >&2
  exit 1
fi

# ── Kiểm tra API URL phải cùng origin với PUBLIC_BASE_URL ──
# Tránh CORS issues do API ở domain khác frontend
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

# ── Pull và deploy ──
echo "[deploy-fe] pulling frontend image: $FRONTEND_IMAGE_REF"
docker pull "$FRONTEND_IMAGE_REF"

export BE_PRIVATE_IP
export FRONTEND_IMAGE="$FRONTEND_IMAGE_REF"

echo "[deploy-fe] deploying frontend + reverse-proxy"
docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up -d --no-deps frontend reverse-proxy

# ──────────────────────────────────────────────────────────────────────
# Health Check #1: Nginx alive
# Kiểm tra endpoint /nginx-health trả về 200 OK
# Retry tối đa 50 lần, mỗi lần cách 2s (tổng ~100s timeout)
# ──────────────────────────────────────────────────────────────────────
echo "[deploy-fe] waiting for nginx health"
for i in $(seq 1 50); do
  health_code="$(curl -sS -o /dev/null -w '%{http_code}' -H "Host: $PUBLIC_BASE_AUTHORITY" "http://localhost/nginx-health" || true)"
  if [[ "$health_code" == "200" ]]; then
    break
  fi
  sleep 2
  if [[ "$i" -eq 50 ]]; then
    echo "[deploy-fe] nginx health check failed (last status: ${health_code:-n/a})" >&2
    exit 1
  fi
done

# ──────────────────────────────────────────────────────────────────────
# Health Check #2: Next.js App Router hoạt động end-to-end qua nginx
#
# THIẾT KẾ QUAN TRỌNG:
# - Sử dụng HEAD request lấy HTML page (Accept: text/html) thay vì
#   giả lập RSC flight request.
# - Lý do: RSC flight request cần header Next-Router-State-Tree đúng
#   format (phụ thuộc vào version Next.js). Nếu format sai, Next.js
#   trả về 500 "router state header could not be parsed".
# - Kiểm tra HTML response đủ để xác nhận Next.js đang xử lý request
#   qua nginx → app hoạt động.
# ──────────────────────────────────────────────────────────────────────
echo "[deploy-fe] verifying Next.js responds through nginx"
health_response_file="$(mktemp)"
if ! curl -sSI \
  -H "Host: $PUBLIC_BASE_AUTHORITY" \
  -H 'Accept: text/html' \
  "http://localhost/vi" \
  >"$health_response_file" 2>&1; then
  echo "[deploy-fe] Next.js health check failed through nginx" >&2
  cat "$health_response_file" >&2
  rm -f "$health_response_file"
  exit 1
fi

# Xác nhận status là 200 thay vì redirect/misroute.
if ! grep -Eqi '^HTTP/[0-9.]+[[:space:]]+200([[:space:]]|$)' "$health_response_file"; then
  echo "[deploy-fe] unexpected Next.js status through nginx (expected 200):" >&2
  cat "$health_response_file" >&2
  rm -f "$health_response_file"
  exit 1
fi

# Xác nhận response là HTML (text/html) chứ không phải error page
if ! grep -qi '^content-type:[[:space:]]*text/html' "$health_response_file"; then
  echo "[deploy-fe] unexpected response content-type from Next.js:" >&2
  cat "$health_response_file" >&2
  rm -f "$health_response_file"
  exit 1
fi
rm -f "$health_response_file"

# ──────────────────────────────────────────────────────────────────────
# Health Check #3: next/image không redirect về HTTP
#
# Mục tiêu:
# - Phát hiện sớm lỗi mixed-content do Nginx tự 301:
#   /_next/image?... -> http://.../_next/image/?...
# - Xác nhận optimizer endpoint trả image trực tiếp.
# ──────────────────────────────────────────────────────────────────────
echo "[deploy-fe] verifying next/image endpoint does not redirect to http"
image_probe_file="$(mktemp)"
if ! curl -sSI \
  -H "Host: $PUBLIC_BASE_AUTHORITY" \
  "http://localhost/_next/image?url=%2Ffavicon.ico&w=64&q=75" \
  >"$image_probe_file" 2>&1; then
  echo "[deploy-fe] next/image health check failed" >&2
  cat "$image_probe_file" >&2
  rm -f "$image_probe_file"
  exit 1
fi

# Endpoint local probe phải trả ảnh trực tiếp (200), không redirect.
if ! grep -Eqi '^HTTP/[0-9.]+[[:space:]]+200([[:space:]]|$)' "$image_probe_file"; then
  echo "[deploy-fe] next/image returned non-200 status on local probe:" >&2
  cat "$image_probe_file" >&2
  rm -f "$image_probe_file"
  exit 1
fi

if grep -qi '^location:[[:space:]]*http://' "$image_probe_file"; then
  echo "[deploy-fe] next/image returned insecure redirect (http://), potential mixed-content regression:" >&2
  cat "$image_probe_file" >&2
  rm -f "$image_probe_file"
  exit 1
fi

if ! grep -qi '^content-type:[[:space:]]*image/' "$image_probe_file"; then
  echo "[deploy-fe] next/image did not return an image response:" >&2
  cat "$image_probe_file" >&2
  rm -f "$image_probe_file"
  exit 1
fi
rm -f "$image_probe_file"

echo "[deploy-fe] health checks passed"

# ── Cập nhật release state cho rollback ──
mkdir -p "$STATE_DIR"
if [[ -f "$CURRENT_FILE" ]]; then
  previous_image="$(cat "$CURRENT_FILE")"
  if [[ -n "$previous_image" && "$previous_image" != "$FRONTEND_IMAGE_REF" ]]; then
    echo "$previous_image" > "$PREVIOUS_FILE"
  fi
fi
echo "$FRONTEND_IMAGE_REF" > "$CURRENT_FILE"

# ── Cleanup ──
echo "[deploy-fe] cleaning up old images"
# Chỉ dọn image/build cache cũ, không xóa volumes toàn cục.
docker image prune -f --filter "until=168h"
docker builder prune -f --filter "until=168h"

echo "[deploy-fe] done"
