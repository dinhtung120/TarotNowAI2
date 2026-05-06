import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { AUTH_COOKIE } from '@/shared/auth/authConstants';
import {
 type ServerSessionSnapshot,
} from '@/shared/auth/serverAuth';
import type { UserProfile } from '@/features/auth/session/types';
import { getCachedServerSessionSnapshot } from '@/app/_shared/server/auth/cachedSessionSnapshot';
import { isServerDocumentNavigationRequest } from '@/shared/server/auth/navigationRequest';

interface RequireSessionWithHandshakeOptions {
 locale: string;
 nextPath: string;
}

interface AuthenticatedServerSessionSnapshot {
 authenticated: true;
 user: UserProfile;
}

function toLoginPath(locale: string): string {
 return `/${locale}/login`;
}

function toHandshakePath(nextPath: string): string {
 return `/api/auth/session/handshake?next=${encodeURIComponent(nextPath)}`;
}

export async function requireSessionWithHandshake(
 options: RequireSessionWithHandshakeOptions,
): Promise<AuthenticatedServerSessionSnapshot> {
 const { locale, nextPath } = options;
 const cookieStore = await cookies();
 const accessToken = cookieStore.get(AUTH_COOKIE.ACCESS)?.value;
 const refreshToken = cookieStore.get(AUTH_COOKIE.REFRESH)?.value;
 if (!accessToken && !refreshToken) {
  redirect(toLoginPath(locale));
 }

 const sessionSnapshot = await getCachedServerSessionSnapshot();
 if (isAuthenticatedSnapshot(sessionSnapshot)) {
  return sessionSnapshot;
 }

 if (refreshToken) {
  const isDocumentNavigation = await isServerDocumentNavigationRequest();
  if (!isDocumentNavigation) {
   redirect(toLoginPath(locale));
  }

  redirect(toHandshakePath(nextPath));
 }

 redirect(toLoginPath(locale));
}

function isAuthenticatedSnapshot(
 snapshot: ServerSessionSnapshot,
): snapshot is AuthenticatedServerSessionSnapshot {
 return snapshot.authenticated && Boolean(snapshot.user);
}
