import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

export async function PATCH(
 _request: NextRequest,
 context: { params: Promise<{ id: string }> },
): Promise<NextResponse> {
 const token = await getServerAccessToken();
 if (!token) {
  return buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 }

 const { id } = await context.params;
 if (!id || id.trim().length === 0) {
  return buildProblemResponse(400, 'Notification id is required.');
 }

 const result = await serverHttpRequest<unknown>(`/Notification/${id}/read`, {
  method: 'PATCH',
  token,
  fallbackErrorMessage: 'Failed to mark notification as read.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json({ success: true }, { status: 200 });
}
