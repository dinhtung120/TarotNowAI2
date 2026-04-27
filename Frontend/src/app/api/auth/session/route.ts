import { NextRequest, NextResponse } from 'next/server';
import type { UserProfile } from '@/features/auth/domain/types';
import { AUTH_ERROR, isTerminalAuthError } from '@/shared/domain/authErrors';
import { AUTH_COOKIE, AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import {
 getServerAccessTokenOrRefresh,
 getServerSessionSnapshot,
} from '@/shared/infrastructure/auth/serverAuth';
import { buildProblemResponse, unauthorizedResponse } from '@/app/api/auth/_shared';

interface SessionResponsePayload {
 success: boolean;
 authenticated: boolean;
 user?: UserProfile;
 error?: string;
}

const SESSION_MODE = {
 FULL: 'full',
 LITE: 'lite',
} as const;

type SessionMode = (typeof SESSION_MODE)[keyof typeof SESSION_MODE];

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
   [AUTH_HEADER.DEVICE_ID]: deviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: request.headers.get('user-agent') ?? '',
  },
  cache: 'no-store',
 });
}

async function resolveAuthError(response: Response): Promise<string | undefined> {
 try {
  const payload = (await response.clone().json()) as {
   error?: string;
   errorCode?: string;
   detail?: string;
  };
  return payload.errorCode ?? payload.error ?? payload.detail;
 } catch {
  return undefined;
 }
}

function resolveTransientStatus(status: number): number {
 return status >= 500 ? 503 : status;
}

function resolveSessionMode(request: NextRequest): SessionMode {
 const rawMode = request.nextUrl.searchParams.get('mode');
 if (rawMode === SESSION_MODE.LITE) {
  return SESSION_MODE.LITE;
 }

 return SESSION_MODE.FULL;
}

function createSuccessResponse(user?: UserProfile): NextResponse {
 return NextResponse.json(
  {
   success: true,
   authenticated: true,
   user,
  } satisfies SessionResponsePayload,
  { status: 200 },
 );
}

function createTemporaryFailureResponse(): NextResponse {
 return buildProblemResponse(503, AUTH_ERROR.TEMPORARY_FAILURE, AUTH_ERROR.TEMPORARY_FAILURE);
}

async function refreshSessionAndBuildResponse(
 request: NextRequest,
 mode: SessionMode,
): Promise<NextResponse> {
 let refreshResponse: Response;
 try {
  refreshResponse = await refreshSessionViaInternalRoute(request);
 } catch {
  return createTemporaryFailureResponse();
 }

 if (!refreshResponse.ok) {
  const error = await resolveAuthError(refreshResponse);
  if (refreshResponse.status === 401
   || refreshResponse.status === 403
   || isTerminalAuthError(error)) {
   return unauthorizedResponse(true);
  }

  const transientStatus = resolveTransientStatus(refreshResponse.status);
  const errorCode = error ?? AUTH_ERROR.TEMPORARY_FAILURE;
  return buildProblemResponse(transientStatus, errorCode, errorCode);
 }

 let refreshPayload: { success?: boolean; user?: UserProfile; error?: string };
 try {
  refreshPayload = (await refreshResponse.clone().json()) as { success?: boolean; user?: UserProfile; error?: string };
 } catch {
  return unauthorizedResponse(true);
 }

 if (!refreshPayload.success) {
  return unauthorizedResponse(true);
 }

 if (mode === SESSION_MODE.FULL && !refreshPayload.user) {
  return unauthorizedResponse(true);
 }

 const response = createSuccessResponse(mode === SESSION_MODE.FULL ? refreshPayload.user : undefined);
 appendSetCookies(refreshResponse.headers, response);
 return response;
}

export async function GET(request: NextRequest): Promise<NextResponse> {
 const mode = resolveSessionMode(request);
 const accessToken = request.cookies.get(AUTH_COOKIE.ACCESS)?.value;
 const refreshToken = request.cookies.get(AUTH_COOKIE.REFRESH)?.value;
 if (!accessToken && !refreshToken) {
  return unauthorizedResponse();
 }

 if (mode === SESSION_MODE.LITE) {
  try {
   const validAccessToken = await getServerAccessTokenOrRefresh({ allowRefresh: false });
   if (validAccessToken) {
    return createSuccessResponse();
   }
  } catch {
   return createTemporaryFailureResponse();
  }

  if (!refreshToken) {
   return unauthorizedResponse(true);
  }

  return refreshSessionAndBuildResponse(request, mode);
 }

 let session: Awaited<ReturnType<typeof getServerSessionSnapshot>>;
 try {
  session = await getServerSessionSnapshot({ allowRefresh: false });
 } catch {
  return createTemporaryFailureResponse();
 }
 if (session.authenticated && session.user) {
  return createSuccessResponse(session.user);
 }

 if (!refreshToken) {
  return unauthorizedResponse(true);
 }

 return refreshSessionAndBuildResponse(request, mode);
}
