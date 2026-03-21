/*
 * ===================================================================
 * FILE: escrowActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Quản lý các thao tác Giao Dịch Trung Gian (Escrow) ngay trong màn hình Chat.
 *   Xử lý việc Chấp Nhận Thanh Toán (Accept Offer), Reader Trả Lời (Reply),
 *   Xác Nhận Nhận Hàng (Confirm Release) và Khiếu Nại (Dispute).
 *
 * KIẾN TRÚC:
 *   Server Action gọi thẳng API Backend, giấu token, an toàn và bảo mật.
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
// Types — phải match backend response
// ======================================================================

/** Kết quả trạng thái escrow */
export interface EscrowStatusResult {
 sessionId: string;
 status: string;
 totalFrozen: number;
 items: QuestionItemResult[];
}

/** Chi tiết từng question item */
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

// ======================================================================
// Server Actions
// ======================================================================

/**
 * Accept offer — freeze diamond, tạo escrow.
 * Gọi khi user accept payment_offer message.
 */
export async function acceptOffer(data: {
 readerId: string;
 conversationRef: string;
 amountDiamond: number;
 proposalMessageRef?: string;
 idempotencyKey: string;
}): Promise<{ success: boolean; itemId?: string }> {
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false };

 try {
 const result = await serverHttpRequest<{ success: boolean; itemId?: string }>('/escrow/accept', {
 method: 'POST',
 token: accessToken,
 json: data,
 fallbackErrorMessage: 'Failed to accept offer',
 });
 if (!result.ok) {
 logger.error('[EscrowAction] acceptOffer', result.error, { status: result.status });
 return { success: false };
 }
 return result.data;
 } catch (error) {
 logger.error('[EscrowAction] acceptOffer', error);
 return { success: false };
 }
}

/**
 * Reader reply — set auto_release_at = +24h.
 */
export async function readerReply(itemId: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/escrow/reply', {
 method: 'POST',
 token: accessToken,
 json: { itemId },
 fallbackErrorMessage: 'Failed to send reader reply',
 });
 if (!result.ok) {
 logger.error('[EscrowAction] readerReply', result.error, { status: result.status, itemId });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('[EscrowAction] readerReply', error, { itemId });
 return false;
 }
}

/**
 * User confirm release — diamond chuyển cho reader (- 10% fee).
 */
export async function confirmRelease(itemId: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/escrow/confirm', {
 method: 'POST',
 token: accessToken,
 json: { itemId },
 fallbackErrorMessage: 'Failed to confirm release',
 });
 if (!result.ok) {
 logger.error('[EscrowAction] confirmRelease', result.error, { status: result.status, itemId });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('[EscrowAction] confirmRelease', error, { itemId });
 return false;
 }
}

/**
 * Mở tranh chấp — validate dispute window.
 */
export async function openDispute(itemId: string, reason: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/escrow/dispute', {
 method: 'POST',
 token: accessToken,
 json: { itemId, reason },
 fallbackErrorMessage: 'Failed to open dispute',
 });
 if (!result.ok) {
 logger.error('[EscrowAction] openDispute', result.error, {
 status: result.status,
 itemId,
 });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('[EscrowAction] openDispute', error, { itemId });
 return false;
 }
}

/**
 * Thêm câu hỏi — freeze thêm diamond.
 */
export async function addQuestion(data: {
 conversationRef: string;
 amountDiamond: number;
 proposalMessageRef?: string;
 idempotencyKey: string;
}): Promise<{ success: boolean; itemId?: string }> {
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false };

 try {
 const result = await serverHttpRequest<{ success: boolean; itemId?: string }>('/escrow/add-question', {
 method: 'POST',
 token: accessToken,
 json: data,
 fallbackErrorMessage: 'Failed to add question',
 });
 if (!result.ok) {
 logger.error('[EscrowAction] addQuestion', result.error, { status: result.status });
 return { success: false };
 }
 return result.data;
 } catch (error) {
 logger.error('[EscrowAction] addQuestion', error);
 return { success: false };
 }
}

/**
 * Lấy trạng thái escrow.
 */
export async function getEscrowStatus(conversationId: string): Promise<EscrowStatusResult | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
 const result = await serverHttpRequest<EscrowStatusResult>(`/escrow/${conversationId}`, {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to get escrow status',
 });
 if (!result.ok) {
 logger.error('[EscrowAction] getEscrowStatus', result.error, {
 status: result.status,
 conversationId,
 });
 return null;
 }
 return result.data;
 } catch (error) {
 logger.error('[EscrowAction] getEscrowStatus', error, { conversationId });
 return null;
 }
}

/**
 * Admin resolve dispute.
 */
export async function resolveDispute(data: {
 itemId: string;
 action: 'release' | 'refund';
 adminNote?: string;
}): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/admin/escrow/resolve-dispute', {
 method: 'POST',
 token: accessToken,
 json: data,
 fallbackErrorMessage: 'Failed to resolve dispute',
 });
 if (!result.ok) {
 logger.error('[EscrowAction] resolveDispute', result.error, { status: result.status });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('[EscrowAction] resolveDispute', error);
 return false;
 }
}
