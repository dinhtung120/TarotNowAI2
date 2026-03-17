'use server';

/**
 * Server Actions xử lý History API (Lịch sử đọc bài)
 *
 * Tại sao cần Server Action?
 * - Phiên bản cũ gọi trực tiếp fetch() với URL hardcode `localhost:5000` (sai port).
 * - Backend thực sự chạy trên port 5037 (theo launchSettings.json).
 * - Dùng Server Action để tập trung cấu hình URL tại một nơi (biến môi trường)
 * và quản lý token xác thực an toàn phía server.
 */

import { cookies } from 'next/headers';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

/**
 * Hàm helper lấy Access Token từ cookie.
 * Tại sao không truyền token từ Zustand store?
 * - Server Actions chạy trên server, không có access vào Zustand (client-side store).
 * - Token đã được lưu vào cookie khi login → đọc từ cookie an toàn hơn.
 */
async function getAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 return cookieStore.get('accessToken')?.value;
}

/**
 * Lấy danh sách phiên đọc bài (Reading Sessions) — có phân trang và bộ lọc.
 *
 * @param page - Số trang hiện tại (1-indexed)
 * @param pageSize - Số lượng item mỗi trang
 * @param spreadType - Bộ lọc loại trải bài (optional)
 * @param date - Bộ lọc ngày (optional)
 */
export async function getHistorySessionsAction(
 page: number = 1, pageSize: number = 10,
 spreadType?: string,
 date?: string
) {
 try {
 const token = await getAccessToken();
 if (!token) {
 return { error: 'Chưa đăng nhập' };
 }

 let query = `page=${page}&pageSize=${pageSize}`;
 if (spreadType && spreadType !== 'all') query += `&spreadType=${encodeURIComponent(spreadType)}`;
 if (date) query += `&date=${encodeURIComponent(date)}`;

 const response = await fetch(
 `${API_BASE_URL}/history/sessions?${query}`,
 {
 headers: {
 'Content-Type': 'application/json',
 'Authorization': `Bearer ${token}`
 },
 }
 );

 if (!response.ok) {
 if (response.status === 401) {
 return { error: 'unauthorized' };
 }
 const result = await response.json().catch(() => ({}));
 return { error: result.message || 'Không thể tải lịch sử đọc bài' };
 }

 const data = await response.json();
 return { success: true, data };
 } catch {
 return { error: 'Lỗi kết nối mạng' };
 }
}

/**
 * Lấy chi tiết một phiên đọc bài cụ thể (bao gồm cards, AI interactions).
 *
 * @param sessionId - ID phiên đọc bài (UUID)
 * @returns Chi tiết session hoặc error
 */
export async function getHistoryDetailAction(sessionId: string) {
 try {
 const token = await getAccessToken();
 if (!token) {
 return { error: 'Chưa đăng nhập' };
 }

 const response = await fetch(
 `${API_BASE_URL}/history/sessions/${sessionId}`,
 {
 headers: {
 'Content-Type': 'application/json',
 'Authorization': `Bearer ${token}`
 },
 }
 );

 if (!response.ok) {
 if (response.status === 401) {
 return { error: 'unauthorized' };
 }
 if (response.status === 404) {
 return { error: 'Không tìm thấy phiên đọc bài này' };
 }
 const result = await response.json().catch(() => ({}));
 return { error: result.message || 'Không thể tải chi tiết phiên đọc bài' };
 }

 const data = await response.json();
 return { success: true, data };
 } catch {
 return { error: 'Lỗi kết nối mạng' };
 }
}
/**
 * Admin: Lấy toàn bộ lịch sử đọc bài của tất cả người dùng với bộ lọc.
 */
export async function getAllHistorySessionsAdminAction(params: {
 page: number;
 pageSize: number;
 username?: string;
 spreadType?: string;
 startDate?: string;
 endDate?: string;
}) {
 try {
 const token = await getAccessToken();
 if (!token) {
 return { error: 'Chưa đăng nhập' };
 }

 let query = `page=${params.page}&pageSize=${params.pageSize}`;
 if (params.username) query += `&username=${encodeURIComponent(params.username)}`;
 if (params.spreadType) query += `&spreadType=${encodeURIComponent(params.spreadType)}`;
 if (params.startDate) query += `&startDate=${encodeURIComponent(params.startDate)}`;
 if (params.endDate) query += `&endDate=${encodeURIComponent(params.endDate)}`;

 const response = await fetch(
 `${API_BASE_URL}/History/admin/all-sessions?${query}`,
 {
 headers: {
 'Content-Type': 'application/json',
 'Authorization': `Bearer ${token}`
 },
 }
 );

 if (!response.ok) {
 if (response.status === 401) return { error: 'unauthorized' };
 if (response.status === 403) return { error: 'forbidden' };
 const result = await response.json().catch(() => ({}));
 return { error: result.message || 'Không thể tải lịch sử Admin' };
 }

 const data = await response.json();
 // Safe Mapping for Admin History
 const safeData = {
 ...data,
 items: data.items || data.Items || [],
 totalCount: data.totalCount ?? data.TotalCount ?? 0,
 totalPages: data.totalPages ?? data.TotalPages ?? 0
 };
 return { success: true, data: safeData };
 } catch (err) {
 console.error("Admin History Action Error:", err);
 return { error: 'Lỗi kết nối mạng' };
 }
}
