'use client';

import { QueryClientProvider } from '@tanstack/react-query';
import type { ReactNode } from 'react';
import PresenceProvider from '@/shared/providers/PresenceProvider';
import { useAppQueryClient } from '@/shared/components/common/hooks/useAppQueryClient';

interface AppQueryProviderProps {
 children: ReactNode;
}

export default function AppQueryProvider({ children }: AppQueryProviderProps) {
 const queryClient = useAppQueryClient();

 return (
  <QueryClientProvider client={queryClient}>
   <PresenceProvider>{children}</PresenceProvider>
  </QueryClientProvider>
 );
}
