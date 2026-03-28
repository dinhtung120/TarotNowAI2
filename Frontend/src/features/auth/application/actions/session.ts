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

/**
 * Xác định xem có nên dùng `Secure` flag cho cookie hay không.
 *
 * VẤN ĐỀ:
 * Trước đây dùng `process.env.NODE_ENV === 'production'` → nhưng khi chạy
 * `npm run build` + `npm start` trên localhost (HTTP, không phải HTTPS),
 * trình duyệt sẽ REJECT tất cả cookies có `Secure` flag vì kết nối không phải HTTPS.
 * → Hậu quả: accessToken và refreshToken cookies không được lưu → mất auth state.
 *
 * GIẢI PHÁP:
 * Dùng env var `NEXT_PUBLIC_BASE_URL` hoặc `NEXTAUTH_URL` để kiểm tra protocol.
 * Nếu URL bắt đầu bằng "https://", set Secure = true. Ngược lại, Secure = false.
 * Fallback: nếu không có env var, mặc định an toàn → Secure = false cho dev/local.
 */
function shouldUseSecureCookie(): boolean {
  const baseUrl = process.env.NEXT_PUBLIC_BASE_URL
    ?? process.env.NEXTAUTH_URL
    ?? '';
  return baseUrl.startsWith('https://');
}

/**
 * Parse refresh token từ set-cookie header(s).
 *
 * VẤN ĐỀ CŨ: `headers.get('set-cookie')` trong Node.js fetch trả về
 * TẤT CẢ set-cookie headers nối lại bằng ", " (comma + space).
 * Cách parse cũ chỉ tách theo ";" → chỉ lấy được cookie đầu tiên,
 * nếu cookie đầu tiên không phải refreshToken thì sẽ trả undefined.
 *
 * GIẢI PHÁP MỚI:
 * 1. Ưu tiên dùng `headers.getSetCookie()` (Node 20+) trả mảng riêng biệt.
 * 2. Nếu không có, tách combined string bằng regex pattern cookie boundary.
 * 3. Duyệt từng cookie string để tìm refreshToken.
 */
function parseRefreshTokenFromHeaders(headers: Headers): string | undefined {
  /* 
   * Bước 1: Lấy danh sách set-cookie headers dưới dạng mảng.
   * `getSetCookie()` có sẵn từ Node.js 20+ / Undici — trả mỗi phần tử là 1 cookie string riêng.
   * Nếu runtime chưa hỗ trợ, fallback sang headers.get('set-cookie') rồi tách thủ công.
   */
  let cookieStrings: string[];
  if (typeof headers.getSetCookie === 'function') {
    cookieStrings = headers.getSetCookie();
  } else {
    const raw = headers.get('set-cookie');
    if (!raw) return undefined;
    /* 
     * Tách combined set-cookie string.
     * Mỗi cookie pair bắt đầu bằng "name=value", sau đó có ";"-separated attributes.
     * Khi nhiều cookies bị nối bằng ", ", ta tách tại ", " NHƯNG chỉ khi phía sau dấu phẩy
     * là một cookie name mới (không phải giá trị attribute như "Mon, 01 Jan...").
     * Cách đơn giản nhất: split theo ", " rồi tìm cookie chứa refreshToken=
     */
    cookieStrings = raw.split(/,\s*(?=[a-zA-Z_]+=)/);
  }

  /* 
   * Bước 2: Tìm cookie string có name = REFRESH_TOKEN_COOKIE ("refreshToken").
   * Mỗi cookie string có dạng: "refreshToken=abc123; HttpOnly; Path=/; ..."
   * Ta chỉ cần phần value (phần trước dấu ";" đầu tiên, sau dấu "=" đầu tiên).
   */
  for (const cookieStr of cookieStrings) {
    const firstPair = cookieStr.trim().split(';')[0];
    const eqIndex = firstPair.indexOf('=');
    if (eqIndex === -1) continue;

    const name = firstPair.substring(0, eqIndex).trim();
    const value = firstPair.substring(eqIndex + 1).trim();

    if (name === REFRESH_TOKEN_COOKIE && value.length > 0) {
      return value;
    }
  }

  return undefined;
}

async function syncRefreshTokenCookie(headers: Headers): Promise<void> {
 const refreshToken = parseRefreshTokenFromHeaders(headers);
 if (!refreshToken) return;

 const cookieStore = await cookies();
 cookieStore.set({
  name: REFRESH_TOKEN_COOKIE,
  value: refreshToken,
  httpOnly: true,
  secure: shouldUseSecureCookie(),
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
  secure: shouldUseSecureCookie(),
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

  await syncRefreshTokenCookie(result.headers);

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

  await syncRefreshTokenCookie(result.headers);

  // Trả về accessToken mới để client cập nhật vào Zustand store
  return actionOk({ accessToken });
 } catch (error) {
  logger.error('[AuthAction] refreshAccessTokenAction', error);
  return actionFail(tApi('network_error'));
 }
}
