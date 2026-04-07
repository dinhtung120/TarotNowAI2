'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { WithdrawalResult } from './types';

export async function createWithdrawal(data: {
 amountDiamond: number;
 bankName: string;
 bankAccountName: string;
 bankAccountNumber: string;
 mfaCode: string;
}): Promise<ActionResult<{ requestId?: string }>> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(tApi('unauthorized'));

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
   return actionFail(result.error || tApi('unknown_error'));
  }
  if (!result.data.success) {
   return actionFail(result.data.error || tApi('unknown_error'));
  }
  return actionOk({ requestId: result.data.requestId });
 } catch (error) {
  logger.error('[WithdrawalAction] createWithdrawal', error);
  return actionFail(tApi('network_error'));
 }
}

export async function listMyWithdrawals(
 page = 1,
 pageSize = 20
): Promise<ActionResult<WithdrawalResult[]>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

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
   return actionFail(result.error || 'Failed to list my withdrawals');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[WithdrawalAction] listMyWithdrawals', error, { page, pageSize });
  return actionFail('Failed to list my withdrawals');
 }
}
