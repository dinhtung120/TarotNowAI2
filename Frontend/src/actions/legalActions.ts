'use server';

import { cookies } from 'next/headers';
import { API_BASE_URL } from '@/lib/api';

// ======================================================================
// Kiểu dữ liệu cho Legal / Consent
// ======================================================================

/** Trạng thái đồng thuận pháp lý của user cho một loại tài liệu */
export interface ConsentStatus {
 documentType: string;
 version: string;
 hasConsented: boolean;
 consentedAt?: string;
}

// ======================================================================
// Actions
// Kết nối Frontend tới Backend Legal APIs:
// - GET /api/v1/legal/consent-status
// - POST /api/v1/legal/consent
// ======================================================================

/**
 * Kiểm tra trạng thái đồng thuận pháp lý hiện tại của user.
 * Dùng để xác định xem user đã consent phiên bản mới nhất chưa.
 * Nếu chưa → hiển thị popup yêu cầu re-consent (spec OPS 4.13.1).
 * * @param documentType - Loại tài liệu: 'TOS' | 'PrivacyPolicy' | 'AiDisclaimer'
 * @param version - Phiên bản cần kiểm tra (ví dụ: '1.0')
 */
export async function checkConsentStatus(documentType?: string, version?: string): Promise<ConsentStatus[] | null> {
 const cookieStore = await cookies();
 const accessToken = cookieStore.get('accessToken')?.value;

 try {
 // Xây dựng query params động dựa trên tham số truyền vào
 const params = new URLSearchParams();
 if (documentType) params.append('documentType', documentType);
 if (version) params.append('version', version);
 const queryString = params.toString() ? `?${params.toString()}` : '';

 const response = await fetch(`${API_BASE_URL}/legal/consent-status${queryString}`, {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) {
 console.error('checkConsentStatus error', response.status);
 return null;
 }

 return await response.json();
 } catch (error) {
 console.error('Failed to check consent status:', error);
 return null;
 }
}

/**
 * Ghi nhận user đã đồng ý với một tài liệu pháp lý cụ thể.
 * Backend sẽ lưu consent kèm version, IP, User-Agent (spec OPS 4.13.1).
 * * @param documentType - Loại tài liệu: 'TOS' | 'PrivacyPolicy' | 'AiDisclaimer'
 * @param version - Phiên bản tài liệu (ví dụ: '1.0')
 * @returns true nếu ghi nhận thành công
 */
export async function recordConsent(documentType: string, version: string): Promise<boolean> {
 const cookieStore = await cookies();
 const accessToken = cookieStore.get('accessToken')?.value;

 try {
 const response = await fetch(`${API_BASE_URL}/legal/consent`, {
 method: 'POST',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 body: JSON.stringify({ documentType, version }),
 });

 return response.ok;
 } catch (error) {
 console.error('Failed to record consent:', error);
 return false;
 }
}
