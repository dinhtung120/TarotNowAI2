import createNextIntlPlugin from 'next-intl/plugin';
import bundleAnalyzer from '@next/bundle-analyzer';
import os from 'node:os';
import type { NextConfig } from 'next';
import { resolveRewriteBackendOrigin } from './src/shared/infrastructure/http/apiUrl';

const withBundleAnalyzer = bundleAnalyzer({ enabled: process.env.ANALYZE === 'true' });

if (process.env.NODE_ENV === 'production' && process.env.NODE_TLS_REJECT_UNAUTHORIZED === '0') {
 throw new Error('NODE_TLS_REJECT_UNAUTHORIZED=0 is not allowed in production.');
}

const withNextIntl = createNextIntlPlugin('./src/i18n/request.ts');
const rewriteBackendOrigin = resolveRewriteBackendOrigin();
const frontendOrigin = resolveFrontendOrigin(
 process.env.NEXT_PUBLIC_BASE_URL ?? process.env.PUBLIC_BASE_URL,
);
ensureCanonicalFrontendOrigin(frontendOrigin, process.env.NEXT_PUBLIC_CANONICAL_HOST);
const frontendHosts = resolveFrontendHosts(frontendOrigin.hostname, process.env.NEXT_ALLOWED_DEV_HOSTS);
const serverActionAllowedOrigins = resolveServerActionAllowedOrigins(frontendOrigin, frontendHosts);

function resolveFrontendOrigin(value: string | undefined): URL {
 const raw = value?.trim();
 if (!raw) {
  throw new Error(
   'Missing required environment variable NEXT_PUBLIC_BASE_URL (or PUBLIC_BASE_URL for the same value as docker-compose.prod).',
  );
 }

 try {
  return new URL(raw);
 } catch {
  throw new Error('NEXT_PUBLIC_BASE_URL is not a valid absolute URL.');
 }
}

function ensureCanonicalFrontendOrigin(origin: URL, canonicalHostValue: string | undefined): void {
 if (process.env.NODE_ENV !== 'production') {
  return;
 }

 if (process.env.ENFORCE_CANONICAL_HOST?.trim().toLowerCase() !== 'true') {
  return;
 }

 const canonicalHost = canonicalHostValue?.trim().toLowerCase();
 if (!canonicalHost) {
  return;
 }

 const configuredHost = origin.hostname.trim().toLowerCase();
 if (configuredHost !== canonicalHost) {
  throw new Error(
   `NEXT_PUBLIC_BASE_URL host (${configuredHost}) must match NEXT_PUBLIC_CANONICAL_HOST (${canonicalHost}) in production.`,
  );
 }
}

function resolveFrontendHosts(primaryHost: string, rawExtraHosts: string | undefined): string[] {
 const hosts = new Set<string>([primaryHost]);
 for (const host of parseExtraHosts(rawExtraHosts)) {
  hosts.add(host);
 }

 const interfaces = os.networkInterfaces();

 for (const addresses of Object.values(interfaces)) {
  if (!addresses) continue;

  for (const address of addresses) {
   if (address.family !== 'IPv4' || address.internal) continue;
   hosts.add(address.address);
  }
 }

 return Array.from(hosts);
}

function parseExtraHosts(rawExtraHosts: string | undefined): string[] {
 if (!rawExtraHosts) return [];
 return rawExtraHosts
  .split(',')
  .map(value => value.trim())
  .filter(value => value.length > 0);
}

function resolveServerActionAllowedOrigins(origin: URL, hosts: string[]): string[] {
 const values = new Set<string>();
 const explicitPort = origin.port;
 const defaultPort = origin.protocol === 'https:' ? '443' : '80';
 const port = explicitPort || defaultPort;

 for (const host of hosts) {
  values.add(host);
  values.add(`${host}:${port}`);
 }

 return Array.from(values);
}

/** Cho phép next/image tải ảnh từ CDN/R2 (bước 4 deploy). */
function buildImageRemotePatterns(): NonNullable<NextConfig['images']>['remotePatterns'] {
 const patterns: NonNullable<NonNullable<NextConfig['images']>['remotePatterns']> = [];
 const seen = new Set<string>();

 for (const raw of [
  process.env.NEXT_PUBLIC_MEDIA_CDN_URL,
  process.env.NEXT_PUBLIC_R2_PUBLIC_URL,
  'https://img.vietqr.io',
  'https://media.tarotnow.xyz',
  'https://ui-avatars.com',
 ]) {
  const value = raw?.trim();
  if (!value) continue;

  try {
   const parsed = new URL(value);
   const protocol = parsed.protocol === 'http:' ? 'http' : 'https';
   const key = `${protocol}://${parsed.hostname}`;
   if (seen.has(key)) continue;
   seen.add(key);
   patterns.push({
    protocol,
    hostname: parsed.hostname,
    pathname: '/**',
   });
  } catch {
   // bỏ qua URL không hợp lệ
  }
 }

 return patterns.length > 0 ? patterns : undefined;
}

const imageRemotePatterns = buildImageRemotePatterns();

const nextConfig: NextConfig = {
 output: 'standalone',
 trailingSlash: false,
  allowedDevOrigins: frontendHosts,
 poweredByHeader: false,
 ...(imageRemotePatterns ? { images: { remotePatterns: imageRemotePatterns } } : {}),
 async rewrites() {
  return [
   {
    source: '/api/v1/chat',
    destination: `${rewriteBackendOrigin}/api/v1/chat`,
   },
   {
    source: '/api/v1/chat/:path*',
    destination: `${rewriteBackendOrigin}/api/v1/chat/:path*`,
   },
   {
    source: '/api/v1/presence',
    destination: `${rewriteBackendOrigin}/api/v1/presence`,
   },
   {
    source: '/api/v1/presence/:path*',
    destination: `${rewriteBackendOrigin}/api/v1/presence/:path*`,
   },
   {
    source: '/uploads/:path*',
    destination: `${rewriteBackendOrigin}/uploads/:path*`,
   },
   {
    source: '/api/v1/:path*',
    destination: `${rewriteBackendOrigin}/api/v1/:path*`,
   },
  ];
 },
 async headers() {
  return [
   {
    source: '/:path*',
    headers: [
     { key: 'Referrer-Policy', value: 'strict-origin-when-cross-origin' },
     { key: 'X-Frame-Options', value: 'DENY' },
     { key: 'X-Content-Type-Options', value: 'nosniff' },
     
     { key: 'Permissions-Policy', value: 'camera=(self), microphone=(self), geolocation=()' },
    ],
   },
  ];
 },
  serverExternalPackages: ['sharp'],
  experimental: {
   serverActions: {
    allowedOrigins: serverActionAllowedOrigins,
    bodySizeLimit: 10485760,
   },
   // Next 16 mặc định dynamic:0 → mỗi chuyển trang refetch RSC; RTT production ~hàng trăm ms làm cảm giác “chậm 1s”.
   staleTimes: {
    dynamic: 300,
    static: 600,
   },
  },
};


export default withBundleAnalyzer(withNextIntl(nextConfig));
