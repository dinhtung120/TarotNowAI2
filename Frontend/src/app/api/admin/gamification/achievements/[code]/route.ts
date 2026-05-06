import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { requireAdminSession } from '@/app/api/admin/gamification/_shared';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';

export async function DELETE(
 _request: Request,
 context: { params: Promise<{ code: string }> },
): Promise<NextResponse> {
 const sessionOrResponse = await requireAdminSession();
 if (sessionOrResponse instanceof NextResponse) {
  return sessionOrResponse;
 }

 const { code } = await context.params;
 if (!code.trim()) {
  return buildProblemResponse(400, 'Achievement code is required.');
 }

 const result = await serverHttpRequest<{ message: string }>(`/admin/gamification/achievements/${encodeURIComponent(code)}`, {
  method: 'DELETE',
  token: sessionOrResponse.token,
  fallbackErrorMessage: 'Failed to delete admin achievement.',
 });

 if (!result.ok) {
  return buildProblemResponse(result.status, result.error);
 }

 return NextResponse.json(result.data ?? { message: 'Deleted.' }, { status: 200 });
}
