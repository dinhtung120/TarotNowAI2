#!/usr/bin/env bash
# Đẩy .env lên server từ GitHub secret PROD_DOTENV_B64 (scp), hoặc kiểm tra đã có .env trên server.
# Chạy trên runner (self-hosted / ubuntu) sau bước cấu hình SSH.
# Usage: ensure-remote-root-env.sh <ssh_key_path> <user> <host> <remote_repo_absolute_path>
set -euo pipefail

if [[ $# -lt 4 ]]; then
  echo "usage: $0 <ssh_key_path> <user> <host> <remote_repo_absolute_path>" >&2
  exit 1
fi

SSH_KEY="$1"
USER="$2"
HOST="$3"
REMOTE_REPO="${4%/}"

if [[ ! -f "$SSH_KEY" ]]; then
  echo "::error::SSH key not found: $SSH_KEY" >&2
  exit 1
fi

SSH_BASE=(ssh -i "$SSH_KEY" -o BatchMode=yes -o StrictHostKeyChecking=yes "$USER@$HOST")
SCP_BASE=(scp -i "$SSH_KEY" -o BatchMode=yes -o StrictHostKeyChecking=yes)

decode_dotenv_b64_to_file() {
  local out="$1"
  # Một dòng hoặc paste nhiều dòng từ GitHub UI — bỏ CR/LF giữa các chunk
  printf '%s' "${PROD_DOTENV_B64:-}" | tr -d '\n\r' | base64 -d >"$out" 2>/dev/null
}

if [[ -n "${PROD_DOTENV_B64:-}" ]]; then
  TMP=$(mktemp)
  chmod 600 "$TMP"
  if ! decode_dotenv_b64_to_file "$TMP"; then
    echo "::error::PROD_DOTENV_B64: không giải mã được (base64 sai định dạng)." >&2
    echo "::error::Tạo lại: Linux: base64 -w0 < .env   macOS: base64 -i .env | tr -d '\n'" >&2
    rm -f "$TMP"
    exit 1
  fi
  if [[ ! -s "$TMP" ]]; then
    echo "::error::PROD_DOTENV_B64 giải mã ra file rỗng — kiểm tra nội dung .env và lệnh base64." >&2
    rm -f "$TMP"
    exit 1
  fi
  REMOTE_PATH="${REMOTE_REPO}/.env"
  "${SCP_BASE[@]}" "$TMP" "${USER}@${HOST}:${REMOTE_PATH}"
  rm -f "$TMP"
  "${SSH_BASE[@]}" "chmod 600 '${REMOTE_REPO}/.env' 2>/dev/null || true"
  echo "[ensure-remote-root-env] Đã scp .env từ PROD_DOTENV_B64 → ${HOST}:${REMOTE_PATH}"
  exit 0
fi

echo "[ensure-remote-root-env] PROD_DOTENV_B64 chưa đặt — kiểm tra .env có sẵn trên server..."
if "${SSH_BASE[@]}" "test -f '${REMOTE_REPO}/.env'"; then
  echo "[ensure-remote-root-env] OK: ${REMOTE_REPO}/.env tồn tại trên ${HOST}"
  exit 0
fi

echo "::error::Thiếu GitHub repository secret PROD_DOTENV_B64 và không có file ${REMOTE_REPO}/.env trên ${HOST}." >&2
echo "::error::Cách sửa — chọn một:" >&2
echo "::error::  (1) GitHub → Settings → Secrets → Actions → New: tên PROD_DOTENV_B64, value = base64 một dòng của file .env production (UTF-8)." >&2
echo "::error::  (2) SSH vào ${HOST}: cd ${REMOTE_REPO} && cp .env.example .env && chỉnh secrets." >&2
exit 1
