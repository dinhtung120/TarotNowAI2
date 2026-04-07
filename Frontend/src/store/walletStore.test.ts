import { beforeEach, describe, expect, it } from 'vitest';
import { setWalletBalanceFetcher, useWalletStore } from '@/store/walletStore';

describe('walletStore snapshot stability', () => {
 beforeEach(() => {
  setWalletBalanceFetcher(undefined);
  useWalletStore.getState().resetWallet();
 });

 it('keeps a cached snapshot reference when state does not change', () => {
  const first = useWalletStore.getState();
  const second = useWalletStore.getState();
  expect(second).toBe(first);
 });

 it('replaces snapshot reference only when state changes', () => {
  const before = useWalletStore.getState();
  before.setBalance({
   goldBalance: 10,
   diamondBalance: 20,
   frozenDiamondBalance: 5,
  });

  const after = useWalletStore.getState();
  expect(after).not.toBe(before);
  expect(after.balance?.diamondBalance).toBe(20);

  const afterAgain = useWalletStore.getState();
  expect(afterAgain).toBe(after);
 });
});
