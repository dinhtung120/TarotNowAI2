import { NextRequest, NextResponse } from 'next/server';
import { AUTH_HEADER } from '@/shared/infrastructure/auth/authConstants';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { internalApiUrl } from '@/shared/infrastructure/http/apiUrl';
import { logger } from '@/shared/infrastructure/logging/logger';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import {
 getSafeFollowupQuestion,
 getSafeStreamLanguage,
 isValidStreamSessionId,
} from '../streamRouteGuards';

interface StreamTicketRequestBody {
 followupQuestion?: string;
 idempotencyKey?: string;
 language?: string;
}

function isTrustedOrigin(request: NextRequest): boolean {
 const origin = request.headers.get('origin');
 if (!origin) return true;
 return origin === request.nextUrl.origin;
}

export const runtime = 'nodejs';
export const dynamic = 'force-dynamic';

export async function POST(
 request: NextRequest,
 context: { params: Promise<{ sessionId: string }> },
): Promise<NextResponse> {
 const requestId = crypto.randomUUID();

 if (!isTrustedOrigin(request)) {
  logger.warn('ReadingSessionStreamTicketRoute', 'Untrusted origin blocked.', {
   requestId,
   origin: request.headers.get('origin') ?? '',
  });
  return buildProblemResponse(403, 'Forbidden');
 }

 const accessToken = await getServerAccessToken();
 if (!accessToken) {
  logger.warn('ReadingSessionStreamTicketRoute', 'Missing access token for stream ticket.', { requestId });
  return buildProblemResponse(401, 'Unauthorized');
 }

 const { sessionId } = await context.params;
 if (!isValidStreamSessionId(sessionId)) {
  return buildProblemResponse(400, 'Invalid session id');
 }

 let payload: StreamTicketRequestBody;
 try {
  payload = (await request.json()) as StreamTicketRequestBody;
 } catch {
  return buildProblemResponse(400, 'Invalid request payload.');
 }

 const followupQuestion = getSafeFollowupQuestion(payload.followupQuestion ?? null);
 if (!followupQuestion) {
  return buildProblemResponse(400, 'Follow-up question is required.');
 }

 const idempotencyKey = payload.idempotencyKey?.trim();
 if (!idempotencyKey) {
  return buildProblemResponse(400, 'Missing idempotency key.');
 }

 const language = getSafeStreamLanguage(payload.language ?? null);
 const upstreamResponse = await fetch(internalApiUrl(`/sessions/${encodeURIComponent(sessionId)}/stream-ticket`), {
  method: 'POST',
  headers: {
   Authorization: `Bearer ${accessToken}`,
   Accept: 'application/json',
   'Content-Type': 'application/json',
   [AUTH_HEADER.IDEMPOTENCY_KEY]: idempotencyKey,
  },
  cache: 'no-store',
  body: JSON.stringify({
   followUpQuestion: followupQuestion,
   language,
  }),
 });

 const responseText = await upstreamResponse.text();
 const contentType = upstreamResponse.headers.get('content-type') ?? 'application/json';

 return new NextResponse(responseText, {
  status: upstreamResponse.status,
  headers: {
   'Content-Type': contentType,
  },
 });
}
