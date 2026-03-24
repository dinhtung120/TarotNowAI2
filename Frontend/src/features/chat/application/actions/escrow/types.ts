export interface EscrowStatusResult {
 sessionId: string;
 status: string;
 totalFrozen: number;
 items: QuestionItemResult[];
}

export interface QuestionItemResult {
 id: string;
 type: string;
 amountDiamond: number;
 status: string;
 acceptedAt?: string | null;
 repliedAt?: string | null;
 autoRefundAt?: string | null;
 autoReleaseAt?: string | null;
 releasedAt?: string | null;
 disputeWindowEnd?: string | null;
}
