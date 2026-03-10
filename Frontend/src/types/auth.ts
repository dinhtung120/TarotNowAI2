export interface UserProfile {
    id: string;
    email: string;
    username: string;
    displayName: string;
    level: number;
    exp: number;
    status: 'Pending' | 'Active' | 'Suspended' | 'Banned';
}

export interface AuthResponse {
    accessToken: string;
    expiresIn: number;
    user: UserProfile;
}

// Responses
export interface ApiSuccessResponse<T> {
    data?: T;
    message?: string;
}

export interface ApiErrorResponse {
    message: string;
    errors?: Record<string, string[]>;
}
