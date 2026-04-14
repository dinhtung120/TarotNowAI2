export const AUTH_ERROR = {
 UNAUTHORIZED: 'AUTH_UNAUTHORIZED',
 TOKEN_REPLAY: 'AUTH_TOKEN_REPLAY',
 TOKEN_EXPIRED: 'AUTH_TOKEN_EXPIRED',
 RATE_LIMITED: 'AUTH_RATE_LIMITED',
} as const;

export type AuthErrorCode = (typeof AUTH_ERROR)[keyof typeof AUTH_ERROR];

export function isUnauthorizedError(error: string | undefined): boolean {
 return error === AUTH_ERROR.UNAUTHORIZED;
}
