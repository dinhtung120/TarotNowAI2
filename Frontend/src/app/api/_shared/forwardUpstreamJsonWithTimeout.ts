import { NextResponse } from 'next/server';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';

interface ForwardUpstreamJsonOptions {
 timeoutMs: number;
 fallbackErrorDetail: string;
 request: RequestInit;
}

interface ProblemPayload {
 detail?: string;
 errorCode?: string;
}

function isAbortError(error: unknown): boolean {
 if (error instanceof DOMException) {
  return error.name === 'AbortError';
 }

 if (error instanceof Error) {
  return error.name === 'AbortError';
 }

 return false;
}

async function fetchWithTimeout(
 url: string,
 options: ForwardUpstreamJsonOptions,
): Promise<Response> {
 const controller = new AbortController();
 const timeoutId = setTimeout(() => controller.abort(), options.timeoutMs);
 try {
  return await fetch(url, {
   ...options.request,
   signal: controller.signal,
  });
 } finally {
  clearTimeout(timeoutId);
 }
}

function parseProblemPayload(rawBody: string): ProblemPayload {
 const trimmed = rawBody.trim();
 if (!trimmed) {
  return {};
 }

 try {
  const parsed = JSON.parse(trimmed) as ProblemPayload;
  return {
   detail: parsed.detail?.trim(),
   errorCode: parsed.errorCode?.trim(),
  };
 } catch {
  return {
   detail: trimmed,
  };
 }
}

export async function forwardUpstreamJsonWithTimeout(
 url: string,
 options: ForwardUpstreamJsonOptions,
): Promise<NextResponse> {
 let upstreamResponse: Response;
 try {
  upstreamResponse = await fetchWithTimeout(url, options);
 } catch (error) {
  if (isAbortError(error)) {
   return buildProblemResponse(504, 'Upstream request timed out.');
  }
  return buildProblemResponse(502, options.fallbackErrorDetail);
 }

 const rawBody = await upstreamResponse.text();
 const contentType = upstreamResponse.headers.get('content-type') ?? 'application/json';

 if (!upstreamResponse.ok) {
  const problem = parseProblemPayload(rawBody);
  return buildProblemResponse(
   upstreamResponse.status,
   problem.detail || options.fallbackErrorDetail,
   problem.errorCode,
  );
 }

 return new NextResponse(rawBody, {
  status: upstreamResponse.status,
  headers: {
   'Content-Type': contentType,
  },
 });
}
