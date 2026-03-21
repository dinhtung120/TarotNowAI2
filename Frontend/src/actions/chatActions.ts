/*
 * ===================================================================
 * FILE: chatActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Cầu nối Server Action cho chức năng Trò chuyện trực tiếp giữa Khách và Reader.
 *
 * CHU TRÌNH XỬ LÝ:
 *   - Tạo Phòng Chat (Create Conversation) an toàn từ phía Server.
 *   - Trích xuất Lịch sử Tin nhắn để kết xuất trước (SSR/SSG).
 *   - Kéo Token uỷ quyền cho giao thức SignalR Websockets.
 * ===================================================================
 */
'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

// ======================================================================
// Helper — lấy token từ HttpOnly cookie
// ======================================================================

const getAccessToken = getServerAccessToken;

// ======================================================================
// Types — phải match backend DTOs
// ======================================================================

/** Conversation header */
export interface ConversationDto {
 id: string;
 userId: string;
 readerId: string;
 // --- CÁC TRƯỜNG BỔ SUNG ĐỂ HIỂN THỊ TÊN/AVATAR TRÊN GIAO DIỆN ---
 userName?: string | null;
 userAvatar?: string | null;
 readerName?: string;
 readerAvatar?: string;
 // --- ESCROW INFO ---
 escrowTotalFrozen?: number;
 escrowStatus?: string;
 // -------------------------------------------------------------
 status: string;
 lastMessageAt?: string | null;
 unreadCountUser: number;
 unreadCountReader: number;
 createdAt: string;
 updatedAt?: string | null;
}

/** Kết quả inbox */
export interface ListConversationsResult {
 conversations: ConversationDto[];
 totalCount: number;
 currentUserId: string;
}

/** Tin nhắn chat */
export interface ChatMessageDto {
 id: string;
 conversationId: string;
 senderId: string;
 type: string;
 content: string;
 paymentPayload?: {
 amountDiamond: number;
 proposalId?: string;
 expiresAt?: string;
 } | null;
 isRead: boolean;
 createdAt: string;
}

/** Kết quả lịch sử chat */
export interface ListMessagesResult {
 messages: ChatMessageDto[];
 totalCount: number;
 conversation?: ConversationDto;
}

// ======================================================================
// Server Actions
// ======================================================================

/**
 * Tạo conversation mới — hoặc trả về conversation active nếu đã có.
 *
 * @param readerId - UUID reader muốn chat.
 * @returns ConversationDto hoặc null nếu lỗi.
 */
export async function createConversation(
 readerId: string
): Promise<ConversationDto | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const result = await serverHttpRequest<ConversationDto>('/conversations', {
 method: 'POST',
 token: accessToken,
 json: { readerId },
 fallbackErrorMessage: 'Failed to create conversation',
 });

 if (!result.ok) {
 logger.error('[ChatAction] createConversation', result.error, { status: result.status, readerId });
 return null;
 }
 return result.data;
 } catch (error) {
 logger.error('[ChatAction] createConversation', error, { readerId });
 return null;
 }
}

/**
 * Lấy danh sách conversations (inbox).
 *
 * @param role - "user" hoặc "reader".
 * @param page - Trang.
 * @param pageSize - Số items mỗi trang.
 */
export async function listConversations(
 role = 'user',
 page = 1,
 pageSize = 20
): Promise<ListConversationsResult | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const query = new URLSearchParams({
 role,
 page: page.toString(),
 pageSize: pageSize.toString(),
 });

 const result = await serverHttpRequest<ListConversationsResult>(
 `/conversations?${query.toString()}`,
 {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to list conversations',
 }
 );

 if (!result.ok) {
 logger.error('[ChatAction] listConversations', result.error, {
 status: result.status,
 role,
 page,
 pageSize,
 });
 return null;
 }
 return result.data;
 } catch (error) {
 logger.error('[ChatAction] listConversations', error, { role, page, pageSize });
 return null;
 }
}

/**
 * Lấy lịch sử tin nhắn trong conversation.
 *
 * @param conversationId - ObjectId conversation.
 */
export async function listMessages(
 conversationId: string,
 page = 1,
 pageSize = 50
): Promise<ListMessagesResult | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const query = new URLSearchParams({
 page: page.toString(),
 pageSize: pageSize.toString(),
 });

 const result = await serverHttpRequest<ListMessagesResult>(
 `/conversations/${conversationId}/messages?${query.toString()}`,
 {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to list messages',
 }
 );
 if (!result.ok) {
 logger.error('[ChatAction] listMessages', result.error, {
 status: result.status,
 conversationId,
 page,
 pageSize,
 });
 return null;
 }
 return result.data;
 } catch (error) {
 logger.error('[ChatAction] listMessages', error, { conversationId, page, pageSize });
 return null;
 }
}

/**
 * Gửi báo cáo vi phạm.
 */
export async function sendReport(data: {
 targetType: string;
 targetId: string;
 conversationRef?: string;
 reason: string;
}): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/reports', {
 method: 'POST',
 token: accessToken,
 json: data,
 fallbackErrorMessage: 'Failed to send report',
 });

 if (!result.ok) {
 logger.error('[ChatAction] sendReport', result.error, {
 status: result.status,
 targetType: data.targetType,
 targetId: data.targetId,
 });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('[ChatAction] sendReport', error, {
 targetType: data.targetType,
 targetId: data.targetId,
 });
 return false;
 }
}

/**
 * Lấy access token cho SignalR client (client-side cần token để connect).
 * Server action trả về token từ HttpOnly cookie.
 */
export async function getSignalRToken(): Promise<string | null> {
 const token = await getAccessToken();
 return token || null;
}
