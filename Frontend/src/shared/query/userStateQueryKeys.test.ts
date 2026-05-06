import { describe, expect, it } from 'vitest';
import { userStateQueryKeys } from '@/shared/query/userStateQueryKeys';

describe('userStateQueryKeys', () => {
 it('builds stable query keys for every user-state surface', () => {
  expect(userStateQueryKeys.auth.session()).toEqual(['auth', 'session']);
  expect(userStateQueryKeys.wallet.balance()).toEqual(['wallet', 'balance']);
  expect(userStateQueryKeys.wallet.ledger(2)).toEqual(['wallet', 'ledger', 2]);
  expect(userStateQueryKeys.wallet.withdrawalsMine()).toEqual(['wallet', 'withdrawals', 'mine']);
  expect(userStateQueryKeys.wallet.depositPromotions()).toEqual(['wallet', 'deposit-promotions']);
  expect(userStateQueryKeys.wallet.depositPackages()).toEqual(['wallet', 'deposit-packages']);
  expect(userStateQueryKeys.wallet.depositOrder(null)).toEqual(['wallet', 'deposit-order', 'none']);
  expect(userStateQueryKeys.wallet.depositOrderHistory(1, 20, 'paid')).toEqual(['wallet', 'deposit-order-history', 1, 20, 'paid']);
  expect(userStateQueryKeys.inventory.mine()).toEqual(['inventory', 'mine']);
  expect(userStateQueryKeys.collection.mine()).toEqual(['collection', 'user']);
  expect(userStateQueryKeys.reading.cardsCatalog()).toEqual(['reading', 'cards-catalog']);
  expect(userStateQueryKeys.reading.setupSnapshot()).toEqual(['me', 'reading-setup-snapshot']);
  expect(userStateQueryKeys.reading.historyRoot()).toEqual(['reading', 'history']);
  expect(userStateQueryKeys.profile.detail()).toEqual(['profile', 'detail']);
  expect(userStateQueryKeys.profile.payoutBanks()).toEqual(['profile', 'payout-banks']);
  expect(userStateQueryKeys.profile.mfaStatus()).toEqual(['profile', 'mfa-status']);
  expect(userStateQueryKeys.reader.directoryRoot()).toEqual(['readers']);
  expect(userStateQueryKeys.reader.myRequest()).toEqual(['reader', 'my-request']);
  expect(userStateQueryKeys.reader.profile('reader-1')).toEqual(['reader-profile', 'reader-1']);
  expect(userStateQueryKeys.notifications.dropdown()).toEqual(['notifications', 'dropdown']);
  expect(userStateQueryKeys.notifications.unreadCount()).toEqual(['notifications', 'unread-count']);
  expect(userStateQueryKeys.notifications.list(3, true)).toEqual(['notifications', 'list', 3, 'unread']);
  expect(userStateQueryKeys.chat.inboxRoot()).toEqual(['chat', 'inbox']);
  expect(userStateQueryKeys.chat.inboxActive()).toEqual(['chat', 'inbox', 'active']);
  expect(userStateQueryKeys.chat.inboxPreview()).toEqual(['chat', 'inbox', 'preview']);
  expect(userStateQueryKeys.chat.unreadBadge()).toEqual(['chat', 'unread-badge']);
  expect(userStateQueryKeys.system.runtimePolicies()).toEqual(['me', 'runtime-policies']);
  expect(userStateQueryKeys.system.publicRuntimePolicies()).toEqual(['legal', 'runtime-policies']);
 });
});
