import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import type { NotificationListResponse } from '@/features/notifications/shared/actions/types';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

function parseReadFilter(raw: string | null): boolean | undefined {
 if (raw === null) {
  return undefined;
 }

 if (raw.toLowerCase() === 'true') {
  return true;
 }

 if (raw.toLowerCase() === 'false') {
  return false;
 }

 return undefined;
}

export async function GET(request: NextRequest): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const page = Math.max(1, Number(request.nextUrl.searchParams.get('page') ?? '1') || 1);
 const pageSize = Math.max(1, Math.min(Number(request.nextUrl.searchParams.get('pageSize') ?? '20') || 20, 100));
 const isRead = parseReadFilter(request.nextUrl.searchParams.get('isRead'));

 const params = new URLSearchParams({
  page: String(page),
  pageSize: String(pageSize),
 });
 if (isRead !== undefined) {
  params.set('isRead', String(isRead));
 }

 const result = await serverHttpRequest<NotificationListResponse>(`/Notification?${params.toString()}`, {
  method: 'GET',
  token,
  fallbackErrorMessage: 'Failed to get notifications.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data, { status: 200 });
}
