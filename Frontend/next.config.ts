import createNextIntlPlugin from 'next-intl/plugin';
import os from 'node:os';
import { resolveApiOrigin } from './src/shared/infrastructure/http/apiUrl';


const withNextIntl = createNextIntlPlugin('./src/i18n/request.ts');
const apiOrigin = resolveApiOrigin(process.env.NEXT_PUBLIC_API_URL);
const frontendOrigin = resolveFrontendOrigin(process.env.NEXT_PUBLIC_BASE_URL);
const frontendHosts = resolveFrontendHosts(frontendOrigin.hostname);
const serverActionAllowedOrigins = resolveServerActionAllowedOrigins(frontendOrigin, frontendHosts);

function resolveFrontendOrigin(value: string | undefined): URL {
 const raw = value?.trim();
 if (!raw) {
  throw new Error('Missing required environment variable NEXT_PUBLIC_BASE_URL.');
 }

 try {
  return new URL(raw);
 } catch {
  throw new Error('NEXT_PUBLIC_BASE_URL is not a valid absolute URL.');
 }
}

function resolveFrontendHosts(primaryHost: string): string[] {
 const hosts = new Set<string>([primaryHost, 'localhost', '127.0.0.1']);
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


const nextConfig = {
  allowedDevOrigins: frontendHosts,
 poweredByHeader: false,
 async rewrites() {
  return [
   {
    source: '/api/v1/chat',
    destination: `${apiOrigin}/api/v1/chat`,
   },
   {
    source: '/api/v1/chat/:path*',
    destination: `${apiOrigin}/api/v1/chat/:path*`,
   },
   {
    source: '/api/v1/presence',
    destination: `${apiOrigin}/api/v1/presence`,
   },
   {
    source: '/api/v1/presence/:path*',
    destination: `${apiOrigin}/api/v1/presence/:path*`,
   },
   {
    source: '/uploads/:path*',
    destination: `${apiOrigin}/uploads/:path*`,
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
  },
};


export default withNextIntl(nextConfig);
