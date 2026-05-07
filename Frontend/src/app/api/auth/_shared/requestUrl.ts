import type { NextRequest } from 'next/server';

type SupportedProtocol = 'http' | 'https';

const PUBLIC_HOST_ALLOWLIST = new Set([
 'localhost',
 '127.0.0.1',
 'www.tarotnow.xyz',
 'tarotnow.xyz',
 'staging.tarotnow.xyz',
]);

function isAllowedPublicHost(hostname: string): boolean {
 return PUBLIC_HOST_ALLOWLIST.has(hostname.toLowerCase());
}

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

function resolveProtocol(request: NextRequest): SupportedProtocol {
 const forwardedProto = readForwardedHeaderValue(request, 'x-forwarded-proto');
 if (forwardedProto === 'http' || forwardedProto === 'https') {
  return forwardedProto;
 }

 const nextProtocol = request.nextUrl.protocol.replace(/:$/, '');
 if (nextProtocol === 'http' || nextProtocol === 'https') {
  return nextProtocol;
 }

 return 'https';
}

function normalizePort(
 protocol: SupportedProtocol,
 rawPort: string | null,
): string | null {
 if (!rawPort || !/^\d+$/.test(rawPort)) {
  return null;
 }

 const portNumber = Number(rawPort);
 if (!Number.isInteger(portNumber) || portNumber < 1 || portNumber > 65535) {
  return null;
 }

 const port = String(portNumber);

 if (protocol === 'https' && (port === '443' || port === '80')) {
  return null;
 }

 if (protocol === 'http' && (port === '80' || port === '443')) {
  return null;
 }

 return port;
}

function parseAuthority(authority: string): { hostname: string; port: string | null } | null {
 try {
  const parsed = new URL(`http://${authority}`);
  const hostname = parsed.hostname;
  if (!hostname) {
   return null;
  }

  return {
   hostname,
   port: parsed.port || null,
  };
 } catch {
  return null;
 }
}

function formatAuthority(hostname: string, port: string | null): string {
 return port ? `${hostname}:${port}` : hostname;
}

function resolvePublicOrigin(request: NextRequest): string {
 const forwardedHost = readForwardedHeaderValue(request, 'x-forwarded-host');
 const forwardedPort = readForwardedHeaderValue(request, 'x-forwarded-port');
 const hostHeader = request.headers.get('host')?.trim() || request.nextUrl.host;
 const protocol = resolveProtocol(request);
 const host = forwardedHost || hostHeader;

 if (!host) {
  return request.nextUrl.origin;
 }

 const parsedAuthority = parseAuthority(host);
 if (!parsedAuthority) {
  return request.nextUrl.origin;
 }

 if (!isAllowedPublicHost(parsedAuthority.hostname)) {
  return request.nextUrl.origin;
 }

 const normalizedHostPort = normalizePort(protocol, parsedAuthority.port);
 const normalizedForwardedPort = normalizePort(protocol, forwardedPort);
 const effectivePort = normalizedHostPort ?? normalizedForwardedPort;
 const authority = formatAuthority(parsedAuthority.hostname, effectivePort);
 return `${protocol}://${authority}`;
}

export function buildPublicRequestUrl(
 request: NextRequest,
 path: string,
): URL {
 return new URL(path, resolvePublicOrigin(request));
}
