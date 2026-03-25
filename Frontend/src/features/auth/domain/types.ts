export interface UserProfile {
 id: string;
 email: string;
 username: string;
 displayName: string;
 level: number;
 exp: number;
 role: string;
 status: 'Pending' | 'Active' | 'Suspended' | 'Banned';
}

export interface AuthResponse {
 accessToken: string;
 expiresIn: number;
 user: UserProfile;
}

export interface ApiSuccessResponse<T> {
 data?: T;
 message?: string;
}

export interface ApiErrorResponse {
 message: string;
 errors?: Record<string, string[]>;
}
