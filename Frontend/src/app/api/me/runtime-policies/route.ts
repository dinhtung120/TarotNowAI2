import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/models/authErrors';
import type { RuntimePoliciesDto } from '@/shared/actions/runtime-policies';
import { getServerAccessToken } from '@/shared/auth/serverAuth';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';

export async function GET(): Promise<NextResponse> {
  const token = await getServerAccessToken();
  if (!token) {
    return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
  }

  const result = await serverHttpRequest<RuntimePoliciesDto>('/me/runtime-policies', {
    method: 'GET',
    token,
    fallbackErrorMessage: 'Failed to load runtime policies.',
  });

  if (!result.ok) {
    return buildProblemResponse(result.status, result.error);
  }

  return NextResponse.json(result.data, { status: 200 });
}
