/*
 * ===================================================================
 * FILE: depositActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao tiếp với API thanh toán để lập lệnh Nạp Tiền (Deposit).
 *   Ví dụ: Tạo Request lên Backend -> Trả về Hóa đơn Payment URL của 
 *   cổng thanh toán (VNPay/Momo) để tự động Redirect khách hàng qua cổng phụ.
 * ===================================================================
 */
'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

// ======================================================================
// Kiểu dữ liệu cho Deposit
// Định nghĩa interface TypeScript tương ứng với DTO backend.
// ======================================================================

/** Kết quả tạo đơn nạp tiền — trả về URL thanh toán */
export interface CreateDepositOrderResponse {
 orderId: string;
 paymentUrl: string;
 amountVnd: number;
 diamondAmount: number;
}

/** Đơn nạp tiền trong danh sách admin */
export interface DepositOrder {
 id: string;
 userId: string;
 username: string; // Thêm username từ backend
 amountVnd: number;
 diamondAmount: number;
 status: string;
 transactionId?: string;
 createdAt: string;
 completedAt?: string;
}

export interface ListDepositsResponse {
 deposits: DepositOrder[];
 totalCount: number;
}

/**
 * Lấy danh sách đơn nạp tiền (dành cho Admin).
 */
export async function listDepositsAdminAction(page: number, pageSize: number, status?: string): Promise<ListDepositsResponse | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

 try {
 const query = new URLSearchParams({
 page: page.toString(),
 pageSize: pageSize.toString(),
 });
 if (status) query.append('status', status);

 const result = await serverHttpRequest<ListDepositsResponse>(`/admin/deposits?${query.toString()}`, {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to list deposits',
 });

 if (!result.ok) {
 logger.error('DepositAction.listDepositsAdminAction', result.error, {
 status: result.status,
 page,
 pageSize,
 statusFilter: status,
 });
 return null;
 }

 return result.data;
 } catch (error) {
 logger.error('DepositAction.listDepositsAdminAction', error, { page, pageSize, statusFilter: status });
 return null;
 }
}

// ======================================================================
// Actions
// Mỗi function sử dụng pattern: lấy accessToken từ cookie → gọi API → parse JSON.
// Dùng 'use server' để chạy trên server side (Next.js Server Actions).
// ======================================================================

/**
 * Tạo đơn nạp tiền mới cho user.
 * Backend tạo order status=pending và trả về payment URL để redirect.
 * * @param amountVnd - Số tiền VND muốn nạp
 * @returns Object chứa orderId + paymentUrl, hoặc null nếu lỗi
 */
export async function createDepositOrder(amountVnd: number): Promise<CreateDepositOrderResponse | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

 try {
 const result = await serverHttpRequest<CreateDepositOrderResponse>('/deposits/orders', {
 method: 'POST',
 token: accessToken,
 json: { amountVnd },
 fallbackErrorMessage: 'Failed to create deposit order',
 });

 if (!result.ok) {
 logger.error('DepositAction.createDepositOrder', result.error, {
 status: result.status,
 amountVnd,
 });
 return null;
 }

 return result.data;
 } catch (error) {
 logger.error('DepositAction.createDepositOrder', error, { amountVnd });
 return null;
 }
}
