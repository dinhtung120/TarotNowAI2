'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { WithdrawalResult } from './types';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

export async function listWithdrawalQueue(
 page = 1,
 pageSize = 20
): Promise<ActionResult<WithdrawalResult[]>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

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
   return actionFail(result.error || 'Failed to list withdrawal queue');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[WithdrawalAction] listWithdrawalQueue', error, { page, pageSize });
  return actionFail('Failed to list withdrawal queue');
 }
}

export async function processWithdrawal(data: {
 withdrawalId: string;
 action: 'approve' | 'reject';
 adminNote?: string;
 mfaCode: string;
}): Promise<ActionResult<undefined>> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(tApi('unauthorized'));

 try {
  const result = await serverHttpRequest<unknown>('/admin/withdrawals/process', {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });
  if (!result.ok) {
   logger.error('[WithdrawalAction] processWithdrawal', result.error, {
    status: result.status,
    withdrawalId: data.withdrawalId,
    action: data.action,
   });
   return actionFail(result.error || tApi('unknown_error'));
  }
  return actionOk();
 } catch (error) {
  logger.error('[WithdrawalAction] processWithdrawal', error, {
   withdrawalId: data.withdrawalId,
   action: data.action,
  });
  return actionFail(tApi('network_error'));
 }
}
