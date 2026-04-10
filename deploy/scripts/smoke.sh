#!/usr/bin/env bash
set -euo pipefail

BASE_URL="${1:-http://localhost}"

echo "[smoke] checking reverse proxy"
curl -fsS "$BASE_URL/nginx-health" >/dev/null

echo "[smoke] checking backend live"
curl -fsS "$BASE_URL/api/v1/health/live" >/dev/null

echo "[smoke] checking backend ready"
curl -fsS "$BASE_URL/api/v1/health/ready" >/dev/null

echo "[smoke] checking frontend"
curl -fsS "$BASE_URL/" >/dev/null

echo "[smoke] all checks passed."
