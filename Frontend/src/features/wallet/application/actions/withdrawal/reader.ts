'use server';

import { randomUUID } from 'node:crypto';
import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { WithdrawalResult } from './types';
import { AUTH_ERROR } from "@/shared/domain/authErrors";
import { AUTH_HEADER } from '@/shared/application/gateways/authConstants';
import { invokeDomainCommand } from '@/shared/application/gateways/domainCommandRegistry';

export async function createWithdrawal(data: {
 amountDiamond: number;
 userNote?: string;
 idempotencyKey?: string;
}): Promise<ActionResult<{ requestId?: string }>> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(tApi('unauthorized'));
 const idempotencyKey = (data.idempotencyKey ?? randomUUID()).trim();

 try {
  const result = await invokeDomainCommand<{ success: boolean; requestId?: string; error?: string }>(
   'wallet.withdrawal.reader.create',
   {
    path: '/withdrawal/create',
    token: accessToken,
    headers: {
     [AUTH_HEADER.IDEMPOTENCY_KEY]: idempotencyKey,
    },
    json: {
     amountDiamond: data.amountDiamond,
     userNote: data.userNote,
     idempotencyKey,
    },
    fallbackErrorMessage: tApi('unknown_error'),
   },
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
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

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
