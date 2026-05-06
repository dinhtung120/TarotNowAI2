import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import type { ListConversationsResult } from '@/features/chat/shared/actions';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

const INBOX_PREVIEW_LIMIT = 8;

export async function GET(): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const params = new URLSearchParams({
  tab: 'active',
  page: '1',
  pageSize: String(INBOX_PREVIEW_LIMIT),
 });
 const result = await serverHttpRequest<ListConversationsResult>(`/conversations?${params.toString()}`, {
  method: 'GET',
  token,
  fallbackErrorMessage: 'Failed to load chat inbox preview.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
