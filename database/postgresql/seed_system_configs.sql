-- Keep bootstrap idempotent even when system_configs was created by older migrations.
ALTER TABLE system_configs
    ALTER COLUMN updated_at SET DEFAULT NOW();

UPDATE system_configs
SET updated_at = NOW()
WHERE updated_at IS NULL;

-- Không seed key legacy tại bootstrap.
-- Bộ key chuẩn được quản lý tập trung bởi SystemConfigRegistry và bootstrap runtime.
