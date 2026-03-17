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
