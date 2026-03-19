/*
 * ===================================================================
 * COMPONENT/FILE: Escrow Types (escrow.ts)
 * BỐI CẢNH (CONTEXT):
 *   Định nghĩa dữ liệu cho Giao dịch Trung gian (Escrow) - Nút thắt tài chính của TarotNow.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Gồm `EscrowSession` (Phiên tổng) chứa tổng số khoá/mở/hoàn và `EscrowQuestionItem` (Từng câu hỏi).
 *   - Hỗ trợ phân định các trạng thái Escrow cực kỳ chặt chẽ (Frozen, Accepted, Released...).
 *   - Chứa DTO của các nút Action như: Chấp nhận Offer, Thêm câu hỏi phụ, Mở tranh chấp.
 * ===================================================================
 */
export interface EscrowSession {
 id: string;
 conversationId: string;
 totalFrozen: number;
 totalReleased: number;
 totalRefunded: number;
 createdAt: string;
 updatedAt: string;
}

export interface EscrowQuestionItem {
 id: string;
 financeSessionId: string;
 amountDiamond: number;
 status: 'Frozen' | 'Accepted' | 'Released' | 'Refunded' | 'Disputed';
 proposalMessageRef?: string;
 repliedAt?: string;
 confirmedAt?: string;
 autoReleaseAt?: string;
 autoRefundAt?: string;
 disputeWindowEnd?: string;
 createdAt: string;
}

export interface EscrowStatus {
 session: EscrowSession | null;
 items: EscrowQuestionItem[];
}

export interface AcceptOfferData {
 readerId: string;
 conversationRef: string;
 amountDiamond: number;
 proposalMessageRef?: string;
 idempotencyKey: string;
}

export interface AddQuestionData {
 conversationRef: string;
 amountDiamond: number;
 proposalMessageRef?: string;
 idempotencyKey: string;
}

export interface OpenDisputeData {
 itemId: string;
 reason: string;
}
