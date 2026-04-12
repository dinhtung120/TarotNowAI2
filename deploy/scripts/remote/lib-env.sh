#!/usr/bin/env bash
# Dùng chung cho deploy_db / deploy_be / deploy_fe.
# Nếu CI/CD đặt PROD_DOTENV_B64 (chuỗi base64 của toàn bộ file .env UTF-8), ghi ra ENV_FILE trước khi kiểm tra tồn tại.
materialize_root_env_from_ci() {
  local env_file="${1:-.env}"
  if [[ -z "${PROD_DOTENV_B64:-}" ]]; then
    return 0
  fi
  if ! printf '%s' "$PROD_DOTENV_B64" | base64 -d >"$env_file" 2>/dev/null; then
    echo "[env] PROD_DOTENV_B64: giải mã base64 thất bại (kỳ vọng: base64 của file .env UTF-8)" >&2
    return 1
  fi
  chmod 600 "$env_file" 2>/dev/null || true
}
