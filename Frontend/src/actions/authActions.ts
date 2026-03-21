/*
 * ===================================================================
 * FILE: authActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Quản lý toàn bộ quy trình Xác Thực & Cấp Quyền (Authentication/Authorization)
 *   bao gồm Login, Register, Logout, Reset Password, OTP.
 *
 * CƠ CHẾ HOẠT ĐỘNG:
 *   - Nhận yêu cầu từ UI Components (Client), gửi request an toàn qua Backend.
 *   - Bóc xuất Header 'set-cookie' từ Backend trả về, đưa thẻ 'refreshToken'
 *     thành HttpOnly Cookie trong hệ thống Router Next.js (bảo vệ khỏi Javascript trộm cắp).
 *   - Tự động làm mới phiên đăng nhập (Refresh Token) êm ru không gián đoạn trải nghiệm.
 * ===================================================================
 */
'use server';

import { cookies } from 'next/headers';
import { getTranslations } from 'next-intl/server';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { AuthResponse } from '@/types/auth';

/**
 * Các Server Actions để thực hiện API call tới Backend Auth.
 * Sử dụng 'use server' để ẩn đi logic thao tác API / che dấu các endpoint và bảo lưu cookies dễ hơn.
 */

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

function readAccessToken(payload: Record<string, unknown>): string | undefined {
 const token = payload.accessToken;
 return typeof token === 'string' ? token : undefined;
}

export async function loginAction(data: Record<string, unknown>) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<AuthResponse>('/auth/login', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  // Chuyển set-cookie (refreshToken HttpOnly) từ BE sang cookie store của Next.js
  await syncRefreshTokenCookie(result.headers.get('set-cookie'));

  if (!result.ok) {
   return { error: result.error || tApi('unknown_error') };
  }

  // Lưu access token để các Server Actions khác dùng được
  const accessToken = result.data.accessToken;
  if (accessToken) {
   const cookieStore = await cookies();
   cookieStore.set({
    name: 'accessToken',
    value: accessToken,
    httpOnly: false, // Để client đọc được nếu cần, hoặc quản lý trong Zustand
    secure: process.env.NODE_ENV === 'production',
    sameSite: 'lax',
    path: '/',
   });
  }

  return { success: true, data: result.data };
 } catch (error) {
  logger.error('[AuthAction] loginAction', error);
  return { error: tApi('network_error') };
 }
}

export async function registerAction(data: Record<string, unknown>) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/register', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true, data: result.data };
 } catch (error) {
  logger.error('[AuthAction] registerAction', error);
  return { error: tApi('network_error') };
 }
}

export async function logoutAction() {
 const tApi = await getTranslations('ApiErrors');

 try {
  const cookieStore = await cookies();
  const refreshToken = cookieStore.get('refreshToken')?.value;

  // Kể cả không có cookie ở client, gửi request để BE xóa Refresh Token DB
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/logout', {
   method: 'POST',
   headers: {
    Cookie: `refreshToken=${refreshToken ?? ''}`,
   },
   fallbackErrorMessage: tApi('unknown_error'),
  });

  // Clear local cookies
  cookieStore.delete('refreshToken');
  cookieStore.delete('accessToken');

  if (!result.ok) {
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true };
 } catch (error) {
  logger.error('[AuthAction] logoutAction', error);
  return { error: tApi('network_error') };
 }
}

export async function verifyEmailAction(data: { email: string; otpCode: string }) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/verify-email', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true, data: result.data };
 } catch (error) {
  logger.error('[AuthAction] verifyEmailAction', error, { email: data.email });
  return { error: tApi('network_error') };
 }
}

export async function forgotPasswordAction(data: { email: string }) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/forgot-password', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true, data: result.data };
 } catch (error) {
  logger.error('[AuthAction] forgotPasswordAction', error, { email: data.email });
  return { error: tApi('network_error') };
 }
}

export async function resetPasswordAction(data: Record<string, unknown>) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/reset-password', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true, data: result.data };
 } catch (error) {
  logger.error('[AuthAction] resetPasswordAction', error);
  return { error: tApi('network_error') };
 }
}

/**
 * [Phase 4] Tự động làm mới Access Token bằng Refresh Token (HttpOnly Cookie).
 * Hàm này thường được gọi bởi Middleware hoặc Client khi nhận được 401.
 */
export async function refreshAccessTokenAction() {
 const tApi = await getTranslations('ApiErrors');

 try {
  const cookieStore = await cookies();
  const refreshToken = cookieStore.get('refreshToken')?.value;

  if (!refreshToken) {
   return { error: tApi('unauthorized') };
  }

  const result = await serverHttpRequest<Record<string, unknown>>('/auth/refresh', {
   method: 'POST',
   headers: {
    Cookie: `refreshToken=${refreshToken}`,
   },
   fallbackErrorMessage: tApi('unauthorized'),
  });

  if (!result.ok) {
   // Nếu refresh thất bại (hết hạn, revoke), ta nên xóa cookies và yêu cầu login lại
   cookieStore.delete('accessToken');
   cookieStore.delete('refreshToken');
   return { error: result.error || tApi('unauthorized') };
  }

  const accessToken = readAccessToken(result.data);

  // Cập nhật Access Token mới vào Cookie
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

  // Cập nhật Refresh Token mới (nếu BE có cơ chế rotation)
  await syncRefreshTokenCookie(result.headers.get('set-cookie'));

  return { success: true, accessToken };
 } catch (error) {
  logger.error('[AuthAction] refreshAccessTokenAction', error);
  return { error: tApi('network_error') };
 }
}

/**
 * [Phase 4] Gửi lại mã OTP xác thực email.
 */
export async function resendVerificationEmailAction(email: string) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<unknown>('/auth/send-verification-email', {
   method: 'POST',
   json: { email },
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true };
 } catch (error) {
  logger.error('[AuthAction] resendVerificationEmailAction', error, { email });
  return { error: tApi('network_error') };
 }
}
