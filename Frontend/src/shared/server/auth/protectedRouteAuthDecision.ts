const JWT_EXPIRY_SKEW_SECONDS = 5;

export const PROTECTED_ROUTE_AUTH_DECISION = {
 ALLOW: 'ALLOW',
 REDIRECT_LOGIN: 'REDIRECT_LOGIN',
 REDIRECT_HANDSHAKE: 'REDIRECT_HANDSHAKE',
} as const;

export type ProtectedRouteAuthDecision =
 (typeof PROTECTED_ROUTE_AUTH_DECISION)[keyof typeof PROTECTED_ROUTE_AUTH_DECISION];

interface ResolveProtectedRouteAuthDecisionOptions {
 accessToken?: string;
 refreshToken?: string;
 locale: string;
 nextPath: string;
}

export interface ProtectedRouteAuthDecisionResult {
 decision: ProtectedRouteAuthDecision;
 redirectPath: string | null;
 reason:
  | 'access_token_valid'
  | 'access_token_invalid_refresh_present'
  | 'missing_session_cookies';
}

function buildLoginPath(locale: string): string {
 return `/${locale}/login`;
}

function buildHandshakePath(nextPath: string): string {
 return `/api/auth/session/handshake?next=${encodeURIComponent(nextPath)}`;
}

function isNonEmptyToken(token: string | undefined): token is string {
 return typeof token === 'string' && token.trim().length > 0;
}

function decodeJwtPart(part: string): Record<string, unknown> | null {
 if (!part) {
  return null;
 }

 try {
  const normalized = part.replace(/-/g, '+').replace(/_/g, '/');
  const padded = normalized.padEnd(normalized.length + ((4 - (normalized.length % 4)) % 4), '=');
  const decoded = typeof atob === 'function'
   ? atob(padded)
   : Buffer.from(padded, 'base64').toString('utf8');
  return JSON.parse(decoded) as Record<string, unknown>;
 } catch {
  return null;
 }
}

function parseNumericClaim(value: unknown): number | null {
 if (typeof value === 'number' && Number.isFinite(value)) {
  return value;
 }

 if (typeof value === 'string' && value.trim().length > 0) {
  const parsed = Number(value);
  if (Number.isFinite(parsed)) {
   return parsed;
  }
 }

 return null;
}

function isAccessTokenValid(accessToken: string | undefined): boolean {
 if (!isNonEmptyToken(accessToken)) {
  return false;
 }

 const parts = accessToken.split('.');
 if (parts.length !== 3 || parts.some((part) => part.length === 0)) {
  return false;
 }

 const header = decodeJwtPart(parts[0]);
 const payload = decodeJwtPart(parts[1]);
 if (!header || !payload) {
  return false;
 }

 const algorithm = header.alg;
 if (typeof algorithm !== 'string' || algorithm.trim().length === 0 || algorithm.toLowerCase() === 'none') {
  return false;
 }

 const exp = parseNumericClaim(payload.exp);
 if (exp === null) {
  return false;
 }

 const now = Math.floor(Date.now() / 1000);
 return exp > now + JWT_EXPIRY_SKEW_SECONDS;
}

export function resolveProtectedRouteAuthDecision(
 options: ResolveProtectedRouteAuthDecisionOptions,
): ProtectedRouteAuthDecisionResult {
 const { accessToken, refreshToken, locale, nextPath } = options;
 if (isAccessTokenValid(accessToken)) {
  return {
   decision: PROTECTED_ROUTE_AUTH_DECISION.ALLOW,
   redirectPath: null,
   reason: 'access_token_valid',
  };
 }

 if (isNonEmptyToken(refreshToken)) {
  return {
   decision: PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_HANDSHAKE,
   redirectPath: buildHandshakePath(nextPath),
   reason: 'access_token_invalid_refresh_present',
  };
 }

 return {
  decision: PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_LOGIN,
  redirectPath: buildLoginPath(locale),
  reason: 'missing_session_cookies',
 };
}
