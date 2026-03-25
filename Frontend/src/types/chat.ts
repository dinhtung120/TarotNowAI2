/*
 * ===================================================================
 * COMPONENT/FILE: Chat Types (chat.ts)
 * BỐI CẢNH (CONTEXT):
 *   Định nghĩa kiểu dữ liệu cho phân hệ Trò chuyện (Chat / SignalR) giữa User và Reader.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Định dạng `Conversation` (Cuộc trò chuyện) và `ChatMessage` (Tin nhắn đơn lẻ).
 *   - Hỗ trợ nhiều loại tin nhắn (`Text`, `Offer`, `System`, `Payment`).
 *   - Định nghĩa DTO cho việc Gửi tin nhắn (`SendMessageData`) và Tạo cuộc hội thoại.
 * ===================================================================
 */
import type { OffsetPaginatedList } from '@/shared/domain/pagination';

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

export type ChatPaginatedList<T> = OffsetPaginatedList<T>;

export interface CreateConversationData {
 readerId: string;
}

export interface SendMessageData {
 conversationId: string;
 content: string;
 type?: 'Text' | 'Offer' | 'System' | 'Payment';
 metadata?: string;
}
