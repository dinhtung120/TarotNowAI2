'use server';
import { cookies } from 'next/headers';
import { getTranslations } from 'next-intl/server';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { AuthResponse } from '@/features/auth/domain/types';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
interface LoginActionPayload {
 user: AuthResponse['user'];
 accessToken: string;
 expiresIn: number;
}
const ACCESS_TOKEN_COOKIE = 'accessToken';
const REFRESH_TOKEN_COOKIE = 'refreshToken';
function shouldUseSecureCookie(): boolean {
  const baseUrl = process.env.NEXT_PUBLIC_BASE_URL ?? process.env.NEXTAUTH_URL ?? '';
  return baseUrl.startsWith('https://');
}
function parseRefreshTokenFromHeaders(headers: Headers): string | undefined {
  let cookieStrings: string[];
  if (typeof headers.getSetCookie === 'function') {
    cookieStrings = headers.getSetCookie();
  } else {
    const raw = headers.get('set-cookie');
    if (!raw) return undefined;
    cookieStrings = raw.split(/,\s*(?=[a-zA-Z_]+=)/);
  }
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
/**
 * Đồng bộ hóa refreshToken vào cookie.
 * @param headers Headers trả về từ API response chứa Set-Cookie
 */
async function syncRefreshTokenCookie(headers: Headers): Promise<void> {
  const refreshToken = parseRefreshTokenFromHeaders(headers);
  if (!refreshToken) return;

  const cookieStore = await cookies();
  // Thiết lập refreshToken với thời hạn 30 ngày để đảm bảo người dùng không bị logout bất ngờ
  cookieStore.set({
    name: REFRESH_TOKEN_COOKIE,
    value: refreshToken,
    httpOnly: true,
    secure: shouldUseSecureCookie(),
    sameSite: 'lax', // Sử dụng 'lax' thay vì 'strict' để ổn định hơn khi F5/Redirect
    path: '/',
    maxAge: 30 * 24 * 60 * 60, // 30 ngày (tính bằng giây)
  });
}

/**
 * Thiết lập accessToken vào cookie.
 * @param accessToken Chuỗi JWT token
 * @param expiresIn Thời gian hết hạn (giây) - mặc định 3600 nếu không có
 */
async function setAccessTokenCookie(accessToken: string, expiresIn?: number): Promise<void> {
  const cookieStore = await cookies();
  
  // Tính toán maxAge. Nếu expiresIn không hợp lệ, mặc định là 1 giờ.
  const maxAge = expiresIn && expiresIn > 0 ? expiresIn : 3600;

  cookieStore.set({
    name: ACCESS_TOKEN_COOKIE,
    value: accessToken,
    httpOnly: true,
    secure: shouldUseSecureCookie(),
    sameSite: 'lax', // Sử dụng 'lax' giúp cookie được gửi đi khi người dùng refresh trang (F5)
    path: '/',
    maxAge: maxAge, // Thiết lập Max-Age để cookie không bị mất khi đóng trình duyệt (nếu chưa hết hạn)
  });
}

/**
 * Đăng nhập người dùng và lưu trữ session.
 */
export async function loginAction(data: { emailOrUsername: string; password: string; rememberMe?: boolean }): Promise<ActionResult<LoginActionPayload>> {
  const tApi = await getTranslations('ApiErrors');
  try {
    const result = await serverHttpRequest<AuthResponse>('/auth/login', {
      method: 'POST',
      json: data,
      fallbackErrorMessage: tApi('unknown_error'),
    });

    // Luôn ưu tiên đồng bộ refreshToken trước
    await syncRefreshTokenCookie(result.headers);

    if (!result.ok) {
      return actionFail(result.error || tApi('unknown_error'));
    }

    const accessToken = result.data.accessToken;
    if (!accessToken) {
      return actionFail(tApi('unauthorized'));
    }

    // Lưu accessToken với maxAge từ backend giúp Middleware nhận diện đúng trạng thái login
    await setAccessTokenCookie(accessToken, result.data.expiresIn);

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

/**
 * Đăng xuất người dùng và xóa các thông tin session.
 */
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

    // Xóa cookie bất kể API logout có thành công hay không để đảm bảo client side sạch sẽ
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
 * Thực hiện làm mới accessToken sử dụng refreshToken.
 */
export async function refreshAccessTokenAction(): Promise<ActionResult<{ accessToken: string }>> {
  const tApi = await getTranslations('ApiErrors');
  try {
    const cookieStore = await cookies();
    const refreshToken = cookieStore.get(REFRESH_TOKEN_COOKIE)?.value;

    if (!refreshToken) {
      return actionFail(tApi('unauthorized'));
    }

    const result = await serverHttpRequest<AuthResponse>('/auth/refresh', {
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

    const accessToken = result.data.accessToken;
    if (!accessToken) {
      cookieStore.delete(ACCESS_TOKEN_COOKIE);
      return actionFail(tApi('unauthorized'));
    }

    // Cập nhật lại accessToken cookie và đồng bộ lại refreshToken mới (nếu có)
    await setAccessTokenCookie(accessToken, result.data.expiresIn);
    await syncRefreshTokenCookie(result.headers);

    return actionOk({ accessToken });
  } catch (error) {
    logger.error('[AuthAction] refreshAccessTokenAction', error);
    return actionFail(tApi('network_error'));
  }
}
