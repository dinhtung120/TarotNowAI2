/*
 * ===================================================================
 * COMPONENT/FILE: Auth Types (auth.ts)
 * BỐI CẢNH (CONTEXT):
 *   Định nghĩa các kiểu dữ liệu (Interfaces/Types) liên quan đến Xác thực 
 *   (Authentication) và hồ sơ Người dùng (UserProfile).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Định dạng chuẩn cho `UserProfile` để Client và Backend đồng bộ.
 *   - Định nghĩa khuôn mẫu (Wrapper) cho mọi Response trả về từ API 
 *     bằng `ApiSuccessResponse` và `ApiErrorResponse` để bắt lỗi dễ dàng.
 * ===================================================================
 */
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

// Responses
export interface ApiSuccessResponse<T> {
 data?: T;
 message?: string;
}

export interface ApiErrorResponse {
 message: string;
 errors?: Record<string, string[]>;
}
