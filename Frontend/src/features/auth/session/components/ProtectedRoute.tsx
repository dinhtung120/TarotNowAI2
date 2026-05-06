'use client';

import type { ReactNode } from 'react';
import AuthGuard from '@/features/auth/session/components/AuthGuard';

interface ProtectedRouteProps {
 children: ReactNode;
 fallback?: ReactNode;
 loginPath?: string;
}

export default function ProtectedRoute({ children, fallback, loginPath }: ProtectedRouteProps) {
 return (
  <AuthGuard fallback={fallback} loginPath={loginPath}>
   {children}
  </AuthGuard>
 );
}
