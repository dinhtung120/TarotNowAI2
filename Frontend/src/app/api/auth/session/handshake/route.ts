import { NextRequest, NextResponse } from 'next/server';
import { appendSetCookieHeaders } from '@/app/api/auth/_shared';
import { buildPublicRequestUrl } from '@/app/api/auth/_shared/requestUrl';
import {
 getSessionRouteResponse,
 SESSION_MODE,
} from '@/app/api/auth/session/sessionRouteHandler';
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

 let sessionResponse: NextResponse;
 try {
  sessionResponse = await getSessionRouteResponse(request, SESSION_MODE.LITE);
 } catch {
  const fallbackPath = buildLoginPath(nextPath);
  return NextResponse.redirect(buildPublicRequestUrl(request, fallbackPath));
 }

 if (!sessionResponse.ok) {
  const fallbackPath = buildLoginPath(nextPath);
  const redirectResponse = NextResponse.redirect(buildPublicRequestUrl(request, fallbackPath));
  appendSetCookieHeaders(sessionResponse.headers, redirectResponse);
  return redirectResponse;
 }

 const redirectResponse = NextResponse.redirect(buildPublicRequestUrl(request, nextPath));
 appendSetCookieHeaders(sessionResponse.headers, redirectResponse);
 return redirectResponse;
}
