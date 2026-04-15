import { NextRequest, NextResponse } from 'next/server';
import type { AuthResponse } from '@/features/auth/domain/types';
import { AUTH_COOKIE, AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import {
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

export async function POST(request: NextRequest): Promise<NextResponse> {
 const refreshToken = request.cookies.get(AUTH_COOKIE.REFRESH)?.value;
 if (!refreshToken) {
  return unauthorizedResponse(true);
 }

 const idempotencyKey = request.headers.get(AUTH_HEADER.IDEMPOTENCY_KEY) ?? crypto.randomUUID();
 const deviceId = resolveDeviceIdFromRequest(request);

  const result = await serverHttpRequest<AuthResponse>('/auth/refresh', {
  method: 'POST',
  headers: {
   Cookie: `${AUTH_COOKIE.REFRESH}=${refreshToken}`,
   [AUTH_HEADER.IDEMPOTENCY_KEY]: idempotencyKey,
   [AUTH_HEADER.DEVICE_ID]: deviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: request.headers.get('user-agent') ?? '',
  },
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });

 if (!result.ok) {
  if (shouldClearCookiesForRefreshFailure(result.status, result.error)) {
   return unauthorizedResponse(true);
  }

  return NextResponse.json(
   { success: false, error: result.error },
   { status: result.status >= 500 ? 503 : result.status },
  );
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
