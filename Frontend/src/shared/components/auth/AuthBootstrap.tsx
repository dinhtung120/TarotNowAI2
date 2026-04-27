'use client';

import { useEffect } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import type { UserProfile } from '@/features/auth/domain/types';
import { registerAuthQueryBridge, useAuthStore } from '@/store/authStore';

interface AuthBootstrapProps {
 initialUser: UserProfile | null;
}

export default function AuthBootstrap({ initialUser }: AuthBootstrapProps) {
 const queryClient = useQueryClient();
 const clearAuth = useAuthStore((state) => state.clearAuth);
 const setSession = useAuthStore((state) => state.setSession);

 useEffect(() => {
  registerAuthQueryBridge(queryClient);
  return () => {
   registerAuthQueryBridge(null);
  };
 }, [queryClient]);

 useEffect(() => {
  if (!initialUser) {
   clearAuth();
   return;
  }

  setSession(initialUser);
 }, [clearAuth, initialUser, setSession]);

 return null;
}
