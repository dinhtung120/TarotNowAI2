'use client';

import { type ReactNode } from 'react';
import { usePresenceConnection } from '@/shared/application/hooks/usePresenceConnection';

interface PresenceProviderProps {
 children: ReactNode;
}

export default function PresenceProvider({ children }: PresenceProviderProps) {
 usePresenceConnection();

 return <>{children}</>;
}
