import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { UserProfile } from '../types/auth';

interface AuthState {
 user: UserProfile | null;
 accessToken: string | null;
 isAuthenticated: boolean;
 setAuth: (user: UserProfile, token: string) => void;
 clearAuth: () => void;
}

/**
 * Zustand Store quản lý trạng thái Đăng nhập của người dùng.
 * Có lưu giữ cấu hình persist vào localStorage để chống mất phiên (session) khi F5.
 */
export const useAuthStore = create<AuthState>()(
 persist(
 (set) => ({
 user: null,
 accessToken: null,
 isAuthenticated: false,
 setAuth: (user, token) => set({ user, accessToken: token, isAuthenticated: true }),
 clearAuth: () => set({ user: null, accessToken: null, isAuthenticated: false }),
 }),
 {
 name: 'tarot-now-auth', // Tên key trong localStorage
 }
 )
);
