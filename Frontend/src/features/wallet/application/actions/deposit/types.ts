export interface CreateDepositOrderResponse {
 orderId: string;
 paymentUrl: string;
 amountVnd: number;
 diamondAmount: number;
}

export interface DepositOrder {
 id: string;
 userId: string;
 username: string;
 amountVnd: number;
 diamondAmount: number;
 status: string;
 transactionId?: string;
 createdAt: string;
 completedAt?: string;
}

export interface ListDepositsResponse {
 deposits: DepositOrder[];
 totalCount: number;
}
