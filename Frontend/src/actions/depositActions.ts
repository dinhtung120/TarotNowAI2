'use server';

import { cookies } from 'next/headers';

// Base URL của Backend API, đọc từ biến môi trường hoặc fallback về localhost.
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

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
  amountVnd: number;
  diamondAmount: number;
  status: string;
  createdAt: string;
  completedAt?: string;
}

// ======================================================================
// Actions
// Mỗi function sử dụng pattern: lấy accessToken từ cookie → gọi API → parse JSON.
// Dùng 'use server' để chạy trên server side (Next.js Server Actions).
// ======================================================================

/**
 * Tạo đơn nạp tiền mới cho user.
 * Backend tạo order status=pending và trả về payment URL để redirect.
 * 
 * @param amountVnd - Số tiền VND muốn nạp
 * @returns Object chứa orderId + paymentUrl, hoặc null nếu lỗi
 */
export async function createDepositOrder(amountVnd: number): Promise<CreateDepositOrderResponse | null> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/deposits/orders`, {
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
