import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { UserProfile } from '../types/auth';
import { isJwtExpired } from '@/lib/jwt';

interface AuthState {
 user: UserProfile | null;
 accessToken: string | null;
 isAuthenticated: boolean;
 setAuth: (user: UserProfile, token: string) => void;
 clearAuth: () => void;
 syncAuth: () => void;
}

/*
 * ===================================================================
 * COMPONENT/FILE: Cửa hàng Trạng thái Xác thực (authStore.ts)
 * BỐI CẢNH (CONTEXT):
 *   Store quản lý trạng thái Đăng nhập của người dùng sử dụng thư viện Zustand.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Lưu trữ thông tin Thông tin cá nhân (User) và Access Token rải rác.
 *   - Sử dụng Middleware `persist` để lưu trạng thái xuống `localStorage`, 
 *     đảm bảo không bị mất Sesion khi người dùng nhấn F5 (Tải lại trang).
 *   - Cung cấp hàm `syncAuth` để đồng bộ và tự động Logout nếu Token bị quá hạn (Expired).
 * ===================================================================
 */
export const useAuthStore = create<AuthState>()(
 persist(
 (set) => ({
 user: null,
 accessToken: null,
 isAuthenticated: false,
 setAuth: (user, token) =>
  set(() => {
   if (!token || isJwtExpired(token, 5)) {
    return { user: null, accessToken: null, isAuthenticated: false };
   }
   return { user, accessToken: token, isAuthenticated: true };
  }),
 clearAuth: () => set({ user: null, accessToken: null, isAuthenticated: false }),
 syncAuth: () =>
  set((state) => {
   const token = state.accessToken;
   if (!token) {
    if (!state.user && state.isAuthenticated === false) return state;
    return { user: null, accessToken: null, isAuthenticated: false };
   }

   if (isJwtExpired(token, 5)) {
    return { user: null, accessToken: null, isAuthenticated: false };
   }

   if (state.isAuthenticated) return state;
   return { ...state, isAuthenticated: true };
  }),
 }),
 {
 name: 'tarot-now-auth', // Tên key trong localStorage
 partialize: (state) => ({
  user: state.user,
  accessToken: state.accessToken,
 }),
 onRehydrateStorage: () => (state) => {
  state?.syncAuth();
 },
 }
 )
);
