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

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
