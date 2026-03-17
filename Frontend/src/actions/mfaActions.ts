'use server';

import { cookies } from 'next/headers';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

async function getAccessToken(): Promise<string | undefined> {
  const cookieStore = await cookies();
  return cookieStore.get('accessToken')?.value;
}

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
    const res = await fetch(`${API_URL}/mfa/status`, {
      method: 'GET',
      headers: { 'Authorization': `Bearer ${accessToken}` },
      cache: 'no-store',
    });
    if (!res.ok) return false;
    const data = await res.json();
    return !!data.mfaEnabled;
  } catch (error) {
    console.error('[MFA] getMfaStatus error:', error);
    return false;
  }
}

/**
 * Bước 1: Setup MFA (Generate secret & QR)
 */
export async function setupMfa(): Promise<{ success: boolean; data?: MfaSetupResult; error?: string }> {
  const accessToken = await getAccessToken();
  if (!accessToken) return { success: false, error: 'Chưa đăng nhập' };

  try {
    const res = await fetch(`${API_URL}/mfa/setup`, {
      method: 'POST',
      headers: { 'Authorization': `Bearer ${accessToken}` },
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      return { success: false, error: err.detail || err.msg || 'Lỗi setup' };
    }
    const data = await res.json();
    return { success: true, data };
  } catch (error) {
    console.error('[MFA] setupMfa error:', error);
    return { success: false, error: 'Lỗi kết nối' };
  }
}

/**
 * Bước 2: Verify TOTP để chính thức bật MFA
 */
export async function verifyMfa(code: string): Promise<{ success: boolean; error?: string }> {
  const accessToken = await getAccessToken();
  if (!accessToken) return { success: false, error: 'Chưa đăng nhập' };

  try {
    const res = await fetch(`${API_URL}/mfa/verify`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ code }),
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      return { success: false, error: err.detail || err.msg || 'Mã xác thực không hợp lệ' };
    }
    return { success: true };
  } catch (error) {
    console.error('[MFA] verifyMfa error:', error);
    return { success: false, error: 'Lỗi kết nối' };
  }
}

/**
 * Challenge: Nhập TOTP code để thực hiện action nhạy cảm (VD: Payout, Admin)
 */
export async function challengeMfa(code: string): Promise<{ success: boolean; error?: string }> {
  const accessToken = await getAccessToken();
  if (!accessToken) return { success: false, error: 'Chưa đăng nhập' };

  try {
    const res = await fetch(`${API_URL}/mfa/challenge`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ code }),
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      return { success: false, error: err.detail || err.msg || 'Mã MFA không đúng' };
    }
    return { success: true };
  } catch (error) {
    console.error('[MFA] challengeMfa error:', error);
    return { success: false, error: 'Lỗi kết nối' };
  }
}
