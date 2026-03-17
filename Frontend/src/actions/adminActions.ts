'use server';

import { cookies } from 'next/headers';
import { API_BASE_URL } from '@/lib/api';

// ======================================================================
// Kiểu dữ liệu cho Admin
// Các interface tương ứng với response DTO từ Backend Admin APIs.
// ======================================================================
// Helper Functions
// ======================================================================

/**
 * Hàm lấy Access Token từ cookie phía Server.
 */
async function getAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 return cookieStore.get('accessToken')?.value;
}

// ======================================================================

/** Thông tin user trong danh sách admin */
export interface AdminUserItem {
 id: string;
 email: string;
 username: string;
 displayName: string;
 status: string;
 role: string;
 level: number;
 exp: number;
 goldBalance: number;
 diamondBalance: number;
 createdAt: string;
}

/** Kết quả danh sách người dùng từ backend */
export interface ListUsersResponse {
 users: AdminUserItem[];
 totalCount: number;
}

/** Kết quả danh sách nạp tiền từ backend */
export interface ListDepositsResponse {
 deposits: AdminDepositOrder[];
 totalCount: number;
}

/** Kết quả phân trang chung */
export interface PaginatedResult<T> {
 items: T[];
 totalCount: number;
 page: number;
 pageSize: number;
}

/** Kết quả kiểm tra bất đồng bộ sổ cái (reconciliation) */
export interface MismatchRecord {
 userId: string;
 userBalance: number;
 ledgerBalance: number;
 difference: number;
}

/** Đơn nạp tiền (dùng cho admin view) */
export interface AdminDepositOrder {
 id: string;
 userId: string;
 username?: string; // Tên người dùng hiển thị
 amountVnd: number;
 diamondAmount: number;
 status: string;
 transactionId?: string; // Mã giao dịch Gateway/Admin
 createdAt: string;
}

// ======================================================================
// Actions — Admin APIs
// Tất cả đều yêu cầu role admin trong Bearer token.
// ======================================================================

/**
 * Lấy danh sách users có phân trang (Admin only).
 * Backend API: GET /api/v1/admin/users
 */
export async function listUsers(page = 1, pageSize = 20, searchTerm = ''): Promise<ListUsersResponse | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const url = new URL(`${API_BASE_URL}/admin/users`);
 url.searchParams.append('page', page.toString());
 url.searchParams.append('pageSize', pageSize.toString());
 if (searchTerm) {
 url.searchParams.append('searchTerm', searchTerm);
 }

 const response = await fetch(url.toString(), {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) {
 console.error(`[AdminAction] listUsers error: ${response.status} - ${await response.text()}`);
 return null;
 }

 const data = await response.json();
 return {
 users: data.users || data.Users || [],
 totalCount: data.totalCount ?? data.TotalCount ?? 0
 };
 } catch (error) {
 console.error('[AdminAction] Failed to list users:', error);
 return null;
 }
}

/**
 * Khóa/Mở khóa tài khoản user (Admin only).
 * Backend API: PATCH /api/v1/admin/users/lock
 *
 * @param userId - ID user cần thay đổi trạng thái
 * @param isLocked - true = khóa, false = mở khóa
 */
export async function toggleUserLock(userId: string, isLocked: boolean): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const response = await fetch(`${API_BASE_URL}/admin/users/lock`, {
 method: 'PATCH',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({
 userId,
 UserId: userId,
 isLocked,
 IsLocked: isLocked
 }),
 });

 if (!response.ok) {
 console.error(`[AdminAction] toggleUserLock error: ${response.status} - ${await response.text()}`);
 }
 return response.ok;
 } catch (error) {
 console.error('[AdminAction] Failed to toggle user lock:', error);
 return false;
 }
}

/**
 * Lấy danh sách đơn nạp tiền có phân trang (Admin only).
 * Backend API: GET /api/v1/admin/deposits
 */
export async function listDeposits(page = 1, pageSize = 20, status = ''): Promise<ListDepositsResponse | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const url = new URL(`${API_BASE_URL}/admin/deposits`);
 url.searchParams.append('page', page.toString());
 url.searchParams.append('pageSize', pageSize.toString());
 if (status) {
 url.searchParams.append('status', status);
 }

 const response = await fetch(url.toString(), {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) {
 console.error(`[AdminAction] listDeposits error: ${response.status} - ${await response.text()}`);
 return null;
 }

 const data = await response.json();
 return {
 deposits: data.deposits || data.Deposits || [],
 totalCount: data.totalCount ?? data.TotalCount ?? 0
 };
 } catch (error) {
 console.error('[AdminAction] Failed to list deposits:', error);
 return null;
 }
}

/**
 * Kiểm tra bất đồng bộ sổ cái — reconciliation (Admin only).
 * Backend API: GET /api/v1/admin/reconciliation/wallet
 * So sánh số dư users vs tổng ledger entries.
 */
