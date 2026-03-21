'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { ListDepositsResponse } from './types';

export async function listDepositsAdminAction(
 page: number,
 pageSize: number,
 status?: string
): Promise<ListDepositsResponse | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

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
   return null;
  }

  return result.data;
 } catch (error) {
  logger.error('DepositAction.listDepositsAdminAction', error, {
   page,
   pageSize,
   statusFilter: status,
  });
  return null;
 }
}
