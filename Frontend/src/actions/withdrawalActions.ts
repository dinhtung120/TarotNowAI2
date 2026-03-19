/*
 * ===================================================================
 * FILE: withdrawalActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Quy trình Rút Tiền Hậu Lộc (Withdrawal) dành cho Reader.
 *   - Reader: Yêu cầu rút tiền, xem lịch sử.
 *   - Admin: Xem hàng đợi duyệt đơn rút tiền và Xử lý (Approve/Reject) kèm MFA.
 * ===================================================================
 */
'use server';

import { cookies } from 'next/headers';
import { API_BASE_URL } from '@/lib/api';
import { getTranslations } from 'next-intl/server';

async function getAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 return cookieStore.get('accessToken')?.value;
}

// ======================================================================
// Types
// ======================================================================

export interface WithdrawalResult {
 id: string;
 userId: string;
 amountDiamond: number;
 amountVnd: number;
 feeVnd: number;
 netAmountVnd: number;
 bankName: string;
 bankAccountName: string;
 bankAccountNumber: string;
 status: string;
 adminNote?: string | null;
 processedAt?: string | null;
 createdAt: string;
}

// ======================================================================
// Server Actions
// ======================================================================

/**
 * Reader tạo yêu cầu rút tiền.
 */
export async function createWithdrawal(data: {
 amountDiamond: number;
 bankName: string;
 bankAccountName: string;
 bankAccountNumber: string;
 mfaCode: string;
}): Promise<{ success: boolean; requestId?: string; error?: string }> {
 const tApi = await getTranslations("ApiErrors");
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi("unauthorized") };

 try {
 const res = await fetch(`${API_BASE_URL}/withdrawal/create`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify(data),
 });
 if (!res.ok) {
 const err = await res.json().catch(() => ({}));
 return { success: false, error: err.detail || err.msg || tApi("unknown_error") };
 }
 return await res.json();
 } catch (error) {
 console.error('[WithdrawalAction] createWithdrawal failed:', error);
 return { success: false, error: tApi("network_error") };
 }
}

/**
 * Reader xem lịch sử rút tiền.
 */
export async function listMyWithdrawals(page = 1, pageSize = 20): Promise<WithdrawalResult[]> {
 const accessToken = await getAccessToken();
 if (!accessToken) return [];

 try {
 const res = await fetch(`${API_BASE_URL}/withdrawal/my?page=${page}&pageSize=${pageSize}`, {
 method: 'GET',
 headers: { 'Authorization': `Bearer ${accessToken}` },
 cache: 'no-store',
 });
 if (!res.ok) return [];
 return await res.json();
 } catch (error) {
 console.error('[WithdrawalAction] listMyWithdrawals failed:', error);
 return [];
 }
}

/**
 * Admin lấy danh sách pending.
 */
export async function listWithdrawalQueue(page = 1, pageSize = 20): Promise<WithdrawalResult[]> {
 const accessToken = await getAccessToken();
 if (!accessToken) return [];

 try {
 const res = await fetch(`${API_BASE_URL}/admin/withdrawals/queue?page=${page}&pageSize=${pageSize}`, {
 method: 'GET',
 headers: { 'Authorization': `Bearer ${accessToken}` },
 cache: 'no-store',
 });
 if (!res.ok) return [];
 return await res.json();
 } catch (error) {
 console.error('[WithdrawalAction] listQueue failed:', error);
 return [];
 }
}

/**
 * Admin approve/reject.
 */
export async function processWithdrawal(data: {
 withdrawalId: string;
 action: 'approve' | 'reject';
 adminNote?: string;
 mfaCode: string;
}): Promise<{ success: boolean; error?: string }> {
 const tApi = await getTranslations("ApiErrors");
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi("unauthorized") };

 try {
 const res = await fetch(`${API_BASE_URL}/admin/withdrawals/process`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify(data),
 });
 if (!res.ok) {
 const err = await res.json().catch(() => ({}));
 return { success: false, error: err.detail || err.msg || tApi("unknown_error") };
 }
 return { success: true };
 } catch (error) {
 console.error('[WithdrawalAction] processWithdrawal failed:', error);
 return { success: false, error: tApi("network_error") };
 }
}
