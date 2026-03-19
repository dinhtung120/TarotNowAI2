/*
 * ===================================================================
 * FILE: profileActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Hỗ trợ xem và cập nhật Thông Tin Cá Nhân (Profile) của User.
 *   (Bao gồm Đổi tên hiển thị, Avatar, và Ngày sinh).
 * ===================================================================
 */
'use server';

/**
 * Server Actions xử lý Profile API
 *
 * Tại sao cần Server Action thay vì gọi fetch trực tiếp từ Client Component?
 * - Bảo mật: Token xác thực được giữ an toàn phía server, không bị lộ ra client code.
 * - Nhất quán: Tất cả các trang khác (Auth, Wallet, Reading) đều dùng Server Actions.
 * Profile page là trang DUY NHẤT gọi trực tiếp fetch() → tạo bất nhất quán và lỗi URL.
 * - Đúng chuẩn Clean Architecture: Client Component → Server Action → Backend API.
 *
 * Trước khi sửa, Profile page gọi "/api/v1/profile" (URL tương đối → Next.js server),
 * nhưng API thực sự nằm ở http://localhost:5037/api/v1/profile (Backend .NET).
 */

import { cookies } from 'next/headers';
import { getTranslations } from 'next-intl/server';
import { API_BASE_URL } from '@/lib/api';

/**
 * Hàm helper lấy Access Token từ cookie.
 * Tại sao dùng cookie thay vì truyền token từ client?
 * - Vì loginAction() đã lưu accessToken vào cookie khi đăng nhập thành công.
 * - Server Actions chạy trên server nên có thể đọc cookie trực tiếp.
 */
async function getAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 return cookieStore.get('accessToken')?.value;
}

/**
 * Lấy thông tin hồ sơ người dùng hiện tại.
 * Gọi [GET] /api/v1/profile với Bearer token.
 */
export async function getProfileAction() {
 const tApi = await getTranslations('ApiErrors');

 try {
 const token = await getAccessToken();
 if (!token) {
 return { error: tApi('unauthorized') };
 }

 const response = await fetch(`${API_BASE_URL}/profile`, {
 headers: {
 'Content-Type': 'application/json',
 'Authorization': `Bearer ${token}`
 },
 });

 if (!response.ok) {
 if (response.status === 401) {
 return { error: tApi('unauthorized') };
 }
 const result = await response.json().catch(() => ({}));
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 const data = await response.json();
 return { success: true, data };
 } catch {
 return { error: tApi('network_error') };
 }
}

/**
 * Cập nhật thông tin hồ sơ người dùng.
 * Gọi [PATCH] /api/v1/profile với dữ liệu form đã validate từ Zod.
 *
 * @param profileData - Đối tượng chứa displayName, avatarUrl, dateOfBirth
 */
export async function updateProfileAction(profileData: {
 displayName: string;
 avatarUrl: string | null;
 dateOfBirth: string;
}) {
 const tApi = await getTranslations('ApiErrors');

 try {
 const token = await getAccessToken();
 if (!token) {
 return { error: tApi('unauthorized') };
 }

 const response = await fetch(`${API_BASE_URL}/profile`, {
 method: 'PATCH',
 headers: {
 'Content-Type': 'application/json',
 'Authorization': `Bearer ${token}`
 },
 body: JSON.stringify(profileData),
 });

 if (!response.ok) {
 if (response.status === 401) {
 return { error: tApi('unauthorized') };
 }
 const result = await response.json().catch(() => ({}));
 return { error: result.message || result.detail || tApi('unknown_error') };
 }

 return { success: true };
 } catch {
 return { error: tApi('network_error') };
 }
}
