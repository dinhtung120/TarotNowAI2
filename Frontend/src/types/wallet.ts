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

export interface PaginatedList<T> {
    items: T[];
    pageIndex: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}
