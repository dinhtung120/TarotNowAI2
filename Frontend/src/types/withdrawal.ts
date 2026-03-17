export interface WithdrawalRequest {
 id: string;
 userId: string;
 amountDiamond: number;
 status: 'Pending' | 'Approved' | 'Rejected';
 bankName: string;
 bankAccountName: string;
 bankAccountNumber: string;
 createdAt: string;
 processedAt?: string;
 processedByAdminId?: string;
}

export interface CreateWithdrawalData {
 amountDiamond: number;
 mfaCode: string;
 bankName: string;
 bankAccountName: string;
 bankAccountNumber: string;
}

export interface ProcessWithdrawalData {
 requestId: string;
 action: 'approve' | 'reject';
 mfaCode: string;
}
