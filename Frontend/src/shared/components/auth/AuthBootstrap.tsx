'use client';

import { useEffect } from 'react';
import type { UserProfile } from '@/features/auth/domain/types';
import { useAuthStore } from '@/store/authStore';

interface AuthBootstrapProps {
 initialUser: UserProfile | null;
}

export default function AuthBootstrap({ initialUser }: AuthBootstrapProps) {
 const clearAuth = useAuthStore((state) => state.clearAuth);
 const setSession = useAuthStore((state) => state.setSession);

 useEffect(() => {
  if (!initialUser) {
   clearAuth();
   return;
  }

  setSession(initialUser);
 }, [clearAuth, initialUser, setSession]);

 return null;
}
