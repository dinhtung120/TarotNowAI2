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
 * - Nhất quán: toàn bộ luồng frontend gọi backend qua cùng một lớp Server Actions.
 * Phiên bản cũ từng gọi trực tiếp fetch() trong page profile gây bất nhất và lỗi URL.
 * - Đúng chuẩn Clean Architecture: Client Component → Server Action → Backend API.
 *
 * Trước khi sửa, Profile page gọi "/api/v1/profile" (URL tương đối → Next.js server),
 * nhưng API thực sự nằm ở http://localhost:5037/api/v1/profile (Backend .NET).
 */

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

/**
 * Hàm helper lấy Access Token từ cookie.
 * Tại sao dùng cookie thay vì truyền token từ client?
 * - Vì loginAction() đã lưu accessToken vào cookie khi đăng nhập thành công.
 * - Server Actions chạy trên server nên có thể đọc cookie trực tiếp.
 */
const getAccessToken = getServerAccessToken;

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

 const result = await serverHttpRequest<unknown>('/profile', {
 method: 'GET',
 token,
 fallbackErrorMessage: tApi('unknown_error'),
 });

 if (!result.ok) {
 if (result.status === 401) {
 return { error: tApi('unauthorized') };
 }
 logger.error('ProfileAction.getProfileAction', result.error, { status: result.status });
 return { error: result.error || tApi('unknown_error') };
 }

 return { success: true, data: result.data };
 } catch (error) {
 logger.error('ProfileAction.getProfileAction', error);
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

 const result = await serverHttpRequest<unknown>('/profile', {
 method: 'PATCH',
 token,
 json: profileData,
 fallbackErrorMessage: tApi('unknown_error'),
 });

 if (!result.ok) {
 if (result.status === 401) {
 return { error: tApi('unauthorized') };
 }
 logger.error('ProfileAction.updateProfileAction', result.error, { status: result.status });
 return { error: result.error || tApi('unknown_error') };
 }

 return { success: true };
 } catch (error) {
 logger.error('ProfileAction.updateProfileAction', error);
 return { error: tApi('network_error') };
 }
}
