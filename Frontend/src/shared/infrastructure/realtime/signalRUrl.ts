import { getPublicApiOrigin } from '@/shared/infrastructure/http/apiUrl';

export function getSignalRHubUrl(hubPath: string): string {
  const origin = getPublicApiOrigin().replace(/\/+$/, '');
  const path = hubPath.startsWith('/') ? hubPath : `/${hubPath}`;
  return `${origin}${path}`;
}
