import { NextRequest, NextResponse } from 'next/server';
import type { UserProfile } from '@/features/auth/domain/types';
import { AUTH_ERROR, isTerminalAuthError } from '@/shared/domain/authErrors';
import { AUTH_COOKIE, AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';
import { unauthorizedResponse } from '@/app/api/auth/_shared';

interface SessionResponsePayload {
 success: boolean;
 authenticated: boolean;
 user?: UserProfile;
 error?: string;
}

function getSetCookieHeaders(headers: Headers): string[] {
 if (typeof headers.getSetCookie === 'function') {
  return headers.getSetCookie();
 }

 const raw = headers.get('set-cookie');
 return raw ? raw.split(/,\s*(?=[a-zA-Z_]+=)/) : [];
}

function appendSetCookies(source: Headers, target: NextResponse): void {
 for (const cookie of getSetCookieHeaders(source)) {
  if (cookie.trim().length > 0) {
   target.headers.append('set-cookie', cookie);
  }
 }
}

async function refreshSessionViaInternalRoute(request: NextRequest): Promise<Response> {
 const refreshUrl = new URL('/api/auth/refresh', request.url);
 const deviceId = request.headers.get(AUTH_HEADER.DEVICE_ID)
  ?? request.cookies.get(AUTH_COOKIE.DEVICE)?.value
  ?? '';
 return fetch(refreshUrl, {
  method: 'POST',
  headers: {
   Cookie: request.headers.get('cookie') ?? '',
   [AUTH_HEADER.IDEMPOTENCY_KEY]: crypto.randomUUID(),
   [AUTH_HEADER.DEVICE_ID]: deviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: request.headers.get('user-agent') ?? '',
  },
  cache: 'no-store',
 });
}

async function resolveAuthError(response: Response): Promise<string | undefined> {
 try {
  const payload = (await response.clone().json()) as { error?: string };
  return payload.error;
 } catch {
  return undefined;
 }
}

function resolveTransientStatus(status: number): number {
 return status >= 500 ? 503 : status;
}

export async function GET(request: NextRequest): Promise<NextResponse> {
 const accessToken = request.cookies.get(AUTH_COOKIE.ACCESS)?.value;
 const refreshToken = request.cookies.get(AUTH_COOKIE.REFRESH)?.value;
 if (!accessToken && !refreshToken) {
  return unauthorizedResponse();
 }

 let session: Awaited<ReturnType<typeof getServerSessionSnapshot>>;
 try {
  session = await getServerSessionSnapshot({ allowRefresh: false });
 } catch {
  return NextResponse.json(
   {
    success: false,
    authenticated: false,
    error: AUTH_ERROR.TEMPORARY_FAILURE,
   } satisfies SessionResponsePayload,
   { status: 503 },
  );
 }
 if (session.authenticated && session.user) {
  return NextResponse.json(
   {
    success: true,
    authenticated: true,
    user: session.user,
   } satisfies SessionResponsePayload,
   { status: 200 },
  );
 }

 if (!refreshToken) {
  return unauthorizedResponse(true);
 }

 let refreshResponse: Response;
 try {
  refreshResponse = await refreshSessionViaInternalRoute(request);
 } catch {
  return NextResponse.json(
   {
    success: false,
    authenticated: false,
    error: AUTH_ERROR.TEMPORARY_FAILURE,
   } satisfies SessionResponsePayload,
   { status: 503 },
  );
 }
 if (!refreshResponse.ok) {
  const error = await resolveAuthError(refreshResponse);
  if (refreshResponse.status === 401
   || refreshResponse.status === 403
   || isTerminalAuthError(error)) {
   return unauthorizedResponse(true);
  }

  return NextResponse.json(
   {
    success: false,
    authenticated: false,
    error: error ?? AUTH_ERROR.TEMPORARY_FAILURE,
   } satisfies SessionResponsePayload,
   { status: resolveTransientStatus(refreshResponse.status) },
  );
 }

 let refreshPayload: { success?: boolean; user?: UserProfile; error?: string };
 try {
  refreshPayload = (await refreshResponse.clone().json()) as { success?: boolean; user?: UserProfile; error?: string };
 } catch {
  return unauthorizedResponse(true);
 }

 if (!refreshPayload.success || !refreshPayload.user) {
  return unauthorizedResponse(true);
 }

 const response = NextResponse.json(
  {
   success: true,
   authenticated: true,
   user: refreshPayload.user,
  } satisfies SessionResponsePayload,
  { status: 200 },
 );
 appendSetCookies(refreshResponse.headers, response);
 return response;
}
