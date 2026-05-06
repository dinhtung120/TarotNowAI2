import { afterEach, beforeEach, describe, expect, it } from 'vitest';
import { QueryClient } from '@tanstack/react-query';
import type { WalletBalance } from '@/features/wallet/shared/types';
import { actionFail, actionOk } from '@/shared/models/actionResult';
import { userStateQueryKeys } from '@/shared/query/userStateQueryKeys';
import {
 registerWalletQueryBridge,
 setWalletBalanceFetcher,
 useWalletStore,
} from '@/features/wallet/shared/walletStore';

const WALLET_BALANCE_QUERY_KEY = userStateQueryKeys.wallet.balance();

const SAMPLE_BALANCE: WalletBalance = {
 goldBalance: 1000,
 diamondBalance: 500,
 frozenDiamondBalance: 20,
};

describe('walletStore', () => {
 beforeEach(() => {
  registerWalletQueryBridge(null);
  setWalletBalanceFetcher();
  useWalletStore.getState().resetWallet();
 });

 afterEach(() => {
  registerWalletQueryBridge(null);
  setWalletBalanceFetcher();
  useWalletStore.getState().resetWallet();
 });

 it('returns an error when wallet fetcher is not configured', async () => {
  const result = await useWalletStore.getState().fetchBalance();

  expect(result).toBeNull();
  expect(useWalletStore.getState().error).toBe('Wallet balance fetcher is not configured');
 });

 it('fetches wallet balance and syncs query cache when bridge is active', async () => {
  const queryClient = new QueryClient();
  registerWalletQueryBridge(queryClient);
  setWalletBalanceFetcher(async () => actionOk(SAMPLE_BALANCE));

  const result = await useWalletStore.getState().fetchBalance();

  expect(result).toEqual(SAMPLE_BALANCE);
  expect(useWalletStore.getState().balance).toEqual(SAMPLE_BALANCE);
  expect(queryClient.getQueryData<WalletBalance | null>(WALLET_BALANCE_QUERY_KEY)).toEqual(SAMPLE_BALANCE);
 });

 it('fetches wallet balance without query bridge', async () => {
  setWalletBalanceFetcher(async () => actionOk(SAMPLE_BALANCE));

  const result = await useWalletStore.getState().fetchBalance();

  expect(result).toEqual(SAMPLE_BALANCE);
  expect(useWalletStore.getState().balance).toEqual(SAMPLE_BALANCE);
 });

 it('keeps previous balance when fetch fails', async () => {
  useWalletStore.getState().setBalance(SAMPLE_BALANCE);
  setWalletBalanceFetcher(async () => actionFail('fetch failed'));

  const result = await useWalletStore.getState().fetchBalance();

  expect(result).toBeNull();
  expect(useWalletStore.getState().balance).toEqual(SAMPLE_BALANCE);
  expect(useWalletStore.getState().error).toBe('fetch failed');
 });

 it('syncs store balance when query cache balance changes', () => {
  const queryClient = new QueryClient();
  registerWalletQueryBridge(queryClient);
  queryClient.setQueryData(WALLET_BALANCE_QUERY_KEY, SAMPLE_BALANCE);

  expect(useWalletStore.getState().balance).toEqual(SAMPLE_BALANCE);
 });
});
