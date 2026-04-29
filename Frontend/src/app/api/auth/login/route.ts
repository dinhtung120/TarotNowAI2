import { NextRequest, NextResponse } from 'next/server';
import type { AuthResponse } from '@/features/auth/domain/types';
import { AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import {
 buildProblemResponse,
 clearAuthCookies,
 resolveAccessTtlSeconds,
 resolveDeviceIdFromRequest,
 setAccessCookie,
 setDeviceCookie,
 setRefreshCookieFromHeaders,
 unauthorizedResponse,
} from '@/app/api/auth/_shared';

interface LoginRequestBody {
 emailOrUsername: string;
 password: string;
 rememberMe?: boolean;
}

export async function POST(request: NextRequest): Promise<NextResponse> {
 const deviceId = resolveDeviceIdFromRequest(request);
 let payload: LoginRequestBody;

 try {
  payload = (await request.json()) as LoginRequestBody;
 } catch {
  return buildProblemResponse(400, AUTH_ERROR.TEMPORARY_FAILURE, AUTH_ERROR.TEMPORARY_FAILURE);
 }

  const result = await serverHttpRequest<AuthResponse>('/auth/login', {
  method: 'POST',
  json: payload,
  headers: {
   [AUTH_HEADER.DEVICE_ID]: deviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: request.headers.get('user-agent') ?? '',
  },
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });

 if (!result.ok) {
  if (result.status === 401) {
   return unauthorizedResponse(true, request);
  }

  return buildProblemResponse(result.status, result.error, result.error);
 }

 const accessToken = result.data.accessToken;
 if (!accessToken) {
  return unauthorizedResponse(true, request);
 }

 const response = NextResponse.json(
  {
   success: true,
   user: result.data.user,
   expiresInSeconds: resolveAccessTtlSeconds(result.data),
 },
 { status: 200 },
);

 clearAuthCookies(response, request);
 setAccessCookie(response, accessToken, resolveAccessTtlSeconds(result.data), request);
 const refreshCookieSet = setRefreshCookieFromHeaders(response, result.headers, request);
 if (!refreshCookieSet) {
  return unauthorizedResponse(true, request);
 }
 setDeviceCookie(response, deviceId, request);
 return response;
}
