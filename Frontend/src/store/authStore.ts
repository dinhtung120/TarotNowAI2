import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { UserProfile } from '@/features/auth/domain/types';

interface AuthState {
 user: UserProfile | null;
 isAuthenticated: boolean;
 setAuth: (user: UserProfile) => void;
 updateUser: (user: Partial<UserProfile>) => void;
 clearAuth: () => void;
 syncAuth: () => void;
}

export const useAuthStore = create<AuthState>()(
 persist(
  (set) => ({
   user: null,
   isAuthenticated: false,
   setAuth: (user) =>
    set(() => ({
     user,
     isAuthenticated: true,
    })),
   updateUser: (partialUser) =>
    set((state) => {
     if (!state.user) return state;
     return { user: { ...state.user, ...partialUser } };
    }),
   clearAuth: () => set({ user: null, isAuthenticated: false }),
   syncAuth: () =>
    set((state) => {
     const shouldBeAuthenticated = !!state.user;
     if (state.isAuthenticated === shouldBeAuthenticated) return state;
     return { ...state, isAuthenticated: shouldBeAuthenticated };
    }),
  }),
  {
   name: 'tarot-now-auth',
   partialize: (state) => ({
    user: state.user,
   }),
   onRehydrateStorage: () => (state) => {
    state?.syncAuth();
   },
  }
 )
);
