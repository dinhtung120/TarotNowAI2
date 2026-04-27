import { verifyAccessToken } from '@/shared/server/auth/accessTokenVerifier';

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
  | 'missing_session_cookies'
  | 'access_token_invalid_missing_verifier_config';
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

export async function resolveProtectedRouteAuthDecision(
 options: ResolveProtectedRouteAuthDecisionOptions,
): Promise<ProtectedRouteAuthDecisionResult> {
 const { accessToken, refreshToken, locale, nextPath } = options;

 const verification = await verifyAccessToken(accessToken);
 if (verification.valid) {
  return {
   decision: PROTECTED_ROUTE_AUTH_DECISION.ALLOW,
   redirectPath: null,
   reason: 'access_token_valid',
  };
 }

 if (verification.reason === 'missing_verifier_config') {
  return {
   decision: PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_LOGIN,
   redirectPath: buildLoginPath(locale),
   reason: 'access_token_invalid_missing_verifier_config',
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
