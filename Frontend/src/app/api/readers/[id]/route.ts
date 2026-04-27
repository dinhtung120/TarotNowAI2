import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { getReaderProfile } from '@/features/reader/application/actions';
import { resolveActionFailureStatus } from '@/shared/domain/actionResult';

export async function GET(
 _request: NextRequest,
 context: { params: Promise<{ id: string }> },
): Promise<NextResponse> {
 const { id } = await context.params;
 if (!id || id.trim().length === 0) {
  return buildProblemResponse(400, 'Reader id is required.');
 }

 const result = await getReaderProfile(id);
 if (!result.success || !result.data) {
  return buildProblemResponse(
   resolveActionFailureStatus(result, 500),
   result.error || 'Failed to load reader profile.',
  );
 }

 return NextResponse.json(result.data, { status: 200 });
}
