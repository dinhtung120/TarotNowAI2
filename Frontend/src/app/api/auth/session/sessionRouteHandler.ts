import { NextRequest, NextResponse } from 'next/server';
import type { UserProfile } from '@/features/auth/session/types';
import { AUTH_ERROR, isTerminalAuthError } from '@/shared/domain/authErrors';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import {
 getServerAccessTokenOrRefresh,
 getServerSessionSnapshot,
} from '@/shared/infrastructure/auth/serverAuth';
import {
 appendSetCookieHeaders,
 buildProblemResponse,
 unauthorizedResponse,
} from '@/app/api/auth/_shared';
import { executeRefreshRoute } from '@/app/api/auth/refresh/refreshRouteHandler';

interface SessionResponsePayload {
 success: boolean;
 authenticated: boolean;
 user?: UserProfile;
 error?: string;
}

export const SESSION_MODE = {
 FULL: 'full',
 LITE: 'lite',
} as const;

export type SessionMode = (typeof SESSION_MODE)[keyof typeof SESSION_MODE];

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

function resolveSessionMode(
 request: NextRequest,
 forcedMode?: SessionMode,
): SessionMode {
 if (forcedMode) {
  return forcedMode;
 }

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
 let refreshResponse: NextResponse;
 try {
  refreshResponse = await executeRefreshRoute(request);
 } catch {
  return createTemporaryFailureResponse();
 }

 if (!refreshResponse.ok) {
  const error = await resolveAuthError(refreshResponse);
  if (refreshResponse.status === 401
   || refreshResponse.status === 403
   || isTerminalAuthError(error)) {
   return refreshResponse;
  }

  return refreshResponse;
 }

 let refreshPayload: { success?: boolean; user?: UserProfile; error?: string };
 try {
  refreshPayload = (await refreshResponse.clone().json()) as {
   success?: boolean;
   user?: UserProfile;
   error?: string;
  };
 } catch {
  return unauthorizedResponse(true, request);
 }

 if (!refreshPayload.success) {
  return unauthorizedResponse(true, request);
 }

 if (mode === SESSION_MODE.FULL && !refreshPayload.user) {
  return unauthorizedResponse(true, request);
 }

 const response = createSuccessResponse(
  mode === SESSION_MODE.FULL ? refreshPayload.user : undefined,
 );
 appendSetCookieHeaders(refreshResponse.headers, response);
 return response;
}

export async function getSessionRouteResponse(
 request: NextRequest,
 forcedMode?: SessionMode,
): Promise<NextResponse> {
 const mode = resolveSessionMode(request, forcedMode);
 const accessToken = request.cookies.get(AUTH_COOKIE.ACCESS)?.value;
 const refreshToken = request.cookies.get(AUTH_COOKIE.REFRESH)?.value;
 if (!accessToken && !refreshToken) {
  return unauthorizedResponse(false, request);
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
   return unauthorizedResponse(true, request);
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
  return unauthorizedResponse(true, request);
 }

 return refreshSessionAndBuildResponse(request, mode);
}
