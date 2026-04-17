import { NextResponse } from 'next/server';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import type { ReadingSetupSnapshotDto } from '@/shared/application/actions/reading-setup-snapshot';
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

export async function GET(): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const result = await serverHttpRequest<ReadingSetupSnapshotDto>('/me/reading-setup-snapshot', {
  method: 'GET',
  token,
  fallbackErrorMessage: 'Failed to load reading setup snapshot.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
