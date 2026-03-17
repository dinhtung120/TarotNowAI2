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
