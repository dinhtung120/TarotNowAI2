import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { WalletBalance } from '@/features/wallet/domain/types';
import { fetchJsonOrThrow } from '@/shared/application/gateways/clientFetch';

const WALLET_BALANCE_API_ROUTE = '/api/wallet/balance';

export async function getWalletBalanceClient(): Promise<ActionResult<WalletBalance>> {
 try {
  const data = await fetchJsonOrThrow<WalletBalance>(
   WALLET_BALANCE_API_ROUTE,
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   'Failed to get wallet balance.',
   8_000,
  );
  return actionOk(data);
 } catch (error) {
  return actionFail(error instanceof Error ? error.message : 'Failed to get wallet balance.');
 }
}
