'use client';

import { useEffect } from 'react';
import type { UserProfile } from '@/features/auth/session/types';
import { useAuthStore } from '@/features/auth/session/authStore';

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
