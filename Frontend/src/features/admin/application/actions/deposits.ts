'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

export interface AdminDepositOrder {
 id: string;
 userId: string;
 username?: string;
 amountVnd: number;
 diamondAmount: number;
 status: string;
 transactionId?: string;
 createdAt: string;
}

interface ListDepositsResponse {
 deposits: AdminDepositOrder[];
 totalCount: number;
}

export async function listDeposits(
 page = 1,
 pageSize = 20,
 status = ''
): Promise<ActionResult<ListDepositsResponse>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (status) query.append('status', status);

  const result = await serverHttpRequest<{
   deposits?: AdminDepositOrder[];
   Deposits?: AdminDepositOrder[];
   totalCount?: number;
   TotalCount?: number;
  }>(`/admin/deposits?${query.toString()}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to list deposits',
  });

  if (!result.ok) {
   logger.error('[AdminAction] listDeposits', result.error, {
    status: result.status,
    page,
    pageSize,
    statusFilter: status,
   });
   return actionFail(result.error || 'Failed to list deposits');
  }

  const data = result.data;
  return actionOk({
   deposits: data.deposits || data.Deposits || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  });
 } catch (error) {
  logger.error('[AdminAction] listDeposits', error, { page, pageSize, statusFilter: status });
  return actionFail('Failed to list deposits');
 }
}

export async function processDeposit(
 depositId: string,
 action: 'approve' | 'reject',
 transactionId?: string
): Promise<ActionResult<undefined>> {
 try {
  const accessToken = await getServerAccessToken();
  if (!accessToken) {
   return actionFail(AUTH_ERROR.UNAUTHORIZED);
  }

  const normalizedTransactionId = transactionId?.trim();
  const bodyPayload: Record<string, string> = {
   depositId,
   DepositId: depositId,
   action,
   Action: action,
  };

  if (normalizedTransactionId) {
   bodyPayload.transactionId = normalizedTransactionId;
   bodyPayload.TransactionId = normalizedTransactionId;
  }

  const result = await serverHttpRequest<unknown>('/admin/deposits/process', {
   method: 'PATCH',
   token: accessToken,
   json: bodyPayload,
   fallbackErrorMessage: 'Failed to process deposit',
  });

  if (!result.ok) {
   logger.error('[AdminAction] processDeposit', result.error, {
    status: result.status,
    depositId,
    action,
   });
   return actionFail(result.error || 'Failed to process deposit');
  }

  return actionOk();
 } catch (error) {
  logger.error('[AdminAction] processDeposit', error, { depositId, action });
  return actionFail('Failed to process deposit');
 }
}
