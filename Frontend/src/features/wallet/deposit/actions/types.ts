export type DepositOrderStatus = 'pending' | 'success' | 'failed';

export interface DepositPackageResponse {
 code: string;
 amountVnd: number;
 baseDiamondAmount: number;
 bonusGoldAmount: number;
 totalDiamondAmount: number;
}

export interface CreateDepositOrderResponse {
 orderId: string;
 status: DepositOrderStatus;
 amountVnd: number;
 paymentUrl?: string;
 diamondAmount?: number;
 baseDiamondAmount: number;
 bonusGoldAmount: number;
 totalDiamondAmount: number;
 payOsOrderCode: number;
 checkoutUrl: string;
 qrCode: string;
 paymentLinkId: string;
 expiresAtUtc?: string | null;
}

export interface MyDepositOrderResponse {
 orderId: string;
 status: DepositOrderStatus;
 packageCode: string;
 amountVnd: number;
 baseDiamondAmount: number;
 bonusGoldAmount: number;
 totalDiamondAmount: number;
 payOsOrderCode: number;
 checkoutUrl: string;
 qrCode: string;
 paymentLinkId: string;
 transactionId?: string | null;
 failureReason?: string | null;
 processedAt?: string | null;
 expiresAtUtc?: string | null;
}

export interface MyDepositOrderHistoryItemResponse {
 orderId: string;
 status: DepositOrderStatus;
 packageCode: string;
 amountVnd: number;
 baseDiamondAmount: number;
 totalDiamondAmount: number;
 bonusGoldAmount: number;
 transactionId?: string | null;
 failureReason?: string | null;
 createdAt: string;
 processedAt?: string | null;
}

export interface MyDepositOrderHistoryResponse {
 items: MyDepositOrderHistoryItemResponse[];
 totalCount: number;
 page: number;
 pageSize: number;
 totalPages: number;
}
