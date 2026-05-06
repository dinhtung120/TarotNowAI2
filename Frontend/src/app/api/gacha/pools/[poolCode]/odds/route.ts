import { NextRequest, NextResponse } from 'next/server';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import type { GachaPoolOdds } from '@/features/gacha/shared/gachaTypes';
import { buildProblemResponse, requireServerAccessToken } from '@/app/api/gacha/_shared';

interface RouteParams {
 params: Promise<{ poolCode: string }>;
}

export async function GET(_request: NextRequest, { params }: RouteParams): Promise<NextResponse> {
 const tokenOrResponse = await requireServerAccessToken();
 if (tokenOrResponse instanceof NextResponse) {
  return tokenOrResponse;
 }

 const { poolCode } = await params;
 if (!poolCode.trim()) {
  return buildProblemResponse(400, 'Missing pool code.');
 }

 const result = await serverHttpRequest<GachaPoolOdds>(`/gacha/pools/${encodeURIComponent(poolCode)}/odds`, {
  method: 'GET',
  token: tokenOrResponse,
  fallbackErrorMessage: 'Failed to load gacha pool odds.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
