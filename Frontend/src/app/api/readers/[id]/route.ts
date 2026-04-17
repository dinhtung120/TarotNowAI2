import { NextRequest, NextResponse } from 'next/server';
import { getReaderProfile } from '@/features/reader/application/actions';

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
  return buildProblemResponse(400, result.error || 'Failed to load reader profile.');
 }

 return NextResponse.json(result.data, { status: 200 });
}
