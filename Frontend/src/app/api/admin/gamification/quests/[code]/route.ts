import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { requireAdminAccessToken } from '@/app/api/admin/gamification/_shared';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

export async function DELETE(
 _request: Request,
 context: { params: Promise<{ code: string }> },
): Promise<NextResponse> {
 const tokenOrResponse = await requireAdminAccessToken();
 if (tokenOrResponse instanceof NextResponse) {
  return tokenOrResponse;
 }

 const { code } = await context.params;
 if (!code.trim()) {
  return buildProblemResponse(400, 'Quest code is required.');
 }

 const result = await serverHttpRequest<{ message: string }>(`/admin/gamification/quests/${encodeURIComponent(code)}`, {
  method: 'DELETE',
  token: tokenOrResponse,
  fallbackErrorMessage: 'Failed to delete admin quest.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data ?? { message: 'Deleted.' }, { status: 200 });
}
