import { NextRequest, NextResponse } from 'next/server';
import { internalApiUrl } from '@/shared/infrastructure/http/apiUrl';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';

const SESSION_ID_PATTERN = /^[A-Za-z0-9-]+$/;
const SESSION_ID_MAX_LENGTH = 64;
const ALLOWED_LANGUAGES = new Set(['vi', 'en', 'zh']);
const FOLLOWUP_MAX_LENGTH = 2000;
const UPSTREAM_OPEN_TIMEOUT_MS = 12_000;

function getSafeLanguage(rawLanguage: string | null): string {
  if (!rawLanguage) return 'en';
  const language = rawLanguage.trim().toLowerCase();
  if (!language) return 'en';
  if (ALLOWED_LANGUAGES.has(language)) return language;
  const baseLanguage = language.split('-')[0];
  return ALLOWED_LANGUAGES.has(baseLanguage) ? baseLanguage : 'en';
}

function getSafeFollowupQuestion(rawQuestion: string | null): string | undefined {
  if (!rawQuestion) return undefined;
  const question = rawQuestion.replace(/\u0000/g, '').replace(/\r/g, '').trim();
  return question ? question.slice(0, FOLLOWUP_MAX_LENGTH) : undefined;
}

function isTrustedOrigin(request: NextRequest): boolean {
  const origin = request.headers.get('origin');
  if (!origin) return true;
  return origin === request.nextUrl.origin;
}

export const runtime = 'nodejs';
export const dynamic = 'force-dynamic';

function buildProblemResponse(status: number, detail: string): NextResponse {
  return NextResponse.json(
    {
      type: 'about:blank',
      title: status >= 500 ? 'Server Error' : status === 401 ? 'Unauthorized' : 'Bad Request',
      status,
      detail,
    },
    { status },
  );
}

async function fetchUpstreamWithTimeout(url: string, accessToken: string): Promise<Response> {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), UPSTREAM_OPEN_TIMEOUT_MS);
  try {
    return await fetch(url, {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${accessToken}`,
        Accept: 'text/event-stream',
        'Cache-Control': 'no-cache',
      },
      cache: 'no-store',
      redirect: 'error',
      signal: controller.signal,
    });
  } finally {
    clearTimeout(timeoutId);
  }
}

export async function GET(
  request: NextRequest,
  context: { params: Promise<{ sessionId: string }> }
): Promise<Response> {
  const requestId = Math.random().toString(36).substring(7);
  console.log(`[StreamRoute][${requestId}] Request received for session stream.`);

  if (!isTrustedOrigin(request)) {
    console.warn(`[StreamRoute][${requestId}] Untrusted origin: ${request.headers.get('origin')}`);
    return buildProblemResponse(403, 'Forbidden');
  }

  const accessToken = await getServerAccessToken();
  if (!accessToken) {
    console.error(`[StreamRoute][${requestId}] Unauthorized: Access token not found in cookies.`);
    return buildProblemResponse(401, 'Unauthorized');
  }

  const { sessionId } = await context.params;
  if (!SESSION_ID_PATTERN.test(sessionId) || sessionId.length > SESSION_ID_MAX_LENGTH) {
    return buildProblemResponse(400, 'Invalid session id');
  }

  const path = `/sessions/${encodeURIComponent(sessionId)}/stream`;
  const upstreamUrlString = internalApiUrl(path);
  const upstreamUrl = new URL(upstreamUrlString);

  const language = getSafeLanguage(request.nextUrl.searchParams.get('language'));
  const followupQuestion = getSafeFollowupQuestion(request.nextUrl.searchParams.get('followupQuestion'));

  upstreamUrl.searchParams.set('language', language);
  if (followupQuestion) {
    upstreamUrl.searchParams.set('followupQuestion', followupQuestion);
  }

  let upstream: Response;
  try {
    upstream = await fetchUpstreamWithTimeout(upstreamUrl.toString(), accessToken);
  } catch {
   console.error(`[StreamRoute][${requestId}] Fetch error when opening upstream stream.`);
   return buildProblemResponse(502, 'Failed to open stream');
  }

  console.log(`[StreamRoute][${requestId}] Upstream response: ${upstream.status} ${upstream.statusText}`);

  if (!upstream.ok || !upstream.body) {
    console.error(`[StreamRoute][${requestId}] Upstream stream rejected. status=${upstream.status}`);
    return buildProblemResponse(upstream.status, 'Failed to open stream');
  }

  const headers = new Headers();
  headers.set('Content-Type', upstream.headers.get('content-type') ?? 'text/event-stream; charset=utf-8');
  headers.set('Cache-Control', 'no-store, no-cache, no-transform');
  headers.set('Connection', 'keep-alive');
  headers.set('X-Accel-Buffering', 'no');

  console.log(`[StreamRoute][${requestId}] Stream opened successfully.`);

  return new Response(upstream.body, {
    status: upstream.status,
    headers,
  });
}
