import { NextResponse } from 'next/server';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import type { GachaPool } from '@/features/gacha/shared/gachaTypes';
import { buildProblemResponse, requireServerAccessToken } from '@/app/api/gacha/_shared';

export async function GET(): Promise<NextResponse> {
 const tokenOrResponse = await requireServerAccessToken();
 if (tokenOrResponse instanceof NextResponse) {
  return tokenOrResponse;
 }

 const result = await serverHttpRequest<GachaPool[]>('/gacha/pools', {
  method: 'GET',
  token: tokenOrResponse,
  fallbackErrorMessage: 'Failed to load gacha pools.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
