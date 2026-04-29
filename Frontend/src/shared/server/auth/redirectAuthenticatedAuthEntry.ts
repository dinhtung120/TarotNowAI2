import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';
import {
 PROTECTED_ROUTE_AUTH_DECISION,
 resolveProtectedRouteAuthDecision,
} from '@/shared/server/auth/protectedRouteAuthDecision';

interface RedirectAuthenticatedAuthEntryOptions {
 locale: string;
 fallbackPath?: string;
}

function buildFallbackPath(locale: string, fallbackPath?: string): string {
 return fallbackPath ?? `/${locale}`;
}

export async function redirectAuthenticatedAuthEntry(
 options: RedirectAuthenticatedAuthEntryOptions,
): Promise<void> {
 const { locale, fallbackPath } = options;
 const nextPath = buildFallbackPath(locale, fallbackPath);
 const cookieStore = await cookies();
 const accessToken = cookieStore.get(AUTH_COOKIE.ACCESS)?.value;

 const authDecision = await resolveProtectedRouteAuthDecision({
  accessToken,
  locale,
  nextPath,
 });

 if (authDecision.decision !== PROTECTED_ROUTE_AUTH_DECISION.ALLOW) {
  return;
 }

 const sessionSnapshot = await getServerSessionSnapshot({ allowRefresh: false });
 if (sessionSnapshot.authenticated) {
  return redirect(nextPath);
 }
}
