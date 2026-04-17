import { NextResponse } from 'next/server';
import type { CardCatalogItemDto } from '@/features/reading/application/actions/cards-catalog';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

interface ProblemDetailsPayload {
 type: string;
 title: string;
 status: number;
 detail: string;
}

function buildProblemResponse(status: number, detail: string): NextResponse {
 const payload: ProblemDetailsPayload = {
  type: 'about:blank',
  title: status >= 500 ? 'Server Error' : status === 401 ? 'Unauthorized' : 'Bad Request',
  status,
  detail,
 };

 return NextResponse.json(payload, { status });
}

export async function GET(): Promise<NextResponse> {
 const result = await serverHttpRequest<CardCatalogItemDto[]>('/reading/cards-catalog', {
  method: 'GET',
  fallbackErrorMessage: 'Failed to load cards catalog.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
