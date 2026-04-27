import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import type { PublicRuntimePoliciesDto } from '@/shared/application/actions/runtime-policies';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

export async function GET(): Promise<NextResponse> {
  const result = await serverHttpRequest<PublicRuntimePoliciesDto>('/legal/runtime-policies', {
    method: 'GET',
    cache: 'no-store',
    fallbackErrorMessage: 'Failed to load public runtime policies.',
  });

  if (!result.ok) {
    return buildProblemResponse(result.status, result.error);
  }

  return NextResponse.json(result.data, { status: 200 });
}
