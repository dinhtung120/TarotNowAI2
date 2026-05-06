import { NextRequest, NextResponse } from 'next/server';
import { AUTH_ERROR } from '@/shared/models/authErrors';
import { AUTH_HEADER } from '@/shared/auth/authConstants';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';
import { buildProblemResponse, clearAuthCookies, resolveDeviceIdFromRequest } from '@/app/api/auth/_shared';

interface LogoutPayload {
 revokeAll?: boolean;
}

function shouldClearCookiesAfterLogout(
 status: number,
 error: string | undefined,
): boolean {
 if (status === 401 || status === 403) {
  return true;
 }

 return error === AUTH_ERROR.UNAUTHORIZED;
}

export async function POST(request: NextRequest): Promise<NextResponse> {
 let payload: LogoutPayload = {};
 try {
  payload = (await request.json()) as LogoutPayload;
 } catch {
  payload = {};
 }

 const revokeAll = payload.revokeAll === true;
 const logoutPath = revokeAll ? '/auth/logout?revokeAll=true' : '/auth/logout';
 const deviceId = resolveDeviceIdFromRequest(request);
 const result = await serverHttpRequest<Record<string, unknown>>(logoutPath, {
  method: 'POST',
  headers: {
   Cookie: request.headers.get('cookie') ?? '',
   [AUTH_HEADER.DEVICE_ID]: deviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: request.headers.get('user-agent') ?? '',
  },
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });

 const shouldClearCookies = result.ok || shouldClearCookiesAfterLogout(result.status, result.error);
 const response = result.ok
  ? NextResponse.json({ success: true }, { status: 200 })
  : buildProblemResponse(result.status >= 500 ? 503 : result.status, result.error ?? AUTH_ERROR.TEMPORARY_FAILURE, result.error);
 if (shouldClearCookies) {
  clearAuthCookies(response, request);
 }

 return response;
}
