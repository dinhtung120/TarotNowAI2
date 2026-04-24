import { NextResponse } from 'next/server';
import type { PublicRuntimePoliciesDto } from '@/shared/application/actions/runtime-policies';
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
