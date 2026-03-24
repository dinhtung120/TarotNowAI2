'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { ListDepositsResponse } from './types';

export async function listDepositsAdminAction(
 page: number,
 pageSize: number,
 status?: string
): Promise<ActionResult<ListDepositsResponse>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (status) query.append('status', status);

  const result = await serverHttpRequest<ListDepositsResponse>(`/admin/deposits?${query.toString()}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to list deposits',
  });

  if (!result.ok) {
   logger.error('DepositAction.listDepositsAdminAction', result.error, {
    status: result.status,
    page,
    pageSize,
    statusFilter: status,
   });
   return actionFail(result.error || 'Failed to list deposits');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('DepositAction.listDepositsAdminAction', error, {
   page,
   pageSize,
   statusFilter: status,
  });
  return actionFail('Failed to list deposits');
 }
}
