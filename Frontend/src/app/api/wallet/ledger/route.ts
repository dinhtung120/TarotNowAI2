import { NextRequest, NextResponse } from 'next/server';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import type { WalletPaginatedList, WalletTransaction } from '@/features/wallet/domain/types';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

interface ProblemDetailsPayload {
 type: string;
 title: string;
 status: number;
 detail: string;
 errorCode?: string;
}

function buildProblemResponse(status: number, detail: string, errorCode?: string): NextResponse {
 const payload: ProblemDetailsPayload = {
  type: 'about:blank',
  title: status >= 500 ? 'Server Error' : status === 401 ? 'Unauthorized' : 'Bad Request',
  status,
  detail,
  ...(errorCode ? { errorCode } : {}),
 };

 return NextResponse.json(payload, { status });
}

export async function GET(request: NextRequest): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const page = Math.max(1, Number(request.nextUrl.searchParams.get('page') ?? '1') || 1);
 const limit = Math.max(1, Math.min(Number(request.nextUrl.searchParams.get('limit') ?? '10') || 10, 50));

 const result = await serverHttpRequest<WalletPaginatedList<WalletTransaction>>(
  `/Wallet/ledger?page=${page}&limit=${limit}`,
  {
   method: 'GET',
   token,
   fallbackErrorMessage: 'Failed to load wallet ledger.',
  },
 );

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
