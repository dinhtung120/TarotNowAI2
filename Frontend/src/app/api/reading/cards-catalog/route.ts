import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import type { CardCatalogItemDto } from '@/features/reading/application/actions/cards-catalog';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

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
