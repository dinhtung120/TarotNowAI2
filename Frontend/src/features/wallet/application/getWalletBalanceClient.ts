import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { WalletBalance } from '@/features/wallet/domain/types';

const WALLET_BALANCE_API_ROUTE = '/api/wallet/balance';

export async function getWalletBalanceClient(): Promise<ActionResult<WalletBalance>> {
 try {
  const response = await fetch(WALLET_BALANCE_API_ROUTE, {
   method: 'GET',
   credentials: 'include',
   cache: 'no-store',
  });

  if (!response.ok) {
   const message = await parseApiError(response, 'Failed to get wallet balance.');
   return actionFail(message);
  }

  const data = (await response.json()) as WalletBalance;
  return actionOk(data);
 } catch {
  return actionFail('Failed to get wallet balance.');
 }
}
