import type { NextRequest } from 'next/server';

function readForwardedHeaderValue(
 request: NextRequest,
 headerName: string,
): string | null {
 const rawValue = request.headers.get(headerName)?.trim();
 if (!rawValue) {
  return null;
 }

 const firstValue = rawValue.split(',')[0]?.trim();
 return firstValue || null;
}

function resolvePublicOrigin(request: NextRequest): string {
 const forwardedProto = readForwardedHeaderValue(request, 'x-forwarded-proto');
 const forwardedHost = readForwardedHeaderValue(request, 'x-forwarded-host');
 const forwardedPort = readForwardedHeaderValue(request, 'x-forwarded-port');
 const hostHeader = request.headers.get('host')?.trim() || request.nextUrl.host;
 const protocol = forwardedProto || request.nextUrl.protocol.replace(/:$/, '') || 'https';
 const host = forwardedHost || hostHeader;

 if (!host) {
  return request.nextUrl.origin;
 }

 const hasExplicitPort = host.includes(':');
 const shouldAppendPort = forwardedPort
  && !hasExplicitPort
  && !((protocol === 'https' && forwardedPort === '443') || (protocol === 'http' && forwardedPort === '80'));
 const authority = shouldAppendPort ? `${host}:${forwardedPort}` : host;
 return `${protocol}://${authority}`;
}

export function buildPublicRequestUrl(
 request: NextRequest,
 path: string,
): URL {
 return new URL(path, resolvePublicOrigin(request));
}
