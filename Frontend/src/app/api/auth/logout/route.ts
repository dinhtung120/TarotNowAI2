import { NextRequest, NextResponse } from 'next/server';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { clearAuthCookies, resolveDeviceIdFromRequest } from '@/app/api/auth/_shared';

interface LogoutPayload {
 revokeAll?: boolean;
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

 const response = NextResponse.json(
  result.ok ? { success: true } : { success: false, error: result.error },
  { status: result.ok ? 200 : result.status },
 );
 clearAuthCookies(response);
 return response;
}
