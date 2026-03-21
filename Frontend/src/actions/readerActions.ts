/*
 * ===================================================================
 * FILE: readerActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xử lý mọi nghiệp vụ liên quan đến Reader (Thợ xem Tarot).
 *   Bao gồm: Gửi đơn đăng ký, Liệt kê danh sách Reader (Kèm bộ lọc/tìm kiếm),
 *   và quản lý trạng thái Online/Offline.
 * ===================================================================
 */
'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

// ======================================================================
// Helper Functions
// ======================================================================

/**
 * Hàm lấy Access Token từ cookie phía Server.
 * * Tại sao dùng server action thay vì fetch trực tiếp từ client?
 * → Bảo mật: Access token lưu trong HttpOnly cookie, client JS không đọc được.
 * → Next.js Server Components/Actions chạy trên server, có quyền đọc cookie.
 */
const getAccessToken = getServerAccessToken;

// ======================================================================
// Kiểu dữ liệu — tương ứng với backend DTOs
// ======================================================================

/** Hồ sơ Reader công khai */
export interface ReaderProfile {
 id: string;
 userId: string;
 status: string;
 diamondPerQuestion: number;
 bioVi: string;
 bioEn: string;
 bioZh: string;
 specialties: string[];
 avgRating: number;
 totalReviews: number;
 displayName: string;
 avatarUrl?: string | null;
 createdAt: string;
 updatedAt?: string | null;
}

/** Kết quả phân trang danh sách Reader */
export interface ListReadersResponse {
 readers: ReaderProfile[];
 totalCount: number;
}

/** Trạng thái đơn xin Reader của user */
export interface MyReaderRequest {
 hasRequest: boolean;
 status?: string;
 introText?: string;
 adminNote?: string;
 createdAt?: string;
 reviewedAt?: string;
}

/** Đơn xin Reader (dùng cho admin) */
export interface ReaderRequestItem {
 id: string;
 userId: string;
 status: string;
 introText: string;
 proofDocuments: string[];
 adminNote?: string;
 reviewedBy?: string;
 reviewedAt?: string;
 createdAt: string;
}

/** Kết quả phân trang danh sách đơn xin Reader */
export interface ListReaderRequestsResponse {
 requests: ReaderRequestItem[];
 totalCount: number;
}

// ======================================================================
// Actions — Reader APIs
// ======================================================================

/**
 * Gửi đơn xin trở thành Reader.
 * Backend API: POST /api/v1/reader/apply
 * * @param introText - Lời giới thiệu bản thân (tối thiểu 20 ký tự).
 * @param proofDocuments - Danh sách URL tài liệu chứng minh (tùy chọn).
 * @returns Kết quả thành công hay thất bại + message.
 */
export async function submitReaderApplication(
 introText: string,
 proofDocuments: string[] = []
): Promise<{ success: boolean; message: string }> {
 const t = await getTranslations("ReaderApply");
 const tApi = await getTranslations("ApiErrors");
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, message: tApi("unauthorized") };

 try {
 const result = await serverHttpRequest<{ message?: string }>('/reader/apply', {
 method: 'POST',
 token: accessToken,
 json: { introText, proofDocuments },
 fallbackErrorMessage: t("errors.submit_failed"),
 });

 if (!result.ok) {
 logger.error('[ReaderAction] submitReaderApplication', result.error, { status: result.status });
 return { success: false, message: result.error || t("errors.submit_failed") };
 }

 return { success: true, message: result.data.message || t("success.submitted") };
 } catch (error) {
 logger.error('[ReaderAction] submitReaderApplication', error);
 return { success: false, message: tApi("network_error") };
 }
}

/**
 * Xem trạng thái đơn xin Reader của mình.
 * Backend API: GET /api/v1/reader/my-request
 */
export async function getMyReaderRequest(): Promise<MyReaderRequest | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const result = await serverHttpRequest<MyReaderRequest>('/reader/my-request', {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to get reader request',
 });

 if (!result.ok) {
 logger.error('[ReaderAction] getMyReaderRequest', result.error, { status: result.status });
 return null;
 }
 return result.data;
 } catch (error) {
 logger.error('[ReaderAction] getMyReaderRequest', error);
 return null;
 }
}

/**
 * Lấy danh sách Reader công khai (directory listing).
 * Backend API: GET /api/v1/readers
 * * @param page - Trang hiện tại.
 * @param pageSize - Số Reader mỗi trang.
 * @param specialty - Lọc theo chuyên môn.
 * @param status - Lọc theo trạng thái online.
 * @param searchTerm - Tìm kiếm theo tên.
 */
