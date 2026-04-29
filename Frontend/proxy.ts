import { NextRequest, NextResponse } from 'next/server';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ENTRY_PATHS, PROTECTED_PREFIXES } from '@/shared/config/authRoutes';
import createMiddleware from 'next-intl/middleware';
import { routing } from '@/i18n/routing';

const intlMiddleware = createMiddleware(routing);

const SUPPORTED_LOCALES = ['vi', 'en', 'zh'] as const;

function hasSessionCookie(request: NextRequest, cookieName: string): boolean {
 const cookieValue = request.cookies.get(cookieName)?.value;
 return typeof cookieValue === 'string' && cookieValue.trim().length > 0;
}

function resolveLocalizedPath(pathname: string): { locale: string | null; localPath: string } {
 const segments = pathname.split('/').filter(Boolean);
 const firstSegment = segments[0];
 if (!firstSegment || !SUPPORTED_LOCALES.includes(firstSegment as (typeof SUPPORTED_LOCALES)[number])) {
  return { locale: null, localPath: pathname };
 }

 const localPath = `/${segments.slice(1).join('/')}` || '/';
 return { locale: firstSegment, localPath: localPath === '/' ? '/' : localPath.replace(/\/+$/, '') };
}

function isProtectedPath(localPath: string): boolean {
 return PROTECTED_PREFIXES.some((prefix) => localPath === prefix || localPath.startsWith(`${prefix}/`));
}

function isAuthEntryPath(localPath: string): boolean {
 return AUTH_ENTRY_PATHS.some((path) => localPath === path || localPath.startsWith(`${path}/`));
}

function buildHandshakePath(nextPath: string): string {
 const encodedNext = encodeURIComponent(nextPath);
 return `/api/auth/session/handshake?next=${encodedNext}`;
}

function buildLoginPath(locale: string): string {
 return `/${locale}/login`;
}

function buildHomePath(locale: string): string {
 return `/${locale}`;
}

function redirectWithPath(request: NextRequest, targetPath: string): NextResponse {
 const url = new URL(targetPath, request.url);
 return NextResponse.redirect(url);
}

export function proxy(request: NextRequest): NextResponse {
 const { pathname } = request.nextUrl;
 if (pathname.startsWith('/api/')) {
  return NextResponse.next();
 }

 const { locale, localPath } = resolveLocalizedPath(pathname);
 if (!locale) {
  return intlMiddleware(request);
 }

 if (localPath.startsWith('/api/')) {
  return NextResponse.next();
 }

 const hasAccessToken = hasSessionCookie(request, AUTH_COOKIE.ACCESS);
 const hasRefreshToken = hasSessionCookie(request, AUTH_COOKIE.REFRESH);

 if (isProtectedPath(localPath) && !hasAccessToken) {
  if (hasRefreshToken) {
   return redirectWithPath(request, buildHandshakePath(pathname));
  }

  return redirectWithPath(request, buildLoginPath(locale));
 }

 if (isAuthEntryPath(localPath) && hasAccessToken) {
  return redirectWithPath(request, buildHomePath(locale));
 }

 return intlMiddleware(request);
}

export const config = {
 matcher: [
  '/((?!_next/static|_next/image|favicon.ico|robots.txt|sitemap.xml).*)',
 ],
};
