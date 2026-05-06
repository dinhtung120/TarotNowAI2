import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/models/authErrors';
import { getServerAccessToken } from '@/shared/auth/serverAuth';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';

export async function PATCH(): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const result = await serverHttpRequest<unknown>('/Notification/read-all', {
  method: 'PATCH',
  token,
  fallbackErrorMessage: 'Failed to mark all notifications as read.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json({ success: true }, { status: 200 });
}
