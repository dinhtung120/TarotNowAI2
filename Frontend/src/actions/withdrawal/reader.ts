'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { WithdrawalResult } from './types';

const getAccessToken = getServerAccessToken;

export async function createWithdrawal(data: {
 amountDiamond: number;
 bankName: string;
 bankAccountName: string;
 bankAccountNumber: string;
 mfaCode: string;
}): Promise<{ success: boolean; requestId?: string; error?: string }> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi('unauthorized') };

 try {
  const result = await serverHttpRequest<{ success: boolean; requestId?: string; error?: string }>(
   '/withdrawal/create',
   {
    method: 'POST',
    token: accessToken,
    json: data,
    fallbackErrorMessage: tApi('unknown_error'),
   }
  );
  if (!result.ok) {
   logger.error('[WithdrawalAction] createWithdrawal', result.error, { status: result.status });
   return { success: false, error: result.error || tApi('unknown_error') };
  }
  return result.data;
 } catch (error) {
  logger.error('[WithdrawalAction] createWithdrawal', error);
  return { success: false, error: tApi('network_error') };
 }
}

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
