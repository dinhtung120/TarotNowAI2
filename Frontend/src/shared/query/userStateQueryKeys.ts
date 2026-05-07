export const userStateQueryKeys = {
 auth: {
  all: ['auth'] as const,
  session: () => [...userStateQueryKeys.auth.all, 'session'] as const,
 },
 wallet: {
  all: ['wallet'] as const,
  balance: () => [...userStateQueryKeys.wallet.all, 'balance'] as const,
  ledger: (page: number) => [...userStateQueryKeys.wallet.all, 'ledger', page] as const,
  withdrawalsMine: () => [...userStateQueryKeys.wallet.all, 'withdrawals', 'mine'] as const,
  depositPromotions: () => [...userStateQueryKeys.wallet.all, 'deposit-promotions'] as const,
  depositPackages: () => [...userStateQueryKeys.wallet.all, 'deposit-packages'] as const,
  depositOrder: (orderId: string | null) =>
   [...userStateQueryKeys.wallet.all, 'deposit-order', orderId ?? 'none'] as const,
  depositOrderHistory: (page: number, pageSize: number, status: string) =>
   [...userStateQueryKeys.wallet.all, 'deposit-order-history', page, pageSize, status] as const,
 },
 inventory: {
  all: ['inventory'] as const,
  mine: () => [...userStateQueryKeys.inventory.all, 'mine'] as const,
 },
 collection: {
  all: ['collection'] as const,
  mine: () => [...userStateQueryKeys.collection.all, 'user'] as const,
  catalog: {
   manifest: () => [...userStateQueryKeys.collection.all, 'catalog', 'manifest'] as const,
   chunk: (version: string, chunkId: number) =>
    [...userStateQueryKeys.collection.all, 'catalog', 'chunk', version, chunkId] as const,
   detail: (version: string, cardId: number) =>
    [...userStateQueryKeys.collection.all, 'catalog', 'detail', version, cardId] as const,
  },
 },
 reading: {
  cardsCatalog: () => ['reading', 'cards-catalog'] as const,
  setupSnapshot: () => ['me', 'reading-setup-snapshot'] as const,
  historyRoot: () => ['reading', 'history'] as const,
 },
 profile: {
  detail: () => ['profile', 'detail'] as const,
  payoutBanks: () => ['profile', 'payout-banks'] as const,
  mfaStatus: () => ['profile', 'mfa-status'] as const,
 },
 reader: {
  directoryRoot: () => ['readers'] as const,
  myRequest: () => ['reader', 'my-request'] as const,
  profile: (readerId: string) => ['reader-profile', readerId] as const,
 },
 notifications: {
  all: ['notifications'] as const,
  dropdown: () => ['notifications', 'dropdown'] as const,
  unreadCount: () => ['notifications', 'unread-count'] as const,
  list: (page: number, unreadOnly: boolean) =>
   ['notifications', 'list', page, unreadOnly ? 'unread' : 'all'] as const,
 },
 chat: {
  inboxRoot: () => ['chat', 'inbox'] as const,
  inbox: (tab: string) => ['chat', 'inbox', tab] as const,
  inboxActive: () => ['chat', 'inbox', 'active'] as const,
  inboxPreview: () => ['chat', 'inbox', 'preview'] as const,
  unreadBadge: () => ['chat', 'unread-badge'] as const,
 },
 system: {
  runtimePolicies: () => ['me', 'runtime-policies'] as const,
  publicRuntimePolicies: () => ['legal', 'runtime-policies'] as const,
 },
} as const;
