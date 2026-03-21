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

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

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
 const result = await serverHttpRequest<{ success: boolean; requestId?: string; error?: string }>(
 '/withdrawal/create',
 {
 method: 'POST',
 token: accessToken,
 json: data,
 fallbackErrorMessage: tApi("unknown_error"),
 }
 );
 if (!result.ok) {
 logger.error('[WithdrawalAction] createWithdrawal', result.error, { status: result.status });
 return { success: false, error: result.error || tApi("unknown_error") };
 }
 return result.data;
 } catch (error) {
 logger.error('[WithdrawalAction] createWithdrawal', error);
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
 const result = await serverHttpRequest<WithdrawalResult[]>(
 `/withdrawal/my?page=${page}&pageSize=${pageSize}`,
 {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to list my withdrawals',
 }
 );
 if (!result.ok) {
 logger.error('[WithdrawalAction] listMyWithdrawals', result.error, {
 status: result.status,
 page,
 pageSize,
 });
 return [];
 }
 return result.data;
 } catch (error) {
 logger.error('[WithdrawalAction] listMyWithdrawals', error, { page, pageSize });
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
 const result = await serverHttpRequest<WithdrawalResult[]>(
 `/admin/withdrawals/queue?page=${page}&pageSize=${pageSize}`,
 {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to list withdrawal queue',
 }
 );
 if (!result.ok) {
 logger.error('[WithdrawalAction] listWithdrawalQueue', result.error, {
 status: result.status,
 page,
 pageSize,
 });
 return [];
 }
 return result.data;
 } catch (error) {
 logger.error('[WithdrawalAction] listWithdrawalQueue', error, { page, pageSize });
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
 const result = await serverHttpRequest<unknown>('/admin/withdrawals/process', {
 method: 'POST',
 token: accessToken,
 json: data,
 fallbackErrorMessage: tApi("unknown_error"),
 });
 if (!result.ok) {
 logger.error('[WithdrawalAction] processWithdrawal', result.error, {
 status: result.status,
 withdrawalId: data.withdrawalId,
 action: data.action,
 });
 return { success: false, error: result.error || tApi("unknown_error") };
 }
 return { success: true };
 } catch (error) {
 logger.error('[WithdrawalAction] processWithdrawal', error, {
 withdrawalId: data.withdrawalId,
 action: data.action,
 });
 return { success: false, error: tApi("network_error") };
 }
}
