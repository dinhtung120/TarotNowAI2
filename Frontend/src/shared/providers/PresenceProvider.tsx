'use client';

import { type ReactNode } from 'react';
import { usePathname } from '@/i18n/routing';
import { usePresenceConnection } from '@/shared/application/hooks/usePresenceConnection';
import { shouldEnableRealtimeForPath } from '@/shared/infrastructure/navigation/normalizePathname';

interface PresenceProviderProps {
 children: ReactNode;
}

export default function PresenceProvider({ children }: PresenceProviderProps) {
 const pathname = usePathname();
 const enableRealtime = shouldEnableRealtimeForPath(pathname);
 usePresenceConnection({ enabled: enableRealtime });

 return <>{children}</>;
}