export async function listReaders(
 page = 1,
 pageSize = 12,
 specialty = '',
 status = '',
 searchTerm = ''
): Promise<ListReadersResponse | null> {
 try {
 const params = new URLSearchParams({
 page: page.toString(),
 pageSize: pageSize.toString(),
 });
 if (specialty) params.append('specialty', specialty);
 if (status) params.append('status', status);
 if (searchTerm) params.append('searchTerm', searchTerm);

 // Endpoint public — không cần auth token
 const result = await serverHttpRequest<{ readers?: ReaderProfile[]; Readers?: ReaderProfile[]; totalCount?: number; TotalCount?: number }>(
 `/readers?${params.toString()}`,
 {
 method: 'GET',
 fallbackErrorMessage: 'Failed to list readers',
 });

 if (!result.ok) {
 logger.error('[ReaderAction] listReaders', result.error, {
 status: result.status,
 page,
 pageSize,
 specialty,
 statusFilter: status,
 searchTerm,
 });
 return null;
 }
 const data = result.data;
 return {
 readers: data.readers || data.Readers || [],
 totalCount: data.totalCount ?? data.TotalCount ?? 0,
 };
 } catch (error) {
 logger.error('[ReaderAction] listReaders', error, {
 page,
 pageSize,
 specialty,
 statusFilter: status,
 searchTerm,
 });
 return null;
 }
}

/**
 * Lấy danh sách Reader nổi bật cho trang chủ với cache ngắn hạn.
 * Dùng cho UX chuyển trang nhanh (đặc biệt sau login).
 */
export async function listFeaturedReaders(
 limit = 4
): Promise<ReaderProfile[]> {
 try {
 const result = await serverHttpRequest<{ readers?: ReaderProfile[]; Readers?: ReaderProfile[] }>(
 `/readers?page=1&pageSize=${limit}`,
 {
 method: 'GET',
 next: { revalidate: 120 },
 fallbackErrorMessage: 'Failed to list featured readers',
 });

 if (!result.ok) {
 logger.error('[ReaderAction] listFeaturedReaders', result.error, {
 status: result.status,
 limit,
 });
 return [];
 }
 const data = result.data;
 return data.readers || data.Readers || [];
 } catch (error) {
 logger.error('[ReaderAction] listFeaturedReaders', error, { limit });
 return [];
 }
}

/**
 * Lấy hồ sơ Reader theo userId.
 * Backend API: GET /api/v1/reader/profile/{userId}
 */
export async function getReaderProfile(userId: string): Promise<ReaderProfile | null> {
 try {
 const result = await serverHttpRequest<ReaderProfile>(`/reader/profile/${userId}`, {
 method: 'GET',
 fallbackErrorMessage: 'Failed to get reader profile',
 });

 if (!result.ok) {
 logger.error('[ReaderAction] getReaderProfile', result.error, {
 status: result.status,
 userId,
 });
 return null;
 }
 return result.data;
 } catch (error) {
 logger.error('[ReaderAction] getReaderProfile', error, { userId });
 return null;
 }
}

/**
 * Cập nhật hồ sơ Reader (chỉ Reader tự cập nhật).
 * Backend API: PATCH /api/v1/reader/profile
 */
export async function updateReaderProfile(data: {
 bioVi?: string;
 bioEn?: string;
 bioZh?: string;
 diamondPerQuestion?: number;
 specialties?: string[];
}): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/reader/profile', {
 method: 'PATCH',
 token: accessToken,
 json: data,
 fallbackErrorMessage: 'Failed to update reader profile',
 });

 if (!result.ok) {
 logger.error('[ReaderAction] updateReaderProfile', result.error, { status: result.status });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('[ReaderAction] updateReaderProfile', error);
 return false;
 }
}

/**
 * Cập nhật trạng thái online của Reader.
 * Backend API: PATCH /api/v1/reader/status
 * * @param status - "online" | "offline" | "accepting_questions"
 */
export async function updateReaderStatus(status: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/reader/status', {
 method: 'PATCH',
 token: accessToken,
 json: { status },
 fallbackErrorMessage: 'Failed to update reader status',
 });

 if (!result.ok) {
 logger.error('[ReaderAction] updateReaderStatus', result.error, {
 status: result.status,
 readerStatus: status,
 });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('[ReaderAction] updateReaderStatus', error, { readerStatus: status });
 return false;
 }
}
