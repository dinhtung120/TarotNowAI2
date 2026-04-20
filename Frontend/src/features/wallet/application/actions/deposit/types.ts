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
