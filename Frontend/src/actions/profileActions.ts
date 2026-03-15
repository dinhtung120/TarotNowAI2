'use server';

/**
 * Server Actions xử lý Profile API
 *
 * Tại sao cần Server Action thay vì gọi fetch trực tiếp từ Client Component?
 * - Bảo mật: Token xác thực được giữ an toàn phía server, không bị lộ ra client code.
 * - Nhất quán: Tất cả các trang khác (Auth, Wallet, Reading) đều dùng Server Actions.
 *   Profile page là trang DUY NHẤT gọi trực tiếp fetch() → tạo bất nhất quán và lỗi URL.
 * - Đúng chuẩn Clean Architecture: Client Component → Server Action → Backend API.
 *
 * Trước khi sửa, Profile page gọi "/api/v1/profile" (URL tương đối → Next.js server),
 * nhưng API thực sự nằm ở http://localhost:5037/api/v1/profile (Backend .NET).
 */

import { cookies } from 'next/headers';

/**
 * Lấy URL gốc của Backend API từ biến môi trường.
 * Fallback về localhost:5037 nếu không có biến môi trường.
 */
const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

/**
 * Hàm helper lấy Access Token từ cookie.
 * Tại sao dùng cookie thay vì truyền token từ client?
 * - Vì loginAction() đã lưu accessToken vào cookie khi đăng nhập thành công.
 * - Server Actions chạy trên server nên có thể đọc cookie trực tiếp.
 */
async function getAccessToken(): Promise<string | undefined> {
    const cookieStore = await cookies();
    return cookieStore.get('accessToken')?.value;
}

/**
 * Lấy thông tin hồ sơ người dùng hiện tại.
 * Gọi [GET] /api/v1/profile với Bearer token.
 */
export async function getProfileAction() {
    try {
        const token = await getAccessToken();
        if (!token) {
            return { error: 'Chưa đăng nhập' };
        }

        const response = await fetch(`${API_BASE_URL}/profile`, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
        });

        if (!response.ok) {
            const result = await response.json().catch(() => ({}));
            return { error: result.message || 'Không thể tải thông tin cá nhân' };
        }

        const data = await response.json();
        return { success: true, data };
    } catch {
        return { error: 'Lỗi kết nối mạng' };
    }
}

/**
 * Cập nhật thông tin hồ sơ người dùng.
 * Gọi [PATCH] /api/v1/profile với dữ liệu form đã validate từ Zod.
 *
 * @param profileData - Đối tượng chứa displayName, avatarUrl, dateOfBirth
 */
export async function updateProfileAction(profileData: {
    displayName: string;
    avatarUrl: string | null;
    dateOfBirth: string;
}) {
    try {
        const token = await getAccessToken();
        if (!token) {
            return { error: 'Chưa đăng nhập' };
        }

        const response = await fetch(`${API_BASE_URL}/profile`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(profileData),
        });

        if (!response.ok) {
            const result = await response.json().catch(() => ({}));
            return { error: result.message || 'Cập nhật thất bại' };
        }

        return { success: true };
    } catch {
        return { error: 'Lỗi kết nối mạng' };
    }
}
