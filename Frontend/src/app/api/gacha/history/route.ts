import { NextRequest, NextResponse } from 'next/server';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import type { GachaHistoryPage } from '@/features/gacha/shared/gachaTypes';
import { buildProblemResponse, requireServerAccessToken } from '@/app/api/gacha/_shared';

export async function GET(request: NextRequest): Promise<NextResponse> {
 const tokenOrResponse = await requireServerAccessToken();
 if (tokenOrResponse instanceof NextResponse) {
  return tokenOrResponse;
 }

  const pageRaw = request.nextUrl.searchParams.get('page');
  const pageSizeRaw = request.nextUrl.searchParams.get('pageSize');
  const parsedPage = Number.parseInt(pageRaw ?? '1', 10);
  const parsedPageSize = Number.parseInt(pageSizeRaw ?? '20', 10);
  const normalizedPage = Number.isFinite(parsedPage) ? Math.max(1, parsedPage) : 1;
  const normalizedPageSize = Number.isFinite(parsedPageSize) ? Math.max(1, Math.min(parsedPageSize, 100)) : 20;

  const result = await serverHttpRequest<GachaHistoryPage>(
    `/gacha/history?page=${normalizedPage}&pageSize=${normalizedPageSize}`,
    {
      method: 'GET',
      token: tokenOrResponse,
      fallbackErrorMessage: 'Failed to load gacha history.',
    },
  );

  if (!result.ok) {
    return buildProblemResponse(result.status, result.error);
  }

  return NextResponse.json(result.data, { status: 200 });
}
