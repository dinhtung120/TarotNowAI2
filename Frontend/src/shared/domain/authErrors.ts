export const AUTH_ERROR = {
 UNAUTHORIZED: 'AUTH_UNAUTHORIZED',
 TOKEN_REPLAY: 'AUTH_TOKEN_REPLAY',
 TOKEN_EXPIRED: 'AUTH_TOKEN_EXPIRED',
 RATE_LIMITED: 'AUTH_RATE_LIMITED',
 TEMPORARY_FAILURE: 'AUTH_TEMPORARY_FAILURE',
} as const;

export type AuthErrorCode = (typeof AUTH_ERROR)[keyof typeof AUTH_ERROR];

export function isUnauthorizedError(error: string | undefined): boolean {
 return error === AUTH_ERROR.UNAUTHORIZED;
}

export function isTerminalAuthError(error: string | undefined): boolean {
 return error === AUTH_ERROR.UNAUTHORIZED
  || error === AUTH_ERROR.TOKEN_EXPIRED
  || error === AUTH_ERROR.TOKEN_REPLAY;
}
