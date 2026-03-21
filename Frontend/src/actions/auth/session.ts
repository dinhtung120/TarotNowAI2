'use server';

import { cookies } from 'next/headers';
import { getTranslations } from 'next-intl/server';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { AuthResponse } from '@/types/auth';
import {
 actionFail,
 actionOk,
 type ActionResult,
} from '@/shared/domain/actionResult';

type RefreshAccessTokenResult =
 | {
  success: true;
  accessToken?: string;
 }
 | {
  success: false;
  error: string;
 };

function parseRefreshTokenFromSetCookie(setCookieHeader: string | null): string | undefined {
 if (!setCookieHeader) return undefined;
 const firstPair = setCookieHeader.split(';')[0];
 const [name, ...valueParts] = firstPair.split('=');
 if (name !== 'refreshToken' || valueParts.length === 0) return undefined;
 return valueParts.join('=');
}

async function syncRefreshTokenCookie(setCookieHeader: string | null): Promise<void> {
 const refreshToken = parseRefreshTokenFromSetCookie(setCookieHeader);
 if (!refreshToken) return;

 const cookieStore = await cookies();
 cookieStore.set({
  name: 'refreshToken',
  value: refreshToken,
  httpOnly: true,
  secure: process.env.NODE_ENV === 'production',
  sameSite: 'strict',
  path: '/',
 });
}

export async function loginAction(data: { emailOrUsername: string; password: string; rememberMe?: boolean }): Promise<ActionResult<AuthResponse>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<AuthResponse>('/auth/login', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  await syncRefreshTokenCookie(result.headers.get('set-cookie'));

  if (!result.ok) {
   return actionFail(result.error || tApi('unknown_error'));
  }

  const accessToken = result.data.accessToken;
  if (accessToken) {
   const cookieStore = await cookies();
   cookieStore.set({
    name: 'accessToken',
    value: accessToken,
    httpOnly: false,
    secure: process.env.NODE_ENV === 'production',
    sameSite: 'lax',
    path: '/',
   });
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[AuthAction] loginAction', error);
  return actionFail(tApi('network_error'));
 }
}

export async function logoutAction(): Promise<ActionResult<undefined>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const cookieStore = await cookies();
  const refreshToken = cookieStore.get('refreshToken')?.value;

  const result = await serverHttpRequest<Record<string, unknown>>('/auth/logout', {
   method: 'POST',
   headers: {
    Cookie: `refreshToken=${refreshToken ?? ''}`,
   },
   fallbackErrorMessage: tApi('unknown_error'),
  });

  cookieStore.delete('refreshToken');
  cookieStore.delete('accessToken');

  if (!result.ok) {
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk();
 } catch (error) {
  logger.error('[AuthAction] logoutAction', error);
  return actionFail(tApi('network_error'));
 }
}

export async function refreshAccessTokenAction(): Promise<RefreshAccessTokenResult> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const cookieStore = await cookies();
  const refreshToken = cookieStore.get('refreshToken')?.value;

  if (!refreshToken) {
   return actionFail(tApi('unauthorized'));
  }

  const result = await serverHttpRequest<Record<string, unknown>>('/auth/refresh', {
   method: 'POST',
   headers: {
    Cookie: `refreshToken=${refreshToken}`,
   },
   fallbackErrorMessage: tApi('unauthorized'),
  });

  if (!result.ok) {
   cookieStore.delete('accessToken');
   cookieStore.delete('refreshToken');
   return actionFail(result.error || tApi('unauthorized'));
  }

  const accessToken = typeof result.data.accessToken === 'string' ? result.data.accessToken : undefined;
  if (accessToken) {
   cookieStore.set({
    name: 'accessToken',
    value: accessToken,
    httpOnly: false,
    secure: process.env.NODE_ENV === 'production',
    sameSite: 'lax',
    path: '/',
   });
  }

  await syncRefreshTokenCookie(result.headers.get('set-cookie'));

  return { success: true, accessToken };
 } catch (error) {
  logger.error('[AuthAction] refreshAccessTokenAction', error);
  return actionFail(tApi('network_error'));
 }
}
