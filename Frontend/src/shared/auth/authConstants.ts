export const AUTH_COOKIE = {
 ACCESS: 'accessToken',
 REFRESH: 'refreshToken',
 DEVICE: 'deviceId',
} as const;

export const AUTH_HEADER = {
 IDEMPOTENCY_KEY: 'x-idempotency-key',
 DEVICE_ID: 'x-device-id',
 FORWARDED_USER_AGENT: 'x-forwarded-user-agent',
} as const;

export const AUTH_SESSION = {
 ACCESS_REFRESH_THRESHOLD_SECONDS: 60,
 DEFAULT_ACCESS_TTL_SECONDS: 600,
 DEFAULT_REFRESH_TTL_SECONDS: 30 * 24 * 60 * 60,
} as const;
