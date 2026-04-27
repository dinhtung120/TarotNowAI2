import { NextRequest, NextResponse } from 'next/server';
import { AUTH_COOKIE, AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { appendSetCookieHeaders } from '@/app/api/auth/_shared';
import { routing } from '@/i18n/routing';

const SAFE_INTERNAL_PATH = /^\/[^\s]*$/;

function resolveSafeNextPath(rawNext: string | null): string {
 if (!rawNext || !SAFE_INTERNAL_PATH.test(rawNext) || rawNext.startsWith('//')) {
  return '/';
 }

 return rawNext;
}

function isSupportedLocale(value: string): value is (typeof routing.locales)[number] {
 return (routing.locales as readonly string[]).includes(value);
}

function resolveLocaleFromPath(path: string): (typeof routing.locales)[number] {
 const segment = path.split('/').filter(Boolean)[0];
 if (segment && isSupportedLocale(segment)) {
  return segment;
 }

 return routing.defaultLocale;
}

function buildLoginPath(path: string): string {
 return `/${resolveLocaleFromPath(path)}/login`;
}

export async function GET(request: NextRequest): Promise<NextResponse> {
 const nextPath = resolveSafeNextPath(request.nextUrl.searchParams.get('next'));
 const sessionUrl = new URL('/api/auth/session?mode=lite', request.url);
 const deviceId = request.cookies.get(AUTH_COOKIE.DEVICE)?.value ?? '';

 let sessionResponse: Response;
 try {
  sessionResponse = await fetch(sessionUrl, {
   method: 'GET',
   headers: {
    Cookie: request.headers.get('cookie') ?? '',
    [AUTH_HEADER.DEVICE_ID]: deviceId,
    [AUTH_HEADER.FORWARDED_USER_AGENT]: request.headers.get('user-agent') ?? '',
   },
   cache: 'no-store',
  });
 } catch {
  const fallbackPath = buildLoginPath(nextPath);
  return NextResponse.redirect(new URL(fallbackPath, request.url));
 }

 if (!sessionResponse.ok) {
  const fallbackPath = buildLoginPath(nextPath);
  return NextResponse.redirect(new URL(fallbackPath, request.url));
 }

 const redirectResponse = NextResponse.redirect(new URL(nextPath, request.url));
 appendSetCookieHeaders(sessionResponse.headers, redirectResponse);
 return redirectResponse;
}
