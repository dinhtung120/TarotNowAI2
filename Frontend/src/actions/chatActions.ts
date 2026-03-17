'use server';

import { cookies } from 'next/headers';

// Base URL của Backend API
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

// ======================================================================
// Helper — lấy token từ HttpOnly cookie
// ======================================================================

async function getAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 return cookieStore.get('accessToken')?.value;
}

// ======================================================================
// Types — phải match backend DTOs
// ======================================================================

/** Conversation header */
export interface ConversationDto {
 id: string;
 userId: string;
 readerId: string;
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
 const response = await fetch(`${API_URL}/conversations`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({ readerId }),
 });

 if (!response.ok) return null;
 return await response.json();
 } catch (error) {
 console.error('[ChatAction] createConversation failed:', error);
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
 const url = new URL(`${API_URL}/conversations`);
 url.searchParams.append('role', role);
 url.searchParams.append('page', page.toString());
 url.searchParams.append('pageSize', pageSize.toString());

 const response = await fetch(url.toString(), {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) return null;
 return await response.json();
 } catch (error) {
 console.error('[ChatAction] listConversations failed:', error);
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
 const url = new URL(`${API_URL}/conversations/${conversationId}/messages`);
 url.searchParams.append('page', page.toString());
 url.searchParams.append('pageSize', pageSize.toString());

 const response = await fetch(url.toString(), {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) return null;
 return await response.json();
 } catch (error) {
 console.error('[ChatAction] listMessages failed:', error);
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
 const response = await fetch(`${API_URL}/reports`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify(data),
 });

 return response.ok;
 } catch (error) {
 console.error('[ChatAction] sendReport failed:', error);
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
