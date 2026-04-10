import { NextRequest } from 'next/server';
import { internalApiUrl } from '@/shared/infrastructure/http/apiUrl';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';

const SESSION_ID_PATTERN = /^[A-Za-z0-9-]+$/;
const SESSION_ID_MAX_LENGTH = 64;
const ALLOWED_LANGUAGES = new Set(['vi', 'en', 'zh']);
const FOLLOWUP_MAX_LENGTH = 2000;

function getSafeLanguage(rawLanguage: string | null): string {
 if (!rawLanguage) return 'en';
 const language = rawLanguage.trim().toLowerCase();
 if (!language) return 'en';

 if (ALLOWED_LANGUAGES.has(language)) {
  return language;
 }

 const baseLanguage = language.split('-')[0];
 if (ALLOWED_LANGUAGES.has(baseLanguage)) {
  return baseLanguage;
 }

 return 'en';
}

function getSafeFollowupQuestion(rawQuestion: string | null): string | undefined {
 if (!rawQuestion) return undefined;
 const question = rawQuestion
  .replace(/\u0000/g, '')
  .replace(/\r/g, '')
  .trim();
 if (!question) return undefined;
 return question.slice(0, FOLLOWUP_MAX_LENGTH);
}

function isTrustedOrigin(request: NextRequest): boolean {
 const origin = request.headers.get('origin');
 if (!origin) return true;
 return origin === request.nextUrl.origin;
}

async function getErrorBody(response: Response): Promise<string> {
 try {
  return await response.text();
 } catch {
  return 'Upstream stream request failed';
 }
}

export const runtime = 'nodejs';
export const dynamic = 'force-dynamic';

export async function GET(
 request: NextRequest,
 context: { params: Promise<{ sessionId: string }> }
): Promise<Response> {
 if (!isTrustedOrigin(request)) {
  return new Response('Forbidden', { status: 403 });
 }

 const accessToken = await getServerAccessToken();
 if (!accessToken) {
  return new Response('Unauthorized', { status: 401 });
 }

 const { sessionId } = await context.params;
 if (!SESSION_ID_PATTERN.test(sessionId) || sessionId.length > SESSION_ID_MAX_LENGTH) {
  return new Response('Invalid session id', { status: 400 });
 }

 const upstreamUrl = new URL(internalApiUrl(`/sessions/${encodeURIComponent(sessionId)}/stream`));
 const language = getSafeLanguage(request.nextUrl.searchParams.get('language'));
 const followupQuestion = getSafeFollowupQuestion(
  request.nextUrl.searchParams.get('followupQuestion')
 );

 upstreamUrl.searchParams.set('language', language);
 if (followupQuestion) {
  upstreamUrl.searchParams.set('followupQuestion', followupQuestion);
 }

 let upstream: Response;
 try {
  upstream = await fetch(upstreamUrl.toString(), {
   method: 'GET',
   headers: {
    Authorization: `Bearer ${accessToken}`,
    Accept: 'text/event-stream',
    'Cache-Control': 'no-cache',
   },
   cache: 'no-store',
   redirect: 'error',
  });
 } catch {
  return new Response('Failed to open stream', { status: 502 });
 }

 if (!upstream.ok || !upstream.body) {
  const body = await getErrorBody(upstream);
  return new Response(body || 'Failed to open stream', {
   status: upstream.status,
   headers: {
    'Content-Type': upstream.headers.get('content-type') ?? 'text/plain; charset=utf-8',
   },
  });
 }

 const headers = new Headers();
 headers.set('Content-Type', upstream.headers.get('content-type') ?? 'text/event-stream; charset=utf-8');
 headers.set('Cache-Control', 'no-store, no-cache, no-transform');
 headers.set('Connection', 'keep-alive');
 headers.set('X-Accel-Buffering', 'no');

 return new Response(upstream.body, {
  status: upstream.status,
  headers,
 });
}
