import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import type { UserCollectionDto } from '@/features/collection/cards/actions/actions';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

export async function GET(): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const result = await serverHttpRequest<UserCollectionDto[]>('/reading/collection', {
  method: 'GET',
  token,
  fallbackErrorMessage: 'Failed to load collection.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
