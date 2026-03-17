'use server';

import { cookies } from 'next/headers';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

/**
 * Các Server Actions để thực hiện API call tới Backend Auth.
 * Sử dụng 'use server' để ẩn đi logic thao tác API / che dấu các endpoint và bảo lưu cookies dễ hơn.
 */

export async function loginAction(data: Record<string, unknown>) {
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
 return { error: result.message || 'Login failed' };
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
 return { error: 'Network error occurred' };
 }
}

export async function registerAction(data: Record<string, unknown>) {
 try {
 const response = await fetch(`${API_BASE_URL}/auth/register`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || 'Registration failed' };
 }

 return { success: true, data: result };
 } catch {
 return { error: 'Network error occurred' };
 }
}

export async function logoutAction() {
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
 return { error: result.message || 'Logout failed' };
 }

 return { success: true };
 } catch {
 return { error: 'Network error occurred' };
 }
}

export async function verifyEmailAction(data: { email: string; otpCode: string }) {
 try {
 const response = await fetch(`${API_BASE_URL}/auth/verify-email`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || 'Verification failed' };
 }

 return { success: true, data: result };
 } catch {
 return { error: 'Network error occurred' };
 }
}

export async function forgotPasswordAction(data: { email: string }) {
 try {
 const response = await fetch(`${API_BASE_URL}/auth/forgot-password`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || 'Failed to send reset email' };
 }

 return { success: true, data: result };
 } catch {
 return { error: 'Network error occurred' };
 }
}

export async function resetPasswordAction(data: Record<string, unknown>) {
 try {
 const response = await fetch(`${API_BASE_URL}/auth/reset-password`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify(data),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || 'Password reset failed' };
 }

 return { success: true, data: result };
 } catch {
 return { error: 'Network error occurred' };
 }
}

/**
 * [Phase 4] Tự động làm mới Access Token bằng Refresh Token (HttpOnly Cookie).
 * Hàm này thường được gọi bởi Middleware hoặc Client khi nhận được 401.
 */
export async function refreshAccessTokenAction() {
 try {
 const cookieStore = await cookies();
 const refreshToken = cookieStore.get('refreshToken')?.value;

 if (!refreshToken) {
 return { error: 'No refresh token available' };
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
 return { error: result.message || 'Refresh token failed' };
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
 return { error: 'Network error occurred' };
 }
}

/**
 * [Phase 4] Gửi lại mã OTP xác thực email.
 */
export async function resendVerificationEmailAction(email: string) {
 try {
 const response = await fetch(`${API_BASE_URL}/auth/send-verification-email`, {
 method: 'POST',
 headers: { 'Content-Type': 'application/json' },
 body: JSON.stringify({ email }),
 });

 const result = await response.json();

 if (!response.ok) {
 return { error: result.message || 'Failed to resend email' };
 }

 return { success: true, message: result.message || 'OTP sent successfully' };
 } catch {
 return { error: 'Network error occurred' };
 }
}
