'use client';

import { useEffect, type ReactNode } from 'react';
import { usePathname } from '@/i18n/routing';
import { useAuth } from '@/shared/hooks/useAuth';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import { AUTH_ENTRY_PATHS } from '@/shared/config/authRoutes';
import { normalizePathname } from '@/shared/infrastructure/navigation/normalizePathname';

interface AuthGuardProps {
 children: ReactNode;
 fallback?: ReactNode;
 loginPath?: string;
}

function resolveLocaleAwareLoginPath(pathname: string): string {
 const segments = pathname.split('/').filter(Boolean);
 const locale = segments[0];
 if (!locale) {
  return '/login';
 }

 return `/${locale}/login`;
}

function isAuthEntryPath(pathname: string): boolean {
 return AUTH_ENTRY_PATHS.some((entryPath) => pathname === entryPath || pathname.startsWith(`${entryPath}/`));
}

export default function AuthGuard({ children, fallback = null, loginPath }: AuthGuardProps) {
 const navigation = useOptimizedNavigation();
 const pathname = usePathname();
 const { isAuthenticated, isSessionLoading } = useAuth();

 useEffect(() => {
  if (isSessionLoading || isAuthenticated) {
   return;
  }

  const normalizedPath = normalizePathname(pathname);
  if (isAuthEntryPath(normalizedPath)) {
   return;
  }

  navigation.replace(loginPath ?? resolveLocaleAwareLoginPath(normalizedPath));
 }, [isAuthenticated, isSessionLoading, loginPath, navigation, pathname]);

 if (isSessionLoading || !isAuthenticated) {
  return <>{fallback}</>;
 }

 return <>{children}</>;
}
