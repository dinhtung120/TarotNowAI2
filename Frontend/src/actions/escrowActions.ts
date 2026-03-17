'use server';

import { cookies } from 'next/headers';
import { API_BASE_URL } from '@/lib/api';

// ======================================================================
// Helper — lấy token từ HttpOnly cookie
// ======================================================================

async function getAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 return cookieStore.get('accessToken')?.value;
}

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
 const res = await fetch(`${API_BASE_URL}/escrow/accept`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify(data),
 });
 if (!res.ok) return { success: false };
 return await res.json();
 } catch (error) {
 console.error('[EscrowAction] acceptOffer failed:', error);
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
 const res = await fetch(`${API_BASE_URL}/escrow/reply`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({ itemId }),
 });
 return res.ok;
 } catch (error) {
 console.error('[EscrowAction] readerReply failed:', error);
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
 const res = await fetch(`${API_BASE_URL}/escrow/confirm`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({ itemId }),
 });
 return res.ok;
 } catch (error) {
 console.error('[EscrowAction] confirmRelease failed:', error);
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
 const res = await fetch(`${API_BASE_URL}/escrow/dispute`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({ itemId, reason }),
 });
 return res.ok;
 } catch (error) {
 console.error('[EscrowAction] openDispute failed:', error);
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
 const res = await fetch(`${API_BASE_URL}/escrow/add-question`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify(data),
 });
 if (!res.ok) return { success: false };
 return await res.json();
 } catch (error) {
 console.error('[EscrowAction] addQuestion failed:', error);
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
 const res = await fetch(`${API_BASE_URL}/escrow/${conversationId}`, {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });
 if (!res.ok) return null;
 return await res.json();
 } catch (error) {
 console.error('[EscrowAction] getEscrowStatus failed:', error);
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
 const res = await fetch(`${API_BASE_URL}/admin/escrow/resolve-dispute`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify(data),
 });
 return res.ok;
 } catch (error) {
 console.error('[EscrowAction] resolveDispute failed:', error);
 return false;
 }
}