export async function getWalletMismatches(): Promise<MismatchRecord[] | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const response = await fetch(`${API_BASE_URL}/admin/reconciliation/wallet`, {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) {
 console.error(`[AdminAction] getWalletMismatches error: ${response.status} - ${await response.text()}`);
 return null;
 }

 const data = await response.json();
 // Safe Mapping for Reconciliation
 if (Array.isArray(data)) {
 return data.map((d: Record<string, unknown>) => ({
 userId: String(d.userId ?? d.UserId ?? ''),
 userBalance: Number(d.userBalance ?? d.UserBalance ?? 0),
 ledgerBalance: Number(d.ledgerBalance ?? d.LedgerBalance ?? 0),
 difference: Number(d.difference ?? d.Difference ?? 0)
 }));
 }
 return data;
 } catch (error) {
 console.error('[AdminAction] Failed to get wallet mismatches:', error);
 return null;
 }
}

/**
 * Cộng tiền (Gold/Diamond) cho người dùng (Admin only).
 * Backend API: POST /api/v1/admin/users/add-balance
 */
export async function addUserBalance(userId: string, currency: string, amount: number, reason?: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const response = await fetch(`${API_BASE_URL}/admin/users/add-balance`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({
 userId,
 UserId: userId,
 currency,
 Currency: currency,
 amount,
 Amount: amount,
 reason,
 Reason: reason
 }),
 });

 if (!response.ok) {
 console.error(`[AdminAction] addUserBalance error: ${response.status} - ${await response.text()}`);
 }
 return response.ok;
 } catch (error) {
 console.error('[AdminAction] Failed to add user balance:', error);
 return false;
 }
}

/**
 * Phê duyệt hoặc từ chối đơn nạp tiền (Admin only).
 * Backend API: PATCH /api/v1/admin/deposits/process
 */
export async function processDeposit(depositId: string, action: 'approve' | 'reject', transactionId?: string): Promise<boolean> {
 try {
 const accessToken = await getAccessToken();
 if (!accessToken) {
 return false;
 }

 const bodyPayload = {
 depositId: depositId,
 DepositId: depositId,
 action: action,
 Action: action,
 transactionId: transactionId || "",
 TransactionId: transactionId || ""
 };

 const response = await fetch(`${API_BASE_URL}/admin/deposits/process`, {
 method: 'PATCH',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify(bodyPayload),
 });

 const responseText = await response.text();
 if (!response.ok) {
 console.error(`[AdminAction] processDeposit error: ${response.status} - ${responseText}`);
 return false;
 }

 return true;
 } catch (error) {
 console.error('[AdminAction] processDeposit failed:', error);
 return false;
 }
}

// ======================================================================
// Phase 2.1 — Reader Request Management (Admin)
// ======================================================================

/** Đơn xin Reader (dùng cho admin view) */
export interface AdminReaderRequest {
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

/** Kết quả danh sách đơn xin Reader */
export interface ListReaderRequestsResponse {
 requests: AdminReaderRequest[];
 totalCount: number;
}

/**
 * Lấy danh sách đơn xin Reader có phân trang (Admin only).
 * Backend API: GET /api/v1/admin/reader-requests
 *
 * @param page - Trang hiện tại.
 * @param pageSize - Số đơn mỗi trang.
 * @param statusFilter - Lọc theo status: pending | approved | rejected.
 */
export async function listReaderRequests(
 page = 1,
 pageSize = 20,
 statusFilter = ''
): Promise<ListReaderRequestsResponse | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const url = new URL(`${API_BASE_URL}/admin/reader-requests`);
 url.searchParams.append('page', page.toString());
 url.searchParams.append('pageSize', pageSize.toString());
 if (statusFilter) url.searchParams.append('statusFilter', statusFilter);

 const response = await fetch(url.toString(), {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) {
 console.error(`[AdminAction] listReaderRequests error: ${response.status}`);
 return null;
 }

 const data = await response.json();
 return {
 requests: data.requests || data.Requests || [],
 totalCount: data.totalCount ?? data.TotalCount ?? 0,
 };
 } catch (error) {
 console.error('[AdminAction] listReaderRequests failed:', error);
 return null;
 }
}

/**
 * Admin phê duyệt hoặc từ chối đơn xin Reader.
 * Backend API: PATCH /api/v1/admin/reader-requests/process
 *
 * @param requestId - ObjectId string của reader_requests document.
 * @param action - "approve" | "reject".
 * @param adminNote - Ghi chú admin (tùy chọn).
 */
export async function processReaderRequest(
 requestId: string,
 action: 'approve' | 'reject',
 adminNote?: string
): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const response = await fetch(`${API_BASE_URL}/admin/reader-requests/process`, {
 method: 'PATCH',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({
 requestId,
 RequestId: requestId,
 action,
 Action: action,
 adminNote: adminNote || '',
 AdminNote: adminNote || '',
 }),
 });

 if (!response.ok) {
 console.error(`[AdminAction] processReaderRequest error: ${response.status}`);
 }
 return response.ok;
 } catch (error) {
 console.error('[AdminAction] processReaderRequest failed:', error);
 return false;
 }
}
