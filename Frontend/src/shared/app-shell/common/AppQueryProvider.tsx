'use client';

import { QueryClientProvider } from '@tanstack/react-query';
import type { ReactNode } from 'react';
import PresenceProvider from '@/shared/providers/PresenceProvider';
import AuthProvider from '@/shared/providers/AuthProvider';
import { useAppQueryClient } from '@/shared/app-shell/common/hooks/useAppQueryClient';

interface AppQueryProviderProps {
 children: ReactNode;
}

export default function AppQueryProvider({ children }: AppQueryProviderProps) {
 const queryClient = useAppQueryClient();

 return (
  <QueryClientProvider client={queryClient}>
   <AuthProvider>
    <PresenceProvider>{children}</PresenceProvider>
   </AuthProvider>
  </QueryClientProvider>
 );
}
