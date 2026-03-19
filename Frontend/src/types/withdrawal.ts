/*
 * ===================================================================
 * COMPONENT/FILE: Withdrawal Types (withdrawal.ts)
 * BỐI CẢNH (CONTEXT):
 *   Cấu trúc dữ liệu cho Luồng Rút Kim Cương (Withdrawal) ra Tiền Mặt.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - DTO `WithdrawalRequest`: Chứa thông tin Ngân hàng của User, trạng thái duyệt.
 *   - `CreateWithdrawalData`: Gói gọn đầu vào từ Client kèm theo mã MFA bắt buộc.
 *   - Được sử dụng chung bởi User (tạo yêu cầu) và Admin (xét duyệt).
 * ===================================================================
 */
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
