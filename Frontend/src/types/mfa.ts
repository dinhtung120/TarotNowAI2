/*
 * ===================================================================
 * COMPONENT/FILE: MFA Types (mfa.ts)
 * BỐI CẢNH (CONTEXT):
 *   Cấu trúc dữ liệu dành riêng cho tính năng Multi-Factor Authentication (Xác thực 2 lớp).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - `MfaSetupData`: Lấy Secret Key và biểu đồ QR Code để cài bằng Google Authenticator.
 *   - Định nghĩa các kiện Challenge (thách thức bảo mật) cần User nhập OTP 
 *     trước khi Rút tiền (withdrawal) hoặc Login ở phiên làm việc mới.
 * ===================================================================
 */
export interface MfaSetupData {
 secret: string;
 qrCodeUri: string;
}

export interface MfaVerifyRequest {
 code: string;
}

export interface MfaChallengeData {
 challengeId: string;
 expiresAt: string;
}

export interface MfaChallengeRequest {
 action: 'withdrawal' | 'admin_action' | 'login';
}

export interface MfaStatus {
 isConfigured: boolean;
}
