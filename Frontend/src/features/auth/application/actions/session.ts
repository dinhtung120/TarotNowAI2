'use server';

import { cookies } from 'next/headers';
import { getTranslations } from 'next-intl/server';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { AuthResponse } from '@/features/auth/domain/types';
import {
 actionFail,
 actionOk,
 type ActionResult,
} from '@/shared/domain/actionResult';

interface LoginActionPayload {
 user: AuthResponse['user'];
 accessToken: string;
 expiresIn: number;
}

const ACCESS_TOKEN_COOKIE = 'accessToken';
const REFRESH_TOKEN_COOKIE = 'refreshToken';

function parseRefreshTokenFromSetCookie(setCookieHeader: string | null): string | undefined {
 if (!setCookieHeader) return undefined;
 const firstPair = setCookieHeader.split(';')[0];
 const [name, ...valueParts] = firstPair.split('=');
 if (name !== REFRESH_TOKEN_COOKIE || valueParts.length === 0) return undefined;
 return valueParts.join('=');
}

async function syncRefreshTokenCookie(setCookieHeader: string | null): Promise<void> {
 const refreshToken = parseRefreshTokenFromSetCookie(setCookieHeader);
 if (!refreshToken) return;

 const cookieStore = await cookies();
 cookieStore.set({
  name: REFRESH_TOKEN_COOKIE,
  value: refreshToken,
  httpOnly: true,
  secure: process.env.NODE_ENV === 'production',
  sameSite: 'strict',
  path: '/',
 });
}

async function setAccessTokenCookie(accessToken: string): Promise<void> {
 const cookieStore = await cookies();
 cookieStore.set({
  name: ACCESS_TOKEN_COOKIE,
  value: accessToken,
  httpOnly: true,
  secure: process.env.NODE_ENV === 'production',
  sameSite: 'strict',
  path: '/',
 });
}

export async function loginAction(data: { emailOrUsername: string; password: string; rememberMe?: boolean }): Promise<ActionResult<LoginActionPayload>> {
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
  if (!accessToken) {
   return actionFail(tApi('unauthorized'));
  }

  await setAccessTokenCookie(accessToken);

  return actionOk({
   user: result.data.user,
   accessToken,
   expiresIn: result.data.expiresIn,
  });
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
   Cookie: `${REFRESH_TOKEN_COOKIE}=${refreshToken ?? ''}`,
  },
  fallbackErrorMessage: tApi('unknown_error'),
 });

  cookieStore.delete(REFRESH_TOKEN_COOKIE);
  cookieStore.delete(ACCESS_TOKEN_COOKIE);

  if (!result.ok) {
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk();
 } catch (error) {
  logger.error('[AuthAction] logoutAction', error);
  return actionFail(tApi('network_error'));
 }
}

/**
 * Refresh access token và trả về token mới cho client.
 *
 * QUAN TRỌNG: Trước đây hàm này chỉ cập nhật cookie trên server mà KHÔNG trả
 * token mới về client. Điều này gây ra lỗi 401 cho SignalR vì:
 *   1. Zustand authStore.token vẫn giữ token cũ (hết hạn)
 *   2. SignalR accessTokenFactory đọc token cũ từ store → gửi expired token → 401
 *
 * Giải pháp: Trả về { accessToken: string } để client cập nhật Zustand store,
 * đảm bảo SignalR luôn có token mới nhất khi negotiate/reconnect.
 */
export async function refreshAccessTokenAction(): Promise<ActionResult<{ accessToken: string }>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const cookieStore = await cookies();
  const refreshToken = cookieStore.get(REFRESH_TOKEN_COOKIE)?.value;

  if (!refreshToken) {
   return actionFail(tApi('unauthorized'));
  }

  const result = await serverHttpRequest<Record<string, unknown>>('/auth/refresh', {
   method: 'POST',
   headers: {
    Cookie: `${REFRESH_TOKEN_COOKIE}=${refreshToken}`,
   },
   fallbackErrorMessage: tApi('unauthorized'),
  });

  if (!result.ok) {
   cookieStore.delete(ACCESS_TOKEN_COOKIE);
   cookieStore.delete(REFRESH_TOKEN_COOKIE);
   return actionFail(result.error || tApi('unauthorized'));
  }

  const accessToken = typeof result.data.accessToken === 'string' ? result.data.accessToken : undefined;
  if (!accessToken) {
   cookieStore.delete(ACCESS_TOKEN_COOKIE);
   return actionFail(tApi('unauthorized'));
  }
  await setAccessTokenCookie(accessToken);

  await syncRefreshTokenCookie(result.headers.get('set-cookie'));

  // Trả về accessToken mới để client cập nhật vào Zustand store
  return actionOk({ accessToken });
 } catch (error) {
  logger.error('[AuthAction] refreshAccessTokenAction', error);
  return actionFail(tApi('network_error'));
 }
}
