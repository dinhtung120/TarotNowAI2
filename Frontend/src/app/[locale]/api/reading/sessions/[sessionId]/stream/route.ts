import { NextRequest } from 'next/server';
import { internalApiUrl } from '@/shared/infrastructure/http/apiUrl';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { logger } from '@/shared/infrastructure/logging/logger';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import {
 getSafeStreamLanguage,
 isValidStreamSessionId,
} from '../streamRouteGuards';
const UPSTREAM_OPEN_TIMEOUT_MS = 12_000;
const FALLBACK_STREAM_ERROR_DETAIL = 'Failed to open stream';

interface UpstreamProblemPayload {
 detail?: string;
 errorCode?: string;
}

function isTrustedOrigin(request: NextRequest): boolean {
  const origin = request.headers.get('origin');
  if (!origin) return true;
  return origin === request.nextUrl.origin;
}

export const runtime = 'nodejs';
export const dynamic = 'force-dynamic';

async function fetchUpstreamWithTimeout(
 url: string,
 accessToken: string,
 idempotencyKey?: string,
): Promise<Response> {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), UPSTREAM_OPEN_TIMEOUT_MS);
  try {
    const headers = new Headers({
      Authorization: `Bearer ${accessToken}`,
      Accept: 'text/event-stream',
      'Cache-Control': 'no-cache',
    });
    if (idempotencyKey) {
      headers.set('x-idempotency-key', idempotencyKey);
    }

    return await fetch(url, {
      method: 'GET',
      headers,
      cache: 'no-store',
      redirect: 'error',
      signal: controller.signal,
    });
  } finally {
    clearTimeout(timeoutId);
  }
}

async function readUpstreamProblem(upstream: Response): Promise<UpstreamProblemPayload> {
  const fallback: UpstreamProblemPayload = { detail: FALLBACK_STREAM_ERROR_DETAIL };

  let rawBody = '';
  try {
    rawBody = await upstream.text();
  } catch {
    return fallback;
  }

  const trimmedBody = rawBody.trim();
  if (!trimmedBody) {
    return fallback;
  }

  const contentType = upstream.headers.get('content-type') ?? '';
  if (contentType.includes('application/json') || contentType.includes('application/problem+json')) {
    try {
      const parsed = JSON.parse(trimmedBody) as UpstreamProblemPayload;
      const detail = parsed.detail?.trim();
      return {
        detail: detail || FALLBACK_STREAM_ERROR_DETAIL,
        errorCode: parsed.errorCode?.trim(),
      };
    } catch {
      return { detail: trimmedBody };
    }
  }

  return { detail: trimmedBody };
}

function buildStreamErrorSseResponse(
  status: number,
  detail: string,
  errorCode?: string,
): Response {
  const payload = JSON.stringify({
    status,
    detail: detail.trim() || FALLBACK_STREAM_ERROR_DETAIL,
    ...(errorCode ? { errorCode } : {}),
  });
  const body = `event: stream_error\ndata: ${payload}\n\ndata: [DONE]\n\n`;
  const headers = new Headers();
  headers.set('Content-Type', 'text/event-stream; charset=utf-8');
  headers.set('Cache-Control', 'no-store, no-cache, no-transform');
  headers.set('Connection', 'keep-alive');
  headers.set('X-Accel-Buffering', 'no');
  return new Response(body, {
    status: 200,
    headers,
  });
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
  if (!isValidStreamSessionId(sessionId)) {
    return buildProblemResponse(400, 'Invalid session id');
  }

  const path = `/sessions/${encodeURIComponent(sessionId)}/stream`;
  const upstreamUrlString = internalApiUrl(path);
  const upstreamUrl = new URL(upstreamUrlString);

  const streamToken = request.nextUrl.searchParams.get('streamToken')?.trim() || '';
  const language = getSafeStreamLanguage(request.nextUrl.searchParams.get('language'));
  const idempotencyKey = request.headers.get('x-idempotency-key')?.trim() || undefined;

  if (request.nextUrl.searchParams.has('followupQuestion')) {
   logger.warn('ReadingSessionStreamRoute', 'Legacy follow-up query transport rejected.', {
    requestId,
    sessionId,
   });
   return buildProblemResponse(400, 'Follow-up question must be sent via stream ticket.');
  }

  if (streamToken) {
    upstreamUrl.searchParams.set('streamToken', streamToken);
  } else {
    upstreamUrl.searchParams.set('language', language);
  }

  let upstream: Response;
  try {
    upstream = await fetchUpstreamWithTimeout(upstreamUrl.toString(), accessToken, idempotencyKey);
  } catch {
   logger.error('ReadingSessionStreamRoute', 'Failed to open upstream stream.', { requestId });
   return buildProblemResponse(502, 'Failed to open stream');
  }

  if (!upstream.ok || !upstream.body) {
    const problem = await readUpstreamProblem(upstream);
    logger.warn('ReadingSessionStreamRoute', 'Upstream stream rejected.', {
      requestId,
      status: upstream.status,
      detail: problem.detail ?? FALLBACK_STREAM_ERROR_DETAIL,
    });
    return buildStreamErrorSseResponse(
      upstream.status,
      problem.detail ?? FALLBACK_STREAM_ERROR_DETAIL,
      problem.errorCode,
    );
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
