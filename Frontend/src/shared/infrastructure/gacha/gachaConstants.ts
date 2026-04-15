export const GACHA_IDEMPOTENCY_HEADER = 'x-idempotency-key';

export const GACHA_API_ROUTES = {
 pools: '/api/gacha/pools',
 poolOdds: (poolCode: string) => `/api/gacha/pools/${encodeURIComponent(poolCode)}/odds`,
 history: '/api/gacha/history',
 pull: '/api/gacha/pull',
};

export const gachaQueryKeys = {
  all: ['gacha'] as const,
  pools: () => [...gachaQueryKeys.all, 'pools'] as const,
  poolOdds: (poolCode: string) => [...gachaQueryKeys.all, 'odds', poolCode] as const,
  history: (page: number, pageSize: number) => [...gachaQueryKeys.all, 'history', page, pageSize] as const,
};

export const gachaRewardKinds = {
 currency: 'currency',
 item: 'item',
} as const;
