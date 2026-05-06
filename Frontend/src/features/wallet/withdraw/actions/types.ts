export interface WithdrawalResult {
 id: string;
 userId: string;
 amountDiamond: number;
 amountVnd: number;
 feeVnd: number;
 netAmountVnd: number;
 bankName: string;
 bankBin: string;
 bankAccountName: string;
 bankAccountNumber: string;
 status: string;
 userNote?: string | null;
 adminNote?: string | null;
 processedAt?: string | null;
 createdAt: string;
}

export interface WithdrawalDetailResult extends WithdrawalResult {
 transferContent: string;
 vietQrImageUrl: string;
}
