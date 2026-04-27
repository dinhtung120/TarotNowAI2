import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import {
 getServerSessionSnapshot,
 type ServerSessionSnapshot,
} from '@/shared/infrastructure/auth/serverAuth';
import type { UserProfile } from '@/features/auth/domain/types';
import {
 PROTECTED_ROUTE_AUTH_DECISION,
 resolveProtectedRouteAuthDecision,
} from '@/shared/server/auth/protectedRouteAuthDecision';

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

 const authDecision = resolveProtectedRouteAuthDecision({
  accessToken,
  refreshToken,
  locale,
  nextPath,
 });
 if (authDecision.decision === PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_LOGIN) {
  redirect(toLoginPath(locale));
 }
 if (authDecision.decision === PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_HANDSHAKE) {
  redirect(toHandshakePath(nextPath));
 }

 const sessionSnapshot = await getServerSessionSnapshot({ allowRefresh: false });
 if (isAuthenticatedSnapshot(sessionSnapshot)) {
  return sessionSnapshot;
 }

 if (refreshToken) {
  redirect(toHandshakePath(nextPath));
 }

 redirect(toLoginPath(locale));
}

function isAuthenticatedSnapshot(
 snapshot: ServerSessionSnapshot,
): snapshot is AuthenticatedServerSessionSnapshot {
 return snapshot.authenticated && Boolean(snapshot.user);
}
