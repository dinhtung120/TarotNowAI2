import { NextResponse } from 'next/server';

interface ProblemDetailsPayload {
 type: string;
 title: string;
 status: number;
 detail: string;
 errorCode?: string;
}

function resolveProblemTitle(status: number): string {
 if (status >= 500) return 'Server Error';
 if (status === 401) return 'Unauthorized';
 if (status === 403) return 'Forbidden';
 if (status === 404) return 'Not Found';
 return 'Bad Request';
}

export function buildProblemResponse(status: number, detail: string, errorCode?: string): NextResponse {
 const payload: ProblemDetailsPayload = {
  type: 'about:blank',
  title: resolveProblemTitle(status),
  status,
  detail,
  ...(errorCode ? { errorCode } : {}),
 };

 return NextResponse.json(payload, { status });
}
