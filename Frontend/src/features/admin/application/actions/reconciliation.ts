'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';

export interface MismatchRecord {
 userId: string;
 userBalance: number;
 ledgerBalance: number;
 difference: number;
}

export async function getWalletMismatches(): Promise<ActionResult<MismatchRecord[]>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>('/admin/reconciliation/wallet', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get wallet mismatches',
  });

  if (!result.ok) {
   logger.error('[AdminAction] getWalletMismatches', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to get wallet mismatches');
  }

  const data = result.data;
  if (Array.isArray(data)) {
   return actionOk(data.map((d: Record<string, unknown>) => ({
    userId: String(d.userId ?? d.UserId ?? ''),
    userBalance: Number(d.userBalance ?? d.UserBalance ?? 0),
    ledgerBalance: Number(d.ledgerBalance ?? d.LedgerBalance ?? 0),
    difference: Number(d.difference ?? d.Difference ?? 0),
   })));
  }

  return actionOk(data as MismatchRecord[]);
 } catch (error) {
  logger.error('[AdminAction] getWalletMismatches', error);
  return actionFail('Failed to get wallet mismatches');
 }
}
