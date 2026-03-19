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

import { cookies } from 'next/headers';
import { API_BASE_URL } from '@/lib/api';

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
 const cookieStore = await cookies();
 const accessToken = cookieStore.get('accessToken')?.value;

 try {
 const query = new URLSearchParams({
 page: page.toString(),
 pageSize: pageSize.toString(),
 });
 if (status) query.append('status', status);

 const response = await fetch(`${API_BASE_URL}/admin/deposits?${query.toString()}`, {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store'
 });

 if (!response.ok) {
 console.error('listDepositsAdminAction error', response.status);
 return null;
 }

 return await response.json();
 } catch (error) {
 console.error('Failed to list deposits:', error);
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
 const cookieStore = await cookies();
 const accessToken = cookieStore.get('accessToken')?.value;

 try {
 const response = await fetch(`${API_BASE_URL}/deposits/orders`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({ amountVnd }),
 });

 if (!response.ok) {
 console.error('createDepositOrder error', response.status);
 return null;
 }

 return await response.json();
 } catch (error) {
 console.error('Failed to create deposit order:', error);
 return null;
 }
}
