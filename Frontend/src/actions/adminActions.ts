'use server';

import { cookies } from 'next/headers';

// Base URL của Backend API
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

// ======================================================================
// Kiểu dữ liệu cho Admin
// Các interface tương ứng với response DTO từ Backend Admin APIs.
// ======================================================================

/** Thông tin user trong danh sách admin */
export interface AdminUserItem {
  id: string;
  email: string;
  username: string;
  displayName: string;
  status: string;
  role: string;
  goldBalance: number;
  diamondBalance: number;
  createdAt: string;
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
  amountVnd: number;
  diamondAmount: number;
  status: string;
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
export async function listUsers(page = 1, pageSize = 20): Promise<PaginatedResult<AdminUserItem> | null> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/admin/users?page=${page}&pageSize=${pageSize}`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      cache: 'no-store',
    });

    if (!response.ok) {
      console.error('listUsers error', response.status);
      return null;
    }

    return await response.json();
  } catch (error) {
    console.error('Failed to list users:', error);
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
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/admin/users/lock`, {
      method: 'PATCH',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ userId, isLocked }),
    });

    return response.ok;
  } catch (error) {
    console.error('Failed to toggle user lock:', error);
    return false;
  }
}

/**
 * Lấy danh sách đơn nạp tiền có phân trang (Admin only).
 * Backend API: GET /api/v1/admin/deposits
 */
export async function listDeposits(page = 1, pageSize = 20): Promise<PaginatedResult<AdminDepositOrder> | null> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/admin/deposits?page=${page}&pageSize=${pageSize}`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      cache: 'no-store',
    });

    if (!response.ok) {
      console.error('listDeposits error', response.status);
      return null;
    }

    return await response.json();
  } catch (error) {
    console.error('Failed to list deposits:', error);
    return null;
  }
}

/**
 * Kiểm tra bất đồng bộ sổ cái — reconciliation (Admin only).
 * Backend API: GET /api/v1/admin/reconciliation/wallet
 * So sánh số dư users vs tổng ledger entries.
 */
export async function getWalletMismatches(): Promise<MismatchRecord[] | null> {
  const cookieStore = await cookies();
  const accessToken = cookieStore.get('accessToken')?.value;

  try {
    const response = await fetch(`${API_URL}/admin/reconciliation/wallet`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      cache: 'no-store',
    });

    if (!response.ok) {
      console.error('getWalletMismatches error', response.status);
      return null;
    }

    return await response.json();
  } catch (error) {
    console.error('Failed to get wallet mismatches:', error);
    return null;
  }
}
