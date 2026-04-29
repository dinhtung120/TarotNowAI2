import { createHash } from 'node:crypto';
import { NextRequest, NextResponse } from 'next/server';
import type { AuthResponse } from '@/features/auth/domain/types';
import { AUTH_COOKIE, AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import {
 buildProblemResponse,
 clearAuthCookies,
 normalizeAuthTokenCookieValue,
 resolveAccessTtlSeconds,
 resolveDeviceIdFromRequest,
 setAccessCookie,
 setDeviceCookie,
 setRefreshCookieFromHeaders,
 unauthorizedResponse,
} from '@/app/api/auth/_shared';

function shouldClearCookiesForRefreshFailure(status: number, error: string): boolean {
 if (status === 401 || status === 403) {
  return true;
 }

 return error === AUTH_ERROR.UNAUTHORIZED
  || error === AUTH_ERROR.TOKEN_EXPIRED
  || error === AUTH_ERROR.TOKEN_REPLAY;
}

function resolveRefreshIdempotencyKey(
 refreshToken: string,
 deviceId: string,
 userAgent: string,
): string {
 const material = `${refreshToken.trim()}|${deviceId.trim()}|${userAgent.trim()}`;
 const digest = createHash('sha256').update(material).digest('hex');
 return `bff-${digest}`;
}

export async function executeRefreshRoute(request: NextRequest): Promise<NextResponse> {
 const refreshTokenRaw = request.cookies.get(AUTH_COOKIE.REFRESH)?.value;
 const refreshToken = refreshTokenRaw ? normalizeAuthTokenCookieValue(refreshTokenRaw) : undefined;
 if (!refreshToken) {
  return unauthorizedResponse(true);
 }

 const deviceId = resolveDeviceIdFromRequest(request);
 const userAgent = request.headers.get('user-agent') ?? '';
 const idempotencyKey = resolveRefreshIdempotencyKey(refreshToken, deviceId, userAgent);

 let result: Awaited<ReturnType<typeof serverHttpRequest<AuthResponse>>>;
 try {
  result = await serverHttpRequest<AuthResponse>('/auth/refresh', {
   method: 'POST',
   headers: {
    Cookie: `${AUTH_COOKIE.REFRESH}=${refreshToken}`,
    [AUTH_HEADER.IDEMPOTENCY_KEY]: idempotencyKey,
    [AUTH_HEADER.DEVICE_ID]: deviceId,
    [AUTH_HEADER.FORWARDED_USER_AGENT]: userAgent,
   },
   cache: 'no-store',
   fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
  });
 } catch {
  return buildProblemResponse(503, AUTH_ERROR.TEMPORARY_FAILURE, AUTH_ERROR.TEMPORARY_FAILURE);
 }

 if (!result.ok) {
  if (shouldClearCookiesForRefreshFailure(result.status, result.error)) {
   const response = buildProblemResponse(
    401,
    result.error || AUTH_ERROR.UNAUTHORIZED,
    result.error || AUTH_ERROR.UNAUTHORIZED,
   );
   clearAuthCookies(response);
   return response;
  }

  const transientStatus = result.status >= 500 ? 503 : result.status;
  const errorCode = result.error ?? AUTH_ERROR.TEMPORARY_FAILURE;
  return buildProblemResponse(transientStatus, errorCode, errorCode);
 }

 const accessToken = result.data.accessToken;
 if (!accessToken) {
  return unauthorizedResponse(true);
 }

 const response = NextResponse.json(
  {
   success: true,
   user: result.data.user,
   expiresInSeconds: resolveAccessTtlSeconds(result.data),
  },
  { status: 200 },
 );

 setAccessCookie(response, accessToken, resolveAccessTtlSeconds(result.data));
 const refreshCookieSet = setRefreshCookieFromHeaders(response, result.headers);
 if (!refreshCookieSet) {
  return unauthorizedResponse(true);
 }
 setDeviceCookie(response, deviceId);
 return response;
}
