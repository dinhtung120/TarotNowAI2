'use server';

import { cookies } from 'next/headers';

// Base URL của Backend API — thiết lập từ biến môi trường
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

// ======================================================================
// Helper Functions
// ======================================================================

/**
 * Hàm lấy Access Token từ cookie phía Server.
 * * Tại sao dùng server action thay vì fetch trực tiếp từ client?
 * → Bảo mật: Access token lưu trong HttpOnly cookie, client JS không đọc được.
 * → Next.js Server Components/Actions chạy trên server, có quyền đọc cookie.
 */
async function getAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 return cookieStore.get('accessToken')?.value;
}

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
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, message: 'Bạn chưa đăng nhập.' };

 try {
 const response = await fetch(`${API_URL}/reader/apply`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({ introText, proofDocuments }),
 });

 if (!response.ok) {
 // Parse error message từ ProblemDetails format
 const errorData = await response.json().catch(() => null);
 const msg = errorData?.detail || errorData?.message || 'Gửi đơn thất bại.';
 return { success: false, message: msg };
 }

 const data = await response.json();
 return { success: true, message: data.message || 'Đơn đã được gửi thành công.' };
 } catch (error) {
 console.error('[ReaderAction] submitReaderApplication failed:', error);
 return { success: false, message: 'Lỗi kết nối. Vui lòng thử lại.' };
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
 const response = await fetch(`${API_URL}/reader/my-request`, {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) return null;
 return await response.json();
 } catch (error) {
 console.error('[ReaderAction] getMyReaderRequest failed:', error);
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
 const url = new URL(`${API_URL}/readers`);
 url.searchParams.append('page', page.toString());
 url.searchParams.append('pageSize', pageSize.toString());
 if (specialty) url.searchParams.append('specialty', specialty);
 if (status) url.searchParams.append('status', status);
 if (searchTerm) url.searchParams.append('searchTerm', searchTerm);

 // Endpoint public — không cần auth token
 const response = await fetch(url.toString(), {
 method: 'GET',
 headers: { 'Content-Type': 'application/json' },
 cache: 'no-store',
 });

 if (!response.ok) return null;

 const data = await response.json();
 return {
 readers: data.readers || data.Readers || [],
 totalCount: data.totalCount ?? data.TotalCount ?? 0,
 };
 } catch (error) {
 console.error('[ReaderAction] listReaders failed:', error);
 return null;
 }
}

/**
 * Lấy hồ sơ Reader theo userId.
 * Backend API: GET /api/v1/reader/profile/{userId}
 */
export async function getReaderProfile(userId: string): Promise<ReaderProfile | null> {
 try {
 const response = await fetch(`${API_URL}/reader/profile/${userId}`, {
 method: 'GET',
 headers: { 'Content-Type': 'application/json' },
 cache: 'no-store',
 });

 if (!response.ok) return null;
 return await response.json();
 } catch (error) {
 console.error('[ReaderAction] getReaderProfile failed:', error);
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
 const response = await fetch(`${API_URL}/reader/profile`, {
 method: 'PATCH',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify(data),
 });

 return response.ok;
 } catch (error) {
 console.error('[ReaderAction] updateReaderProfile failed:', error);
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
 const response = await fetch(`${API_URL}/reader/status`, {
 method: 'PATCH',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({ status }),
 });

 return response.ok;
 } catch (error) {
 console.error('[ReaderAction] updateReaderStatus failed:', error);
 return false;
 }
}
