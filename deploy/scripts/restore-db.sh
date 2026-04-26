#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "usage: $0 <backup_dir>"
  exit 1
fi

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
COMPOSE_FILE="${COMPOSE_FILE:-$ROOT_DIR/docker-compose.prod.yml}"
ENV_FILE="${ENV_FILE:-$ROOT_DIR/.env}"
BACKUP_DIR="$1"

POSTGRES_FILE="$BACKUP_DIR/postgres.sql"
MONGO_FILE="$BACKUP_DIR/mongodb.archive.gz"
REDIS_FILE="$BACKUP_DIR/redis.dump.rdb"

for file in "$POSTGRES_FILE" "$MONGO_FILE" "$REDIS_FILE"; do
  if [[ ! -f "$file" ]]; then
    echo "missing backup file: $file"
    exit 1
  fi
done

compose_args=(-f "$COMPOSE_FILE")
if [[ -f "$ENV_FILE" ]]; then
  compose_args=(--env-file "$ENV_FILE" -f "$COMPOSE_FILE")
fi

dc() {
  docker compose "${compose_args[@]}" "$@"
}

echo "[restore] restoring PostgreSQL..."
cat "$POSTGRES_FILE" | dc exec -T postgres sh -lc \
  'psql -v ON_ERROR_STOP=1 -1 -U "$POSTGRES_USER" -d "$POSTGRES_DB"'

echo "[restore] restoring MongoDB..."
cat "$MONGO_FILE" | dc exec -T mongodb sh -lc \
  'mongorestore --drop --archive --gzip'

echo "[restore] restoring Redis..."
dc cp "$REDIS_FILE" redis:/data/dump.rdb
dc exec -T redis redis-cli SHUTDOWN NOSAVE || true
dc up -d redis

echo "[restore] completed."
