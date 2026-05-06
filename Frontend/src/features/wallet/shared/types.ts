import type { IndexPaginatedList } from '@/features/wallet/shared/models/pagination';

export interface WalletBalance {
 goldBalance: number;
 diamondBalance: number;
 frozenDiamondBalance: number;
}

export interface WalletTransaction {
 id: string;
 currency: string;
 type: string;
 amount: number;
 balanceBefore: number;
 balanceAfter: number;
 description?: string;
 createdAt: string;
}

export type WalletPaginatedList<T> = IndexPaginatedList<T>;
