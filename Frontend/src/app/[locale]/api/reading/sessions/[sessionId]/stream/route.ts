import { NextRequest } from 'next/server';
import { internalApiUrl } from '@/shared/infrastructure/http/apiUrl';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { logger } from '@/shared/infrastructure/logging/logger';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';

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
  const requestId = crypto.randomUUID();

  if (!isTrustedOrigin(request)) {
    logger.warn('ReadingSessionStreamRoute', 'Untrusted origin blocked.', {
      requestId,
      origin: request.headers.get('origin') ?? '',
    });
    return buildProblemResponse(403, 'Forbidden');
  }

  const accessToken = await getServerAccessToken();
  if (!accessToken) {
    logger.warn('ReadingSessionStreamRoute', 'Missing access token for stream.', { requestId });
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
   logger.error('ReadingSessionStreamRoute', 'Failed to open upstream stream.', { requestId });
   return buildProblemResponse(502, 'Failed to open stream');
  }

  if (!upstream.ok || !upstream.body) {
    logger.warn('ReadingSessionStreamRoute', 'Upstream stream rejected.', {
      requestId,
      status: upstream.status,
    });
    return buildProblemResponse(upstream.status, 'Failed to open stream');
  }

  const headers = new Headers();
  headers.set('Content-Type', upstream.headers.get('content-type') ?? 'text/event-stream; charset=utf-8');
  headers.set('Cache-Control', 'no-store, no-cache, no-transform');
  headers.set('Connection', 'keep-alive');
  headers.set('X-Accel-Buffering', 'no');

  logger.debug('ReadingSessionStreamRoute', 'Stream opened.', {
    requestId,
    status: upstream.status,
  });

  return new Response(upstream.body, {
    status: upstream.status,
    headers,
  });
}
