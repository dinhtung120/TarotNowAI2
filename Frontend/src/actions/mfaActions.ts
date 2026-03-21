/*
 * ===================================================================
 * FILE: mfaActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Server Actions xử lý quy trình Xác thực Đa Yếu Tố (MFA/2FA) bằng TOTP.
 *   Hỗ trợ Lấy Trạng thái, Cài đặt Mã QR, và Xác minh rào chắn an ninh (Challenge).
 * ===================================================================
 */
'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export interface MfaSetupResult {
 qrCodeUri: string;
 secretDisplay: string;
 backupCodes: string[];
}

/**
 * Láy trạng thái MFA của user hiện tại.
 */
export async function getMfaStatus(): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<{ mfaEnabled?: boolean }>('/mfa/status', {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to get MFA status',
 });
 if (!result.ok) {
 logger.error('[MFA] getMfaStatus', result.error, { status: result.status });
 return false;
 }
 return !!result.data.mfaEnabled;
 } catch (error) {
 logger.error('[MFA] getMfaStatus', error);
 return false;
 }
}

/**
 * Bước 1: Setup MFA (Generate secret & QR)
 */
export async function setupMfa(): Promise<{ success: boolean; data?: MfaSetupResult; error?: string }> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi('unauthorized') };

 try {
 const result = await serverHttpRequest<MfaSetupResult>('/mfa/setup', {
 method: 'POST',
 token: accessToken,
 fallbackErrorMessage: tApi('unknown_error'),
 });
 if (!result.ok) {
 if (result.status === 401) return { success: false, error: tApi('unauthorized') };
 logger.error('[MFA] setupMfa', result.error, { status: result.status });
 return { success: false, error: result.error || tApi('unknown_error') };
 }
 return { success: true, data: result.data };
 } catch (error) {
 logger.error('[MFA] setupMfa', error);
 return { success: false, error: tApi('network_error') };
 }
}

/**
 * Bước 2: Verify TOTP để chính thức bật MFA
 */
export async function verifyMfa(code: string): Promise<{ success: boolean; error?: string }> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi('unauthorized') };

 try {
 const result = await serverHttpRequest<unknown>('/mfa/verify', {
 method: 'POST',
 token: accessToken,
 json: { code },
 fallbackErrorMessage: tApi('unknown_error'),
 });
 if (!result.ok) {
 if (result.status === 401) return { success: false, error: tApi('unauthorized') };
 if (result.status === 400) return { success: false, error: tApi('invalid_code') };
 logger.error('[MFA] verifyMfa', result.error, { status: result.status });
 return { success: false, error: result.error || tApi('unknown_error') };
 }
 return { success: true };
 } catch (error) {
 logger.error('[MFA] verifyMfa', error);
 return { success: false, error: tApi('network_error') };
 }
}

/**
 * Challenge: Nhập TOTP code để thực hiện action nhạy cảm (VD: Payout, Admin)
 */
export async function challengeMfa(code: string): Promise<{ success: boolean; error?: string }> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi('unauthorized') };

 try {
 const result = await serverHttpRequest<unknown>('/mfa/challenge', {
 method: 'POST',
 token: accessToken,
 json: { code },
 fallbackErrorMessage: tApi('unknown_error'),
 });
 if (!result.ok) {
 if (result.status === 401) return { success: false, error: tApi('unauthorized') };
 if (result.status === 400) return { success: false, error: tApi('invalid_code') };
 logger.error('[MFA] challengeMfa', result.error, { status: result.status });
 return { success: false, error: result.error || tApi('unknown_error') };
 }
 return { success: true };
 } catch (error) {
 logger.error('[MFA] challengeMfa', error);
 return { success: false, error: tApi('network_error') };
 }
}
