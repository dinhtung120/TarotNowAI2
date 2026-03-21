/*
 * ===================================================================
 * FILE: adminActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa các Server Actions (chạy hoàn toàn trên máy chủ Node.js của Next.js)
 *   để giao tiếp với Backend API dành riêng cho nghiệp vụ Quản trị (Admin).
 *
 * BẢO MẬT & KIẾN TRÚC:
 *   - Sử dụng strict chỉ thị 'use server' giúp mã không bao giờ lọt xuống Client (Trình duyệt).
 *   - Lấy accessToken từ cookie HttpOnly hoặc thẻ cookie mã hóa tăng tính bảo mật chống XSS.
 *   - Điểm kiểm soát tập trung cho các chức năng nhạy cảm như khoá user, xem sai lệch số dư.
 * ===================================================================
 */
'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

// ======================================================================
// Kiểu dữ liệu cho Admin
// Các interface tương ứng với response DTO từ Backend Admin APIs.
// ======================================================================
// Helper Functions
// ======================================================================

const getAccessToken = getServerAccessToken;

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
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (searchTerm) query.append('searchTerm', searchTerm);

  const result = await serverHttpRequest<{ users?: AdminUserItem[]; Users?: AdminUserItem[]; totalCount?: number; TotalCount?: number }>(
   `/admin/users?${query.toString()}`,
   {
    method: 'GET',
    token: accessToken,
    fallbackErrorMessage: 'Failed to list users',
   }
  );

  if (!result.ok) {
   logger.error('[AdminAction] listUsers', result.error, {
    status: result.status,
    page,
    pageSize,
    searchTerm,
   });
   return null;
  }

  const data = result.data;
  return {
   users: data.users || data.Users || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  };
 } catch (error) {
  logger.error('[AdminAction] listUsers', error, { page, pageSize, searchTerm });
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
  const result = await serverHttpRequest<unknown>('/admin/users/lock', {
   method: 'PATCH',
   token: accessToken,
   json: {
    userId,
    UserId: userId,
    isLocked,
    IsLocked: isLocked,
   },
   fallbackErrorMessage: 'Failed to toggle user lock',
  });

  if (!result.ok) {
   logger.error('[AdminAction] toggleUserLock', result.error, {
    status: result.status,
    userId,
    isLocked,
   });
   return false;
  }
  return true;
 } catch (error) {
  logger.error('[AdminAction] toggleUserLock', error, { userId, isLocked });
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
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (status) query.append('status', status);

  const result = await serverHttpRequest<{ deposits?: AdminDepositOrder[]; Deposits?: AdminDepositOrder[]; totalCount?: number; TotalCount?: number }>(
   `/admin/deposits?${query.toString()}`,
   {
    method: 'GET',
    token: accessToken,
    fallbackErrorMessage: 'Failed to list deposits',
   }
  );

  if (!result.ok) {
   logger.error('[AdminAction] listDeposits', result.error, {
    status: result.status,
    page,
    pageSize,
    statusFilter: status,
   });
   return null;
  }

  const data = result.data;
  return {
   deposits: data.deposits || data.Deposits || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  };
 } catch (error) {
  logger.error('[AdminAction] listDeposits', error, { page, pageSize, statusFilter: status });
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
  const result = await serverHttpRequest<unknown>('/admin/reconciliation/wallet', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get wallet mismatches',
  });

  if (!result.ok) {
   logger.error('[AdminAction] getWalletMismatches', result.error, { status: result.status });
   return null;
  }

  const data = result.data;
  // Safe Mapping for Reconciliation
  if (Array.isArray(data)) {
   return data.map((d: Record<string, unknown>) => ({
    userId: String(d.userId ?? d.UserId ?? ''),
    userBalance: Number(d.userBalance ?? d.UserBalance ?? 0),
    ledgerBalance: Number(d.ledgerBalance ?? d.LedgerBalance ?? 0),
    difference: Number(d.difference ?? d.Difference ?? 0),
   }));
  }
  return data as MismatchRecord[];
 } catch (error) {
  logger.error('[AdminAction] getWalletMismatches', error);
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
  const result = await serverHttpRequest<unknown>('/admin/users/add-balance', {
   method: 'POST',
   token: accessToken,
   json: {
    userId,
    UserId: userId,
    currency,
    Currency: currency,
    amount,
    Amount: amount,
    reason,
    Reason: reason,
   },
   fallbackErrorMessage: 'Failed to add user balance',
  });

  if (!result.ok) {
   logger.error('[AdminAction] addUserBalance', result.error, {
    status: result.status,
    userId,
    currency,
    amount,
   });
   return false;
  }
  return true;
 } catch (error) {
  logger.error('[AdminAction] addUserBalance', error, { userId, currency, amount });
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

  const normalizedTransactionId = transactionId?.trim();
  const bodyPayload: Record<string, string> = {
   depositId: depositId,
   DepositId: depositId,
   action: action,
   Action: action,
  };

  if (normalizedTransactionId) {
   bodyPayload.transactionId = normalizedTransactionId;
   bodyPayload.TransactionId = normalizedTransactionId;
  }

  const result = await serverHttpRequest<unknown>('/admin/deposits/process', {
   method: 'PATCH',
   token: accessToken,
   json: bodyPayload,
   fallbackErrorMessage: 'Failed to process deposit',
  });

  if (!result.ok) {
   logger.error('[AdminAction] processDeposit', result.error, {
    status: result.status,
    depositId,
    action,
   });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[AdminAction] processDeposit', error, { depositId, action });
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
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (statusFilter) query.append('statusFilter', statusFilter);

  const result = await serverHttpRequest<{ requests?: AdminReaderRequest[]; Requests?: AdminReaderRequest[]; totalCount?: number; TotalCount?: number }>(
   `/admin/reader-requests?${query.toString()}`,
   {
    method: 'GET',
    token: accessToken,
    fallbackErrorMessage: 'Failed to list reader requests',
   }
  );

  if (!result.ok) {
   logger.error('[AdminAction] listReaderRequests', result.error, {
    status: result.status,
    page,
    pageSize,
    statusFilter,
   });
   return null;
  }

  const data = result.data;
  return {
   requests: data.requests || data.Requests || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  };
 } catch (error) {
  logger.error('[AdminAction] listReaderRequests', error, { page, pageSize, statusFilter });
  return null;
 }
}

export interface UpdateUserParams {
 role: string;
 status: string;
 diamondBalance: number;
 goldBalance: number;
}

/**
 * Cập nhật toàn diện thông tin User (Admin only).
 * Backend API: PUT /api/v1/Admin/users/{id}
 */
export async function updateUser(userId: string, data: UpdateUserParams): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>(`/admin/users/${userId}`, {
   method: 'PUT',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to update user',
  });

  if (!result.ok) {
   logger.error('[AdminAction] updateUser', result.error, { status: result.status, userId });
   return false;
  }
  return true;
 } catch (error) {
  logger.error('[AdminAction] updateUser', error, { userId });
  return false;
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
  const result = await serverHttpRequest<unknown>('/admin/reader-requests/process', {
   method: 'PATCH',
   token: accessToken,
   json: {
    requestId,
    RequestId: requestId,
    action,
    Action: action,
    adminNote: adminNote || '',
    AdminNote: adminNote || '',
   },
   fallbackErrorMessage: 'Failed to process reader request',
  });

  if (!result.ok) {
   logger.error('[AdminAction] processReaderRequest', result.error, {
    status: result.status,
    requestId,
    action,
   });
   return false;
  }
  return true;
 } catch (error) {
  logger.error('[AdminAction] processReaderRequest', error, { requestId, action });
  return false;
 }
}
