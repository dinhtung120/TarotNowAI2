/*
 * ===================================================================
 * FILE: historyActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Kéo Lịch sử Trải bài (Reading History) của User và lấy Chi tiết phiên đọc.
 *   Cũng như hỗ trợ Quản trị viên (Admin) xem xét toàn bộ sổ truy lục lịch sử.
 * ===================================================================
 */
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

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

interface AdminHistorySessionItem {
 id: string;
 userId: string;
 username: string;
 spreadType: string;
 question: string | null;
 isCompleted: boolean;
 createdAt: string;
}

interface AdminHistoryPaginatedResponse {
 page: number;
 pageSize: number;
 totalPages: number;
 totalCount: number;
 items: AdminHistorySessionItem[];
}

/**
 * Hàm helper lấy Access Token từ cookie.
 * Tại sao không truyền token từ Zustand store?
 * - Server Actions chạy trên server, không có access vào Zustand (client-side store).
 * - Token đã được lưu vào cookie khi login → đọc từ cookie an toàn hơn.
 */
const getAccessToken = getServerAccessToken;

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
 const tApi = await getTranslations('ApiErrors');

 try {
 const token = await getAccessToken();
 if (!token) {
 return { error: 'unauthorized' };
 }

 let query = `page=${page}&pageSize=${pageSize}`;
 if (spreadType && spreadType !== 'all') query += `&spreadType=${encodeURIComponent(spreadType)}`;
 if (date) query += `&date=${encodeURIComponent(date)}`;

 const result = await serverHttpRequest<unknown>(`/history/sessions?${query}`, {
 method: 'GET',
 token,
 fallbackErrorMessage: tApi('unknown_error'),
 });

 if (!result.ok) {
 if (result.status === 401) {
 return { error: 'unauthorized' };
 }
 logger.error('HistoryAction.getHistorySessionsAction', result.error, {
 status: result.status,
 page,
 pageSize,
 spreadType,
 date,
 });
 return { error: result.error || tApi('unknown_error') };
 }

 return { success: true, data: result.data };
 } catch (error) {
 logger.error('HistoryAction.getHistorySessionsAction', error, { page, pageSize, spreadType, date });
 return { error: tApi('network_error') };
 }
}

/**
 * Lấy chi tiết một phiên đọc bài cụ thể (bao gồm cards, AI interactions).
 *
 * @param sessionId - ID phiên đọc bài (UUID)
 * @returns Chi tiết session hoặc error
 */
export async function getHistoryDetailAction(sessionId: string) {
 const tApi = await getTranslations('ApiErrors');

 try {
 const token = await getAccessToken();
 if (!token) {
 return { error: 'unauthorized' };
 }

 const result = await serverHttpRequest<unknown>(`/history/sessions/${sessionId}`, {
 method: 'GET',
 token,
 fallbackErrorMessage: tApi('unknown_error'),
 });

 if (!result.ok) {
 if (result.status === 401) {
 return { error: 'unauthorized' };
 }
 if (result.status === 404) {
 return { error: tApi('not_found') };
 }
 logger.error('HistoryAction.getHistoryDetailAction', result.error, {
 status: result.status,
 sessionId,
 });
 return { error: result.error || tApi('unknown_error') };
 }

 return { success: true, data: result.data };
 } catch (error) {
 logger.error('HistoryAction.getHistoryDetailAction', error, { sessionId });
 return { error: tApi('network_error') };
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
 const tApi = await getTranslations('ApiErrors');

 try {
 const token = await getAccessToken();
 if (!token) {
 return { error: 'unauthorized' };
 }

 let query = `page=${params.page}&pageSize=${params.pageSize}`;
 if (params.username) query += `&username=${encodeURIComponent(params.username)}`;
 if (params.spreadType) query += `&spreadType=${encodeURIComponent(params.spreadType)}`;
 if (params.startDate) query += `&startDate=${encodeURIComponent(params.startDate)}`;
 if (params.endDate) query += `&endDate=${encodeURIComponent(params.endDate)}`;

 const result = await serverHttpRequest<Record<string, unknown>>(`/History/admin/all-sessions?${query}`, {
 method: 'GET',
 token,
 fallbackErrorMessage: tApi('unknown_error'),
 });

 if (!result.ok) {
 if (result.status === 401) return { error: 'unauthorized' };
 if (result.status === 403) return { error: tApi('forbidden') };
 logger.error('HistoryAction.getAllHistorySessionsAdminAction', result.error, {
 status: result.status,
 params,
 });
 return { error: result.error || tApi('unknown_error') };
 }

 const data = result.data as Record<string, unknown>;
 // Safe Mapping for Admin History
 const safeData: AdminHistoryPaginatedResponse = {
 ...data,
 items: (data.items as AdminHistorySessionItem[] | undefined) || (data.Items as AdminHistorySessionItem[] | undefined) || [],
 totalCount: (data.totalCount as number | undefined) ?? (data.TotalCount as number | undefined) ?? 0,
 totalPages: (data.totalPages as number | undefined) ?? (data.TotalPages as number | undefined) ?? 0,
 page: (data.page as number | undefined) ?? (data.Page as number | undefined) ?? params.page,
 pageSize: (data.pageSize as number | undefined) ?? (data.PageSize as number | undefined) ?? params.pageSize,
 };
 return { success: true, data: safeData };
 } catch (err) {
 logger.error('HistoryAction.getAllHistorySessionsAdminAction', err, { params });
 return { error: tApi('network_error') };
 }
}
