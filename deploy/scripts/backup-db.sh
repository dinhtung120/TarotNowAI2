#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
COMPOSE_FILE="${COMPOSE_FILE:-$ROOT_DIR/docker-compose.prod.yml}"
ENV_FILE="${ENV_FILE:-$ROOT_DIR/.env}"
BACKUP_DIR="${1:-$ROOT_DIR/backups/$(date +%Y%m%d-%H%M%S)}"

mkdir -p "$BACKUP_DIR"

compose_args=(-f "$COMPOSE_FILE")
if [[ -f "$ENV_FILE" ]]; then
  compose_args=(--env-file "$ENV_FILE" -f "$COMPOSE_FILE")
fi

dc() {
  docker compose "${compose_args[@]}" "$@"
}

echo "[backup] output directory: $BACKUP_DIR"

echo "[backup] dumping PostgreSQL..."
dc exec -T postgres sh -lc \
  'pg_dump -U "$POSTGRES_USER" -d "$POSTGRES_DB" --format=plain --no-owner --no-privileges' \
  > "$BACKUP_DIR/postgres.sql"

echo "[backup] dumping MongoDB..."
dc exec -T mongodb sh -lc \
  'mongodump --db "$MONGO_DB" --archive --gzip' \
  > "$BACKUP_DIR/mongodb.archive.gz"

echo "[backup] dumping Redis..."
dc exec -T redis redis-cli BGSAVE >/dev/null
sleep 3
dc cp redis:/data/dump.rdb "$BACKUP_DIR/redis.dump.rdb"

echo "[backup] completed."
