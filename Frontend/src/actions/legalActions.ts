/*
 * ===================================================================
 * FILE: legalActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Quản lý sự Đồng thuận Pháp lý (Legal Consent) của người dùng đối với
 *   Điều khoản dịch vụ (TOS), Chính sách bảo mật (Privacy Policy), và 
 *   Cảnh báo AI (AI Disclaimer).
 * ===================================================================
 */
'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

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
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

 try {
 // Xây dựng query params động dựa trên tham số truyền vào
 const params = new URLSearchParams();
 if (documentType) params.append('documentType', documentType);
 if (version) params.append('version', version);
 const queryString = params.toString() ? `?${params.toString()}` : '';

 const result = await serverHttpRequest<ConsentStatus[]>(`/legal/consent-status${queryString}`, {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to check consent status',
 });

 if (!result.ok) {
 logger.error('LegalAction.checkConsentStatus', result.error, {
 status: result.status,
 documentType,
 version,
 });
 return null;
 }

 return result.data;
 } catch (error) {
 logger.error('LegalAction.checkConsentStatus', error, { documentType, version });
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
 const accessToken = await getServerAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/legal/consent', {
 method: 'POST',
 token: accessToken,
 json: { documentType, version },
 fallbackErrorMessage: 'Failed to record consent',
 });

 if (!result.ok) {
 logger.error('LegalAction.recordConsent', result.error, {
 status: result.status,
 documentType,
 version,
 });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('LegalAction.recordConsent', error, { documentType, version });
 return false;
 }
}
