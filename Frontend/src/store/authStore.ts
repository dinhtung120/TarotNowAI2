import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { UserProfile } from '@/features/auth/domain/types';

interface AuthState {
 user: UserProfile | null;
 token: string | null;
 isAuthenticated: boolean;
 setAuth: (user: UserProfile, token: string) => void;
 updateUser: (user: Partial<UserProfile>) => void;
 clearAuth: () => void;
 syncAuth: () => void;
}

export const useAuthStore = create<AuthState>()(
 persist(
  (set) => ({
   user: null,
   token: null,
   isAuthenticated: false,
   setAuth: (user, token) =>
    set(() => ({
     user,
     token,
     isAuthenticated: true,
    })),
   updateUser: (partialUser) =>
    set((state) => {
     if (!state.user) return state;
     return { user: { ...state.user, ...partialUser } };
    }),
   clearAuth: () => set({ user: null, token: null, isAuthenticated: false }),
   syncAuth: () =>
    set((state) => {
     const shouldBeAuthenticated = !!state.user && !!state.token;
     if (state.isAuthenticated === shouldBeAuthenticated) return state;
     return { ...state, isAuthenticated: shouldBeAuthenticated };
    }),
  }),
  {
   name: 'tarot-now-auth',
   partialize: (state) => ({
    user: state.user,
    token: state.token,
   }),
   onRehydrateStorage: () => (state) => {
    state?.syncAuth();
   },
  }
 )
);
