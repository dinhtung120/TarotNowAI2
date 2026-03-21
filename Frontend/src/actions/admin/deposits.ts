'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

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

export interface ListDepositsResponse {
 deposits: AdminDepositOrder[];
 totalCount: number;
}

export async function listDeposits(page = 1, pageSize = 20, status = ''): Promise<ListDepositsResponse | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

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
   return null;
  }

  const data = result.data;
  return {
   deposits: data.deposits || data.Deposits || [],
   totalCount: data.totalCount ?? data.TotalCount ?? 0,
  };
 } catch (error) {
  logger.error('[AdminAction] listDeposits', error, { page, pageSize, statusFilter: status });
  return null;
 }
}

export async function processDeposit(
 depositId: string,
 action: 'approve' | 'reject',
 transactionId?: string
): Promise<boolean> {
 try {
  const accessToken = await getAccessToken();
  if (!accessToken) {
   return false;
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
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[AdminAction] processDeposit', error, { depositId, action });
  return false;
 }
}
