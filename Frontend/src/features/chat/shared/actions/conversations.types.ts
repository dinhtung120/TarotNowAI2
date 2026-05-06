export type InboxTab = 'active' | 'pending' | 'completed' | 'all';
export type CompletionStatus = 'pending' | 'awaiting_acceptance' | 'ongoing' | 'completed' | 'cancelled' | 'expired' | 'disputed';

export interface ConversationConfirmDto {
 userAt?: string | null;
 readerAt?: string | null;
 requestedBy?: string | null;
 requestedAt?: string | null;
 autoResolveAt?: string | null;
}

export interface ConversationDto {
 id: string; userId: string; readerId: string; userName?: string | null; userAvatar?: string | null; readerName?: string | null; readerAvatar?: string | null;
 readerStatus?: string | null; escrowTotalFrozen?: number; escrowStatus?: string | null; status: CompletionStatus; lastMessageAt?: string | null; lastMessagePreview?: string | null;
 offerExpiresAt?: string | null; slaHours?: number; confirm?: ConversationConfirmDto | null; unreadCountUser: number; unreadCountReader: number;
 canSubmitReview?: boolean; hasSubmittedReview?: boolean; reviewedAt?: string | null; createdAt: string; updatedAt?: string | null
}

export interface MediaPayloadDto {
 url: string; objectKey?: string | null; uploadToken?: string | null; mimeType?: string | null; sizeBytes?: number | null; durationMs?: number | null; width?: number | null; height?: number | null;
 thumbnailUrl?: string | null; description?: string | null; processingStatus?: string | null
}

export interface ChatMessageDto {
 id: string; conversationId: string; senderId: string; type: string; content: string;
 clientMessageId?: string | null;
 localStatus?: 'sending' | 'sent' | 'failed';
 paymentPayload?: { amountDiamond: number; proposalId?: string; expiresAt?: string; description?: string } | null;
 mediaPayload?: MediaPayloadDto | null; isRead: boolean; createdAt: string
}

export interface ListConversationsResult { conversations: ConversationDto[]; totalCount: number; currentUserId: string }
export interface ListMessagesResult { messages: ChatMessageDto[]; nextCursor?: string | null; conversation?: ConversationDto }

export interface AdminDisputeItemDto {
 id: string; financeSessionId: string; payerId: string; receiverId: string; amountDiamond: number; status: string; createdAt: string; updatedAt?: string | null
}

export interface ListAdminDisputesResult { items: AdminDisputeItemDto[]; totalCount: number; page: number; pageSize: number }
