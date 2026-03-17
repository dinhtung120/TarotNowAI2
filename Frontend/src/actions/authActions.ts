'use server';

import { cookies } from 'next/headers';
import { API_BASE_URL } from '@/lib/api';
import { getTranslations } from 'next-intl/server';

/**
 * Các Server Actions để thực hiện API call tới Backend Auth.
 * Sử dụng 'use server' để ẩn đi logic thao tác API / che dấu các endpoint và bảo lưu cookies dễ hơn.
 */

export async function loginAction(data: Record<string, unknown>) {
 const tApi = await getTranslations('ApiErrors');

 try {
 const response = await fetch(`${API_BASE_URL}/auth/login`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 // Chuyển set-cookie (refreshToken HttpOnly) từ BE sang Client
 const setCookieHeader = response.headers.get('set-cookie');
 if (setCookieHeader) {
 const cookieStore = await cookies();
 // Lấy cookie refreshToken và ghi đè vào hệ thống Cookie của trình duyệt (via Nextjs)
 const parts = setCookieHeader.split(';')[0].split('=');
 if (parts.length === 2 && parts[0] === 'refreshToken') {
 cookieStore.set({
 name: 'refreshToken',
 value: parts[1],
 httpOnly: true,
 secure: process.env.NODE_ENV === 'production',
 sameSite: 'strict',
 path: '/'
 });
 }
 }

 if (!response.ok) {
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 // Cần lưu ý access token cũng phải được set cookie để các Server Action (Collection, Reading) dùng được
 if (result.accessToken) {
 const cookieStore = await cookies();
 cookieStore.set({
 name: 'accessToken',
 value: result.accessToken,
 httpOnly: false, // Để client đọc được nếu cần, hoặc quản lý trong Zustand
 secure: process.env.NODE_ENV === 'production',
 sameSite: 'lax',
 path: '/'
 });
 }

 return { success: true, data: result };
 } catch {
 return { error: tApi('network_error') };
 }
}

export async function registerAction(data: Record<string, unknown>) {
 const tApi = await getTranslations('ApiErrors');

 try {
 const response = await fetch(`${API_BASE_URL}/auth/register`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 return { success: true, data: result };
 } catch {
 return { error: tApi('network_error') };
 }
}

export async function logoutAction() {
 const tApi = await getTranslations('ApiErrors');

 try {
 const cookieStore = await cookies();
 const refreshToken = cookieStore.get('refreshToken')?.value;

 // Kể cả không có cookie ở client, gửi request để BE xóa Refresh Token DB
 const response = await fetch(`${API_BASE_URL}/auth/logout`, {
 method: 'POST',
 headers: {
 'Content-Type': 'application/json',
 'Cookie': `refreshToken=${refreshToken}`
 },
 });

 // Clear local cookies
 cookieStore.delete('refreshToken');
 cookieStore.delete('accessToken');

 if (!response.ok) {
 const result = await response.json();
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 return { success: true };
 } catch {
 return { error: tApi('network_error') };
 }
}

export async function verifyEmailAction(data: { email: string; otpCode: string }) {
 const tApi = await getTranslations('ApiErrors');

 try {
 const response = await fetch(`${API_BASE_URL}/auth/verify-email`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 return { success: true, data: result };
 } catch {
 return { error: tApi('network_error') };
 }
}

export async function forgotPasswordAction(data: { email: string }) {
 const tApi = await getTranslations('ApiErrors');

 try {
 const response = await fetch(`${API_BASE_URL}/auth/forgot-password`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 return { success: true, data: result };
 } catch {
 return { error: tApi('network_error') };
 }
}

export async function resetPasswordAction(data: Record<string, unknown>) {
 const tApi = await getTranslations('ApiErrors');

 try {
 const response = await fetch(`${API_BASE_URL}/auth/reset-password`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 return { success: true, data: result };
 } catch {
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

 const response = await fetch(`${API_BASE_URL}/auth/refresh`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json',
 'Cookie': `refreshToken=${refreshToken}` },
 });

 const result = await response.json();

 if (!response.ok) {
 // Nếu refresh thất bại (hết hạn, revoke), ta nên xóa cookies và yêu cầu login lại
 cookieStore.delete('accessToken');
 cookieStore.delete('refreshToken');
 return { error: result.message || result.detail || tApi('unauthorized') };
 }

 // Cập nhật Access Token mới vào Cookie
 if (result.accessToken) {
 cookieStore.set({
 name: 'accessToken',
 value: result.accessToken,
 httpOnly: false,
 secure: process.env.NODE_ENV === 'production',
 sameSite: 'lax',
 path: '/'
 });
 }

 // Cập nhật Refresh Token mới (nếu BE có cơ chế rotation)
 const setCookieHeader = response.headers.get('set-cookie');
 if (setCookieHeader) {
 const parts = setCookieHeader.split(';')[0].split('=');
 if (parts.length === 2 && parts[0] === 'refreshToken') {
 cookieStore.set({
 name: 'refreshToken',
 value: parts[1],
 httpOnly: true,
 secure: process.env.NODE_ENV === 'production',
 sameSite: 'strict',
 path: '/'
 });
 }
 }

 return { success: true, accessToken: result.accessToken };
 } catch {
 return { error: tApi('network_error') };
 }
}

/**
 * [Phase 4] Gửi lại mã OTP xác thực email.
 */
export async function resendVerificationEmailAction(email: string) {
 const tApi = await getTranslations('ApiErrors');

 try {
 const response = await fetch(`${API_BASE_URL}/auth/send-verification-email`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify({ email }),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 return { success: true };
 } catch {
 return { error: tApi('network_error') };
 }
}
