#!/usr/bin/env bash
# Đẩy file env lên server từ GitHub Actions Secrets rồi scp lên EC2 (không cần SSH tay từng máy).
# Thứ tự: nếu có PROD_DOTENV_B64 (không rỗng sau chuẩn hóa) thì dùng base64; không thì PROD_DOTENV_PLAIN; không thì kiểm tra file trên server.
# Tên file đích: PROD_ENV_FILE (mặc định .env), ví dụ .env.prod.
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

ENV_REL="${PROD_ENV_FILE:-.env}"
if [[ "$ENV_REL" == *".."* ]] || [[ "$ENV_REL" == /* ]]; then
  echo "::error::PROD_ENV_FILE phải là đường dẫn tương đối trong repo (không .., không bắt đầu bằng /)" >&2
  exit 1
fi
REMOTE_ENV_PATH="${REMOTE_REPO}/${ENV_REL}"

if [[ ! -f "$SSH_KEY" ]]; then
  echo "::error::SSH key not found: $SSH_KEY" >&2
  exit 1
fi

SSH_BASE=(ssh -i "$SSH_KEY" -o BatchMode=yes -o StrictHostKeyChecking=yes "$USER@$HOST")
SCP_BASE=(scp -i "$SSH_KEY" -o BatchMode=yes -o StrictHostKeyChecking=yes)

# Chuẩn hóa secret: paste nhiều dòng / có space / BOM UTF-8 — trước đây chỉ tr -d \\n\\n nên vẫn lỗi.
normalize_prod_dotenv_b64() {
  local raw="${PROD_DOTENV_B64:-}"
  # Bỏ BOM UTF-8 nếu có (paste từ editor)
  raw="${raw#$'\xef\xbb\xbf'}"
  # Bỏ mọi khoảng trắng (paste nhiều dòng / khoảng giữa các block base64)
  printf '%s' "$raw" | tr -d '[:space:]'
}

try_b64_decode_to_file() {
  local out="$1"
  local norm="$2"
  [[ -n "$norm" ]] || return 1
  if printf '%s' "$norm" | base64 -d >"$out" 2>/dev/null && [[ -s "$out" ]]; then
    return 0
  fi
  # Base64 URL-safe (một số tool dùng - _ thay vì + /)
  if printf '%s' "$norm" | tr '_-' '/+' | base64 -d >"$out" 2>/dev/null && [[ -s "$out" ]]; then
    return 0
  fi
  if command -v openssl >/dev/null 2>&1; then
    if printf '%s' "$norm" | openssl base64 -d -A -out "$out" 2>/dev/null && [[ -s "$out" ]]; then
      return 0
    fi
    if printf '%s' "$(printf '%s' "$norm" | tr '_-' '/+')" | openssl base64 -d -A -out "$out" 2>/dev/null && [[ -s "$out" ]]; then
      return 0
    fi
  fi
  return 1
}

decode_dotenv_b64_to_file() {
  local out="$1"
  local norm
  norm="$(normalize_prod_dotenv_b64)"
  [[ -n "$norm" ]] || return 1
  if try_b64_decode_to_file "$out" "$norm"; then
    return 0
  fi
  # Padding / biến thể base64 (python3 có trên runner GitHub và hầu hết self-hosted)
  if command -v python3 >/dev/null 2>&1; then
    if PROD_NORM="$norm" python3 -c '
import os, base64, sys
path = sys.argv[1]
s = "".join(os.environ.get("PROD_NORM", "").split())
if not s:
    sys.exit(1)
for dec in (base64.standard_b64decode, base64.urlsafe_b64decode):
    try:
        pad = (-len(s)) % 4
        data = dec(s + ("=" * pad))
        if data:
            open(path, "wb").write(data)
            sys.exit(0)
    except Exception:
        pass
sys.exit(1)
' "$out" 2>/dev/null && [[ -s "$out" ]]; then
      return 0
    fi
  fi
  return 1
}

write_plain_dotenv_to_file() {
  local out="$1"
  if [[ -z "${PROD_DOTENV_PLAIN:-}" ]]; then
    return 1
  fi
  if command -v python3 >/dev/null 2>&1; then
    python3 -c '
import os, sys
path = sys.argv[1]
text = os.environ.get("PROD_DOTENV_PLAIN", "")
if not text.strip():
    sys.exit(1)
open(path, "w", encoding="utf-8", newline="\n").write(text)
' "$out" 2>/dev/null && [[ -s "$out" ]]
  else
    printf '%s' "$PROD_DOTENV_PLAIN" >"$out" && [[ -s "$out" ]]
  fi
}

if [[ -n "$(normalize_prod_dotenv_b64)" ]]; then
  TMP=$(mktemp)
  chmod 600 "$TMP"
  if ! decode_dotenv_b64_to_file "$TMP" || [[ ! -s "$TMP" ]]; then
    norm_dbg="$(normalize_prod_dotenv_b64)"
    echo "::error::PROD_DOTENV_B64: không giải mã được." >&2
    if [[ -z "${norm_dbg:-}" ]]; then
      echo "::error::Secret base64 rỗng sau khi bỏ khoảng trắng — hoặc xóa PROD_DOTENV_B64 và chỉ dùng PROD_DOTENV_PLAIN (dán nội dung .env thô)." >&2
    else
      echo "::error::Độ dài ${#norm_dbg} ký tự — không phải base64 hợp lệ. Linux: base64 -w0 < .env  |  macOS: base64 -i .env | tr -d '[:space:]'  |  Hoặc tạo secret PROD_DOTENV_PLAIN (dán nguyên file .env)." >&2
    fi
    rm -f "$TMP"
    exit 1
  fi
  "${SCP_BASE[@]}" "$TMP" "${USER}@${HOST}:${REMOTE_ENV_PATH}"
  rm -f "$TMP"
  "${SSH_BASE[@]}" "chmod 600 '${REMOTE_ENV_PATH}' 2>/dev/null || true"
  echo "[ensure-remote-root-env] Đã scp (${ENV_REL}) từ PROD_DOTENV_B64 → ${HOST}:${REMOTE_ENV_PATH}"
  exit 0
fi

if [[ -n "${PROD_DOTENV_PLAIN:-}" ]]; then
  TMP=$(mktemp)
  chmod 600 "$TMP"
  if ! write_plain_dotenv_to_file "$TMP"; then
    echo "::error::PROD_DOTENV_PLAIN: không ghi được file (rỗng hoặc lỗi). GitHub secret tối đa ~48KB; file lớn hơn hãy dùng PROD_DOTENV_B64." >&2
    rm -f "$TMP"
    exit 1
  fi
  "${SCP_BASE[@]}" "$TMP" "${USER}@${HOST}:${REMOTE_ENV_PATH}"
  rm -f "$TMP"
  "${SSH_BASE[@]}" "chmod 600 '${REMOTE_ENV_PATH}' 2>/dev/null || true"
  echo "[ensure-remote-root-env] Đã scp (${ENV_REL}) từ PROD_DOTENV_PLAIN → ${HOST}:${REMOTE_ENV_PATH}"
  exit 0
fi

echo "[ensure-remote-root-env] Không có PROD_DOTENV_B64 / PROD_DOTENV_PLAIN — kiểm tra file ${ENV_REL} có sẵn trên server..."
if "${SSH_BASE[@]}" "test -f '${REMOTE_ENV_PATH}'"; then
  echo "[ensure-remote-root-env] OK: ${REMOTE_ENV_PATH} tồn tại trên ${HOST}"
  exit 0
fi

echo "::error::Thiếu secret (PROD_DOTENV_B64 hoặc PROD_DOTENV_PLAIN) và không có file ${REMOTE_ENV_PATH} trên ${HOST}." >&2
echo "::error::Cách sửa — chọn một:" >&2
echo "::error::  (1) Secret PROD_DOTENV_PLAIN = dán toàn bộ nội dung file .env local (Actions → Secrets; không dùng Variables công khai)." >&2
echo "::error::  (2) Hoặc PROD_DOTENV_B64 = base64 của file env (ổn định với file lớn / ký tự đặc biệt)." >&2
echo "::error::  (3) SSH: cd ${REMOTE_REPO} && cp .env.example ${ENV_REL} && chỉnh. Variable PROD_ENV_FILE nếu tên file khác .env." >&2
exit 1
