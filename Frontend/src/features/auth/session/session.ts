'use client';

import type { UserProfile } from '@/features/auth/session/types';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import { AUTH_ERROR } from '@/shared/models/authErrors';
import { AUTH_HEADER } from '@/shared/gateways/authConstants';
import { getOrCreateDeviceId } from '@/shared/gateways/deviceId';

interface LoginActionPayload {
 user: UserProfile;
 expiresInSeconds: number;
}

interface RefreshActionPayload {
 user: UserProfile;
 expiresInSeconds: number;
}

interface AuthRouteSuccessPayload {
 success: true;
 user: UserProfile;
 expiresInSeconds: number;
}

interface AuthRouteErrorPayload {
 success: false;
 error?: string;
}

type AuthRoutePayload = AuthRouteSuccessPayload | AuthRouteErrorPayload;

function resolveAuthError(payload: AuthRoutePayload | undefined, fallback: string): string {
 if (payload && payload.success === false && payload.error && payload.error.trim().length > 0) {
  return payload.error;
 }

 return fallback;
}

function resolveStatusFallback(status: number): string {
 if (status === 429) {
  return AUTH_ERROR.RATE_LIMITED;
 }

 if (status === 401) {
  return AUTH_ERROR.UNAUTHORIZED;
 }

 if (status >= 500) {
  return AUTH_ERROR.TEMPORARY_FAILURE;
 }

 return AUTH_ERROR.UNAUTHORIZED;
}

async function parseAuthPayload(response: Response): Promise<AuthRoutePayload | undefined> {
 try {
  return (await response.json()) as AuthRoutePayload;
 } catch {
  return undefined;
 }
}

export async function loginAction(data: {
 emailOrUsername: string;
 password: string;
 rememberMe?: boolean;
}): Promise<ActionResult<LoginActionPayload>> {
 try {
  const response = await fetch('/api/auth/login', {
   method: 'POST',
   credentials: 'include',
   cache: 'no-store',
   headers: {
    'Content-Type': 'application/json',
    [AUTH_HEADER.DEVICE_ID]: getOrCreateDeviceId(),
   },
   body: JSON.stringify(data),
  });

  const payload = await parseAuthPayload(response);
  if (!response.ok || !payload || payload.success === false) {
   return actionFail(resolveAuthError(payload, resolveStatusFallback(response.status)));
  }

  return actionOk({
   user: payload.user,
   expiresInSeconds: payload.expiresInSeconds,
  });
 } catch {
  return actionFail(AUTH_ERROR.TEMPORARY_FAILURE);
 }
}

export async function logoutAction(): Promise<ActionResult<undefined>> {
 try {
  const response = await fetch('/api/auth/logout', {
   method: 'POST',
   credentials: 'include',
   cache: 'no-store',
  });

  if (!response.ok) {
   return actionFail(resolveStatusFallback(response.status));
  }

  return actionOk();
 } catch {
  return actionFail(AUTH_ERROR.TEMPORARY_FAILURE);
 }
}

let refreshPromise: Promise<ActionResult<RefreshActionPayload>> | null = null;

export async function refreshAccessTokenAction(): Promise<ActionResult<RefreshActionPayload>> {
 if (refreshPromise) {
  return refreshPromise;
 }

 refreshPromise = (async () => {
  try {
   const response = await fetch('/api/auth/refresh', {
    method: 'POST',
    credentials: 'include',
    cache: 'no-store',
    headers: {
     [AUTH_HEADER.DEVICE_ID]: getOrCreateDeviceId(),
    },
   });

   const payload = await parseAuthPayload(response);
   if (!response.ok || !payload || payload.success === false) {
    return actionFail(resolveAuthError(payload, resolveStatusFallback(response.status)));
   }

  return actionOk({
   user: payload.user,
   expiresInSeconds: payload.expiresInSeconds,
  });
  } catch {
   return actionFail(AUTH_ERROR.TEMPORARY_FAILURE);
  } finally {
   refreshPromise = null;
  }
 })();

 return refreshPromise;
}
