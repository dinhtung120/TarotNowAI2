import { NextResponse } from 'next/server';

interface ProblemDetailsPayload {
 type: string;
 title: string;
 status: number;
 detail: string;
 errorCode?: string;
}

interface BuildProblemResponseOptions {
 status: number;
 detail: string;
 title?: string;
 type?: string;
 errorCode?: string;
}

function resolveProblemTitle(status: number): string {
 if (status >= 500) return 'Internal Server Error';
 if (status === 400) return 'Bad Request';
 if (status === 401) return 'Unauthorized';
 if (status === 403) return 'Forbidden';
 if (status === 404) return 'Not Found';
 if (status === 409) return 'Conflict';
 if (status === 422) return 'Unprocessable Entity';
 if (status === 429) return 'Too Many Requests';
 return 'Request Failed';
}

function createProblemResponse(options: BuildProblemResponseOptions): NextResponse {
 const { status, detail, title, type, errorCode } = options;
 const payload: ProblemDetailsPayload = {
  type: type ?? 'about:blank',
  title: title ?? resolveProblemTitle(status),
  status,
  detail,
  ...(errorCode ? { errorCode } : {}),
 };

 return new NextResponse(JSON.stringify(payload), {
  status,
  headers: {
   'Content-Type': 'application/problem+json',
  },
 });
}

export function buildProblemResponse(status: number, detail: string, errorCode?: string): NextResponse;
export function buildProblemResponse(options: BuildProblemResponseOptions): NextResponse;
export function buildProblemResponse(
 statusOrOptions: number | BuildProblemResponseOptions,
 detail?: string,
 errorCode?: string,
): NextResponse {
 if (typeof statusOrOptions === 'number') {
  return createProblemResponse({
   status: statusOrOptions,
   detail: detail ?? '',
   errorCode,
  });
 }

 return createProblemResponse(statusOrOptions);
}
