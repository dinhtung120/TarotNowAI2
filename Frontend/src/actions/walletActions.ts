/*
 * ===================================================================
 * FILE: walletActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Truy vấn Lịch sử giao dịch (Ledger) và Số dư khả dụng (Balance).
 *
 * LƯU Ý CACHE:
 *   Luôn sử dụng `cache: 'no-store'` vì số dư ví là dữ liệu nhạy cảm
 *   cần độ chính xác tuyệt đối ở thời gian thực.
 * ===================================================================
 */
'use server';

import { PaginatedList, WalletBalance, WalletTransaction } from '@/types/wallet';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

export async function getWalletBalance(): Promise<WalletBalance | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

 try {
 const result = await serverHttpRequest<WalletBalance>('/Wallet/balance', {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to get wallet balance',
 });
 if (!result.ok) {
 logger.error('WalletAction.getWalletBalance', result.error, { status: result.status });
 return null;
 }
 return result.data;
 } catch (error) {
 logger.error('WalletAction.getWalletBalance', error);
 return null;
 }
}

export async function getLedger(page = 1, limit = 20): Promise<PaginatedList<WalletTransaction> | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

 try {
 const result = await serverHttpRequest<PaginatedList<WalletTransaction>>(
 `/Wallet/ledger?page=${page}&limit=${limit}`,
 {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to get ledger',
 }
 );
 if (!result.ok) {
 logger.error('WalletAction.getLedger', result.error, { status: result.status, page, limit });
 return null;
 }
 return result.data;
 } catch (error) {
 logger.error('WalletAction.getLedger', error, { page, limit });
 return null;
 }
}
