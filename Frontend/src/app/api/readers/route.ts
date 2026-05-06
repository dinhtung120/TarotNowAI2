import { NextRequest, NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
import { listReaders } from '@/features/reader/shared';
import { resolveActionFailureStatus } from '@/shared/domain/actionResult';

export async function GET(request: NextRequest): Promise<NextResponse> {
 const page = Math.max(1, Number(request.nextUrl.searchParams.get('page') ?? '1') || 1);
 const pageSize = Math.max(1, Math.min(Number(request.nextUrl.searchParams.get('pageSize') ?? '12') || 12, 50));
 const specialty = request.nextUrl.searchParams.get('specialty') ?? '';
 const status = request.nextUrl.searchParams.get('status') ?? '';
 const searchTerm = request.nextUrl.searchParams.get('searchTerm') ?? '';

 const result = await listReaders(page, pageSize, specialty, status, searchTerm);
 if (!result.success || !result.data) {
  return buildProblemResponse(
   resolveActionFailureStatus(result, 500),
   result.error || 'Failed to load readers.',
  );
 }

 return NextResponse.json(result.data, { status: 200 });
}
