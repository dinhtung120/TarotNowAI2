import { NextRequest, NextResponse } from 'next/server';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { GACHA_IDEMPOTENCY_HEADER } from '@/shared/infrastructure/gacha/gachaConstants';
import type { PullGachaResult } from '@/shared/infrastructure/gacha/gachaTypes';
import { buildProblemResponse, requireServerAccessToken } from '@/app/api/gacha/_shared';

interface PullPayload {
 poolCode: string;
 count: number;
 idempotencyKey?: string;
}

export async function POST(request: NextRequest): Promise<NextResponse> {
 const routeStartAt = performance.now();
 const tokenOrResponse = await requireServerAccessToken();
 if (tokenOrResponse instanceof NextResponse) {
  return tokenOrResponse;
 }

 let payload: PullPayload;
 try {
  payload = (await request.json()) as PullPayload;
 } catch {
  return buildProblemResponse(400, 'Invalid request payload.');
 }

 const headerIdempotencyKey = request.headers.get(GACHA_IDEMPOTENCY_HEADER);
 const idempotencyKey = (headerIdempotencyKey || payload.idempotencyKey || '').trim();
 if (!idempotencyKey) {
  return buildProblemResponse(400, 'Missing idempotency key.');
 }

 const count = Number.isFinite(payload.count) ? payload.count : 1;
 const normalizedCount = Math.max(1, Math.min(Math.floor(count), 10));

 const upstreamStartAt = performance.now();
 const result = await serverHttpRequest<PullGachaResult>('/gacha/pull', {
  method: 'POST',
  token: tokenOrResponse,
  json: {
   poolCode: payload.poolCode,
   count: normalizedCount,
   idempotencyKey,
  },
  headers: {
   [GACHA_IDEMPOTENCY_HEADER]: idempotencyKey,
  },
  fallbackErrorMessage: 'Failed to pull gacha.',
 });
 const upstreamDurationMs = Math.round(performance.now() - upstreamStartAt);
 const totalDurationMs = Math.round(performance.now() - routeStartAt);

 if (!result.ok) {
  const response = buildProblemResponse(result.status, result.error);
  response.headers.set('x-gacha-upstream-duration-ms', String(upstreamDurationMs));
  response.headers.set('x-gacha-total-duration-ms', String(totalDurationMs));
  return response;
 }

 const response = NextResponse.json(result.data, { status: 200 });
 response.headers.set('x-gacha-upstream-duration-ms', String(upstreamDurationMs));
 response.headers.set('x-gacha-total-duration-ms', String(totalDurationMs));
 return response;
}
