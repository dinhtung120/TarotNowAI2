import { NextRequest, NextResponse } from 'next/server';
import { listReaders } from '@/features/reader/application/actions';

interface ProblemDetailsPayload {
 type: string;
 title: string;
 status: number;
 detail: string;
}

function buildProblemResponse(status: number, detail: string): NextResponse {
 const payload: ProblemDetailsPayload = {
  type: 'about:blank',
  title: status >= 500 ? 'Server Error' : status === 401 ? 'Unauthorized' : 'Bad Request',
  status,
  detail,
 };

 return NextResponse.json(payload, { status });
}

export async function GET(request: NextRequest): Promise<NextResponse> {
 const page = Math.max(1, Number(request.nextUrl.searchParams.get('page') ?? '1') || 1);
 const pageSize = Math.max(1, Math.min(Number(request.nextUrl.searchParams.get('pageSize') ?? '12') || 12, 50));
 const specialty = request.nextUrl.searchParams.get('specialty') ?? '';
 const status = request.nextUrl.searchParams.get('status') ?? '';
 const searchTerm = request.nextUrl.searchParams.get('searchTerm') ?? '';

 const result = await listReaders(page, pageSize, specialty, status, searchTerm);
 if (!result.success || !result.data) {
  return buildProblemResponse(400, result.error || 'Failed to load readers.');
 }

 return NextResponse.json(result.data, { status: 200 });
}
