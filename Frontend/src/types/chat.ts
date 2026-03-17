export interface Conversation {
    id: string;
    userId: string;
    readerId: string;
    userDisplayName: string;
    readerDisplayName: string;
    status: 'Active' | 'Pending' | 'Completed' | 'Cancelled' | 'Disputed';
    offerExpiresAt?: string;
    unreadCountUser: number;
    unreadCountReader: number;
    createdAt: string;
    updatedAt: string;
}

export interface ChatMessage {
    id: string;
    conversationId: string;
    senderId: string;
    type: 'Text' | 'Offer' | 'System' | 'Payment';
    content: string;
    metadata?: string;
    isRead: boolean;
    createdAt: string;
}

export interface PaginatedList<T> {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
}

export interface CreateConversationData {
    readerId: string;
}

export interface SendMessageData {
    conversationId: string;
    content: string;
    type?: 'Text' | 'Offer' | 'System' | 'Payment';
    metadata?: string;
}
