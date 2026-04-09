export interface UserProfile {
 id: string;
 email: string;
 username: string;
 displayName: string;
 avatarUrl: string | null;
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
