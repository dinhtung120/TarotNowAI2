import { NextRequest, NextResponse } from 'next/server';
import { appendSetCookieHeaders, clearAuthCookies } from '@/app/api/auth/_shared';
import { buildPublicRequestUrl } from '@/app/api/auth/_shared/requestUrl';
import {
 getSessionRouteResponse,
 SESSION_MODE,
} from '@/app/api/auth/session/sessionRouteHandler';
import { routing } from '@/i18n/routing';

const SAFE_INTERNAL_PATH = /^\/[^\s]*$/;
const HANDSHAKE_LOOP_WINDOW_MS = 5_000;
const HANDSHAKE_GUARD_COOKIE = '__tn_handshake_guard';
const HANDSHAKE_GUARD_MAX_AGE_SECONDS = 10;

interface HandshakeGuardState {
 nextPath: string;
 issuedAtMs: number;
 count: number;
}

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

function isDocumentNavigationRequest(request: NextRequest): boolean {
 if (request.method !== 'GET' && request.method !== 'HEAD') {
  return false;
 }

 if (request.headers.has('next-router-prefetch')) {
  return false;
 }

 const purpose = (request.headers.get('purpose') ?? request.headers.get('sec-purpose') ?? '').toLowerCase();
 if (purpose === 'prefetch') {
  return false;
 }

 const hasRscSignal = request.headers.get('rsc') === '1' || request.nextUrl.searchParams.has('_rsc');
 if (hasRscSignal) {
  return false;
 }

 const accept = (request.headers.get('accept') ?? '').toLowerCase();
 if (!accept.includes('text/html') || accept.includes('text/x-component')) {
  return false;
 }

 const fetchDest = (request.headers.get('sec-fetch-dest') ?? '').toLowerCase();
 if (fetchDest.length > 0 && fetchDest !== 'document') {
  return false;
 }

 const fetchMode = (request.headers.get('sec-fetch-mode') ?? '').toLowerCase();
 if (fetchMode.length > 0 && fetchMode !== 'navigate') {
  return false;
 }

 return true;
}

function parseHandshakeGuardCookie(rawValue: string | undefined): HandshakeGuardState | null {
 if (!rawValue) {
  return null;
 }

 const [encodedPath, issuedAtRaw, countRaw] = rawValue.split(':');
 if (!encodedPath || !issuedAtRaw || !countRaw) {
  return null;
 }

 let decodedPath = '';
 try {
  decodedPath = decodeURIComponent(encodedPath);
 } catch {
  return null;
 }

 const issuedAtMs = Number.parseInt(issuedAtRaw, 10);
 const count = Number.parseInt(countRaw, 10);
 if (!Number.isFinite(issuedAtMs) || !Number.isFinite(count) || count <= 0) {
  return null;
 }

 return {
  nextPath: decodedPath,
  issuedAtMs,
  count,
 };
}

function buildHandshakeGuardCookieValue(state: HandshakeGuardState): string {
 return `${encodeURIComponent(state.nextPath)}:${state.issuedAtMs}:${state.count}`;
}

function resolveHandshakeGuardState(request: NextRequest, nextPath: string): HandshakeGuardState {
 const now = Date.now();
 const previous = parseHandshakeGuardCookie(request.cookies.get(HANDSHAKE_GUARD_COOKIE)?.value);
 const isSamePathWithinWindow = Boolean(
  previous
  && previous.nextPath === nextPath
  && now - previous.issuedAtMs <= HANDSHAKE_LOOP_WINDOW_MS,
 );

 return {
  nextPath,
  issuedAtMs: now,
  count: isSamePathWithinWindow ? previous.count + 1 : 1,
 };
}

function shouldUseSecureCookie(request: NextRequest): boolean {
 const forwardedProto = request.headers.get('x-forwarded-proto')?.split(',')[0]?.trim().toLowerCase();
 if (forwardedProto === 'https') {
  return true;
 }

 return request.nextUrl.protocol === 'https:';
}

function setHandshakeGuardCookie(
 response: NextResponse,
 request: NextRequest,
 state: HandshakeGuardState,
): void {
 response.cookies.set({
  name: HANDSHAKE_GUARD_COOKIE,
  value: buildHandshakeGuardCookieValue(state),
  httpOnly: true,
  secure: shouldUseSecureCookie(request),
  sameSite: 'strict',
  path: '/',
  maxAge: HANDSHAKE_GUARD_MAX_AGE_SECONDS,
 });
}

function clearHandshakeGuardCookie(response: NextResponse, request: NextRequest): void {
 response.cookies.set({
  name: HANDSHAKE_GUARD_COOKIE,
  value: '',
  httpOnly: true,
  secure: shouldUseSecureCookie(request),
  sameSite: 'strict',
  path: '/',
  maxAge: 0,
 });
}

export async function GET(request: NextRequest): Promise<NextResponse> {
 const nextPath = resolveSafeNextPath(request.nextUrl.searchParams.get('next'));
 const fallbackPath = buildLoginPath(nextPath);

 if (!isDocumentNavigationRequest(request)) {
  return NextResponse.redirect(buildPublicRequestUrl(request, fallbackPath));
 }

const guardState = resolveHandshakeGuardState(request, nextPath);
 if (guardState.count > 1) {
  const redirectResponse = NextResponse.redirect(buildPublicRequestUrl(request, fallbackPath));
  clearAuthCookies(redirectResponse, request);
  clearHandshakeGuardCookie(redirectResponse, request);
  redirectResponse.headers.set('x-handshake-loop-guard', 'triggered');
  return redirectResponse;
 }

 let sessionResponse: NextResponse;
 try {
  sessionResponse = await getSessionRouteResponse(request, SESSION_MODE.LITE);
 } catch {
  const redirectResponse = NextResponse.redirect(buildPublicRequestUrl(request, fallbackPath));
  setHandshakeGuardCookie(redirectResponse, request, guardState);
  return redirectResponse;
 }

 if (!sessionResponse.ok) {
  const redirectResponse = NextResponse.redirect(buildPublicRequestUrl(request, fallbackPath));
  appendSetCookieHeaders(sessionResponse.headers, redirectResponse);
  setHandshakeGuardCookie(redirectResponse, request, guardState);
  return redirectResponse;
 }

 const redirectResponse = NextResponse.redirect(buildPublicRequestUrl(request, nextPath));
 appendSetCookieHeaders(sessionResponse.headers, redirectResponse);
 setHandshakeGuardCookie(redirectResponse, request, guardState);
 return redirectResponse;
}
