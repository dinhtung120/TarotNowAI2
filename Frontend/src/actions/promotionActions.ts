'use server';

import { cookies } from 'next/headers';

// Base URL của Backend API
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

// ======================================================================
// Kiểu dữ liệu cho Promotions
// ======================================================================

/** Chương trình khuyến mãi nạp tiền */
export interface DepositPromotion {
  id: string;
  minAmountVnd: number;
  bonusDiamond: number;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}

// ======================================================================
// Actions — Admin Promotions APIs
// CRUD đầy đủ cho quản lý chương trình khuyến mãi nạp tiền.
// Backend route: /api/v1/admin/promotions
// ======================================================================

/**
 * Lấy danh sách tất cả chương trình khuyến mãi.
 * Endpoint AllowAnonymous nên có thể gọi từ phía client lẫn admin.
 * 
 * @param onlyActive - true = chỉ lấy promo đang hoạt động
 */
export async function listPromotions(onlyActive = false): Promise<DepositPromotion[] | null> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/admin/promotions?onlyActive=${onlyActive}`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      cache: 'no-store',
    });

    if (!response.ok) {
      console.error('listPromotions error', response.status);
      return null;
    }

    return await response.json();
  } catch (error) {
    console.error('Failed to list promotions:', error);
    return null;
  }
}

/**
 * Tạo chương trình khuyến mãi mới (Admin only).
 * 
 * @param minAmountVnd - Số tiền tối thiểu để kích hoạt khuyến mãi
 * @param bonusDiamond - Số Diamond thưởng thêm
 */
export async function createPromotion(minAmountVnd: number, bonusDiamond: number): Promise<boolean> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/admin/promotions`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ minAmountVnd, bonusDiamond }),
    });

    return response.ok;
  } catch (error) {
    console.error('Failed to create promotion:', error);
    return false;
  }
}

/**
 * Cập nhật chương trình khuyến mãi (Admin only).
 * 
 * @param id - ID promotion cần cập nhật
 * @param data - Dữ liệu mới (minAmountVnd, bonusDiamond, isActive)
 */
export async function updatePromotion(
  id: string, 
  data: { minAmountVnd: number; bonusDiamond: number; isActive: boolean }
): Promise<boolean> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/admin/promotions/${id}`, {
      method: 'PUT',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });

    return response.ok;
  } catch (error) {
    console.error('Failed to update promotion:', error);
    return false;
  }
}

/**
 * Xóa chương trình khuyến mãi (Admin only).
 * 
 * @param id - ID promotion cần xóa
 */
export async function deletePromotion(id: string): Promise<boolean> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/admin/promotions/${id}`, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
    });

    return response.ok;
  } catch (error) {
    console.error('Failed to delete promotion:', error);
    return false;
  }
}
