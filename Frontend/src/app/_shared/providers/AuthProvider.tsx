'use client';

import { useEffect, type ReactNode } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { registerAuthQueryBridge } from '@/features/auth/session/authStore';

interface AuthProviderProps {
 children: ReactNode;
}

export default function AuthProvider({ children }: AuthProviderProps) {
 const queryClient = useQueryClient();

 useEffect(() => {
  registerAuthQueryBridge(queryClient);
  return () => {
   registerAuthQueryBridge(null);
  };
 }, [queryClient]);

 return <>{children}</>;
}
