'use server';

import { randomUUID } from 'node:crypto';
import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import type { WithdrawalDetailResult, WithdrawalResult } from './types';
import { AUTH_ERROR } from "@/shared/models/authErrors";
import { AUTH_HEADER } from '@/shared/gateways/authConstants';
import { invokeDomainCommand } from '@/shared/gateways/domainCommandRegistry';

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
 idempotencyKey?: string;
}): Promise<ActionResult<undefined>> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(tApi('unauthorized'));
 const idempotencyKey = (data.idempotencyKey ?? randomUUID()).trim();

 try {
  const result = await invokeDomainCommand<unknown>('wallet.withdrawal.admin.process', {
   path: '/admin/withdrawals/process',
   token: accessToken,
   headers: {
    [AUTH_HEADER.IDEMPOTENCY_KEY]: idempotencyKey,
   },
   json: {
    withdrawalId: data.withdrawalId,
    action: data.action,
    adminNote: data.adminNote,
    idempotencyKey,
   },
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

export async function getWithdrawalDetail(withdrawalId: string): Promise<ActionResult<WithdrawalDetailResult>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<WithdrawalDetailResult>(
   `/admin/withdrawals/${withdrawalId}`,
   {
    method: 'GET',
    token: accessToken,
    fallbackErrorMessage: 'Failed to get withdrawal detail',
   },
  );

  if (!result.ok) {
   logger.error('[WithdrawalAction] getWithdrawalDetail', result.error, {
    status: result.status,
    withdrawalId,
   });
   return actionFail(result.error || 'Failed to get withdrawal detail');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[WithdrawalAction] getWithdrawalDetail', error, { withdrawalId });
  return actionFail('Failed to get withdrawal detail');
 }
}
